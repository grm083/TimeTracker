using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ReportModel
    {
        public List<KeyValuePair<string, string>> ListReports { get; set; }

        public bool IsReportVisible { get; set; }
    }

    public class ReportRegistryModel
    {
        public int ReportRegistryId { get; set; }
        public string DisplayName { get; set; }
        public string ReportPath { get; set; }
        public string ReportFolder { get; set; }
        public int ReportServerId { get; set; }
        public Nullable<int> IsUserIdRequired { get; set; }
        public string Notes { get; set; }
        public string ReportCategory { get; set; }
        public string SourceDescription { get; set; }
        public byte[] SourceImage { get; set; }
        public Nullable<int> IsDraft { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string ToolTip { get; set; }
        public string Guid_PK { get; set; }
        public Nullable<int> IsClientFriendly { get; set; }
        public string ReportServerCode { get; set; }
        public string ReportServerName { get; set; }
        public string ReportServerURL { get; set; }
    }

    public class ReportViewModel
    {
        public List<ReportRegistryModel> ReportList { get; set; }
        public List<string> ReportHeader { get; set; }
        public bool IsReportVisible { get; set; }
    }
}