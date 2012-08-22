using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ComLib.Lang.Helpers;


namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class UnitsTests
    {
        private Units Create()
        {
            var units = new Units();

            units.RegisterGroup("length", "in", "inches", "inch");            
            units.RegisterUnit("length", "ft", "foot", 12);
            units.RegisterUnit("length", "ft", "feet", 12);            
            units.RegisterUnit("length", "yd", "yard", 36);
            units.RegisterUnit("length", "yd", "yards", 36);
            units.RegisterUnit("length", "mi", "mile", 63360);
            units.RegisterUnit("length", "mi", "miles", 63360);
            return units;
        }


        [Test]
        public void Can_Register_Units()
        {
            var units = Create();
            Assert.AreEqual("inches", units.GetBaseNameFor("length"));
            Assert.AreEqual(36, units.ConversionValueFor("length", "yard"));
        }


        [Test]
        public void Can_Create_Units()
        {
            var units = Create();
            var u1 = units.ConvertToUnits(3, "feet");

            Assert.AreEqual(36, u1.BaseValue);
            Assert.AreEqual("length", u1.Group);
            Assert.AreEqual("feet", u1.SubGroup);

            Assert.AreEqual(3, units.ConvertToUnits(3, "inches").BaseValue);
            Assert.AreEqual(36, units.ConvertToUnits(3, "feet").BaseValue);
            Assert.AreEqual(108, units.ConvertToUnits(3, "yards").BaseValue);
        }



        [Test]
        public void Can_Convert_Values()
        {
            var units = Create();
            Assert.AreEqual(60, units.Convert( 5, "feet", "inches"));
            Assert.AreEqual(60, units.Convert(5, "feet", "inch"));

            Assert.AreEqual(180, units.Convert(5, "yards", "inches"));
            Assert.AreEqual(180, units.Convert(5, "yard", "inch"));
            
            Assert.AreEqual(15, units.Convert(5, "yards", "feet"));
            Assert.AreEqual(15, units.Convert(5, "yard", "feet"));
            
            Assert.AreEqual(2, units.Convert(6, "feet", "yards"));
            Assert.AreEqual(2, units.Convert(6, "foot", "yard"));
        }


        [Test]
        public void Can_Convert_Values_Using_ShortName()
        {
            var units = Create();
            Assert.AreEqual(60, units.Convert(5, "ft", "in"));
            Assert.AreEqual(60, units.Convert(5, "ft", "in"));

            Assert.AreEqual(180, units.Convert(5, "yd", "in"));
            Assert.AreEqual(180, units.Convert(5, "yd", "in"));

            Assert.AreEqual(15, units.Convert(5, "yd", "ft"));
            Assert.AreEqual(15, units.Convert(5, "yd", "ft"));

            Assert.AreEqual(2, units.Convert(6, "ft", "yd"));
            Assert.AreEqual(2, units.Convert(6, "ft", "yd"));
        }


        [Test]
        public void Can_Add_Values()
        {
            var units = Create();

            // Same order.
            Assert.AreEqual(5, units.Add(3, "inches", 2, "inches"));


            // Increasing order - starting with base
            Assert.AreEqual(27,  units.Add(3, "inches", 2, "feet"));
            Assert.AreEqual(75,  units.Add(3, "inches", 2, "yards"));

            // Decreasing order - ending with base
            Assert.AreEqual(3.5,  units.Add(3, "feet",   6, "inches"));

            // Mixed - 
            Assert.AreEqual(5,   units.Add(3, "yards",  6, "feet"));
            Assert.AreEqual(9,   units.Add(3, "feet",   2, "yards"));

            Assert.AreEqual(126722, units.Add(2, "inches", 2, "miles"));
            Assert.AreEqual(3522, units.Add(2, "yards", 2, "miles"));
        }
    }
}
