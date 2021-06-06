using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentRouteChecker
{
    /// <summary>
    /// Holds info about a point along the route.
    /// Helps avoid having to refactor code when adding info to nodes.
    /// </summary>
    public class RouteNode
    {
        public string IATACode { get; }

        public RouteNode(string IATACode)
        {
            this.IATACode = IATACode;
        }

        public override string ToString()
        {
            return IATACode;
        }
    }
}
