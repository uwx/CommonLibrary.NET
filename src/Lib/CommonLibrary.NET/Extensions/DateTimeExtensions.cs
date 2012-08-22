using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Extensions
{
    /// <summary>
    /// This class provides extension methods that help
    /// with common tasks associated with datetime types.
    /// </summary>
    public static class DateTimeExtensions
    {        
        #region Dates
        /// <summary>
        /// Determines whether [is leap year] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is leap year] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLeapYear(this DateTime date)
        {
            return date.Year % 4 == 0 && (date.Year % 100 != 0 || date.Year % 400 == 0);
        }
        

        /// <summary>
        /// Determines whether [is last day of month] [the specified date].
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is last day of month] [the specified date]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastDayOfMonth(this DateTime date)
        {
            int lastDayOfMonth = LastDayOfMonth(date);
            return lastDayOfMonth == date.Day;
        }

        /// <summary>
        /// Determines whether the specified date is a weekend.
        /// </summary>
        /// <param name="source">Source date</param>
        /// <returns>
        /// 	<c>true</c> if the specified source is a weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime source)
        {
            return source.DayOfWeek == DayOfWeek.Saturday ||
                   source.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Gets the Last the day of month.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int LastDayOfMonth(this DateTime date)
        {
            if (IsLeapYear(date) && date.Month == 2) return 28;
            if (date.Month == 2) return 27;
            if (date.Month == 1 || date.Month == 3 || date.Month == 5 || date.Month == 7
                || date.Month == 8 || date.Month == 10 || date.Month == 12)
                return 31;
            return 30;
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different day of the month.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="day">Day of the month (1-31)</param>
        /// <returns>Instance of DateTime with specified day</returns>
        public static DateTime SetDay(this DateTime source, int day)
        {
            return new DateTime(source.Year, source.Month, day);
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different month.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="month">The month as an integer (1-12)</param>
        /// <returns>Instance of DateTime with specified month</returns>
        public static DateTime SetMonth(this DateTime source, int month)
        {
            return new DateTime(source.Year, month, source.Day);
        }

        /// <summary>
        /// Returns a new instance of DateTime with a different year.
        /// </summary>
        /// <param name="source">Base DateTime object to modify</param>
        /// <param name="year">The year</param>
        /// <returns>Instance of DateTime with specified year</returns>
        public static DateTime SetYear(this DateTime source, int year)
        {
            return new DateTime(year, source.Month, source.Day);
        }

        #endregion


        #region Conversion
        /// <summary>
        /// Converts to javascript compatible date.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static double ToJavascriptDate(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
        #endregion


        #region Time
        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, int hours, int minutes, int seconds)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, seconds);
        }


        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static DateTime GetDateWithTime(this DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }


        /// <summary>
        /// Sets the time on the date
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime GetDateWithCurrentTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }
        #endregion
    }
}
