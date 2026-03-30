using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.IT.Utilities.Shared.APIClient.Implementation
{
    public class APIConfiguration : IAPIConfiguration
    {
        public string ServiceBaseAddress { get; set; }
        public string APIUsername { get; set; }
        public string APIPassword { get; set; }
        public string ConsumerCode { get; set; }


        private readonly ILogger logger;
        public APIConfiguration()
        {
            logger = new Log4NetLogger();
            LoadConfigurationFromAppSettings();
        }
        private void LoadConfigurationFromAppSettings()
        {
            try
            {
                this.APIUsername = ConfigurationManager.AppSettings["APIUserName"];
                this.APIPassword = ConfigurationManager.AppSettings["APIPassword"];
                this.ConsumerCode = ConfigurationManager.AppSettings["ConsumerCode"];

                this.ServiceBaseAddress = ConfigurationManager.AppSettings["TimeTrackerAPIUrl"];
                if (string.IsNullOrEmpty(ServiceBaseAddress))
                {
                    throw new Exception("TimeTrackerAPIUrl not defined in web.config settings");
                }
            }
            catch (Exception e)
            {
                logger.WriteMessage(this.GetType(), LogLevel.FATAL, string.Empty, e);
            }
        }


    }

}
