using System.Collections.Generic;
using System.Data;
using System.Data.Linq;

using ComLib.Entities;
using ComLib.Data;


namespace ComLib.Logging
{
    /// <summary>
    /// This class represents a logger that can log
    /// messages to a database.
    /// </summary>
    public class LogDatabase : LogBase
    {
        private const int FlushInterval = 5;
        // private readonly IDbConnection _connection;
        private IList<LogEventEntity> _uncommitedList;
        private IRepository<LogEventEntity> _repo;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appName">Name of application.</param>
        /// <param name="name">Logger name.</param>
        /// <param name="connectionString">Connection string to a database that contains the log events table</param>
        /// <param name="logLevel">Log level to use.</param>
        public LogDatabase(string appName, string name, string connectionString, LogLevel logLevel)
        {
            // Initialize with real Database using the RepositoryLinq2Sql.
            var conInfo = new ConnectionInfo(connectionString);
            _repo = new RepositoryLinq2Sql<LogEventEntity>(conInfo, new Database());
            Init(appName, name, logLevel);
        }


        /// <summary>
        /// Constructor taking the IRepository that will handle CRUD operations
        /// of log messages to the Database.
        /// Also, the repo provided can be a FAKE( In-Memory ) implementation which
        /// is useful in testing.
        /// </summary>
        /// <param name="appName">Name of application.</param>
        /// <param name="name">Logger name.</param>
        /// <param name="logLevel">Log level to use.</param>
        /// <param name="repo"></param>
        public LogDatabase(string appName, string name, IRepository<LogEventEntity> repo, LogLevel logLevel)
        {
            _repo = repo;
            Init(appName, name, logLevel);    
        }


        private void Init(string appName, string loggerName, LogLevel logLevel)
        {
            this.Name = string.IsNullOrEmpty(loggerName) ? "db" : loggerName;
            var settings = new LogSettings();
            settings.AppName = appName;
            settings.Level = logLevel;
            Settings = settings;
            //_connection = connection;
            _uncommitedList = new List<LogEventEntity>();
        }


        /// <summary>
        /// Adds a log event to a internal list then runs a flush check.
        /// </summary>
        /// <param name="logEvent">Event to log.</param>
        public override void Log(LogEvent logEvent)
        {
            LogEventEntity eventEntity = new LogEventEntityMapper().MapFrom(logEvent);
            eventEntity.Application = Settings.AppName;
            _uncommitedList.Add(eventEntity);
            FlushCheck();
        }


        /// <summary>
        /// Persists a batch of log events to a database table.
        /// </summary>
        public override void Flush()
        {
            lock (_uncommitedList)
            {
                Try.Catch(() =>
                {
                    // Let the Repository take care of CRUD actions.
                    _repo.Create(_uncommitedList);
        
                    // Clear the internal uncomitted log event list
                    _uncommitedList.Clear();
                });
            }
        }


        /// <summary>
        /// Checks if the internal list has reached the flush interval.
        /// </summary>
        private void FlushCheck()
        {
            // If the uncommitted list has reached the flush interval then flush the log entries.
            if (_uncommitedList.Count.Equals(FlushInterval))
            {
                Flush();
            }
        }
    }
}