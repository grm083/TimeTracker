using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ALMModel
    {
        public int ALMId { get; set; }
        public int WorkItemId { get; set; }
        [Display(Name = "Project Item Name")]
        public string WorkItemName { get; set; }
        public int ProjectId { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Defect/Requirement No.")]
        public string ALM { get; set; }
        [Display(Name = "ALM Description")]
        public string ALMDescription { get; set; }
        public Nullable<double> Budget { get; set; }
        public Nullable<double> Benefit { get; set; }
        public List<Projects> ProjectList { get; set; }
        public List<WorkItemModel> WorkitemList { get; set; }
    }
}