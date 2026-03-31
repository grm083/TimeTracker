using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Services
{
    /// <summary>
    /// Maps team codes to their default work type IDs.
    /// </summary>
    public static class TeamWorkTypeHelper
    {
        private static readonly Dictionary<string, string> TeamToWorkTypeCode = new Dictionary<string, string>
        {
            { "DEV", "DEV" },
            { "QA", "TES" },
            { "PS", "PST" },
            { "RPT", "RPT" },
            { "BA", "PMT" },
            { "MGT", "ADN" }
        };

        /// <summary>
        /// Returns the default work type ID for the given team code, or 0 if no mapping exists.
        /// </summary>
        public static int GetDefaultWorkTypeId(string teamCode, List<WorkTypeModel> workTypes)
        {
            if (string.IsNullOrEmpty(teamCode) || workTypes == null)
                return 0;

            string workTypeCode;
            if (!TeamToWorkTypeCode.TryGetValue(teamCode, out workTypeCode))
                return 0;

            var workType = workTypes.FirstOrDefault(x => x.WorkTypeCode == workTypeCode);
            return workType != null ? workType.WorkTypeId : 0;
        }
    }
}
