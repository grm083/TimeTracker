using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/WorkTpe")]
    public class WorkTypeCategoryController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public WorkTypeCategoryController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }
        [HttpGet]
        [Route("GetWorkTypeCategory")]
        public IHttpActionResult GetWorkTypeCategory(Nullable<int> isActive)
        {
            IEnumerable<WorkTypeCategoryModel> _workTypeCategory = trackerDbRepository.GetWorkTypeCategory(isActive);

            if (_workTypeCategory.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workTypeCategory);
        }

        [HttpGet]
        [Route("GetWorkTypeCategory")]
        public IHttpActionResult GetWorkTypeCategory()
        {
            IEnumerable<WorkTypeCategoryModel> _workTypeCategory = trackerDbRepository.GetWorkTypeCategory(null);

            if (_workTypeCategory.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_workTypeCategory);
        }
    }
}
