using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ComLib;
using ComLib.Types;
using ComLib.Collections;
using ComLib.Extensions;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class StringDelimitedTests
    {
        [Test]
        public void CanParseKeyValuePairs()
        {
            string test = "city=Queens, state=Ny, zipcode=12345, Country=usa";
            IDictionary<string, string> pairs = StringHelper.ToMap(test, ',', '=', false, true);

            Assert.AreEqual(pairs["city"], "Queens");
            Assert.AreEqual(pairs["state"], "Ny");
            Assert.AreEqual(pairs["zipcode"], "12345");
            Assert.AreEqual(pairs["Country"], "usa");
        }
    }



    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void CanParseDelimitedData()
        {
            string[] pageNums = StringHelper.GetDelimitedChars("search-classes-workshops-4-1-2-6",
                "search-classes-workshops-", '-');
            Assert.AreEqual(4, pageNums.Length);
            Assert.AreEqual("4", pageNums[0]);
            Assert.AreEqual("1", pageNums[1]);
            Assert.AreEqual("2", pageNums[2]);
            Assert.AreEqual("6", pageNums[3]);
        }


        [Test]
        public void CanConvertSingleWordToSentenceCase()
        {
            string lowerCase = "newyork";
            string sentenceCase = StringHelper.ConvertToSentanceCase(lowerCase, ' ');

            Assert.AreEqual("Newyork", sentenceCase);
        }


        [Test]
        public void CanConvertMultipleWordsToSentenceCase()
        {
            string lowerCase = "AMERICAN SAMOA";
            string sentenceCase = StringHelper.ConvertToSentanceCase(lowerCase, ' ');

            Assert.AreEqual("American Samoa", sentenceCase);
        }


        [Test]
        public void CanTrucateNullOrEmpty()
        {
            string txt = null;
            string t = StringHelper.Truncate(txt, 10);
            Assert.AreEqual(null, t);

            txt = string.Empty;
            string t2 = StringHelper.Truncate(txt, 10);
            Assert.AreEqual(string.Empty, t2);
        }


        [Test]
        public void CanTrucate()
        {
            string txt = "1234567890";
            string t = StringHelper.Truncate(txt, 5);
            Assert.AreEqual("12345", t);

            txt = "1234567890";
            string t2 = StringHelper.Truncate(txt, 15);
            Assert.AreEqual("1234567890", t2);
        }

        [Test]
        public void ConvertLineSeparators()
        {
            var expected = "Expected text " + StringHelper.UnixLineSeparator +
                           "Line 2 " + StringHelper.UnixLineSeparator +
                           "Line 3";
            var starting = "Expected text " + StringHelper.DosLineSeparator +
                           "Line 2 " + StringHelper.DosLineSeparator +
                           "Line 3";
            Assert.AreEqual(expected, 
                StringHelper.ConvertLineSeparators(starting,
                    StringHelper.UnixLineSeparator),
                    "Dud not correctly convert line breaks from DOS to UNIX format");
        }


        [Test]
        public void CanSubstituteValues()
        {
            Assert.AreEqual("@month_@year_@day", StringHelper.Substitute("${month}_${year}_${day}", (name) => "@" + name));
            Assert.AreEqual("_@month_@year_@day_", StringHelper.Substitute("_${month}_${year}_${day}_", (name) => "@" + name));
            Assert.AreEqual("abc_@month_@year_@day_123", StringHelper.Substitute("abc_${month}_${year}_${day}_123", (name) => "@" + name));
            Assert.AreEqual("123_@month_a_@year_b_@day_cd", StringHelper.Substitute("123_${month}_a_${year}_b_${day}_cd", (name) => "@" + name));
        }
    }


    [TestFixture]
    public class TextSplitterTests
    {
        [Test]
        public void CanSplitSingleLongWord()
        {
            string longWord = "abcdefghijklmnopqrstuvwxyz";
            string splitWord = TextSplitter.SplitWord(longWord, 5, " ");

            Assert.AreEqual("abcde fghij klmno pqrst uvwxy z", splitWord);
        }


        [Test]
        public void DoesNotSplitSingleShortWord()
        {
            string longWord = "abcdefghijklmn";
            string splitWord = TextSplitter.SplitWord(longWord, 15, " ");
            Assert.AreEqual("abcdefghijklmn", splitWord);

            longWord = null;
            splitWord = TextSplitter.SplitWord(longWord, 15, " ");
            Assert.IsNull(splitWord);

            longWord = string.Empty;
            splitWord = TextSplitter.SplitWord(longWord, 15, " ");
            Assert.AreEqual(string.Empty, splitWord);

        }


        [Test]
        public void CanCheckSingleLinWithSplitting()
        {
            string text = "kishore 12345678901234567890 reddy";
            string splitText = TextSplitter.CheckAndSplitText(text, 15);

            Assert.AreEqual("kishore 123456789012345 67890 reddy", splitText);
        }


        [Test]
        public void CanCheckSingleLineWithoutSplitting()
        {
            string text = "kishore 1234567890 1234567890 reddy";
            string splitText = TextSplitter.CheckAndSplitText(text, 15);

            Assert.AreEqual("kishore 1234567890 1234567890 reddy", splitText);
        }


        [Test]
        public void CanCheckMultiLineWithOutSplitting()
        {
            string text = "Famed archaeologist/adventurer Dr. Henry \"Indiana\" Jones" + Environment.NewLine
                        + "is called back into action when he becomes entangled in a " + Environment.NewLine
                        + "Soviet plot to uncover the secret behind mysterious artifacts" + Environment.NewLine
                        + "known as the Crystal Skulls";


            string splitText = TextSplitter.CheckAndSplitText(text, 25);

            Assert.AreEqual(text, splitText);
        }


        [Test]
        public void CanCheckMultipleWithSplitting()
        {
            string text = "Famed archaeologist/adventurer123 Dr. Henry \"Indiana\" Jones" + Environment.NewLine
                        + "is called back into action when he becomes entangled in a " + Environment.NewLine
                        + "Soviet plot to uncoverTheSecretBehindMysteriousArtifacts" + Environment.NewLine
                        + "known as the Crystal Skulls";

            string splitText = TextSplitter.CheckAndSplitText(text, 25);
            string expected = "Famed archaeologist/adventurer1 23 Dr. Henry \"Indiana\" Jones" + Environment.NewLine
                        + "is called back into action when he becomes entangled in a " + Environment.NewLine
                        + "Soviet plot to uncoverTheSecretBehindMys teriousArtifacts" + Environment.NewLine
                        + "known as the Crystal Skulls";
            Assert.AreEqual(expected, splitText);
        }
    }    
}
