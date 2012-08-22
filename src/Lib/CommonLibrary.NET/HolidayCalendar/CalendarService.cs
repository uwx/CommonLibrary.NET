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
    /// Holiday calendar service.
    /// </summary>
    public class CalendarService : ICalendar
    {
        private string _calendarCode;
        private IDictionary<int, IDictionary<string, DateTime>> _yearHolidayMap = new Dictionary<int, IDictionary<string, DateTime>>();
        private IDictionary<string, DateTime> _holidayMap = new Dictionary<string, DateTime>();
        private IDictionary<int, List<DateTime>> _holidayListMap = new Dictionary<int, List<DateTime>>();
        private ICalendarDataProvider _holidayDataProvider;

        /// <summary>
        /// Raised when the calendar is changed.
        /// </summary>
        public event EventHandler OnCalendarChanged;


        /// <summary>
        /// Initialize using defaults.
        /// </summary>
        public CalendarService()
        {
        }


        /// <summary>
        /// Initialize using calendar code and holiday dates provider.
        /// This defaults the # of years of holidays that can be handled to +- 50;
        /// <param name="calendarCode">"U.S.-NewYork"</param>
        /// <param name="provider">The underlying holiday dates provider.</param>
        /// <param name="forwardBackRange">The number of years before and after current year 
        /// for which to calculate dates.</param>
        /// </summary>
        public CalendarService(string calendarCode, ICalendarDataProvider provider, int forwardBackRange)
        {
            Init(calendarCode, provider, DateTime.Today.Year - forwardBackRange, DateTime.Today.Year + forwardBackRange);
        }


        #region IHolidayCalendar Members
        /// <summary>
        /// Calendar code used to indentify which holiday calendar data source to use.
        /// e.g. "country='US', province='NewYork', ref='some value to indicate source'";
        /// </summary>
        public string CalendarCode
        {
            get { return _calendarCode; }
        }


        /// <summary>
        /// Holiday data provider.
        /// </summary>
        public ICalendarDataProvider HolidayProvider
        {
            get { return _holidayDataProvider; }
        }


        /// <summary>
        /// Initialize the Holiday calendar data provider and calendar code.
        /// </summary>
        /// <param name="calendarCode">"U.S.-NewYork"</param>
        /// <param name="provider">The underlying holiday dates provider.</param>
        /// <param name="startYear">Used for initialization. Loads holidays from the starting year.</param>
        /// <param name="endYear">Used for initialization. Loads holidays up to the ending year.</param>
        public virtual void Init(string calendarCode, ICalendarDataProvider provider, int startYear, int endYear)
        {
            _calendarCode = calendarCode;
            _holidayDataProvider = provider;
            _holidayDataProvider.CalendarCode = _calendarCode;
            List<KeyValuePair<int, List<DateTime>>> holidays = _holidayDataProvider.Holidays(startYear, endYear);

            // Now sort the holidays into ascending order.
            foreach (KeyValuePair<int, List<DateTime>> pair in holidays)
            {
                _holidayListMap[pair.Key] = pair.Value;
                Dictionary<string, DateTime> holidaysByYearMap = new Dictionary<string,DateTime>();

                // Now store each date into map.
                foreach (DateTime date in pair.Value)
                {
                    string format = GetDateFormat(date);
                    _holidayMap[format] = date;
                    holidaysByYearMap[format] = date;
                }
                _yearHolidayMap[pair.Key] = holidaysByYearMap;
            }

            // Notify.
            if (OnCalendarChanged != null)
                OnCalendarChanged(this, new EventArgs());
        }


        /// <summary>
        /// Indicates if there is holiday data available for the
        /// specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool IsHolidayDataAvailable(int year)
        {
            return _yearHolidayMap.ContainsKey(year);
        }


        /// <summary>
        /// Determine if the date is a business date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsBusinessDay(DateTime date)
        {
            if (!IsHolidayDataAvailable(date.Year)) return false;

            return IsHoliday(_yearHolidayMap[date.Year], date);
        }


        /// <summary>
        /// Get the holidays for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<DateTime> Holidays(int year)
        {
            if (!IsHolidayDataAvailable(year)) return null;
            return new List<DateTime>(_holidayListMap[year]);
        }


        /// <summary>
        /// Get the weekends in the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<DateTime> Weekends(int year)
        {
            if (!IsHolidayDataAvailable(year)) return null;
            
            return ForEachDay(year, delegate(DateTime day)
            {
                return day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday;
            });
        }


        /// <summary>
        /// Get the business dates for the specified year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<DateTime> BusinessDays(int year)
        {
            if (!IsHolidayDataAvailable(year)) return null;
            IDictionary<string, DateTime> holidayMap = _yearHolidayMap[year];
            
            return ForEachDay(year, delegate(DateTime day) { return IsHoliday(holidayMap, day); } );
        }

        
        /// <summary>
        /// Get the next business date after the date supplied.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime NextBusinessDate(DateTime date)
        {
            return NextBusinessDate(date, true, 1);
        }


        /// <summary>
        /// Get the previous business date from the date supplied.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DateTime PreviousBusinessDate(DateTime date)
        {
            return NextBusinessDate(date, true, -1);
        }


        /// <summary>
        /// Get the first business date of the month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime FirstBusinessDateOfMonth(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);
            date = date.AddDays(-1);
            return NextBusinessDate(date, date.Year == year, 1);
        }


        /// <summary>
        /// Get the first business date of the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime FirstBusinessDateOfYear(int year)
        {
            DateTime date = new DateTime(year, 1, 1);
            date = date.AddDays(-1);

            // The check for year needs to be done so 
            // that only years that are supported are checked.
            return NextBusinessDate(date, date.Year == year, 1);
        }


        /// <summary>
        /// Get the last business date fo the month.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime LastBusinessDateOfMonth(int month, int year)
        {
            DateTime date = new DateTime(year, month, 1);
            return NextBusinessDate(date, true, -1);
        }


        /// <summary>
        /// Get the last business date of the year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime LastBusinessDateOfYear(int year)
        {
            DateTime date = new DateTime(year, 1, 1);
            return NextBusinessDate(date, true, -1);
        }

        #endregion


        #region Private members
        private string GetDateFormat(DateTime dt)
        {
            return dt.ToString("MM/dd/yyyy");
        }


        private bool IsHoliday(IDictionary<string, DateTime> holidayLookup, DateTime date)
        {
            string key = date.ToString("MM/dd/yyyy");
            bool isNotHoliday = !holidayLookup.ContainsKey(key) && date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
            return !isNotHoliday;
        }


        private DateTime NextBusinessDate(DateTime date, bool checkYear, int incrementor)
        {
            if (checkYear && !IsHolidayDataAvailable(date.Year)) return DateTime.MinValue;

            int maxIterations = 100;
            int attempt = 0;
            date = date.AddDays(incrementor);
            IDictionary<string, DateTime> holidays = _yearHolidayMap[date.Year];
            DateTime nextBusinessDate = date;

            while (attempt < maxIterations)
            {
                if (!IsHoliday(holidays, date))
                {
                    nextBusinessDate = date;
                    break;
                }
                attempt++;
                date = date.AddDays(incrementor);
            }
            return nextBusinessDate;
        }


        private List<DateTime> ForEachDay(int year, Predicate<DateTime> IsEligible)
        {
            List<DateTime> filteredDates = new List<DateTime>();
            DateTime start = new DateTime(year, 1, 1);

            while (start.Year == year)
            {
                string key = start.ToString("MM/dd/yyyy");
                if (IsEligible(start))
                {
                    filteredDates.Add(start);
                }
                start = start.AddDays(1);
            }
            return filteredDates;
        }
        #endregion
    }

}
