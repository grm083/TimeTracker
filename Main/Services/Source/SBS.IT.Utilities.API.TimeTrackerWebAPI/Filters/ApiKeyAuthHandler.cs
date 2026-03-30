using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Filters
{
    /// <summary>
    /// Delegating handler that validates an API key on every request.
    /// The expected key is stored in AppSettings["ApiKey"].
    /// Clients must send the key in the X-Api-Key header.
    /// If no ApiKey is configured, authentication is bypassed (backward compatible).
    /// </summary>
    public class ApiKeyAuthHandler : DelegatingHandler
    {
        private const string ApiKeyHeader = "X-Api-Key";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var expectedKey = ConfigurationManager.AppSettings["ApiKey"];

            // If no API key is configured, bypass authentication (backward compatible)
            if (string.IsNullOrEmpty(expectedKey))
                return base.SendAsync(request, cancellationToken);

            if (!request.Headers.Contains(ApiKeyHeader) ||
                !request.Headers.GetValues(ApiKeyHeader).Any(v => v == expectedKey))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Invalid or missing API key",
                    Content = new StringContent("Unauthorized: A valid X-Api-Key header is required.")
                });
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
