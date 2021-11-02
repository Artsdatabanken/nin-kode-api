namespace NiN.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            NinLoader.CreateDatabase();
        }
    }
}
