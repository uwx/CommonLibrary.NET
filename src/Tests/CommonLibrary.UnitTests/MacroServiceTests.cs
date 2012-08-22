using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Macros;
using CommonLibrary.Tests.Common;


namespace CommonLibrary.Tests.MacroTests
{

    [Macro(Name = "hello", DisplayName = "Widget", Description = "Renders a hello world macro", IsReusable = true)]
    public class HelloWorldMacro : IMacro
    {
        /// <summary>
        /// Process the tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string Process(Tag tag)
        {
            if(tag.Attributes.Contains("name"))
                return "hello name: " + tag.Attributes["name"].ToString() + ", message: " + tag.InnerContent;
            return "hello name: " + tag.InnerContent;
        }
    }

    
    [TestFixture]
    public class MacroServiceTests
    {
        [Test]
        public void CanLoad()
        {
            var service = new MacroService();
            service.Load(ContentLoader.TestDllName);
            var macro = service.Create("hello");

            Assert.IsTrue(service.Lookup.Count == 1);
            Assert.IsNotNull(macro);
        }


        [Test]
        public void CanProcessSimple()
        {
            var service = new MacroService(Char.MinValue, '[', ']');
            service.Load(ContentLoader.TestDllName);
            var result = service.BuildContent("[hello]kishore[/hello]");
            Assert.AreEqual(result, "hello name: kishore");
        }


        [Test]
        public void CanProcessViaLoad()
        {
            var service = new MacroService('$', '[', ']');
            service.Load(ContentLoader.TestDllName);

            var result = service.BuildContent(@"testing $[hello name=""kishore""]commonlibrary.net.cms[/hello]");
            Assert.AreEqual(result, "testing hello name: kishore, message: commonlibrary.net.cms");
        }


        [Test]
        public void CanProcessViaAPI()
        {
            var service = new MacroService('$', '[', ']');
            // 1. verbose api
            service.Register(new MacroAttribute() { Name = "helloworld1" },
                            new MacroParameterAttribute[] { new MacroParameterAttribute() { Name = "language" } },
                           (tag) => "hello world 1 : " + tag.Name + ", " + tag.InnerContent);


            // 2. quick api
            service.Register("helloworld2", "", "Simple hello world macro", (tag) => "hello world 2 : " + tag.InnerContent);

            var result1 = service.BuildContent(@"testing $[helloworld1 name=""kishore""]commonlibrary.net.cms[/hello]");
            var result2 = service.BuildContent(@"testing $[helloworld2 name=""kishore""]commonlibrary.net.cms[/hello]");
            Assert.AreEqual(result1, "testing hello world 1 : helloworld1, commonlibrary.net.cms");
            Assert.AreEqual(result2, "testing hello world 2 : commonlibrary.net.cms");
        }
        
        
        [Test]
        public void CanProcessViaAPI_EmptyTag_No_Attributes_DifferentLocations()
        {
            var service = new MacroService('$', '[', ']');
            // 1. quick api
            service.Register("helloworld1", "", "", (tag) => "hello world 1");
            service.Register("helloworld2", "", "", (tag) => "hello world 2");
            service.Register("helloworld3", "", "", (tag) => "hello world 3");
            service.Register("helloworld4", "", "", (tag) => "hello world 4");
            service.Register("helloworld5", "", "", (tag) => "hello world 5");

            // Case 1 - no external content
            var result = service.BuildContent("$[helloworld1/]");
            Assert.AreEqual(result, "hello world 1");

            // Case 2 - content after
            result = service.BuildContent("$[helloworld1/] after");
            Assert.AreEqual(result, "hello world 1 after");

            // Case 3 - content before
            result = service.BuildContent("before $[helloworld1/]");
            Assert.AreEqual(result, "before hello world 1");

            // Case 4 - content before and after
            result = service.BuildContent("before $[helloworld1/] after");
            Assert.AreEqual(result, "before hello world 1 after");
        }


        [Test]
        public void CanProcessViaAPI_EmptyTag_Attributes_DifferentLocations()
        {
            var service = new MacroService('$', '[', ']');
            // 1. quick api
            service.Register("helloworld1", "", "", (tag) => "hello world 1 " + tag.Attributes.Get<string>("saying"));

            // Case 1 - no external content
            var result = service.BuildContent("$[helloworld1 saying=\"hi\"/]");
            Assert.AreEqual(result, "hello world 1 hi");

            // Case 2 - content after
            result = service.BuildContent("$[helloworld1 saying=\"hi\"/] after");
            Assert.AreEqual(result, "hello world 1 hi after");

            // Case 3 - content before
            result = service.BuildContent("before $[helloworld1 saying=\"hi\"/]");
            Assert.AreEqual(result, "before hello world 1 hi");

            // Case 4 - content before and after
            result = service.BuildContent("before $[helloworld1 saying=\"hi\"/] after");
            Assert.AreEqual(result, "before hello world 1 hi after");
        }


        [Test]
        public void CanProcessViaAPI_NonEmptyTag_Attributes_DifferentLocations()
        {
            var service = new MacroService('$', '[', ']');
            // 1. quick api
            service.Register("helloworld1", "", "", (tag) => "hello world 1 " 
                + tag.Attributes.Get<string>("saying") + ", " + tag.InnerContent);

            // Case 1 - no external content
            var result = service.BuildContent("$[helloworld1 saying=\"hi\"]inner[/helloworld1]");
            Assert.AreEqual(result, "hello world 1 hi, inner");

            // Case 2 - content after
            result = service.BuildContent("$[helloworld1 saying=\"hi\"]inner[/helloworld1] after");
            Assert.AreEqual(result, "hello world 1 hi, inner after");

            // Case 3 - content before
            result = service.BuildContent("before $[helloworld1 saying=\"hi\"]inner[/helloworld1]");
            Assert.AreEqual(result, "before hello world 1 hi, inner");

            // Case 4 - content before and after
            result = service.BuildContent("before $[helloworld1 saying=\"hi\"]inner[/helloworld1] after");
            Assert.AreEqual(result, "before hello world 1 hi, inner after");
        }


        [Test]
        public void CanProcessViaAPI_Multiple_NonEmptyTag_Attributes_DifferentLocations()
        {
            var service = new MacroService('$', '[', ']');
            // 1. quick api
            service.Register("helloworld1", "", "", (tag) => "hello world 1 "
                + tag.Attributes.Get<string>("saying") + ", " + tag.InnerContent);
            
            // 2. quick api
            service.Register("helloworld2", "", "", (tag) => "hello world 2 "
                + tag.Attributes.Get<string>("saying") + ", " + tag.InnerContent);

            // Case 1 - no external content
            var result = service.BuildContent("$[helloworld1 saying=\"hi\"]inner[/helloworld1] $[helloworld2 saying=\"hi\"]inner[/helloworld2]");
            Assert.AreEqual(result, "hello world 1 hi, inner hello world 2 hi, inner");

            // Case 2 - content after
            result = service.BuildContent("$[helloworld1 saying=\"hi\"]inner[/helloworld1] after 1 $[helloworld2 saying=\"hi\"]inner[/helloworld2] after 2");
            Assert.AreEqual(result, "hello world 1 hi, inner after 1 hello world 2 hi, inner after 2");

            // Case 3 - content before
            result = service.BuildContent("before 1 $[helloworld1 saying=\"hi\"]inner[/helloworld1] before 2 $[helloworld2 saying=\"hi\"]inner[/helloworld2]");
            Assert.AreEqual(result, "before 1 hello world 1 hi, inner before 2 hello world 2 hi, inner");

            // Case 4 - content before and after
            result = service.BuildContent("before 1 $[helloworld1 saying=\"hi\"]inner[/helloworld1] after 1 before 2 $[helloworld2 saying=\"hi\"]inner[/helloworld2] after 2");
            Assert.AreEqual(result, "before 1 hello world 1 hi, inner after 1 before 2 hello world 2 hi, inner after 2");
        }
    }
}
