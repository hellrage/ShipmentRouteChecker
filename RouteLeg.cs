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

        public RouteLeg(RouteNode source, RouteNode destination, long weight, DateTime date)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
            ShipmentDate = date;
        }

        public override string ToString()
        {
            return string.Format("From: {0}, to: {1}, weights: {2}, on: {3}", Source.IATACode, Destination.IATACode, Weight, ShipmentDate);
        }
    }
}
