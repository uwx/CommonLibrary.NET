using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Net.Mail;
using ComLib;
using ComLib.Arguments;
using ComLib.Logging;
using ComLib.Environments;
using ComLib.Authentication;
using ComLib.Configuration;
using ComLib.EmailSupport;
using ComLib.Notifications;
using ComLib.Reflection;
using ComLib.Collections;
using ComLib.Extensions;

namespace ComLib.Application
{
    /// <summary>
    /// <para>
    /// A template driven base class for any application. This provides out-of the box functionality containing:
    /// 1. Environment selection.
    /// 2. Argument parsing
    /// 3. Configuration file loading ( with inheritance )
    /// 4. Logging
    /// 5. Email notification on completion
    /// 6. Error handling
    /// 7. 0/1 Exit codes on completion
    /// </para>
    /// </summary>
    public class App : IApp, IDisposable
    {
        /// <summary>
        /// Runs the application provided.
        /// </summary>
        /// <param name="app">The application to run.</param>
        /// <param name="args">Command line arguments.</param>
        public static BoolMessageItem Run(IApp app, string[] args)
        {
            return Run(app, args, true, string.Empty);
        }


        /// <summary>
        /// Runs the application provided.
        /// </summary>
        /// <param name="app">The application to run.</param>
        /// <param name="args">Command line arguments.</param>
        /// <param name="decorations">Decorations around the application. e.g. "diagnostics,statusupdates"</param>
        public static BoolMessageItem Run(IApp app, string[] args, string decorations)
        {
            return Run(app, args, true, decorations);
        }


        /// <summary>
        /// Runs the application provided.
        /// </summary>
        /// <param name="app">The application to run.</param>
        /// <param name="args">Command line arguments.</param>
        /// <param name="requireConfigFiles">Whether or not to throw error if the config files are not available.</param>
        /// <param name="decorations">Decorations around the application. e.g. "diagnostics,statusupdates"</param>
        public static BoolMessageItem Run(IApp app, string[] args, bool requireConfigFiles, string decorations)
        {
            // Validation.
            if (app == null) throw new ArgumentNullException("ApplicationTemplate to run was not supplied.");

            // Set the configfile requirement flag.
            app.Settings.RequireConfigs = requireConfigFiles;

            // Replace the application with the decorator.
            // This provides diagnostics, status updates out of the box, shutdown of logging. etc.
            app = new AppDecorator(decorations, app);
 
            BoolMessageItem result = null;
            bool validArgs = false;

            try
            {
                // Validate the arguments.
                result = app.AcceptArgs(args);
                validArgs = result.Success;
                if (!result.Success) return result;

                // Initalize.
                app.Init();
                app.InitComplete();

                // Execute.
                result = app.Execute();
                app.ExecuteComplete();
            }
            catch (Exception ex)
            {
                Try.HandleException(ex);
            }
            finally
            {
                Try.Catch(() =>
                {
                    if (validArgs) app.ShutDown();
                });
            }            
            return result;
        }


        #region Constructors
        /// <summary>
        /// <para>
        /// Default construction. Populates the arguments schema with some basic arguments to support this template.
        /// This includes the following:
        ///    _args.Schema.AddNamed(bool)("pause", false, false, "Pause the application to attach debugger.", "true", "true|false", false, false, "common", true);            
        ///    _args.Schema.AddNamed(string)("env", false, "dev", "Environment to run in.", "dev", "dev | qa | uat | prod", false, false, "common", false);
        ///    _args.Schema.AddNamed(string)("config", false, "dev", "Configuration file to use. If not supplied, default to env selected", "dev.config", "dev.config | qa.config", false, false, "common", false);
        ///    _args.Schema.AddNamed(string)("log", false, @"%name%-%yyyy%-%MM%-%dd%-%env%-%user%.log", "Log file name", "myapp.log", @"myapp.log | %name%-%yyyy%-%MM%-%dd%-%env%-%user%.log", false, false, "common", true);
        ///    _args.Schema.AddNamed(bool)("email", false, false, "Whether or not to send an email on completion.", "true", "true|false", false, false, "common", true);
        ///    _args.Schema.AddNamed(bool)("emailOnlyOnFailure", false, false, "Whether or not to send an email only if application fails.", "true", "true|false", false, false, "common", true);
        ///</para>
        /// </summary>
        public App()
        {
            Settings = new AppConfig();
            _startTime = DateTime.Now;
            _args = new Args("-", ":");
            _args.Schema.AddNamed<bool>("pause", "pause", false, false, "Pause the application to attach debugger.", "true", "true|false", false, false, "common", true);            
            _args.Schema.AddNamed<string>("env", "env", false, "dev", "Environment to run in.", "dev", "dev | qa | uat | prod", false, false, "common", false);
            _args.Schema.AddNamed<string>("config", "config", false, "dev", "Configuration file to use. If not supplied, default to env selected", "dev.config", "dev.config | qa.config", false, false, "common", false);
            _args.Schema.AddNamed<string>("log", "log", false, @"%name%-%yyyy%-%MM%-%dd%-%env%-%user%.log", "Log file name", "myapp.log", @"myapp.log | %name%-%yyyy%-%MM%-%dd%-%env%-%user%.log", false, false, "common", true);
            _args.Schema.AddNamed<bool>("email", "email", false, false, "Whether or not to send an email on completion.", "true", "true|false", false, false, "common", true);
            _args.Schema.AddNamed<bool>("emailOnlyOnFailure", "emailOnlyOnFailure", false, false, "Whether or not to send an email only if application fails.", "true", "true|false", false, false, "common", true);
        }
        #endregion


        #region IBatchApplication Members
        /// <summary>
        /// The configuration for this application.
        /// </summary>
        public IConfigSource Conf
        {
            get { return _config; }
            set { _config = value; }
        }


        /// <summary>
        /// The instance of the logger to use for the application.
        /// </summary>
        public ILogMulti Log
        {
            get { return _log; }
            set { _log = value; }
        }


        /// <summary>
        /// The instance of the email service.
        /// </summary>
        public IEmailService Emailer
        {
            get { return _emailer; }
            set { _emailer = value; }
        }


        /// <summary>
        /// The result of the execution.
        /// </summary>
        public BoolMessageItem Result
        {
            get { return _result; }
            set { _result = value; }
        }


        /// <summary>
        /// Get the start time of the application.
        /// </summary>
        public DateTime StartTime { get { return _startTime; } }


        /// <summary>
        /// Application name from either the settings or this.GetType().
        /// </summary>
        public virtual string Name
        {
            get { return this.GetType().Name; }
        }


        /// <summary>
        /// Get the application description.
        /// </summary>
        public virtual string Description
        {
            get { return AttributeHelper.GetAssemblyInfoDescription(this.GetType(), Assembly.GetEntryAssembly().GetName().Name); }
        }


        /// <summary>
        /// Get the version of this application.
        /// </summary>
        public virtual string Version
        {
            get { return this.GetType().Assembly.GetName().Version.ToString(); }
        }


        /// <summary>
        /// Get list of command line options that are supported.
        /// By default only supports the --pause option.
        /// </summary>
        public virtual List<ArgAttribute> Options
        {
            get
            {
                // Get all arguments supported from argument reciever's attributes.
                if (Settings.ArgsRequired && Settings.ArgsReciever != null)                
                    return ArgsHelper.GetArgsFromReciever(Settings.ArgsReciever);

                return _args.Schema.Items;
            }
        }


        /// <summary>
        /// Get example of how to run this application.
        /// </summary>
        public virtual List<string> OptionsExamples
        {
            get { return ArgsUsage.BuildSampleRuns(Name, Options, Settings.ArgsPrefix, Settings.ArgsSeparator); }
        }


        /// <summary>
        /// Show the command line options.
        /// </summary>
        public virtual void ShowOptions()
        {
            ArgsUsage.Show(Options, OptionsExamples);
        }


        /// <summary>
        /// Determine if the arguments can be accepted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>True if success. False otherwise.</returns>        
        public virtual BoolMessageItem<Args> AcceptArgs(string[] args)
        {
            return AcceptArgs(args, Settings.ArgsPrefix, Settings.ArgsSeparator);
        }


        /// <summary>
        /// Determine if the arguments can be accepted.
        /// </summary>
        /// <param name="rawArgs">Arguments from commandline.</param>
        /// <param name="prefix">-</param>
        /// <param name="separator">:</param>
        /// <returns>True if success. False otherwise.</returns>        
        public virtual BoolMessageItem<Args> AcceptArgs(string[] rawArgs, string prefix, string separator)
        {
            Settings.ArgsPrefix = prefix;
            Settings.ArgsSeparator = separator;

            // If there is an argument reciever, then reset the _args.Schema 
            // using the arg attributes in the reciever.
            if( IsArgumentRecieverApplicable)
                _args.Schema.Items = ArgsHelper.GetArgsFromReciever(Settings.ArgsReciever);

            Args args = new Args(rawArgs, Options);

            // Are the args required?
            if (args.IsEmpty && !Settings.ArgsRequired) 
                return new BoolMessageItem<Args>(args, true, "Arguments not required.");

            // Handle -help, -about, -pause.
            BoolMessageItem<Args> optionsResult = AppHelper.HandleOptions(this, args);
            if (!optionsResult.Success) 
                return optionsResult;

            // Validate/Parse args.
            BoolMessageItem<Args> result = Args.Accept(rawArgs, prefix, separator, 1, Options, OptionsExamples);
                        
            // Successful ? Apply args to object reciever
            if (result.Success)
            {
                // Store the parsed args.
                _args = result.Item;

                if (IsArgumentRecieverApplicable)
                    Args.Accept(rawArgs, prefix, separator, Settings.ArgsReciever);
            }
            
            // Errors ? Show them.
            if (!result.Success)
                ArgsUsage.ShowError(result.Message);

            return result;
        }


        /// <summary>
        /// Determine if the arguments can be accepted.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>True if success. False otherwise.</returns>        
        public virtual bool Accept(string[] args)
        {
            return AcceptArgs(args).Success;
        }


        /// <summary>
        /// Initialize the application.
        /// </summary>        
        public virtual void Init()
        {
            Init(null);
        }


        /// <summary>
        /// Initialize with contextual data.
        /// </summary>
        /// <param name="context"></param>
        public virtual void Init(object context)
        {
            string env = _args.Get("env", "dev");
            string log = _args.Get("log", "%name%-%yyyy%-%MM%-%dd%-%env%-%user%.log");
            string config = _args.Get("config", string.Format(@"config\{0}.config", env));

            // 1. Initialize the environment. prod, prod.config
            Envs.Set(env, "prod,uat,qa,dev", config);
            Env.Get(env).RefPath = config;

            // 2. Append the file based logger.
            Logger.Default.Append(new LogFile(this.GetType().Name, log, DateTime.Now, env));
            
            // 3. Initialize config inheritance from environment inheritance.
            Config.Init(Configs.LoadFiles(Env.RefPath));

            // 4. Set the config and logger, and emailer instances on the application.
            _config = Config.Current;
            _log = Logger.Default;
            _emailer = new EmailService(_config, "EmailServiceSettings");
        }


        /// <summary>
        /// On initialization complete and before execution begins.
        /// </summary>
        public virtual void InitComplete()
        {
            DisplayStart();
        }


        /// <summary>
        /// Execute the application without any arguments.
        /// </summary>
        public virtual BoolMessageItem Execute()
        {
            return Execute(null);
        }


        /// <summary>
        /// Execute the application with context data.
        /// </summary>
        /// <param name="context"></param>
        public virtual BoolMessageItem Execute(object context)
        {
            return new BoolMessageItem(0, true, string.Empty);
        }


        /// <summary>
        /// Used to perform some post execution processing before
        /// shutting down.
        /// </summary>
        public virtual void ExecuteComplete()
        {
        }


        /// <summary>
        /// Shutdown the application.
        /// </summary>
        public virtual void ShutDown()
        {
        }


        /// <summary>
        /// Display information at the start of the application.
        /// </summary>
        public virtual void DisplayStart()
        {
            Display(true, new OrderedDictionary());
        }


        /// <summary>
        /// Display information at the end of the application.
        /// </summary>
        public virtual void DisplayEnd()
        {
            Display(false, new OrderedDictionary());
        }


        /// <summary>
        /// Send an email notification.
        /// </summary>
        public virtual void Notify()
        {
            // Check for null.
            var msg = new Dictionary<string, string>();
            msg["emailTo"] = Conf.Get<string>("EmailSettings", "emailTo");
            msg["emailFrom"] = Conf.Get<string>("EmailSettings", "emailFrom");
            msg["emailSubject"] = Conf.Get<string>("EmailSettings", "emailSubject");
            Notify(msg);
        }


        /// <summary>
        /// Send an email at the end of the completion of the application.
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Notify(IDictionary msg)
        {            
            string successFailMsg = _result.Success ? Name + " Successful" : Name + " Failed";
            
            // Command line has highest precedence, it overrides config setting.
            bool sendEmailCommandLine = _args.Get<bool>("email", false);
            bool sendEmailConfig = Conf.GetDefault<bool>("EmailSettings", "enableEmails", false);
            bool sendEmail = sendEmailCommandLine ? true : sendEmailConfig;

            bool sendEmailOnlyOnFailureCommandLine = _args.Get<bool>("emailOnlyOnFailure", false);
            bool sendEmailOnlyOnFailureConfig = Conf.GetDefault<bool>("EmailSettings", "enableEmailsOnlyOnFailures", false);
            bool sendEmailOnlyOnFailure = sendEmailOnlyOnFailureCommandLine ? true : sendEmailOnlyOnFailureConfig;

            if ((sendEmail && !sendEmailOnlyOnFailure) ||
                (sendEmail && sendEmailOnlyOnFailure && !_result.Success))
            {
                
                var message = new NotificationMessage(null, "", "", "", "");
                message.To = (string)msg["emailTo"];
                message.From = (string)msg["emailFrom"];
                message.Subject = msg.Get<string>("emailSubject", successFailMsg);
                message.Body = BuildSummary(false, null);

                _emailer.Send(message);
            }
        }


        /// <summary>
        /// Display information about the application.
        /// </summary>
        /// <param name="isStart"></param>
        /// <param name="summaryInfo">The key/value pairs can be supplied
        /// if this is derived and the derived class wants to add additional
        /// summary information.</param>
        public virtual void Display(bool isStart, IDictionary summaryInfo)
        {
            string summary = BuildSummary(isStart, summaryInfo);
            Log.Info(summary, null, null);
        }


        /// <summary>
        /// Builds up a string representing the summary of the application.
        /// </summary>
        /// <param name="isStart"></param>
        /// <param name="summaryInfo"></param>
        public virtual string BuildSummary(bool isStart, IDictionary summaryInfo)
        {
            return AppHelper.GetAppRunSummary(this, isStart, summaryInfo);
        }

        #endregion


        #region Public Properties
        /// <summary>
        /// Settings.
        /// </summary>
        public AppConfig Settings { get; set; }


        /// <summary>
        /// Determines whether or not the argument reciever is capable of recieving the arguments.
        /// </summary>
        public bool IsArgumentRecieverApplicable
        {
            get { return Settings.ArgsReciever != null && Settings.ArgsRequired && Settings.ArgsAppliedToReciever; }
        }
        #endregion


        #region IDisposable Members
        /// <summary>
        /// Currently disposing.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Overloaded dispose method indicating if dispose was 
        /// called explicitly.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ShutDown();
            }
        }


        /// <summary>
        /// Finalization.
        /// </summary>
        ~App()
        {
            // Simply call Dispose(false).
            Dispose(false);
        }
        #endregion


        #region Private Data
        /// <summary>
        /// The logger for the app.
        /// </summary>
        protected ILogMulti _log;


        /// <summary>
        /// The configuration source.
        /// </summary>
        protected IConfigSource _config;        


        /// <summary>
        /// The email service to send emails after completion.
        /// </summary>
        protected IEmailService _emailer;


        /// <summary>
        /// The result of the execution.
        /// </summary>
        protected BoolMessageItem _result = BoolMessageItem.False;


        /// <summary>
        /// Arguments supplied to the application.
        /// </summary>
        protected Args _args;


        /// <summary>
        /// The starttime of the application.
        /// </summary>
        protected DateTime _startTime;
        #endregion
    }
}
