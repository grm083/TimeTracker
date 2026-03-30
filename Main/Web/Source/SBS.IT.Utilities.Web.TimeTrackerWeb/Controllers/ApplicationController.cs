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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class ApplicationController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;

        public ApplicationController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration, ISessionCacheManager sessionCacheManager)
        {
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
            this.sessionCacheManager = sessionCacheManager;
        }
        // GET: Application
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to bind application grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadApplicationGrid([DataSourceRequest] DataSourceRequest Request,string searchText)
        {
            List<ApplicationModel> ApplicationLst = new List<ApplicationModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            string sortColumn = "Date";
            int sortOrder = 0; // Desc = 0 and Asc=1
            int pageSize = 25;
            int pageNumber = 1;
            if (Request.Sorts != null && Request.Sorts.Count > 0)
            {
                sortColumn = Request.Sorts.FirstOrDefault().Member;
                if (Request.Sorts.FirstOrDefault().SortDirection == ListSortDirection.Ascending)
                    sortOrder = 1;
            }
            pageSize = Request.PageSize;
            pageNumber = Request.Page;
            ApplicationLst = getapplicationList(searchText, sortColumn, sortOrder, pageNumber, pageSize);
            int RowCount = (ApplicationLst!=null && ApplicationLst.Count >0 ? ApplicationLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0);
            return Json(new DataSourceResult()
            {
                Data = ApplicationLst,
                Total = RowCount,
            });
        }

        /// <summary>
        /// bind application grid
        /// </summary>
        /// <returns></returns>
        private List<ApplicationModel> getapplicationList(string searchText, string sortColumn, int sortOrder, int pageNumber, int pageSize)
        {
            List<ApplicationModel> applicationlst = apiExtension.InvokeGet<List<ApplicationModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetApplicationSearch + "?applicationId=null&searchBy="+ Uri.EscapeDataString(!string.IsNullOrEmpty(searchText) ? searchText : string.Empty)+ "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + sortOrder + "&sortColumn=" + sortColumn));
            return applicationlst;
        }

        /// <summary>
        /// method to add Application
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AddApplication()
        {
            ApplicationModel applicationModel = new ApplicationModel();
            return PartialView("_AddEditApplication", applicationModel);
        }

        /// <summary>
        /// method to save application
        /// </summary>
        /// <param name="applicationModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveApplication(ApplicationModel applicationModel)
        {
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
            string postData = JsonConvert.SerializeObject(applicationModel);
            savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((applicationModel != null && applicationModel.ApplicationId > 0) ? APIResources.ApplicationUpdate : APIResources.ApplicationAdd)), postData);
            if (applicationModel != null && applicationModel.ApplicationId > 0 && savedCount == 1)
                savedCount = -1;
            ModelState.Clear();
            return Json(savedCount);
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteApplication(int ApplicationId)
        {
            int deletedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int UpdatedUserId = authenticationModel.EmployeeId;
            if (ApplicationId > 0)
            {
                deletedCount = apiExtension.InvokeGet<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ApplicationDelete + "?applicationId=" + ApplicationId + "&deleteUserId=" + UpdatedUserId));
            }
            return Json(deletedCount);
        }

        /// <summary>
        /// Edit application
        /// </summary>
        /// <param name="ApplicationId"></param>
        /// <returns></returns>
        public virtual ActionResult EditApplication(int ApplicationId)
        {
            ApplicationModel applicationModel = apiExtension.InvokeGet<ApplicationModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByApplicationId + "?applicationId=" + ApplicationId));
            return PartialView("_AddEditApplication", applicationModel);
        }
    }
}