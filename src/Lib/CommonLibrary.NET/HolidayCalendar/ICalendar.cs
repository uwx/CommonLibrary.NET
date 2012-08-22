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
    /// The standard interface to be implemented by a class that
    /// will provide holidays, business days and related information.
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// Calendar code.
        /// </summary>
        string CalendarCode { get; }


        /// <summary>
        /// The underlying holiday data provider.
        /// </summary>
        ICalendarDataProvider HolidayProvider { get; }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="calendarCode"></param>
        /// <param name="provider"></param>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        void Init(string calendarCode, ICalendarDataProvider provider, int startYear, int endYear);


        /// <summary>
        /// Determine if holiday data is available for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        bool IsHolidayDataAvailable(int year);


        /// <summary>
        /// Is the supplied business date a holiday.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        bool IsBusinessDay(DateTime date);


        /// <summary>
        /// Get all the holidays for the specified year.
        /// </summary>
        /// <returns></returns>
        List<DateTime> Holidays(int year);


        /// <summary>
        /// Get all the weekends for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        List<DateTime> Weekends(int year);


        /// <summary>
        /// Get all the business dates for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        List<DateTime> BusinessDays(int year);


        /// <summary>
        /// Get the next business date.
        /// </summary>
        /// <param name="afterDate"></param>
        /// <returns></returns>
        DateTime NextBusinessDate(DateTime afterDate);


        /// <summary>
        /// Get the previous business date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        DateTime PreviousBusinessDate(DateTime date);


        /// <summary>
        /// Get the first business date for the specified month, year.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime FirstBusinessDateOfMonth(int month, int year);


        /// <summary>
        /// Get the first business date for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime FirstBusinessDateOfYear(int year);


        /// <summary>
        /// Get the last business date for the specified month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime LastBusinessDateOfMonth(int month, int year);
        
        
        /// <summary>
        /// Get the last businessdate for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DateTime LastBusinessDateOfYear(int year);
    }



    /// <summary>
    /// Holidays data provider.
    /// </summary>
    public interface ICalendarDataProvider
    {
        /// <summary>
        /// Calendar code.
        /// </summary>
        string CalendarCode { get; set; }


        /// <summary>
        /// Get all the holidays from the starting year the ending year specified.
        /// </summary>
        /// <param name="startyear"></param>
        /// <param name="endYear"></param>
        /// <returns></returns>
        List<KeyValuePair<int, List<DateTime>>> Holidays(int startyear, int endYear);
    }
}
