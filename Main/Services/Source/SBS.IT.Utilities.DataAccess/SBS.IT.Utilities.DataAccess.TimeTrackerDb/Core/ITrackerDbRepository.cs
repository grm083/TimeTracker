using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core
{
    public interface ITrackerDbRepository
    {
        IEnumerable<ApplicationModel> GetApplication(Nullable<int> isActive);
        Nullable<int> ApplicationAdd(ApplicationModel application);
        Nullable<int> ApplicationUpdate(ApplicationModel application);
        Nullable<int> ApplicationDelete(Nullable<int> applicationId, Nullable<int> deleteUserId);
        IEnumerable<ApplicationSearchModel> ApplicationSearch(Nullable<int> applicationId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        IEnumerable<WorkTypeModel> GetWorkType(Nullable<int> isActive);
        Nullable<int> WorkTypeAdd(WorkTypeModel workType);
        Nullable<int> WorkTypeUpdate(WorkTypeModel workType);
        Nullable<int> WorkTypeDelete(Nullable<int> workTypeId, Nullable<int> deleteUserId);
        IEnumerable<WorkTypeSearchModel> WorkTypeSearch(Nullable<int> workTypeId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        Nullable<int> ProjectAdd(ProjectModel project);
        Nullable<int> ProjectUpdate(ProjectModel project);
        Nullable<int> ProjectDelete(Nullable<int> projectId, Nullable<int> deleteUserId);
        IEnumerable<ProjectModel> GetProject(Nullable<int> isActive);
        IEnumerable<ProjectSearchModel> ProjectSearch(Nullable<int> projectId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        IEnumerable<ProjectItemModel> GetProjectItem(Nullable<int> isActive, Nullable<int> projectId);
        IEnumerable<ProjectItemSearchModel> ProjectItemSearch(Nullable<int> projectItemId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        Nullable<int> ProjectItemAdd(ProjectItemModel projectItem);
        Nullable<int> ProjectItemUpdate(ProjectItemModel projectItem);
        Nullable<int> ProjectItemDelete(Nullable<int> projectItemId, Nullable<int> deleteUserId);
        IEnumerable<TimeEntryModel> GetTimeEntry(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<System.DateTime> date);
        IEnumerable<TimeEntryModel> GetTimeEntryDistinctRecords(Nullable<int> employeeId, Nullable<System.DateTime> date);
        IEnumerable<TimeEntrySearchModel> TimeEntrySearch(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<int> workTypeId, Nullable<int> projectId, Nullable<int> projectItemId, string searchBy, Nullable<System.DateTime> timeEntryDateFrom, Nullable<System.DateTime> timeEntryDateTo, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        Nullable<int> TimeEntryAdd(TimeEntryRequestModel request);
        Nullable<int> TimeEntryDelete(TimeEntryDeleteModel request);
        Nullable<int> TimeEntryUpdate(TimeEntryRequestModel request);
        IEnumerable<TimeEntryWeeklyStatusModel> GetTimeEntryWeeklyStatus(Nullable<int> employeeId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber);
        string GetLastTimeEntry(Nullable<int> employeeId, Nullable<System.DateTime> productionDate);
        IEnumerable<TeamModel> GetTeam(Nullable<int> isActive);
        IEnumerable<TimeZoneModel> GetTimeZone(Nullable<int> isActive);
        Nullable<int> EmployeeAdd(EmployeeModel employee);
        Nullable<int> EmployeeUpdate(EmployeeModel employee);
        Nullable<int> EmployeeDelete(Nullable<int> employeeId, Nullable<int> deleteUserId);
        Nullable<int> EmployeeUpdatePassword(Nullable<int> employeeId, string logonName, string logonPassword, Nullable<int> updateUserId);
        IEnumerable<EmployeeManagerModel> GetManager();
        IEnumerable<EmployeeSearchModel> EmployeeSearch(Nullable<int> employeeId, Nullable<int> managerId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn);
        IEnumerable<EmployeeAuthenticationModel> GetAuthentication(EmployeeAuthenticationRequestModel request);
        bool CheckLogonName(string logonName, string domain);
        IEnumerable<EmployeeListModel> GetEmployeeList(Nullable<int> managerId);
        IEnumerable<ReportRegistryDetailModel> GetReportRegistryDetail(Nullable<int> userTypeId);
        IEnumerable<ProjectTypeModel> GetProjectType(Nullable<int> isActive);
        IEnumerable<ProjectItemStatusModel> GetProjectItemStatus(Nullable<int> isActive);
        IEnumerable<EmploymentTypeModel> GetEmploymentType(Nullable<int> isActive);
        IEnumerable<LocationModel> GetLocation(Nullable<int> isActive);
        IEnumerable<UserTypeModel> GetUserType(Nullable<int> isActive);
        IEnumerable<WorkTypeCategoryModel> GetWorkTypeCategory(Nullable<int> isActive);
        IEnumerable<ProjectViewModel> GetAdminProjectView(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber);
        IEnumerable<ProjectViewExportModel> GetAdminProjectViewExport(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber);
    }
}
