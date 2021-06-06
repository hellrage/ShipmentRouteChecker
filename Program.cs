using System;

namespace ShipmentRouteChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputPath;
            if (args.Length == 0)
                inputPath = "TestInput.txt";
            else
                inputPath = args[0];

            // Assume correctness of input, immediately start parsing
            var route = new ShipmentRoute(inputPath);
            var checker = new RouteChecker();
            Console.WriteLine(checker.VerifyRoute(route));
            Console.ReadLine();
        }
    }
}