using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.IT.Utilities.Logger.Core
{
    public interface ILogger
    {
        void WriteMessage(Type ClassName, LogLevel level, string Message, Exception Error = null);
        void WriteMessage(Type ClassName, LogLevel level, string UserLogOn, string CorrelationId, string Message, Exception Error = null);
        void WriteMessage(Type ClassName, LogLevel level, string UserLogOn, string CorrelationId, string SourceApplicationName, string Message, Exception Error = null);
    }
    public enum LogLevel
    {
        FATAL = 0,
        ERROR = 1,
        WARN = 2,
        INFO = 3,
        VERBOSE = 4
    }
}
