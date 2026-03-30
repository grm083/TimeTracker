using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ProjectTypeModel
    {
        public int ProjectTypeId { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectTypeName { get; set; }
        public string Description { get; set; }
    }
}