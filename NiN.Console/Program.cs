namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
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

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.local.json", true, true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();

            var connectionString = config.GetConnectionString("Default");

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = config.GetValue("NinApiConnectionString", "");
                if (string.IsNullOrEmpty(connectionString))
                    throw new Exception("Could not find 'NinApiConnectionString'");
            }

            var services = new ServiceCollection();
            services.AddDbContext<NiNDbContext>(options => options.UseSqlServer(connectionString));
            _serviceProvider = services.BuildServiceProvider();

            var arguments = args.Select(x => x.ToLowerInvariant().Trim()).ToList();

            if (arguments.Count == 0) return;

            switch (arguments[0])
            {
                case "import":
                    Import();
                    break;
                case "export":
                    Export(arguments.Skip(1));
                    break;
                case "migrate":
                    Migrate();
                    break;
                case "test":
                    Test("NA T4", "E-1", new[] { 1, 5 }, "2.2");
                    Test("NA T4", "E-2", new[] { 2, 6, 17 }, "2.2");
                    Test("NA T4", "E-3", new[] { 3, 4, 7, 8, 18, 19 }, "2.2");
                    Test("NA T4", "E-4", new[] { 9, 13 }, "2.2");
                    Test("NA T4", "E-5", new[] { 10, 14 }, "2.2");
                    Test("NA T4", "E-6", new[] { 11, 12, 15, 16, 20 }, "2.2");
                    break;
            }
        }

        private static void Test(string ninkodeprefix, string ninkode, int[] grunntypekoder, string version)
        {
            Test($"{ninkodeprefix}-{ninkode}", grunntypekoder.Select(i => $"{ninkodeprefix}-{i}").ToArray(), version);
        }

    private static void Test(string ninkode, string[] grunntypekoder, string version)
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
                    Console.WriteLine($"Added {grunntype.Kode.KodeName} to {kartleggingsenhet.Kode.KodeName}");
                    storeChanges = true;
                }

                if (storeChanges) dbContext.Kartleggingsenhet.Update(kartleggingsenhet);
            }

            if (storeChanges) dbContext.SaveChanges();
        }

        private static void Migrate()
        {
            var dbContext = _serviceProvider.GetService<NiNDbContext>();
            if (dbContext == null)
            {
                throw new Exception("Could not find DbContext");
            }

            Console.WriteLine("Migrating database");

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

        private static void Import()
        {
            Stopwatch.Start();

            Console.WriteLine("Building database...");

            NinLoader.CreateCodeDatabase(_serviceProvider, "1");
            NinLoader.CreateCodeDatabase(_serviceProvider, "2");
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.1");
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.1b");
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.2");
            //NinLoader.CreateCodeDatabase(_serviceProvider, "2.3");

            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1");
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1b");
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.2");
            //NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.3");

            Console.WriteLine("Finished building database");

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }
    }
}
