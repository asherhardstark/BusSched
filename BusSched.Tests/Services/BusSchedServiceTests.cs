using BusSched.Services.Models;
using BusSched.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusSched.Tests.Services
{
    [TestClass]
    public class BusSchedServiceTests
    {
        [TestMethod]
        public void GetNextStop_Stop1Midnight_ReturnsCorrectTimes()
        {
            var expectedStopTimes = new[]
            {
                DateTime.Parse("2018-01-01T12:15:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:30:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:02:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:17:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:04:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:19:00.0000000Z")
            };

            this.RunTest(DateTime.Parse("2018-01-01T12:00:00.0000000Z"), 1, 2, expectedStopTimes);
        }

        [TestMethod]
        public void GetNextStop_Stop10007_ReturnsCorrectTimes()
        {
            var expectedStopTimes = new[]
            {
                DateTime.Parse("2018-01-01T12:15:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:30:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:17:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:32:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:19:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:34:00.0000000Z")
            };

            this.RunTest(DateTime.Parse("2018-01-01T12:07:00.0000000Z"), 1, 2, expectedStopTimes);
        }

        [TestMethod]
        public void GetNextStop_Stop2Midnight_ReturnsCorrectTimes()
        {
            var expectedStopTimes = new[]
            {
                DateTime.Parse("2018-01-01T12:02:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:17:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:04:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:19:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:06:00.0000000Z"),
                DateTime.Parse("2018-01-01T12:21:00.0000000Z")
            };

            this.RunTest(DateTime.Parse("2018-01-01T12:00:00.0000000Z"), 2, 2, expectedStopTimes);
        }

        private void RunTest(DateTime currentTime, int stopId, int numberOfArrivalTimes, DateTime[] expectedStopTimes)
        {
            Stop nextStop = this.GetService().GetNextStop(currentTime, stopId, numberOfArrivalTimes);

            Assert.AreEqual(stopId, nextStop.Id);

            List<DateTime> stopTimes = nextStop.RouteStops.SelectMany(s => s.NextStops).ToList();
            Assert.AreEqual(expectedStopTimes.Length, stopTimes.Count);

            for (int i = 0; i < stopTimes.Count; i++)
            {
                Assert.AreEqual(expectedStopTimes[i], stopTimes[i]);
            }
        }

        private BusSchedService GetService() => new BusSchedService(new[] { 1, 2, 3 }, 15, 2, 2);
    }
}
