using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ProjectItemStatusModel
    {
        public int ProjectItemStatusId { get; set; }
        public string ProjectItemStatusCode { get; set; }
        public string ProjectItemStatusName { get; set; }
        public string ProjectItemStatusDescription { get; set; }
    }
}