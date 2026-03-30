using System.Web;
using System.Web.Mvc;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
