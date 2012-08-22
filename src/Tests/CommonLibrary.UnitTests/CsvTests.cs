using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;

using ComLib;
using ComLib.CsvParse;

using CommonLibrary.Tests.Common;


namespace CommonLibrary.Tests
{
    public class CsvCheck
    {
        public static void AssertColumnDataInt(CsvDoc doc, string columnName, int startingRowIndex, int startingValue, int increment)
        {
            // Iterate over each cell in column.
            doc.ForEach<int>(columnName, 0, (row, col, val) =>
            {
                Assert.AreEqual(startingValue, val);
                startingValue += increment;
            });
        }


        public static void AssertColumnData(CsvDoc doc, string column, int startingRowIndex, string expectedValue)
        {
            // Iterate over each cell in column.
            doc.ForEach( column, 0, (row, col, val) =>
            {
                Assert.AreEqual(expectedValue, val);
            });
        }


        public static void AssertColumnDataDate(CsvDoc doc, string column, int startingRowIndex, DateTime startingValue, int dayIncrement)
        {
            // Iterate over each cell in column.
            doc.ForEach<DateTime>(column, 0, (row, col, val) =>
            {
                Assert.IsTrue(startingValue.Date.CompareTo(val.Date) == 0);
                startingValue = startingValue.AddDays(dayIncrement);
            });
        }
    }


    [TestFixture]
    public class CsvTests
    {
        [Test]
        public void CanParseCsv1()
        {
            CanParseCsv(@"Csv.Csv_NoQuotes.csv");
        }


        [Test]
        public void CanParseCsv_WithQuotes()
        {
            CanParseCsv(@"Csv.Csv_Quotes.csv");
        }


        [Test]
        public void CanParseCsv_WithDifferentDelimeter()
        {
            CanParseCsv(@"Csv.Csv_Delimeter.csv", ';', "(`~!@#$%^&*()_+-=[]\\{}|<>?./;,:)", false);
        }


        [Test]
        public void CanParseCsv_WithTabDelimeter()
        {
            CanParseCsv(@"Csv.Csv_TabDelimeter.csv", '\t', "(`~!@#$%^&*()_+-=[]\\{}|<>?./;,:)", false);
        }


        [Test]
        public void CanParseCsv_WithNoHeader()
        {
            CanParseCsvNoHeader(@"Csv.Csv_NoHeader.csv", '|', "(`~!@#$%^&*()_+-=[]\\{}|<>?./;,:)");
        }


        [Test]
        public void CanParseCsv_WithQuotesRandom()
        {
            CanParseCsv(@"Csv.Csv_QuotesRandom.csv");
        }


        [Test]
        public void CanParseCsv_DoubleQuotes()
        {
            CanParseCsv(@"Csv.Csv_DoubleQuotes.csv");
        }


        [Test]
        public void CanParseCsv_EscapedDoubleQuotes()
        {
            CanParseCsv(@"Csv.Csv_EscapedDoubleQuotes.csv", ',', "(`~!@#$%^&*()_+-=[]\\{}|<>?./;:)", true);
        }


        [Test]
        public void CanParseCsv_WithSpaces()
        {
            CanParseCsv(@"Csv.Csv_Spaces.csv");
        }


        [Test]
        public void CanParseCsv_WithNewLines()
        {
            string expected = "Testing" + Environment.NewLine + "new line";
            string text = ContentLoader.GetTextFileContent("Csv.Csv_MultiLine.csv");
            text = CultureInfoHelper.FixDates(text);
            CsvDoc csv = Csv.LoadText(text, true);

            // Check the csv data.
            CsvCheck.AssertColumnDataInt(csv, "Id", 0, 1, 1);
            CsvCheck.AssertColumnData(csv, "NonAlphaNumeric", 0, @"(`~!@#$%^&*()_+-=[]\{}|<>?./;:)");
            CsvCheck.AssertColumnData(csv, "Description", 0, expected);
            CsvCheck.AssertColumnDataDate(csv, "Date", 0, DateTime.Parse(CultureInfoHelper.FixDates("4/10/2009")), 1);
        }


        public void CanParseCsv(string filePath)
        {
            CanParseCsv(filePath, ',', "(`~!@#$%^&*()_+-=[]\\{}|<>?./;:)", false);
        }


        public void CanParseCsv(string filePath, char delimeter, string nonAlphaNumericTextExpected, bool expectEscapedQuote)
        {
            string text = ContentLoader.GetTextFileContent(filePath);
            text = CultureInfoHelper.FixDates(text);
            CsvDoc csv = Csv.LoadText(text, true, false, delimeter);

            // Check the csv data.
            CsvCheck.AssertColumnDataInt(csv, "Id", 0, 1, 1);
            CsvCheck.AssertColumnData(csv, "NonAlphaNumeric", 0, nonAlphaNumericTextExpected);
            if (!expectEscapedQuote)
                CsvCheck.AssertColumnData(csv, "FilePath", 0, @"C:\pictures\100_01.JPG");
            else
                CsvCheck.AssertColumnData(csv, "FilePath", 0, @"C"":\pictures\100_01.JPG");
            CsvCheck.AssertColumnDataDate(csv, "Date", 0, DateTime.Parse(CultureInfoHelper.FixDates("4/10/2009")), 1);
        }


        public void CanParseCsvNoHeader(string filePath, char delimeter, string nonAlphaNumericTextExpected)
        {
            string text = ContentLoader.GetTextFileContent(filePath);
            text = CultureInfoHelper.FixDates(text);
            CsvDoc csv = Csv.LoadText(text, false, false, delimeter);

            // Check the csv data.
            CsvCheck.AssertColumnDataInt(csv, "0", 0, 1, 1);
            CsvCheck.AssertColumnData(csv, "1", 0, nonAlphaNumericTextExpected);
            CsvCheck.AssertColumnData(csv, "2", 0, @"C:\pictures\100_01.JPG");
            CsvCheck.AssertColumnDataDate(csv, "3", 0, DateTime.Parse(CultureInfoHelper.FixDates("4/10/2009")), 1);
        }


        [Test]
        public void CanParseCsv()
        {
            string text = "'Id', 'Name',   'Desc'" + Environment.NewLine
                        + "'0',  'Art',    'Art class'" + Environment.NewLine
                        + "'1',  'Sports', 'all sports'";

            CsvDoc doc = Csv.LoadText(text, true);

            Assert.IsTrue(doc.Columns.Contains("Id"));
            Assert.IsTrue(doc.Columns.Contains("Name"));
            Assert.IsTrue(doc.Columns.Contains("Desc"));            
            Assert.IsTrue(string.Compare((string)doc.Data[0]["Id"], "0") == 0);
            Assert.IsTrue(string.Compare((string)doc.Data[0]["Name"], "Art") == 0);
            Assert.IsTrue(string.Compare((string)doc.Data[0]["Desc"], "Art class") == 0);
            Assert.IsTrue(string.Compare((string)doc.Data[1]["Id"], "1") == 0);
            Assert.IsTrue(string.Compare((string)doc.Data[1]["Name"], "Sports") == 0);
            Assert.IsTrue(string.Compare((string)doc.Data[1]["Desc"], "all sports") == 0);
        }


        [Test]
        public void CanParseCsvAndGetTypedVal()
        {
            string text = "'Id', 'Name',   'Desc'" + Environment.NewLine
                        + "'0',  'Art',    'Art class'" + Environment.NewLine
                        + "'1',  'Sports', 'all sports'";

            CsvDoc doc = Csv.LoadText(text, true);

            Assert.IsTrue(doc.Columns.Contains("Id"));
            Assert.IsTrue(doc.Columns.Contains("Name"));
            Assert.IsTrue(doc.Columns.Contains("Desc"));
            Assert.AreEqual(doc.Get<string>(0, "Id"), "0");
            Assert.AreEqual(doc.Get<string>(0, "Name"), "Art");
            Assert.AreEqual(doc.Get<string>(0, "Desc"), "Art class");
            Assert.AreEqual(doc.Get<string>(1, "Id"), "1");
            Assert.AreEqual(doc.Get<string>(1, "Name"), "Sports");
            Assert.AreEqual(doc.Get<string>(1, "Desc"), "all sports");
        }


        [Test]
        public void CanWriteCsv()
        {
            string text = "'Id', 'Name',   'Desc'" + Environment.NewLine
                        + "'0',  'Art',    'Art class'" + Environment.NewLine
                        + "'1',  'Sports', 'all sports'";
            CsvDoc doc = Csv.LoadText(text, true);
            //doc.Write(@"C:\temp\test.csv", ",");
        }


        [Test]
        public void CanParseFailed1()
        {
            string text = "Id,Name" + Environment.NewLine
                      + "0,Art";

            CsvDoc doc = Csv.LoadText(text, true);

            Assert.IsTrue(doc.Columns.Contains("Id"));
            Assert.IsTrue(doc.Columns.Contains("Name"));
            Assert.AreEqual(doc.Get<string>(0, "Id"), "0");
            Assert.AreEqual(doc.Get<string>(0, "Name"), "Art");
        }
    }
}
