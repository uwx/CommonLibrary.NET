using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Security.Principal;

//<doc:using>
using ComLib;
using ComLib.Automation;
//<doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the automation component.
    /// </summary>
    public class Example_Automation : App
    {
        /// <summary>
        /// Execute the sample
        /// </summary>
        /// <returns></returns>
        public override BoolMessageItem Execute()
        {
            string sampleScript =
@"<automation>	
	<!-- Variables -->
	<var name=""first""   value=""kishore"" />
	<var name=""last""    value=""reddy"" />
    <var name=""version"" value=""0.9.7"" />
	
	<!-- commands -->
	<command name=""helloworld"" email=""${first}.${last}@mysite.com"" subject=""CommonLibrary.NET ${version} is out"" />
</automation>";


            // create script runner and tell it to run xml scripts, and to load
            // the commands from the CommonLibrary.dll which
            // only has one command ( bottom of this file "CommandHelloWorld" ).
            var runner = new AutomationRunner("xml", "Commonlibrary");
            
            // 1. Xml automation script
            var results = runner.RunText(sampleScript);
            runner.WriteResultsToFile("c:/temp/automation.xml");

            // 2. Command Line style automation script
            runner.Init("cmd", "CommonLibrary");
            runner.RunText(@"helloword email=""kishore@c.com"" subject=""comlib automation component""");
            return new BoolMessageItem(null, true, string.Empty);
        }
    }


    
    /// <summary>
    /// Sample command for showing the automation component.
    /// </summary>
    [Command(Name="HelloWorld")]    
    public class CommandHelloWorld : Command
    {
        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            var name = Get<string>("name");
            var version = Get<string>("subject");
            Console.WriteLine("HelloWorld command running with args: " + name + ", " + version);
            return new BoolMessageItem("helloworld command can return a value", true, string.Empty);
        }
    }
}
