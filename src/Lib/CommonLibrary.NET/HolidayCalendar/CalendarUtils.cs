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
    /// Helper class for holidays.
    /// </summary>
    public class CalendarUtils
    {
        /// <summary>
        /// Converts the holiday items into DateTime objects for the specified year.
        /// </summary>
        /// <param name="holidays"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static List<DateTime> InterpretHolidays(List<Holiday> holidays, int year)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (Holiday holiday in holidays)
            {
                if (holiday.IsHardDay)
                    dates.Add(new DateTime(year, holiday.Month, holiday.Day));
                else
                {
                    const int DaysInWeek = 7;

                    // init with first date of month
                    DateTime holidayDate = new DateTime(year, holiday.Month, 1);

                    // move to holiday day of week
                    while (holidayDate.DayOfWeek != holiday.DayOfTheWeek)
                    {
                        holidayDate = holidayDate.AddDays(1);
                    }

                    // now, apply week count shift
                    holidayDate = holidayDate.AddDays((holiday.WeekOfMonth - 1) * DaysInWeek);

                    // store
                    dates.Add(holidayDate);
                }

            }
            return dates;
        }


        /// <summary>
        /// Get United States holidays.
        /// </summary>
        /// <returns></returns>
        public static List<Holiday> GetUnitedStatesHolidays()
        {
            // This can be loaded from the database.
            List<Holiday> holidays = new List<Holiday>()
            {
                new Holiday(1, 1, true, DayOfWeek.Monday, -1, "New Years"),
                new Holiday(1, 19, true, DayOfWeek.Monday, -1, "Martin Luther King"),
                new Holiday(2, 16, true, DayOfWeek.Monday, -1, "Washingtons Birthday"),
                new Holiday(5, 25, true, DayOfWeek.Monday, -1, "Memorial Day"),
                new Holiday(7, 4, true, DayOfWeek.Monday, -1, "Independence Day"),
                new Holiday(9, 7, true, DayOfWeek.Monday, -1, "Labor Day"),
                new Holiday(10, 12, true, DayOfWeek.Monday, -1, "Columbus Day"),
                new Holiday(11, 11, true, DayOfWeek.Monday, -1, "Veterans Day"),
                new Holiday(11, 26, true, DayOfWeek.Thursday, 4, "Thanks Giving"),
                new Holiday(12, 25, true, DayOfWeek.Monday, -1, "Christmas Day")
            };
            return holidays;
        }
    }
}
