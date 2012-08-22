using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using ComLib;
using ComLib.Arguments;
using ComLib.Diagnostics;
using ComLib.Logging;
using ComLib.Caching;
using ComLib.Configuration;
using ComLib.EmailSupport;


namespace ComLib.Application
{
    /// <summary>
    /// Base class for the Batch application.
    /// </summary>
    public class AppDecorator : IApp
    {
        private IApp _instance;
        private AppDecoratorHelper _helper;
        private string _decorators;
        private BoolMessageItem _result;


        /// <summary>
        /// Initialize the underlying instance.
        /// </summary>
        /// <param name="delimitedDecorators"></param>
        /// <param name="instance"></param>
        public AppDecorator(string delimitedDecorators, IApp instance)
        {
            _instance = instance;
            _decorators = delimitedDecorators;
            InitDecorations();
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public void InitDecorations()
        {
            _helper = new AppDecoratorHelper(_instance.GetType(), _decorators);
        }


        #region IApplicationTemplate Members
        /// <summary>
        /// Get all the options that are supported.
        /// </summary>
        public List<ArgAttribute> Options
        {
            get { return _instance.Options; }
        }


        /// <summary>
        /// Get examples of the command line options.
        /// </summary>
        public List<string> OptionsExamples
        {
            get { return _instance.OptionsExamples; }
        }


        /// <summary>
        /// The config source for the application.
        /// </summary>
        public IConfigSource Conf
        {
            get { return _instance.Conf; }
            set { _instance.Conf = value; }
        }


        /// <summary>
        /// The logger for the application.
        /// </summary>
        public ILogMulti Log
        {
            get { return _instance.Log; }
            set { _instance.Log = value; }
        }


        /// <summary>
        /// Result of the execution.
        /// </summary>
        public BoolMessageItem Result
        {
            get { return _instance.Result; }
            set { _instance.Result = value; }
        }


        /// <summary>
        /// The Emailer for this application.
        /// </summary>
        public IEmailService Emailer
        {
            get { return _instance.Emailer; }
            set { _instance.Emailer = value; }
        }


        /// <summary>
        /// Application Settings
        /// </summary>
        public AppConfig Settings
        {
            get { return _instance.Settings; }
            set { _instance.Settings = value; }
        }


        /// <summary>
        /// Start time of the application.
        /// </summary>
        public DateTime StartTime { get { return _instance.StartTime; } }


        /// <summary>
        /// Application name from either the settings or this.GetType().
        /// </summary>
        public virtual string Name
        {
            get { return _instance.Name; }
        }


        /// <summary>
        /// Get the application description.
        /// </summary>
        public virtual string Description
        {
            get { return _instance.Description; }
        }


        /// <summary>
        /// Get the version of this application.
        /// </summary>
        public virtual string Version
        {
            get { return _instance.Version; }
        }


        /// <summary>
        /// The command line options.
        /// </summary>
        public virtual void ShowOptions()
        {            
            _instance.ShowOptions();
        }


        /// <summary>
        /// Accept args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Accept(string[] args)
        {
            return _instance.Accept(args);
        }


        /// <summary>
        /// Accept arguments.
        /// </summary>
        /// <param name="args">Command line args.</param>
        /// <returns></returns>
        public BoolMessageItem<Args> AcceptArgs(string[] args)
        {
            var result = _instance.AcceptArgs(args);
            return result;
        }


        /// <summary>
        /// Accept arguments.
        /// </summary>
        /// <param name="args">e.g. -env:Prod -batchsize:100</param>
        /// <param name="prefix">-</param>
        /// <param name="separator">:</param>
        /// <returns></returns>
        public virtual BoolMessageItem<Args> AcceptArgs(string[] args, string prefix, string separator)
        {
            return _instance.AcceptArgs(args, prefix, separator);
        }


        /// <summary>
        /// Initialize application.
        /// </summary>
        public void Init()
        {
            _helper.Execute("Init()", "", false, () => _instance.Init());
        }


        /// <summary>
        /// Initialize 
        /// </summary>
        /// <param name="context"></param>
        public void Init(object context)
        {
            _helper.Execute("Init(context)", "", false, () => _instance.Init(context));
        }


        /// <summary>
        /// Initialization complete.
        /// </summary>
        public void InitComplete()
        {
            _helper.Execute("InitComplete()", "", true, () => _instance.InitComplete());
        }


        /// <summary>
        /// Execute the core logic.
        /// </summary>
        public BoolMessageItem Execute()
        {
            return ExecuteInternal(() => _instance.Execute());
        }


        /// <summary>
        /// Execute the core logic.
        /// </summary>
        /// <param name="context"></param>
        public BoolMessageItem Execute(object context)
        {
            return ExecuteInternal(() => _instance.Execute(context));
        }


        /// <summary>
        /// Execute Complete.
        /// </summary>
        public void ExecuteComplete()
        {
            _helper.Execute("ExecuteComplete", "", true, () => _instance.ExecuteComplete());
        }


        /// <summary>
        /// Shutdown.
        /// </summary>
        public void ShutDown()
        {
            // Shutdown services and application.
            _helper.Execute("ShutDown()", "", true, () =>
            {
                RunDiagnostics();
                _instance.ShutDown();
                _instance.DisplayEnd();
                Notify();
            });
            ShutdownServices();
        }


        /// <summary>
        /// Send emails only if the decoration is enabled.
        /// </summary>
        public void Notify()
        {
            if (_helper.IsDecoratedWith("email"))
                _helper.Execute("Notify()", "Notify", true, () => _instance.Notify());
        }


        /// <summary>
        /// Notify using the message supplied.
        /// </summary>
        /// <param name="msg"></param>
        public void Notify(IDictionary msg)
        {
            if (_helper.IsDecoratedWith("email"))
                _helper.Execute("Notify()", "Notify", true, () => _instance.Notify(msg));
        }


        /// <summary>
        /// Display start.
        /// </summary>
        public void DisplayStart()
        {
            _helper.Execute("DisplayStart()", "", true, () => _instance.DisplayStart());
        }


        /// <summary>
        /// Display end.
        /// </summary>
        public void DisplayEnd()
        {
            _helper.Execute("DisplayEnd()", "", true, () => _instance.DisplayEnd());
        }
        #endregion


        /// <summary>
        /// Executes the application.execute method via a lamda and logs the success/failure.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected BoolMessageItem ExecuteInternal(Func<BoolMessageItem> action)
        {
            string methodName = _instance.GetType().FullName + ".Execute";
            _helper.Execute("Execute(context)", "", () =>
            {
                _result = Try.CatchLogGet<BoolMessageItem>("Error in " + methodName, action);
            });

            // Handle possiblity of applicationTemplate returning null for result.
            if (_result == null)
            {
                _result = new BoolMessageItem(null, false, _instance.GetType().Name + " returned null result, converting this to a failure result.");
                _instance.Result = _result;
            }

            string message = _result.Success ? "Successful" : "Failed : " + _result.Message;
            Logger.Info(methodName + " : " + message);
            return _result;
        }


        /// <summary>
        /// Run Diagnostics.
        /// </summary>
        protected void RunDiagnostics()
        {
            if (_helper.IsDecoratedWith("diagnostics"))
                Diagnostics.Diagnostics.WriteInfo("MachineInfo,AppDomain,Env_System,Env_User", "Diagnostics.txt");
        }


        /// <summary>
        /// Shutdown various Services.
        /// </summary>
        protected void ShutdownServices()
        {
            Logger.ShutDown();
            Cacher.Clear();
        }
    }
}
