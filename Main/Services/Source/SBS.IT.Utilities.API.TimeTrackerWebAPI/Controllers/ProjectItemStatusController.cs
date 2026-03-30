using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Project")]
    public class ProjectItemStatusController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ProjectItemStatusController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetProjectItemStatus")]
        public IHttpActionResult GetProjectItemStatus(Nullable<int> isActive)
        {
            IEnumerable<ProjectItemStatusModel> _projectItemStatus = trackerDbRepository.GetProjectItemStatus(isActive);

            if (_projectItemStatus.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItemStatus);
        }

        [HttpGet]
        [Route("GetProjectItemStatus")]
        public IHttpActionResult GetProjectItemStatus()
        {
            IEnumerable<ProjectItemStatusModel> _projectItemStatus = trackerDbRepository.GetProjectItemStatus(null);

            if (_projectItemStatus.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItemStatus);
        }
    }
}
