using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using ComLib.Application;
using ComLib.Arguments;
using ComLib.Lang;
using ComLib.Lang.Templating;


namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// FluentSharp executeable.
    /// </summary>
    public class FS : App
    {
        private FSArgs _fsargs;
        private string[] _cmdArgs;


        /// <summary>
        /// Run the application via it's interface ( Init - Execute - Shutdown )
        /// using the static Run utility method.
        /// </summary>
        /// <param name="args">command line arguments.
        /// e.g. -env:Prod,Dev -date:${today-1} -config:config\prod.config,config\dev.config -source:Reuters 10</param>
        static int Main(string[] args)
        {
            Args.InitServices((textargs) => ComLib.LexArgs.ParseList(textargs), (arg) => ComLib.Subs.Substitutor.Substitute(arg));
            int result = Run(new FS(), args, string.Empty).AsExitCode();
            return result;
        }


        /// <summary>
        /// Set the settings to indictate that 
        /// 1. Command line arguments are required
        /// 2. Command line args should be transferred to args reciever.
        /// </summary>
        public FS()
        {            
            Settings.ArgsReciever = new FSArgs();
            Settings.ArgsRequired = true;
            Settings.ArgsAppliedToReciever = true;
            Settings.OutputStartInfo = false;
            Settings.OutputEndInfo = false;
            
        }


        /// <summary>
        /// Set the settings to indictate that 
        /// 1. Command line arguments are required
        /// 2. Command line args should be transferred to args reciever.
        /// </summary>
        public FS(string[] args)
        {
            _cmdArgs = args;
        }


        /// <summary>
        /// Name of this application.
        /// </summary>
        public override string Name
        {
            get { return "FluentScript"; }
        }


        /// <summary>
        /// Company name
        /// </summary>
        public override string Company
        {
            get { return "CodeHelix Solutions Inc"; }
        }


        /// <summary>
        /// Company website
        /// </summary>
        public override string Website
        {
            get { return "www.codehelixsolutions.com  | http://fluentscript.codeplex.com"; }
        }


        /// <summary>
        /// Description of this application.
        /// </summary>
        public override string Description
        {
            get
            {
                return Environment.NewLine
                     + Environment.NewLine + "FluentScript is a scripting language for .NET to faciliate "
                     + Environment.NewLine + "the development of DSLs( Domain Specific Languages )."
                     + Environment.NewLine + "It has very flexible syntax, intuitive datatypes and a plugin"
                     + Environment.NewLine + "based model for extending the language."
                     + Environment.NewLine;
            }
        }


        /// <summary>
        /// Get list of examples for command line.
        /// </summary>
        public override List<string> OptionsExamples
        {
            get
            {
                return new List<string>()
                {
                    @"fs -exec c:\fs\scripts\example_1.fs",
                    @"fs -tokens -out:exampl_1_tokens.txt     c:\fs\scripts\example_1.fs",
                    @"fs -nodes  -out:exampl_1_statements.txt c:\fs\scripts\example_1.fs"
                };
            }
        }


        /// <summary>
        /// Execute the core logic of the application.
        /// </summary>
        /// <remarks>Note this does not need to be inside of a try-catch 
        /// if using the ApplicationDecorator.</remarks>
        public override BoolMessageItem Execute()
        {
            var args = this.Settings.ArgsReciever as FSArgs;
            _fsargs = args;

            // 1. validate the args
            var result = FSHelper.Validate(_fsargs);
            if (!result.Success)
            {
                HandleValidationFailure(result);
                return new BoolMessageItem(1, false, result.Message);
            }

            // 2. Create interpreter
            var interpreter = CreateInterpreter();
            
            // 3. Execute the code.
            ExecuteCode(interpreter);

            // 4. Get the result of the execution
            var runResult = interpreter.Result;
            FSHelper.WriteScriptStatus(runResult.Success, runResult.Message);
            return new BoolMessageItem(null, runResult.Success, runResult.Message);
        }


        private void ExecuteCode(Interpreter i)
        {
            var file = new FileInfo(_fsargs.FilePath);
            
            // Case 1: Print tokens.
            if (_fsargs.Tokenize)
            {
                i.PrintTokens(file.FullName, _fsargs.Out);
            }
            // Case 2: Print nodes.
            else if (_fsargs.Nodes)
            {
                i.PrintStatements(file.FullName, _fsargs.Out);
            }
            // Case 3: Execute.
            else if (_fsargs.Execute)
            {
                i.ExecuteFile(file.FullName);
            }
        }


        private Interpreter CreateInterpreter()
        {
            var i = new Interpreter();

            // 1. Printing/logging.
            i.Context.Settings.EnablePrinting = true;
            i.Context.Settings.EnableLogging = true;

            // 2. What plugins to register?
            if (_fsargs.Plugins == "sys")
                i.Context.Plugins.RegisterAllSystem();
            else if (_fsargs.Plugins == "all")
                i.Context.Plugins.RegisterAll();
            else if (string.IsNullOrEmpty(_fsargs.Plugins))
                i.Context.Plugins.RegisterAll();
            return i;
        }


        private void HandleValidationFailure(BoolMsgItem result)
        {
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unable to execute. Some settings/inputs have errors:");
            Console.WriteLine(result.Message);
            Console.WriteLine("Type 'fs help' for more information");
            Console.ResetColor();
        }
    }
}
