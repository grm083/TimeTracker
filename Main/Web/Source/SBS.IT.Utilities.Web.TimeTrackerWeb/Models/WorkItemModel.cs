using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    /// <summary>
    /// Model created to handle work items
    /// </summary>
    public class WorkItemModel
    {
        public int WorkItemId { get; set; }
        public int ProjectId { get; set; }
        [Display(Name = "Application Name")]
        public string ProjectName { get; set; }
        [Display(Name = "Project Name")]
        public string WorkItemName { get; set; }
        [Display(Name = "Description")]
        public string WorkItemDescription { get; set; }
        //[Display(Name = "Defect/Requirement No.")]
        //public string DefectNumber { get; set; }
        //[Display(Name = "Incident Number")]
        //public string IncidentNumber { get; set; }
        [Display(Name = "Planned Deployment Date")]
        public Nullable<DateTime> PlannedDeploymentDate { get; set; }
        [Display(Name = "Actual Deployment Date")]
        public Nullable<DateTime> ActualDeploymentDate { get; set; }
        [Display(Name = "Completion Comment")]
        public string CompletionComment { get; set; }
        //public Nullable<int> BusinessOwnerId { get; set; }
        [Display(Name = "Business Owner")]
        public string BusinessOwner { get; set; }
        public Nullable<double> Budget { get; set; }
        public Nullable<double> Benefit { get; set; }
        public List<Projects> ProjectList { get; set; }
    }

    public class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}