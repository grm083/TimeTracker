using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/TimeZone")]
    public class TimeZoneController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;
        public TimeZoneController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetTimeZone")]
        public IHttpActionResult GetTimeZone(Nullable<int> isActive)
        {
            IEnumerable<TimeZoneModel> _timeZone = trackerDbRepository.GetTimeZone(isActive);

            if (_timeZone.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeZone);
        }

        [HttpGet]
        [Route("GetTimeZone")]
        public IHttpActionResult GetTimeZone()
        {
            IEnumerable<TimeZoneModel> _timeZone = trackerDbRepository.GetTimeZone(null);

            if (_timeZone.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_timeZone);
        }

    }
}
