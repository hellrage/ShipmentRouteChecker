using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentRouteChecker
{
    /// <summary>
    /// Holds info about a point along the route and its connections to other points.
    /// Helps avoid having to refactor code when adding info to nodes.
    /// </summary>
    public class RouteNode
    {
        public string IATACode { get; }
        public List<RouteLeg> InboundLegs { get; private set; }

        public List<RouteLeg> OutboundLegs { get; private set; }


        public RouteNode(string IATACode)
        {
            this.IATACode = IATACode;
            InboundLegs = new();
            OutboundLegs = new();
        }

        public void AddLeg(RouteLeg leg, bool inbound)
        {
            if (inbound)
                InboundLegs.Add(leg);
            else
                OutboundLegs.Add(leg);
        }

        public override string ToString()
        {
            return IATACode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is RouteNode routeNode)
                return this.IATACode == routeNode.IATACode;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
