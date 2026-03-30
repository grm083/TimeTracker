using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class MonthlyViewModel
    {
        [Display(Name = "Search")]
        public string Search { get; set; }
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }
        [Required(ErrorMessage = "Manager Name is required")]
        public List<Manager> ManagersList { get; set; }
        public int? ManagerId { get; set; }
        [Display(Name = "Select Month")]
        public string Month { get; set; }
        [Display(Name = "Select Year")]
        public string Year { get; set; }
        public List<EmployeeDetails> EmployeeList { get; set; }
        public List<Month> MonthList { get; set; }
        public int? MonthId { get; set; }
        public List<Year> YearList { get; set; }
        public int? YearId { get; set; }
    }

    public class Month
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
    }
    public class Year
    {
        public int YearId { get; set; }
        public string YearName { get; set; }
    }
    public class EmployeeDetails
    {
        [Display(Name = "Employee")]
        public string EmpolyeeName { get; set; }
        [Display(Name = "Total")]
        public string Total { get; set; }
        [Display(Name = "FirstDateColumn")]
        public string FirstDateColumn { get; set; }
        [Display(Name = "SecondDateColumn")]
        public string SecondDateColumn { get; set; }
        [Display(Name = "ThirdDateColumn")]
        public string ThirdDateColumn { get; set; }
        [Display(Name = "FourthDateColumn")]
        public string FourthDateColumn { get; set; }
        [Display(Name = "FifthDateColumn")]
        public string FifthDateColumn { get; set; }
        [Display(Name = "SixthDateColumn")]
        public string SixthDateColumn { get; set; }
        public int? RowTotal { get; set; }

        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }
    }


}