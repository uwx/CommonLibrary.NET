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


namespace ComLib.Logging
{
    /// <summary>
    /// Interface for a logger that represents a chain(multiple) loggers.
    /// </summary>
    public interface ILogMulti : ILog
    {
        /// <summary>
        /// Get a logger by it's name.
        /// </summary>
        /// <param name="loggerName">Name of logger to retrieve.</param>
        ILog this[string loggerName] { get; }


        /// <summary>
        /// Get a logger by it's index position.
        /// </summary>
        /// <param name="index">Index of logger to retrieve.</param>
        ILog this[int index] { get; }


        /// <summary>
        /// Append another logger to the chain of loggers.
        /// </summary>
        /// <param name="logger">Instance of logger to append.</param>
        void Append(ILog logger);

        
        /// <summary>
        /// Replaces all the existing loggers w/ the supplied logger.
        /// </summary>
        /// <param name="logger">Instance of logger to use.</param>
        void Replace(ILog logger);


        /// <summary>
        /// Clear all the chained loggers.
        /// </summary>
        void Clear();
                

        /// <summary>
        /// Get the number of loggers that are in here.
        /// </summary>
        int Count { get; }
    }



    /// <summary>
    /// Simple interface for logging information.
    /// This extends the common Log4net interface by 
    /// 
    /// 1. Taking additional argument as an object array
    /// 2. Exposing a simple Log method that takes in the loglevel.
    /// 
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Get the name of the logger.
        /// </summary>
        string Name { get; }

        
        /// <summary>
        /// Get / set the loglevel.
        /// </summary>
        LogLevel Level { get; set; }


        /// <summary>
        /// Logs the specified level.
        /// </summary>
        void Log(LogEvent logEvent);


        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(object message);


        /// <summary>
        /// Logs a warning message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Warn(object message, Exception exception);


        /// <summary>
        /// Logs a warning message with exception and additional arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">Additional arguments.</param>
        void Warn(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Error message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(object message);


        /// <summary>
        /// Logs a Error message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(object message, Exception exception);
        
        
        /// <summary>
        /// Logs an error message with the exception additional arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Error(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(object message);


        /// <summary>
        /// Logs a Debug message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Debug(object message, Exception exception);
        
        
        /// <summary>
        /// Logs a debug message with the exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Debug(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Fatal message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(object message);


        /// <summary>
        /// Logs a Fatal message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Fatal(object message, Exception exception);
        
        
        /// <summary>
        /// Logs a fatal message with exception and arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Fatal(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Info message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(object message);


        /// <summary>
        /// Logs a Info message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Info(object message, Exception exception);


        /// <summary>
        /// Logs a info message with the arguments.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Info(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Message(object message);


        /// <summary>
        /// Logs a Message with exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Message(object message, Exception exception);
        
        
        /// <summary>
        /// Messages should always get logged.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The args.</param>
        void Message(object message, Exception exception, object[] args);


        /// <summary>
        /// Logs a Message.
        /// </summary>
        /// <param name="level">Logging level of message.</param>
        /// <param name="message">The message.</param>
        void Log(LogLevel level, object message);


        /// <summary>
        /// Logs a Message with exception.
        /// </summary>
        /// <param name="level">Logging level of message.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Log(LogLevel level, object message, Exception exception);


        /// <summary>
        /// Messages should always get logged.
        /// </summary>
        /// <param name="level">Logging level of message.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="args">The args.</param>        
        void Log(LogLevel level, object message, System.Exception ex, object[] args);


        /// <summary>
        /// Is the level enabled.
        /// </summary>
        /// <param name="level">Logging level to check for.</param>
        /// <returns>True if specified logging level is enabled.</returns>
        bool IsEnabled(LogLevel level);


        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsDebugEnabled { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsErrorEnabled { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsFatalEnabled { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is info enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsInfoEnabled { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsWarnEnabled { get; }


        /// <summary>
        /// Builds a log event from the parameters supplied.
        /// </summary>
        /// <param name="level">Level of message.</param>
        /// <param name="message">Message to log.</param>
        /// <param name="ex">Exception to log.</param>
        /// <param name="args">Arguments to use.</param>
        /// <returns>Created log event.</returns>
        LogEvent BuildLogEvent(LogLevel level, object message, System.Exception ex, object[] args);


        /// <summary>
        /// Flushes the buffers.
        /// </summary>
        void Flush();


        /// <summary>
        /// Shutdown the logger.
        /// </summary>
        void ShutDown();
    }
}
