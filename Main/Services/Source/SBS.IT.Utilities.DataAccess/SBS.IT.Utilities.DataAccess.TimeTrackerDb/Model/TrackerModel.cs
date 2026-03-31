using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.IT.Utilities.Shared.Model;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model
{

    public class WorkTypeModel : BaseModel
    {
        public int WorkTypeId { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkTypeName { get; set; }
        public string Description { get; set; }
        public Nullable<int> WorkTypeCategoryId { get; set; }
        public Nullable<int> IsCapitalizable { get; set; }
        public int IsActive { get; set; }
    }
    public class WorkTypeSearchModel : BaseModel
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
    public class TimeEntryModel : BaseModel
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public int WorkTypeId { get; set; }
        public int ProjectItemId { get; set; }
        public int ProjectId { get; set; }
        public System.DateTime Date { get; set; }
        public decimal WorkHour { get; set; }
        public string Comments { get; set; }
        public string WorkItem { get; set; }
        public string WeekDays { get; set; }
        public string TeamCode { get; set; }
    }
    public class TimeEntrySearchModel : BaseModel
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string EmailAddress { get; set; }
        public string EmploymentType { get; set; }
        public string EmployeeLocation { get; set; }
        public string Designation { get; set; }
        public Nullable<int> TimeZoneId { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeCode { get; set; }
        public string WorkTypeName { get; set; }
        public Nullable<int> ProjectItemId { get; set; }
        public string ProjectItemName { get; set; }
        public string ProjectItemDescription { get; set; }
        public string ProjectItemNameDescription { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public decimal WorkHour { get; set; }
        public string Comments { get; set; }
        public string WorkItem { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
    public class TimeEntryWeeklyStatusModel
    {
        public Nullable<int> EmployeeId { get; set; }
        public Nullable<System.DateTime> WeekStartDT { get; set; }
        public DateTime? WeekEndDT { get; set; }
        public string WeeklyUpdate { get; set; }
        public string WeekWithMonth { get; set; }
        public Nullable<decimal> WeeklyTotalHours { get; set; }
        public string WeeklyStatus { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
    public class TimeEntryRequestModel
    {
        public List<TimeEntryModel> TimeEntry { get; set; }
    }
    public class TimeEntryDeleteModel : BaseModel
    {
        public string TimeEntryId { get; set; }
    }
    public class ApplicationModel : BaseModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
    }
    public class ApplicationSearchModel : BaseModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationCode { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
    public class ProjectModel : BaseModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public Nullable<int> ProjectTypeId { get; set; }
        public int IsActive { get; set; }
    }
    public class ProjectSearchModel : BaseModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public Nullable<int> ProjectTypeId { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectTypeName { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
    public class ProjectItemModel : BaseModel
    {
        public int ProjectItemId { get; set; }
        public string ProjectItemName { get; set; }
        public string ProjectItemDescription { get; set; }
        public string ProjectItemNameDescription { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<decimal> Benefit { get; set; }
        public Nullable<decimal> DevLOE { get; set; }
        public Nullable<decimal> QaLOE { get; set; }
        public Nullable<decimal> BaLOE { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> ProjectItemStatusId { get; set; }
        public int IsActive { get; set; }
    }
    public class ProjectItemSearchModel : BaseModel
    {
        public int ProjectItemId { get; set; }
        public string ProjectItemName { get; set; }
        public string ProjectItemDescription { get; set; }
        public decimal? Budget { get; set; }
        public Nullable<decimal> Benefit { get; set; }
        public Nullable<decimal> DevLOE { get; set; }
        public Nullable<decimal> QaLOE { get; set; }
        public Nullable<decimal> BaLOE { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Nullable<int> ProjectItemStatusId { get; set; }
        public string ProjectItemStatusCode { get; set; }
        public string ProjectItemStatusName { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> RowTotal { get; set; }
    }
    public class TeamModel : BaseModel
    {
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
    }
    public class TimeZoneModel : BaseModel
    {
        public int TimeZoneId { get; set; }
        public string TimeZoneCode { get; set; }
        public string TimeZoneName { get; set; }
        public string UtcOffset { get; set; }
        public string Description { get; set; }
    }
    public class EmployeeModel : BaseModel
    {
        public int EmployeeId { get; set; }
        public Nullable<int> EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string LogonPassword { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContactNumber { get; set; }
        public System.DateTime DOB { get; set; }
        public System.DateTime DOJ { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string EmployeeLocation { get; set; }
        public string EmploymentType { get; set; }
        public Nullable<int> EmploymentTypeId { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public string UserType { get; set; }
        public Nullable<int> IsTimeEntryEnable { get; set; }
        public int TimeZoneId { get; set; }
        public int TeamId { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public int IsActive { get; set; }
        public string Domain { get; set; }
        public Nullable<System.DateTime> DOT { get; set; }
    }
    public class EmployeeSearchModel : BaseModel
    {
        public int EmployeeId { get; set; }
        public Nullable<int> EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContactNumber { get; set; }
        public System.DateTime DOB { get; set; }
        public System.DateTime DOJ { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string LocationCode { get; set; }
        public string EmployeeLocation { get; set; }
        public Nullable<int> EmploymentTypeId { get; set; }
        public string EmploymentTypeCode { get; set; }
        public string EmploymentType { get; set; }
        public string CompanyName { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public string UserTypeCode { get; set; }
        public string UserType { get; set; }
        public Nullable<int> IsTimeEntryEnable { get; set; }
        public int TimeZoneId { get; set; }
        public string TimeZoneCode { get; set; }
        public string TimeZoneName { get; set; }
        public string UtcOffset { get; set; }
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public Nullable<int> ManagerId { get; set; }
        public string EmployeeManager { get; set; }
        public int IsActive { get; set; }
        public Nullable<int> RowTotal { get; set; }
        public Nullable<System.DateTime> DOT { get; set; }
    }
    public class EmployeeManagerModel : BaseModel
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
    public class EmployeeListModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
    public class EmployeeAuthenticationModel : BaseModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<System.DateTime> DOJ { get; set; }
        public string Designation { get; set; }
        public Nullable<int> LocationId { get; set; }
        public string EmployeeLocation { get; set; }
        public Nullable<int> EmploymentTypeId { get; set; }
        public string EmploymentType { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public string UserType { get; set; }
        public Nullable<int> IsTimeEntryEnable { get; set; }
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public Nullable<int> TimeZoneId { get; set; }
        public string TimeZoneCode { get; set; }
        public string UtcOffset { get; set; }
    }
    public class EmployeeAuthenticationRequestModel
    {
        public string LogonName { get; set; }
        public string LogonPassword { get; set; }
        public string DomainName { get; set; }
        public bool IsProduction { get; set; }
    }
    public class EmploymentTypeModel : BaseModel
    {
        public int EmploymentTypeId { get; set; }
        public string EmploymentTypeCode { get; set; }
        public string EmploymentTypeName { get; set; }
        public string Description { get; set; }
    }
    public class ReportRegistryDetailModel
    {
        public int ReportRegistryId { get; set; }
        public string DisplayName { get; set; }
        public string ReportPath { get; set; }
        public string ReportFolder { get; set; }
        public int ReportServerId { get; set; }
        public Nullable<int> IsUserIdRequired { get; set; }
        public string Notes { get; set; }
        public string ReportCategory { get; set; }
        public string SourceDescription { get; set; }
        public byte[] SourceImage { get; set; }
        public Nullable<int> IsDraft { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string ToolTip { get; set; }
        public string Guid_PK { get; set; }
        public Nullable<int> IsClientFriendly { get; set; }
        public string ReportServerCode { get; set; }
        public string ReportServerName { get; set; }
        public string ReportServerURL { get; set; }
        public int? userTypeId { get; set; }
        public string UserTypeCode { get; set; }
        public string UserTypeName { get; set; }
    }
    public class UserTypeModel : BaseModel
    {
        public int UserTypeId { get; set; }
        public string UserTypeCode { get; set; }
        public string UserTypeName { get; set; }
        public string Description { get; set; }
    }
    public class LocationModel : BaseModel
    {
        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
    }

    public class ProjectViewModel : BaseModel
    {
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public Nullable<decimal> WorkHour { get; set; }
    }

    public class ProjectViewExportModel
    {
        public Nullable<int> ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int WorkTypeId { get; set; }
        public string WorkTypeName { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public Nullable<decimal> WorkHour { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
