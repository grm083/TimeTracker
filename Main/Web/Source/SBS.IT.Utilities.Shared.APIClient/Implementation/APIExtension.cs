using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.BaseMessage;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace SBS.IT.Utilities.Shared.APIClient.Implementation
{
    public class APIExtension : IAPIExtension
    {
        private readonly ILogger logger;
        public APIExtension()
        {
            logger = new Log4NetLogger();
        }

        public TResponse InvokeServiceWithBasicAuth<TResponse>(Uri ServiceURL, string ServiceMethod, APIRequestBase Request)
        {
            try
            {
                var client = new RestClient();
                client.Timeout = TimeSpan.FromMinutes(5).Milliseconds;
                client.BaseUrl = ServiceURL;
                var restRequest = new RestRequest(ServiceURL.AbsoluteUri, string.Equals(ServiceMethod, "POST") ? Method.POST : Method.GET);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };
                var requestContentJson = JsonConvert.SerializeObject(Request, settings);
                if (requestContentJson != null && requestContentJson != "" && requestContentJson != "{}")
                {
                    restRequest.AddParameter("application/json", requestContentJson, ParameterType.RequestBody);
                    restRequest.AddJsonBody(Request);
                }
                var restRawResponse = client.Execute(restRequest);
                var restResponse = JsonConvert.DeserializeObject<TResponse>(restRawResponse.Content);
                return restResponse;
            }
            catch (Exception e)
            {
                logger.WriteMessage(this.GetType(), LogLevel.FATAL, e.Message, e);
                throw;
            }
        }

        /// <summary>
        /// method to invoke get
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ServiceURL"></param>
        /// <returns></returns>
        public TResponse InvokeGet<TResponse>(Uri ServiceURL)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceURL.AbsoluteUri);
            request.Method = "GET";
            String responseData = String.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                        }
                    }
                }
            }
            var rawResponse = JsonConvert.DeserializeObject<TResponse>(responseData);
            return rawResponse;

        }

        /// <summary>
        /// method to Post data
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ServiceURL"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public TResponse InvokePost<TResponse>(Uri ServiceURL,string postData)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ServiceURL.AbsoluteUri);
            request.Method = "POST";
            var data = Encoding.ASCII.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            String responseData = String.Empty;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseData = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                        }
                    }
                }
            }
            var rawResponse = JsonConvert.DeserializeObject<TResponse>(responseData);
            return rawResponse;
        }
    }
}
