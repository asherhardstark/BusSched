using System;
using System.Web.Http;
using BusSched.Services.Services;
using BusSched.Services.Models;

namespace BusSched.Controllers
{
    public class BusSchedController : ApiController
    {
        private readonly IBusSchedService busSchedService;
        private readonly IAppConfigService appConfigService;

        public BusSchedController(IBusSchedService busSchedService, IAppConfigService appConfigService)
        {
            this.busSchedService = busSchedService;
            this.appConfigService = appConfigService;
        }

        public Stop Get(int id)
        {
            return this.busSchedService.GetNextStop(DateTime.UtcNow, id, this.appConfigService.NumberOfArrivalTimesToReturn);
        }
    }
}
