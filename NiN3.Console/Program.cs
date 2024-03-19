﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NiN.Infrastructure.Services;
using NiN3.Infrastructure.DbContexts;
using Sharprompt;
// See https://aka.ms/new-console-template for more information

//IConfiguration config;
NiN3DbContext db = null;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

void LoadDB()
{
    var optionsBuilder = new DbContextOptionsBuilder<NiN3DbContext>();
    optionsBuilder.UseSqlite(config.GetConnectionString("Extract"));
    db = new NiN3DbContext(optionsBuilder.Options);
    db.Database.EnsureCreated();
}
var buildtDbFileName = config.GetValue<string>("bildtDbFileName");
var buildtDbFileFullPath = config.GetValue<string>("buildtDBFilePath");
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("NonHostConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
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
            LoadDB();
            //db.Database.EnsureCreated();
            // run loader
            var loader = new LoaderService(config, db, _logger);
            loader.load_all_data();
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
            break;
        case "wipe":
            if (db == null)
            {
                Console.WriteLine($"Running choice 'wipe'");
                var filename = config.GetValue<string>("buildtDBFilePath");
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
            var arr = config.GetConnectionString("Extract").Split("=");
            var sourcepath = buildtDbFileFullPath;
            var path = config.GetValue<string>("webapiDBpath");
            try
            {
                using (var sourceStream = new FileStream(sourcepath, FileMode.Open))
                {
                    using (var destinationStream = new FileStream(path, FileMode.Create))
                    {
                        sourceStream.CopyTo(destinationStream);
                    }
                }
                Console.WriteLine($"Copied new db file to webproject ({path})");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            };
            break;
        case "info":
            var sourcedbpath = buildtDbFileFullPath;
            var webdbpath = config.GetValue<string>("webapiDBpath");

            var sourceInfo = new FileInfo(sourcedbpath);
            sourceInfo.Refresh();
            Console.WriteLine($"Changed time of source db file: {sourceInfo.LastWriteTime}");

            var webInfo = new FileInfo(webdbpath);
            webInfo.Refresh();
            Console.WriteLine($"Changed time of web db file: {webInfo.LastWriteTime}");
            break;
        default:
            Console.WriteLine("No knows command, options:");
            Console.WriteLine(meny);
            break;
    }
}