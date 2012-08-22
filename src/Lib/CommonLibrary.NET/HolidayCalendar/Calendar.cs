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

namespace ComLib.Calendars
{    

    /// <summary>
    /// Provides static access to all the <see cref="ICalendar"/> functionality using the Holiday calendar service initialized.
    /// </summary>
    public class Calendar
    {
        private static ICalendar _provider;


        static Calendar()
        {
            _provider = new CalendarService();
            List<Holiday> holidays = CalendarUtils.GetUnitedStatesHolidays();
            CalanderDao dao = new CalanderDao("usa-holidays", holidays);
            _provider.Init("usa-holidays", dao, DateTime.Today.Year - 1, DateTime.Today.Year + 2);
        }


        #region IHolidayCalendar Members
        /// <summary>
        /// Calendar code used to indentify which holiday calendar data source to use.
        /// e.g. "country='US', province='NewYork', ref='some value to indicate source'";
        /// </summary>
        public static string CalendarCode
        {
            get { return _provider.CalendarCode; }
        }


        /// <summary>
        /// Holiday data provider.
        /// </summary>
        public static ICalendarDataProvider HolidayProvider
        {
            get { return _provider.HolidayProvider; }
        }


        /// <summary>
        /// Initialize the Holiday calendar data provider and calendar code.
        /// </summary>
        /// <param name="calendarCode">"country='US', province='NewYork', ref='some value to indicate source'"</param>
        /// <param name="provider">The underlying holiday dates provider.</param>
        /// <param name="startYear">Used for initialization. Loads holidays from the starting year.</param>
        /// <param name="endYear">Used for initialization. Loads holidays up to the ending year.</param>
        public static void Init(string calendarCode, ICalendarDataProvider provider, int startYear, int endYear)
        {
            _provider.Init(calendarCode, provider, startYear, endYear);            
        }


        /// <summary>
        /// Indicates if there is holiday data available for the
        /// specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsHolidayDataAvailable(int year)
        {
            return _provider.IsHolidayDataAvailable(year);
        }


        /// <summary>
        /// Determine if the date is a business date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsBusinessDay(DateTime date)
        {
            return _provider.IsBusinessDay(date);
        }


        /// <summary>
        /// Get the holidays for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<DateTime> Holidays(int year)
        {
            return _provider.Holidays(year);
        }


        /// <summary>
        /// Get the weekends in the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<DateTime> Weekends(int year)
        {
            return _provider.Weekends(year);
        }


        /// <summary>
        /// Get the business dates for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<DateTime> BusinessDays(int year)
        {
            return _provider.BusinessDays(year);
        }

        
        /// <summary>
        /// Get the next business date after the date supplied.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime NextBusinessDate(DateTime date)
        {
            return _provider.NextBusinessDate(date);
        }


        /// <summary>
        /// Get the last 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime PreviousBusinessDate(DateTime date)
        {
            return _provider.PreviousBusinessDate(date);
        }


        /// <summary>
        /// Get the first business date of the month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime FirstBusinessDateOfMonth(int month, int year)
        {
            return _provider.FirstBusinessDateOfMonth(month, year);
        }


        /// <summary>
        /// Get the first business date of the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime FirstBusinessDateOfYear(int year)
        {
            return _provider.FirstBusinessDateOfYear(year);
        }


        /// <summary>
        /// Get the last business date fo the month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime LastBusinessDateOfMonth(int month, int year)
        {
            return _provider.LastBusinessDateOfMonth(month, year);
        }


        /// <summary>
        /// Get the last business date of the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DateTime LastBusinessDateOfYear(int year)
        {
            return _provider.LastBusinessDateOfYear(year);
        }

        #endregion

    }
}
