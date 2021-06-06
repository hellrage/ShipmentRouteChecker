using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentRouteChecker
{
    public class RouteLeg
    {
        public RouteNode Source { get; }
        public RouteNode Destination { get; }
        public DateTime ShipmentDate { get; }
        public long Weight { get; }

        public RouteLeg(string description)
        {
            var components = description.Split(' ');
            Source = new RouteNode(components[1]);
            Destination = new RouteNode(components[2]);
            Weight = Convert.ToInt64(components[3]);
            ShipmentDate = DateTime.Parse(components[4]);
        }

        public override string ToString()
        {
            return string.Format("From: {0}, to: {1}, weights: {2}, on: {3}", Source.IATACode, Destination.IATACode, Weight, ShipmentDate);
        }
    }
}
