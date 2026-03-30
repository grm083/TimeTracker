using log4net;
using SBS.IT.Utilities.Logger.Core;
using System;
using System.Diagnostics;


namespace SBS.IT.Utilities.Logger.Implementation
{
    public class Log4NetLogger : ILogger
    {
        private static readonly ILog _log = LogManager.GetLogger(System.Environment.MachineName);

        public void WriteMessage(Type ClassName, LogLevel level, string Message, Exception Error = null)
        {
            //ILog _log = LogManager.GetLogger(ClassName);
            //_log = LogManager.GetLogger(System.Environment.MachineName);
            log4net.GlobalContext.Properties["userLogOn"] = string.Empty;
            log4net.GlobalContext.Properties["correlationId"] = string.Empty;
            log4net.GlobalContext.Properties["server"] = Environment.MachineName;
            switch (level)
            {
                case LogLevel.FATAL:
                    if (_log.IsFatalEnabled) _log.Fatal(Message, Error);
                    break;
                case LogLevel.ERROR:
                    if (_log.IsErrorEnabled) _log.Error(Message, Error);
                    break;
                case LogLevel.WARN:
                    if (_log.IsWarnEnabled) _log.Warn(Message, Error);
                    break;
                case LogLevel.INFO:
                    if (_log.IsInfoEnabled) _log.Info(Message, Error);
                    break;
                case LogLevel.VERBOSE:
                    if (_log.IsDebugEnabled) _log.Debug(Message, Error);
                    break;
            }
        }
        public void WriteMessage(Type ClassName, LogLevel level, string UserLogOn, string CorrelationId, string Message, Exception Error = null)
        {
            ILog _log = LogManager.GetLogger(ClassName);
            _log = LogManager.GetLogger(System.Environment.MachineName);
            log4net.GlobalContext.Properties["userLogOn"] = UserLogOn;
            log4net.GlobalContext.Properties["correlationId"] = CorrelationId;
            log4net.GlobalContext.Properties["server"] = Environment.MachineName;
            switch (level)
            {
                case LogLevel.FATAL:
                    if (_log.IsFatalEnabled) _log.Fatal(Message, Error);
                    break;
                case LogLevel.ERROR:
                    if (_log.IsErrorEnabled) _log.Error(Message, Error);
                    break;
                case LogLevel.WARN:
                    if (_log.IsWarnEnabled) _log.Warn(Message, Error);
                    break;
                case LogLevel.INFO:
                    if (_log.IsInfoEnabled) _log.Info(Message, Error);
                    break;
                case LogLevel.VERBOSE:
                    if (_log.IsDebugEnabled) _log.Debug(Message, Error);
                    break;
            }
        }
        public void WriteMessage(Type ClassName, LogLevel level, string UserLogOn, string CorrelationId, string SourceApplicationName, string Message, Exception Error = null)
        {
            ILog _log = LogManager.GetLogger(ClassName);
            _log = LogManager.GetLogger(System.Environment.MachineName);
            log4net.GlobalContext.Properties["userLogOn"] = UserLogOn;
            log4net.GlobalContext.Properties["correlationId"] = CorrelationId;
            log4net.GlobalContext.Properties["server"] = Environment.MachineName;
            switch (level)
            {
                case LogLevel.FATAL:
                    if (_log.IsFatalEnabled) _log.Fatal(Message, Error);
                    break;
                case LogLevel.ERROR:
                    if (_log.IsErrorEnabled) _log.Error(Message, Error);
                    break;
                case LogLevel.WARN:
                    if (_log.IsWarnEnabled) _log.Warn(Message, Error);
                    break;
                case LogLevel.INFO:
                    if (_log.IsInfoEnabled) _log.Info(Message, Error);
                    break;
                case LogLevel.VERBOSE:
                    if (_log.IsDebugEnabled) _log.Debug(Message, Error);
                    break;
            }
        }
    }
}
