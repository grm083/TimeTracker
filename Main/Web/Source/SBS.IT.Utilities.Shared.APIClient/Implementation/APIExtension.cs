using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using SBS.IT.Utilities.Shared.APIClient.Core;
using SBS.IT.Utilities.Shared.BaseMessage;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace SBS.IT.Utilities.Shared.APIClient.Implementation
{
    public class APIExtension : IAPIExtension
    {
        public const string CorrelationIdHeader = "X-Correlation-ID";
        private const string ApiKeyHeader = "X-Api-Key";

        private static readonly HttpClient httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        private readonly ILogger logger;
        private readonly string apiKey;

        public APIExtension()
        {
            logger = new Log4NetLogger();
            apiKey = ConfigurationManager.AppSettings["ApiKey"];
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
            var correlationId = Guid.NewGuid().ToString();
            string responseData;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, ServiceURL);
                request.Headers.Add(CorrelationIdHeader, correlationId);
                if (!string.IsNullOrEmpty(apiKey))
                    request.Headers.Add(ApiKeyHeader, apiKey);
                var response = httpClient.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                responseData = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR,
                    string.Format("[{0}] API GET request failed: {1}", correlationId, ServiceURL), ex);
                throw;
            }
            return JsonConvert.DeserializeObject<TResponse>(responseData);
        }

        /// <summary>
        /// method to Post data
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="ServiceURL"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public TResponse InvokePost<TResponse>(Uri ServiceURL, string postData)
        {
            var correlationId = Guid.NewGuid().ToString();
            string responseData;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, ServiceURL);
                request.Headers.Add(CorrelationIdHeader, correlationId);
                if (!string.IsNullOrEmpty(apiKey))
                    request.Headers.Add(ApiKeyHeader, apiKey);
                request.Content = new StringContent(postData, Encoding.UTF8, "application/json");
                var response = httpClient.SendAsync(request).Result;
                response.EnsureSuccessStatusCode();
                responseData = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                logger.WriteMessage(this.GetType(), LogLevel.ERROR,
                    string.Format("[{0}] API POST request failed: {1}", correlationId, ServiceURL), ex);
                throw;
            }
            return JsonConvert.DeserializeObject<TResponse>(responseData);
        }
    }
}
