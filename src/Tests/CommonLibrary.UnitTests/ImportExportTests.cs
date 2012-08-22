using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Collections;
using NUnit.Framework;


using ComLib;
using ComLib.IO;
using ComLib.LocationSupport;
using ComLib.ImportExport;
using ComLib.CsvParse;
using ComLib.MapperSupport;


namespace CommonLibrary.Tests
{

    [TestFixture]
    public class ImportExportTests
    {
        public class Conference        
        {
            public string Title { get; set; }
            public DateTime StartDate { get; set; }
            public TimeSpan StartTime { get; set; }
            public bool IsFree { get; set; }
            public int MaxSeats { get; set; }
            public double Cost { get; set; }
            public Address Location { get; set; }
            public string Url { get; set; }


            public Conference() { }
            public Conference(string title, DateTime startDate, bool isFree, int maxSeats, double cost, string url, Address loc)
            {
                Title = title;
                StartDate = startDate;
                //StartTime = timespan;
                IsFree = isFree;
                MaxSeats = maxSeats;
                Cost = cost;
                Url = url;
                Location = loc;
            }

            public Conference(string title, DateTime startDate, TimeSpan timespan, bool isFree, int maxSeats, double cost, string url, Address loc)
            {
                Title = title;
                StartDate = startDate;
                StartTime = timespan;
                IsFree = isFree;
                MaxSeats = maxSeats;
                Cost = cost;
                Url = url;
                Location = loc;
            }
        }


        
        public IniDocument LoadDataIni()
        {
            string iniContent = "[post]" + Environment.NewLine
                              + "Title:Learn .NET 0" + Environment.NewLine
                              + "StartDate:${today}" + Environment.NewLine
                              + "StartTime:9:30pm" + Environment.NewLine
                              + "IsFree:yes" + Environment.NewLine
                              + "Location.Street:100 Broadway0" + Environment.NewLine
                              + "Location.City:Queens0" + Environment.NewLine
                              + "Location.State:New York0" + Environment.NewLine
                              + "Location.Country:USA0" + Environment.NewLine
                              + "Location.Zip:11240" + Environment.NewLine
                              + "MaxSeats:850" + Environment.NewLine
                              + "Cost:$250" + Environment.NewLine
                              + "Url:http://www.comlib0.com" + Environment.NewLine + Environment.NewLine
                              + "[post]" + Environment.NewLine
                              + "Title:Learn .NET 1" + Environment.NewLine
                              + "StartDate:${t+1}" + Environment.NewLine
                              + "StartTime:11:30 a.m" + Environment.NewLine
                              + "IsFree:no" + Environment.NewLine                              
                              + "Location.Street:100 Broadway1" + Environment.NewLine
                              + "Location.City:Queens1" + Environment.NewLine
                              + "Location.State:New York1" + Environment.NewLine
                              + "Location.Country:USA1" + Environment.NewLine
                              + "Location.Zip:11241" + Environment.NewLine
                              + "MaxSeats:851" + Environment.NewLine
                              + "Cost:251" + Environment.NewLine
                              + "Url:http://www.comlib1.com" + Environment.NewLine + Environment.NewLine;

            IniDocument doc = new IniDocument(iniContent, false);
            return doc;
        }


        public string LoadDataCsv()
        {
            string content = "Title,StartDate,StartTime,IsFree,Location.Street,Location.City,Location.State,Location.Country,Location.Zip,MaxSeats,Cost,Url" + Environment.NewLine
                              + "Learn .NET 0, ${today}, 9:30 pm, true,  100 Broadway0, Queens0, New York0, USA0, 11240, 850, $250, http://www.comlib0.com" + Environment.NewLine
                              + "Learn .NET 1, ${t+4},   11:30am, false, 100 Broadway1, Queens1, New York1, USA1, 11241, 851, $251, http://www.comlib1.com" + Environment.NewLine;

            return content;
        }


        [Test]
        public void CanMapFromIni()
        {
            var data = LoadDataIni();
            IDictionary map = data as IDictionary;
            var io = new ImportExportService<Conference>();
            var result = io.Load(map, "ini");
            Assert.AreEqual(result.Item[0].Title, data.GetSection("post", 0).Get<string>("Title"));
            Assert.AreEqual(result.Item[0].StartDate.Date, DateTime.Today.Date);
            Assert.AreEqual(result.Item[0].StartTime, new TimeSpan(21, 30, 0));
            Assert.AreEqual(result.Item[0].IsFree, true);
            Assert.AreEqual(result.Item[0].MaxSeats, data.GetSection("post", 0).Get<int>("MaxSeats"));
            Assert.AreEqual(result.Item[0].Cost, 250);
            Assert.AreEqual(result.Item[0].Url, data.GetSection("post", 0).Get<string>("Url"));

            Assert.AreEqual(result.Item[1].Title, data.GetSection("post", 1).Get<string>("Title"));
            Assert.AreEqual(result.Item[1].StartDate.Date, DateTime.Today.AddDays(1).Date);
            Assert.AreEqual(result.Item[1].StartTime, new TimeSpan(11, 30, 0));
            Assert.AreEqual(result.Item[1].IsFree, false);
            Assert.AreEqual(result.Item[1].MaxSeats, data.GetSection("post", 1).Get<int>("MaxSeats"));
            Assert.AreEqual(result.Item[1].Cost, data.GetSection("post", 1).Get<double>("Cost"));
            Assert.AreEqual(result.Item[1].Url, data.GetSection("post", 1).Get<string>("Url"));
        }


        [Test]
        public void CanMapFromCsv()
        {
            var csv = LoadDataCsv();
            var io = new ImportExportService<Conference>();
            CsvDoc data = new CsvDoc(csv, false);
            var boolresult = io.LoadText(csv, "csv");
            var result = boolresult.Item;
            Assert.AreEqual(result[0].Title, data.Get<string>(0, "Title"));
            Assert.AreEqual(result[0].StartDate.Date, DateTime.Today.Date);
            Assert.AreEqual(result[0].StartTime, new TimeSpan(21, 30, 0));
            Assert.AreEqual(result[0].IsFree, true);
            Assert.AreEqual(result[0].MaxSeats, data.Get<int>(0, "MaxSeats"));
            Assert.AreEqual(result[0].Cost, 250);
            Assert.AreEqual(result[0].Url, data.Get<string>(0, "Url"));

            Assert.AreEqual(result[1].Title, data.Get<string>(1, "Title"));
            Assert.AreEqual(result[1].StartDate.Date, DateTime.Today.AddDays(4).Date);
            Assert.AreEqual(result[1].StartTime, new TimeSpan(11, 30, 0));
            Assert.AreEqual(result[1].IsFree, false);
            Assert.AreEqual(result[1].MaxSeats, data.Get<int>(1, "MaxSeats"));
            Assert.AreEqual(result[1].Cost, 251);
            Assert.AreEqual(result[1].Url, data.Get<string>(1, "Url"));
        }


        
        public void CanMapToCsv()
        {
            List<Conference> cons = new List<Conference>()
            {
                new Conference("learn .NET 1", DateTime.Today, new TimeSpan(0, 21, 30, 0), true, 20, 98.9, "http://conf1.com", new Address(null, "100 street", "queens", "new york", "ny", "11100")),
                new Conference("learn .NET 2", DateTime.Today.AddDays(1), new TimeSpan(0, 11, 30, 0), true, 21, 99.9, "http://conf2.com", new Address(null, "101 street", "queens1", "new york1", "ny1", "11101")),
            };
            var errors = new Errors();
            Mapper.MapToCsv<Conference>(cons, @"c:\temp\conference.csv", errors);
        }
    }
}
