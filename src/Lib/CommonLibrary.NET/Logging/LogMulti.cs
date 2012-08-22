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
    /// Logging class that will log to multiple loggers.
    /// </summary>
    public class LogMulti : LogBase, ILogMulti
    {
        private DictionaryOrdered<string, ILog> _loggers;
        private LogLevel _lowestLevel = LogLevel.Debug;


        /// <summary>
        /// Initalize a logger.
        /// </summary>
        /// <param name="logger">Logging object.</param>
        /// <param name="name">Name of application.</param>
        public LogMulti(string name, ILog logger) : base(typeof(LogMulti).FullName)
        {
            Init(name, new List<ILog>() { logger });
        }


        /// <summary>
        /// Initalize multiple loggers.
        /// </summary>
        /// <param name="name">Name of application.</param>
        /// <param name="loggers">List of logging objects.</param>
        public LogMulti(string name, IList<ILog> loggers) : base(typeof(LogMulti).FullName)
        {
            Init(name, loggers);
        }


        /// <summary>
        /// Initialize with loggers.
        /// </summary>
        /// <param name="name">Name of application.</param>
        /// <param name="loggers">List of logging objects.</param>
        public void Init(string name, IList<ILog> loggers)
        {
            this.Name = name;
            _loggers = new DictionaryOrdered<string, ILog>();
            loggers.ForEach(logger => _loggers.Add(logger.Name, logger));
            ActivateOptions();
        }


        /// <summary>
        /// Log the event to each of the loggers.
        /// </summary>
        /// <param name="logEvent">Event to log.</param>
        public override void Log(LogEvent logEvent)
        {
            // Log using the readerlock.
            ExecuteRead(() => _loggers.ForEach(logger => logger.Value.Log(logEvent)));
        }


        /// <summary>
        /// Append to the chain of loggers.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public void Append(ILog logger)
        {
            // Add to loggers.
            ExecuteWrite(() => _loggers.Add(logger.Name, logger) );
        }



        /// <summary>
        /// Replaces all the existing loggers w/ the supplied logger.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public void Replace(ILog logger)
        {
            Clear();
            Append(logger);
        }


        /// <summary>
        /// Get the number of loggers that are part of this loggerMulti.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                ExecuteRead(() => count = _loggers.Count );
                return count;
            }
        }


        /// <summary>
        /// Clear all the exiting loggers and only add the console logger.
        /// </summary>
        public void Clear()
        {
            ExecuteWrite(() =>
            {
                _loggers.Clear();
                _lowestLevel = LogLevel.Message;
                _loggers.Add("console", new LogConsole());
            });
        }


        /// <summary>
        /// Get a logger by it's name.
        /// </summary>
        /// <param name="loggerName">Name of logger.</param>
        /// <returns>Logger corresponding to supplied name.</returns>
        public override ILog this[string loggerName]
        {
            get
            {
                ILog logger = null;
                ExecuteRead( () =>
                {
                    if (!_loggers.ContainsKey(loggerName))
                        return;

                    logger = _loggers[loggerName];
                });
                return logger;
            }
        }


        /// <summary>
        /// Get a logger by it's index.
        /// </summary>
        /// <param name="logIndex">Index of logger.</param>
        /// <returns>Logger corresponding to supplied index.</returns>
        public override ILog this[int logIndex]
        {
            get
            {
                ILog logger = null;     
                if(logIndex < 0 ) return null;

                ExecuteRead(() =>
                {
                    if (logIndex >= _loggers.Count)
                        return;

                    logger = _loggers[logIndex];
                });
                return logger;
            }
        }


        /// <summary>
        /// Get the level. ( This is the lowest level of all the loggers. ).
        /// </summary>
        public override LogLevel Level
        {
            get { return _lowestLevel; }
            set
            {
                ExecuteWrite(() =>
                {
                    _loggers.ForEach(logger => logger.Value.Level = value);
                    _lowestLevel = value;
                });
            }
        }


        /// <summary>
        /// Whether or not the level specified is enabled.
        /// </summary>
        /// <param name="level">Level to check.</param>
        /// <returns>True if the supplied level is enabled.</returns>
        public override bool IsEnabled(LogLevel level)
        {
            return _lowestLevel <= level;
        }


        /// <summary>
        /// Flushes the buffers.
        /// </summary>
        public override void Flush()
        {
            ExecuteRead(() => { _loggers.ForEach(logger => logger.Value.Flush()); } );
        }


        /// <summary>
        /// Shutdown all loggers.
        /// </summary>
        public override void ShutDown()
        {
            ExecuteRead(() => { _loggers.ForEach(logger => logger.Value.ShutDown()); });
        }


        #region Helper Methods
        /// <summary>
        /// Determine the lowest level by getting the lowest level
        /// of all the loggers.
        /// </summary>
        public void ActivateOptions()
        {
            // Get the lowest level from all the loggers.
            ExecuteRead(() =>
            {
                LogLevel level = LogLevel.Fatal;
                for(int ndx = 0; ndx < _loggers.Count; ndx++)
                {
                    ILog logger = _loggers[ndx];
                    if (logger.Level <= level) level = logger.Level;
                }
                _lowestLevel = level;
            });
        }
        #endregion


    }
}
