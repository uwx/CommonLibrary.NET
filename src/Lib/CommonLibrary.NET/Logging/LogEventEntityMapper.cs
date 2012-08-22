using System;

namespace ComLib.Logging
{
    /// <summary>
    /// Creates a LogEventEntity from the contents of a LogEvent
    /// </summary>
    public class LogEventEntityMapper : IMapper<LogEvent, LogEventEntity>
    {
        public LogEventEntity MapFrom(LogEvent mapFrom)
        {
            LogEventEntity entity = new LogEventEntity();
            entity.UserName = mapFrom.UserName;
            entity.Computer = mapFrom.Computer;
            entity.Exception = mapFrom.Ex == null ? "" : mapFrom.Ex.Message;
            entity.LogLevel = mapFrom.Level;
            entity.CreateDate = mapFrom.CreateTime;
            entity.UpdateDate = mapFrom.CreateTime;
            return entity;                       
        }
    }

    /// <summary>
    /// Lightweight input/output mapping interface
    /// </summary>
    /// <typeparam name="T">The type to be mapped from</typeparam>
    /// <typeparam name="TK">The type to be mapped to</typeparam>
    public interface IMapper<T, TK>
    {
        TK MapFrom(T mapFrom);
    }    
}