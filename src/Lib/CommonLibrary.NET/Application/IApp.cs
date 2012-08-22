using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using ComLib;
using ComLib.Arguments;
using ComLib.Logging;
using ComLib.Configuration;
using ComLib.EmailSupport;


namespace ComLib.Application
{
    /// <summary>
    /// Interface for any console/batch application.
    /// </summary>
    public interface IApp
    {      
        /// <summary>
        /// The configuration for this application.
        /// </summary>
        IConfigSource Conf { get; set;  }


        /// <summary>
        /// The logger for this application.
        /// </summary>
        ILogMulti Log { get; set; }


        /// <summary>
        /// The emailer for this application.
        /// </summary>
        IEmailService Emailer { get; set; }


        /// <summary>
        /// The result of the execution.
        /// </summary>
        BoolMessageItem Result { get; set; }


        /// <summary>
        /// Application configurable settings.
        /// </summary>
        AppConfig Settings { get; set; }


        /// <summary>
        /// Get the starttime of the application.
        /// </summary>
        DateTime StartTime { get; }


        /// <summary>
        /// Application name from either the settings or this.GetType().
        /// </summary>
        string Name { get; }


        /// <summary>
        /// Get the application description.
        /// </summary>
        string Description { get; }


        /// <summary>
        /// Get the version of this application.
        /// </summary>
        string Version { get; }


        /// <summary>
        /// Get the definition of command line options that
        /// are acceptable for this application.
        /// </summary>
        List<ArgAttribute> Options { get; }
        
        
        /// <summary>
        /// Get a list of examples that show how to launch this application.
        /// </summary>
        List<string> OptionsExamples { get; }


        /// <summary>
        /// The the command line options.
        /// </summary>
        void ShowOptions();


        /// <summary>
        /// Determine if the string[] arguments (command line arguments) can be accepted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Accept(string[] args);


        /// <summary>
        /// Determine if the string[] arguments (command line arguments) can be accepted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        BoolMessageItem<Args> AcceptArgs(string[] args);


        /// <summary>
        /// Determine if the string[] arguments (command line arguments) can be accepted.
        /// </summary>
        /// <param name="args">e.g. -env:Prod -batchsize:100</param>
        /// <param name="prefix">-</param>
        /// <param name="separator">:</param>
        /// <returns></returns>
        BoolMessageItem<Args> AcceptArgs(string[] args, string prefix, string separator);
        

        /// <summary>
        /// Initialize the application.
        /// </summary>
        void Init();


        /// <summary>
        /// Initialize with some contextual data.
        /// </summary>
        /// <param name="context"></param>
        void Init(object context);


        /// <summary>
        /// Perform some post initialization processing.
        /// and before execution begins.
        /// </summary>
        void InitComplete();
        

        /// <summary>
        /// Execute the application without any arguments.
        /// </summary>
        BoolMessageItem Execute();


        /// <summary>
        /// Execute the application with context data.
        /// </summary>
        /// <param name="context"></param>
        BoolMessageItem Execute(object context);


        /// <summary>
        /// Used for performing some post execution processing before
        /// shutting down the application.
        /// </summary>
        void ExecuteComplete();


        /// <summary>
        /// Shutdown the application.
        /// </summary>
        void ShutDown();


        /// <summary>
        /// Send an email after the application execution completed.
        /// </summary>
        void Notify();


        /// <summary>
        /// Send a notification message.
        /// </summary>
        /// <param name="msg"></param>
        void Notify(IDictionary msg);


        /// <summary>
        /// Display the start of the application.
        /// </summary>
        void DisplayStart();


        /// <summary>
        /// Display the end of the application.
        /// </summary>
        void DisplayEnd();
    }
}
