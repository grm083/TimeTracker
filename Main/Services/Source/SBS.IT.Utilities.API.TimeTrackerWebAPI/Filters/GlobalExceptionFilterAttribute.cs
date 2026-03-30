using SBS.IT.Utilities.Logger.Core;
using SBS.IT.Utilities.Logger.Implementation;
using System.Linq;
using System.Web.Http.Filters;

namespace SBS.IT.Utilities.API.TimeTrackerWebAPI.Filters
{
    /// <summary>
    /// Global exception filter that logs all unhandled API exceptions with correlation ID.
    /// </summary>
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly ILogger logger = new Log4NetLogger();

        public override void OnException(HttpActionExecutedContext context)
        {
            var correlationId = string.Empty;
            if (context.Request.Headers.Contains(CorrelationIdHeader))
            {
                correlationId = context.Request.Headers.GetValues(CorrelationIdHeader).FirstOrDefault();
            }

            logger.WriteMessage(
                this.GetType(),
                LogLevel.ERROR,
                string.Format("[{0}] Unhandled exception in {1} {2}",
                    correlationId,
                    context.Request.Method,
                    context.Request.RequestUri),
                context.Exception);

            base.OnException(context);
        }
    }
}
