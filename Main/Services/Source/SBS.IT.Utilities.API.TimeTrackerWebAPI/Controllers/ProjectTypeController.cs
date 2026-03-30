using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Project")]
    public class ProjectTypeController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ProjectTypeController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetProjectType")]
        public IHttpActionResult GetProjectType(Nullable<int> isActive)
        {
            IEnumerable<ProjectTypeModel> _projectType = trackerDbRepository.GetProjectType(isActive);

            if (_projectType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectType);
        }

        [HttpGet]
        [Route("GetProjectType")]
        public IHttpActionResult GetProjectType()
        {
            IEnumerable<ProjectTypeModel> _projectType = trackerDbRepository.GetProjectType(null);

            if (_projectType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectType);
        }
    }
}
