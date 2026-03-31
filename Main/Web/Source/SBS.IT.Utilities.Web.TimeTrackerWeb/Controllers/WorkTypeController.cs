using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Filters;
using SBS.IT.Utilities.Shared.Model;
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
    public class WorkTypeController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;

        public WorkTypeController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration, ISessionCacheManager sessionCacheManager)
        {
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
            this.sessionCacheManager = sessionCacheManager;
        }
        // GET: WorkType
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to bind work type grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadWorkTypeGrid([DataSourceRequest] DataSourceRequest Request, string searchText)
        {
            List<WorkTypeViewModel> WorkTypeLst = new List<WorkTypeViewModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            string sortColumn = "WorkTypeName";
            string sortOrder = "ASC";
            int pageSize = 20;
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
            WorkTypeLst = getWorkTypeList(searchText, sortColumn, sortOrder, pageNumber, pageSize);
            int Total = WorkTypeLst != null && WorkTypeLst.Count > 0 ? WorkTypeLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return Json(new DataSourceResult()
            {
                Data = WorkTypeLst,
                Total = Total,
            });
        }

        /// <summary>
        /// method to bind workType
        /// </summary>
        /// <returns></returns>
        private List<WorkTypeViewModel> getWorkTypeList(string searchText, string sortColumn, string sortOrder, int pageNumber, int pageSize)
        {
            List<WorkTypeViewModel> workTypelst = apiExtension.InvokeGet<List<WorkTypeViewModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.WorkTypeSearch + "?searchBy=" + Uri.EscapeDataString(!string.IsNullOrEmpty(searchText) ? searchText : string.Empty) + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + (sortOrder=="ASC"?true:false) + "&sortColumn=" + sortColumn));
            return workTypelst;
        }

        /// <summary>
        /// Add Worktype
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AddWorkType()
        {
            WorkTypeModel workTypeModel = new WorkTypeModel();
            workTypeModel.WorkTypeCategoryModellst = getWorkTypeCategoryList();
            return PartialView("_AddEditWorkType", workTypeModel);
        }
        private List<WorkTypeCategoryModel> getWorkTypeCategoryList()
        {
            return ReferenceDataCache.GetWorkTypeCategories(apiExtension, apiConfiguration);
        }
        /// <summary>
        /// Saving work type
        /// </summary>
        /// <param name="workTypeModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveWorkType(WorkTypeModel workTypeModel)
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
            workTypeModel.UserId = AuthenticateExtension.UserId;
            string postData = JsonConvert.SerializeObject(workTypeModel);
            savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((workTypeModel != null && workTypeModel.WorkTypeId > 0) ? APIResources.WorkTypeUpdate : APIResources.WorkTypeAdd)), postData);
            if (workTypeModel != null && workTypeModel.WorkTypeId > 0 && savedCount == 1)
                savedCount = -1;
            ModelState.Clear();
            return Json(savedCount);
        }

        /// <summary>
        /// get worktype by work typeID
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        public virtual ActionResult EditWorkType(int WorkTypeId)
        {
            WorkTypeModel workTypeMode = apiExtension.InvokeGet<WorkTypeModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByWorkTypeId + "?workTypeId=" + WorkTypeId));
            workTypeMode.WorkTypeCategoryModellst = getWorkTypeCategoryList();
            return PartialView("_AddEditWorkType", workTypeMode);
        }

        /// <summary>
        /// Delete workType
        /// </summary>
        /// <param name="WorkTypeId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteWokType(int WorkTypeId)
        {
            int deletedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int UpdatedUserId = authenticationModel.EmployeeId;
            if (WorkTypeId > 0)
            {
                deletedCount = apiExtension.InvokeGet<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.WorkTypeDelete + "?workTypeId=" + WorkTypeId + "&deleteUserId=" + UpdatedUserId));
            }
            return Json(deletedCount);
        }
    }
}