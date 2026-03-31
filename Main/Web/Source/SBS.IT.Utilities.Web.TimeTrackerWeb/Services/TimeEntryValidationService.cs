using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Services
{
    /// <summary>
    /// Validates time entry submissions before saving.
    /// Extracted from TimeSheetController.ValidateTimeEntries.
    /// </summary>
    public class TimeEntryValidationService
    {
        private readonly ILogger logger;

        public TimeEntryValidationService(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Validates a batch of time entries. Returns true if valid, false otherwise.
        /// On failure, responseData contains the user-facing error message.
        /// </summary>
        public bool Validate(TimeSheetModel model, List<WorkTypeModel> workTypelst, List<ProjectListModel> projectlst, List<ProjectItemListModel> projectItemlst, ref string responseData)
        {
            try
            {
                if (!ValidateDailyHourLimits(model, ref responseData))
                    return false;

                if (!ValidateNoDuplicateEntries(model, ref responseData))
                    return false;

                if (!ValidateRequiredFields(model, workTypelst, projectlst, ref responseData))
                    return false;

                if (!ValidateWorkItemRequired(model, projectItemlst, ref responseData))
                    return false;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(typeof(TimeEntryValidationService), LogLevel.ERROR, ex.Message, ex);
                return false;
            }
            return true;
        }

        private bool ValidateDailyHourLimits(TimeSheetModel model, ref string responseData)
        {
            var groupedResult = from obj in model.TimeEntry
                                where obj.WorkHour > 0
                                group obj by obj.Date into g
                                select new { date = g.Key, TimeEntries = g.ToList() };

            foreach (var dateEntry in groupedResult)
            {
                if (dateEntry?.TimeEntries != null && dateEntry.TimeEntries.Sum(x => x.WorkHour) >= 24)
                {
                    responseData = "Time entries for a day cannot exceed 24 hours";
                    return false;
                }
            }
            return true;
        }

        private bool ValidateNoDuplicateEntries(TimeSheetModel model, ref string responseData)
        {
            var duplicateKeys = from dp in model.TimeEntry
                                group dp by new
                                {
                                    dp.ProjectItemId,
                                    dp.ProjectId,
                                    dp.WorkTypeId,
                                    dp.WorkItem
                                } into gc
                                select new { timeEntries = gc.Key, rowcount = gc.Count() };

            foreach (var item in duplicateKeys)
            {
                if (item.rowcount > 7)
                {
                    responseData = "Please select unique Project , Project Item and Work Type";
                    return false;
                }
            }
            return true;
        }

        private bool ValidateRequiredFields(TimeSheetModel model, List<WorkTypeModel> workTypelst, List<ProjectListModel> projectlst, ref string responseData)
        {
            var distinctKeys = from dp in model.TimeEntry
                               group dp by new
                               {
                                   dp.ProjectItemId,
                                   dp.WorkTypeId,
                                   dp.ProjectId
                               } into gc
                               select new { timeEntries = gc.Key };

            if (distinctKeys == null || distinctKeys.Count() < 1)
                return true;

            var PSWorkType = workTypelst.Where(x => x.WorkTypeCode == "PST").FirstOrDefault();
            var PSProject = projectlst.Where(x => x.ProjectName.Contains("Production Support")).OrderBy(x => x.ProjectId).FirstOrDefault();

            foreach (var item in distinctKeys)
            {
                if (item.timeEntries.ProjectId == 0 || item.timeEntries.ProjectItemId == 0 || item.timeEntries.WorkTypeId == 0)
                {
                    responseData = "Please select Project or Project Item or Work Type";
                    return false;
                }

                if (PSWorkType != null && PSProject != null)
                {
                    if (item.timeEntries.WorkTypeId == PSWorkType.WorkTypeId && item.timeEntries.ProjectId != PSProject.ProjectId)
                    {
                        responseData = "Please select 'Production Support' workType for 'Production Support' Project";
                        return false;
                    }
                }

                if (model != null && !string.IsNullOrEmpty(model.TeamCode) && model.TeamCode.ToUpper() == "PS")
                {
                    string projectIds = Convert.ToString(ConfigurationManager.AppSettings["ProdSupprtProjectIds"]);
                    string projects = Convert.ToString(ConfigurationManager.AppSettings["ProdSupprtProjects"]);
                    if (!string.IsNullOrEmpty(projectIds))
                    {
                        List<int> prjIds = projectIds.Split(',').Select(int.Parse).ToList();
                        if (prjIds.IndexOf(item.timeEntries.ProjectId.GetValueOrDefault()) == -1)
                        {
                            responseData = "Production support team can only select : Production " + projects + " as project(s)";
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool ValidateWorkItemRequired(TimeSheetModel model, List<ProjectItemListModel> projectItemlst, ref string responseData)
        {
            var distinctKeysWithWorkItem = from dp in model.TimeEntry
                                           group dp by new
                                           {
                                               dp.ProjectItemId,
                                               dp.WorkTypeId,
                                               dp.ProjectId,
                                               dp.WorkItem
                                           } into gc
                                           select new { timeEntries = gc.Key };

            if (distinctKeysWithWorkItem == null || distinctKeysWithWorkItem.Count() < 1)
                return true;

            var projectitem = (projectItemlst != null && projectItemlst.Count > 0)
                ? projectItemlst.Where(x => x.ProjectItemName.ToLower().Contains("other")).FirstOrDefault()
                : null;

            foreach (var item in distinctKeysWithWorkItem)
            {
                if (projectitem != null && item.timeEntries.ProjectItemId == projectitem.ProjectItemId && string.IsNullOrEmpty(item.timeEntries.WorkItem))
                {
                    responseData = "Work Item required for 'Other' project item";
                    return false;
                }
            }
            return true;
        }
    }
}
