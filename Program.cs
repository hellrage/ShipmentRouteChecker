using System;
using System.Diagnostics;
using System.Text;

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
            var route = ShipmentRoute.FromFile(inputPath);
            var checker = new RouteChecker();
            Console.WriteLine(checker.VerifyRoute(route));
            Console.ReadLine();
        }

        /// <summary>
        /// Helper to see how long it takes\ how much memory it takes to verify a route
        /// </summary>
        private static void Profile()
        {
            var checker = new RouteChecker();
            Stopwatch sw = new Stopwatch();

            var test = TestRoute(1000000);

            // Warmup, JIT optimization
            checker.VerifyRoute(test);
            bool result = false;

            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                result = checker.VerifyRoute(test);
            }
            sw.Stop();

            Console.WriteLine(result);
            Console.WriteLine(sw.ElapsedMilliseconds);
        }

        private static ShipmentRoute TestRoute(long legs)
        {
            StringBuilder sb = new StringBuilder();
            string dt = DateTime.Now.ToShortDateString();
            string dtt = DateTime.Now.AddDays(1).ToShortDateString();

            sb.AppendLine("SVO AMS " + (legs / 2).ToString());

            for (int i = 0; i < legs / 2; i++)
            {
                sb.AppendLine("0 SVO FVA 1 " + dt);
            }
            for (int i = 0; i < legs / 2; i++)
            {
                sb.AppendLine("0 FVA AMS 1 " + dtt);
            }

            return ShipmentRoute.FromString(sb.ToString());
        }
    }
}