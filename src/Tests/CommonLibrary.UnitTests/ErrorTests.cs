using System;
using System.Collections.Generic;
using System.Text;

using ComLib;
using ComLib.ValidationSupport;
using NUnit.Framework;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class ErrorTests
    {
        [Test]
        public void CanUseMessages()
        {
            Messages messages = new Messages();
            messages.Add("user creation successful");
            Assert.AreEqual(messages.Count, 1);
        }



        [Test]
        public void CanAddErrorKeys()
        {
            var errors = new Errors();
            errors.Add("Url", "Is not a valid format");
            errors.Add("Phone", "Must be in united states format");

            string fullErrorMessage = "Url Is not a valid format" + Environment.NewLine
                                    + "Phone Must be in united states format" + Environment.NewLine;

            Assert.IsTrue(errors.Count == 2);
            Assert.IsTrue(errors.HasAny);
            Assert.AreEqual(errors.On("Url"), "Is not a valid format");
            Assert.AreEqual(errors.On("Phone"), "Must be in united states format");
            Assert.AreEqual(errors.Message(), fullErrorMessage);
        }


        [Test]
        public void CanAddErrorMessagesAll()
        {
            var errors = new Errors();
            errors.Add("Url", "Is not a valid format");
            errors.Add("Phone", "Must be in united states format");
            errors.Add("General error 1");
            errors.Add("General error 2");

            string fullErrorMessage = "General error 1" + Environment.NewLine
                                    + "General error 2" + Environment.NewLine
                                    + "Url Is not a valid format" + Environment.NewLine
                                    + "Phone Must be in united states format" + Environment.NewLine;

            Assert.IsTrue(errors.Count == 4);
            Assert.IsTrue(errors.HasAny);
            Assert.AreEqual(errors.On().Count, 2);
            Assert.AreEqual(errors.Message(), fullErrorMessage);
        }


        [Test]
        public void CanAddErrorMessages()
        {
            var errors = new Errors();
            errors.Add("Url Is not a valid format");
            errors.Add("Phone Must be in united states format");

            string fullErrorMessage = "Url Is not a valid format" + Environment.NewLine
                                    + "Phone Must be in united states format" + Environment.NewLine;

            Assert.IsTrue(errors.Count == 2);
            Assert.IsTrue(errors.HasAny);
            Assert.AreEqual(errors.Message(), fullErrorMessage);
        }
    }
}
