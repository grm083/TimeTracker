using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Core;
using SBS.IT.Utilities.DataAccess.TimeTrackerDb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        private readonly ITrackerDbRepository trackerDbRepository;

        public EmployeeController(ITrackerDbRepository _trackerDbRepository)
        {
            trackerDbRepository = _trackerDbRepository;
        }

        [HttpPost]
        [Route("GetAuthentication")]
        public IHttpActionResult GetAuthentication(EmployeeAuthenticationRequestModel request)
        {
            request.DomainName = System.Configuration.ConfigurationManager.AppSettings["NetworkDomain"].ToString();
            request.IsProduction = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsProduction"]);

            var _employee = trackerDbRepository.GetAuthentication(request);

            if (_employee == null)
            {
                return NotFound();
            }
            return  Ok(_employee.FirstOrDefault());
        }

        [HttpGet]
        [Route("CheckLogonName")]
        public IHttpActionResult CheckLogonName(string logonName)
        {
            string domainName = System.Configuration.ConfigurationManager.AppSettings["NetworkDomain"].ToString();
            bool isProduction = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsProduction"]);
            var _employee = trackerDbRepository.CheckLogonName(logonName, domainName);

            return Ok(_employee);
        }

        [HttpPost]
        [Route("EmployeeAdd")]
        public IHttpActionResult EmployeeAdd(EmployeeModel employee)
        {
            employee.Domain = System.Configuration.ConfigurationManager.AppSettings["NetworkDomain"].ToString();
            int? _employee = trackerDbRepository.EmployeeAdd(employee);

            if (!_employee.HasValue)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpPost]
        [Route("EmployeeUpdate")]
        public IHttpActionResult EmployeeUpdate(EmployeeModel employee)
        {
            int? _employee = trackerDbRepository.EmployeeUpdate(employee);

            if (!_employee.HasValue)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public IHttpActionResult ChangePassword(Nullable<int> employeeId, string logonName, string logonPassword, Nullable<int> updateUserId)
        {
            int? _employee = trackerDbRepository.EmployeeUpdatePassword(employeeId, logonName, logonPassword, updateUserId);

            if (!_employee.HasValue)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpPost]
        [Route("EmployeeDelete")]
        public IHttpActionResult EmployeeDelete(Nullable<int> employeeId, Nullable<int> deleteUserId)
        {
            int? _employee = trackerDbRepository.EmployeeDelete(employeeId, deleteUserId);

            if (!_employee.HasValue)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("GetManager")]
        public IHttpActionResult GetManager()
        {
            IEnumerable<EmployeeManagerModel> _employee = trackerDbRepository.GetManager();

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("GetEmployeeListByManagerId")]
        public IHttpActionResult GetEmployeeListByManagerId(Nullable<int> managerId)
        {
            IEnumerable<EmployeeListModel> _employee = trackerDbRepository.GetEmployeeList(managerId);

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("GetEmployeeList")]
        public IHttpActionResult GetEmployeeList()
        {
            IEnumerable<EmployeeListModel> _employee = trackerDbRepository.GetEmployeeList(null);

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("EmployeeSearch")]
        public IHttpActionResult EmployeeSearch(Nullable<int> employeeId, Nullable<int> managerId, string searchBy, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<EmployeeSearchModel> _employee = trackerDbRepository.EmployeeSearch(employeeId, managerId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("EmployeeSearch")]
        public IHttpActionResult EmployeeSearch(string searchBy, Nullable<int> managerId, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<bool> sortOrder, string sortColumn)
        {
            IEnumerable<EmployeeSearchModel> _employee = trackerDbRepository.EmployeeSearch(null, managerId, searchBy, pageSize, pageNumber, sortOrder, sortColumn);

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }

        [HttpGet]
        [Route("GetByEmployeeId")]
        public IHttpActionResult GetByEmployeeId(Nullable<int> employeeId)
        {
            EmployeeSearchModel _employee = trackerDbRepository.EmployeeSearch(employeeId, null, null, null, null, null, null).FirstOrDefault();

            if (_employee == null)
            {
                return NotFound();
            }
            return Ok(_employee);
        }
    }
}
