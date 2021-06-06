using System;
using System.Collections.Generic;

using System.IO;

namespace ShipmentRouteChecker
{
    /// <summary>
    /// Class for a single complete route.
    /// </summary>
    public class ShipmentRoute
    {
        public RouteNode Source { get; private set; }
        public RouteNode Destination { get; private set; }
        // No limit or units given for weight, assume very large
        public long TotalWeight { get; private set; }
        public List<RouteLeg> RouteLegs { get; }

        public ShipmentRoute(string inputFile)
        {
            RouteLegs = new();
            ParseInputFile(inputFile);
        }

        private void ParseInputFile(string inputFile)
        {
            using var input = File.OpenText(inputFile);

            var routeParameters = input.ReadLine().Split(' ');

            //Initially source holds all the cargo
            Destination = new RouteNode(routeParameters[1]);
            TotalWeight = Convert.ToInt64(routeParameters[2]);
            Source = new RouteNode(routeParameters[0]);

            string line = string.Empty;
            while (!input.EndOfStream)
            {
                line = input.ReadLine();
                var leg = new RouteLeg(line);
                RouteLegs.Add(leg);
            }
        }
    }
}