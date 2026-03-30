using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class ApplicationModel
    {
        [Display(Name = "Code")]
        [Required(ErrorMessage ="Code is Required ")]
        public string ApplicationCode { get; set; }
        public int ApplicationId { get; set; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Application Name is required")]
        public string ApplicationName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public int IsActive { get; set; }
        public int? RowTotal { get; set; }
    }
}