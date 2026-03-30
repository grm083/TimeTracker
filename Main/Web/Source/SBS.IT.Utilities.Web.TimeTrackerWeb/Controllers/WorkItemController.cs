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

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [Authorize]
    public class WorkItemController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;

        HttpClient client;
        //The URL of the WEB API Service
        string url = "http://mpindo02pc207.wm.com/SBS.IT.Utilities.TimeTrackerWebAPI/api/Project/";

        public WorkItemController(IAPIExtension apiExtension, IAPIConfiguration apiConfiguration)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.apiExtension = apiExtension;
            this.apiConfiguration = apiConfiguration;
        }

        // GET: WorkItem
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Add work item
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AddWorkItem()
        {
            WorkItemModel workitemModel = new WorkItemModel();
            workitemModel.ProjectList = getProjectList();
            return PartialView("_AddEditWorkItem", workitemModel);
        }

        /// <summary>
        /// method to populate projects
        /// </summary>
        /// <returns></returns>
        private List<Projects> getProjectList()
        {
            List<Projects> projectList = new List<Projects>();
            projectList = apiExtension.InvokeServiceWithBasicAuth<List<Projects>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAllActiveProjects + "?isActive=1"),"GET", new ProjectModelRequest() { IsActive=1});
            return projectList;
        }

        /// <summary>
        /// method to bind workitem grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadWorkItemGrid([DataSourceRequest] DataSourceRequest Request)
        {
            List<WorkItemModel> WorkItemLst = new List<WorkItemModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            return Json(new DataSourceResult()
            {
                Data = WorkItemLst,
                Total = 0,
            });
        }

        /// <summary>
        /// method to save work item
        /// </summary>
        /// <param name="workitemModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveWorkItems(WorkItemModel workitemModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            if (ModelState.IsValid)
            {
                TempData["WorkItemMessage"] = "WorkItem Added Successfully";
                ModelState.Clear();
            }
            return Json(null);
        }

    }
}
