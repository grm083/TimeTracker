using SBS.IT.Utilities.Logger.Core;
using System;
using System.Threading;

namespace SBS.IT.Utilities.Web.TimeTrackerWeb.Services
{
    /// <summary>
    /// Retries an API call once on failure before throwing.
    /// Designed to handle transient failures (network blips, timeouts, brief DB locks).
    /// </summary>
    public static class ApiRetryHelper
    {
        private const int RetryDelayMs = 1000;

        /// <summary>
        /// Executes the given function. If it throws, waits briefly and retries once.
        /// If the retry also fails, the exception propagates to the caller.
        /// </summary>
        public static T ExecuteWithRetry<T>(Func<T> apiCall, ILogger logger, Type callerType, string context)
        {
            try
            {
                return apiCall();
            }
            catch (Exception firstEx)
            {
                logger.WriteMessage(callerType, LogLevel.WARN,
                    string.Format("API call failed ({0}), retrying once: {1}", context, firstEx.Message), firstEx);

                Thread.Sleep(RetryDelayMs);

                // Second attempt - let it throw if it fails
                return apiCall();
            }
        }
    }
}
