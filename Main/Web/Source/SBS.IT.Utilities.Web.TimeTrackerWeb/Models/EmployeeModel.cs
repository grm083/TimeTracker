using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "User Name is Required")]
        [Display(Name = "User Name")]
        public string LogonName { get; set; }
        [Display(Name = "Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Team is Required")]
        public int? TeamId { get; set; }
        public string Team { get; set; }
        public string Gender { get; set; }
        
        [Display(Name = "Password")]
        public string LogonPassword { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Emergency Contact")]
        public string EmergencyContactNumber { get; set; }
        [Display(Name = "Date of Birth")]
        public Nullable<DateTime> DOB { get; set; }

        [Required(ErrorMessage = "Location Time Zone is Required")]
        [Display(Name = "Location Time Zone")]
        public int TimeZoneId { get; set; }
        public string TimeZone { get; set; }
        public string TimeZoneCode { get; set; }
        [Required(ErrorMessage = "Enable Time is Required")]
        [Display(Name = "Enable Time Entry")]
        public int? IsTimeEntryEnable { get; set; }
        [Required(ErrorMessage = "Time Entry Date is Required")]
        [Display(Name = "Time Entry Date")]
        public Nullable<DateTime> DOJ { get; set; }
        public string Designation { get; set; }        
        public string TeamCode { get; set; }
        public int? ManagerId { get; set; }
        public string Manager { get; set; }
        [Display(Name = "Location")]
        public string EmployeeLocation { get; set; }
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; }
        [Display(Name = "Active")]
        public int IsActive { get; set; }       

        [Display(Name = "Time Tracker Role")]
        public string UserType { get; set; }

        public int? CompanyId { get; set; }
        [Display(Name = "Company")]
        public string CompanyName { get; set; }
        public List<Team> TeamList { get; set; }
        public List<Manager> ManagerList { get; set; }
        public List<EmployeeTimeZone> TimeZonelst{ get; set; }
        public int? RowTotal { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        [Display(Name = "Manager")]
        public string EmployeeManager { get; set; }
        [Display(Name = "Team")]
        public string TeamName { get; set; }
        public List<EmploymentType> EmploymentTypeList { get; set; }
        [Required(ErrorMessage = "Employment Type is Required")]
        public int? EmploymentTypeId { get; set; }
        public List<Location> LocationList { get; set; }
        [Required(ErrorMessage = "Location is Required")]
        public int? LocationId { get; set; }
        public List<UserType> UserTypeList { get; set; }
        [Required(ErrorMessage = "UserType is Required")]
        public int? UserTypeId { get; set; }
        public int? TeamWorkType { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Date Of Termination")]
        public Nullable<DateTime> DOT { get; set; }
    }

    public class EmployeeViewModel
    {
        public int employeeId { get; set; }
        public string logonName { get; set; }

        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Current Password Required")]
        public string currentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New Password Required")]
        public string newPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password Required")]
        [Compare("newPassword", ErrorMessage = "Confirm password doesn't match with New Password")]
        public string confirmNewPassword { get; set; }
        public int? updateUserId { get; set; }
    }

    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamCode { get; set; }
        public string Description { get; set; }
    }
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string UserTypeCode { get; set; }
        public string UserTypeDescription { get; set; }
    }
    public class Location
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
    }
    public class EmploymentType
    {
        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeCode { get; set; }
        public string EmploymentTypeName { get; set; }
        public string Description { get; set; }
    }

    public class Manager
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
    }

    public class EmployeeTimeZone
    {
        public int TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneCode { get; set; }
        public string Description { get; set; }
        public decimal UtcOffset { get; set; }
    }

}