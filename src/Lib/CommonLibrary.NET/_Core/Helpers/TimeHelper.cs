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
using System.Text.RegularExpressions;



namespace ComLib
{
    /// <summary>
    /// Enum to represent the time as a part of the day.
    /// </summary>
    public enum StartTimeOfDay { 
        /// <summary>
        /// All day.
        /// </summary>
        All = 0, 
        
        /// <summary>
        /// Morning part.
        /// </summary>
        Morning, 
        
        /// <summary>
        /// Afternoon part.
        /// </summary>
        Afternoon, 
        
        /// <summary>
        /// Evening part.
        /// </summary>
        Evening };


    /// <summary>
    /// Time parse result.
    /// </summary>
    public class TimeParseResult
    {
        /// <summary>
        /// True if the parse was valid.
        /// </summary>
        public readonly bool IsValid;


        /// <summary>
        /// Validation error.
        /// </summary>
        public readonly string Error;


        /// <summary>
        /// Start of period.
        /// </summary>
        public readonly TimeSpan Start;


        /// <summary>
        /// End of period.
        /// </summary>
        public readonly TimeSpan End;


        /// <summary>
        /// Constructor to initialize the results.
        /// </summary>
        /// <param name="valid">Validation result.</param>
        /// <param name="error">Error string.</param>
        /// <param name="start">Start of period.</param>
        /// <param name="end">End of period.</param>
        public TimeParseResult(bool valid, string error, TimeSpan start, TimeSpan end)
        {
            IsValid = valid;
            Error = error;
            Start = start;
            End = end;
        }


        /// <summary>
        /// Get the start time as a datetime.
        /// </summary>
        public DateTime StartTimeAsDate
        {
            get 
            {
                if (Start == TimeSpan.MinValue) 
                    return TimeParserConstants.MinDate;

                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Start.Hours, Start.Minutes, Start.Seconds); 
            }
        }


        /// <summary>
        /// Get the end time as a datetime.
        /// </summary>
        public DateTime EndTimeAsDate
        {
            get 
            {
                if (End == TimeSpan.MaxValue) 
                    return TimeParserConstants.MaxDate;

                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, End.Hours, End.Minutes, End.Seconds); 
            }
        }
    }



    /// <summary>
    /// constants used by the time parser.
    /// </summary>
    public class TimeParserConstants
    {
        /// <summary>
        /// Am string.
        /// </summary>
        public const string Am = "am";

        /// <summary>
        /// Am string with periods a.m.
        /// </summary>
        public const string AmWithPeriods = "a.m.";
        
        /// <summary>
        /// Pm string.
        /// </summary>
        public const string Pm = "pm";

        /// <summary>
        /// Pm string with periods p.m.
        /// </summary>
        public const string PmWithPeriods = "p.m.";

        /// <summary>
        /// Min Time to represent All times for a post.
        /// </summary>
        public static readonly DateTime MinDate = new DateTime(2000, 1, 1);

        /// <summary>
        /// Max Time to represent all times for a post.
        /// </summary>
        public static readonly DateTime MaxDate = new DateTime(2050, 1, 1);      

        /// <summary>
        /// Error string for end time less than start time.
        /// </summary>
        public const string ErrorEndTimeLessThanStart = "End time must be greater than or equal to start time.";


        /// <summary>
        /// Error string for no separator between start and end time.
        /// </summary>
        public const string ErrorStartEndTimeSepartorNotPresent = "Start and end time separator not present, use '-' or 'to'";
        
        
        /// <summary>
        /// Error string for no start time provided.
        /// </summary>
        public const string ErrorStartTimeNotProvided = "Start time not provided";


        /// <summary>
        /// Error string for no end time provided.
        /// </summary>
        public const string ErrorEndTimeNotProvided = "End time not provided";
    }



    /// <summary>
    /// Class to parse time in following formats.
    /// 
    /// 1. 1
    /// 2. 1am
    /// 3. 1pm
    /// 4. 1:30
    /// 5. 1:30am
    /// 6. 12pm
    /// </summary>
    public class TimeHelper
    {
        #region Parse Methods
        /// <summary>
        /// Parses the start and (optional) end time supplied as a single string.
        /// 
        /// e.g.
        ///     11:30am
        ///     11am    -  1pm
        ///     11am    to 1pm
        /// </summary>
        /// <remarks>If only 1 time is provided, it's assumed to be the starttime,
        /// and the end time is set to TimeSpan.MaxValue</remarks>
        /// <param name="startAndEndTimeRange">String with start and end time.</param>
        /// <returns>An instance of time parse result for the supplied string.</returns>
        public static TimeParseResult ParseStartEndTimes(string startAndEndTimeRange)
        {
            string dateSeparator = "-";
            int ndxOfSeparator = startAndEndTimeRange.IndexOf(dateSeparator);
            
            if (ndxOfSeparator < 0 )
            {
                dateSeparator = "to";
                ndxOfSeparator = startAndEndTimeRange.IndexOf(dateSeparator);

                // No end time specified.
                if (ndxOfSeparator < 0)
                {
                    var startParseResult  = Parse(startAndEndTimeRange);
                    TimeSpan startOnlyTime = TimeSpan.MinValue;

                    // Error parsing?
                    if (!startParseResult.Success)
                    {
                        return new TimeParseResult(false, startParseResult.Message, startOnlyTime, TimeSpan.MaxValue);
                    }
                    startOnlyTime = startParseResult.Item;
                    return new TimeParseResult(true, string.Empty, startOnlyTime, TimeSpan.MaxValue);
                }
            }

            string starts = startAndEndTimeRange.Substring(0, ndxOfSeparator);
            string ends = startAndEndTimeRange.Substring(ndxOfSeparator + dateSeparator.Length);
            return ParseStartEndTimes(starts, ends, true);
        }


        /// <summary>
        /// Parses the start and end time and confirms if the end time is greater than
        /// the start time.
        /// e.g. 11am, 1:30pm
        /// </summary>
        /// <param name="starts">Start time.</param>
        /// <param name="ends">End time.</param>
        /// <param name="checkEndTime">True to check end time.</param>
        /// <returns>An instance of time parse result.</returns>
        public static TimeParseResult ParseStartEndTimes(string starts, string ends, bool checkEndTime)
        {
            // Validate start and end times were provided.
            if (string.IsNullOrEmpty(starts))
                return new TimeParseResult(false, TimeParserConstants.ErrorStartTimeNotProvided, TimeSpan.MinValue, TimeSpan.MaxValue);

            if ( checkEndTime && string.IsNullOrEmpty(ends))
                return new TimeParseResult(false, TimeParserConstants.ErrorEndTimeNotProvided, TimeSpan.MinValue, TimeSpan.MaxValue);
            
            var startResult = Parse(starts);
            TimeSpan start = TimeSpan.MinValue;
            TimeSpan end = TimeSpan.MaxValue;

            // Validate start time is ok.
            if (!startResult.Success) 
                return new TimeParseResult(false, startResult.Message, TimeSpan.MinValue, TimeSpan.MaxValue);

            start = startResult.Item;
            // Validate end time is ok.
            if (checkEndTime)
            {
                var endResult = Parse(ends);
                if(!endResult.Success)
                {
                    return new TimeParseResult(false, endResult.Message, TimeSpan.MinValue, TimeSpan.MaxValue);
                }
                end = endResult.Item;
                if (end < start)
                {
                    return new TimeParseResult(false, TimeParserConstants.ErrorEndTimeLessThanStart, start, end);
                }
            }
            return new TimeParseResult(true, string.Empty, start, end);
        }


        /// <summary>
        /// Parse the time using Regular expression.
        /// </summary>
        /// <param name="strTime">Text with time.</param>
        /// <returns>Time parse result.</returns>
        public static BoolMessageItem<TimeSpan> Parse(string strTime)
        {
            strTime = strTime.Trim().ToLower();
            string pattern = @"(?<hours>[0-9]+)((\:)(?<minutes>[0-9]+))?\s*(?<ampm>(am|a\.m\.|a\.m|pm|p\.m\.|p\.m))?\s*";
            Match match = Regex.Match(strTime, pattern);
            if (!match.Success)
                return new BoolMessageItem<TimeSpan>(TimeSpan.MinValue, false, "Time : " + strTime + " is not a valid time.");

            string strhours = match.Groups["hours"] != null ? match.Groups["hours"].Value : string.Empty;
            string strminutes = match.Groups["minutes"] != null ? match.Groups["minutes"].Value : string.Empty;
            string ampm = match.Groups["ampm"] != null ? match.Groups["ampm"].Value : string.Empty;
            int hours = 0; 
            int minutes = 0;
            
            if (!string.IsNullOrEmpty(strhours) && !Int32.TryParse(strhours, out hours))
            {
                return new BoolMessageItem<TimeSpan>(TimeSpan.MinValue, false, "Hours are invalid.");
            }
            if (!string.IsNullOrEmpty(strminutes) && !Int32.TryParse(strminutes, out minutes))
            {
                return new BoolMessageItem<TimeSpan>(TimeSpan.MinValue, false, "Minutes are invalid.");
            }

            bool isAm = false;
            if (string.IsNullOrEmpty(ampm) || ampm == "am" || ampm == "a.m" || ampm == "a.m.")
                isAm = true;
            else if (ampm == "pm" || ampm == "p.m" || ampm == "p.m.")
                isAm = false;
            else
            {
                return new BoolMessageItem<TimeSpan>(TimeSpan.MinValue, false, "unknown am/pm statement");
            }

            // Add 12 hours for pm specification.
            if (hours != 12 && !isAm)
                hours += 12;

            // Handles 12 12am.
            if (hours == 12 && isAm)
                return new BoolMessageItem<TimeSpan>(new TimeSpan(0, minutes, 0), true, string.Empty);

            return new BoolMessageItem<TimeSpan>(new TimeSpan(hours, minutes, 0), true, string.Empty);
        }
        #endregion

        
        #region Time Conversion methods
        /// <summary>
        /// Convert military time ( 1530 = 3:30 pm ) to a TimeSpan.
        /// </summary>
        /// <param name="military">Military time.</param>
        /// <returns>An instance of timespan corresponding to the military time supplied.</returns>
        public static TimeSpan ConvertFromMilitaryTime(int military)
        {
            TimeSpan time = TimeSpan.MinValue;
            int hours = military / 100;
            int hour = hours;
            int minutes = military % 100;

            time = new TimeSpan(hours, minutes, 0);
            return time;
        }


        /// <summary>
        /// Converts to military time.
        /// </summary>
        /// <param name="timeSpan">Instance of time span with time to convert.</param>
        /// <returns>Military time corresponding to the time span supplied.</returns>
        public static int ConvertToMilitary(TimeSpan timeSpan)
        {
            return (timeSpan.Hours * 100) + timeSpan.Minutes;
        }
        #endregion


        #region Formatting methods
        /// <summary>
        /// Returns a military time formatted as a string.
        /// </summary>
        /// <param name="militaryTime">Military time.</param>
        /// <param name="convertSingleDigit">True to convert times with single digit.</param>
        /// <returns>String with formatted equivalent of passed military time.</returns>
        public static string Format(int militaryTime, bool convertSingleDigit)
        {
            if (convertSingleDigit && militaryTime < 10)
                militaryTime = militaryTime * 100;

            TimeSpan t = ConvertFromMilitaryTime(militaryTime);
            string formatted = Format(t);
            return formatted;
        }


        /// <summary>
        /// Get the time formatted correctly to exclude the minutes if
        /// there aren't any. Also includes am - pm.
        /// </summary>
        /// <param name="time">Time span to format.</param>
        /// <returns>String with formatted time span.</returns>
        public static string Format(TimeSpan time)
        {            
            int hours = time.Hours;
            string amPm = hours < 12 ? TimeParserConstants.Am : TimeParserConstants.Pm;

            // Convert military time 13 hours to standard time 1pm
            if (hours > 12)
                hours = hours - 12;

            if (time.Minutes == 0)
                return hours + amPm;

            // Handles 11:10 - 11:59
            if (time.Minutes > 10)
                return hours + ":" + time.Minutes + amPm;

            // Handles 11:01 - 11:09
            return hours + ":0" + time.Minutes + amPm;
        }
        #endregion


        #region Miscellaneous Methods
        /// <summary>
        /// Gets the time as a part of the day.( morning, afternoon, evening ).
        /// </summary>
        /// <param name="time">Time of day.</param>
        /// <returns>Instance of start time of day.</returns>
        public static StartTimeOfDay GetTimeOfDay(TimeSpan time)
        {           
            if (time.Hours < 12) return StartTimeOfDay.Morning;
            if (time.Hours >= 12 && time.Hours <= 16) return StartTimeOfDay.Afternoon;
            return StartTimeOfDay.Evening;
        }


        /// <summary>
        /// Get the time of day ( morning, afternoon, etc. ) from military time.
        /// </summary>
        /// <param name="militaryTime">Military time.</param>
        /// <returns>Instance with corresponding start time of day.</returns>
        public static StartTimeOfDay GetTimeOfDay(int militaryTime)
        {
            return GetTimeOfDay(ConvertFromMilitaryTime(militaryTime));
        }
        #endregion        
    }
}
