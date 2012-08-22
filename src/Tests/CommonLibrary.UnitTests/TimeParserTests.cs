using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Types;


namespace CommonLibrary.Tests
{
    

    [TestFixture]
    public class TimeParserTests
    {
        [Test]
        public void CanNotParse()
        {
            BoolMessageItem<TimeSpan> result = TimeHelper.Parse("sf");
            Assert.IsFalse(result.Success);
        }


        [Test]
        public void CanNotParseWithOutAmPm()
        {
            TimeSpan time = TimeHelper.Parse("1").Item;
            Assert.AreEqual(time.Hours, 1);
            Assert.AreEqual(time.Minutes, 0);           
        }


        [Test]
        public void CanParseWithoutMinutesInPm()
        {
            TimeSpan time = TimeHelper.Parse("1pm").Item;
            Assert.AreEqual(time.Hours, 13);
            Assert.AreEqual(time.Minutes, 0);       
        }


        [Test]
        public void CanParseWithoutMinutesInAm()
        {
            TimeSpan time = TimeHelper.Parse("8am").Item;
            Assert.AreEqual(time.Hours, 8);
            Assert.AreEqual(time.Minutes, 0);       
        }


        [Test]
        public void CanParseWithMinutesInPm()
        {
            TimeSpan time = TimeHelper.Parse("10:45pm").Item;
            Assert.AreEqual(time.Hours, 22);
            Assert.AreEqual(time.Minutes, 45);       
        }


        [Test]
        public void CanParseWithMinutesInAm()
        {
            TimeSpan time = TimeHelper.Parse("6:30am").Item;
            Assert.AreEqual(time.Hours, 6);
            Assert.AreEqual(time.Minutes, 30);       
        }


        [Test]
        public void CanParseTimeRange()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30am to 1pm");

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(end, result.End);           
        }


        [Test]
        public void CanParseTimeRangeWithPeriodsInAmPm()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30 a.m. to 1 p.m.");

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(end, result.End);
        }


        [Test]
        public void CanParseTimeRangeWithSpacesBetweenTimeAndAm()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30 am to 1 pm");

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(end, result.End);
        }


        [Test]
        public void CanParseTimeRangeWithOutEndTime()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30am");

            TimeSpan start = new TimeSpan(11, 30, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(TimeSpan.MaxValue, result.End);
            Assert.AreEqual(TimeParserConstants.MaxDate, result.EndTimeAsDate);           
        }


        [Test]
        public void CanParseIndividualStartEndTimes()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30am", "1pm", true);

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(end, result.End);   
        }


        [Test]
        public void CanNotParseIndividualStartEndTimesWithMissingEndTime()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30am", "", true);

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(TimeSpan.MinValue, result.Start);
            Assert.AreEqual(TimeSpan.MaxValue, result.End);
        }


        [Test]
        public void CanParseIndividualStartEndTimesWithMissingEndTime()
        {
            TimeParseResult result = TimeHelper.ParseStartEndTimes("11:30am", "", false);

            TimeSpan start = new TimeSpan(11, 30, 0);
            TimeSpan end = new TimeSpan(13, 0, 0);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(start, result.Start);
            Assert.AreEqual(TimeSpan.MaxValue, result.End);
        }
    }
}
