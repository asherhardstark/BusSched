using System.Configuration;
using BusSched.Services.Services;

namespace BusSched.Services
{
    public class AppConfigService : IAppConfigService
    {
        public int NumberOfArrivalTimesToReturn
        {
            get
            {
                int numberOfArrivalTimesToReturn;
                return int.TryParse(ConfigurationManager.AppSettings["NumberOfArrivalTimesToReturn"], out numberOfArrivalTimesToReturn)
                    ? numberOfArrivalTimesToReturn
                    : 1;
            }
        }
    }
}