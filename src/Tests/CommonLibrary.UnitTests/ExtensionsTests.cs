using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;

using ComLib;
using ComLib.Extensions;

namespace CommonLibrary.Tests
{

    [TestFixture]
    public class ExtensionsTests
    {
        [Test]
        public void CanParseStringAsBool()
        {
            Assert.AreEqual(StringExtensions.ToBool("yes"), true);
            Assert.AreEqual(StringExtensions.ToBool("true"), true);
            Assert.AreEqual(StringExtensions.ToBool("YES"), true);
            Assert.AreEqual(StringExtensions.ToBool("TrUe"), true);
            Assert.AreEqual(StringExtensions.ToBool("1"), true);
            Assert.AreEqual(StringExtensions.ToBool("no"), false);
            Assert.AreEqual(StringExtensions.ToBool("false"), false);
            Assert.AreEqual(StringExtensions.ToBool("0"), false);
        }


        [Test]
        public void CanParseStringAsIntWithMoney()
        {
            Assert.AreEqual(StringExtensions.ToInt("100"), 100);
            Assert.AreEqual(StringExtensions.ToInt("$100"), 100);
            Assert.AreEqual(StringExtensions.ToInt(" 100"), 100);
            Assert.AreEqual(StringExtensions.ToInt(" $100 "), 100);
            Assert.AreEqual(StringExtensions.ToInt(" 100" + CultureInfoHelper.GetDecimalSeparator() + "55"), 101);
            Assert.AreEqual(StringExtensions.ToInt(" $100" + CultureInfoHelper.GetDecimalSeparator() + "55 "), 101);
            Assert.AreEqual(StringExtensions.ToInt(" $100" + CultureInfoHelper.GetDecimalSeparator() + "35 "), 100);
        }


        [Test]
        public void CanParseStringAsDoubleWithMoney()
        {
            Assert.AreEqual(StringExtensions.ToDouble("101" + CultureInfoHelper.GetDecimalSeparator() + "5"), 101.5);
            Assert.AreEqual(StringExtensions.ToDouble("$101"), 101);
            Assert.AreEqual(StringExtensions.ToDouble(" 101"), 101);
            Assert.AreEqual(StringExtensions.ToDouble(" $101 "), 101);
            Assert.AreEqual(StringExtensions.ToDouble("101"), 101);
            Assert.AreEqual(StringExtensions.ToDouble("$101"), 101);
            Assert.AreEqual(StringExtensions.ToDouble(" 101" + CultureInfoHelper.GetDecimalSeparator() + "55"), 101.55);
            Assert.AreEqual(StringExtensions.ToDouble(" $101" + CultureInfoHelper.GetDecimalSeparator() + "55 "), 101.55);
            Assert.AreEqual(StringExtensions.ToDouble(" $101" + CultureInfoHelper.GetDecimalSeparator() + "35 "), 101.35);
        }

        
        [Test]
        public void CanParseStringAsDate()
        {
            Assert.AreEqual(StringExtensions.ToDateTime("${t}").Date, DateTime.Today.Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${t-1}").Date, DateTime.Today.AddDays(-1).Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${t-4}").Date, DateTime.Today.AddDays(-4).Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${t+1}").Date, DateTime.Today.AddDays(1).Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${t+4}").Date, DateTime.Today.AddDays(4).Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${today}").Date, DateTime.Today.Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${yesterday}").Date, DateTime.Today.AddDays(-1).Date);
            Assert.AreEqual(StringExtensions.ToDateTime("${tommorrow}").Date, DateTime.Today.AddDays(1).Date);
        }


        [Test]
        public void CanParseStringAsTime()
        {
            Assert.AreEqual(StringExtensions.ToTime("9"), new TimeSpan(9, 0, 0)); 
            Assert.AreEqual(StringExtensions.ToTime("9am"), new TimeSpan(9, 0, 0));
            Assert.AreEqual(StringExtensions.ToTime("9pm"), new TimeSpan(21, 0, 0));
            Assert.AreEqual(StringExtensions.ToTime("9 am"), new TimeSpan(9, 0, 0));
            Assert.AreEqual(StringExtensions.ToTime("9 pm"), new TimeSpan(21, 0, 0));
            Assert.AreEqual(StringExtensions.ToTime("9:35"), new TimeSpan(9, 35, 0));
            Assert.AreEqual(StringExtensions.ToTime("9:35am"), new TimeSpan(9, 35, 0));
            Assert.AreEqual(StringExtensions.ToTime("9:35pm"), new TimeSpan(21, 35, 0));
            Assert.AreEqual(StringExtensions.ToTime("9:35 am"), new TimeSpan(9, 35, 0));
            Assert.AreEqual(StringExtensions.ToTime("9:35 pm"), new TimeSpan(21, 35, 0));
            Assert.AreEqual(StringExtensions.ToTime("12am"), new TimeSpan(0, 0, 0));
            Assert.AreEqual(StringExtensions.ToTime("12pm"), new TimeSpan(12, 0, 0));
        }

        [Test]
        public void Levenshtein()
        {
            // Example with the same string
            var same = "I am a string";
            var sameResult = StringExtensions.Levenshtein(same, same);
            Console.WriteLine("Difference for same string: {0}", sameResult);
            Assert.AreEqual(0, sameResult, "Both strings are equal - " +
                "the Levenshtein algorithm should return 0");

            // Wikipedia example
            var result = StringExtensions.Levenshtein("kitten", "sitting");
            Console.WriteLine("Difference 'for kitten' and 'sitting': {0}", result);
            Assert.AreEqual(3, result, "Unexpected Levenshtein value for strings");
        }

        [Test]
        public void SimplifiedSoundex()
        {
            /* Test to see if soundex values correspond to known trouble values
             * as published on the web. */
            Assert.AreEqual("R163", "Robert".SimplifiedSoundex(4));
            Assert.AreEqual("B625", "Baragwanath".SimplifiedSoundex(4));
            Assert.AreEqual("D540", "Donnell".SimplifiedSoundex(4));
            Assert.AreEqual("L300", "Lloyd".SimplifiedSoundex(4));
            Assert.AreEqual("W422", "Woolcock".SimplifiedSoundex(4));
        }


        #region Hexadecimal and binary
        [Test]
        public void TestHexadecimalValidation()
        {
            Assert.IsTrue("01898ffFFeabcD".IsHex());
            Assert.IsFalse("z01898ffFFeabcD".IsHex());
            Assert.IsFalse("".IsHex());
        }


        [Test]
        public void TestBinaryValidation()
        {
            Assert.IsTrue("010100100101001".IsBinary());
            Assert.IsFalse("0101001001010012345".IsBinary());
            Assert.IsFalse("".IsBinary());
        }


        [Test]
        public void TestHexByteConversions()
        {
            byte[] bytes = { 1, 2, 3, 4, 5, 255 , 16};
            byte[] newBytes = "0102030405FF10".HexToByteArray();
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                Assert.AreEqual(bytes[i], newBytes[i]);
            }

            Assert.AreEqual("0102030405FF10", newBytes.ByteArrayToHex());
        }

        [Test]
        public void TestHexadecimalConversions()
        {
            // Test string hex conversions --> binary.
            Assert.AreEqual("0", "0".HexToBinary());
            Assert.AreEqual("1", "1".HexToBinary());
            Assert.AreEqual("10", "2".HexToBinary());
            Assert.AreEqual("11", "3".HexToBinary());
            Assert.AreEqual("100", "4".HexToBinary());
            Assert.AreEqual("101", "5".HexToBinary());
            Assert.AreEqual("110", "6".HexToBinary());
            Assert.AreEqual("111", "7".HexToBinary());
            Assert.AreEqual("1000", "8".HexToBinary());
            Assert.AreEqual("1001", "9".HexToBinary());
            Assert.AreEqual("1010", "A".HexToBinary());
            Assert.AreEqual("1011", "B".HexToBinary());
            Assert.AreEqual("1100", "C".HexToBinary());
            Assert.AreEqual("1101", "D".HexToBinary());
            Assert.AreEqual("1110", "E".HexToBinary());
            Assert.AreEqual("1111", "F".HexToBinary());
            Assert.AreEqual("10000", "10".HexToBinary());
            Assert.AreEqual("1000101000111", "1147".HexToBinary());

            // Test string hex conversions --> decimal.
            Assert.AreEqual("0", "0".HexToDecimal());
            Assert.AreEqual("1", "1".HexToDecimal());
            Assert.AreEqual("2", "2".HexToDecimal());
            Assert.AreEqual("3", "3".HexToDecimal());
            Assert.AreEqual("4", "4".HexToDecimal());
            Assert.AreEqual("5", "5".HexToDecimal());
            Assert.AreEqual("6", "6".HexToDecimal());
            Assert.AreEqual("7", "7".HexToDecimal());
            Assert.AreEqual("8", "8".HexToDecimal());
            Assert.AreEqual("9", "9".HexToDecimal());
            Assert.AreEqual("10", "A".HexToDecimal());
            Assert.AreEqual("11", "B".HexToDecimal());
            Assert.AreEqual("12", "C".HexToDecimal());
            Assert.AreEqual("13", "D".HexToDecimal());
            Assert.AreEqual("14", "E".HexToDecimal());
            Assert.AreEqual("15", "F".HexToDecimal());
            Assert.AreEqual("16", "10".HexToDecimal());
            Assert.AreEqual("4423", "1147".HexToDecimal());
        }


        [Test]
        public void TestBinaryConversions()
        {
            // Test string binary conversions --> hex.
            Assert.AreEqual("0", "0".BinaryToHex());
            Assert.AreEqual("1", "1".BinaryToHex());
            Assert.AreEqual("2", "10".BinaryToHex());
            Assert.AreEqual("3", "11".BinaryToHex());
            Assert.AreEqual("4", "100".BinaryToHex());
            Assert.AreEqual("5", "101".BinaryToHex());
            Assert.AreEqual("6", "110".BinaryToHex());
            Assert.AreEqual("7", "111".BinaryToHex());
            Assert.AreEqual("8", "1000".BinaryToHex());
            Assert.AreEqual("9", "1001".BinaryToHex());
            Assert.AreEqual("A", "1010".BinaryToHex().ToUpper());
            Assert.AreEqual("B", "1011".BinaryToHex().ToUpper());
            Assert.AreEqual("C", "1100".BinaryToHex().ToUpper());
            Assert.AreEqual("D", "1101".BinaryToHex().ToUpper());
            Assert.AreEqual("E", "1110".BinaryToHex().ToUpper());
            Assert.AreEqual("F", "1111".BinaryToHex().ToUpper());
            Assert.AreEqual("10", "10000".BinaryToHex());
            Assert.AreEqual("1147", "1000101000111".BinaryToHex());

            // Test string binary conversions --> decimal.
            Assert.AreEqual("0", "0".BinaryToDecimal());
            Assert.AreEqual("1", "1".BinaryToDecimal());
            Assert.AreEqual("2", "10".BinaryToDecimal());
            Assert.AreEqual("3", "11".BinaryToDecimal());
            Assert.AreEqual("4", "100".BinaryToDecimal());
            Assert.AreEqual("5", "101".BinaryToDecimal());
            Assert.AreEqual("6", "110".BinaryToDecimal());
            Assert.AreEqual("7", "111".BinaryToDecimal());
            Assert.AreEqual("8", "1000".BinaryToDecimal());
            Assert.AreEqual("9", "1001".BinaryToDecimal());
            Assert.AreEqual("10", "1010".BinaryToDecimal());
            Assert.AreEqual("11", "1011".BinaryToDecimal());
            Assert.AreEqual("12", "1100".BinaryToDecimal());
            Assert.AreEqual("13", "1101".BinaryToDecimal());
            Assert.AreEqual("14", "1110".BinaryToDecimal());
            Assert.AreEqual("15", "1111".BinaryToDecimal());
            Assert.AreEqual("16", "10000".BinaryToDecimal());
            Assert.AreEqual("4423", "1000101000111".BinaryToDecimal());
        }


        [Test]
        public void TestDecimalConversions()
        {
            // Test string decimal conversions --> hex.
            Assert.AreEqual("0", "0".DecimalToHex());
            Assert.AreEqual("1", "1".DecimalToHex());
            Assert.AreEqual("2", "2".DecimalToHex());
            Assert.AreEqual("3", "3".DecimalToHex());
            Assert.AreEqual("4", "4".DecimalToHex());
            Assert.AreEqual("5", "5".DecimalToHex());
            Assert.AreEqual("6", "6".DecimalToHex());
            Assert.AreEqual("7", "7".DecimalToHex());
            Assert.AreEqual("8", "8".DecimalToHex());
            Assert.AreEqual("9", "9".DecimalToHex());
            Assert.AreEqual("A", "10".DecimalToHex().ToUpper());
            Assert.AreEqual("B", "11".DecimalToHex().ToUpper());
            Assert.AreEqual("C", "12".DecimalToHex().ToUpper());
            Assert.AreEqual("D", "13".DecimalToHex().ToUpper());
            Assert.AreEqual("E", "14".DecimalToHex().ToUpper());
            Assert.AreEqual("F", "15".DecimalToHex().ToUpper());
            Assert.AreEqual("10", "16".DecimalToHex());
            Assert.AreEqual("1147", "4423".DecimalToHex());

            // Test string decimal conversions --> binary.
            Assert.AreEqual("0", "0".DecimalToBinary());
            Assert.AreEqual("1", "1".DecimalToBinary());
            Assert.AreEqual("10", "2".DecimalToBinary());
            Assert.AreEqual("11", "3".DecimalToBinary());
            Assert.AreEqual("100", "4".DecimalToBinary());
            Assert.AreEqual("101", "5".DecimalToBinary());
            Assert.AreEqual("110", "6".DecimalToBinary());
            Assert.AreEqual("111", "7".DecimalToBinary());
            Assert.AreEqual("1000", "8".DecimalToBinary());
            Assert.AreEqual("1001", "9".DecimalToBinary());
            Assert.AreEqual("1010", "10".DecimalToBinary());
            Assert.AreEqual("1011", "11".DecimalToBinary());
            Assert.AreEqual("1100", "12".DecimalToBinary());
            Assert.AreEqual("1101", "13".DecimalToBinary());
            Assert.AreEqual("1110", "14".DecimalToBinary());
            Assert.AreEqual("1111", "15".DecimalToBinary());
            Assert.AreEqual("10000", "16".DecimalToBinary());
            Assert.AreEqual("1000101000111", "4423".DecimalToBinary());

            // Test integer conversions --> hex.
            Assert.AreEqual("0", 0.ToHex());
            Assert.AreEqual("1", 1.ToHex());
            Assert.AreEqual("2", 2.ToHex());
            Assert.AreEqual("3", 3.ToHex());
            Assert.AreEqual("4", 4.ToHex());
            Assert.AreEqual("5", 5.ToHex());
            Assert.AreEqual("6", 6.ToHex());
            Assert.AreEqual("7", 7.ToHex());
            Assert.AreEqual("8", 8.ToHex());
            Assert.AreEqual("9", 9.ToHex());
            Assert.AreEqual("A", 10.ToHex().ToUpper());
            Assert.AreEqual("B", 11.ToHex().ToUpper());
            Assert.AreEqual("C", 12.ToHex().ToUpper());
            Assert.AreEqual("D", 13.ToHex().ToUpper());
            Assert.AreEqual("E", 14.ToHex().ToUpper());
            Assert.AreEqual("F", 15.ToHex().ToUpper());
            Assert.AreEqual("10", 16.ToHex());
            Assert.AreEqual("1147", 4423.ToHex());

            // Test integer conversions --> binary.
            Assert.AreEqual("0", 0.ToBinary());
            Assert.AreEqual("1", 1.ToBinary());
            Assert.AreEqual("10", 2.ToBinary());
            Assert.AreEqual("11", 3.ToBinary());
            Assert.AreEqual("100", 4.ToBinary());
            Assert.AreEqual("101", 5.ToBinary());
            Assert.AreEqual("110", 6.ToBinary());
            Assert.AreEqual("111", 7.ToBinary());
            Assert.AreEqual("1000", 8.ToBinary());
            Assert.AreEqual("1001", 9.ToBinary());
            Assert.AreEqual("1010", 10.ToBinary());
            Assert.AreEqual("1011", 11.ToBinary());
            Assert.AreEqual("1100", 12.ToBinary());
            Assert.AreEqual("1101", 13.ToBinary());
            Assert.AreEqual("1110", 14.ToBinary());
            Assert.AreEqual("1111", 15.ToBinary());
            Assert.AreEqual("10000", 16.ToBinary());
            Assert.AreEqual("1000101000111", 4423.ToBinary());
        }
        #endregion


        #region Replacements
        [Test]
        public void ReplacementTests()
        {
            Assert.AreEqual("12A4B6", "123456".ReplaceChars("35", "AB"));
            Assert.AreEqual("      ", "123456".ReplaceChars("123456", "      "));
            Assert.AreEqual("12A4B66B4A21", "123456654321".ReplaceChars("35", "AB"));
        }
        #endregion


        #region Byte array
        [Test]
        public void TestByteConversionsUsingDefaultCodepage()
        {
            byte[] bytes = { };
            Assert.IsNull(bytes.StringFromBytes());

            bytes = new byte[100];
            Random rnd = new Random();
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                bytes[i] = (byte)rnd.Next(127, 255);
            }

            String rndString = bytes.StringFromBytes();
            byte[] newBytes = rndString.ToBytes();

            Assert.AreEqual(bytes, newBytes);
        }


        [Test]
        public void TestByteConversionsASCII()
        {
            byte[] bytes = { };
            Assert.IsNull(bytes.StringFromBytesASCII());

            bytes = new byte[100];
            Random rnd = new Random();
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                bytes[i] = (byte)rnd.Next(97, 122);
            }

            String rndString = bytes.StringFromBytesASCII();
            byte[] newBytes = rndString.ToBytesAscii();

            Assert.AreEqual(bytes, newBytes);
        }
        #endregion


        #region DecimalExtensions
        [Test]
        public void DigitAtPosition()
        {
            const decimal number = 328.24568015823m;
            var decimalArray = "24568015823".ToCharArray();

            for (var i = 0; i < decimalArray.Length; i++)
            {
                var expected = decimalArray[i].ToString();
                var actual = number.DigitAtPosition(i + 1).ToString();
                Assert.AreEqual(expected, actual,
                    "Decimal value at position was incorrect");
            }
        }

        [Test]
        public void Pow()
        {
            Assert.AreEqual(16m, 4m.Pow(2), "4^2 should equal 16");
            Assert.AreEqual(0.25m, 4m.Pow(-1), "4^-1 should equal 1/4");
            Assert.AreEqual(1237940039285380274899124224m, 1024m.Pow(9),
                "1024^9 should equal 1237940039285380274899124224");
            Assert.AreEqual(2m, 16m.Pow(0.25m), "16^0.25 should equal 2");
        }
        #endregion

    }
}
