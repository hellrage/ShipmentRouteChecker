using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentRouteChecker
{
    public class RouteChecker
    {
        private Dictionary<string, long> weights;
        private Dictionary<string, bool> dailyDeficit;
        private DateTime schedulePosition;

        public RouteChecker()
        {
            weights = new();
            dailyDeficit = new();
        }

        /// <summary>
        /// Goes through the route day-by-day, calculating resulting weights at each node.
        /// </summary>
        /// <param name="route">Route to verify</param>
        /// <returns></returns>
        public bool VerifyRoute(ShipmentRoute route)
        {
            schedulePosition = route.RouteLegs.First().ShipmentDate;
            weights.Clear();
            weights[route.Source.IATACode] = route.TotalWeight;
            dailyDeficit.Clear();

            // At the end of each day we check if everything's balanced (no cargo shipped before it arrived).
            bool DeficitResolved(DateTime newDate)
            {
                foreach (var node in dailyDeficit)
                {
                    if (weights[node.Key] < 0)
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
                    dailyDeficit[leg.Source.IATACode] = true;
            }

            // If everything's balanced and cargo arrived at the destination, check the total weight.
            if (DeficitResolved(schedulePosition) && weights.ContainsKey(route.Destination.IATACode))
                return weights[route.Destination.IATACode] == route.TotalWeight;
            else return false;
        }
    }
}
