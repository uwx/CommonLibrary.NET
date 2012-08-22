using System;
using System.Collections.Generic;
using System.Linq;

namespace ComLib
{
    /// <summary>
    /// Logger using lamda method to call external logging code.
    /// This decouples code from using a specific ILogging interface via lamdas.
    /// </summary>
    public class LamdaLogger
    {
        private static Action<object, Exception, object[]> _criticalLogger;
        private static Action<object, Exception, object[]> _errorLogger;
        private static Action<object, Exception, object[]> _infoLogger;
        private static Action<object, Exception, object[]> _debugLogger;


        /// <summary>
        /// Initialize default loggers to console.
        /// </summary>
        public LamdaLogger()
        {
            // Default logger to console.
            _criticalLogger = (message, ex, args) => Console.WriteLine(BuildMessage("critical", message, ex, args));
            _errorLogger = (message, ex, args) => Console.WriteLine(BuildMessage("error", message, ex, args));
            _infoLogger = (message, ex, args) => Console.WriteLine(BuildMessage("info", message, ex, args));
            _debugLogger = (message, ex, args) => Console.WriteLine(BuildMessage("debug", message, ex, args));
        }


        /// <summary>
        /// Initialize the different level lamda loggers.
        /// </summary>
        /// <param name="criticalLogger">Critical logger.</param>
        /// <param name="errorLogger">Error logger.</param>
        /// <param name="infoLogger">Information logger.</param>
        /// <param name="debugLogger">Debug logger.</param>
        public void Init(Action<object, Exception, object[]> criticalLogger, Action<object, Exception, object[]> errorLogger, 
            Action<object, Exception, object[]> infoLogger, Action<object, Exception, object[]> debugLogger)
        {
            if (criticalLogger != null) _criticalLogger = criticalLogger;
            if (errorLogger != null) _errorLogger = errorLogger;
            if (infoLogger != null) _infoLogger = infoLogger;
            if (debugLogger != null) _debugLogger = debugLogger;
        }


        /// <summary>
        /// Log a critical error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="ex">Relevant exception.</param>
        /// <param name="args">Logging arguments.</param>
        public void Critical(object message, Exception ex = null, object[] args = null)
        {
            if (_criticalLogger != null)
                _criticalLogger(message, ex, args);
        }


        /// <summary>
        /// Log an error.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="ex">Relevant exception.</param>
        /// <param name="args">Logging arguments.</param>
        public void Error(object message, Exception ex = null, object[] args = null)
        {
            if (_errorLogger != null)
                _errorLogger(message, ex, args);
        }


        /// <summary>
        /// Log info.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="ex">Relevant exception.</param>
        /// <param name="args">Logging arguments.</param>
        public void Info(object message, Exception ex = null, object[] args = null)
        {
            if (_infoLogger != null)
                _infoLogger(message, ex, args);
        }


        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="ex">Relevant exception.</param>
        /// <param name="args">Logging arguments.</param>
        public void Debug(object message, Exception ex = null, object[] args = null)
        {
            if (_debugLogger != null)
                _debugLogger(message, ex, args);
        }


        private static string BuildMessage(string level, object message, Exception ex, object[] args)
        {
            string finalMessage = level.ToUpper() + " : " + message + Environment.NewLine
                                    + ex.Message + Environment.NewLine
                                    + ex.StackTrace + Environment.NewLine;
            return finalMessage;
        }
    }
}