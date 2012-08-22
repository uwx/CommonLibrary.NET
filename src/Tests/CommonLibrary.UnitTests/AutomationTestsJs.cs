using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml;
using NUnit.Framework;

using ComLib;
using ComLib.Automation;

using CommonLibrary.Tests.Common;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class AutomationTestsJs
    {

        [Test]
        public void CanParseVariableRef()
        {
            var js = ContentLoader.GetTextFileContent("Lang.script1.js");
            var runner = new AutomationRunner("js", ContentLoader.TestDllName);
            
            var results = runner.RunText(js);
            int hour = runner.Script.Scope.Get<int>("hour");
            Assert.AreEqual(hour, DateTime.Now.Hour);
        }


        [Test]
        public void CanGetSampleRun()
        {
            var svc = new CommandService();
            svc.Load(ContentLoader.TestDllName);
            var example = svc.GetSampleRun("HelloWorldAutoMap");
            Assert.AreEqual(example, "HelloWorldAutoMap( { User : \"user01\", Age : true, IsActive : 32, BirthDate : \"06/20/2011\"} );");
        }


        [Test]
        public void CanCallFuncWithAutoMap()
        {
            var js = ContentLoader.GetTextFileContent("Lang.script1.js");
            var runner = new AutomationRunner("js", ContentLoader.TestDllName);

            var results = runner.RunText(js);

            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 17);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, @"HelloWorldAutoMap kishore true 32 2/2/1979");    
        }


        [Test]
        public void CanCallFunc()
        {
            var js = ContentLoader.GetTextFileContent("Lang.script3.js");
            var runner = new AutomationRunner("js", ContentLoader.TestDllName);

            var results = runner.RunText(js);

            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorld");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 14);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, @"HelloWorld return value");
        }


        [Test]
        public void CanCallFuncWithVarRefs()
        {
            var js = ContentLoader.GetTextFileContent("Lang.script2.js");
            var runner = new AutomationRunner("js", ContentLoader.TestDllName);

            var results = runner.RunText(js);


            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 18);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorldAutoMap kishore true 32 2/2/1979");
        }


        [Test]
        public void CanCallMutlipleMethods()
        {
            var js = ContentLoader.GetTextFileContent("Lang.script4.js");
            var runner = new AutomationRunner("js", ContentLoader.TestDllName);

            var results = runner.RunText(js);


            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "BlogTest.Delete");
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "Delete called with : 2");
            Assert.AreEqual(runner.Result.CommandResults[1].Name, "BlogTest.Create");
            Assert.AreEqual((string)runner.Result.CommandResults[1].Item, "Create called with : blog 1, 20, True");
            Assert.AreEqual(runner.Result.CommandResults[2].Name, "BlogTest.Create");
            Assert.AreEqual((string)runner.Result.CommandResults[2].Item, "Create called with : blog 1, 20, True");
            Assert.AreEqual(runner.Result.CommandResults[3].Name, "BlogTest.DeleteAll");
            Assert.AreEqual((string)runner.Result.CommandResults[3].Item, "DeleteAll called");
        }


        [Test]
        public void CanParseFuncCallsUsingIndexPositions()
        {
            var funcCalls = new string[]
            {
                @"HelloWorld(""abc"", 123, true, false, " + (23.56).ToString("N2") + ", 'kishore');",
                @"HelloWorld( ""abc"", 123, true, false, " + (23.56).ToString("N2") + ", 'kishore');",
                @"HelloWorld( ""abc"" , 123 , true , false , " + (23.56).ToString("N2") + " , 'kishore' );",
                @"HelloWorld(""abc"",123,true,false,23.56,'kishore');"            
            };
            var interpreter = new Interpreter(new Scope());
            foreach (var line in funcCalls)
            {
                var result = interpreter.TokenizeFunctionCall(line, 1);
                Assert.AreEqual(result.ParamMap["0"], "abc");
                Assert.AreEqual(result.ParamMap["1"], 123);
                Assert.AreEqual(result.ParamMap["2"], true);
                Assert.AreEqual(result.ParamMap["3"], false);
                Assert.AreEqual(result.ParamMap["4"], 23.56);
                Assert.AreEqual(result.ParamMap["5"], "kishore");
            }
        }


        [Test]
        public void CanParseFuncCallsUsingJsonNamedParameters()
        {
            var funcCalls = new string[]
            {
                @"HelloWorld({first:""abc"",age:123,isActive:true,isRegistered:false,cost:" + (23.56).ToString("N2") + ",alias:'kishore'});",
                @"HelloWorld( {first:""abc"",age:123,isActive:true,isRegistered:false,cost:" + (23.56).ToString("N2") + ",alias:'kishore'} );",
                @"HelloWorld({ first:""abc"",age:123,isActive:true,isRegistered:false,cost:" + (23.56).ToString("N2") + ",alias:'kishore' });",
                @"HelloWorld( { first:""abc"",age:123,isActive:true,isRegistered:false,cost:" + (23.56).ToString("N2") + ",alias:'kishore' } );",
                @"HelloWorld( { first:""abc"", age:123, isActive:true, isRegistered:false, cost:" + (23.56).ToString("N2") + ", alias:'kishore' } );",
                @"HelloWorld( { first : ""abc"", age : 123, isActive : true, isRegistered : false, cost : " + (23.56).ToString("N2") + ", alias : 'kishore' } );",
                @"HelloWorld( { first : ""abc"" , age : 123 , isActive : true , isRegistered : false , cost : " + (23.56).ToString("N2") + " , alias : 'kishore' } );",
                @"HelloWorld(  {  first  :  ""abc""  ,  age  :  123  ,  isActive  :  true  ,  isRegistered  :  false  ,  cost  :  23.56  ,  alias  :  'kishore'  }  );"
            };
            var interpreter = new Interpreter(new Scope());
            for(int ndx = 0; ndx< funcCalls.Length; ndx++)
            {
                var line = funcCalls[ndx];
                var result = interpreter.TokenizeFunctionCall(line, 1);
                Assert.AreEqual(result.ParamMap["first"], "abc");
                Assert.AreEqual(result.ParamMap["age"], 123);
                Assert.AreEqual(result.ParamMap["isActive"], true);
                Assert.AreEqual(result.ParamMap["isRegistered"], false);
                Assert.AreEqual(result.ParamMap["cost"], 23.56);
                Assert.AreEqual(result.ParamMap["alias"], "kishore");
            }
        }
    }


    [Command(Name = "BlogTest", Description = "Sample command that supports multiple method calls", IsMultiMethodEnabled = true, AutoHandleMethodCalls = true)]
    public class BlogTest : Command
    {

        public string Delete(int id)
        {
            return "Delete called with : " + id.ToString();
        }


        public string Create(string title, int refid, bool activate)
        {
            return "Create called with : " + title + ", " + refid + ", " + activate.ToString();
        }


        public string DeleteAll()
        {
            return "DeleteAll called";
        }
    }
}
