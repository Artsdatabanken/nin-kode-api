namespace NiN.Console
{
    using System;
    using System.Diagnostics;

    public class Program
    {
        private static readonly Stopwatch Stopwatch = new();

        public static void Main(string[] args)
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
