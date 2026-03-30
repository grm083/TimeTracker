using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace SBS.IT.Utilities.Shared.BaseMessage
{
    public static class BaseServiceMessageUtility
    {
        public static T PrepareResponse<T>(this T Response, BaseServiceRequest Request = null) where T : BaseServiceResponse
        {
            if (Request != null)
            {
                Response.LogonName = Request.LogonName;
                Response.CorrelationId = Request.CorrelationId;
            }
            Response.ResponseIPAddress = GetServerIPAddress();
            Response.AcknowledgeType = eServiceAcknowledgeType.Success;
            Response.ResponseDateTime = DateTime.Now;
            Response.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Response.Errors = new List<ServiceError>();
            return Response;
        }
        internal static string GetRequestId
        {
            get { return Guid.NewGuid().ToString(); }
        }
        internal static string GetServerIPAddress()
        {
            string _ServerIP = string.Empty;
            try
            {
                IPHostEntry _IPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] _IPAddresses = _IPHostEntry.AddressList;
                if (_IPAddresses != null && _IPAddresses.Length > 0)
                {
                    _ServerIP = _IPAddresses[_IPAddresses.Length - 1].ToString();
                }
                return _ServerIP;
            }
            catch
            {
                return _ServerIP;
            }
        }
    }
}
