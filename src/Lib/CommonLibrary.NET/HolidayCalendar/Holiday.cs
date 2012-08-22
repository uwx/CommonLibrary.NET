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
    /// Represents a single holiday.
    /// </summary>
    public class Holiday
    {
        /// <summary>
        /// Month of the holiday.
        /// </summary>
        public int Month;


        /// <summary>
        /// Day of the holiday.
        /// </summary>
        public int Day;


        /// <summary>
        /// Whether or not this holiday always lands on a specific day e.g. Jan 1st. or July 4th. As opposed to thanksgiving ( 3rd thursday )
        /// </summary>
        public bool IsHardDay;


        /// <summary>
        /// The day in the week of this holiday. Used for relative(non-hard day) holidays. e.g. Thursday for Thanksgiving.
        /// </summary>
        public DayOfWeek DayOfTheWeek;


        /// <summary>
        /// The week of this holiday. Eg. 3 for Thanksgiving.
        /// </summary>
        public int WeekOfMonth;


        /// <summary>
        /// Description for the holiday
        /// </summary>
        public string Description;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isHardDay"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="weekOfMonth"></param>
        /// <param name="holidayDescription"></param>
        public Holiday(int month, int day, bool isHardDay, DayOfWeek dayOfWeek, int weekOfMonth, string holidayDescription)
        {
            Month = month;
            Day = day;
            IsHardDay = isHardDay;
            DayOfTheWeek = dayOfWeek;
            WeekOfMonth = weekOfMonth;
            Description = holidayDescription;
        }
    }
}
