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
using System.Globalization;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models.Common;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class MonthlyViewController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        private readonly ILogger logger;
        public MonthlyViewController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration, ISessionCacheManager sessionCacheManager, ILogger logger)
        {
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
            this.sessionCacheManager = sessionCacheManager;
            this.logger = logger;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<Manager> lstTeam = ReferenceDataCache.GetManagers(apiExtension, apiConfiguration);
            lstTeam.Insert(0, new Manager { ManagerId = -1, ManagerName = "All" });
            MonthlyViewModel model = new MonthlyViewModel();
            //Add Months
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            List<Month> lstMonths = new List<Month>();
            List<Year> lstYears = new List<Year>();

            lstMonths = Services.GetMonthsForDropDown();
            //Add Years
            lstYears = Services.GetYearsForDropDown();
            model.ManagersList = lstTeam;
            model.MonthList = lstMonths;
            model.YearList = lstYears;
            return View(model);
        }

        [HttpPost]
        public JsonResult GetEmployeeDetails(string managerNameId, string selectedMonth, string selectedYear)
        {
            DataSourceRequest request = new DataSourceRequest();
            request.PageSize = 25;
            request.Page = 1;

            var startDate = selectedYear + '-' + selectedMonth + '-' + "01";
            var lastDate = DateTime.DaysInMonth(Convert.ToInt32(selectedYear), Convert.ToInt32(selectedMonth));
            var endDate = selectedYear + '-' + selectedMonth + '-' + lastDate;
            var datesColumn = GetSundayDates(startDate, endDate);
            var empData = ReadEmployeeDetailsGrid(request, null, Convert.ToInt32(managerNameId), datesColumn.Count(), Convert.ToDateTime(startDate));
            return Json(new
            {
                GridData = empData.Data,
                DatesColumn = datesColumn,
            });
        }

        public virtual JsonResult ReadEmployeeDetailsGrid([DataSourceRequest] DataSourceRequest Request, string searchText, int managerId, int datesColumnLength, DateTime startDate)
        {
            List<EmployeeDetails> EmployeeLst = new List<EmployeeDetails>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }

            var details = getEmployeeList(managerId);
            var concurrentResults = new ConcurrentBag<EmployeeDetails>();
            Parallel.ForEach(details, emp =>
            {
                var empData = getTimeSheetData(emp.EmployeeId, startDate);
                EmployeeDetails employee = new EmployeeDetails();
                employee.EmpolyeeName = emp.EmployeeName;
                employee.ManagerName = emp.EmployeeManager;
                if (empData != null)
                {
                    var empDataLength = empData.Count();
                    switch (datesColumnLength)
                    {
                        case 4:
                            {
                                employee.FirstDateColumn = empDataLength <= 0 ? "0" : empData[empDataLength - 1].WeeklyTotalHours.ToString();
                                employee.SecondDateColumn = empDataLength - 1 <= 0 ? "0" : empData[empDataLength - 2].WeeklyTotalHours.ToString();
                                employee.ThirdDateColumn = empDataLength - 2 <= 0 ? "0" : empData[empDataLength - 3].WeeklyTotalHours.ToString();
                                employee.FourthDateColumn = empDataLength - 3 <= 0 ? "0" : empData[empDataLength - 4].WeeklyTotalHours.ToString();
                                employee.Total = (Convert.ToDecimal(employee.FirstDateColumn) + Convert.ToDecimal(employee.SecondDateColumn) + Convert.ToDecimal(employee.ThirdDateColumn) + Convert.ToDecimal(employee.FourthDateColumn)).ToString();
                                break;
                            }
                        case 5:
                            {
                                employee.FirstDateColumn = empDataLength <= 0 ? "0" : empData[empDataLength - 1].WeeklyTotalHours.ToString();
                                employee.SecondDateColumn = empDataLength - 1 <= 0 ? "0" : empData[empDataLength - 2].WeeklyTotalHours.ToString();
                                employee.ThirdDateColumn = empDataLength - 2 <= 0 ? "0" : empData[empDataLength - 3].WeeklyTotalHours.ToString();
                                employee.FourthDateColumn = empDataLength - 3 <= 0 ? "0" : empData[empDataLength - 4].WeeklyTotalHours.ToString();
                                employee.FifthDateColumn = empDataLength - 4 <= 0 ? "0" : empData[empDataLength - 5].WeeklyTotalHours.ToString();
                                employee.Total = (Convert.ToDecimal(employee.FirstDateColumn) + Convert.ToDecimal(employee.SecondDateColumn) + Convert.ToDecimal(employee.ThirdDateColumn) + Convert.ToDecimal(employee.FourthDateColumn) + Convert.ToDecimal(employee.FifthDateColumn)).ToString();
                                break;
                            }
                        case 6:
                            {
                                employee.FirstDateColumn = empDataLength <= 0 ? "0" : empData[empDataLength - 1].WeeklyTotalHours.ToString();
                                employee.SecondDateColumn = empDataLength - 1 <= 0 ? "0" : empData[empDataLength - 2].WeeklyTotalHours.ToString();
                                employee.ThirdDateColumn = empDataLength - 2 <= 0 ? "0" : empData[empDataLength - 3].WeeklyTotalHours.ToString();
                                employee.FourthDateColumn = empDataLength - 3 <= 0 ? "0" : empData[empDataLength - 4].WeeklyTotalHours.ToString();
                                employee.FifthDateColumn = empDataLength - 4 <= 0 ? "0" : empData[empDataLength - 5].WeeklyTotalHours.ToString();
                                employee.SixthDateColumn = empDataLength - 5 <= 0 ? "0" : empData[empDataLength - 6].WeeklyTotalHours.ToString();
                                employee.Total = (Convert.ToDecimal(employee.FirstDateColumn) + Convert.ToDecimal(employee.SecondDateColumn) + Convert.ToDecimal(employee.ThirdDateColumn) + Convert.ToDecimal(employee.FourthDateColumn) + Convert.ToDecimal(employee.FifthDateColumn) + Convert.ToDecimal(employee.SixthDateColumn)).ToString();
                                break;
                            }

                    }
                }
                concurrentResults.Add(employee);
            });
            EmployeeLst.AddRange(concurrentResults);
            int Total = EmployeeLst != null && EmployeeLst.Count > 0 ? EmployeeLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return Json(new DataSourceResult()
            {
                Data = EmployeeLst,
                Total = Total,
            });
        }

        private List<EmployeeModel> getEmployeeList(int selectedManagerId)
        {
            try
            {
                EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
                List<EmployeeModel> EmployeeModelLst = apiExtension.InvokeGet<List<EmployeeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.EmployeeSearch + "?searchBy=" + string.Empty + "&managerId=" + string.Empty + "&pageSize=300" + "&pageNumber=1" + "&sortOrder=" + true + "&sortColumn=EmployeeName"));
                if (selectedManagerId != -1)
                {
                    EmployeeModelLst = EmployeeModelLst.Where(x => x.IsActive == 1 && x.ManagerId == selectedManagerId).ToList();
                }
                return EmployeeModelLst;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message);
                return null;
            }
        }

        private List<string> GetSundayDates(string startDate, string endDate)
        {
            var FromDate = Convert.ToDateTime(startDate);
            var ToDate = Convert.ToDateTime(endDate);
            List<string> dates = new List<string>();
            TimeSpan diff = ToDate - FromDate;
            int days = diff.Days;
            var day = Convert.ToDateTime(startDate).ToString("dddd");
            if (day != "Sunday")
            {
                dates.Add(Convert.ToDateTime(startDate).ToString("dd MMM yyyy", CultureInfo.InvariantCulture));
            }
            for (var i = 0; i <= days; i++)
            {
                var nextDate = FromDate.AddDays(i);
                switch (nextDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        {
                            dates.Add(nextDate.ToString("dd MMM yyyy", CultureInfo.InvariantCulture));
                            break;
                        }

                }
            }
            return dates;
        }

        private List<TimeEntryWeeklyStatusModel> getTimeSheetData(int? employeeId, DateTime searchDate)
        {
            Nullable<DateTime> productionDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ProductionDate"]);
            Nullable<int> monthBack = 12;
            var month = searchDate.ToString("MMMM");
            List<TimeEntryWeeklyStatusModel> WeeklyTimeSheetlst = apiExtension.InvokeGet<List<TimeEntryWeeklyStatusModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntryWeeklyStatus + "?employeeId=" + employeeId + "&monthsBack=" + (monthBack != null ? monthBack : null) + "&productionDate=" + (productionDate != null ? productionDate : null) + "&searchDate=" + searchDate + "&pageSize=300" + "&pageNumber=1" + "&sortOrder=" + String.Empty + "&sortColumn=" + String.Empty));
            if (WeeklyTimeSheetlst != null && WeeklyTimeSheetlst.Count > 0)
            {
                WeeklyTimeSheetlst = WeeklyTimeSheetlst.Where(x => x.WeekWithMonth.Contains(month)).OrderByDescending(x=>x.WeekStartDT).ToList();
            }
            return WeeklyTimeSheetlst;
        }

        [HttpPost]
        public ActionResult ExcelExport(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }
    }
}
