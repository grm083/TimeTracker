using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Project")]
    public class ProjectController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ProjectController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetProject")]
        public IHttpActionResult GetProject(Nullable<int> isActive)
        {
            IEnumerable<ProjectModel> _project = trackerDbRepository.GetProject(isActive);

            if (_project.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpGet]
        [Route("GetProject")]
        public IHttpActionResult GetProject()
        {
            IEnumerable<ProjectModel> _project = trackerDbRepository.GetProject(null);

            if (_project.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpGet]
        [Route("ProjectDelete")]
        public IHttpActionResult ProjectDelete(Nullable<int> projectId, Nullable<int> deleteUserId)
        {
            int? _project = trackerDbRepository.ProjectDelete(projectId, deleteUserId);

            if (!_project.HasValue)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpPost]
        [Route("ProjectUpdate")]
        public IHttpActionResult ProjectUpdate(ProjectModel project)
        {
            int? _project = trackerDbRepository.ProjectUpdate(project);

            if (!_project.HasValue)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpPost]
        [Route("ProjectAdd")]
        public IHttpActionResult ProjectAdd(ProjectModel project)
        {
            int? _project = trackerDbRepository.ProjectAdd(project);

            if (!_project.HasValue)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpGet]
        [Route("ProjectSearch")]
        public IHttpActionResult ProjectSearch(Nullable<int> projectId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ProjectSearchModel> _project = trackerDbRepository.ProjectSearch(projectId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_project.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpGet]
        [Route("ProjectSearch")]
        public IHttpActionResult ProjectSearch(string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ProjectSearchModel> _project = trackerDbRepository.ProjectSearch(null, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_project.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_project);
        }

        [HttpGet]
        [Route("GetByProjectId")]
        public IHttpActionResult GetByProjectId(Nullable<int> projectId)
        {
            ProjectSearchModel _project = trackerDbRepository.ProjectSearch(projectId, null, null, null, null, null).FirstOrDefault();

            if (_project == null)
            {
                return NotFound();
            }
            return Ok(_project);
        }
    }
}
