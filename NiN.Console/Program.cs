namespace NiN.Console
{
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");

            //NinLoader.RemoveAll("2.1b");

            NinLoader.CreateDatabase("1");
            NinLoader.CreateDatabase("2");
            NinLoader.CreateDatabase("2.1");
            NinLoader.CreateDatabase("2.1b");
            NinLoader.CreateDatabase("2.2");
            NinLoader.CreateDatabase("2.3");
        }
    }
}
