using System;
using System.Collections.Generic;

using ComLib;
using ComLib.Calendars;
using ComLib.Entities;
using ComLib.Account;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Calendars namespace.
    /// </summary>
    public class Example_HolidayCalendar : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // 1. Use the default holiday calendar loaded with U.S. holidays.
            // What is next business date after 1/1/<current_year>
            DateTime nextBusDay = Calendar.NextBusinessDate(new DateTime(DateTime.Today.Year, 1, 1));
            DateTime previousBusinessDay = Calendar.PreviousBusinessDate(new DateTime(DateTime.Today.Year, 1, 1));
            
            // 2. Use other holiday calendar.
            // NOTE:( Current implementation only uses hard dates ( no-relative dates 3rd thursday of november )
            // can be supplied. This limitation will be fixed.
            CalanderDao calDao = new CalanderDao("kishore's_holiday_calendar", GetSampleHolidays());
            ICalendar cal = new CalendarService("kishore's_holiday_calendar", calDao, 5);

            nextBusDay = cal.NextBusinessDate(DateTime.Today);
            Console.WriteLine("Next business date using \"kishore's_holiday_calendar\" : " + nextBusDay.ToString());
            return BoolMessageItem.True;
        }



        /// <summary>
        /// Get sample united states holidays.
        /// 
        /// This can be loaded from an XML file.
        /// </summary>
        /// <returns></returns>
        private static List<Holiday> GetSampleHolidays()
        {
            // For testing, New Years, July 4th, Christmas.
            // This can be loaded from the database.
            var holidays = new List<Holiday>()
            {
                new Holiday(1, 1, true, DayOfWeek.Monday, -1,   "Start the year fresh day"),
                new Holiday(1, 19, true, DayOfWeek.Monday, -1,  "King Midas"),
                new Holiday(2, 14, true, DayOfWeek.Monday, -1,  "Get it on with your girlfriend day"),
                new Holiday(5, 25, true, DayOfWeek.Monday, -1,  "World War 1 Day"),
                new Holiday(7, 4, true, DayOfWeek.Monday, -1,   "Free at last day"),
                new Holiday(9, 7, true, DayOfWeek.Monday, -1,   "Get your ass to work Day"),
                new Holiday(10, 12, true, DayOfWeek.Monday, -1, "I found a piece of land day."),
                new Holiday(11, 11, true, DayOfWeek.Monday, -1, "War - What is it good for day."),
                new Holiday(11, 26, true, DayOfWeek.Monday, -1, "Thank you god for everything i have day."),
                new Holiday(12, 25, true, DayOfWeek.Monday, -1, "I want my xbox 360 gift day.")
            };
            return holidays;
        }
    }
}
