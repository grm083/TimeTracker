using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/TimeEntry")]
    public class TimeEntryController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;
        public TimeEntryController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetTimeEntry")]
        public IHttpActionResult GetTimeEntry(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<System.DateTime> date)
        {
            IEnumerable<TimeEntryModel> _timeEntry = trackerDbRepository.GetTimeEntry(timeEntryId, employeeId, date);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetTimeEntry")]
        public IHttpActionResult GetTimeEntry(Nullable<int> employeeId, Nullable<System.DateTime> date)
        {
            IEnumerable<TimeEntryModel> _timeEntry = trackerDbRepository.GetTimeEntry(null, employeeId, date);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetTimeEntry")]
        public IHttpActionResult GetTimeEntry(Nullable<int> timeEntryId)
        {
            TimeEntryModel _timeEntry = trackerDbRepository.GetTimeEntry(timeEntryId, null, null).FirstOrDefault();

            if (_timeEntry == null)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetTimeEntryDistinctRecords")]
        public IHttpActionResult GetTimeEntryDistinctRecords(Nullable<int> employeeId, Nullable<System.DateTime> date)
        {
            IEnumerable<TimeEntryModel> _timeEntry = trackerDbRepository.GetTimeEntryDistinctRecords(employeeId, date);

            if (_timeEntry == null)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpPost]
        [Route("TimeEntryAdd")]
        public IHttpActionResult TimeEntryAdd(TimeEntryRequestModel request)
        {
            int?  _timeEntry = trackerDbRepository.TimeEntryAdd(request);

            if (!_timeEntry.HasValue)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("TimeEntrySearch")]
        public IHttpActionResult TimeEntrySearch(Nullable<int> timeEntryId, Nullable<int> employeeId, Nullable<int> workTypeId, Nullable<int> projectId, Nullable<int> projectItemId, string searchBy, Nullable<System.DateTime> timeEntryDateFrom, Nullable<System.DateTime> timeEntryDateTo, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<TimeEntrySearchModel> _timeEntry = trackerDbRepository.TimeEntrySearch(timeEntryId, employeeId, workTypeId, projectId, projectItemId, searchBy, timeEntryDateFrom, timeEntryDateTo, pageSize, pageNumber, sortOrder, sortColumn);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("TimeEntrySearch")]
        public IHttpActionResult TimeEntrySearch(Nullable<int> employeeId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<TimeEntrySearchModel> _timeEntry = trackerDbRepository.TimeEntrySearch(null, employeeId, null, null, null, searchBy, null, null, pageSize, pageNumber, sortOrder, sortColumn);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("TimeEntrySearch")]
        public IHttpActionResult TimeEntrySearch(Nullable<int> employeeId, string searchBy, Nullable<System.DateTime> timeEntryDateFrom, Nullable<System.DateTime> timeEntryDateTo, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<TimeEntrySearchModel> _timeEntry = trackerDbRepository.TimeEntrySearch(null, employeeId, null, null, null, searchBy, timeEntryDateFrom, timeEntryDateTo, pageSize, pageNumber, sortOrder, sortColumn);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("TimeEntrySearch")]
        public IHttpActionResult TimeEntrySearch(string searchBy)
        {
            IEnumerable<TimeEntrySearchModel> _timeEntry = trackerDbRepository.TimeEntrySearch(null, null, null, null, null, searchBy, null, null, null, null, null, null);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpPost]
        [Route("TimeEntryDelete")]
        public IHttpActionResult TimeEntryDelete(TimeEntryDeleteModel request)
        {
            int? _timeEntry = trackerDbRepository.TimeEntryDelete(request);

            if (!_timeEntry.HasValue)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpPost]
        [Route("TimeEntryUpdate")]
        public IHttpActionResult TimeEntryUpdate(TimeEntryRequestModel request)
        {
            int? _timeEntry = trackerDbRepository.TimeEntryUpdate(request);

            if (!_timeEntry.HasValue)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetTimeEntryWeeklyStatus")]
        public IHttpActionResult GetTimeEntryWeeklyStatus(Nullable<int> employeeId, Nullable<int> monthsBack, Nullable<System.DateTime> productionDate, Nullable<System.DateTime> searchDate, Nullable<int> pageSize, Nullable<int> pageNumber)
        {
            IEnumerable<TimeEntryWeeklyStatusModel> _timeEntry = trackerDbRepository.GetTimeEntryWeeklyStatus(employeeId, monthsBack, productionDate, searchDate, pageSize, pageNumber);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetTimeEntryWeeklyStatus")]
        public IHttpActionResult GetTimeEntryWeeklyStatus(Nullable<int> employeeId, Nullable<System.DateTime> productionDate, Nullable<int> pageSize, Nullable<int> pageNumber)
        {
            IEnumerable<TimeEntryWeeklyStatusModel> _timeEntry = trackerDbRepository.GetTimeEntryWeeklyStatus(employeeId, null, productionDate, null, pageSize, pageNumber);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetLastTimeEntry")]
        public IHttpActionResult GetLastTimeEntry(Nullable<int> employeeId, Nullable<System.DateTime> productionDate)
        {
            string _timeEntry = trackerDbRepository.GetLastTimeEntry(employeeId, productionDate);

            //if (!_timeEntry.HasValue)
            //{
            //    return NotFound();
            //}
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetAdminProjectView")]
        public IHttpActionResult GetAdminProjectView(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> searchDate)
        {
            IEnumerable<ProjectViewModel> _timeEntry = trackerDbRepository.GetAdminProjectView(projectId, monthsBack, null, searchDate, null, null);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }

        [HttpGet]
        [Route("GetAdminProjectViewExport")]
        public IHttpActionResult GetAdminProjectViewExport(Nullable<int> projectId, Nullable<int> monthsBack, Nullable<System.DateTime> searchDate)
        {
            IEnumerable<ProjectViewExportModel> _timeEntry = trackerDbRepository.GetAdminProjectViewExport(projectId, monthsBack, null, searchDate, null, null);

            if (_timeEntry.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeEntry);
        }
    }
}
