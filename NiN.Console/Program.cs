namespace NiN.Console
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using NiN.Database;
    using NiN.Export;

    public class Program
    {
        private static readonly Stopwatch Stopwatch = new();

        public static void Main(string[] args)
        {
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
            }
        }

        private static void Export(IEnumerable<string> arguments)
        {
            var enumerable = arguments as string[] ?? arguments.ToArray();
            if (!enumerable.Any())
            {
                Console.WriteLine("Missing version.. (1, 2, 2.1, 2.1b, 2.2 or 2.3)");
                return;
            }

            using (var context = new NiNContext())
            {
                foreach (var argument in enumerable)
                {
                    var ninCodeExport = new NinCodeExport(context, argument);

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
        }

        private static void Import()
        {
            Stopwatch.Start();

            Console.WriteLine("Building database...");

            NinLoader.CreateCodeDatabase("1");
            NinLoader.CreateCodeDatabase("2");
            NinLoader.CreateCodeDatabase("2.1");
            NinLoader.CreateCodeDatabase("2.1b");
            NinLoader.CreateCodeDatabase("2.2");
            NinLoader.CreateCodeDatabase("2.3");

            NinVarietyLoader.CreateVarietyDatabase("2.1");
            NinVarietyLoader.CreateVarietyDatabase("2.1b");
            NinVarietyLoader.CreateVarietyDatabase("2.2");
            NinVarietyLoader.CreateVarietyDatabase("2.3");

            Console.WriteLine("Finished building database");

            Stopwatch.Stop();

            Console.WriteLine($"Total time: {Stopwatch.ElapsedMilliseconds / 1000.0:N} seconds");
        }
    }
}
