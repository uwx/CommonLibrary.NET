/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using ComLib;
using ComLib.Collections;


namespace ComLib.Logging
{

    /// <summary>
    /// Light weight logging class.
    /// </summary>
    /// <remarks>
    /// The provider is initialize to an instance of <see cref="LogConsole"/>
    /// so it's ready be used immediately.
    /// </remarks>
    public class Logger
    {
        /// <summary>
        /// Default the logger provider to the consolelogger so the logger
        /// is ready to use immediately.
        /// </summary>
        // private static LoggerMulti _logger = new LoggerMulti(new List<ILog>() { new LoggerConsole() });
        private static DictionaryOrdered<string, ILogMulti> _loggers = new DictionaryOrdered<string, ILogMulti>()
        {
            { "default", new LogMulti( "default", new LogConsole("console")) }
        };


        private static ReaderWriterLock _readwriteLock = new ReaderWriterLock();
        // TO_DO: KISHORE - Need to make these settings available to caller.
        private static int _lockMilliSecondsForRead = 1000;
        private static int _lockMilliSecondsForWrite = 1000;


        /// <summary>
        /// Prevent instantiation. This is a static class.
        /// </summary>
        private Logger()
        {
        }        


        #region Log using level
        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        public static void Log(LogLevel level, object message)
        {
            Log(level, message, null, null);
        }


        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Log(LogLevel level, object message, Exception exception)
        {
            Log(level, message, exception, null);
        }


        /// <summary>
        /// Logs the specified level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        public static void Log(LogLevel level, object message, Exception exception, object[] args)
        {
            LogEvent logEvent = LogHelper.BuildLogEvent(typeof(Logger), level, message, exception, args);
            Default.Log(logEvent);
        }
        #endregion


        #region Log Warnings
        /// <summary>
        /// Logs as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(object message)
        {
            Log(LogLevel.Warn, message, null, null);
        }


        /// <summary>
        /// Logs as warning.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Warn(object message, System.Exception exception)
        {
            Log(LogLevel.Warn, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Warn(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Warn, message, exception, arguments);
        }
        #endregion


        #region Log Errors
        /// <summary>
        /// Logs as error.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(object message)
        {
            Log(LogLevel.Error, message, null, null);
        }


        /// <summary>
        /// Logs as error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Error(object message, System.Exception exception)
        {
            Log(LogLevel.Error, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Error(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Error, message, exception, arguments);
        }
        #endregion


        #region Log Debug
        /// <summary>
        /// Logs as debug.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Debug(object message)
        {
            Log(LogLevel.Debug, message, null, null);
        }


        /// <summary>
        /// Logs as debug.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Debug(object message, System.Exception exception)
        {
            Log(LogLevel.Debug, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Debug(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Debug, message, exception, arguments);
        }
        #endregion


        #region Log Fatal
        /// <summary>
        /// Logs as fatal.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Fatal(object message)
        {
            Log(LogLevel.Fatal, message, null, null);
        }


        /// <summary>
        /// Logs as fatal.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Fatal(object message, System.Exception exception)
        {
            Log(LogLevel.Fatal, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Fatal(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Fatal, message, exception, arguments);
        }
        #endregion


        #region Log Info
        /// <summary>
        /// Logs as info.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(object message)
        {
            Log(LogLevel.Info, message, null, null);
        }


        /// <summary>
        /// Logs as info.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Info(object message, System.Exception exception)
        {
            Log(LogLevel.Info, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Info(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Info, message, exception, arguments);
        }      
        #endregion


        #region Log Message
        /// <summary>
        /// Logs as message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Message(object message)
        {            
            Log(LogLevel.Message, message, null, null);
        }


        /// <summary>
        /// Logs as message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public static void Message(object message, System.Exception exception)
        {
            Log(LogLevel.Message, message, exception, null);
        }


        /// <summary>
        /// Logs the message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Message(object message, System.Exception exception, object[] arguments)
        {
            Log(LogLevel.Message, message, exception, arguments);
        }
        #endregion


        #region Check loglevels
        /// <summary>
        /// Determine if the loglevel is enabled for the following 
        /// </summary>
        /// <param name="loggerName">The name of the logger to check loglevel for.</param>
        /// <param name="logLevel">The log level.</param>
        /// <returns>True if the supplied log level is enabled.</returns>
        public static bool IsEnabled(string loggerName, LogLevel logLevel)
        {
            ILog logger = null;
            ExecuteRead(() => logger = Get(loggerName));

            if (logger == null)
                return false;

            return logger.IsEnabled(logLevel);
        }


        /// <summary>
        /// Gets a value indicating whether default logger is debug enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if default logger is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDebugEnabled
        {
            get { return IsEnabled("default", LogLevel.Debug); }
        }


        /// <summary>
        /// Gets a value indicating whether default logger is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if default logger is error enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsErrorEnabled
        {
            get { return IsEnabled("default", LogLevel.Error); }
        }


        /// <summary>
        /// Gets a value indicating whether default logger is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if default logger is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsFatalEnabled
        {
            get { return IsEnabled("default", LogLevel.Fatal); }
        }


        /// <summary>
        /// Gets a value indicating whether default logger is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if default logger is info enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInfoEnabled
        {
            get { return IsEnabled("default", LogLevel.Info); }
        }


        /// <summary>
        /// Gets a value indicating whether default logger is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if default logger is warn enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWarnEnabled
        {
            get { return IsEnabled("default", LogLevel.Warn); }
        }
        #endregion


        #region Non-Logging Methods
        /// <summary>
        /// Get a logger. 
        /// </summary>
        /// <typeparam name="T">Type of logger.</typeparam>
        /// <returns>Instance of logger.</returns>
        public static ILog GetNew<T>()
        {
            return GetNew<T>("default");
        }


        /// <summary>
        /// Get a new logger and associate with the type specified.
        /// </summary>
        /// <typeparam name="T">Type of logger.</typeparam>
        /// <param name="loggerName">Name of application.</param>
        /// <returns>Instance of logger.</returns>
        public static ILog GetNew<T>(string loggerName)
        {
            ILog logger = new LogInstance(loggerName, typeof(T));
            return logger;
        }


        /// <summary>
        /// Add a named logger.
        /// </summary>
        /// <param name="logger">Named logger to add.</param>
        public static void Add(ILogMulti logger)
        {
            // Add new logger.
            ExecuteWrite(() => _loggers[logger.Name] = logger);
        }


        /// <summary>
        /// Clear all the loggers and add only the Console logger to the 
        /// the default logger.
        /// </summary>
        public static void Clear()
        {
            ExecuteWrite(() =>
            {
                _loggers.Clear();
                _loggers["default"] = new LogMulti("default", new LogConsole());
            });
        }


        /// <summary>
        /// Get the default logger.
        /// </summary>
        public static ILogMulti Default
        {
            get { return Get("default"); }
        }


        /// <summary>
        /// Get the number of the loggers.
        /// </summary>
        public static int Count
        {
            get
            {
                int count = 0;
                ExecuteRead(() => count = _loggers.Count);
                return count;
            }
        }


        /// <summary>
        /// Get the named logger using the string indexer.
        /// </summary>
        /// <param name="name">Name of logger.</param>
        /// <returns>Logger with supplied name.</returns>
        public static ILogMulti Get(string name)
        {
            ILogMulti logger = null;
            ExecuteRead(() =>
            {
                if (!_loggers.ContainsKey(name))
                    return;

                logger = _loggers[name];
            });

            return logger;
        }


        /// <summary>
        /// Get the named logger using the string indexer.
        /// </summary>
        /// <param name="index">Index of logger.</param>
        /// <returns>Logger with supplied index.</returns>
        public static ILogMulti Get(int index)
        {
            ILogMulti logger = null;
            if (index < 0) return logger;

            ExecuteRead(() =>
            {
                if (index >= _loggers.Count )
                    return;

                logger = _loggers[index];
            });

            return logger;
        }

        
        /// <summary>
        /// Initialize the default logger.
        /// </summary>
        /// <param name="logger">Instance of logger to initialise.</param>
        public static void Init(ILogMulti logger)
        {
            ExecuteWrite(() => 
            {
                if (_loggers == null) _loggers = new DictionaryOrdered<string, ILogMulti>();
                _loggers["default"] = new LogMulti("default", new List<ILog>() { logger }); 
            });
        }


        /// <summary>
        /// Flushes the buffers.
        /// </summary>
        public static void Flush()
        {
            ExecuteRead( () => _loggers.ForEach(logger => logger.Value.Flush()));
        }


        /// <summary>
        /// Shutdown all loggers.
        /// </summary>
        public static void ShutDown()
        {
            // First flush.
            Flush();
            ExecuteRead(() => _loggers.ForEach(logger => logger.Value.ShutDown()));
        }


        /// <summary>
        /// Get all the log files.
        /// </summary>
        /// <returns>List with all log files.</returns>
        public static List<string> GetLogInfo()
        {
            List<string> logSummary = new List<string>();

            // Iterate through all the named loggers.
            ExecuteRead(() => _loggers.ForEach(logger =>
            {
                // Iterate through all the loggers in the LoggerMutli.
                for (int ndx = 0; ndx < logger.Value.Count; ndx++)
                {
                    // Get each logger by index position.
                    ILog log = logger.Value[ndx];

                    if (log is LogFile)
                    {
                        string info = "NAME:'" + logger.Key + "-" + log.Name + "'"
                                    + ", LEVEL: " + log.Level.ToString() 
                                    + ", PATH: " + ((LogFile)log).FilePath;
                        logSummary.Add(info);
                    }
                }
            }));
            return logSummary;
        }
        #endregion


        #region Synchronization Helper Methods
        /// <summary>
        /// Exectutes the action under a read operation after
        /// aquiring the reader lock.
        /// </summary>
        /// <param name="executor">Action to execute.</param>
        protected static void ExecuteRead(Action executor)
        {
            AcquireReaderLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute write action in Logger." + ex.Message);
            }
            finally
            {
                ReleaseReaderLock();
            }
        }


        /// <summary>
        /// Exectutes the action under a write operation after
        /// aquiring the writer lock.
        /// </summary>
        /// <param name="executor">Action to execute.</param>
        protected static void ExecuteWrite(Action executor)
        {
            AcquireWriterLock();
            try
            {
                executor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to execute write action in Logger." + ex.Message);
            }
            finally
            {
                ReleaseWriterLock();
            }
        }


        /// <summary>
        /// Gets the reader lock.
        /// </summary>
        protected static void AcquireReaderLock()
        {
            _readwriteLock.AcquireReaderLock(_lockMilliSecondsForRead);
        }


        /// <summary>
        /// Release the reader lock.
        /// </summary>
        protected static void ReleaseReaderLock()
        {
            _readwriteLock.ReleaseReaderLock();
        }


        /// <summary>
        /// Acquire the writer lock.
        /// </summary>
        protected static void AcquireWriterLock()
        {
            _readwriteLock.AcquireWriterLock(_lockMilliSecondsForWrite);
        }


        /// <summary>
        /// Release the writer lock.
        /// </summary>
        protected static void ReleaseWriterLock()
        {
            _readwriteLock.ReleaseWriterLock();
        }
        #endregion
    }
}
