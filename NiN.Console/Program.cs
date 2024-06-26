﻿namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using NiN.Database;
    using NiN.Database.Models.Code.Enums;
    using NiN.ExportImport;

    public class Program
    {
        private static readonly Stopwatch Stopwatch = new();

        private static ServiceProvider _serviceProvider;
        private static IConfiguration config;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);

            config = builder.Build();

            var connectionString = config.GetConnectionString("Default");

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = config.GetValue("NinApiConnectionString", "");
                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Could not find 'NinApiConnectionString'");
            }

            var services = new ServiceCollection();
            services.AddDbContext<NiNDbContext>(options => options.UseSqlite(connectionString));
            _serviceProvider = services.BuildServiceProvider();

            var arguments = args.Select(x => x.ToLowerInvariant().Trim()).ToList();

            if (arguments.Count == 0) return;

            switch (arguments[0])
            {
                case "test":
                    FixSingleLkm("2.3", "T4-SS_1", "SS-h", "SS-j");
                    FixSingleLkm("2.3", "T4-SS_2", "SS-i", "SS-k");
                    break;
                case "kartlegging":
                    DeleteKartleggingConnection();
                    CreateKartleggingConnection("2.2");
                    CreateKartleggingConnection("2.3");
                    break;
                case "importnin":
                    Migrate();
                    Import(arguments.Skip(1));
                    break;
                case "complete":
                    Migrate();
                    Import();
                    Import(new[] { "2.3" }, true);
                    ImportVariasjon("2.3");
                    CreateKartleggingConnection("2.2");
                    CreateKartleggingConnection("2.3");
                    FixLkm(new[] { "2.2", "2.3" });
                    CreateLkmConnection("2.2");
                    CreateLkmConnection("2.3");
                    break;
                case "unraven":
                    //todo-sat: receive version param
                    //todo-sat: Move ravendb to json for current version
                    //var p = System.AppContext.BaseDirectory;
                    //var all = config.AsEnumerable();
                    //Testing fetching sosijson filename for user secrets
                    //var sosiv1jsonFileStr = config.GetValue("SOSINiNv1Json", "");
                    //System.IO.File.WriteAllText($"{sosiv1jsonFileStr}", "123123");
                    //Console.WriteLine($"Current path: {p}");
                    //throw new NotImplementedException();
                    //NinVarietyLoader.FixLandform(serviceProvider, Directory.GetCurrentDirectory(),"2.3");
                    ImportVariasjon("2.3");
                    break;
                case "import":
                    Import();
                    break;
                case "export":
                    Export(arguments.Skip(1));
                    break;
                case "migrate":
                    Migrate();
                    break;
                //case "grunntype":
                //    CreateKartleggingConnection("NA T4", "E-1", new[] { 1, 5 }, "2.2");
                //    CreateKartleggingConnection("NA T4", "E-2", new[] { 2, 6, 17 }, "2.2");
                //    CreateKartleggingConnection("NA T4", "E-3", new[] { 3, 4, 7, 8, 18, 19 }, "2.2");
                //    CreateKartleggingConnection("NA T4", "E-4", new[] { 9, 13 }, "2.2");
                //    CreateKartleggingConnection("NA T4", "E-5", new[] { 10, 14 }, "2.2");
                //    CreateKartleggingConnection("NA T4", "E-6", new[] { 11, 12, 15, 16, 20 }, "2.2");
                //    break;
                case "basistrinn":
                    CreateLkmConnection("NA T4", "1", "KA", new[] { "a", "b", "c" }, "2.2");
                    CreateLkmConnection("NA T4", "1", "UF", new[] { "a", "b" }, "2.2");
                    CreateLkmConnection("NA T4", "2", "KA", new[] { "d", "e" }, "2.2");
                    CreateLkmConnection("NA T4", "2", "UF", new[] { "a", "b" }, "2.2");
                    CreateLkmConnection("NA T4", "3", "KA", new[] { "f", "g" }, "2.2");
                    CreateLkmConnection("NA T4", "3", "UF", new[] { "a", "b" }, "2.2");
                    CreateLkmConnection("NA T4", "4", "KA", new[] { "h", "i" }, "2.2");
                    CreateLkmConnection("NA T4", "4", "UF", new[] { "a", "b" }, "2.2");
                    CreateLkmConnection("NA T4", "5", "KA", new[] { "a", "b", "c" }, "2.2");
                    CreateLkmConnection("NA T4", "5", "UF", new[] { "c", "d" }, "2.2");
                    CreateLkmConnection("NA T4", "6", "KA", new[] { "d", "e" }, "2.2");
                    CreateLkmConnection("NA T4", "6", "UF", new[] { "c", "d" }, "2.2");
                    break;
            }
        }

        private static void CreateLkmConnection(string version)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            var importer = new NinCodeImport(_serviceProvider.GetService<NiNDbContext>(), version);
            var records = importer.GetGrunntypeBasistrinnRecords($"CsvFiles\\v{version}\\import_grunntyper_basistrinn_v{version}.csv");

            if (records == null) return;

            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
            if (ninVersion == null) return;

            // reset database if connections exist
            var grunntypes = dbContext.Grunntype
                .Include(x => x.Kode)
                .Include(x => x.Basistrinn)
                .Where(x =>
                    x.Version.Id == ninVersion.Id &&
                    x.Basistrinn.Count > 0);
            if (grunntypes.Any())
            {
                if (!dbContext.Database.ProviderName.Equals("Microsoft.EntityFrameworkCore.SqlServer")) return;

                foreach (var grunntype in grunntypes)
                {
                    dbContext.Database.ExecuteSqlRaw($"DELETE FROM BasistrinnGrunntype WHERE GrunntypeId = '{grunntype.Id}'");
                }
            }

            var i = 0;
            foreach (var @record in records)
            {
                i++;
                var ninkodeprefix = record.GrunntypeKode;
                if (ninkodeprefix.StartsWith("NA-", StringComparison.OrdinalIgnoreCase))
                    ninkodeprefix = ninkodeprefix.Replace("NA-", "NA ");

                var iPos = ninkodeprefix.IndexOf("-", StringComparison.Ordinal);
                if (iPos < 0)
                {
                    Console.WriteLine($"Illegal values in line {i}: {record.GrunntypeKode};{record.Basistrinn}");
                    continue;
                }
                var grunntypekode = ninkodeprefix.Substring(iPos + 1);
                ninkodeprefix = ninkodeprefix.Substring(0, iPos);

                var basistrinns = record.Basistrinn.Replace(" ", "").Split(",", StringSplitOptions.RemoveEmptyEntries);

                foreach (var b in basistrinns)
                {
                    if (string.IsNullOrWhiteSpace(b)) continue;

                    iPos = b.IndexOf("-", StringComparison.Ordinal);
                    if (iPos < 0)
                    {
                        Console.WriteLine($"Illegal values in line {i}: {record.GrunntypeKode};{record.Basistrinn}");
                        continue;
                    }
                    var basistrinnprefix = b.Substring(0, iPos);

                    var basistrinn = b.Substring(iPos + 1);

                    CreateLkmConnection(ninkodeprefix, grunntypekode, basistrinnprefix, basistrinn.ToCharArray(), version);
                }
            }

            Stopwatch.Stop();
            Console.WriteLine($"\nProcessed {i} records in {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }

        private static void CreateLkmConnection(string ninkodeprefix, string grunntypekode, string basistrinnprefix, char[] basistrinn, string version)
        {
            CreateLkmConnection($"{ninkodeprefix}-{grunntypekode}", basistrinn.Select(i => $"{basistrinnprefix}-{i}").ToArray(), version);
        }

        private static void CreateLkmConnection(string ninkodeprefix, string grunntypekode, string basistrinnprefix, string[] basistrinn, string version)
        {
            CreateLkmConnection($"{ninkodeprefix}-{grunntypekode}", basistrinn.Select(i => $"{basistrinnprefix}-{i}").ToArray(), version);
        }

        private static void CreateLkmConnection(string ninkode, string[] basistrinns, string version)
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            var kode = dbContext.Kode
                .Include(x => x.Version)
                .FirstOrDefault(x => x.KodeName.Equals(ninkode) && x.Version.Navn.Equals(version));

            if (kode == null) throw new Exception("Could not find kode");

            var storeChanges = false;

            if (kode.Kategori == KategoriEnum.Grunntype)
            {
                var grunntype = dbContext.Grunntype
                    .Include(x => x.Basistrinn)
                    .FirstOrDefault(x => x.Kode.Id == kode.Id);
                if (grunntype == null) return;

                foreach (var b in basistrinns)
                {
                    var basistrinn = dbContext.Basistrinn
                        .Include(x => x.Grunntype)
                        .FirstOrDefault(x => x.Version.Id == grunntype.Version.Id && x.Navn.Equals(b));
                    if (basistrinn == null) continue;
                    
                    if (basistrinn.Grunntype.Contains(grunntype)) continue;

                    grunntype.Basistrinn.Add(basistrinn);
                    //Console.WriteLine($"Added {grunntype.Kode.KodeName} to {basistrinn.Navn}");
                    storeChanges = true;
                }

                if (storeChanges) dbContext.Grunntype.Update(grunntype);
            }

            if (storeChanges) dbContext.SaveChanges();
        }

        private static void DeleteKartleggingConnection()
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            foreach (var grunntype in dbContext.Grunntype.Include(x => x.Kartleggingsenhet))
            {
                grunntype.Kartleggingsenhet.Clear();
                dbContext.Grunntype.Update(grunntype);
            }

            dbContext.SaveChanges();
        }

        private static void CreateKartleggingConnection(string version)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            var importer = new NinCodeImport(_serviceProvider.GetService<NiNDbContext>(), version);
            var records = importer.FixKartleggingConnections($"CsvFiles\\v{version}\\import_grunntyper_kartleggingsenheter_v{version}.csv");

            if (records == null) return;

            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            var k = 0;
            var i = 0;
            foreach (var @record in records)
            {
                i++;
                string[] grunntyper = null;
                var gtlist = record.Grunntypekoder.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                for (var index = 0; index < gtlist.Count; index++)
                {
                    var iPos = gtlist[index].IndexOf("-", StringComparison.Ordinal);
                    if (iPos < 0) continue;
                    gtlist[index] = gtlist[index].Substring(iPos + 1);
                }

                grunntyper = gtlist.ToArray();
                
                var ninkodeprefix = record.SammensattKode.Substring(0, record.SammensattKode.IndexOf("-", StringComparison.Ordinal));
                CreateKartleggingConnection(record.SammensattKode, grunntyper.Select(x => $"{ninkodeprefix}-{x}").ToArray(), version);
            }

            if (i > 0)
            {
                Console.WriteLine("\nSaving changes");
                dbContext.SaveChanges();
            }

            Stopwatch.Stop();

            Console.WriteLine($"\nProcessed {i} records in {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }

        private static void CreateKartleggingConnection(string ninkodeprefix, string ninkode, int[] grunntypekoder, string version)
        {
            CreateKartleggingConnection($"{ninkodeprefix}-{ninkode}", grunntypekoder.Select(i => $"{ninkodeprefix}-{i}").ToArray(), version);
        }

        private static void CreateKartleggingConnection(string ninkode, string[] grunntypekoder, string version)
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            var kode = dbContext.Kode
                .Include(x => x.Version)
                .FirstOrDefault(x => x.KodeName.Equals(ninkode) && x.Version.Navn.Equals(version));

            var storeChanges = false;

            if (kode == null) throw new Exception("Could not find kode");

            if (kode.Kategori == KategoriEnum.Kartleggingsenhet)
            {
                var kartleggingsenhet = dbContext.Kartleggingsenhet
                    .Include(x => x.Grunntype)
                    .FirstOrDefault(x => x.Kode.Id == kode.Id);
                if (kartleggingsenhet == null) return;

                foreach (var grunntypeKode in grunntypekoder)
                {
                    var gkode = dbContext.Kode
                        .FirstOrDefault(x => x.KodeName.Equals(grunntypeKode) && x.Version.Id == kode.Version.Id);
                    if (gkode == null) continue;

                    if (gkode.Kategori != KategoriEnum.Grunntype) continue;

                    var grunntype = dbContext.Grunntype
                        .FirstOrDefault(x => x.Kode.Id == gkode.Id);

                    if (grunntype == null) continue;

                    if (kartleggingsenhet.Grunntype.Contains(grunntype)) continue;

                    kartleggingsenhet.Grunntype.Add(grunntype);
                    //Console.WriteLine($"Added {grunntype.Kode.KodeName} to {kartleggingsenhet.Kode.KodeName}");
                    storeChanges = true;
                }

                if (storeChanges) dbContext.Kartleggingsenhet.Update(kartleggingsenhet);
            }

            //if (storeChanges) dbContext.SaveChanges();
        }

        private static void Migrate()
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            Console.WriteLine("Migrating database");
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();

            Console.WriteLine("Migrate database - ok");
        }

        private static void Export(IEnumerable<string> arguments)
        {
            var enumerable = arguments as string[] ?? arguments.ToArray();
            if (!enumerable.Any())
            {
                Console.WriteLine("Missing version.. (1, 2, 2.1, 2.1b, 2.2 or 2.3)");
                return;
            }

            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            foreach (var argument in enumerable)
            {
                var ninCodeExport = new NinCodeExport(dbContext, argument);

                var stream = ninCodeExport.GenerateStream();

                if (stream == null)
                {
                    Console.WriteLine($"v{argument} - empty");
                    continue;
                }

                long size;
                stream.Seek(0, SeekOrigin.Begin);
                using (var fileStream = new FileStream($"C:/temp/NiN_v{argument}.zip", FileMode.Create, FileAccess.Write))
                {
                    stream.WriteTo(fileStream);
                    fileStream.Flush();
                    size = fileStream.Length;
                    fileStream.Close();
                }

                Console.WriteLine($"v{argument}\tlength: {size}");
            }
        }

        private static void ImportVariasjon(string version)
        {
            Stopwatch.Reset();
            Stopwatch.Start();

            Console.WriteLine($"Building variasjon v{version} ...");

            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, version, config);

            Console.WriteLine("Finished building variasjon");

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");

            Stopwatch.Reset();
            Stopwatch.Start();

            NinVarietyLoader.FixLandform(_serviceProvider, $"CsvFiles\\v{version}",version);

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }

        private static void Import(bool allowUpdate = false)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            
            Console.WriteLine("Building database...");

            NinLoader.CreateCodeDatabase(_serviceProvider, "1", allowUpdate, config);
            NinLoader.CreateCodeDatabase(_serviceProvider, "2", allowUpdate, config);
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.1", allowUpdate, config);
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.1b", allowUpdate, config);
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.2", allowUpdate, config);

            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1", config);
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1b", config);
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.2", config);
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.3", config);

            Console.WriteLine("Finished building database");

            VacuumSqlLite();

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }

        private static void VacuumSqlLite()
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }
            using (Microsoft.Data.Sqlite.SqliteConnection connection = (Microsoft.Data.Sqlite.SqliteConnection)dbContext.Database.GetDbConnection())
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "VACUUM";
                cmd.ExecuteNonQuery();
                connection.Close();
                Microsoft.Data.Sqlite.SqliteConnection.ClearPool(connection);
            }
        }

        private static void Import(IEnumerable<string> arguments, bool allowUpdate = false)
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            foreach (var argument in arguments)
            {
                Stopwatch.Reset();
                Stopwatch.Start();

                var ninCodeImport = new NinCodeImport(dbContext, argument);
                ninCodeImport.ImportCompleteNin($"CsvFiles\\v{argument}", allowUpdate);

                Stopwatch.Stop();
                Console.WriteLine($"v{argument}: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }

        private static void FixLkm(IEnumerable<string> arguments, bool allowUpdate = false)
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            foreach (var argument in arguments)
            {
                Stopwatch.Reset();
                Stopwatch.Start();

                var ninCodeImport = new NinCodeImport(dbContext, argument);

                ninCodeImport.FixLkm($"CsvFiles\\v{argument}");

                Stopwatch.Stop();
                Console.WriteLine($"v{argument}: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
            }
        }

        private static void FixSingleLkm(string version, string id, string fromBasistrinn, string toBasistrinn)
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            Stopwatch.Reset();
            Stopwatch.Start();

            var ninVersion = dbContext.NinVersion.FirstOrDefault(x => x.Navn.Equals(version));
            if (ninVersion == null)
            {
                Console.WriteLine($"NiN-code version {version} doesn't exist. Skipping...");
                return;
            }

            var kode = id.Substring(0, id.IndexOf("-", StringComparison.Ordinal));

            var nin = dbContext.Kode
                .FirstOrDefault(x => x.Version.Id == ninVersion.Id && x.KodeName.Equals($"NA {kode}"));

            if (nin != null && nin.Kategori == KategoriEnum.Hovedtype)
            {
                var hovedtype = dbContext.Hovedtype
                    .Include(x => x.Miljovariabler)
                    .FirstOrDefault(x => x.Kode.Id == nin.Id);

                if (hovedtype != null && hovedtype.Miljovariabler.Any())
                {
                    foreach (var m in hovedtype.Miljovariabler)
                    {
                        var miljovariabel = dbContext.Miljovariabel
                            .Include(x => x.Trinn)
                            .FirstOrDefault(x => x.Id == m.Id);
                        if (miljovariabel == null) continue;

                        foreach (var t in miljovariabel.Trinn)
                        {
                            var trinn = dbContext.Trinn
                                .Include(x => x.Kode)
                                .Include(x => x.Basistrinn)
                                .FirstOrDefault(x => x.Id == t.Id);

                            if (trinn == null) continue;
                            if (!trinn.Kode.KodeName.Equals(id)) continue;

                            if (!trinn.Basistrinn.Any()) continue;

                            // locate trinn to remove
                            var remove = trinn.Basistrinn.FirstOrDefault(x => x.Navn.Equals(fromBasistrinn));
                            if (remove == null) continue;

                            trinn.Basistrinn.Remove(remove);
                            Console.WriteLine($"{kode}: {trinn.Kode.KodeName} - {trinn.Navn}, removed {remove.Navn}");

                            // find new basistrinn
                            var add = dbContext.Basistrinn
                                .FirstOrDefault(x => x.Version.Id == ninVersion.Id && x.Navn.Equals(toBasistrinn));

                            if (add != null)
                            {
                                trinn.Basistrinn.Add(add);
                                Console.WriteLine($"{kode}: {trinn.Kode.KodeName} - {trinn.Navn}, added {add.Navn}");
                            }

                            dbContext.Trinn.Update(trinn);
                        }
                    }

                    dbContext.SaveChanges();
                }
            }


            Stopwatch.Stop();
            Console.WriteLine($"v{version}, {id}, {toBasistrinn}: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }
    }
}
