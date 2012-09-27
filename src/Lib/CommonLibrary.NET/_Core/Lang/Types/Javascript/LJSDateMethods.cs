using System;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array type.
    /// </summary>
    public class LJSDateMethods : LTypeMethods
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LJSDateMethods()
        {
            DataType = typeof(LDate);

            AddMethod("getDate", 			"GetDate", 				typeof(double), "Returns the day of the month (from 1-31)");
            AddMethod("getDay", 	 		"GetDay", 				typeof(double), "Returns the day of the week (from 0-6)");
            AddMethod("getFullYear", 		"GetFullYear", 			typeof(double), "Returns the year (four digits)");
            AddMethod("getHours", 			"GetHours", 			typeof(double), "Returns the hour (from 0-23)");
            AddMethod("getMilliseconds", 	"GetMilliseconds", 		typeof(double), "Returns the milliseconds (from 0-999)");
            AddMethod("getMinutes", 		"GetMinutes", 			typeof(double), "Returns the minutes (from 0-59)");
            AddMethod("getMonth", 			"GetMonth", 			typeof(double), "Returns the month (from 0-11)");
            AddMethod("getSeconds", 		"GetSeconds", 		 	typeof(double), "Returns the seconds (from 0-59)");
            AddMethod("getTime", 			"GetTime", 			 	typeof(double), "Returns the number of milliseconds since midnight Jan 1 1970");
            AddMethod("getTimezoneOffset",  "GetTimezoneOffset", 	typeof(double), "Returns the time difference between UTC time and local time in minutes");
            AddMethod("getUTCDate", 		"GetUtcDate", 			typeof(double), "Returns the day of the month according to universal time (from 1-31)");
            AddMethod("getUTCDay", 		    "GetUtcDay",			typeof(double), "Returns the day of the week according to universal time (from 0-6)");
            AddMethod("getUTCFullYear", 	"GetUtcFullYear", 		typeof(double), "Returns the year according to universal time (four digits)");
            AddMethod("getUTCHours", 		"GetUtcHours", 			typeof(double), "Returns the hour according to universal time (from 0-23)");
            AddMethod("getUTCMilliseconds", "GetUtcMilliseconds", 	typeof(double), "Returns the milliseconds according to universal time (from 0-999)");
            AddMethod("getUTCMinutes", 	    "GetUtcMinutes", 		typeof(double), "Returns the minutes according to universal time (from 0-59)");
            AddMethod("getUTCMonth", 		"GetUtcMonth", 			typeof(double), "Returns the month according to universal time (from 0-11)");
            AddMethod("getUTCSeconds", 	    "GetUtcSeconds", 		typeof(double), "Returns the seconds according to universal time (from 0-59)");

            AddMethod("setDate", 			"SetDate", 				null,           "Sets the day of the month of a date object");
            AddMethod("setFullYear", 		"SetFullYear", 			null,           "Sets the year (four digits) of a date object");
            AddMethod("setHours", 			"SetHours", 			null,           "Sets the hour of a date object");
            AddMethod("setMilliseconds", 	"SetMilliseconds", 		null,           "Sets the milliseconds of a date object");
            AddMethod("setMinutes", 		"SetMinutes", 			null,           "Set the minutes of a date object");
            AddMethod("setMonth", 			"SetMonth", 			null,           "Sets the month of a date object");
            AddMethod("setSeconds", 		"SetSeconds", 			null,           "Sets the seconds of a date object");
            AddMethod("setTime", 			"SetTime", 				null,           "Sets a date and time by adding or subtracting a specified number of milliseconds to/from midnight January 1, 1970");
            AddMethod("setUTCDate", 		"SetUtcDate", 			null,           "Sets the day of the month of a date object, according to universal time");
            AddMethod("setUTCFullYear", 	"SetUtcFullYear", 		null,           "Sets the year of a date object, according to universal time (four digits)");
            AddMethod("setUTCHours", 		"SetUtcHours", 			null,           "Sets the hour of a date object, according to universal time");
            AddMethod("setUTCMilliseconds", "SetUtcMilliseconds", 	null,           "Sets the milliseconds of a date object, according to universal time");
            AddMethod("setUTCMinutes", 	    "SetUtcMinutes", 		null,           "Set the minutes of a date object, according to universal time");
            AddMethod("setUTCMonth", 		"SetUtcMonth", 			null,           "Sets the month of a date object, according to universal time");
            AddMethod("setUTCSeconds",  	"SetUtcSeconds", 		null,           "Set the seconds of a date object, according to universal time");
        }


        /// <summary>
        /// The raw datetime.
        /// </summary>
        public DateTime Raw;


        #region Javascript API methods
        internal int      GetDate              (LDate date) { return date.Raw.Day;                                                      }      	
        internal int      GetDay               (LDate date) { return (int)date.Raw.DayOfWeek;                                           }
        internal int      GetFullYear          (LDate date) { return date.Raw.Year;                                                     }
        internal int      GetHours             (LDate date) { return date.Raw.Hour;                                                     }
        internal int      GetMilliseconds      (LDate date) { return date.Raw.Millisecond;                                              }
        internal int      GetMinutes           (LDate date) { return date.Raw.Minute;		                                           }
        internal int      GetMonth             (LDate date) { return date.Raw.Month;                                                    }
        internal int      GetSeconds           (LDate date) { return date.Raw.Second;                                                   }
        internal int      GetUtcDate           (LDate date) { return date.Raw.ToUniversalTime().Day;                                    }
        internal int      GetUtcDay            (LDate date) { return (int)date.Raw.ToUniversalTime().DayOfWeek;                         }
        internal int      GetUtcFullYear       (LDate date) { return date.Raw.ToUniversalTime().Year;                                   }
        internal int      GetUtcHours          (LDate date) { return date.Raw.ToUniversalTime().Hour;                                   }
        internal int      GetUtcMilliseconds   (LDate date) { return date.Raw.ToUniversalTime().Millisecond;                            }  
        internal int      GetUtcMinutes        (LDate date) { return date.Raw.ToUniversalTime().Minute;                                 }
        internal int      GetUtcMonth          (LDate date) { return date.Raw.ToUniversalTime().Month;                                  }
        internal int      GetUtcSeconds        (LDate date) { return date.Raw.ToUniversalTime().Second;                                 }
        internal string   ToDateString         (LDate date) { return date.Raw.ToString("ddd MMM dd yyyy");                              }
        internal string   ToLocaleDateString   (LDate date) { return date.Raw.ToLocalTime().ToString("ddd MMM dd yyyy");                }
        internal string   ToLocaleTimeString   (LDate date) { return date.Raw.ToLocalTime().ToString("hh mm ss");                       }
        internal string   ToLocaleString       (LDate date) { return date.Raw.ToLocalTime().ToString("ddd MMM dd yyyy hh mm ss");       }
        internal string   ToString             (LDate date) { return date.Raw.ToString("ddd MMM dd yyyy hh mm ss");                     }
        internal string   ToTimeString         (LDate date) { return date.Raw.ToString("hh mm ss");                                     }
        internal string   ToUtcString          (LDate date) { return date.Raw.ToUniversalTime().ToString("ddd MMM dd yyyy hh mm ss");   }



        /// <summary>
        /// Can create from the paramelist expressions supplied.
        /// </summary>
        /// <returns></returns>
        public static bool CanCreateFrom(int paramCount)
        {
            if (paramCount != 0 && paramCount != 1 && paramCount != 3 && paramCount != 6)
                return false;
            return true;
        }


        /// <summary>
        /// Creates a datetime from the parameters supplied.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DateTime CreateFrom(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return DateTime.Now;
            }

            var result = DateTime.MinValue;

            // Case 1: From string
            if (parameters.Length == 1 && parameters[0] is string)
            {
                result = DateTime.Parse((string)parameters[0]);
                return result;
            }

            // Case 2: From Date
            if (parameters.Length == 1 && parameters[0] is DateTime)
            {
                var d = (DateTime)parameters[0];
                result = new DateTime(d.Ticks);
                return result;
            }

            // Convert all parameters to int            
            var args = new int[parameters.Length];
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                args[ndx] = Convert.ToInt32(parameters[ndx]);
            }

            // Case 3: 3 parameters month, day, year
            if (parameters.Length == 3)
                return new DateTime(args[0], args[1], args[2]);

            // Case 4: 6 parameters
            if (parameters.Length == 6)
                return new DateTime(args[0], args[1], args[2], args[3], args[4], args[5]);

            // TODO: Need to handle this better.
            return DateTime.MinValue;
        }

        
        /// <summary>
        /// Sets the full year on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="year">The year to set</param>
        /// <param name="month">The month to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetFullYear(LDate date, int year, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Local, year, month, day);
        }


        /// <summary>
        /// Sets the month on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="month">The month to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetMonth(LDate date, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Local, -1, month, day);
        }


        /// <summary>
        /// Sets the day of the month on the date
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetDate(LDate date,int day)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, day);
        }


        /// <summary>
        /// Sets the hours of the date
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="hours">The hours to set</param>
        /// <param name="minutes">The minutes to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetHours(LDate date, int hours, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, hours, minutes, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the minutes on the date
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="minutes">The minutes to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetMinutes(LDate date, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, minutes, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the seconds on the date
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetSeconds(LDate date, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, -1, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the milliseconds on the date
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetMilliseconds(LDate date, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, -1, -1, milliseconds);
        }


        /// <summary>
        /// Sets the full year on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="year">The year to set</param>
        /// <param name="month">The month to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetUtcFullYear(LDate date, int year, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, year, month, day);
        }


        /// <summary>
        /// Sets the month on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="month">The month to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetUtcMonth(LDate date, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, month, day);
        }


        /// <summary>
        /// Sets the day of the month on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="day">The day of the month to set</param>
        public void SetUtcDate(LDate date, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, day);
        }


        /// <summary>
        /// Sets the hours on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="hours">The hours to set</param>
        /// <param name="minutes">The minutes to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetUtcHours(LDate date, int hours, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, hours, minutes, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the minutes on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="minutes">The minutes to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetUtcMinutes(LDate date, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, minutes, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the seconds on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="seconds">The seconds to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetUtcSeconds(LDate date, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, -1, seconds, milliseconds);
        }


        /// <summary>
        /// Sets the milliseconds on the date.
        /// </summary>
        /// <param name="date">The LDate to set</param>
        /// <param name="milliseconds">The milliseconds to set</param>
        public void SetUtcMilliseconds(LDate date, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, -1, -1, milliseconds);
        }


        private static void SetDateTime(LDate date, DateTimeKind kind, int year = -1, int month = -1, int day = -1,
            int hour = -1, int minute = -1, int second = -1, int millisecond = -1)
        {
            var target = date.Raw;
            DateTime dt = kind == DateTimeKind.Utc ? target.ToUniversalTime() : target;
            year = year == -1 ? dt.Year : year;
            month = month == -1 ? dt.Month : month;
            day = day == -1 ? dt.Day : day;
            hour = hour == -1 ? dt.Hour : hour;
            minute = minute == -1 ? dt.Minute : minute;
            second = second == -1 ? dt.Second : second;
            millisecond = millisecond == -1 ? dt.Millisecond : millisecond;

            var finalDateTime = new DateTime(year, month, day, hour, minute, second, millisecond, kind);
            date.Raw = finalDateTime;
        }
        #endregion
    }
}
