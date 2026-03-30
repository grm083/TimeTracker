using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.EntityFramework.Implementation;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Application")]
    public class ApplicationController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ApplicationController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetApplication")]
        public IHttpActionResult GetApplication(Nullable<int> isActive)
        {
            IEnumerable<ApplicationModel> _application = trackerDbRepository.GetApplication(isActive);

            if (_application.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_application);

        }

        [HttpGet]
        [Route("GetApplication")]
        public IHttpActionResult GetApplication()
        {
            IEnumerable<ApplicationModel> _application = trackerDbRepository.GetApplication(null);

            if (_application.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_application);

        }

        [HttpGet]
        [Route("ApplicationSearch")]
        public IHttpActionResult ApplicationSearch(Nullable<int> applicationId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ApplicationSearchModel> _application = trackerDbRepository.ApplicationSearch(applicationId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_application.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_application);

        }

        [HttpGet]
        [Route("GetByApplicationId")]
        public IHttpActionResult GetByApplicationId(Nullable<int> applicationId)
        {
            ApplicationSearchModel _application = trackerDbRepository.ApplicationSearch(applicationId, string.Empty, null, null, null, null).FirstOrDefault();

            if (_application == null)
            {
                return NotFound();
            }
            return Ok(_application);

        }

        [HttpGet]
        [Route("ApplicationSearch")]
        public IHttpActionResult ApplicationSearch(string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ApplicationSearchModel> _application = trackerDbRepository.ApplicationSearch(null, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_application.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_application);

        }

        [HttpGet]
        [Route("ApplicationSearch")]
        public IHttpActionResult ApplicationSearch(string searchBy)
        {
            IEnumerable<ApplicationSearchModel> _application = trackerDbRepository.ApplicationSearch(null, searchBy, null, null, null, null);

            if (_application.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_application);
        }

        [HttpPost]
        [Route("ApplicationAdd")]
        public IHttpActionResult ApplicationAdd(ApplicationModel application)
        {
            int? _application = trackerDbRepository.ApplicationAdd(application);

            if (!_application.HasValue)
            {
                return NotFound();
            }
            return Ok(_application);
        }

        [HttpPost]
        [Route("ApplicationUpdate")]
        public IHttpActionResult ApplicationUpdate(ApplicationModel application)
        {
            int? _application = trackerDbRepository.ApplicationUpdate(application);

            if (!_application.HasValue)
            {
                return NotFound();
            }
            return Ok(_application);
        }

        [HttpGet]
        [Route("ApplicationDelete")]
        public IHttpActionResult ApplicationDelete(Nullable<int> applicationId, Nullable<int> deleteUserId)
        {
            int? _application = trackerDbRepository.ApplicationDelete(applicationId, deleteUserId);

            if (!_application.HasValue)
            {
                return NotFound();
            }
            return Ok(_application);
        }
    }
}
