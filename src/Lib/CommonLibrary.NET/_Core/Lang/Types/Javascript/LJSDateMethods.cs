using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Types;
using ComLib.Lang.Helpers;
using ComLib.Lang.Parsing;
// </lang:using>

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
        public override void Init()
        {
            this.AddMethod("getDate", 			"GetDate", 				typeof(double), "Returns the day of the month (from 1-31)");
            this.AddMethod("getDay", 	 		"GetDay", 				typeof(double), "Returns the day of the week (from 0-6)");
            this.AddMethod("getFullYear", 		"GetFullYear", 			typeof(double), "Returns the year (four digits)");
            this.AddMethod("getHours", 			"GetHours", 			typeof(double), "Returns the hour (from 0-23)");
            this.AddMethod("getMilliseconds", 	"GetMilliseconds", 		typeof(double), "Returns the milliseconds (from 0-999)");
            this.AddMethod("getMinutes", 		"GetMinutes", 			typeof(double), "Returns the minutes (from 0-59)");
            this.AddMethod("getMonth", 			"GetMonth", 			typeof(double), "Returns the month (from 0-11)");
            this.AddMethod("getSeconds", 		"GetSeconds", 		 	typeof(double), "Returns the seconds (from 0-59)");
            this.AddMethod("getTime", 			"GetTime", 			 	typeof(double), "Returns the number of milliseconds since midnight Jan 1 1970");
            this.AddMethod("getTimezoneOffset", "GetTimezoneOffset", 	typeof(double), "Returns the time difference between UTC time and local time in minutes");
            this.AddMethod("getUTCDate", 		"GetUTCDate", 			typeof(double), "Returns the day of the month according to universal time (from 1-31)");
            this.AddMethod("getUTCDay", 		"GetUTCDay",			typeof(double), "Returns the day of the week according to universal time (from 0-6)");
            this.AddMethod("getUTCFullYear", 	"GetUTCFullYear", 		typeof(double), "Returns the year according to universal time (four digits)");
            this.AddMethod("getUTCHours", 		"GetUTCHours", 			typeof(double), "Returns the hour according to universal time (from 0-23)");
            this.AddMethod("getUTCMilliseconds","GetUTCMilliseconds", 	typeof(double), "Returns the milliseconds according to universal time (from 0-999)");
            this.AddMethod("getUTCMinutes", 	"GetUTCMinutes", 		typeof(double), "Returns the minutes according to universal time (from 0-59)");
            this.AddMethod("getUTCMonth", 		"GetUTCMonth", 			typeof(double), "Returns the month according to universal time (from 0-11)");
            this.AddMethod("getUTCSeconds", 	"GetUTCSeconds", 		typeof(double), "Returns the seconds according to universal time (from 0-59)");

            this.AddMethod("setDate", 			"SetDate", 				null,           "Sets the day of the month of a date object");
            this.AddMethod("setFullYear", 		"SetFullYear", 			null,           "Sets the year (four digits) of a date object");
            this.AddMethod("setHours", 			"SetHours", 			null,           "Sets the hour of a date object");
            this.AddMethod("setMilliseconds", 	"SetMilliseconds", 		null,           "Sets the milliseconds of a date object");
            this.AddMethod("setMinutes", 		"SetMinutes", 			null,           "Set the minutes of a date object");
            this.AddMethod("setMonth", 			"SetMonth", 			null,           "Sets the month of a date object");
            this.AddMethod("setSeconds", 		"SetSeconds", 			null,           "Sets the seconds of a date object");
            this.AddMethod("setTime", 			"SetTime", 				null,           "Sets a date and time by adding or subtracting a specified number of milliseconds to/from midnight January 1, 1970");
            this.AddMethod("setUTCDate", 		"SetUTCDate", 			null,           "Sets the day of the month of a date object, according to universal time");
            this.AddMethod("setUTCFullYear", 	"SetUTCFullYear", 		null,           "Sets the year of a date object, according to universal time (four digits)");
            this.AddMethod("setUTCHours", 		"SetUTCHours", 			null,           "Sets the hour of a date object, according to universal time");
            this.AddMethod("setUTCMilliseconds","SetUTCMilliseconds", 	null,           "Sets the milliseconds of a date object, according to universal time");
            this.AddMethod("setUTCMinutes", 	"SetUTCMinutes", 		null,           "Set the minutes of a date object, according to universal time");
            this.AddMethod("setUTCMonth", 		"SetUTCMonth", 			null,           "Sets the month of a date object, according to universal time");
            this.AddMethod("setUTCSeconds", 	"SetUTCSeconds", 		null,           "Set the seconds of a date object, according to universal time");
        }


        /// <summary>
        /// The raw datetime.
        /// </summary>
        public DateTime Raw;


        #region Javascript API methods
        internal int      GetDate              (DateTime date) { return date.Day;                                                      }      	
        internal int      GetDay               (DateTime date) { return (int)date.DayOfWeek;                                           }
        internal int      GetFullYear          (DateTime date) { return date.Year;                                                     }
        internal int      GetHours             (DateTime date) { return date.Hour;                                                     }
        internal int      GetMilliseconds      (DateTime date) { return date.Millisecond;                                              }
        internal int      GetMinutes           (DateTime date) { return date.Minute;		                                           }
        internal int      GetMonth             (DateTime date) { return date.Month;                                                    }
        internal int      GetSeconds           (DateTime date) { return date.Second;                                                   }
        internal int      GetUTCDate           (DateTime date) { return date.ToUniversalTime().Day;                                    }
        internal int      GetUTCDay            (DateTime date) { return (int)date.ToUniversalTime().DayOfWeek;                         }
        internal int      GetUTCFullYear       (DateTime date) { return date.ToUniversalTime().Year;                                   }
        internal int      GetUTCHours          (DateTime date) { return date.ToUniversalTime().Hour;                                   }
        internal int      GetUTCMilliseconds   (DateTime date) { return date.ToUniversalTime().Millisecond;                            }  
        internal int      GetUTCMinutes        (DateTime date) { return date.ToUniversalTime().Minute;                                 }
        internal int      GetUTCMonth          (DateTime date) { return date.ToUniversalTime().Month;                                  }
        internal int      GetUTCSeconds        (DateTime date) { return date.ToUniversalTime().Second;                                 }
        internal string   ToDateString         (DateTime date) { return date.ToString("ddd MMM dd yyyy");                              }
        internal string   ToLocaleDateString   (DateTime date) { return date.ToLocalTime().ToString("ddd MMM dd yyyy");                }
        internal string   ToLocaleTimeString   (DateTime date) { return date.ToLocalTime().ToString("hh mm ss");                       }
        internal string   ToLocaleString       (DateTime date) { return date.ToLocalTime().ToString("ddd MMM dd yyyy hh mm ss");       }
        internal string   ToString             (DateTime date) { return date.ToString("ddd MMM dd yyyy hh mm ss");                     }
        internal string   ToTimeString         (DateTime date) { return date.ToString("hh mm ss");                                     }
        internal string   ToUTCString          (DateTime date) { return date.ToUniversalTime().ToString("ddd MMM dd yyyy hh mm ss");   }



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

            DateTime result = DateTime.MinValue;

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
            int[] args = new int[parameters.Length];
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

        
        private void SetFullYear(DateTime date, int year, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Local, year, month, day);
        }


        private void SetMonth(DateTime date, DateTimeKind kind, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Local, -1, month, day);
        }


        private void SetDate(DateTime date, DateTimeKind kind, int day)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, day);
        }


        private void SetHours(DateTime date, DateTimeKind kind, int hours, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, hours, minutes, seconds, milliseconds);
        }


        private void SetMinutes(DateTime date, DateTimeKind kind, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, minutes, seconds, milliseconds);
        }


        private void SetSeconds(DateTime date, DateTimeKind kind, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, -1, seconds, milliseconds);
        }


        private void SetMilliseconds(DateTime date, DateTimeKind kind, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Local, -1, -1, -1, -1, -1, -1, milliseconds);
        }


        private void SetUTCFullYear(DateTime date, int year, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, year, month, day);
        }


        private void SetUTCMonth(DateTime date, DateTimeKind kind, int month, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, month, day);
        }


        private void SetUTCDate(DateTime date, DateTimeKind kind, int day)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, day);
        }


        private void SetUTCHours(DateTime date, DateTimeKind kind, int hours, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, hours, minutes, seconds, milliseconds);
        }


        private void SetUTCMinutes(DateTime date, DateTimeKind kind, int minutes, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, minutes, seconds, milliseconds);
        }


        private void SetUTCSeconds(DateTime date, DateTimeKind kind, int seconds, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, -1, seconds, milliseconds);
        }


        private void SetUTCMilliseconds(DateTime date, DateTimeKind kind, int milliseconds)
        {
            SetDateTime(date, DateTimeKind.Utc, -1, -1, -1, -1, -1, -1, milliseconds);
        }


        private static void SetDateTime(DateTime target, DateTimeKind kind, int year = -1, int month = -1, int day = -1,
            int hour = -1, int minute = -1, int second = -1, int millisecond = -1)
        {
            DateTime dt = kind == DateTimeKind.Utc ? target.ToUniversalTime() : target;
            year = year == -1 ? dt.Year : year;
            month = month == -1 ? dt.Month : month;
            day = day == -1 ? dt.Day : day;
            hour = hour == -1 ? dt.Hour : hour;
            minute = minute == -1 ? dt.Minute : minute;
            second = second == -1 ? dt.Second : second;
            millisecond = millisecond == -1 ? dt.Millisecond : millisecond;

            var finalDateTime = new DateTime(year, month, day, hour, minute, second, millisecond, kind);
            //_context.Memory.SetValue(_varName, finalDateTime);
        }
        #endregion
    }
}
