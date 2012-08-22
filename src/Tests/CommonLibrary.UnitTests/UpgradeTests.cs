using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ComLib;

using NUnit.Framework;


namespace CommonLibrary.Tests
{

    [TestFixture]
    public class UpgradeTests
    {
        [Test]
        public void CanCheckIfLower()
        {
            var upgrade = new Upgrade(currentVersion: "0.9.6", catchErrors: true);
            var map = new Dictionary<string, bool>();
            map["0.9.6 < 0.9.5"]  = false;
            map["0.9.6 < 0.9.6"]  = false;
            map["0.9.6 < 0.9.7"]  = false;

            map["0.9.6 <= 0.9.5"] = false;
            map["0.9.6 <= 0.9.6"] = false;
            map["0.9.6 <= 0.9.7"] = false;

            map["0.9.6 > 0.9.5"]  = false;
            map["0.9.6 > 0.9.6"]  = false;
            map["0.9.6 > 0.9.7"]  = false;

            map["0.9.6 >= 0.9.5"] = false;
            map["0.9.6 >= 0.9.6"] = false;
            map["0.9.6 >= 0.9.7"] = false;

            map["0.9.6 = 0.9.5"]  = false;
            map["0.9.6 = 0.9.6"]  = false;
            map["0.9.6 = 0.9.7"]  = false;

            map["0.9.6 != 0.9.5"] = false;
            map["0.9.6 != 0.9.6"] = false;
            map["0.9.6 != 0.9.7"] = false;

            upgrade.ExecuteIf("<", "0.9.5",  (args) => map["0.9.6 < 0.9.5"]  = true);
            upgrade.ExecuteIf("<", "0.9.6",  (args) => map["0.9.6 < 0.9.6"]  = true);
            upgrade.ExecuteIf("<", "0.9.7",  (args) => map["0.9.6 < 0.9.7"]  = true);
            upgrade.ExecuteIf("<=", "0.9.5", (args) => map["0.9.6 <= 0.9.5"] = true);
            upgrade.ExecuteIf("<=", "0.9.6", (args) => map["0.9.6 <= 0.9.6"] = true);
            upgrade.ExecuteIf("<=", "0.9.7", (args) => map["0.9.6 <= 0.9.7"] = true);
            upgrade.ExecuteIf(">", "0.9.5",  (args) => map["0.9.6 > 0.9.5"]  = true);
            upgrade.ExecuteIf(">", "0.9.6",  (args) => map["0.9.6 > 0.9.6"]  = true);
            upgrade.ExecuteIf(">", "0.9.7",  (args) => map["0.9.6 > 0.9.7"]  = true);
            upgrade.ExecuteIf(">=", "0.9.5", (args) => map["0.9.6 >= 0.9.5"] = true);
            upgrade.ExecuteIf(">=", "0.9.6", (args) => map["0.9.6 >= 0.9.6"] = true);
            upgrade.ExecuteIf(">=", "0.9.7", (args) => map["0.9.6 >= 0.9.7"] = true);
            upgrade.ExecuteIf("=", "0.9.5",  (args) => map["0.9.6 = 0.9.5"]  = true);
            upgrade.ExecuteIf("=", "0.9.6",  (args) => map["0.9.6 = 0.9.6"]  = true);
            upgrade.ExecuteIf("=", "0.9.7",  (args) => map["0.9.6 = 0.9.7"]  = true);
            upgrade.ExecuteIf("!=", "0.9.5", (args) => map["0.9.6 != 0.9.5"] = true);
            upgrade.ExecuteIf("!=", "0.9.6", (args) => map["0.9.6 != 0.9.6"] = true);
            upgrade.ExecuteIf("!=", "0.9.7", (args) => map["0.9.6 != 0.9.7"] = true);

            Assert.AreEqual(map["0.9.6 < 0.9.5"]  , false);
            Assert.AreEqual(map["0.9.6 < 0.9.6"]  , false);
            Assert.AreEqual(map["0.9.6 < 0.9.7"]  , true);
            Assert.AreEqual(map["0.9.6 <= 0.9.5"] , false);
            Assert.AreEqual(map["0.9.6 <= 0.9.6"] , true);
            Assert.AreEqual(map["0.9.6 <= 0.9.7"] , true);
            Assert.AreEqual(map["0.9.6 > 0.9.5"]  , true);
            Assert.AreEqual(map["0.9.6 > 0.9.6"]  , false);
            Assert.AreEqual(map["0.9.6 > 0.9.7"]  , false);
            Assert.AreEqual(map["0.9.6 >= 0.9.5"] , true);
            Assert.AreEqual(map["0.9.6 >= 0.9.6"] , true);
            Assert.AreEqual(map["0.9.6 >= 0.9.7"] , false);
            Assert.AreEqual(map["0.9.6 = 0.9.5"]  , false);
            Assert.AreEqual(map["0.9.6 = 0.9.6"]  , true);
            Assert.AreEqual(map["0.9.6 = 0.9.7"]  , false);
            Assert.AreEqual(map["0.9.6 != 0.9.5"] , true);
            Assert.AreEqual(map["0.9.6 != 0.9.6"] , false);
            Assert.AreEqual(map["0.9.6 != 0.9.7"] , true);
        }
    }
}
