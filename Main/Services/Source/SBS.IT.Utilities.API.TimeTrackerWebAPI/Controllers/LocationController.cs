using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Location")]
    public class LocationController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public LocationController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetLocation")]
        public IHttpActionResult GetLocation(Nullable<int> isActive)
        {
            IEnumerable<LocationModel> _location = trackerDbRepository.GetLocation(isActive);

            if (_location.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_location);
        }

        [HttpGet]
        [Route("GetLocation")]
        public IHttpActionResult GetLocation()
        {
            IEnumerable<LocationModel> _location = trackerDbRepository.GetLocation(null);

            if (_location.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_location);
        }
    }
}
