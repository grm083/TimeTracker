using System;
using System.ComponentModel.DataAnnotations;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username Required")]
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Required")]
        public string Password { get; set; }
    }
    public class EmployeeAuthenticationModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string EmployeeLocation { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<int> EmploymentTypeId { get; set; }
        public string EmploymentType { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public string UserType { get; set; }
        public string UserName { get; set; }
        public Nullable<int> IsTimeEntryEnable { get; set; }
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public int IsEmployeeBirthDay { get; set; }
    }
    public class EmployeeAuthenticationRequestModel
    {
        public string LogonName { get; set; }
        public string LogonPassword { get; set; }
        public string DomainName { get; set; }
        public bool IsProduction { get; set; }
    }
}