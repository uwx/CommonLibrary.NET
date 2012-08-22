using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Extensions;
using NUnit.Framework;
using ComLib;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class LatinWordExtensionsTest
    {
        [Test]
        public void Initials()
        {
            Assert.AreEqual("eof", "end of file".Initials());
            Assert.AreEqual("eof", " end of file".Initials());
            Assert.AreEqual("eof", "end  of file".Initials());
            Assert.AreEqual("eof", "end of file ".Initials());
            Assert.AreEqual("eof", " end of file ".Initials());
            Assert.AreEqual("eof", " end  of  file ".Initials());
            Assert.AreEqual("eof", " end  of     file ".Initials());
            Assert.AreEqual("ef", "end of file".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", " end of file".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", "end  of file".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", "end of file ".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", " end of file ".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", " end  of  file ".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
            Assert.AreEqual("ef", " end  of     file ".Initials(null, LatinWordExtensions.EnglishInitialsExclusionWords));
        }

        [Test]
        public void Wrap()
        {
            var fourty = "123456789+123456789+123456789+123456789+";
            var text = "A collection of very reusable code and components in C# 3.5 ranging from ActiveRecord, Csv, Command Line Parsing, Configuration, Validation, Logging, Collections, Authentication, and much more.";

            var expected = "A collection of very reusable code and\n" +
                           "components in C# 3.5 ranging from\n" +
                           "ActiveRecord, Csv, Command Line Parsing,\n" +
                           "Configuration, Validation, Logging,\n" +
                           "Collections, Authentication, and much\n" +
                           "more.";

            var wrapped = text.Wrap(40, "\n");

            Assert.AreEqual(expected, wrapped, "Wrapped lines do not matched " +
                "expected pattern");

            Console.WriteLine(fourty);
            Console.WriteLine(wrapped);
        }

        [Test]
        public void NormalizeWhitespace()
        {
            var source = "\"With\tmy tongue firmly planted in my cheek, I call " +
                           "programmers Homo logicus:\n\n a species slightly - " +
                           "but distinctly   different   from Homo sapiens.\" \f" +
                           "-Alan Cooper From The Inmates \r" +
                           "Are Running the Asylum";
            var expected = "\"With my tongue firmly planted in my cheek, I call " +
                           "programmers Homo logicus: a species slightly - but distinctly " +
                           "different from Homo sapiens.\" -Alan Cooper From The Inmates " +
                           "Are Running the Asylum";
            Assert.AreEqual(expected, source.NormalizeWhitespace(),
                "Source text was not normalized correctly");
        }
    }
}
