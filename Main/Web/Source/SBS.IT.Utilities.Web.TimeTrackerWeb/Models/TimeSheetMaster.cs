using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class TimeSheetMaster
    {
        [Key]
        public int TimeSheetMasterID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? TotalHours { get; set; }
        public int UserID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int TimeSheetStatus { get; set; }
        public string Comment { get; set; }
    }
}