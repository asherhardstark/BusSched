using System;
using System.Collections.Generic;

namespace BusSched.Services.Models
{
    public class RouteStop
    {
        public int RouteId { get; set; }
        public ICollection<DateTime> NextStops { get; set; }
    }
}
