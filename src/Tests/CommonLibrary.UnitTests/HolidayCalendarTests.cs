using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Calendars;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class HolidayCalendarTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            CalanderDao dao = new CalanderDao();

            // For testing, jan1, july4, christmas.
            List<Holiday> holidays = new List<Holiday>()
            {
                new Holiday(1, 1, true, DayOfWeek.Monday, -1, "New Years"),
                new Holiday(7, 4, true, DayOfWeek.Monday, -1, "Independence Day"),
                new Holiday(11, 1, false, DayOfWeek.Thursday, 3, "ThanksGiving"),
                new Holiday(12, 25, true, DayOfWeek.Monday, -1, "Christmas Day")
            };
            dao.Load("usa-bronx-holidays", holidays);
            Calendar.Init("usa-bronx-holidays", dao, 2009, DateTime.Today.Year);
        }


        [Test]
        public void Can_Work_With_Relative_Dates()
        {
            DateTime t1 = Calendar.NextBusinessDate(new DateTime(2011, 11, 16));
            DateTime t2 = Calendar.NextBusinessDate(new DateTime(2010, 11, 17));
            DateTime t3 = Calendar.NextBusinessDate(new DateTime(2009, 11, 18));

            Assert.AreEqual(new DateTime(2011, 11, 18), t1);
            Assert.AreEqual(new DateTime(2010, 11, 19), t2);
            Assert.AreEqual(new DateTime(2009, 11, 20), t3);
        }


        [Test]
        public void Can_Get_Next_Business_Date()
        {
            DateTime busDay = Calendar.NextBusinessDate(new DateTime(DateTime.Today.Year, 1, 1));

            Assert.AreEqual(Calendar.CalendarCode, "usa-bronx-holidays");
            Assert.AreEqual(busDay, new DateTime(DateTime.Today.Year, 1, 2));
        }


        [Test]
        public void Can_Get_First_Business_Date_Of_Year()
        {
            DateTime busDay = Calendar.FirstBusinessDateOfYear(DateTime.Today.Year);

            Assert.AreEqual(busDay, new DateTime(DateTime.Today.Year, 1, 2));
        }


        [Test]
        public void Can_Get_Total_Holidays_In_Year()
        {
        }


        [Test]
        public void Can_Get_First_Business_Date_Of_Month()
        {
            DateTime busDay = Calendar.FirstBusinessDateOfMonth(1, DateTime.Today.Year);

            Assert.AreEqual(busDay, new DateTime(DateTime.Today.Year, 1, 2));
        }
    }
}