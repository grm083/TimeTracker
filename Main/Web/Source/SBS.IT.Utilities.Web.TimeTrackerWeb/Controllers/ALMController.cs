using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [Authorize]
    public class ALMController : Controller
    {
        // GET: ALM
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// method to bind ALM grid
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public virtual JsonResult ReadALMGrid([DataSourceRequest] DataSourceRequest Request)
        {
            List<ALMModel> ALMModelLst = new List<ALMModel>();
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            return Json(new DataSourceResult()
            {
                Data = ALMModelLst,
                Total = 0,
            });
        }

        public virtual ActionResult AddALM()
        {
            ALMModel almModel = new ALMModel();
            almModel.ProjectList = getProjectList();
            almModel.WorkitemList = getWorkItemList();
            return PartialView("_AddEditALM", almModel);
        }

        /// <summary>
        /// Get project list
        /// </summary>
        /// <returns></returns>
        private List<Projects> getProjectList()
        {
            return new List<Projects>();
        }

        /// <summary>
        /// get work item list by project id
        /// </summary>
        /// <returns></returns>
        private List<WorkItemModel> getWorkItemList()
        {
            return new List<WorkItemModel>();
        }

        /// <summary>
        /// method to save ALM
        /// </summary>
        /// <param name="almModel"></param>
        /// <returns></returns>
        public virtual JsonResult SaveALM(ALMModel almModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }
            if (ModelState.IsValid)
            {
                TempData["ALMMessage"] = "Requirement/ALM Added Successfully";
                ModelState.Clear();
            }
            return Json(null);
        }
    }
}