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
    using NiN.Export;

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
                if (string.IsNullOrEmpty(connectionString)) throw new Exception("Could not find 'NinApiConnectionString'");
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
            }
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
            NinLoader.CreateCodeDatabase(_serviceProvider, "2.3");

            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1");
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.1b");
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.2");
            NinVarietyLoader.CreateVarietyDatabase(_serviceProvider, "2.3");

            Console.WriteLine("Finished building database");

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }
    }
}
