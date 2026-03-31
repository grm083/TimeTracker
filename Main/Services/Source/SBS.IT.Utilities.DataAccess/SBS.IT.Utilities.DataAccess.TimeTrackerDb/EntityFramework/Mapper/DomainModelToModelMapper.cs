using AutoMapper;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Edmx;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using SBS.IT.Utilities.Shared.Model;

namespace SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework.Mapper
{
    class DomainModelToModelMapper : Profile
    {
        public override string ProfileName
        {
            get
            {
                return GetType().Name;
            }
        }

        protected override void Configure()
        {
            AutoMapper.Mapper.CreateMap<usp_Application_Get_Result, ApplicationModel>()
                                   .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                                   .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                                   .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.IsActive, o => o.Ignore());


            AutoMapper.Mapper.CreateMap<usp_WorkType_Get_Result, WorkTypeModel>()
                                   .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                                   .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                                   .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.IsActive, o => o.Ignore())
                                   .ForMember(d => d.WorkTypeCategoryId, o => o.Ignore())
                                   .ForMember(d => d.IsCapitalizable, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_WorkType_Search_Result, WorkTypeSearchModel>()
                       .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                       .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                       .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());


            AutoMapper.Mapper.CreateMap<usp_Project_Get_Result, ProjectModel>()
                        .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                        .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                        .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.IsActive, o => o.Ignore())
                        .ForMember(d => d.ProjectTypeId, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Project_Search_Result, ProjectSearchModel>()
                                   .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                                   .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                                   .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_ProjectItem_Get_Result, ProjectItemModel>()
                       .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                       .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                       .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.IsActive, o => o.Ignore())
                       .ForMember(d => d.Budget, o => o.Ignore()).ForMember(d => d.Benefit, o => o.Ignore()).ForMember(d => d.ProjectItemStatusId, o => o.Ignore())
                       .ForMember(d => d.DevLOE, o => o.Ignore()).ForMember(d => d.QaLOE, o => o.Ignore()).ForMember(d => d.BaLOE, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_ProjectItem_Search_Result, ProjectItemSearchModel>()
                                  .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                                  .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                                  .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_TimeEntry_Get_Result, TimeEntryModel>()
                      .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                      .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                      .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.TeamCode, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_TimeEntry_GetDistinctRecords_Result, TimeEntryModel>()
                  .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                  .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                  .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.TimeEntryId, o => o.Ignore())
                  .ForMember(d => d.EmployeeId, o => o.Ignore()).ForMember(d => d.Date, o => o.Ignore()).ForMember(d => d.WorkHour, o => o.Ignore())
                  .ForMember(d => d.Comments, o => o.Ignore()).ForMember(d => d.WeekDays, o => o.Ignore()).ForMember(d => d.TeamCode, o => o.Ignore());


            AutoMapper.Mapper.CreateMap<usp_Team_Get_Result, TeamModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());
            AutoMapper.Mapper.CreateMap<usp_WorkTypeCategory_Get_Result, WorkTypeCategoryModel>()
             .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
             .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
             .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_TimeZone_Get_Result, TimeZoneModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Employee_Authentication_Result, EmployeeAuthenticationModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_TimeEntry_Search_Result, TimeEntrySearchModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Application_Search_Result, ApplicationSearchModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Employee_GetManager_Result, EmployeeManagerModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Employee_Search_Result, EmployeeSearchModel>()
              .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
              .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
              .ForMember(d => d.UserId, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_EmploymentType_Get_Result, EmploymentTypeModel>()
                .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_Location_Get_Result, LocationModel>()
                .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_UserType_Get_Result, UserTypeModel>()
                .ForMember(d => d.CreateUserId, o => o.Ignore()).ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore())
                .ForMember(d => d.UpdateDate, o => o.Ignore()).ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore())
                .ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore());

            AutoMapper.Mapper.CreateMap<usp_TimeEntry_GetWeeklyStatus_Result, TimeEntryWeeklyStatusModel>();
            AutoMapper.Mapper.CreateMap<usp_ReportRegistry_Detail_Result, ReportRegistryDetailModel>();
            AutoMapper.Mapper.CreateMap<usp_Employee_GetList_Result, EmployeeListModel>();
            AutoMapper.Mapper.CreateMap<usp_ProjectType_Get_Result, ProjectTypeModel>();
            AutoMapper.Mapper.CreateMap<usp_ProjectItemStatus_Get_Result, ProjectItemStatusModel>();
            AutoMapper.Mapper.CreateMap<usp_TimeEntry_GetProjectView_Result, ProjectViewModel>().ForMember(d => d.UserId, o => o.Ignore()).ForMember(d => d.LogonName, o => o.Ignore()).ForMember(d => d.CreateUserId, o => o.Ignore())
                .ForMember(d => d.CreateDate, o => o.Ignore()).ForMember(d => d.UpdateUserId, o => o.Ignore()).ForMember(d => d.UpdateDate, o => o.Ignore())
                .ForMember(d => d.DeleteUserId, o => o.Ignore()).ForMember(d => d.DeleteDate, o => o.Ignore());
            AutoMapper.Mapper.CreateMap<usp_TimeEntry_GetProjectViewForExport_Result, ProjectViewExportModel>();
        }
    }
}
