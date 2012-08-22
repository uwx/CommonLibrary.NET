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
    public class AutomationTestsXml
    {
        [Test]
        public void Can_Get_CommandInfo()
        {
            var svc = new CommandService();
            svc.Load(ContentLoader.TestDllName);

            var helptext = svc.GetHelpOn("HelloWorldAutoMap");
            Assert.AreEqual(4, svc.Lookup["HelloWorldAutoMap"].AdditionalAttributes.Count);
        }
        
        [Test]
        public void Can_Call_Command()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script1.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            // Check the entrie script result.
            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorld");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorld return value");            
        }


        [Test]
        public void Can_Call_Command_With_AutoMapping()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script2.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, @"HelloWorldAutoMap kishore true 32 2/2/1979");    
        }


        [Test]
        public void Can_Call_Multiple_Commands()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script3.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorld");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorld return value"); 

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[1].Success);
            Assert.AreEqual(runner.Result.CommandResults[1].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[1].Index, 1);
            Assert.AreEqual((string)runner.Result.CommandResults[1].Item, @"HelloWorldAutoMap kishore true 32 2/2/1979");
        }


        [Test]
        public void Can_Substitue_Variables_And_Command_Args()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script4.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorldAutoMap kishore.reddy_0.9.7 true 101 2/2/1979");
        }


        [Test]       
        public void Can_Fail_With_UnKnown_Command()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script_error1_unknown_command.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            // Check the entrie script result.
            Assert.IsFalse(runner.Result.Success);
            //Assert.IsTrue(runner.Result.StartTime < runner.Result.EndTime);

            // Check the command result.
            Assert.IsFalse(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "some-crazy-command");
            Assert.AreEqual(runner.Result.CommandResults[0].Message, "Unknown command : some-crazy-command");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
        }


        [Test]
        public void Can_Fail_With_Invalid_Xml()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script_error2_invalid_xml.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            // Check the entrie script result.
            Assert.IsFalse(runner.Result.Success);
            Assert.IsTrue(runner.Result.Message.Contains("Unable to load xml :"));
        }


        [Test]
        public void Can_Fail_With_Exception()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script_error3_exception.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            // Check the entrie script result.
            Assert.IsFalse(runner.Result.Success);
            Assert.IsFalse(runner.Result.CommandResults[0].Success);
        }


        [Test]
        public void Can_Continue_With_Exception()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script5.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);

            // Check the command result.
            Assert.IsFalse(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "ExceptionTest");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[1].Success);
            Assert.AreEqual(runner.Result.CommandResults[1].Name, "HelloWorld");
            Assert.AreEqual(runner.Result.CommandResults[1].Index, 1);
            Assert.AreEqual((string)runner.Result.CommandResults[1].Item, "HelloWorld return value");   
        }


        [Test]
        public void Can_Access_InnerXml_Element()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script6.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);

            

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "CreateEvent");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "Disucss .NET in startups"); 
        }


        [Test]
        public void Can_Get_Total_Output_Of_Commands()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script3.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            var results = runner.RunText(xml);
            var output = results.Messages(Environment.NewLine);

            var expectedMessage = 
                "HelloWorld : HelloWorld message" + Environment.NewLine
                + "HelloWorldAutoMap : HelloWorldAutoMap message" + Environment.NewLine;

            Assert.IsTrue(runner.Result.Success);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults.Count == 2);
            Assert.AreEqual(output, expectedMessage);
        }


        [Test]
        public void Can_Use_RefKey()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script5.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            
            var results = runner.RunText(xml);

            Assert.IsTrue(runner.Result.Success);
            Assert.AreEqual(results.ValueForRefKey("k1"), "HelloWorld return value");
        }


        [Test]
        public void Can_Include_External_File()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script7.xml");
            var settings = ContentLoader.GetTextFileContent("Automation.CommonSettings.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            runner.Script.Include("commonsettings.xml", settings);

            var results = runner.RunText(xml);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorldAutoMap kishore.reddy_0.9.7 true 101 2/2/1979");
        }


        [Test]
        public void Can_Register_Variables()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script8.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);
            runner.Script.SetVariable("first", "kishore");
            runner.Script.SetVariable("last", "reddy");
            runner.Script.SetVariable("age", "101");
            runner.Script.SetVariable("birthdate", "2/2/1979");
            runner.Script.SetVariable("active", "true");
            runner.Script.SetVariable("major", "0");
            runner.Script.SetVariable("minor", "9");
            runner.Script.SetVariable("revision", "7");
            runner.Script.SetVariable("comlibversion", "${major}.${minor}.${revision}");

            var results = runner.RunText(xml);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorldAutoMap");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorldAutoMap kishore.reddy_0.9.7 true 101 2/2/1979");
        }


        [Test]
        public void Can_Assign_Result_To_Variable()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script9.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);

            var results = runner.RunText(xml);

            // Check the command result.
            Assert.IsTrue(runner.Result.CommandResults[0].Success);
            Assert.AreEqual(runner.Result.CommandResults[0].Name, "HelloWorld");
            Assert.AreEqual(runner.Result.CommandResults[0].Index, 0);
            Assert.AreEqual((string)runner.Result.CommandResults[0].Item, "HelloWorld return value");
            
            // Check that the variable greetingresult get the return value of the command.
            Assert.AreEqual(runner.Script.Get<string>("greetingresult"), "HelloWorld return value");
        }


        [Test]
        public void Can_Set_Default_Variables()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script10.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);

            Assert.AreEqual(runner.Script.Get<int>("date.month"), DateTime.Now.Month);
            Assert.AreEqual(runner.Script.Get<string>("date.monthabbr"), DateTime.Now.ToString("MMM"));
            Assert.AreEqual(runner.Script.Get<string>("date.monthname"), DateTime.Now.ToString("MMMM"));
            Assert.AreEqual(runner.Script.Get<int>("date.day"), DateTime.Now.Day);
            Assert.AreEqual(runner.Script.Get<string>("date.dayabbr"), DateTime.Now.ToString("ddd"));
            Assert.AreEqual(runner.Script.Get<string>("date.dayname"), DateTime.Now.ToString("dddd"));
        }


        [Test]
        public void Can_Run_Loop()
        {
            var xml = ContentLoader.GetTextFileContent("Automation.Script11.xml");
            var runner = new AutomationRunner("xml", ContentLoader.TestDllName);

            CommandCountTest.Count = 0;
            
            var result = runner.RunText(xml);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(CommandCountTest.Count, 4);
        }
    }



    /// <summary>
    /// Sample automation script command class.
    /// </summary>
    [Command(Name="HelloWorld", Description = "Sample command for testing" )]
    [CommandParameter(Name = "first", DataType = typeof(string), Description = "First name", IsRequired = true, OrderNum = 1, Example = "kishore")]
    [CommandParameter(Name = "last", DataType = typeof(string), Description = "First name", IsRequired = true, OrderNum = 1, Example = "reddy")]
    public class CommandHelloWorld : Command
    {
        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            var first = this.Get<string>("first");
            var last = this.Get<string>("last");
            Console.WriteLine("Name : " + first + ", " + last);

            return new BoolMessageItem("HelloWorld return value", true, "HelloWorld message");
        }
    }


    /// <summary>
    /// Sample automation script command class.
    /// </summary>
    [Command(Name = "ExceptionTest", Description = "Sample command throws exception for negative testing")]
    public class CommandExceptionTest : Command
    {
        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            if (!string.IsNullOrEmpty(this.Script.Assemblies))
                throw new ArgumentException("testing exception");

            return new BoolMessageItem("ExceptionTest return value", true, string.Empty);
        }
    }


    /// <summary>
    /// Sample automation script command class.
    /// </summary>
    [Command(Name = "CreateEvent", Description = "Sample command that access internal xml element")]
    public class CommandCreateEventTest : Command
    {
        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            XmlElement elem = context.ParamMap as XmlElement;
            var text = elem.ChildNodes[0].ChildNodes[1].InnerText;
            return new BoolMessageItem(text, true, string.Empty);
        }
    }


    /// <summary>
    /// Sample automation script command class.
    /// </summary>
    [Command(Name = "CountTest", Description = "Sample command to test xml loop")]
    public class CommandCountTest : Command
    {
        public static int Count = 0;

        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            Count++;
            return new BoolMessageItem(string.Empty, true, string.Empty);
        }
    }


    /// <summary>
    /// Sample automation script command class with automap feature enabled to map xml attribute values to it's properties.
    /// </summary>
    [Command(Name = "HelloWorldAutoMap", Description = "Sample command with automapping of inputs to properties", AutoMap = true)]
    [CommandParameter(Name = "User",      DataType = typeof(string),  Description = "Sample automap parameter for string",   IsRequired = true, OrderNum = 1, Example = "user01")]
    [CommandParameter(Name = "IsActive",  DataType = typeof(bool),    Description = "Sample automap parameter for bool",     IsRequired = true, OrderNum = 3, Example = "32")]
    [CommandParameter(Name = "Age",       DataType = typeof(int),     Description = "Sample automap parameter for int",      IsRequired = true, OrderNum = 2, Example = "true")]
    [CommandParameter(Name = "BirthDate", DataType = typeof(DateTime),Description = "Sample automap parameter for datetime", IsRequired = true, OrderNum = 4, Example = "06/20/2011")]
    public class CommandHelloWorldAutoMap : Command
    {
        /// <summary>
        /// string property for testing
        /// </summary>
        public string User { get; set; }


        /// <summary>
        /// bool flag for testing
        /// </summary>
        public bool IsActive { get; set; }


        /// <summary>
        /// int property for testing
        /// </summary>
        public int Age { get; set; }


        /// <summary>
        /// Date property for testing
        /// </summary>
        public DateTime BirthDate { get; set; }


        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            var result = User + " " + IsActive.ToString().ToLower() + " " + Age + " " + BirthDate.ToShortDateString();
            return new BoolMessageItem("HelloWorldAutoMap " + result, true, "HelloWorldAutoMap message");
        }
    }
}
