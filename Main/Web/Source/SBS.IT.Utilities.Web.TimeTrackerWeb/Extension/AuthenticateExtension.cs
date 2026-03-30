using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Extension
{
    public static class AuthenticateExtension
    {
        public static string LogonName
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser()?.UserName;
            }
        }
        public static int UserId
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().EmployeeId;
            }
        }
        public static string FirstName
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().FirstName;
            }
        }
        public static string LastName
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().LastName;
            }
        }

        public static string FullName
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().EmployeeName;
            }
        }
        public static Nullable<int> UserTypeId
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().UserTypeId;
            }
        }
        public static string UserType
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().UserType;
            }
        }
        public static string TeamCode
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().TeamCode;
            }
        }
        public static int IsTimeEntryEnable
        {
            get
            {
                Authentication authentication = new Authentication();
                return authentication.GetAuthenticatedUser().IsTimeEntryEnable.GetValueOrDefault(0);
            }
        }

    }

    public class Authentication
    {
        public EmployeeAuthenticationModel GetAuthenticatedUser()
        {
            ISessionCacheManager sessionCacheManager = new SessionCacheManager();
            if (sessionCacheManager.Get<EmployeeAuthenticationModel>() != null)
            {
                return sessionCacheManager.Get<EmployeeAuthenticationModel>();
            }
            if (HttpContext.Current == null || HttpContext.Current.Request == null || !HttpContext.Current.Request.IsAuthenticated || !(HttpContext.Current.User.Identity is FormsIdentity))
            {
                return new EmployeeAuthenticationModel();
            }
            var formsIdentity = (FormsIdentity)HttpContext.Current.User.Identity;
            var userDataViewModel = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            if (userDataViewModel != null)
            {
                sessionCacheManager.Set<EmployeeAuthenticationModel>(userDataViewModel);
            }
            return userDataViewModel;
        }

        public EmployeeAuthenticationModel GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            // Session has expired but the Forms Auth cookie is still valid.
            // Rather than replaying stored credentials, return null to force
            // the user back to the login page via SessionTimeoutAttribute.
            return null;
        }
    }
}