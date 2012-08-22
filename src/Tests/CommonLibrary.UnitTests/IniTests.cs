using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.IO;



namespace CommonLibrary.Tests
{
   
    [TestFixture]
    public class IniTests
    {
        [Test]
        public void CanLoadEmptyDoc()
        {
            IniDocument doc = new IniDocument();
            doc["globalSettings", "time"] = "all";
            doc["globalSettings", "category"] = "Art,Drawing";
            doc["", "pageSize"] = 20;

            Assert.IsTrue(doc.Contains("globalSettings"));
            Assert.AreEqual(doc["globalSettings", "category"], "Art,Drawing");
            Assert.AreEqual(doc["", "pageSize"], 20);
        }


        [Test]
        public void CanParseSingleGroupCaseSensitive()
        {
            string iniContent = "[globalSettings]" + Environment.NewLine
                              + "time: all" + Environment.NewLine
                              + "category: Art,Drawing";

            IniDocument document = new IniDocument(iniContent, false);
            Assert.IsTrue(document.Contains("globalSettings"));
            Assert.AreEqual(document["globalSettings", "category"], "Art,Drawing");
        }


        [Test]
        public void CanParseCaseInsensitiveGroup()
        {
            string iniContent = "[GlobalSettings]" + Environment.NewLine
                              + "Time: all" + Environment.NewLine
                              + "Category: Art,Drawing";

            IniDocument document = new IniDocument(iniContent, false, false);
            Assert.IsTrue(document.Contains("globalsettings"));
            Assert.AreEqual(document["globalsettings", "category"], "Art,Drawing");
        }


        [Test]
        public void CanParseMultipleGroups()
        {
            string iniContent = "[globalSettings]"      + Environment.NewLine
                              + "time: all"             + Environment.NewLine
                              + "category: Art,Drawing" + Environment.NewLine
                              + "[post1]"               + Environment.NewLine
                              + "title:Build a website" + Environment.NewLine
                              + "CreatedBy: user1";

            IniDocument document = new IniDocument(iniContent, false);
            Assert.IsTrue(document.Contains("globalSettings"));
            Assert.AreEqual(document["globalSettings", "category"], "Art,Drawing");

            Assert.AreEqual(document["post1","title"], "Build a website");
            Assert.AreEqual(document["post1","CreatedBy"], "user1");
        }


        [Test]
        public void CanParseMultipleGroupsWithSameName()
        {
            string iniContent = "[post]" + Environment.NewLine
                              + "title:Build a website0" + Environment.NewLine
                              + "CreatedBy: user0" + Environment.NewLine + Environment.NewLine
                              + "[post]" + Environment.NewLine
                              + "title:Build a website1" + Environment.NewLine
                              + "CreatedBy: user1";

            IniDocument doc = new IniDocument(iniContent, false);
            Assert.IsTrue(doc.Contains("post"));
            Assert.AreEqual(doc.Count, 1);
            Assert.AreEqual(doc.GetSection("post", 0).Get<string>("title"), "Build a website0");
            Assert.AreEqual(doc.GetSection("post", 0).Get<string>("CreatedBy"), "user0");
            Assert.AreEqual(doc.GetSection("post", 1).Get<string>("title"), "Build a website1");
            Assert.AreEqual(doc.GetSection("post", 1).Get<string>("CreatedBy"), "user1");

        }


        [Test]
        public void CanParseMultilineValueWithSingleGroup()
        {
            string iniContent = "[globalSettings]" + Environment.NewLine
                              + "Title:Learn painting" + Environment.NewLine
                              + "Description:\"Learn to paint" + Environment.NewLine 
                              + " using oil\"" + Environment.NewLine
                              + "[post1]" + Environment.NewLine
                              + "title:Build a website" + Environment.NewLine
                              + "CreatedBy: user1";

            IniDocument document = new IniDocument(iniContent, false);
            string title = document.Get<string>("globalSettings", "Title");
            string desc = document.Get<string>("globalSettings", "Description");

            Assert.AreEqual(title, "Learn painting");
            Assert.AreEqual(desc, "Learn to paint" + Environment.NewLine + " using oil");

            Assert.AreEqual(document["post1", "title"], "Build a website");
            Assert.AreEqual(document["post1", "CreatedBy"], "user1");

            //Assert.IsTrue(groupedDict.ContainsKey("globalSettings"));
            //Assert.AreEqual(groupedDict["globalSettings"]["category"], "Art,Drawing");
            //Assert.IsTrue(groupedDict.ContainsKey("post1"));
            //Assert.AreEqual(groupedDict["post1"]["title"], "Build a website");
            //Assert.AreEqual(groupedDict["post1"]["CreatedBy"], "user1");
        }



        [Test]
        public void CanParseWithMultiLinesAfterGroups()
        {
            string iniContent = "[globalSettings]" + Environment.NewLine
                              + "Title:Learn painting" + Environment.NewLine
                              + "Description:Learn to paint" + Environment.NewLine + Environment.NewLine + Environment.NewLine
                              + "[post1]" + Environment.NewLine
                              + "title:Build a website" + Environment.NewLine
                              + "CreatedBy: user1";

            IniDocument document = new IniDocument(iniContent, false);
            string title = document.Get<string>("globalSettings", "Title");
            string desc = document.Get<string>("globalSettings", "Description");

            Assert.AreEqual(title, "Learn painting");
            Assert.AreEqual(desc, "Learn to paint");

            Assert.AreEqual(document["post1", "title"], "Build a website");
            Assert.AreEqual(document["post1", "CreatedBy"], "user1");
        }


        //[Test]
        public void CanParseFailed()
        {
            string iniContent = @"F:\Dev\Workshops\knowledgedrink\src\Tests\CommonLibrary.WebModules.Tests\Workshops\Import\FileDiverse_failed_small.txt";
            
            IniDocument document = new IniDocument(iniContent, true, false);
            Assert.IsTrue(document.Contains("globalsettings"));
            Assert.AreEqual(document["globalsettings", "category"], "Art,Sculpture");

            document.Save(@"F:\Dev\Workshops\knowledgedrink\src\Tests\failureTest.txt");
        }
    }


    public enum SkillLevel2 { All = 0, Beginner = 1, Intermediate = 2, Advanced = 3 };


    [TestFixture]
    public class EnumLookupTests
    {
        [Test]
        public void CanParseEnums()
        {
            EnumLookup.Register(typeof(SkillLevel2), "pro=Advanced,guru=Advanced,master=Advanced");
            SkillLevel2 skill = (SkillLevel2)EnumLookup.GetValue(typeof(SkillLevel2), "guru");
            Assert.AreEqual(skill, SkillLevel2.Advanced);
        }
    }
}
