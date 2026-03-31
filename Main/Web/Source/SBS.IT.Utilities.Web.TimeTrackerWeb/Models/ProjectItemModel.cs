using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBS.IT.Utilities.Shared.Model;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    /// <summary>
    /// Model created to handle work items
    /// </summary>
    public class ProjectItemModel : BaseModel
    {
        //[Required(ErrorMessage = "Project Name is required")]
        public int? ProjectId { get; set; }
        [Display(Name = "Project")]
        public int? ProjectItemId { get; set; }
        [Display(Name = "Project Item")]
        [Required(ErrorMessage = "Project Item is required")]
        public string ProjectItemName { get; set; }
        [Display(Name = "Description")]
        public string ProjectItemDescription { get; set; }
        [Display(Name = "Budget($)")]
        public Nullable<decimal> Budget { get; set; }
        [Display(Name = "Benefit($)")]
        public Nullable<decimal> Benefit { get; set; }
        [Display(Name = "DEV LOE(In Hrs)")]
        public Nullable<decimal> DevLOE { get; set; }
        [Display(Name = "QA LOE(In Hrs)")]
        public Nullable<decimal> QaLOE { get; set; }
        [Display(Name = "BA LOE(In Hrs)")]
        public Nullable<decimal> BaLOE { get; set; }
        [Display(Name = "Project Name")]    
        public string ProjectName { get; set; }
        public int? RowTotal { get; set; }
        public int IsActive { get; set; }
        [Required(ErrorMessage = "Project Item Status is required")]
        public int? ProjectItemStatusId { get; set; }
        public string ProjectItemStatusCode { get; set; }
        [Display(Name = "Project Item Status")]
        public string ProjectItemStatusName { get; set; }
        public List<ProjectListModel> ProjectList { get; set; }
        public List<ProjectItemStatusModel> ProjectItemStatusList { get; set; }
        [UIHint("_ProjectItemStatus")]
        public ProjectItemStatusModel ProjectItemStatus
        {
            get;
            set;
                      
        }


    }

    public class ProjectItemListModel
    {
        public int ProjectItemId { get; set; }
        public string ProjectItemName { get; set; }
        public string ProjectItemDescription { get; set; }
        public string ProjectItemNameDescription { get; set; }
        public Nullable<int> ProjectId { get; set; }
    }

    public class Projects
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}