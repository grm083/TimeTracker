using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Extension;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Filters;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class TimeSheetController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        private readonly ILogger logger;
        public TimeSheetController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration, ISessionCacheManager sessionCacheManager, ILogger logger)
        {
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
            this.sessionCacheManager = sessionCacheManager;
            this.logger = logger;
        }
        // GET: TimeSheet
        public ActionResult Index(Nullable<int> employeeId, string startDate, int isFilter = 0)
        {

            if (employeeId != null && employeeId > 0 && !string.IsNullOrEmpty(startDate))
            {
                ViewBag.EmployeeId = employeeId;
                ViewBag.StartDate = startDate;
                ViewBag.IsFilter = isFilter;
            }
            return View();
        }

        /// <summary>
        /// bind timesheet grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadTimeSheetGrid([DataSourceRequest] DataSourceRequest Request, string searchText, Nullable<int> employeeId, string startDate, int isFilter = 0)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            List<TimeEntrySearchModel> TimeSheetlst = new List<TimeEntrySearchModel>();
            int Total = 0;

            try
            {
                if (authenticationModel != null && !string.IsNullOrEmpty(authenticationModel.UserType) && authenticationModel.UserType == "SAN")
                    employeeId = null;
                else
                    employeeId = (employeeId != null && employeeId > 0) ? employeeId : authenticationModel.EmployeeId;
                string sortColumn = "Date";
                int sortOrder = 0; // Desc = 0 and Asc=1
                int pageSize = 20;
                int pageNumber = 1;
                if (!ModelState.IsValid)
                {
                    return Json(ModelState.ToDataSourceResult());
                }
                if (Request.Sorts != null && Request.Sorts.Count > 0)
                {
                    sortColumn = Request.Sorts.FirstOrDefault().Member;
                    if (Request.Sorts.FirstOrDefault().SortDirection == ListSortDirection.Ascending)
                        sortOrder = 1;
                }
                pageSize = Request.PageSize;
                pageNumber = Request.Page;
                TimeSheetlst = getTimeSheetData(employeeId, sortColumn, sortOrder, pageNumber, pageSize, searchText, startDate);
                Total = TimeSheetlst != null && TimeSheetlst.Count > 0 ? TimeSheetlst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return Json(new DataSourceResult()
            {
                Data = TimeSheetlst,
                Total = Total,
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, TimeEntrySearchModel timeentrySearchModel)
        {
            int savedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                if (timeentrySearchModel != null && ModelState.IsValid)
                {
                    if (!ModelState.IsValid)
                    {
                        return Json(ModelState.ToDataSourceResult());
                    }
                    if (timeentrySearchModel.TimeEntryId > 0)
                    {
                        TimeSheetModel model = new TimeSheetModel();
                        model.TimeEntry = new List<TimeEntry>();
                        TimeEntry requestModel = new TimeEntry();
                        requestModel.TimeEntryId = timeentrySearchModel.TimeEntryId;
                        requestModel.EmployeeId = timeentrySearchModel.EmployeeId;
                        requestModel.WorkTypeId = timeentrySearchModel.WorkTypeId;
                        requestModel.ProjectId = timeentrySearchModel.ProjectId;
                        requestModel.ProjectItemId = timeentrySearchModel.ProjectItemId;
                        requestModel.Date = timeentrySearchModel.Date.Value;
                        requestModel.WorkHour = timeentrySearchModel.WorkHour;
                        requestModel.Comments = timeentrySearchModel.Comments;
                        requestModel.WorkItem = timeentrySearchModel.WorkItem;
                        model.UserId = requestModel.UserId = authenticationModel.EmployeeId;
                        model.TimeEntry.Add(requestModel);
                        string postData = JsonConvert.SerializeObject(model);
                        savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.UpdateTimeEntry), postData);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }

            return Json(savedCount);
        }

        /// <summary>
        /// method to bind Timesheet grid
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<TimeEntrySearchModel> getTimeSheetData(int? employeeId, string sortColumn, int sortOrder, int pageNumber, int pageSize, string searchText, string startDate)
        {
            string endDate = string.Empty;
            if (!string.IsNullOrEmpty(startDate))
            {
                endDate = Convert.ToDateTime(startDate).AddDays(6).ToString();
            }
            List<TimeEntrySearchModel> TimeSheetlst = apiExtension.InvokeGet<List<TimeEntrySearchModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntrySearch + "?employeeId=" + (employeeId == 0 ? null : employeeId) + "&searchBy=" + Uri.EscapeDataString(!string.IsNullOrEmpty(searchText) ? searchText : string.Empty) + "&timeEntryDateFrom=" + (!string.IsNullOrEmpty(startDate) ? startDate : string.Empty) + "&timeEntryDateTo=" + (!string.IsNullOrEmpty(endDate) ? endDate : string.Empty) + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + sortOrder + "&sortColumn=" + sortColumn));
            return TimeSheetlst;
        }

        /// <summary>
        /// method to edit the timesheet
        /// </summary>
        /// <param name="TimeEntryId"></param>
        /// <returns></returns>
        public virtual ActionResult EditTimeSheet(Nullable<int> TimeEntryId)
        {
            TimeEntrySearchModel timeentrySearchModel = new TimeEntrySearchModel();
            try
            {
                if (TimeEntryId > 0)
                {
                    timeentrySearchModel = getTimeEntryById(TimeEntryId);
                    timeentrySearchModel.WorkTypelst = getWorkTypeList();
                    timeentrySearchModel.Applicationlst = getapplicationList();
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
            }
            return PartialView("_EditTimeSheet", timeentrySearchModel);
        }

        /// <summary>
        /// method to bind Get TimeEntry
        /// </summary>
        /// <param name="timeEntryId"></param>
        /// <returns></returns>
        private TimeEntrySearchModel getTimeEntryById(int? timeEntryId)
        {
            TimeEntrySearchModel timeentrySearchModel = new TimeEntrySearchModel();
            try
            {
                TimeEntry timeEntry = apiExtension.InvokeGet<TimeEntry>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntry + "?timeEntryId=" + timeEntryId));
                if (timeEntry != null && timeEntry.TimeEntryId > 0)
                {
                    timeentrySearchModel.TimeEntryId = timeEntry.TimeEntryId;
                    timeentrySearchModel.EmployeeId = timeEntry.EmployeeId;
                    timeentrySearchModel.WorkTypeId = timeEntry.WorkTypeId;
                    timeentrySearchModel.ApplicationId = timeEntry.ApplicationId;
                    timeentrySearchModel.ProjectItemId = timeEntry.ProjectItemId;
                    timeentrySearchModel.Date = timeEntry.Date;
                    timeentrySearchModel.WorkHour = Convert.ToDecimal(timeEntry.WorkHour);
                    timeentrySearchModel.Comments = timeEntry.Comments;
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
            }
            return timeentrySearchModel;
        }

        /// <summary>
        /// bind application
        /// </summary>
        /// <returns></returns>
        private List<ApplicationModel> getapplicationList()
        {
            return ReferenceDataCache.GetApplications(apiExtension, apiConfiguration);
        }

        /// <summary>
        /// get project list
        /// </summary>
        /// <returns></returns>
        private List<ProjectListModel> getallProjectList()
        {
            return ReferenceDataCache.GetProjects(apiExtension, apiConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<WorkTypeModel> getWorkTypeList()
        {
            return ReferenceDataCache.GetWorkTypes(apiExtension, apiConfiguration);
        }

        private List<ProjectItemListModel> getProjectItemList()
        {
            List<ProjectItemListModel> projectTypelst = apiExtension.InvokeGet<List<ProjectItemListModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProjectItem + "?projectId="));
            return projectTypelst;
        }

        /// <summary>
        /// method to update the timesheet
        /// </summary>
        /// <param name="timeentrySearchModel"></param>
        /// <returns></returns>
        public virtual JsonResult UpdateTimeSheet(TimeEntrySearchModel timeentrySearchModel)
        {
            int savedCount = 0;
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(ModelState.ToDataSourceResult());
                }
                if (timeentrySearchModel.TimeEntryId > 0)
                {
                    TimeSheetModel timeSheetmodel = new TimeSheetModel();
                    TimeEntry requestModel = new TimeEntry();
                    timeSheetmodel.TimeEntry = new List<TimeEntry>();
                    requestModel.TimeEntryId = timeentrySearchModel.TimeEntryId;
                    requestModel.EmployeeId = timeentrySearchModel.EmployeeId;
                    requestModel.WorkTypeId = timeentrySearchModel.WorkTypeId;
                    requestModel.ApplicationId = timeentrySearchModel.ApplicationId;
                    requestModel.ProjectItemId = timeentrySearchModel.ProjectItemId;
                    requestModel.Date = timeentrySearchModel.Date.Value;
                    requestModel.WorkHour = timeentrySearchModel.WorkHour;
                    requestModel.Comments = timeentrySearchModel.Comments;
                    timeSheetmodel.TimeEntry.Add(requestModel);
                    string postData = JsonConvert.SerializeObject(timeSheetmodel);
                    savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.SaveTimeSheet), postData);
                    if (savedCount > 0)
                        TempData["TimeSheetMessage"] = "TimeSheet updated successfully";
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
            }
            return Json(savedCount);
        }

        public ActionResult Add(string startDate)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                if (authenticationModel != null && !string.IsNullOrEmpty(authenticationModel.TeamCode) && (authenticationModel.UserType == "ADN" || authenticationModel.UserType == "SAN"))
                {
                    return RedirectToAction("AdminTimesheet", new { startDate = startDate });
                }
                if (authenticationModel != null && authenticationModel.IsTimeEntryEnable == 0)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }

                if (string.IsNullOrEmpty(startDate))
                {
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");
                }
                DateTime startDT;
                if (!string.IsNullOrEmpty(startDate))
                {
                    startDT = Convert.ToDateTime(startDate);
                    startDate = startDT.AddDays(-(int)startDT.DayOfWeek).ToString("yyyy-MM-dd");
                }
                string pendingDate = getLastTimeEntry();
                ViewBag.IsPendingAvailable = true;
                ViewBag.UserId = authenticationModel.EmployeeId;
                if (!string.IsNullOrEmpty(pendingDate))
                {
                    pendingDate = Convert.ToDateTime(pendingDate).ToString("yyyy-MM-dd");
                    var thisWeekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");
                    if (string.Equals(thisWeekStart, pendingDate))
                    {
                        ViewBag.IsPendingAvailable = false;
                    }
                    ViewBag.thisWeekStart = thisWeekStart;
                }
                ViewBag.TeamCode = authenticationModel.TeamCode;
                var workTypelst = getWorkTypeList();
                ViewBag.TeamWorkType = TeamWorkTypeHelper.GetDefaultWorkTypeId(authenticationModel.TeamCode, workTypelst);
                ViewBag.PendingDate = pendingDate;
                ViewBag.StartDate = startDate;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return View();
        }

        public ActionResult AdminTimesheet(string startDate)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                string pendingDate = getLastTimeEntry();
                ViewBag.IsPendingAvailable = true;
                if (string.IsNullOrEmpty(startDate))
                {
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");

                }
                DateTime startDT;
                if (!string.IsNullOrEmpty(startDate))
                {
                    startDT = Convert.ToDateTime(startDate);
                    startDate = startDT.AddDays(-(int)startDT.DayOfWeek).ToString("yyyy-MM-dd");
                }
                ViewBag.UserId = authenticationModel.EmployeeId;
                ViewBag.TeamCode = authenticationModel.TeamCode;
                if (!string.IsNullOrEmpty(pendingDate))
                {
                    pendingDate = Convert.ToDateTime(pendingDate).ToString("yyyy-MM-dd");
                    var thisWeekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");
                    if (string.Equals(thisWeekStart, pendingDate))
                    {
                        ViewBag.IsPendingAvailable = false;
                    }
                    ViewBag.thisWeekStart = thisWeekStart;
                }
                var workTypelst = getWorkTypeList();
                ViewBag.TeamWorkType = TeamWorkTypeHelper.GetDefaultWorkTypeId(authenticationModel.TeamCode, workTypelst);
                ViewBag.PendingDate = pendingDate;
                ViewBag.StartDate = startDate;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return View();
        }

        public ActionResult Timesheet(string startDate)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                string pendingDate = getLastTimeEntry();
                ViewBag.IsPendingAvailable = true;

                if (string.IsNullOrEmpty(startDate))
                {
                    startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");

                }
                DateTime startDT;
                if (!string.IsNullOrEmpty(startDate))
                {
                    startDT = Convert.ToDateTime(startDate);
                    startDate = startDT.AddDays(-(int)startDT.DayOfWeek).ToString("yyyy-MM-dd");
                }
                ViewBag.UserId = authenticationModel.EmployeeId;
                if (!string.IsNullOrEmpty(pendingDate))
                {
                    pendingDate = Convert.ToDateTime(pendingDate).ToString("yyyy-MM-dd");
                    var thisWeekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");
                    if (string.Equals(thisWeekStart, pendingDate))
                    {
                        ViewBag.IsPendingAvailable = false;
                    }
                    ViewBag.thisWeekStart = thisWeekStart;
                }

                ViewBag.PendingDate = pendingDate;
                ViewBag.StartDate = startDate;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return View();
        }

        public string getLastTimeEntry()
        {
            string lastTimeEntry;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            Nullable<DateTime> productionDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ProductionDate"]);
            lastTimeEntry = apiExtension.InvokeGet<string>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntryLastEntry + "?employeeId=" + authenticationModel.EmployeeId + "&productionDate=" + productionDate));

            if (!string.IsNullOrEmpty(lastTimeEntry))
            {
                //lastTimeEntry = lastTimeEntry.Replace("1", "1st").Replace("2", "2nd").Replace("3", "3rd").Replace("4", "4th").Replace("5", "5th");
                return lastTimeEntry;
            }
            else
                return string.Empty;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TimeSheetModel timesheetmodel)
        {

            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                if (timesheetmodel == null)
                {
                    ModelState.AddModelError("", "Values Posted Are Not Accurate");
                    return View();
                }

                TimeSheetMaster objtimesheetmaster = new TimeSheetMaster();
                objtimesheetmaster.TimeSheetMasterID = 0;
                objtimesheetmaster.UserID = authenticationModel.EmployeeId;
                objtimesheetmaster.CreatedOn = DateTime.Now;

                TempData["TimeCardMessage"] = "Data Saved Successfully";

            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return RedirectToAction("Add", "TimeSheet");
        }

        public JsonResult GetAllApplication()
        {
            try
            {
                List<ApplicationModel> applicationList = apiExtension.InvokeGet<List<ApplicationModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAllApplication));
                return Json(applicationList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
                return Json(new List<ApplicationModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllProjectItem(int applicationId)
        {
            try
            {
                List<ProjectItemListModel> projectItemList = apiExtension.InvokeGet<List<ProjectItemListModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProjectItemByApplicationId + "?applicationId=" + applicationId));
                return Json(projectItemList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
                return Json(new List<ProjectItemListModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllProjectItembyProjectId(int projectId)
        {
            List<ProjectItemListModel> projectItemList = new List<ProjectItemListModel>();
            try
            {
                projectItemList = apiExtension.InvokeGet<List<ProjectItemListModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProjectItem + "?projectId=" + projectId));
                return Json(projectItemList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
                return Json(projectItemList, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetAllWorkType()
        {
            try
            {
                List<WorkTypeModel> workTypeList = apiExtension.InvokeGet<List<WorkTypeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAllWorkType + "?isActive=true"));
                return Json(workTypeList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
                return Json(new List<WorkTypeModel>(), JsonRequestBehavior.AllowGet);
            }
        }


        [ValidateInput(false)]
        public JsonResult SavetimeSheet(TimeSheetModel model)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            String responseData = String.Empty;
            try
            {
                if (authenticationModel == null)
                {
                    return Json(new
                    {
                        redirectUrl = Url.Action("Login", "Account"),
                        isRedirect = true
                    });
                }
                var workTypelst = getWorkTypeList();
                var projectlst = getallProjectList();
                var projectItemlst = getProjectItemList();
                var validationService = new TimeEntryValidationService(logger);
                if (validationService.Validate(model, workTypelst, projectlst, projectItemlst, ref responseData))
                {
                    model.CreateUserId = model.UserId = authenticationModel.EmployeeId;
                    var postData = JsonConvert.SerializeObject(model);
                    responseData = apiExtension.InvokePost<string>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.SaveTimeSheet), postData);

                    return Json(responseData, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(responseData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// method to deletetime sheet
        /// </summary>
        /// <param name="TimeEntryId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteRows(string timeentryIds)
        {
            int deletedCount = 0;
            int deletedUserId = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            try
            {
                if (authenticationModel != null && authenticationModel.EmployeeId > 0)
                    deletedUserId = authenticationModel.EmployeeId;
                if (!string.IsNullOrEmpty(timeentryIds))
                {
                    TimeEntryDeleteModel requestModel = new TimeEntryDeleteModel() { TimeEntryId = timeentryIds, DeleteUserId = deletedUserId };
                    string postData = JsonConvert.SerializeObject(requestModel);
                    deletedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.DeleteTimeEntry), postData);
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return Json(deletedCount, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// method to deletetime sheet
        /// </summary>
        /// <param name="TimeEntryId"></param>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteTimeSheet(int TimeEntryId, int EmployeeId)
        {
            int deletedCount = 0;
            try
            {
                if (TimeEntryId > 0)
                {
                    TimeEntryDeleteModel requestModel = new TimeEntryDeleteModel() { TimeEntryId = Convert.ToString(TimeEntryId), DeleteUserId = EmployeeId };
                    string postData = JsonConvert.SerializeObject(requestModel);
                    deletedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.DeleteTimeEntry), postData);
                    //deletedCount = apiExtension.InvokeGet<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.DeleteTimeEntry + "?timeEntryId=" + TimeEntryId + "&deleteUserId=" + EmployeeId));
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, string.Empty, ex);
            }
            return Json(deletedCount);
        }

        public ActionResult WeeklyTimeSheet()
        {
            return View();
        }

        public virtual JsonResult ReadWeeklyTimeSheetGrid([DataSourceRequest] DataSourceRequest Request)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            List<TimeEntryWeeklyStatusModel> WeeklyTimeSheetlst = new List<TimeEntryWeeklyStatusModel>();
            int Total = 0;
            try
            {
                Nullable<int> employeeId = 0;
                employeeId = authenticationModel.EmployeeId;
                string sortColumn = "Date";
                int sortOrder = 0; // Desc = 0 and Asc=1
                int pageSize = 20;
                int pageNumber = 1;
                if (!ModelState.IsValid)
                {
                    return Json(ModelState.ToDataSourceResult());
                }
                if (Request.Sorts != null && Request.Sorts.Count > 0)
                {
                    sortColumn = Request.Sorts.FirstOrDefault().Member;
                    if (Request.Sorts.FirstOrDefault().SortDirection == ListSortDirection.Ascending)
                        sortOrder = 1;
                }
                pageSize = Request.PageSize;
                pageNumber = Request.Page;
                WeeklyTimeSheetlst = getTimeSheetData(employeeId, null, sortColumn, sortOrder, pageNumber, pageSize);
                Total = WeeklyTimeSheetlst != null && WeeklyTimeSheetlst.Count > 0 ? WeeklyTimeSheetlst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, authenticationModel?.UserName, string.Empty, ex.Message, ex);
            }
            return Json(new DataSourceResult()
            {
                Data = WeeklyTimeSheetlst,
                Total = Total,
            });
        }

        /// <summary>
        /// Bind timesheet grid
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<TimeEntryWeeklyStatusModel> getTimeSheetData(int? employeeId, DateTime? searchDate, string sortColumn, int sortOrder, int pageNumber, int pageSize)
        {
            Nullable<DateTime> productionDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ProductionDate"]);
            Nullable<int> monthBack = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MonthBack"]);
            List<TimeEntryWeeklyStatusModel> WeeklyTimeSheetlst = apiExtension.InvokeGet<List<TimeEntryWeeklyStatusModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntryWeeklyStatus + "?employeeId=" + employeeId + "&monthsBack=" + (monthBack != null ? monthBack : null) + "&productionDate=" + (productionDate != null ? productionDate : null) + "&searchDate=" + searchDate + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + sortOrder + "&sortColumn=" + sortColumn));
            return WeeklyTimeSheetlst;
        }

        /// <summary>
        /// get existing time entry data
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public virtual JsonResult getExistingTimeSheet(string startDate, int? employeeId)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (employeeId == null || employeeId <= 0)
                employeeId = authenticationModel.EmployeeId;
            List<TimeEntry> model = apiExtension.InvokeGet<List<TimeEntry>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntry + "?employeeId=" + employeeId + "&date=" + startDate));
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get existing time entry projects
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public virtual JsonResult getExistingTimeSheetProjects(string startDate, int? employeeId)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (employeeId == null || employeeId <= 0)
                employeeId = authenticationModel.EmployeeId;
            List<TimeEntry> model = apiExtension.InvokeGet<List<TimeEntry>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeEntryDistinctRecords + "?employeeId=" + employeeId + "&date=" + startDate));
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get project list
        /// </summary>
        /// <returns></returns>
        public JsonResult getProjectList()
        {
            try
            {
                List<ProjectListModel> projectTypelst = apiExtension.InvokeGet<List<ProjectListModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProject + "?applicationId="));
                return Json(projectTypelst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message);
                return Json(new List<ProjectListModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllEmployee()
        {
            try
            {
                EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
                int? managerId = authenticationModel.EmployeeId;
                List<EmployeeModel> employeelst = new List<EmployeeModel>();
                employeelst.Add(new EmployeeModel { EmployeeName = "Self", EmployeeId = managerId.GetValueOrDefault(0) });
                employeelst.AddRange(apiExtension.InvokeGet<List<EmployeeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetEmployeeListByManagerId + "?managerId=" + managerId)));
                return Json(employeelst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message);
                return Json(new List<EmployeeModel>(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get by employee ID
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public JsonResult GetByEmployeeId(int employeeId)
        {
            EmployeeModel employeeModel = apiExtension.InvokeGet<EmployeeModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByEmployeeId + "?employeeId=" + employeeId));
            try
            {
                var workTypelst = getWorkTypeList();
                employeeModel.TeamWorkType = TeamWorkTypeHelper.GetDefaultWorkTypeId(employeeModel?.TeamCode, workTypelst);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message);
            }
            return Json(employeeModel, JsonRequestBehavior.AllowGet);
        }
    }
}