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
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class ProjectController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        public ProjectController()
        {
            apiExtension = new APIExtension();
            apiConfiguration = new APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
        }
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to bind Project grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadProjectGrid([DataSourceRequest] DataSourceRequest Request, string searchText)
        {
            List<ProjectModel> ProjectModelLst = new List<ProjectModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            string sortColumn = "ProjectName";
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

            ProjectModelLst = getProjectSearch(searchText, sortColumn, sortOrder, pageNumber, pageSize);
            int Total = ProjectModelLst != null && ProjectModelLst.Count > 0 ? ProjectModelLst.FirstOrDefault().RowTotal.GetValueOrDefault(0) : 0;
            return Json(new DataSourceResult()
            {
                Data = ProjectModelLst,
                Total = Total,
            });
        }

        private List<ProjectModel> getProjectSearch(string searchText, string sortColumn, int sortOrder, int pageNumber, int pageSize)
        {
            List<ProjectModel> ProjectModelLst = apiExtension.InvokeGet<List<ProjectModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectSearch + "?searchBy=" + Uri.EscapeDataString(!string.IsNullOrEmpty(searchText) ? searchText : string.Empty) + "&pageSize=" + pageSize + "&pageNumber=" + pageNumber + "&sortOrder=" + (sortOrder == 1 ? true:false) + "&sortColumn=" + sortColumn));
            return ProjectModelLst;
        }

        public virtual ActionResult AddProject()
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.ProjectTypelst = getProjectTypeList();
            return PartialView("_AddEditProject", projectModel);
        }

        /// <summary>
        /// get application
        /// </summary>
        /// <returns></returns>
        private List<ProjectTypeModel> getProjectTypeList()
        {
            List<ProjectTypeModel> projectTypelst = apiExtension.InvokeGet<List<ProjectTypeModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetProjectType));
            return projectTypelst;
        }

        /// <summary>
        /// method to save Project
        /// </summary>
        /// <param name="ProjectModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveProject(ProjectModel projectModel)
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
            projectModel.UserId = AuthenticateExtension.UserId;
            string postData = JsonConvert.SerializeObject(projectModel);
            savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + ((projectModel != null && projectModel.ProjectId > 0) ? APIResources.ProjectUpdate : APIResources.ProjectAdd)), postData);
            if (projectModel != null && projectModel.ProjectId > 0 && savedCount == 1)
                savedCount = -1;
            ModelState.Clear();
            return Json(savedCount);
        }

        /// <summary>
        /// Delete projects
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public virtual JsonResult DeleteProjects(int ProjectId)
        {
            int deletedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            int EmployeeId = authenticationModel.EmployeeId;
            if (ProjectId > 0)
            {
                string postData = JsonConvert.SerializeObject(new { projectId = ProjectId, deleteUserId = EmployeeId });
                deletedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ProjectDelete), postData);
            }
            return Json(deletedCount);
        }

        public virtual ActionResult EditProjects(int ProjectId)
        {
            ProjectModel projectModel = apiExtension.InvokeGet<ProjectModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetByProjectId + "?projectId=" + ProjectId));
            projectModel.ProjectTypelst = getProjectTypeList();
            return PartialView("_AddEditProject", projectModel);
        }
    }
}