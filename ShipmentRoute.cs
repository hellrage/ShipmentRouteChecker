using System;
using System.Collections.Generic;

using System.IO;
using System.Text;

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
        public List<RouteNode> RouteNodes { get; }

        private ShipmentRoute()
        {
            RouteNodes = new();
        }

        public static ShipmentRoute FromFile(string filePath)
        {
            using var fr = File.OpenText(filePath);
            var result = new ShipmentRoute();
            result.ParseInput(fr);
            return result;
        }

        public static ShipmentRoute FromString(string description)
        {
            using var ms = new MemoryStream();
            ms.Write(Encoding.UTF8.GetBytes(description));
            ms.Seek(0, SeekOrigin.Begin);

            var result = new ShipmentRoute();
            using var sr = new StreamReader(ms);
            result.ParseInput(sr);

            return result;
        }

        private void ParseInput(StreamReader input)
        {
            var routeParameters = input.ReadLine().Split(' ');

            //Initially source holds all the cargo
            Destination = new RouteNode(routeParameters[1]);
            TotalWeight = Convert.ToInt64(routeParameters[2]);
            Source = new RouteNode(routeParameters[0]);

            string line = string.Empty;
            while (!input.EndOfStream)
            {
                line = input.ReadLine();
                var components = line.Split(' ');
                RouteNode source;
                RouteNode destination;
                var sourceID = RouteNodes.FindIndex(0, (node) => node.IATACode == components[1]);
                var destinationID = RouteNodes.FindIndex(0, (node) => node.IATACode == components[2]);

                if (sourceID >= 0)
                    source = RouteNodes[sourceID];
                else
                {
                    source = new RouteNode(components[1]);
                    RouteNodes.Add(source);
                }

                if (destinationID >= 0)
                    destination = RouteNodes[destinationID];
                else
                {
                    destination = new RouteNode(components[2]);
                    RouteNodes.Add(destination);
                }

                var leg = new RouteLeg(source, destination, Convert.ToInt64(components[3]), DateTime.Parse(components[4]));

                leg.Source.AddLeg(leg, false);
                leg.Destination.AddLeg(leg, true);
            }
        }
    }
}