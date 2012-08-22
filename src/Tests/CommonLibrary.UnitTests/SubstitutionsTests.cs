using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using ComLib;
using ComLib.Subs;



namespace CommonLibrary.Tests
{
    [TestFixture]
    public class SubstitutionTests
    {
        [Test]
        public void CanSubstitute()
        {
            List<string> vals  = new List<string>(){ "${today}", "${yesterday}" };

            Substitutor.Substitute(vals);

            Assert.AreEqual(vals[0], DateTime.Today.ToShortDateString());
            Assert.AreEqual(vals[1], DateTime.Today.AddDays(-1).ToShortDateString());
        }


        [Test]
        public void CanSubstituteString()
        {
            string val = Substitutor.Substitute("${t}");

            Assert.AreEqual(val, DateTime.Today.ToShortDateString());
        }


        [Test]
        public void CanSubstituteCustom()
        {
            Substitutor.Register(string.Empty, "user", (key) => Environment.UserName);
            string val = Substitutor.Substitute("${user}");

            Assert.AreEqual(val, Environment.UserName);
        }
    }
}