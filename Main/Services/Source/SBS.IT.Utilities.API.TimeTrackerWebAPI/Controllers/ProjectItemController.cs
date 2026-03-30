using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Project")]
    public class ProjectItemController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public ProjectItemController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpGet]
        [Route("GetProjectItem")]
        public IHttpActionResult GetProjectItem(Nullable<int> isActive, Nullable<int> applicationId, Nullable<int> projectId)
        {
            IEnumerable<ProjectItemModel> _projectItem = trackerDbRepository.GetProjectItem(isActive, projectId);

            if (_projectItem.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItem);
        }

        [HttpGet]
        [Route("GetProjectItem")]
        public IHttpActionResult GetProjectItem(Nullable<int> projectId)
        {
            IEnumerable<ProjectItemModel> _projectItem = trackerDbRepository.GetProjectItem(null, projectId);

            if (_projectItem.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItem);
        }

        [HttpGet]
        [Route("ProjectItemSearch")]
        public IHttpActionResult ProjectItemSearch(Nullable<int> projectItemId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ProjectItemSearchModel> _projectItem = trackerDbRepository.ProjectItemSearch(projectItemId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_projectItem.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItem);
        }

        [HttpGet]
        [Route("ProjectItemSearch")]
        public IHttpActionResult ProjectItemSearch(string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<ProjectItemSearchModel> _projectItem = trackerDbRepository.ProjectItemSearch(null, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_projectItem.Count() == 0)
            {
                return NotFound();
            }
            return Ok(_projectItem);
        }

        [HttpGet]
        [Route("GetByProjectItemId")]
        public IHttpActionResult GetByProjectItemId(Nullable<int> projectItemId)
        {
            ProjectItemSearchModel _projectItem = trackerDbRepository.ProjectItemSearch(projectItemId, null, null, null, null, null).FirstOrDefault();

            if (_projectItem == null)
            {
                return NotFound();
            }
            return Ok(_projectItem);
        }

        [HttpPost]
        [Route("ProjectItemAdd")]
        public IHttpActionResult ProjectItemAdd(ProjectItemModel projectItem)
        {
            int? _projectItemId = trackerDbRepository.ProjectItemAdd(projectItem);

            if (!_projectItemId.HasValue)
            {
                return NotFound();
            }
            return Ok(_projectItemId);
        }

        [HttpPost]
        [Route("ProjectItemUpdate")]
        public IHttpActionResult ProjectItemUpdate(ProjectItemModel projectItem)
        {
            int? _projectItemId = trackerDbRepository.ProjectItemUpdate(projectItem);

            if (!_projectItemId.HasValue)
            {
                return NotFound();
            }
            return Ok(_projectItemId);
        }

        [HttpGet]
        [Route("ProjectItemDelete")]
        public IHttpActionResult ProjectItemDelete(Nullable<int> projectItemId, Nullable<int> deleteUserId)
        {
            int? _projectItemId = trackerDbRepository.ProjectItemDelete(projectItemId, deleteUserId);

            if (!_projectItemId.HasValue)
            {
                return NotFound();
            }
            return Ok(_projectItemId);
        }
    }
}
