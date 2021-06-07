using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentRouteChecker
{
    public class RouteChecker
    {
        public RouteChecker()
        {
        }

        /// <summary>
        /// Goes through the nodes, totalling the in- and outbound weights, respecting the date.
        /// </summary>
        /// <param name="route">Route to verify</param>
        /// <returns></returns>
        public bool VerifyRoute(ShipmentRoute route)
        {
            long currentWeight = 0;
            bool result = false;
            foreach (var node in route.RouteNodes)
            {
                // Index of last arrived shipment
                var inIndex = 0;
                if (node.IATACode == route.Source.IATACode)
                    currentWeight = route.TotalWeight;
                else
                    currentWeight = 0;

                node.InboundLegs.Sort((leg1, leg2) => leg1.ShipmentDate.CompareTo(leg2.ShipmentDate));
                node.OutboundLegs.Sort((leg1, leg2) => leg1.ShipmentDate.CompareTo(leg2.ShipmentDate));

                // Try to resolve all outbound shipments with available cargo
                foreach (var leg in node.OutboundLegs)
                {
                    // Calculate how much cargo has arrived at this point
                    while (inIndex<node.InboundLegs.Count && node.InboundLegs[inIndex].ShipmentDate <= leg.ShipmentDate)
                    {
                        currentWeight += node.InboundLegs[inIndex].Weight;
                        inIndex++;
                    }
                    currentWeight -= leg.Weight;
                    // If we have a deficit, route is invalid
                    if (currentWeight < 0) return false;
                }

                // If we're procesing the destination, it might not be the last in node list, save the result in a variable.
                if (node.IATACode == route.Destination.IATACode)
                {
                    currentWeight = node.InboundLegs.Sum((leg) => leg.Weight);
                    result = currentWeight == route.TotalWeight;
                }
                else if (currentWeight != 0)
                    return false;
            }
            return result;
        }
    }
}
