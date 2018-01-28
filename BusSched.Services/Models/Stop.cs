using System.Collections.Generic;

namespace BusSched.Services.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public ICollection<RouteStop> RouteStops { get; set; }
    }
}