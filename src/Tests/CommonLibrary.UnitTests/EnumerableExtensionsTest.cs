using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ComLib;

namespace CommonLibrary.Tests
{
    [TestFixture]
    class EnumerableExtensionsTest
    {
        /// <summary>
        /// Tests IsEmpty method to see that it returns correctly when presented
        /// with a collection of zero items.
        /// </summary>
        [Test]
        public void IsEmpty()
        {
            // List is empty
            var empty = new List<int>();
            Assert.IsTrue(empty.IsEmpty(), "Empty list should be empty");
            // List is not empty
            var notEmpty = new List<int> { 12, 1, 5 };
            Assert.IsFalse(notEmpty.IsEmpty(), "Not empty list should not be empty");
        }

        /// <summary>
        /// Tests IsNullOrEmpty to see that it returns correctly when presented
        /// with a collection of zero or null items.
        /// </summary>
        [Test]
        public void IsNullOrEmpty()
        {
            // List is empty
            var empty = new List<int>();
            Assert.IsTrue(empty.IsNullOrEmpty(), "Empty list should be empty");

            // List is not empty
            var notEmpty = new List<int> { 12, 1, 5 };
            Assert.IsFalse(notEmpty.IsNullOrEmpty(), "Not empty list should not be empty");

            // List is null
            List<int> nullList = null;
            Assert.IsTrue(nullList.IsNullOrEmpty(), "Null list with IsEmpty should return true");
        }
    }
}
