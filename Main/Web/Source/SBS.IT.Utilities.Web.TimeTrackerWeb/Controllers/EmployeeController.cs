using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Filters;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class EmployeeController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        public EmployeeController()
        {
            apiExtension = new APIExtension();
            apiConfiguration = new APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
        }
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to bind managers
        /// </summary>
        /// <returns></returns>
        private List<Manager> getManagerList()
        {
            List<Manager> TeamlLst = apiExtension.InvokeGet<List<Manager>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetManager));
            return TeamlLst;
        }

        /// <summary>
        /// method to bind Teams
        /// </summary>
        /// <returns></returns>
        private List<Team> getTeamList()
        {
            List<Team> TeamlLst = apiExtension.InvokeGet<List<Team>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTeam));
            return TeamlLst;
        }
        private List<UserType> getUserTypeList()
        {
            List<UserType> UserTypelLst = apiExtension.InvokeGet<List<UserType>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetUserType));
            return UserTypelLst;
        }
        private List<Location> getLocationList()
        {
            List<Location> LocationLst = apiExtension.InvokeGet<List<Location>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetLocation));
            return LocationLst;
        }

        /// <summary>
        /// method to bind employee
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadEmployeeGrid([DataSourceRequest] DataSourceRequest Request,string searchText)
        {
            List<EmployeeModel> EmployeeLst = new List<EmployeeModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            string sortColumn = "EmployeeName";
            string sortOrder = "ASC"; 
            int pageSize = 25;
            int pageNumber = 1;
            if (Request.Sorts != null && Request.Sorts.Count > 0)
            {
                sortColumn = Request.Sorts.FirstOrDefault().Member;
                if (Request.Sorts.FirstOrDefault().SortDirection == ListSortDirection.Ascending)
                    sortOrder = "ASC";
                else
                    sortOrder = "Desc";
            }
            pageSize = Request.PageSize;
            pageNumber = Request.Page;
            EmployeeLst = getEmployeeList(searchText, sortColumn, sortOrder, pageNumber, pageSize);
            int Total = EmployeeLst != null && EmployeeLst.Count > 0 ? EmployeeLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return Json(new DataSourceResult()
            {
                Data = EmployeeLst,
                Total = Total,
            });
        }

        private List<EmployeeModel> getEmployeeList(string searchText, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int managerId = authenticationModel.EmployeeId;
            List<EmployeeModel> EmployeeModelLst = apiExtension.InvokeGet<List<EmployeeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.EmployeeSearch + "?searchBy=" + (!string.IsNullOrEmpty(searchText) ? searchText : string.Empty) + "&managerId=" + managerId + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + (sortOrder == "ASC" ? true : false) + "&sortColumn=" + sortColumn));
            return EmployeeModelLst;
        }

        /// <summary>
        /// method to add Employee
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AddEmployee()
        {
            EmployeeModel employeeModel = new EmployeeModel();
            employeeModel.TeamList = getTeamList();
            employeeModel.ManagerList = getManagerList();
            employeeModel.TimeZonelst = getTimeZoneList();
            employeeModel.LocationList = getLocationList();
            employeeModel.UserTypeList = getUserTypeList();
            employeeModel.EmploymentTypeList = getEmploymentTypeList();
            return PartialView("_AddEditEmployee", employeeModel);
        }

        /// <summary>
        /// bind timezone
        /// </summary>
        /// <returns></returns>
        private List<EmployeeTimeZone> getTimeZoneList()
        {
            List<EmployeeTimeZone> TimeZonelst = apiExtension.InvokeGet<List<EmployeeTimeZone>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetTimeZone));
            return TimeZonelst;
        }
        private List<EmploymentType> getEmploymentTypeList()
        {
            List<EmploymentType> EmploymentTypelst = apiExtension.InvokeGet<List<EmploymentType>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetEmploymentType));
            return EmploymentTypelst;
        }

        /// <summary>
        /// method to save employee
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveEmployee(EmployeeModel employeeModel)
        {

            if(employeeModel.EmploymentTypeId == 2 && string.IsNullOrEmpty(employeeModel.CompanyName)) //Contractor
            {
                ModelState.AddModelError("Contractor", "Company is Required");
            }
            if (!(string.IsNullOrEmpty(employeeModel.LogonName) || employeeModel.LogonName.Length == 0) && employeeModel.EmployeeId<=0)
            {
                if (!apiExtension.InvokeGet<bool>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.CheckLogonName + "?logonName=" + employeeModel.LogonName)))
                {
                    ModelState.AddModelError("InvalidUser", "User not exist");
                }
            }
            if (!ModelState.IsValid)
            {
                List<string> modelErrors = new List<string>();
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        modelErrors.Add(error.ErrorMessage);
                    }
                }
                return Json(modelErrors);
            }

            int savedCount = 0;
            employeeModel.UserId = AuthenticateExtension.UserId;
            string postData = JsonConvert.SerializeObject(employeeModel);
            savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((employeeModel != null && employeeModel.EmployeeId > 0) ? APIResources.EmployeeUpdate : APIResources.EmployeeAdd)), postData);
            if (employeeModel != null && employeeModel.EmployeeId > 0 && savedCount == 1)
                savedCount = -1;
            ModelState.Clear();
            return Json(savedCount);
        }
        public JsonResult ValidateUserName(string userName)
        {
            bool IsExist = false;
            IsExist = apiExtension.InvokeGet<bool>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.CheckLogonName + "?logonName=" + userName));
            return Json(IsExist, JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult DeleteEmployee(int EmployeeId)
        {
            int deletedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int userId = authenticationModel.EmployeeId;
            if (EmployeeId > 0)
            {
                deletedCount = apiExtension.InvokeGet<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.EmployeeDelete + "?employeeId=" + EmployeeId + "&deleteUserId=" + userId));
            }
            return Json(deletedCount);
        }

        public virtual ActionResult EditEmployee(int EmployeeId)
        {
            EmployeeModel employeeModel = apiExtension.InvokeGet<EmployeeModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByEmployeeId + "?employeeId=" + EmployeeId));
            employeeModel.TeamList = getTeamList();
            employeeModel.ManagerList = getManagerList();
            employeeModel.TimeZonelst = getTimeZoneList();
            employeeModel.LocationList = getLocationList();
            employeeModel.UserTypeList = getUserTypeList();
            employeeModel.EmploymentTypeList = getEmploymentTypeList();
            return PartialView("_AddEditEmployee", employeeModel);
        }
    }
}