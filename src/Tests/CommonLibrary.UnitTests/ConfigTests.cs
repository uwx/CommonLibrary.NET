using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using ComLib;
using ComLib.Configuration;
using ComLib.Entities;
using NUnit.Framework;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class ConfigSectionTests
    {
        [Test]
        public void CanSave()
        {
            var config = new ConfigSource();
            config["name"] = "kishore";
            config["opensource", "projectname"] = "commonlibrary.net";
            config["opensource", "years"] = 1;
            config["job", "title"] = "developer";
            config["job", "skill"] = 2;
            config["job", "pay"] = 90000.52;
            config["job", "isManager"] = true;
            config["job", "startdate"] = DateTime.Today.Date;

            Assert.AreEqual(config.Sections.Count, 2);
            Assert.AreEqual(config.Sections[0], "opensource");
            Assert.AreEqual(config.Sections[1], "job");
            Assert.AreEqual(config.Count, 3);
            Assert.AreEqual(config.Get<string>("job", "title"), "developer");
            Assert.AreEqual(config.Get<int>("job", "skill"), 2);
            Assert.AreEqual(config.Get<double>("job", "pay"), 90000.52);
            Assert.AreEqual(config.Get<bool>("job", "isManager"), true);
            Assert.AreEqual(config.Get<DateTime>("job", "startdate"), DateTime.Today.Date);
            Assert.AreEqual(config.GetSection("job").Name, "job");
            Assert.AreEqual(config.GetSection("job").Count, 5);
        }
    }


    [TestFixture]
    public class ConfigDatabaseTests
    {
        class ConfigSample : ConfigSourceDynamic
        {
            public ConfigSample()
                : base(string.Empty, string.Empty, string.Empty, false)
            {
            }


            public ConfigSample(string appName, string configName, string sourcePath, bool isSourcePathFileBased)
                : base(appName, configName, sourcePath, isSourcePathFileBased)
            {
            }


            public string Header { get; set; }
            public bool EnableEmails { get; set; }
            public int PageSize { get; set; }
            public double MaxAmount { get; set; }
            public DateTime BusinessDate { get; set; }
        }


        private void InitSample(ConfigSample sample, IRepository<ConfigItem> repo)
        {
            // Init w/ fake in-memory repository.
            sample.SetRepository(null, repo);
            sample.BusinessDate = DateTime.Today.Date;
            sample.EnableEmails = false;
            sample.Header = "My C# Framework";
            sample.MaxAmount = 50.2;
            sample.PageSize = 15;
        }


        [Test]
        public void CanDynamicallySave()
        {
            var sample = new ConfigSample("stockapp", "dev.config", string.Empty, false);
            var repo = new RepositoryInMemory<ConfigItem>();
            InitSample(sample, repo);

            sample.Save();

            var config2 = new ConfigSourceDb("stockapp", "dev.config", repo, true);
            // Load from the repo/datasource.
            Assert.AreEqual(sample.AppName, "stockapp");
            Assert.AreEqual(sample.ConfigName, "dev.config");
            Assert.AreEqual(sample.Header, config2["", "Header"]);
            Assert.AreEqual(sample.PageSize, config2["", "PageSize"]);
            Assert.AreEqual(sample.EnableEmails, config2["", "EnableEmails"]);
            Assert.AreEqual(sample.MaxAmount, config2["", "MaxAmount"]);
            Assert.AreEqual(sample.BusinessDate, config2["", "BusinessDate"]);
        }


        [Test]
        public void CanDynamicallySaveLoad()
        {
            var sample = new ConfigSample("stockapp", "dev.config", null, false);
            var repo = new RepositoryInMemory<ConfigItem>();
            InitSample(sample, repo);
            sample.Save();

            var sampleReloaded = new ConfigSample("stockapp", "dev.config", null, false);
            sampleReloaded.SetRepository(null, repo);
            sampleReloaded.Load();

            // Load from the repo/datasource.
            Assert.AreEqual(sample.AppName, "stockapp");
            Assert.AreEqual(sample.ConfigName, "dev.config");
            Assert.AreEqual(sample.Header, sampleReloaded.Header);
            Assert.AreEqual(sample.PageSize, sampleReloaded.PageSize);
            Assert.AreEqual(sample.EnableEmails, sampleReloaded.EnableEmails);
            Assert.AreEqual(sample.MaxAmount, sampleReloaded.MaxAmount);
            Assert.AreEqual(sample.BusinessDate, sampleReloaded.BusinessDate);
        }


        [Test]
        public void CanSaveToRepo()
        {
            var repo = new RepositoryInMemory<ConfigItem>();
            var config = new ConfigSourceDb("stockapp", "dev.config", repo, false);
            config["app", "name"] = "my app name";
            config["app", "pagesize"] = 10;
            config["app", "enableEmails"] = false;
            config["app", "businessdate"] = DateTime.Today.Date;
            config["app", "maxAmount"] = 20.5;

            config.Save();

            var config2 = new ConfigSourceDb("stockapp", "dev.config", repo, true);
            // Load from the repo/datasource.
            Assert.AreEqual(config2.Name, "dev.config");
            Assert.AreEqual("my app name", config2["app", "name"]);
            Assert.AreEqual(10, config2["app", "pagesize"]);
            Assert.AreEqual(false, config2["app", "enableEmails"]);
            Assert.AreEqual(20.5, config2["app", "maxAmount"]);
            Assert.AreEqual(DateTime.Today.Date, config2["app", "businessdate"]);
        }



        [Test]
        public void CanRetrieveLookUp()
        {
            var repo = new RepositoryInMemory<ConfigItem>();
            var config = new ConfigSourceDb("stockapp", "dev.config", repo, false);
            config["app", "name"] = "my app name";
            config["app", "pagesize"] = 10;
            config["app", "enableEmails"] = false;
            config["app", "businessdate"] = DateTime.Today.Date;
            config["app", "maxAmount"] = 20.5;

            config.Save();

            // Load from the repo/datasource.
            var lookup = repo.ToLookUpMulti<string>("Key");
            Assert.AreEqual("my app name", lookup["name"].Val);
            Assert.AreEqual("10", lookup["pagesize"].Val);
            Assert.AreEqual("False", lookup["enableEmails"].Val);
            // (20.5).ToString() allows for different locales.
            Assert.AreEqual((20.5).ToString(), lookup["maxAmount"].Val);
            Assert.AreEqual(DateTime.Today.Date.ToString(), lookup["businessdate"].Val);
        }        


        [Test]
        public void CanLoadFromRepo()
        {
            var repo = new RepositoryInMemory<ConfigItem>();
            var items = new List<ConfigItem>()
            {
                new ConfigItem(){ App = "stockapp", Name = "dev.config", Section = "app", Key = "name", ValType = "System.String", Val = "my app name" },
                new ConfigItem(){ App = "stockapp", Name = "dev.config", Section = "app", Key = "pagesize", ValType = "System.Int32", Val = "10" },
                new ConfigItem(){ App = "stockapp", Name = "dev.config", Section = "app", Key = "enableEmails", ValType = "System.Boolean", Val = "False" },                
                new ConfigItem(){ App = "stockapp", Name = "dev.config", Section = "app", Key = "maxAmount", ValType = "System.Double", Val = (20.5).ToString() },
                new ConfigItem(){ App = "stockapp", Name = "dev.config", Section = "app", Key = "businessdate", ValType = "System.DateTime", Val = DateTime.Today.Date.ToString() },
            };
            
            // Create in repo.
            foreach (var item in items) repo.Create(item);

            // Load the items into the config source.
            var config = new ConfigSourceDb("stockapp", "dev.config", repo, true);

            // Load from the repo/datasource.
            Assert.AreEqual("my app name", config["app", "name"]);
            Assert.AreEqual(10, config["app", "pagesize"]);
            Assert.AreEqual(false, config["app", "enableEmails"]);
            Assert.AreEqual(20.5, config["app", "maxAmount"]);
            Assert.AreEqual(DateTime.Today.Date, config["app", "businessdate"]);
        }
    }
}
