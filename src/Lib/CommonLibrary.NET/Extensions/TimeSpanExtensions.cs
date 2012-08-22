using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Extensions
{
    /// <summary>
    /// This static class provides several common extensions for TimeSpan.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Determines whether [is midnight exactly] [the specified t].
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>
        /// 	<c>true</c> if [is midnight exactly] [the specified t]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMidnightExactly(this TimeSpan t)
        {
            return t.Hours == 0 && t.Minutes == 0 && t.Seconds == 0;
        }


        /// <summary>
        /// Get simple time format
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToMilitaryString(this TimeSpan t)
        {
            string time = t.Hours + ":" + t.Minutes + ":" + t.Seconds;

            if (t.Days > 0)
                time = t.Days + "dys " + time;

            return time;
        }


        /// <summary>
        /// Returns an integer representing mii
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int ToMilitaryInt(this TimeSpan t)
        {
            int time = t.Hours * 100;
            time += t.Minutes;
            return time;
        }
    }
}
