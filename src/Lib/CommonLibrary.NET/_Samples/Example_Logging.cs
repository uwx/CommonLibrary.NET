using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

using ComLib;
using ComLib.Application;
using ComLib.Logging;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Logging namespace.
    /// </summary>
    public class Example_Logging : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            Console.WriteLine("====================================================");
            Console.WriteLine("LOGGING ");

            // 1. Use default logger which logs to console.
            Logger.Info("The default logger log to the console.");

            // 2. Another way to access the default logger.
            Logger.Default.Info("Accessing default logger using Logger.Default", null, null);

            // 3. Append a new logger to the default logger and log.
            Logger.Default.Append(new LogFile("kishores_log", "kishore.txt"));
            Logger.Info("After appending to default logger. Logs both to console and file.");

            // 4. Log only to the named logger "kishores_log" in the default logger.
            Logger.Default["kishores_log"].Info("Logging only to logger named 'kishores_log' in the default logger.", null, null);
            
            // 5. Get a new instance of a logger( associated with default logger)
            //    that is specific to this type. 
            //    - This logger only exists in the current scope and is not appended to any other logger.
            ILog mylog = Logger.GetNew<Example_Logging>();
            mylog.Info("Logger.GetNew<Example_Logging>() is equivalent to log4net.getlogger(typeof(abc));", null, null);
            
            // 6. Add a new logger 
            // ( This is a completely separate logger from "default" logger.
            Logger.Add(new LogMulti("admin_logger", new LogFile("logger1", "admin.txt")));
            Logger.Get("admin_logger").Info("logging to admin_logger, this is NOT the default logger.", null, null);

            // 7. Force a flush only on the "admin_logger"
            Logger.Get("admin_logger").Flush();

            // 8. Always logs the message regardless of log-level.
            Logger.Message("Logger.Message calls always get logged.");

            // 9. Change the log level to error only on the Default loggers named logger "kishores_log".
            Logger.Default["kishores_log"].Level = LogLevel.Error;
            Logger.Default["kishores_log"].Info("this should not get logged.", null, null);

            return BoolMessageItem.True;
        }


        /// <summary>
        /// Shutdown dependent services.
        /// </summary>
        public override void ShutDown()
        {
            Logger.Flush();
            Logger.ShutDown();
        }
    }
}
