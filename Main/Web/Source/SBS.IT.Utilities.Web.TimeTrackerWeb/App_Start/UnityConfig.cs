using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IAPIExtension, APIExtension>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAPIConfiguration, APIConfiguration>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISessionCacheManager, SessionCacheManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILogger, Log4NetLogger>(new ContainerControlledLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
