using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBS.IT.Utilities.Shared.APIClient;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SBS.IT.Utilities.Shared.APIClient.Core;
using System.ComponentModel;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Filters;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Extension;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models.Common;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class ProjectViewController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;

        public ProjectViewController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration, ISessionCacheManager sessionCacheManager)
        {
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
            this.sessionCacheManager = sessionCacheManager;
        }

        public ActionResult Index()
        {
            List<Projects> lstTeam = apiExtension.InvokeGet<List<Projects>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProject));
            ViewBag.Projects = lstTeam.Select(x => new SelectListItem { Text = x.ProjectName, Value = x.ProjectId.ToString() }).ToList();
            ViewBag.Months = Services.GetMonthsForDropDown().Select(x => new SelectListItem { Text = x.MonthName.ToString(), Value = x.MonthId.ToString() }).ToList();
            ViewBag.Years = Services.GetYearsForDropDown().Select(x => new SelectListItem { Text = x.YearName.ToString(), Value = x.YearId.ToString() }).ToList();
            return View();
        }

        public ActionResult GetProjectDetail(int projectId, int? month, int? year)
        {
            string startDate = string.Empty;
            if (month == null || year == null)
            {
                startDate = null;
            }
            else
            {
                startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).ToShortDateString(); //Convert.ToDateTime(year + '-' + month + '-' + "01");
            }
            Nullable<int> monthBack = 1;
            List<ProjectViewResponse> lstTeam = apiExtension.InvokeGet<List<ProjectViewResponse>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAdminProjectView + "?projectId=" + projectId + "&monthsBack=" + monthBack + " &searchDate=" + startDate));
            List<ProjectViewModel> result = new List<ProjectViewModel>();
            if (lstTeam != null && lstTeam.Count > 0)
            {
                result = lstTeam.GroupBy(x => x.LocationCode)
                    .Select(x => new ProjectViewModel
                    {
                        EmployeeTypeCode = x.Key,
                        ProjectViews = x.OrderBy(c => c.WorkTypeName).ToList(),
                        EmployeeTypeTotalHour = x.Sum(c => c.WorkHour),
                        ProjectName = x.Select(c => c.ProjectName).FirstOrDefault(),
                        EmployeeType = x.Select(c => c.LocationName).FirstOrDefault(),
                        TotalHour = lstTeam.GroupBy(c => c.ProjectName).Select(c => c.Sum(y => y.WorkHour)).FirstOrDefault()
                    }).ToList();
            }
            return PartialView(result);
        }


        public ActionResult ExportProjectDetail(int projectId, int? month, int? year)
        {
            string startDate = string.Empty;
            if (month == null || year == null)
            {
                startDate = null;
            }
            else
            {
                startDate = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).ToShortDateString(); //Convert.ToDateTime(year + '-' + month + '-' + "01");
            }
            Nullable<int> monthBack = 1;
            List<ProjectViewExportResponse> lstTeam = apiExtension.InvokeGet<List<ProjectViewExportResponse>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAdminProjectViewExport + "?projectId=" + projectId + "&monthsBack=" + monthBack + " &searchDate=" + startDate));

            ExportProjectViewModel exportProjectViewModel = new ExportProjectViewModel();

            if (lstTeam != null && lstTeam.Count > 0)
            {
                exportProjectViewModel.EmployeeList = lstTeam.GroupBy(x => x.EmployeeId).Select(x => new SelectListItem { Value = x.Key.ToString(), Text = x.FirstOrDefault().EmployeeName }).OrderBy(x => x.Text).ToList();
                exportProjectViewModel.ExportProjectViews = lstTeam.GroupBy(x => x.LocationCode)
                    .Select(x => new ExportProjectView
                    {
                        LocationCode = x.Key,
                        LocationName = x.FirstOrDefault().LocationName,
                        TotalHour = x.Sum(c => c.WorkHour),
                        workTypes = x.GroupBy(c => c.WorkTypeId).Select(c => new WorkType
                        {
                            WorkTypeId = c.Key,
                            WorkTypeName = c.FirstOrDefault().WorkTypeName,
                            TotalHour = c.Sum(a => a.WorkHour),
                            ProjectViewExportResponses = c.OrderBy(v => v.EmployeeName).ToList()
                        }).ToList()
                    }).ToList();
            }

            return PartialView(exportProjectViewModel);
        }

    }

}
