namespace NiN.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            //NinLoader.RemoveAll("2.1b");

            NinLoader.CreateCodeDatabase("1");
            NinLoader.CreateCodeDatabase("2");
            NinLoader.CreateCodeDatabase("2.1");
            NinLoader.CreateCodeDatabase("2.1b");
            NinLoader.CreateCodeDatabase("2.2");
            NinLoader.CreateCodeDatabase("2.3");

            NinLoader.CreateVarietyDatabase("2.1");
            NinLoader.CreateVarietyDatabase("2.1b");
            NinLoader.CreateVarietyDatabase("2.2");
            NinLoader.CreateVarietyDatabase("2.3");
        }
    }
}
