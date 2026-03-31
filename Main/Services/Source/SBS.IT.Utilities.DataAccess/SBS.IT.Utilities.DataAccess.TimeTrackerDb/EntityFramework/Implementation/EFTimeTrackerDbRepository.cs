using System;
using System.Collections.Generic;
using System.Linq;
using SBS.IT.Utilities.Shared.UtilityExtension;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices.AccountManagement;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using SBS.IT.Utilities.Shared.Model;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Edmx;
using System.Transactions;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework.Implementation
{
    public class EFTimeTrackerDbRepository : ITrackerDbRepository
    {
        private readonly ILogger logger = new Log4NetLogger();

        #region Application Method
        public IEnumerable<ApplicationModel> GetApplication(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Application_Get_Result>, IEnumerable<ApplicationModel>>(context.usp_Application_Get(isActive));
            }
        }
        public IEnumerable<ApplicationSearchModel> ApplicationSearch(Nullable<int> applicationId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Application_Search_Result>, IEnumerable<ApplicationSearchModel>>(context.usp_Application_Search(applicationId, searchBy, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }
        public Nullable<int> ApplicationAdd(ApplicationModel application)
        {
            Nullable<int> applicationId = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                var applicationAddedId = new ObjectParameter("applicationId", typeof(Int32));
                context.usp_Application_Add(application.ApplicationCode, application.ApplicationName, application.Description, application.CreateUserId, applicationAddedId);
                applicationId = Convert.IsDBNull(applicationAddedId.Value) ? 0 : (int)applicationAddedId.Value;
                context.SaveChanges();
            }
            return applicationId;
        }

        public Nullable<int> ApplicationUpdate(ApplicationModel application)
        {
            Nullable<int> isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_Application_Update(application.ApplicationId, application.ApplicationCode, application.ApplicationName, application.Description, application.IsActive, application.UpdateUserId);
                context.SaveChanges();
            }
            return isUpdate;
        }

        public Nullable<int> ApplicationDelete(Nullable<int> applicationId, Nullable<int> deleteUserId)
        {
            Nullable<int> isDelete = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDelete = context.usp_Application_Delete(applicationId, deleteUserId);
                context.SaveChanges();
            }
            return isDelete;
        }
        #endregion

        #region WorkType Method
        public IEnumerable<WorkTypeModel> GetWorkType(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_WorkType_Get_Result>, IEnumerable<WorkTypeModel>>(context.usp_WorkType_Get(isActive));
            }
        }

        public IEnumerable<WorkTypeSearchModel> WorkTypeSearch(Nullable<int> workTypeId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_WorkType_Search_Result>, IEnumerable<WorkTypeSearchModel>>(context.usp_WorkType_Search(workTypeId, searchBy, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }

        public Nullable<int> WorkTypeAdd(WorkTypeModel workType)
        {
            Nullable<int> workTypeId = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                var workTypeAddedId = new ObjectParameter("workTypeId", typeof(Int32));
                context.usp_WorkType_Add(workType.WorkTypeCode, workType.WorkTypeName, workType.Description, workType.WorkTypeCategoryId, workType.IsCapitalizable, workType.UserId, workTypeAddedId);
                workTypeId = Convert.IsDBNull(workTypeAddedId.Value) ? 0 : (int)workTypeAddedId.Value;
                context.SaveChanges();
            }
            return workTypeId;
        }

        public Nullable<int> WorkTypeUpdate(WorkTypeModel workType)
        {
            Nullable<int> isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_WorkType_Update(workType.WorkTypeCode, workType.WorkTypeName, workType.Description, workType.WorkTypeCategoryId, workType.IsCapitalizable, workType.IsActive, workType.UserId, workType.WorkTypeId);
                context.SaveChanges();
            }
            return isUpdate;
        }
        public Nullable<int> WorkTypeDelete(Nullable<int> workTypeId, Nullable<int> deleteUserId)
        {
            Nullable<int> isDelete = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDelete = context.usp_WorkType_Delete(deleteUserId, workTypeId);
                context.SaveChanges();
            }
            return isDelete;
        }
        #endregion

        #region Project Method
        public IEnumerable<ProjectModel> GetProject(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Project_Get_Result>, IEnumerable<ProjectModel>>(context.usp_Project_Get(isActive));
            }
        }

        public IEnumerable<ProjectSearchModel> ProjectSearch(Nullable<int> projectId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Project_Search_Result>, IEnumerable<ProjectSearchModel>>(context.usp_Project_Search(projectId, searchBy, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }
        public Nullable<int> ProjectAdd(ProjectModel project)
        {
            Nullable<int> projectId = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                var projectAddedId = new ObjectParameter("projectId", typeof(Int32));
                context.usp_Project_Add(project.ProjectName, project.ProjectDescription, project.ProjectTypeId, project.UserId, projectAddedId);
                projectId = Convert.IsDBNull(projectAddedId.Value) ? 0 : (int)projectAddedId.Value;
                context.SaveChanges();
            }
            return projectId;
        }
        public Nullable<int> ProjectDelete(Nullable<int> projectId, Nullable<int> deleteUserId)
        {
            int? isDeleted = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDeleted = context.usp_Project_Delete(projectId, deleteUserId);
                context.SaveChanges();
            }
            return isDeleted;
        }
        public Nullable<int> ProjectUpdate(ProjectModel project)
        {
            int? isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_Project_Update(project.ProjectName, project.ProjectDescription, project.ProjectTypeId, project.IsActive, project.UserId, project.ProjectId);
                context.SaveChanges();
            }
            return isUpdate;
        }
        #endregion

        #region Project Item Method
        public IEnumerable<ProjectItemModel> GetProjectItem(Nullable<int> isActive, Nullable<int> projectId)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_ProjectItem_Get_Result>, IEnumerable<ProjectItemModel>>(context.usp_ProjectItem_Get(isActive, projectId));
            }
        }

        public IEnumerable<ProjectItemSearchModel> ProjectItemSearch(Nullable<int> projectItemId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_ProjectItem_Search_Result>, IEnumerable<ProjectItemSearchModel>>(context.usp_ProjectItem_Search(projectItemId, searchBy, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }

        public Nullable<int> ProjectItemAdd(ProjectItemModel projectItem)
        {
            Nullable<int> projectItemId = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                var projectItemAddedKey = new ObjectParameter("projectItemId", typeof(Int32));
                context.usp_ProjectItem_Add(projectItem.ProjectItemName, projectItem.ProjectItemDescription, projectItem.ProjectId, projectItem.ProjectItemStatusId, projectItem.Budget, projectItem.Benefit, projectItem.DevLOE, projectItem.QaLOE, projectItem.BaLOE, projectItem.UserId, projectItemAddedKey);
                projectItemId = Convert.IsDBNull(projectItemAddedKey) ? 0 : (int)projectItemAddedKey.Value;
                context.SaveChanges();
            }
            return projectItemId;
        }

        public Nullable<int> ProjectItemUpdate(ProjectItemModel projectItem)
        {
            int? isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_ProjectItem_Update(projectItem.ProjectItemName, projectItem.ProjectItemDescription, projectItem.ProjectId, projectItem.ProjectItemStatusId, projectItem.Budget, projectItem.Benefit, projectItem.DevLOE, projectItem.QaLOE, projectItem.BaLOE, projectItem.IsActive, projectItem.UserId, projectItem.ProjectItemId);
                context.SaveChanges();
            }
            return isUpdate;
        }
        public Nullable<int> ProjectItemDelete(Nullable<int> projectItemId, Nullable<int> deleteUserId)
        {
            int? isDeleted = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDeleted = context.usp_ProjectItem_Delete(projectItemId, deleteUserId);
                context.SaveChanges();
            }
            return isDeleted;
        }
        #endregion

        #region TimeEntry Method
        public Nullable<int> TimeEntryAdd(TimeEntryRequestModel request)
        {
            Nullable<int> recordCount = 0;
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (request != null && request.TimeEntry != null)
                    {
                        using (var context = new EFContextFactory().TimeTrackerDBContext)
                        {

                            Nullable<int> timeEntryId = null;
                            recordCount = 0;
                            foreach (TimeEntryModel timeEntry in request.TimeEntry)
                            {
                                if (timeEntry.TimeEntryId == 0)
                                {
                                    timeEntryId = null;
                                    var timeEntryAddedId = new ObjectParameter("timeEntryId", typeof(Int32));
                                    context.usp_TimeEntry_Add(timeEntry.EmployeeId, timeEntry.WorkTypeId, timeEntry.ProjectId, timeEntry.ProjectItemId, timeEntry.TeamCode, timeEntry.Date, timeEntry.WorkHour, timeEntry.Comments, timeEntry.WorkItem, timeEntry.UserId, timeEntryAddedId);
                                    timeEntryId = Convert.IsDBNull(timeEntryAddedId) ? 0 : (int)timeEntryAddedId.Value;
                                }
                                else
                                {
                                    timeEntryId = context.usp_TimeEntry_Update(timeEntry.TimeEntryId, timeEntry.EmployeeId, timeEntry.WorkTypeId, timeEntry.ProjectId, timeEntry.ProjectItemId, timeEntry.Date, timeEntry.WorkHour, timeEntry.Comments, timeEntry.WorkItem, timeEntry.UserId);
                                }
                                if (timeEntryId.HasValue && timeEntryId > 0)
                                    recordCount++;
                            }
                            context.SaveChanges();
                        }
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    logger.WriteMessage(this.GetType(), LogLevel.ERROR, "TimeEntryAdd failed", ex);
                    throw;
                }
            }
            return recordCount;
        }
        public Nullable<int> TimeEntryDelete(TimeEntryDeleteModel request)
        {
            int? isDeleted = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDeleted = context.usp_TimeEntry_Delete(request.TimeEntryId, request.DeleteUserId);
                context.SaveChanges();
            }
            return isDeleted;
        }
        public Nullable<int> TimeEntryUpdate(TimeEntryRequestModel request)
        {
            Nullable<int> isUpdate = 0;
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (request != null && request.TimeEntry != null)
                    {
                        using (var context = new EFContextFactory().TimeTrackerDBContext)
                        {

                            int recordCount = 0;
                            foreach (TimeEntryModel timeEntry in request.TimeEntry)
                            {

                                recordCount = context.usp_TimeEntry_Update(timeEntry.TimeEntryId, timeEntry.EmployeeId, timeEntry.WorkTypeId, timeEntry.ProjectId, timeEntry.ProjectItemId, timeEntry.Date, timeEntry.WorkHour, timeEntry.Comments, timeEntry.WorkItem, timeEntry.UserId);
                                if (recordCount > 0)
                                    isUpdate++;
                            }
                            context.SaveChanges();
                        }
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    logger.WriteMessage(this.GetType(), LogLevel.ERROR, "TimeEntryUpdate failed", ex);
                    throw;
                }
            }
            return isUpdate;
        }

        public IEnumerable<TimeEntryModel> GetTimeEntry(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<System.DateTime> date)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_Get_Result>, IEnumerable<TimeEntryModel>>(context.usp_TimeEntry_Get(timeEntryId, employeeId, date));
            }
        }
        public IEnumerable<TimeEntryModel> GetTimeEntryDistinctRecords(Nullable<int> employeeId, Nullable<System.DateTime> date)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_GetDistinctRecords_Result>, IEnumerable<TimeEntryModel>>(context.usp_TimeEntry_GetDistinctRecords(employeeId, date));
            }
        }
        public IEnumerable<TimeEntrySearchModel> TimeEntrySearch(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<int> workTypeId, Nullable<int> projectId, Nullable<int> projectItemId, string searchBy, Nullable<System.DateTime> timeEntryDateFrom, Nullable<System.DateTime> timeEntryDateTo, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_Search_Result>, IEnumerable<TimeEntrySearchModel>>(context.usp_TimeEntry_Search(timeEntryId, employeeId, workTypeId, projectId, projectItemId, searchBy, timeEntryDateFrom, timeEntryDateTo, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }
        public IEnumerable<TimeEntryWeeklyStatusModel> GetTimeEntryWeeklyStatus(Nullable<int> employeeId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_GetWeeklyStatus_Result>, IEnumerable<TimeEntryWeeklyStatusModel>>(context.usp_TimeEntry_GetWeeklyStatus(employeeId, monthsBack, productionDate, searchDate, pageSize, pageNumber));
            }
        }
        public string GetLastTimeEntry(Nullable<int> employeeId, Nullable<System.DateTime> productionDate)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return context.usp_TimeEntry_GetLastEntry(employeeId, productionDate).FirstOrDefault();
            }
        }
        #endregion

        #region Employee Method
        public IEnumerable<EmployeeAuthenticationModel> GetAuthentication(EmployeeAuthenticationRequestModel request)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                bool valid = ADAuthentication(request.LogonName, request.LogonPassword, request.DomainName, request.IsProduction);
                if (valid)
                {
                    return AutoMapper.Mapper.Map<IEnumerable<usp_Employee_Authentication_Result>, IEnumerable<EmployeeAuthenticationModel>>(context.usp_Employee_Authentication(request.LogonName, null));
                }
                return null;
            }
        }
        public bool ADAuthentication(string logonName, string logonPassword, string domainName, bool isProduction)
        {
            bool isValid = false;
            // create a "principal context" - e.g. your domain (could be machine, too)
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName))
            {
                var identity = UserPrincipal.FindByIdentity(pc, IdentityType.Name, logonName);

                if (isProduction == true && identity != null && identity.Name == logonName)
                {
                    // validate the credentials
                    isValid = pc.ValidateCredentials(logonName, logonPassword, ContextOptions.Negotiate);
                }
                else if (isProduction == false && identity != null && identity.Name == logonName)
                {
                    logger.WriteMessage(this.GetType(), LogLevel.WARN, string.Format("Non-production auth bypass: user '{0}' authenticated without password validation", logonName));
                    isValid = true;
                }
            }
            return isValid;
        }
        public bool CheckLogonName(string logonName, string domain)
        {
            bool isValid = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                var identity = UserPrincipal.FindByIdentity(pc, IdentityType.Name, logonName);

                if (identity != null && identity.Name == logonName)
                {
                    isValid = true;
                }
            }
            return isValid;
        }
        public Nullable<int> EmployeeAdd(EmployeeModel employee)
        {
            Nullable<int> projectId = null;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, employee.Domain))
            {
                var identity = UserPrincipal.FindByIdentity(pc, IdentityType.Name, employee.LogonName);

                if (identity != null && identity.Name == employee.LogonName)
                {
                    // validate the credentials
                    employee.EmailAddress = identity.EmailAddress;
                }
            }
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                var employeeAddedId = new ObjectParameter("employeeId", typeof(Int32));
                context.usp_Employee_Add(employee.LogonName, employee.FirstName, employee.LastName, employee.EmailAddress, employee.DOJ, employee.Designation, employee.TimeZoneId, employee.TeamId, employee.ManagerId, employee.LocationId, employee.EmploymentTypeId, employee.CompanyName, employee.UserTypeId, employee.IsTimeEntryEnable, employee.UserId, employeeAddedId,employee.DOT);
                projectId = Convert.IsDBNull(employeeAddedId.Value) ? 0 : (int)employeeAddedId.Value;
                context.SaveChanges();
            }
            return projectId;
        }

        public Nullable<int> EmployeeUpdate(EmployeeModel employee)
        {
            Nullable<int> isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_Employee_Update(employee.EmployeeId, employee.FirstName, employee.LastName, employee.DOJ, employee.Designation, employee.TimeZoneId, employee.TeamId, employee.ManagerId, employee.LocationId, employee.EmploymentTypeId, employee.CompanyName, employee.UserTypeId, employee.IsTimeEntryEnable, employee.IsActive, employee.UserId,employee.DOT);
                context.SaveChanges();
            }
            return isUpdate;
        }
        public Nullable<int> EmployeeDelete(Nullable<int> employeeId, Nullable<int> deleteUserId)
        {
            Nullable<int> isDelete = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isDelete = context.usp_Employee_Delete(employeeId, deleteUserId);
                context.SaveChanges();
            }
            return isDelete;
        }

        public Nullable<int> EmployeeUpdatePassword(Nullable<int> employeeId, string logonName, string logonPassword, Nullable<int> updateUserId)
        {
            Nullable<int> isUpdate = null;
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                isUpdate = context.usp_Employee_UpdatePassword(employeeId, logonName, logonPassword, updateUserId);
                context.SaveChanges();
            }
            return isUpdate;
        }
        public IEnumerable<EmployeeSearchModel> EmployeeSearch(Nullable<int> employeeId, Nullable<int> managerId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Employee_Search_Result>, IEnumerable<EmployeeSearchModel>>(context.usp_Employee_Search(employeeId, managerId, searchBy, pageSize, pageNumber, sortOrder, sortColumn));
            }
        }

        public IEnumerable<EmployeeManagerModel> GetManager()
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Employee_GetManager_Result>, IEnumerable<EmployeeManagerModel>>(context.usp_Employee_GetManager());
            }
        }
        public IEnumerable<EmployeeListModel> GetEmployeeList(Nullable<int> managerId)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Employee_GetList_Result>, IEnumerable<EmployeeListModel>>(context.usp_Employee_GetList(managerId));
            }
        }
        #endregion
        public IEnumerable<TeamModel> GetTeam(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Team_Get_Result>, IEnumerable<TeamModel>>(context.usp_Team_Get(isActive));
            }
        }
        public IEnumerable<ProjectTypeModel> GetProjectType(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_ProjectType_Get_Result>, IEnumerable<ProjectTypeModel>>(context.usp_ProjectType_Get(isActive));
            }
        }
        public IEnumerable<ProjectItemStatusModel> GetProjectItemStatus(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_ProjectItemStatus_Get_Result>, IEnumerable<ProjectItemStatusModel>>(context.usp_ProjectItemStatus_Get(isActive));
            }
        }
        public IEnumerable<TimeZoneModel> GetTimeZone(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeZone_Get_Result>, IEnumerable<TimeZoneModel>>(context.usp_TimeZone_Get(isActive));
            }
        }
        public IEnumerable<ReportRegistryDetailModel> GetReportRegistryDetail(Nullable<int> userTypeId)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_ReportRegistry_Detail_Result>, IEnumerable<ReportRegistryDetailModel>>(context.usp_ReportRegistry_Detail(userTypeId));
            }
        }
        public IEnumerable<EmploymentTypeModel> GetEmploymentType(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_EmploymentType_Get_Result>, IEnumerable<EmploymentTypeModel>>(context.usp_EmploymentType_Get(isActive));
            }
        }
        public IEnumerable<LocationModel> GetLocation(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_Location_Get_Result>, IEnumerable<LocationModel>>(context.usp_Location_Get(isActive));
            }
        }
        public IEnumerable<UserTypeModel> GetUserType(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_UserType_Get_Result>, IEnumerable<UserTypeModel>>(context.usp_UserType_Get(isActive));
            }
        }
        public IEnumerable<WorkTypeCategoryModel> GetWorkTypeCategory(Nullable<int> isActive)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_WorkTypeCategory_Get_Result>, IEnumerable<WorkTypeCategoryModel>>(context.usp_WorkTypeCategory_Get(isActive));
            }
        }
        public IEnumerable<ProjectViewModel> GetAdminProjectView(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_GetProjectView_Result>, IEnumerable<ProjectViewModel>>(context.usp_TimeEntry_GetProjectView(projectId, monthsBack, productionDate, searchDate, pageSize, pageNumber));
            }
        }

        public IEnumerable<ProjectViewExportModel> GetAdminProjectViewExport(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber)
        {
            using (var context = new EFContextFactory().TimeTrackerDBContext)
            {
                return AutoMapper.Mapper.Map<IEnumerable<usp_TimeEntry_GetProjectViewForExport_Result>, IEnumerable<ProjectViewExportModel>>(context.usp_TimeEntry_GetProjectViewForExport(projectId, monthsBack, productionDate, searchDate, pageSize, pageNumber));
            }
        }
    }
}
