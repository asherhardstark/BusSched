namespace BusSched.Services.Services
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Models;

    public interface IBusSchedService
    {
        Stop GetNextStop(DateTime startingTime, int stopId, int numberOfArrivalTimes);
    }

    public class BusSchedService : IBusSchedService
    {
        private readonly ICollection<int> routeIds;
        private readonly int interval;
        private readonly int stopTimeBetweenRouteStops;
        private readonly int routeTravelTime;

        public BusSchedService(ICollection<int> routeIds, int interval, int stopTimeBetweenRouteStops, int routeTravelTime)
        {
            this.routeIds = routeIds;
            this.interval = interval;
            this.stopTimeBetweenRouteStops = stopTimeBetweenRouteStops;
            this.routeTravelTime = routeTravelTime;
        }

        public Stop GetNextStop(DateTime startingTime, int stopId, int numberOfArrivalTimes)
        {
            if (numberOfArrivalTimes < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArrivalTimes));
            }

            DateTime now = startingTime.TruncateToMinute();
            int minute = now.Minute;

            DateTime nextBaseStopTime = now
                .AddMinutes(this.interval - (minute % this.interval));

            var stop = new Stop
            {
                Id = stopId,
                RouteStops = this.routeIds.Select(routeId => new RouteStop
                {
                    RouteId = routeId,
                    NextStops = Enumerable.Range(1, numberOfArrivalTimes)
                        .Select(x => nextBaseStopTime.AddMinutes(((stopId - 1) * this.stopTimeBetweenRouteStops + (routeId - 1) * this.routeTravelTime) + this.interval * (x - 1)))
                        .ToList()
                }).ToList()
            };

            // Fix the times that are more than one interval from now
            foreach (var routeStop in stop.RouteStops)
            {
                DateTime firstStop = routeStop.NextStops.First();
                if (firstStop.AddMinutes(-1 * this.interval) > now)
                {
                    routeStop.NextStops = routeStop.NextStops.Select(r => r.AddMinutes(-1 * this.interval)).ToList();
                }
            }

            return stop;
        }
    }
}