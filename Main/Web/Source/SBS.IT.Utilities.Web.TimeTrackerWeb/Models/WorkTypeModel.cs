using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class WorkTypeModel : BaseModel
    {
        public int WorkTypeId { get; set; }
        [Display(Name = "Code")]
        [Required(ErrorMessage = "Code is required")]
        public string WorkTypeCode { get; set; }

        [Display(Name = "Work Type")]
        [Required(ErrorMessage = "Work Type is required")]
        public string WorkTypeName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Is Capitalizable")]        
        public int? IsCapitalizable { get; set; }
        [Display(Name = "Active")]
        public int IsActive { get; set; }
        public List<WorkTypeCategoryModel> WorkTypeCategoryModellst { get; set; }
        [Required(ErrorMessage = "Work Type Category is required")]
        public int? WorkTypeCategoryId { get; set; }
        [Display(Name = "Work Type Category")]
        public string WorkTypeCategoryName { get; set; }
        public int? RowTotal { get; set; }
    }
    public class WorkTypeViewModel
    {
        public int WorkTypeId { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkTypeName { get; set; }
        public string Description { get; set; }
        public Nullable<int> IsCapitalizable { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> WorkTypeCategoryId { get; set; }
        public string WorkTypeCategoryCode { get; set; }
        public string WorkTypeCategoryName { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
}