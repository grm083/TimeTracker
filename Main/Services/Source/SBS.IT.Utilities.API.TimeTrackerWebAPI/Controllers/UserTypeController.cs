using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/UserType")]
    public class UserTypeController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public UserTypeController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetUserType")]
        public IHttpActionResult GetUserType(Nullable<int> isActive)
        {
            IEnumerable<UserTypeModel> _userType = trackerDbRepository.GetUserType(isActive);

            if (_userType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_userType);
        }

        [HttpGet]
        [Route("GetUserType")]
        public IHttpActionResult GetUserType()
        {
            IEnumerable<UserTypeModel> _userType = trackerDbRepository.GetUserType(null);

            if (_userType.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_userType);
        }
    }
}
