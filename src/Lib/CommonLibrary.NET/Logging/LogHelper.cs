using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Authentication;


namespace ComLib.Logging
{
    /// <summary>
    /// Helper class for logging.
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Logs to the console.
        /// </summary>
        /// <typeparam name="T">The datatype of the caller that is logging the event.</typeparam>
        /// <param name="level">The log level</param>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="args">Additional arguments.</param>
        public void LogToConsole<T>(LogLevel level, string message, Exception ex, object[] args)
        {
            LogEvent logevent = BuildLogEvent(typeof(T), level, message, ex, null);
            Console.WriteLine(logevent.FinalMessage);
        }


        /// <summary>
        /// Construct the logevent using the values supplied.
        /// Fills in other data values in the log event.
        /// </summary>
        /// <param name="logType">The type the logger is associated with.</param>
        /// <param name="level">The log level</param>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="args">Additional args.</param>
        /// <returns>Created log event.</returns>
        public static LogEvent BuildLogEvent(Type logType, LogLevel level, object message, System.Exception ex, object[] args)
        {
            LogEvent logevent = new LogEvent();
            logevent.Level = level;
            logevent.Message = message;
            logevent.Error = ex;
            logevent.Args = args;
            logevent.Computer = System.Environment.MachineName;
            logevent.CreateTime = DateTime.Now;
            logevent.ThreadName = System.Threading.Thread.CurrentThread.Name;
            logevent.UserName = Auth.UserShortName;
            logevent.LogType = logType;
            logevent.FinalMessage = LogFormatter.Format(null, logevent);
            return logevent;
        }


        /// <summary>
        /// Builds a log level from a string.
        /// </summary>
        /// <param name="loglevel">Log level : "critical | error | warning | info | debug".</param>
        /// <returns>Parsed log level.</returns>
        public static LogLevel GetLogLevel(string loglevel)
        {
            LogLevel level = (LogLevel)Enum.Parse(typeof(LogLevel), loglevel, true);
            return level;
        }


        /// <summary>
        /// Build the log file name.
        /// </summary>
        /// <param name="appName">E.g. "StockMarketApplication".</param>
        /// <param name="date">E.g. Date to put in the name.</param>
        /// <param name="env">Environment name. E.g. "DEV", "PROD".</param>
        /// <param name="logFileName">E.g. "%name%-%yyyy%-%MM%-%dd%-%env%-%user%.log".
        /// Name of logfile containing substituions. </param>
        /// <returns>Log file name.</returns>
        public static string BuildLogFileName( string logFileName, string appName, DateTime date, string env)
        {
            if(string.IsNullOrEmpty(env)) env = string.Empty;

            // Log file name = <app>-<date>-<env>.log
            // e.g.  StockMarketApp-2009-10-30-PROD.log
            IDictionary<string, string> subs = new Dictionary<string, string>();
            subs["%datetime%"] = date.ToString("yyyy-MM-dd-HH-mm-ss"); 
            subs["%date%"] = date.ToString("yyyy-MM-dd");            
            subs["%yyyy%"] = date.ToString("yyyy");
            subs["%MM%"] = date.ToString("MM");
            subs["%dd%"] = date.ToString("dd");
            subs["%MMM%"] = date.ToString("MMM");
            subs["%hh%"] =  date.ToString("hh");
            subs["%HH%"] =  date.ToString("HH");
            subs["%mm%"] = date.ToString("mm");
            subs["%ss%"] =  date.ToString("ss");
            subs["%name%"] = appName;
            subs["%env%"] = env.ToUpper();
            subs["%user%"] = Auth.UserShortName;
            subs.ForEach<KeyValuePair<string, string>>( pair => 
            {
               logFileName = logFileName.Replace(pair.Key, pair.Value);
            });
            if (!logFileName.Contains(".log") && !logFileName.Contains(".txt"))
                logFileName += ".log";

            // Replace any left over % with underscore "_".
            logFileName = logFileName.Replace("%", "_");
            logFileName = logFileName.Replace("--", "-");
            logFileName = logFileName.Replace("__", "_");
            if (logFileName.StartsWith("-")) logFileName = "Log" + logFileName;
            if (logFileName.StartsWith("_")) logFileName = "Log" + logFileName;
            
            return logFileName;
        }
    }



    /// <summary>
    /// Log formatter.
    /// </summary>
    public class LogFormatter
    {
        /// <summary>
        /// Quick formatter that toggles between delimited and xml.
        /// </summary>
        /// <param name="formatter">Formatter to use (empty or "xml").</param>
        /// <param name="logEvent">Event to log.</param>
        /// <returns>Formatted string with event.</returns>
        public static string Format(string formatter, LogEvent logEvent)
        {
            if (string.IsNullOrEmpty(formatter))
                return Format(logEvent);

            if (formatter.ToLower().Trim() == "xml")
                return FormatXml(logEvent);

            return Format(logEvent);
        }


        /// <summary>
        /// Builds the log message using message and arguments.
        /// </summary>
        /// <param name="logEvent">The log event object.</param>
        /// <returns>Formatted string with event.</returns>
        public static string Format(LogEvent logEvent)
        {
            string msg = StringHelper.ConvertToString(logEvent.Args);
            string message = logEvent.Message == null ? string.Empty : logEvent.Message.ToString();
            msg = string.IsNullOrEmpty(msg) ? message.ToString() : message.ToString() + " - " + msg;

            // Build a delimited string
            // <time>:<thread>:<level>:<loggername>:<message>
            string line = logEvent.CreateTime.ToString();

            if (!string.IsNullOrEmpty(logEvent.ThreadName)) line += ":" + logEvent.ThreadName;
            line += ":" + logEvent.Level.ToString();
            line += ":" + logEvent.LogType.Name;
            line += ":" + msg;
            return line;
        }


        /// <summary>
        /// Builds the log message using message and arguments.
        /// </summary>
        /// <param name="logEvent">The log event object</param>
        /// <returns>Formatted string with event.</returns>
        public static string FormatXml(LogEvent logEvent)
        {
            string msg = StringHelper.ConvertToString(logEvent.Args);
            string message = logEvent.Message.ToString();
            msg = string.IsNullOrEmpty(msg) ? message.ToString() : message.ToString() + " - " + msg;

            // Build a delimited string
            // <time>:<thread>:<level>:<loggername>:<message>
            string line = string.Format("<time>{0}</time>", logEvent.CreateTime.ToString());

            if (!string.IsNullOrEmpty(logEvent.ThreadName)) line += string.Format("<thread>{0}</thread>", logEvent.ThreadName);
            line += string.Format("<level>{0}</level>", logEvent.Level.ToString());
            line += string.Format("<type>{0}</type>", logEvent.LogType.Name);
            line += string.Format("<message>{0}</message>", msg);
            return line;
        }
    }
}
