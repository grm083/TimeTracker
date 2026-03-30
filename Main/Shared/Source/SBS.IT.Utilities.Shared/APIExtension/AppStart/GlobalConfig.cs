using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;


namespace SBS.IT.Utilities.Shared.APIExtension.AppStart
{
    public class GlobalConfig
    {
        public static void RegisterConfig()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
