using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Filters;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Extension;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class HomeController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        public HomeController()
        {
            apiExtension = new APIExtension();
            apiConfiguration = new APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
        }
        public ActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            EmployeeAuthenticationModel employeeAuthenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (employeeAuthenticationModel != null && !string.IsNullOrEmpty(employeeAuthenticationModel.UserType) && (employeeAuthenticationModel.UserType == "ADN" || employeeAuthenticationModel.UserType == "SAN"))
            {
                model.UserCount = getUserCount();
                model.ProjectItemCount = getProjectItemCount();
                model.ProjectCount = getProjectCount();
            }
            else
            {
                ViewBag.LastTimeEntry = getLastTimeEntry();
            }
            if (employeeAuthenticationModel != null && employeeAuthenticationModel.IsEmployeeBirthDay == 1)
            {
                model.IsLoggedInUserBirthday = Convert.ToBoolean(employeeAuthenticationModel.IsEmployeeBirthDay);
            }
            model.ReportCount = getReportCount();

            return View(model);
        }

        /// <summary>
        /// get last time entry
        /// </summary>
        /// <returns></returns>
        private string getLastTimeEntry()
        {
            string lastTimeEntry;
            Nullable<DateTime> productionDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ProductionDate"]);
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            lastTimeEntry = apiExtension.InvokeGet<string>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntryLastEntry + "?employeeId=" + authenticationModel.EmployeeId + "&productionDate=" + productionDate));

            if (!string.IsNullOrEmpty(lastTimeEntry))
            {
                //lastTimeEntry = lastTimeEntry.Replace("1", "1st").Replace("2", "2nd").Replace("3", "3rd").Replace("4", "4th").Replace("5", "5th");
                return lastTimeEntry;
            }
            else
                return string.Empty;
        }

        private int getUserCount()
        {
            int rowCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int? managerId = authenticationModel.EmployeeId;
            List<EmployeeModel> EmployeeModelLst = apiExtension.InvokeGet<List<EmployeeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.EmployeeSearch + "?searchBy=&managerId=" + managerId + "&pageSize=&pageNumber=&sortOrder=1&sortColumn=EmployeeId"));
            rowCount = EmployeeModelLst != null && EmployeeModelLst.Count > 0 ? EmployeeModelLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return rowCount;
        }

        private int getProjectItemCount()
        {
            int rowCount = 0;
            List<ProjectItemModel> projectItemModelLst = apiExtension.InvokeGet<List<ProjectItemModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectItemSearch + "?searchBy=&pageSize=&pageNumber=&sortOrder=1&sortColumn="));
            rowCount = projectItemModelLst != null && projectItemModelLst.Count > 0 ? projectItemModelLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return rowCount;
        }
        private int getProjectCount()
        {
            int rowCount = 0;
            List<ProjectModel> ProjectModelLst = apiExtension.InvokeGet<List<ProjectModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectSearch + "?searchBy=&pageSize=&pageNumber=&sortOrder=1&sortColumn="));
            rowCount = ProjectModelLst != null && ProjectModelLst.Count > 0 ? ProjectModelLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return rowCount;
        }
        private int getReportCount()
        {
            int rowCount = 0;
            List<ReportRegistryModel> reportList = apiExtension.InvokeGet<List<ReportRegistryModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetReportDetail + "?userTypeId=" + AuthenticateExtension.UserTypeId));
            rowCount = reportList != null && reportList.Count > 0 ? reportList.Count : 0;
            return rowCount;
        }

        private List<EmployeeModel> GetBirthdays()
        {
            List<EmployeeModel> EmployeeModelLst = apiExtension.InvokeGet<List<EmployeeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.EmployeeBirthday + "?date="));
            return EmployeeModelLst != null && EmployeeModelLst.Count > 0 ? EmployeeModelLst : null;
        }

        private bool IsLoggedInUserBirthday(ref List<EmployeeModel> birthdayEmployees, ref DashboardViewModel model)
        {
            bool bResult = false;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (birthdayEmployees != null)
            {
                var loggedInUserBirthday = birthdayEmployees.Where(x => x.LogonName.Equals(authenticationModel.UserName));
                if (loggedInUserBirthday.Count() > 0)
                {
                    bResult = true;
                    model.LoggedUserFirstName = loggedInUserBirthday.FirstOrDefault().FirstName;
                    birthdayEmployees.Remove(loggedInUserBirthday.FirstOrDefault());
                }
            }
            return bResult;
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}