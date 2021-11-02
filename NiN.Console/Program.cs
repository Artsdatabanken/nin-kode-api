namespace NiN.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            NinLoader.CreateDatabase("2.2");
            NinLoader.CreateDatabase("2.3");
        }
    }
}
