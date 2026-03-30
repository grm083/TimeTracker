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
using Microsoft.Reporting.WebForms;
using System.Web.UI.WebControls;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    [SessionTimeout]
    public class ReportController : Controller
    {
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;

        public ReportController()
        {
            apiExtension = new APIExtension();
            apiConfiguration = new APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
        }

        [HttpGet]
        // GET: Report
        public ActionResult Index()
        {
            Session["ReportParameter"] = "";
            Session["ServerUrl"] = "";

            ReportViewModel model = new ReportViewModel();
            var reportList = GetReportDetail();
            var menuHdrList = reportList.Select(x => x.ReportCategory).Distinct().ToList();
            model.ReportHeader = menuHdrList;
            model.ReportList = reportList;
            model.IsReportVisible = false;
            return View(model);
        }


        public ActionResult OpenReport(string reportFolder, string reportPath, string reportServer)
        {
            ReportViewModel model = new ReportViewModel();
            var reportList = GetReportDetail();
            var menuHdrList = reportList.Select(x => x.ReportCategory).Distinct().ToList();
            model.ReportHeader = menuHdrList;
            model.ReportList = reportList;
            model.IsReportVisible = true;
            string path = reportFolder + reportPath;
            Session["ReportParameter"] = path; // set your dynamic URL here
            Session["ServerUrl"] = reportServer;

            //Render report
            Microsoft.Reporting.WebForms.ReportViewer reportViewer = new Microsoft.Reporting.WebForms.ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;
            reportViewer.ServerReport.ReportServerUrl = new Uri(reportServer);
            reportViewer.ServerReport.ReportPath = string.Concat(path);
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(80);
            reportViewer.InteractivityPostBackMode = InteractivityPostBackMode.SynchronousOnDrillthrough;
            ViewBag.ReportViewer = reportViewer;

            return View("Index", model);
        }

        /// <summary>
        /// method to populate reports
        /// </summary>
        /// <returns></returns>
        private List<ReportRegistryModel> GetReportDetail()
        {
            List<ReportRegistryModel> reportList = apiExtension.InvokeGet<List<ReportRegistryModel>>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetReportDetail + "?userTypeId=" + AuthenticateExtension.UserTypeId));
            return reportList;
        }
    }
}