using Kendo.Mvc.Extensions;
using Newtonsoft.Json;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.APIClient.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Message;
using SBS.IT.Utilities.Shared.Cache.Core;
using SBS.IT.Utilities.Shared.Cache.Implementation;
using SBS.IT.Utilities.Web.TimeTrackerWeb.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly TimeSpan _expirationTimeSpan;
        private readonly IAPIExtension apiExtension;
        private readonly IAPIConfiguration apiConfiguration;
        private readonly ISessionCacheManager sessionCacheManager;
        private readonly ILogger logger;
        // GET: Account
        public AccountController()
        {
            this._expirationTimeSpan = FormsAuthentication.Timeout;
            apiExtension = new APIExtension();
            apiConfiguration = new APIConfiguration();
            sessionCacheManager = new SessionCacheManager();
            logger = new Log4NetLogger();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginModel.UserName) && !string.IsNullOrEmpty(loginModel.Password))
                {
                    sessionCacheManager.Clear();
                    EmployeeAuthenticationModel employeeAuthenticationModel = ValidateUser(loginModel.UserName, loginModel.Password);
                    if (employeeAuthenticationModel != null && employeeAuthenticationModel.EmployeeId > 0)
                    {
                        var now = DateTime.UtcNow.ToLocalTime();
                        var userDataViewModelJson = new JavaScriptSerializer().Serialize(loginModel);
                        var ticket = new FormsAuthenticationTicket(1, loginModel.UserName, now, now.Add(_expirationTimeSpan), false, userDataViewModelJson, FormsAuthentication.FormsCookiePath);
                        var encryptedTicket = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        cookie.HttpOnly = true;
                        if (ticket.IsPersistent)
                        {
                            cookie.Expires = ticket.Expiration;
                        }
                        cookie.Secure = FormsAuthentication.RequireSSL;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        if (FormsAuthentication.CookieDomain != null)
                        {
                            cookie.Domain = FormsAuthentication.CookieDomain;
                        }
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                        //Session["UserType"] = employeeAuthenticationModel.UserType;
                        Session["FullName"] = string.Concat(employeeAuthenticationModel.FirstName, " ", employeeAuthenticationModel.LastName);
                        if (employeeAuthenticationModel != null && !string.IsNullOrEmpty(employeeAuthenticationModel.UserType) && (employeeAuthenticationModel.UserType == "ADN" || employeeAuthenticationModel.UserType == "SAN"))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("Add", "Timesheet");
                        }

                    }
                    else
                        ViewBag.errormessage = "Invalid User Name or Password";
                }
                else
                {
                    ViewBag.errormessage = "Entered Invalid Username and Password";
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.FATAL, ex.Message, ex);
            }
            return View(loginModel);
        }

        /// <summary>
        /// method to validate users
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public EmployeeAuthenticationModel ValidateUser(string userName, string password)
        {
            EmployeeAuthenticationRequestModel request = new EmployeeAuthenticationRequestModel()
            {
                LogonName = userName,
                LogonPassword = password
            };
            string postData = JsonConvert.SerializeObject(request);
            EmployeeAuthenticationModel employeeAuthenticationModel = null;
            try
            {
                employeeAuthenticationModel = apiExtension.InvokePost<EmployeeAuthenticationModel>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.GetAuthentication), postData);

                if (employeeAuthenticationModel != null && employeeAuthenticationModel.EmployeeId > 0)
                {
                    employeeAuthenticationModel.UserName = userName;
                    employeeAuthenticationModel.EmployeeName = string.Concat(employeeAuthenticationModel.FirstName, " ", employeeAuthenticationModel.LastName);
                    sessionCacheManager.Set<EmployeeAuthenticationModel>(employeeAuthenticationModel);
                }
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
            }
            return employeeAuthenticationModel;
        }

        /// <summary>
        /// method to logout
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                sessionCacheManager.Clear();
                System.Web.HttpContext.Current.Session.Clear();
                System.Web.HttpContext.Current.Session.Abandon();
                HttpCookie _FormsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                _FormsCookie.Expires = DateTime.Now.AddYears(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(_FormsCookie);
                HttpCookie _SessionCookie = new HttpCookie("ASP.NET_SessionId", "");
                _SessionCookie.Expires = DateTime.Now.AddYears(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(_SessionCookie);
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR, ex.Message, ex);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            EmployeeViewModel employeeModel = new EmployeeViewModel();
            EmployeeAuthenticationModel employeeAuthenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (employeeModel != null && employeeModel.employeeId > 0)
            {
                employeeModel.employeeId = employeeAuthenticationModel.EmployeeId;
                employeeModel.logonName = employeeAuthenticationModel.UserName;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(employeeModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(EmployeeViewModel employeeModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(ModelState.ToDataSourceResult());
            }

            EmployeeAuthenticationModel employeeAuthenticationModel = ValidateUser(employeeModel.logonName, employeeModel.currentPassword);
            if (employeeAuthenticationModel == null)
            {
                ViewBag.errormessage = "Invalid";
                return View("ChangePassword", employeeModel);
            }
            int savedCount = 0;
            EmployeeAuthenticationModel authenticationModel = sessionCacheManager.Get<EmployeeAuthenticationModel>();
            if (employeeModel == null)
            {
                return RedirectToAction("Login", "Account");
            }
            employeeModel.updateUserId = authenticationModel.EmployeeId;
            if (employeeModel != null && employeeModel.employeeId > 0)
            {
                string postData = JsonConvert.SerializeObject(employeeModel);
                //savedCount = apiExtension.InvokePost<int>(new Uri(apiConfiguration.ServiceBaseAddress + APIResources.ChangePassword + "?employeeId=" + employeeModel.employeeId + "&logonName=" + employeeModel.logonName + "&logonPassword=" + employeeModel.newPassword + "&updateUserId=" + employeeModel.updateUserId), postData);
                ModelState.Clear();
            }
            if (savedCount == 1)
            {
                ViewBag.errormessage = "Success";
                return View("ChangePassword", employeeModel);
            }
            else
            {
                ViewBag.errormessage = "Fail";
                return View("ChangePassword", employeeModel);
            }
        }

        public virtual ViewResult AccessDenied()
        {
            return View();
        }
    }
}