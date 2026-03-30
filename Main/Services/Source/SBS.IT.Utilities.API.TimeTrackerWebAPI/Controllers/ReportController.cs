using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Report")]
    public class ReportController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ReportController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetReportDetail")]
        public IHttpActionResult GetReportDetail(Nullable<int> userTypeId)
        {
            IEnumerable<ReportRegistryDetailModel> _report = trackerDbRepository.GetReportRegistryDetail(userTypeId);

            if (_report.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_report);
        }
    }
}
