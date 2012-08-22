using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using ComLib.Web;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class UrlTests
    {
        [Test]
        public void CanBuildValidUrlWithOutAnyInvalidChars()
        {
            string title = "This is a post about optimizing a title";
            string url = UrlSeoUtils.BuildValidUrl(title);
            string expected = "this-is-a-post-about-optimizing-a-title";

            Assert.AreEqual(expected, url);
        }


        


        [Test]
        public void CanBuildValidUrlWithInvalidChars()
        {
            string title = "Really~1@# Bad-*7s;:Title";
            string url = UrlSeoUtils.BuildValidUrl(title);
            string expected = "really1-bad-7stitle";

            Assert.AreEqual(expected, url);
        }


        [Test]
        public void CanBuildValidUrlWithSequentialSpaces()
        {
            string title = "Too    many    spaces";
            string url = UrlSeoUtils.BuildValidUrl(title);
            string expected = "too-many-spaces";

            Assert.AreEqual(expected, url);
        }


        [Test]
        public void CanBuildValidUrlWithSequentialDashes()
        {
            string title = "Too---many---dashes";
            string url = UrlSeoUtils.BuildValidUrl(title);
            string expected = "too-many-dashes";

            Assert.AreEqual(expected, url);
        }


        [Test]
        public void CanBuildValidUrlWithInvalidCharsBetweenSpaces()
        {
            string title = "Too ^&* many ;'\"[]\\_+= invalidChars";
            string url = UrlSeoUtils.BuildValidUrl(title);
            string expected = "too-many-invalidchars";

            Assert.AreEqual(expected, url);
        }
    }
}
