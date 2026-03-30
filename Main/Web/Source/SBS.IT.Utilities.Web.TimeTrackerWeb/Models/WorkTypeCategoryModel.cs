using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class WorkTypeCategoryModel
    {
        public int WorkTypeCategoryId { get; set; }
        public string WorkTypeCategoryCode { get; set; }
        public string WorkTypeCategoryName { get; set; }
        public string WorkTypeCategoryDescription { get; set; }
    }
}