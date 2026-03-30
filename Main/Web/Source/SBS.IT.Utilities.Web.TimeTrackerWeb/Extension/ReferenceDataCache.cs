using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Collections.Generic;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Extension
{
    /// <summary>
    /// Caches reference/lookup data in MemoryCache to avoid re-fetching on every page load.
    /// Uses a 5-minute absolute expiration.
    /// </summary>
    public static class ReferenceDataCache
    {
        private static readonly MemoryDataCacheManager cache = new MemoryDataCacheManager();
        private const int CacheTTLMinutes = 5;

        private static T GetOrFetch<T>(string key, Func<T> fetch) where T : class
        {
            var cached = cache.Get(key) as T;
            if (cached != null)
                return cached;

            var data = fetch();
            if (data != null)
                cache.Set(key, data, CacheTTLMinutes);

            return data;
        }

        public static List<WorkTypeModel> GetWorkTypes(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_WorkTypes",
                () => api.InvokeGet<List<WorkTypeModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetAllWorkType)));
        }

        public static List<ProjectListModel> GetProjects(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_Projects",
                () => api.InvokeGet<List<ProjectListModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetProject)));
        }

        public static List<Team> GetTeams(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_Teams",
                () => api.InvokeGet<List<Team>>(new Uri(config.ServiceBaseAddress + APIResources.GetTeam)));
        }

        public static List<Manager> GetManagers(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_Managers",
                () => api.InvokeGet<List<Manager>>(new Uri(config.ServiceBaseAddress + APIResources.GetManager)));
        }

        public static List<EmployeeTimeZone> GetTimeZones(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_TimeZones",
                () => api.InvokeGet<List<EmployeeTimeZone>>(new Uri(config.ServiceBaseAddress + APIResources.GetTimeZone)));
        }

        public static List<Location> GetLocations(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_Locations",
                () => api.InvokeGet<List<Location>>(new Uri(config.ServiceBaseAddress + APIResources.GetLocation)));
        }

        public static List<UserType> GetUserTypes(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_UserTypes",
                () => api.InvokeGet<List<UserType>>(new Uri(config.ServiceBaseAddress + APIResources.GetUserType)));
        }

        public static List<EmploymentType> GetEmploymentTypes(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_EmploymentTypes",
                () => api.InvokeGet<List<EmploymentType>>(new Uri(config.ServiceBaseAddress + APIResources.GetEmploymentType)));
        }

        public static List<ProjectTypeModel> GetProjectTypes(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_ProjectTypes",
                () => api.InvokeGet<List<ProjectTypeModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetProjectType)));
        }

        public static List<WorkTypeCategoryModel> GetWorkTypeCategories(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_WorkTypeCategories",
                () => api.InvokeGet<List<WorkTypeCategoryModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetAllWorkTypeCategory)));
        }

        public static List<ProjectItemStatusModel> GetProjectItemStatuses(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_ProjectItemStatuses",
                () => api.InvokeGet<List<ProjectItemStatusModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetProjectItemStatus)));
        }

        public static List<ApplicationModel> GetApplications(IAPIExtension api, IAPIConfiguration config)
        {
            return GetOrFetch("ref_Applications",
                () => api.InvokeGet<List<ApplicationModel>>(new Uri(config.ServiceBaseAddress + APIResources.GetAllApplication)));
        }
    }
}
