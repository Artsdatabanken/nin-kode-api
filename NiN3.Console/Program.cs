using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NiN.Infrastructure.Services;
using NiN3.Infrastructure.DbContexts;

using NLog.Config;
using NLog.Extensions.Logging;

using Sharprompt;
// See https://aka.ms/new-console-template for more information

// IConfiguration config;
NiN3DbContext db = null;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();


var builtDbFileName = config.GetValue<string>("builtDbFileName");

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
        .AddConsole()
        .AddNLog();
});
ILogger logger = loggerFactory.CreateLogger<Program>();
ILogger<LoaderService> _logger = loggerFactory.CreateLogger<LoaderService>();

// api klient
var run = true;
var meny = @"Commands:
            wipe    : delete temporary databasefile
            makeDB  : Full import of datasets, use 'wipe' before this
            copy    : Move new db file to webproject
            info    : Show info about db files (last time changed)
            exit    : Close program";
while (run)
{
    var input = Prompt.Input<string>("nin3console ");
    switch (input)
    {
        case "help":
            Console.WriteLine(meny);
            break;
        case "makeDB":
            // ensure db is created
            try
            {
                LoadDB();
                // run loader
                var loader = new LoaderService(config, db, _logger);
                loader.load_all_data();
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.InnerException);
            };
            //..optimize file/indexes and flush pool to disk
            using (Microsoft.Data.Sqlite.SqliteConnection connection = (Microsoft.Data.Sqlite.SqliteConnection)db.Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "VACUUM";
                cmd.ExecuteNonQuery();
                connection.Close();
                Microsoft.Data.Sqlite.SqliteConnection.ClearPool(connection);
            }
            Console.WriteLine($"Created new db file (temporary database based on current model and csv-files)");
            break;
        case "exit":
            return;
        case "wipe":
        Console.WriteLine($"Database object state is null: {db == null}");
            if (db == null)
            {
                Console.WriteLine($"Running choice 'wipe'");
                var filename = builtDbFileFullPath();
                Console.WriteLine($"Database file path: {filename}");

                //var droptablesquery = $"drop table if exists Domene Grunntype";
                FileInfo fi = new FileInfo(filename);
                if (fi.Exists)
                {
                    fi.Delete();
                    Console.WriteLine($"Deleted temporary db file ({filename}), you can now run 'makeDB' to create fresh dbfile");
                }
            }
            else
            {
                Console.WriteLine("Cannot delete db file while in use");
            }
            //File.Delete(filename);
            break;
        case "copy":
            var sourcepath = builtDbFileFullPath();
            var webApiDbPath = buildWebApiDbPath();
            try
            {
                using (var sourceStream = new FileStream(sourcepath, FileMode.Open))
                {
                    using (var destinationStream = new FileStream(webApiDbPath, FileMode.Create))
                    {
                        sourceStream.CopyTo(destinationStream);
                    }
                }
                Console.WriteLine($"Copied new db file to webproject ({webApiDbPath})");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            };
            break;
        case "info":
            var sourcedbpath = builtDbFileFullPath();
            var webApiPath = buildWebApiDbPath();

            var sourceInfo = new FileInfo(sourcedbpath);
            sourceInfo.Refresh();
            Console.WriteLine($"Changed time of source db file: {sourceInfo.LastWriteTime}");

            var webInfo = new FileInfo(webApiPath);
            webInfo.Refresh();
            Console.WriteLine($"Changed time of web db file: {webInfo.LastWriteTime}");
            break;
        default:
            Console.WriteLine("No knows command, options:");
            Console.WriteLine(meny);
            break;
    }
}

return;

string builtDbFileFullPath()
{
    var relativeDbPath = config.GetValue<string>("builtDBFilePath");
    var dbRootPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName);
    var dbFileFullPath = $"{dbRootPath}{relativeDbPath}";
    var dbDirectory = Path.GetDirectoryName(dbFileFullPath);
    Directory.CreateDirectory(dbDirectory);
    return dbFileFullPath;
}

string buildWebApiDbPath()
{
    var relativeWebApiPath = config.GetValue<string>("webapiDBpath");
    var rootFolder = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName);
    return $"{rootFolder}{relativeWebApiPath}";
}

void LoadDB()
{
    var optionsBuilder = new DbContextOptionsBuilder<NiN3DbContext>();
    var connectionString = builtDbFileFullPath();
    optionsBuilder.UseSqlite($"Data Source={connectionString}");
    db = new NiN3DbContext(optionsBuilder.Options);
    var test = db.Database.EnsureCreated();
}