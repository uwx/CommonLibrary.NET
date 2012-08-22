using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;

namespace CommonLibrary.Tests
{

    [TestFixture]
    public class LexArgsTests
    {

        [Test]
        public void CanParseArguments()
        {
            IDictionary<string, string> args = LexArgs.Parse(" pythontests.py  --config=\"prod.config\"  --trace=4  BLOGS 'c: d: e:'");

            Assert.IsTrue(args.ContainsKey("pythontests.py"));
            Assert.IsTrue(args.ContainsKey("--config=\"prod.config\""));
            Assert.IsTrue(args.ContainsKey("--trace=4"));
            Assert.IsTrue(args.ContainsKey("BLOGS"));
            Assert.IsTrue(args.ContainsKey("c: d: e:"));
            Assert.AreEqual(args.Count, 5);
        }


        [Test]
        public void CanParseArgumentsQuoted()
        {
            IDictionary<string, string> args = LexArgs.Parse(" 'squote' 's\"quote' 's\\'quote' \"dquote\" \"d'quote\" \"d\\\"quote\"");

            Assert.IsTrue(args.ContainsKey("squote"));
            Assert.IsTrue(args.ContainsKey("s\"quote"));
            Assert.IsTrue(args.ContainsKey("s'quote"));
            Assert.IsTrue(args.ContainsKey("dquote"));
            Assert.IsTrue(args.ContainsKey("d'quote"));
            Assert.IsTrue(args.ContainsKey("d\"quote"));
            Assert.AreEqual(args.Count, 6);
        }


        [Test]
        public void CanParseArgumentsWithTabs()
        {
            IDictionary<string, string> args = LexArgs.Parse(" 'squote'	's\"quote'	's\\'quote'");
            
            Assert.IsTrue(args.ContainsKey("squote"));
            Assert.IsTrue(args.ContainsKey("s\"quote"));
            Assert.IsTrue(args.ContainsKey("s'quote"));
            Assert.AreEqual(args.Count, 3);
        }


        [Ignore]
        public void CanParseArgumentsWithNewLine()
        {
            IDictionary<string, string> args = LexArgs.Parse(" appid=knowledgedrink QA " + Environment.NewLine);
            Assert.AreEqual(args.Count, 2);
            Assert.IsTrue(args.ContainsKey("appid=knowledgedrink"));
            Assert.IsTrue(args.ContainsKey("QA"));
        }
    }



    [TestFixture]
    public class LexListTests
    {
        [Test]
        public void CanParseNonQuotedListWithCommaSeparator()
        {
            string line = "batman, bruce wayne, gambit";
            IDictionary<string, string> items = LexList.Parse(line);

            Assert.IsTrue(items.ContainsKey("batman"));
            Assert.IsTrue(items.ContainsKey("bruce wayne"));
            Assert.IsTrue(items.ContainsKey("gambit"));
        }


        [Test]
        public void CanParseSingleQuotedListWithCommaSeparator()
        {
            string line = "'batman', 'bruce wayne', 'gambit'";
            IDictionary<string, string> items = LexList.Parse(line);

            Assert.IsTrue(items.ContainsKey("batman"));
            Assert.IsTrue(items.ContainsKey("bruce wayne"));
            Assert.IsTrue(items.ContainsKey("gambit"));
        }


        [Test]
        public void CanParseMultiLineSingleQuotedListWithCommaSeparator()
        {
            string line = "'batman', 'lantern', 'superman' " + Environment.NewLine
                        + "'cyclops', 'colossus', 'logon'";
            List<List<string>> items = LexList.ParseTable(line);

            Assert.IsTrue(items[0].Contains("batman"));
            Assert.IsTrue(items[0].Contains("lantern"));
            Assert.IsTrue(items[0].Contains("superman"));
            Assert.IsTrue(items[1].Contains("cyclops"));
            Assert.IsTrue(items[1].Contains("colossus"));
            Assert.IsTrue(items[1].Contains("logon"));
        }


        [Test]
        public void CanParseSingleQuotedListWithCommaSeparatorWithInnerComma()
        {
            string line = "'batman', 'bruce, wayne', 'gambit'";
            IDictionary<string, string> items = LexList.Parse(line);

            Assert.IsTrue(items.ContainsKey("batman"));
            Assert.IsTrue(items.ContainsKey("bruce, wayne"));
            Assert.IsTrue(items.ContainsKey("gambit"));
        }


        [Test]
        public void CanParseDoubleQuotedListWithCommaSeparatorWithInnerComma()
        {
            string line = "\"batman\", \"bruce, wayne\", \"gambit\"";
            IDictionary<string, string> items = LexList.Parse(line);

            Assert.IsTrue(items.ContainsKey("batman"));
            Assert.IsTrue(items.ContainsKey("bruce, wayne"));
            Assert.IsTrue(items.ContainsKey("gambit"));
        }


        [Test]
        public void CanParseQuotedListNewLine()
        {
            string line = "'batman', 'bruce wayne', 'gambit'\r\n";
            IDictionary<string, string> items = LexList.Parse(line);

            Assert.IsTrue(items.ContainsKey("batman"));
            Assert.IsTrue(items.ContainsKey("bruce wayne"));
            Assert.IsTrue(items.ContainsKey("gambit"));
        }
    }
}
