using SBS.IT.Utilities.Web.TimeTrackerWeb.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILogger _Logger;
        public MvcApplication()
        {
            _Logger = new Log4NetLogger();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FluentValidationConfig.RegisterFluentValidation();
        }
        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            // Handle HTTP errors
            if (exc.GetType() == typeof(HttpException))
            {
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    _Logger.WriteMessage(this.GetType(), LogLevel.FATAL, exc.Message, exc);
                return;
            }
            _Logger.WriteMessage(this.GetType(), LogLevel.FATAL, exc.Message, exc);
        }
    }
}
