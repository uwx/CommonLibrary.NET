using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Information;
using CommonLibrary.Tests.Common;


namespace CommonLibrary.Tests.InformationTests
{

    [Info(Name = "InfoTask.Test1", Description = "Information test", Roles = "Admin")]
    public class InfoTest1 : IInformation
    {
        /// <summary>
        /// The supported formats.
        /// </summary>
        public string SupportedFormats { get { return "html"; } }


        /// <summary>
        /// The current format to get the information in.
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            return "Testing information task";
        }
    }



    [TestFixture]
    public class InformationServiceTests
    {
        [Test]
        public void CanRegisterInformation()
        {
            var service = new InformationService();
            service.Load(ContentLoader.TestDllName);

            Assert.AreEqual(service.Lookup.Count, 1);
            Assert.IsNotNull(service.Lookup["InfoTask.Test1"]);
            Assert.IsNotNull(service.Create("InfoTask.Test1"));
            Assert.AreEqual(service.Create("InfoTask.Test1").GetInfo(), "Testing information task");
        }
    }
}
