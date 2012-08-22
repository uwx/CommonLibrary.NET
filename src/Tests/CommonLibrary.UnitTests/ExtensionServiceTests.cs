using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;



namespace CommonLibrary.Tests.ExtensionTests
{
    [TestFixture]
    public class ExtensionServiceTests
    {
        [Extension(Name = "InfoTask.Test1", Description = "Information test", Roles = "Admin")]
        public class ExtensionTest
        {
        }


        [Test]
        public void CanLoadViaAssembly()
        {
            //var service = new ExtensionService();
            //service.Load("CommonLibrary.Web.Modules.Tests");
        }
    }
}
