using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.IT.Utilities.Shared.APIClient.Message
{
    class CommonTypeMessages
    {
    }
    public static class APIResources
    {
        public const string GetTeam = "api/Team/GetTeam";
        public const string GetLocation = "api/Location/GetLocation";
        public const string GetTimeZone = "api/TimeZone/GetTimeZone";
        public const string GetUserType = "api/UserType/GetUserType";
        public const string GetEmploymentType = "api/EmploymentType/GetEmploymentType";
        public const string GetAllWorkTypeCategory = "api/WorkTpe/GetWorkTypeCategory";
        public const string GetReportDetail = "api/Report/GetReportDetail";

        #region application
        public const string ApplicationAdd = "api/Application/ApplicationAdd";
        public const string ApplicationUpdate = "api/Application/ApplicationUpdate";
        public const string ApplicationDelete = "api/Application/ApplicationDelete";
        public const string GetByApplicationId = "api/Application/GetByApplicationId";

        public const string GetAllActiveProjects = "api/Project/GetProject";
        public const string GetApplicationSearch = "api/Application/ApplicationSearch";
        public const string GetAllApplication = "api/Application/GetApplication";
        public const string GetProjectItemByApplicationId = "api/Project/GetProjectItemByApplicationId";
        #endregion

        #region Project
        public const string ProjectAdd = "api/Project/ProjectAdd";
        public const string ProjectUpdate = "api/Project/ProjectUpdate";
        public const string ProjectDelete = "api/Project/ProjectDelete";

        public const string GetByProjectId = "api/Project/GetByProjectId";
        public const string GetProject = "api/Project/GetProject";
        public const string ProjectSearch = "api/Project/ProjectSearch";

        #endregion

        #region WorkType
        public const string WorkTypeSearch = "api/WorkTpe/WorkTypeSearch";
        public const string WorkTypeAdd = "api/WorkTpe/WorkTypeAdd";
        public const string WorkTypeUpdate = "api/WorkTpe/WorkTypeUpdate";
        public const string WorkTypeDelete = "api/WorkTpe/WorkTypeDelete";
        public const string GetByWorkTypeId = "api/WorkTpe/GetByWorkTypeId";
        public const string GetAllWorkType = "api/WorkTpe/GetWorkType";
        #endregion

        #region ProjectItem
        public const string ProjectItemAdd = "api/Project/ProjectItemAdd";
        public const string ProjectItemUpdate = "api/Project/ProjectItemUpdate";
        public const string ProjectItemDelete = "api/Project/ProjectItemDelete";

        public const string GetByProjectItemId = "api/Project/GetByProjectItemId";
        public const string GetProjectItem = "api/Project/GetProjectItem";
        public const string ProjectItemSearch = "api/Project/ProjectItemSearch";
        public const string GetActiveProjectItem = "api/Project/GetProjectItem";
        public const string ProjectItemListGetBySearch = "api/Project/ProjectItemListGetBySearch";
        #endregion

        #region Employee
        public const string EmployeeSearch = "api/Employee/EmployeeSearch";
        public const string EmployeeAdd = "api/Employee/EmployeeAdd";
        public const string EmployeeUpdate = "api/Employee/EmployeeUpdate";
        public const string EmployeeDelete = "api/Employee/EmployeeDelete";

        public const string GetByEmployeeId = "api/Employee/GetByEmployeeId";
        public const string GetManager = "api/Employee/GetManager";
        public const string GetEmployeeListByManagerId = "api/Employee/GetEmployeeListByManagerId";
        public const string EmployeeBirthday = "api/Employee/GetEmployeesBirthday";
        public const string CheckLogonName = "api/Employee/CheckLogonName";
        public const string GetAuthentication = "api/Employee/GetAuthentication";
        #endregion

        #region ProjectType
        public const string GetProjectType = "api/Project/GetProjectType";
        #endregion

        #region ProjectItemStatus
        public const string GetProjectItemStatus = "api/Project/GetProjectItemStatus";
        #endregion

        #region TimeEntry
        public const string SaveTimeSheet = "api/TimeEntry/TimeEntryAdd";
        public const string UpdateTimeEntry = "api/TimeEntry/TimeEntryUpdate";
        public const string DeleteTimeEntry = "api/TimeEntry/TimeEntryDelete";

        public const string GetTimeEntrySearch = "api/TimeEntry/TimeEntrySearch";
        public const string GetTimeEntry = "api/TimeEntry/GetTimeEntry";

        public const string GetTimeEntryWeeklyStatus = "api/TimeEntry/GetTimeEntryWeeklyStatus";
        public const string GetTimeEntryLastEntry = "api/TimeEntry/GetLastTimeEntry";
        public const string GetTimeEntryDistinctRecords = "api/TimeEntry/GetTimeEntryDistinctRecords";
        public const string GetAdminProjectView = "api/TimeEntry/GetAdminProjectView";
        public const string GetAdminProjectViewExport = "api/TimeEntry/GetAdminProjectViewExport";
        #endregion

    }
}
