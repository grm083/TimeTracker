using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/WorkTpe")]
    public class WorkTypeController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public WorkTypeController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }        
        [HttpGet]
        [Route("GetWorkType")]
        public IHttpActionResult GetWorkType(Nullable<int> isActive)
        {
            IEnumerable<WorkTypeModel> _workType = trackerDbRepository.GetWorkType(isActive);

            if (_workType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpGet]
        [Route("GetWorkType")]
        public IHttpActionResult GetWorkType()
        {
            IEnumerable<WorkTypeModel> _workType = trackerDbRepository.GetWorkType(null);

            if (_workType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpGet]
        [Route("WorkTypeSearch")]
        public IHttpActionResult WorkTypeSearch(Nullable<int> workTypeId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<WorkTypeSearchModel> _workType = trackerDbRepository.WorkTypeSearch(workTypeId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_workType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpGet]
        [Route("WorkTypeSearch")]
        public IHttpActionResult WorkTypeSearch(string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<WorkTypeSearchModel> _workType = trackerDbRepository.WorkTypeSearch(null, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_workType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpGet]
        [Route("GetByWorkTypeId")]
        public IHttpActionResult GetByWorkTypeId(Nullable<int> workTypeId)
        {
            WorkTypeSearchModel _workType = trackerDbRepository.WorkTypeSearch(workTypeId, null, null, null, null, null).FirstOrDefault();

            if (_workType == null)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpPost]
        [Route("WorkTypeAdd")]
        public IHttpActionResult WorkTypeAdd(WorkTypeModel workType)
        {
            int? _workType = trackerDbRepository.WorkTypeAdd(workType);

            if (!_workType.HasValue)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpPost]
        [Route("WorkTypeUpdate")]
        public IHttpActionResult WorkTypeUpdate(WorkTypeModel workType)
        {
            int? _workType = trackerDbRepository.WorkTypeUpdate(workType);

            if (!_workType.HasValue)
            {
                return NotFound();
            }
            return Ok(_workType);
        }

        [HttpGet]
        [Route("WorkTypeDelete")]
        public IHttpActionResult WorkTypeDelete(Nullable<int> workTypeId, Nullable<int> deleteUserId)
        {
            int? _workType = trackerDbRepository.WorkTypeDelete(workTypeId, deleteUserId);

            if (!_workType.HasValue)
            {
                return NotFound();
            }
            return Ok(_workType);
        }
    }
}
