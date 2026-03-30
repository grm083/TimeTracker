using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/EmploymentType")]
    public class EmploymentTypeController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public EmploymentTypeController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetEmploymentType")]
        public IHttpActionResult GetEmploymentType(Nullable<int> isActive)
        {
            IEnumerable<EmploymentTypeModel> _employmentType = trackerDbRepository.GetEmploymentType(isActive);

            if (_employmentType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_employmentType);
        }

        [HttpGet]
        [Route("GetEmploymentType")]
        public IHttpActionResult GetEmploymentType()
        {
            IEnumerable<EmploymentTypeModel> _employmentType = trackerDbRepository.GetEmploymentType(null);

            if (_employmentType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_employmentType);
        }
    }
}
