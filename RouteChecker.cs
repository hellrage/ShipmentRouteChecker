using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentRouteChecker
{
    public class RouteChecker
    {
        /// <summary>
        /// Goes through the route day-by-day, calculating resulting weights at each node.
        /// </summary>
        /// <param name="route">Route to verify</param>
        /// <returns></returns>
        public bool VerifyRoute(ShipmentRoute route)
        {
            Dictionary<string, long> weights = new Dictionary<string, long> { { route.Source.IATACode, route.TotalWeight } };
            List<string> dailyDeficit = new();
            DateTime schedulePosition = route.RouteLegs.First().ShipmentDate;

            // At the end of each day we check if everything's balanced (no cargo shipped before it arrived).
            bool DeficitResolved(DateTime newDate)
            {
                foreach (var node in dailyDeficit)
                {
                    if (weights[node] < 0)
                        return false;
                }
                dailyDeficit.Clear();
                schedulePosition = newDate;
                return true;
            }

            route.RouteLegs.Sort(new Comparison<RouteLeg>((leg1, leg2) => leg1.ShipmentDate.CompareTo(leg2.ShipmentDate)));

            //  Go through the route that's sorted by date. Move cargo, remember deficit.
            foreach (var leg in route.RouteLegs)
            {
                if (leg.ShipmentDate != schedulePosition)
                {
                    if (!DeficitResolved(leg.ShipmentDate)) return false;
                }

                if (weights.ContainsKey(leg.Destination.IATACode))
                    weights[leg.Destination.IATACode] += leg.Weight;
                else
                    weights[leg.Destination.IATACode] = leg.Weight;

                if (weights.ContainsKey(leg.Source.IATACode))
                    weights[leg.Source.IATACode] -= leg.Weight;
                else
                    weights[leg.Source.IATACode] = -leg.Weight;

                if (weights[leg.Source.IATACode] < 0)
                    dailyDeficit.Add(leg.Source.IATACode);
            }

            // If everything's balanced and cargo arrived at the destination, check the total weight.
            if(DeficitResolved(schedulePosition) && weights.ContainsKey(route.Destination.IATACode))
                return weights[route.Destination.IATACode] == route.TotalWeight;
            else return false;
        }
    }
}
