using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Macros;


namespace CommonLibrary.Tests.MarkupTests
{
    
    [TestFixture]
    public class MarkupServiceTests
    {
        private MacroDoc _domdoc;
                

        [Test]
        public void Can_Parse_Html()
        {
            Test1(char.MinValue, '<', '>', @"<div id=""2"" class=""post"">Content in here</div>", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_Html_With_Tabs()
        {
            Test1(char.MinValue, '<', '>', @"<div id=""2""  class=""post""> Content in  here        </div>",
                "div", " Content in  here        ", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_WithSpaces()
        {
            Test1(char.MinValue, '<', '>', @" <div  id=""2""  class=""post"" >  Content in here </div> ",
                "div", "  Content in here ", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 1, "  ");
        }



        [Test]
        public void Can_Parse_CustomBrackets()
        {
            Test1(char.MinValue, '[', ']', @"[div id=""2"" class=""post""]Content in here[/div]", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_All_Characters_In_Inner_Content()
        {
            Test1(char.MinValue, '[', ']', @"[div id=""2"" class=""post""]abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?[/div]",
                "div", @"abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_All_Characters_In_External_Content()
        {
            Test1('$', '[', ']', @"abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? $[div id=""2"" class=""post""]abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?[/div]abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? ",
                "div", @"abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 41, @"abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? ");
        }


        [Test]
        public void Can_Parse_Open_Close_Bracket_In_External_Content()
        {
            Test1(char.MinValue, '[', ']', @"before [] [div id=""2"" class=""post""]Content in here[/div] after[]",
                "div", @"Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 10, "before []  after[]");
        }


        [Test]
        public void Can_Parse_EmptyTag_With_No_Attributes()
        {
            Test1('$', '[', ']', @"$[date/]", "date", "", null, 0, "");
        }

        
        [Test]
        public void Can_Parse_EmptyTag_With_Attributes()
        {
            Test1(char.MinValue, '[', ']', @"-[div id=""2"" class=""post""/]-",
                "div", "", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 1, "--");
        }


        [Test]
        public void Can_Parse_With_Escaping_With_Prefix()
        {
            TestEscaping('$', '[', ']', "Escaping $$[div]", "Escaping $[div]");
        }


        [Test]
        public void Can_Parse_With_Escaping_No_Prefix()
        {
            TestEscaping(char.MinValue, '[', ']', "Escaping [[div]", "Escaping [div]");
        }


        [Test]
        public void Can_Parse_With_Escaping_With_Prefix_And_With_EmptyTag()
        {
            TestEscaping('$', '[', ']', "Escaping $$[test/] $[date/] finished", "Escaping $[test/]  finished", 1, "date", null,
                new Dictionary<string, string>()
                {

                });
        }


        [Test]
        public void Can_Parse_With_Escaping_With_Prefix_And_OpenCloseTag()
        {
            TestEscaping('$', '[', ']', @"Escaping $$[test/] $[date format=""MM"" plus=""1""]today[/date] finished", "Escaping $[test/]  finished", 1, "date", "today",
                new Dictionary<string, string>()
                {
                    { "format", "MM" }, { "plus", "1" }
                });
        }


        [Test]
        public void Can_Parse_CustomBrackets_And_Prefix()
        {
            Test1('$', '[', ']', @"$[div id=""2"" class=""post""]Content in here[/div]", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_CustomBrackets_And_Prefix_With_ExternalContent()
        {
            Test1('$', '[', ']', @"text before $[div id=""2"" class=""post""]Content in here[/div] text after",
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 12, "text before  text after");
        }


        private void Test1(char prefix, char open, char close, string content, 
            string expectedTagName, string expectedTagContent, IDictionary<string, string> args,
            int expectedTagPosition = 0, string expectedContent = "")
        {
            var doc = new MacroDocParser(prefix, open, close);
            var result = doc.Parse(content);            
            var first = result.Tags[0];
           
            Assert.AreEqual(result.Content, expectedContent);
            Assert.AreEqual(result.Tags.Count, 1);
            Assert.AreEqual(first.Position, expectedTagPosition);
            Assert.AreEqual(first.Name,  expectedTagName);
            Assert.AreEqual(first.InnerContent, expectedTagContent);
            if (args == null) return;

            foreach (var pair in args)
            {
                Assert.IsTrue(first.Attributes.Contains(pair.Key));
                Assert.AreEqual(first.Attributes[pair.Key], pair.Value);
            }
        }


        private void TestEscaping(char prefix, char open, char close, string content, string expectedContent, 
            int expectedTagCount = 0, string expectedTagName = "", string expectedTagContent = "", IDictionary<string, string> args = null)
        {
            var doc = new MacroDocParser(prefix, open, close);
            var result = doc.Parse(content);
            Assert.AreEqual(result.Content, expectedContent);
            Assert.AreEqual(result.Tags.Count, expectedTagCount);

            if(result.Tags.Count == 0 )return;

            var first = result.Tags[0];
            if (!string.IsNullOrEmpty(expectedTagName))
                Assert.AreEqual(first.Name, expectedTagName);

            if (!string.IsNullOrEmpty(expectedTagContent))
                Assert.AreEqual(first.InnerContent, expectedTagContent);

            foreach (var pair in args)
            {
                Assert.IsTrue(first.Attributes.Contains(pair.Key));
                Assert.AreEqual(first.Attributes[pair.Key], pair.Value);
            }
        }
    }
}
