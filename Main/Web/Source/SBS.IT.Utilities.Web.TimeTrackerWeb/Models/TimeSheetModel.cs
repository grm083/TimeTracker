using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Models
{
    public class TimeSheetModel
    {
        public List<TimeEntry> TimeEntry { get; set; }
        public int? UserId { get; set; }
        public string LogonName { get; set; }
        public int? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string TeamCode { get; set; }
    }
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public int WorkTypeId { get; set; }
        public int? ApplicationId { get; set; }
        public int? ProjectItemId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime Date { get; set; }
        public double WorkHour { get; set; }
        public string Comments { get; set; }
        public string WeekDays { get; set; }
        public int UserId { get; set; }
        public string LogonName { get; set; }
        public int? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? DeleteUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string WorkItem { get; set; }
    }

    public class TimeEntrySearchModel
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string LogonName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmploymentType { get; set; }
        public string EmployeeLocation { get; set; }
        public string Designation { get; set; }
        public int TimeZoneId { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeCode { get; set; }
        [Display(Name = "Work Type")]
        public string WorkTypeName { get; set; }
        public int? ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationName { get; set; }
        public int? ProjectItemId { get; set; }
        public string ProjectItemName { get; set; }
        [Display(Name = "Date")]
        public Nullable<System.DateTime> Date { get; set; }
        public decimal WorkHour { get; set; }
        public string Comments { get; set; }
        public Nullable<int> RowTotal { get; set; }
        public List<EmployeeModel> Employeelst { get; set; }
        public List<WorkTypeModel> WorkTypelst { get; set; }
        public List<ProjectItemListModel> ProjectItemlst { get; set; }
        public List<ApplicationModel> Applicationlst { get; set; }
        [Display(Name = "Project Item")]
        public string ProjectItemNameDescription { get; set; }
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        public int? ProjectId { get; set; }
        [Display(Name = "ALM\\INC number")]
        public string WorkItem { get; set; }

    }

    public class TimeEntryWeeklyStatusModel
    {
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.DateTime> WeekStartDT { get; set; }
        public DateTime? WeekEndDT { get; set; }
        [Display(Name = "Week of Month")]
        public string WeekWithMonth { get; set; }
        [Display(Name = "Hour(s)")]
        public Nullable<decimal> WeeklyTotalHours { get; set; }
        [Display(Name = "Status")]
        public string WeeklyStatus { get; set; }
        public int? RowTotal { get; set; }
        [Display(Name = "Week")]
        public string WeeklyUpdate { get; set; }
    }

    public class TimeEntryDeleteModel
    {
        public string TimeEntryId { get; set; }
        public virtual Nullable<int> DeleteUserId { get; set; }
    }

}