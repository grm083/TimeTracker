using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SBS.IT.Utilities.API.TimeTrackerWebAPI.Filters;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new ValidateModelAttribute());
            config.Filters.Add(new GlobalExceptionFilterAttribute());

            config.MessageHandlers.Add(new ApiKeyAuthHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
