using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

//<doc:using>
using ComLib;
using ComLib.Application;
//</doc:using>
using ComLib.Entities;
using ComLib.Account;
using ComLib.Arguments;
using ComLib.Logging;
using ComLib.Configuration;
using ComLib.IO;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of ActiveRecord Initialization/Configuration.
    /// </summary>
    public class Example_AppTemplate : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_AppTemplate()
        {
            // Tell the base framework/base class to parse the 
            // command line arguments and store them in the argument reciever.
            Settings.ArgsAppliedToReciever = true;
            Settings.ArgsRequired = true;
            Settings.ArgsReciever = new StartupArgs();            
        }


        /// <summary>
        /// List of command line launches examples of how to run this program.
        /// </summary>
        public override List<string> OptionsExamples
        {
            get 
            { 
                return new List<string>() 
                { 
                     "Example_AppTemplate.exe -env:dev  -log:app.dev.log  -config:dev.config -date:${today} setting_01",
                     "Example_AppTemplate.exe -env:qa   -log:app.qa.log   -config:qa.config  -date:${t-1}   -readonly:true  setting_01",
                     "Example_AppTemplate.exe -env:prod -log:app.prod.log -config:dev.config -date:${today} -readonly:false 2 setting_01",
                }; 
            }
        }

		
		//<doc:example>
        /// <summary>
        /// Validate arguments passed, if they are valid, accept them.
        /// </summary>
        /// <remarks>This is the first method called in the workflow,
        /// because the arguments supplied can determine how to initialize
        /// the application.</remarks>
        /// <param name="args">Command line arguments supplied.</param>
        /// <returns>True if args are valid, false otherwise.</returns>
        public override bool Accept(string[] args)
        {
            Logger.Info("\r\n\r\n==================================");
            Logger.Info("Example_AppTemplate Accept(args) called.");
            Logger.Info("Validating & Accepting arguments passed");
            bool accepted = base.Accept(args);
            if (accepted)
            {
                StartupArgs startupArgs = Settings.ArgsReciever as StartupArgs;
                Logger.Info("Using Arguments ===========================");
                Logger.Info("Environment         : " + startupArgs.Envrionment);
                Logger.Info("BusinessDate        : " + startupArgs.BusinessDate);
                Logger.Info("Config              : " + startupArgs.Config);
                Logger.Info("Log                 : " + startupArgs.LogFile);
                Logger.Info("ReadonlyMode        : " + startupArgs.ReadonlyMode); 
                Logger.Info("CategoriesToDisplay : " + startupArgs.CategoriesToDisplay);
                Logger.Info("DefaultSettingsId   : " + startupArgs.DefaultSettingsId);                
            }
            return accepted;
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public override void Init()
        {
            Logger.Info("\r\n\r\n==================================");
            Logger.Info("Example_AppTemplate Init() called.");
            Logger.Info("Initializing application from command line args.");
            
            StartupArgs args = Settings.ArgsReciever as StartupArgs;

            // 1. Configure logging : Append a new file logger to default logger.
            Logger.Default.Append(new LogFile("Example_AppTemplate_LogFile", args.LogFile));

            // 2. Configure configuration data.
            Config.Init(new IniDocument(args.Config, GetSampleContents("dev"), false));
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            Logger.Info("\r\n\r\n==================================");            
            Logger.Info("Example_AppTemplate Execute() called.");
            Logger.Info("Executing business logic here...");
            return BoolMessageItem.True;
        }


        /// <summary>
        /// Shutdown various services.
        /// </summary>
        public override void ShutDown()
        {
            Logger.Info("\r\n\r\n==================================");            
            Logger.Info("Example_AppTemplate ShutDown() called.");
            Logger.Info("Shutting down application and dependent services.");
            Logger.ShutDown();            
        }
		//</doc:example>
		

        private string GetSampleContents(string env)
        {
            // This is just an example of loading configuration data from a string.
            // This could be from a file.
            // - INI files ( for most practical purposes I've found are more than enough )
            // - Also JSON documents could be used instead of XML.            
            // This is equivalent to a java .props file.
            string dev = "[Global]" + Environment.NewLine
                             + "AppName:CommonLibary.App1" + Environment.NewLine
                             + "Env: Dev" + Environment.NewLine

                             + "[DB]" + Environment.NewLine
                             + "server: dev.server01" + Environment.NewLine
                             + "user: devuser1" + Environment.NewLine
                             + "password: pass123" + Environment.NewLine
                             + "port:8081" + Environment.NewLine;

            // This is equivalent to a java .props file.
            string prod = "[Global]" + Environment.NewLine
                             + "Env: Prod" + Environment.NewLine

                             + "[DB]" + Environment.NewLine
                             + "server: prod_srv" + Environment.NewLine
                             + "user: readonly" + Environment.NewLine
                             + "password: ro9999 " + Environment.NewLine
                             + "desc: primary prod server" + Environment.NewLine;

            if (env == "dev") return dev;
            if (env == "prod") return prod;
            return dev;
        }


        /// <summary>
        /// Sample object that should recieve the arguments.
        /// </summary>
        public class StartupArgs
        {
            /// <summary>
            /// Configuration file
            /// </summary>
            [Arg("config", "c", "config file for environment", typeof(string), true, "", "dev.config", "dev.config | qa.config | prod.config")]
            public string Config { get; set; }


            /// <summary>
            /// Log file name/path
            /// </summary>
            [Arg("log", "l", "Log file to write to", typeof(string), true, "", "app.log", "app.log | log.txt")]
            public string LogFile { get; set; }


            /// <summary>
            /// Environment to run in
            /// </summary>
            [Arg("env", "e", "Environment to run in", typeof(string), true, "dev", "dev", "dev | qa | prod | uat")]
            public string Envrionment { get; set; }


            /// <summary>
            /// The business date to run in
            /// </summary>
            [Arg("date", "d", "The business date", typeof(int), true, null, "${today}", "${today} | 05/12/2009", true)]
            public DateTime BusinessDate { get; set; }


            /// <summary>
            /// Whether or not to run in read-only mode
            /// </summary>
            [Arg("readonly", "r", "readonly mode", typeof(bool), false, false, "true", "true | false")]
            public bool ReadonlyMode { get; set; }


            /// <summary>
            /// Number of categories to display
            /// </summary>
            [Arg(1, "Number of categories to display", typeof(int), false, 1, "1", "1 | 2 | 3 etc.")]
            public int CategoriesToDisplay { get; set; }


            /// <summary>
            /// The default settings to load.
            /// </summary>
            [Arg(0, "settings id to load on startup", typeof(string), true, "", "settings_01")]
            public string DefaultSettingsId { get; set; }
        }
    }
}
