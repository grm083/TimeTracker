using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Team")]
    public class TeamController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public TeamController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetTeam")]
        public IHttpActionResult GetTeam(Nullable<int> isActive)
        {
            IEnumerable<TeamModel> _team = trackerDbRepository.GetTeam(isActive);

            if (_team.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_team);
        }

        [HttpGet]
        [Route("GetTeam")]
        public IHttpActionResult GetTeam()
        {
            IEnumerable<TeamModel> _team = trackerDbRepository.GetTeam(null);

            if (_team.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_team);
        }
    }
}
