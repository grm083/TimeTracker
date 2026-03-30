using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class DashboardViewModel
    {
        public int ProjectItemCount { get; set; }
        public int UserCount { get; set; }
        public int ReportCount { get; set; }
        public bool IsLoggedInUserBirthday { get; set; }
        public string LoggedUserFirstName { get; set; }
        public List<EmployeeModel> BirthdayEmployees { get; set; }
        public int ProjectCount { get; set; }
    }
}