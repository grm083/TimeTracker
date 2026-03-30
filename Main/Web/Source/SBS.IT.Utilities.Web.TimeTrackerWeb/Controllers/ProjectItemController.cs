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

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class ProjectItemController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;

        public ProjectItemController()
        {
            apiExtension = new APIExtension();
            apiConfiguration =new  APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
        }

        // GET: ProjectItem
        public ActionResult Index()
        {
            var getProjectItemStatusLst = getProjectItemStatusList();
            ViewData["projectItemStatusList"] = getProjectItemStatusLst;
            ViewData["defaultProjectItemStatus"] = getProjectItemStatusLst.First();
          
            return View();
        }

        /// <summary>
        /// Add work item
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AddProjectItem()
        {
            ProjectItemModel projectItemModel = new ProjectItemModel();
            projectItemModel.ProjectList = getProjectList();
            projectItemModel.ProjectItemStatusList = getProjectItemStatusList();
            return PartialView("_AddEditProjectItem", projectItemModel);
        }

        /// <summary>
        /// method to populate projects
        /// </summary>
        /// <returns></returns>
        private List<ProjectListModel> getProjectList()
        {
            List<ProjectListModel> projectTypelst = apiExtension.InvokeGet<List<ProjectListModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProject));
            return projectTypelst;
        }

        /// <summary>
        /// get ProjectItemStatus
        /// </summary>
        /// <returns></returns>
        private List<ProjectItemStatusModel> getProjectItemStatusList()
        {
            List<ProjectItemStatusModel> projectItemStatuslst = apiExtension.InvokeGet<List<ProjectItemStatusModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProjectItemStatus));
            return projectItemStatuslst;
        }

        /// <summary>
        /// method to bind ProjectItem grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadProjectItemGrid([DataSourceRequest] DataSourceRequest Request,string searchText)
        {
            List<ProjectItemModel> ProjectItemLst = new List<ProjectItemModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            string sortColumn = "ProjectItemName";
            int sortOrder = 1; // Desc = 0 and Asc=1
            int pageSize = 25;
            int pageNumber = 1;
            if (Request.Sorts != null && Request.Sorts.Count > 0)
            {
                sortColumn = Request.Sorts.FirstOrDefault().Member;
                if (Request.Sorts.FirstOrDefault().SortDirection == ListSortDirection.Ascending)
                    sortOrder = 1;
                else
                    sortOrder = 0;
            }
            pageSize = Request.PageSize;
            pageNumber = Request.Page;
            ProjectItemLst = ProjectItems(searchText, sortColumn, sortOrder, pageNumber, pageSize);
            foreach (var item in ProjectItemLst)
            {
                item.ProjectItemStatus = new ProjectItemStatusModel { ProjectItemStatusId = (int)item.ProjectItemStatusId, ProjectItemStatusName = item.ProjectItemStatusName };
              
            }
            int Total = ProjectItemLst != null && ProjectItemLst.Count > 0 ? ProjectItemLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;

            return Json(new DataSourceResult()
            {
                Data = ProjectItemLst,
                Total = Total,
            });
        }

        //public virtual JsonResult GetProjectByApplicationId(int? applicationId)
        //{
        //    List<ProjectListModel> projectLst = new List<ProjectListModel>();
        //    projectLst = getProjectList();
        //    return Json(projectLst, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// list of project items
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrder"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<ProjectItemModel> ProjectItems(string searchText, string sortColumn, int sortOrder, int pageNumber, int pageSize)
        {
            List<ProjectItemModel> ProjectItemLst = apiExtension.InvokeGet<List<ProjectItemModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectItemSearch + "?searchBy=" + (!string.IsNullOrEmpty(searchText) ? searchText : string.Empty) + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + (sortOrder==1?true:false) + "&sortColumn=" + sortColumn));
            return ProjectItemLst;
        }

        /// <summary>
        /// method to save project item
        /// </summary>
        /// <param name="projectItemModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveProjectItems(ProjectItemModel projectItemModel)
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
            projectItemModel.UserId = AuthenticateExtension.UserId;
            string postData = JsonConvert.SerializeObject(projectItemModel);
            savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((projectItemModel != null && projectItemModel.ProjectItemId > 0) ? APIResources.ProjectItemUpdate : APIResources.ProjectItemAdd)), postData);
            if (projectItemModel != null && projectItemModel.ProjectItemId > 0 && savedCount == 1)
                savedCount = -1;
            ModelState.Clear();
            return Json(savedCount);
        }

        /// <summary>
        /// Delete project items
        /// </summary>
        /// <param name="ProjectItemId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteProjectItem(int ProjectItemId)
        {
            int deletedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int EmployeeId = authenticationModel.EmployeeId;
            if (ProjectItemId > 0)
            {
                deletedCount = apiExtension.InvokeGet<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectItemDelete + "?projectItemId=" + ProjectItemId + "&deleteUserId=" + EmployeeId));
            }
            return Json(deletedCount);
        }

        /// <summary>
        /// get project items
        /// </summary>
        /// <param name="ProjectItemId"></param>
        /// <returns></returns>
        public virtual ActionResult EditProjectItems(int ProjectItemId)
        {
            ProjectItemModel projectItemModel = apiExtension.InvokeGet<ProjectItemModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByProjectItemId + "?projectItemId=" + ProjectItemId));
            projectItemModel.ProjectList = getProjectList();
            projectItemModel.ProjectItemStatusList = getProjectItemStatusList();
            return PartialView("_AddEditProjectItem", projectItemModel);
        }
        /// <summary>
        /// update project item status on grid
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ProjectItems"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProjectItem_Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProjectItemModel> ProjectItems)
        {
            if (ProjectItems != null && ModelState.IsValid)
            {
                foreach (var projectItemModel in ProjectItems)
                {

                    projectItemModel.ProjectItemStatusId = projectItemModel.ProjectItemStatus.ProjectItemStatusId;
                    string postData = JsonConvert.SerializeObject(projectItemModel);
                    apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((projectItemModel != null && projectItemModel.ProjectItemId > 0) ? APIResources.ProjectItemUpdate : APIResources.ProjectItemAdd)), postData);
                    //productService.Update(product);
                }
            }
            return Json(ProjectItems.ToDataSourceResult(request, ModelState));
           // return Json(1);
        }

    }
}
