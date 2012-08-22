using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;


namespace ComLib.Automation
{
    /// <summary>
    /// Represents a script containing commands that can be executed.
    /// </summary>
    public abstract class Script
    {
        private DateTime _started;
        private DateTime _ended;
        private ScriptResult _result;
        
        /// <summary>
        /// List of registered functions
        /// </summary>
        protected IDictionary<string, Func<IList<string>,object>> _functions;


        /// <summary>
        /// Structure for storing variables.
        /// </summary>
        protected Scope _scope = new Scope();
        

        /// <summary>
        /// Default initialization.
        /// </summary>
        public Script() 
        {
            Init();
        }


        /// <summary>
        /// Initialize with assembly list.
        /// </summary>
        /// <param name="assemblies">Delimited list of assemblies.</param>
        public Script(string assemblies) 
        { 
            Assemblies = assemblies;
            Init();
        }
        

        /// <summary>
        /// The command service for loading the commands.
        /// </summary>
        protected CommandService _cmdService;


        /// <summary>
        /// The List of command results.
        /// </summary>
        protected List<CommandResult> _cmdResults;


        /// <summary>
        /// Whether or not all commands are successful.
        /// </summary>
        protected bool _allCommandsSuccessful;


        /// <summary>
        /// A delimited list of assemblies to load the commands contained in the automation script.
        /// </summary>
        public string Assemblies { get; set; }


        /// <summary>
        /// Get / set the command service.
        /// </summary>
        public CommandService Service { get { return _cmdService; } set { _cmdService = value; } }


        /// <summary>
        /// Scope
        /// </summary>
        public Scope Scope { get { return _scope; } }


        /// <summary>
        /// Runs automation commands using supplied text rather than filepath.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ScriptResult Run(string content)
        {
            _started = DateTime.Now;
            string error = "Assemblies for commands not specified. 'Assemblies' property must be set with delimited list of assemblies containing commands";
            if (string.IsNullOrEmpty(this.Assemblies)) throw new InvalidOperationException(error);

            // Load all the commands in the dlls.
            _cmdService = new CommandService();
            _cmdService.Load(this.Assemblies);

            // Register defaults
            RegisterSystemCommands();

            // Check for "list all"
            _cmdResults = new List<CommandResult>();

            // Reset.
            _allCommandsSuccessful = true;
            var result = DoRun(content);

            _ended = DateTime.Now;
            _result = new ScriptResult(_started, _ended, result.Success, result.Message);
            _result.CommandResults = _cmdResults;
            return _result;
        }


        /// <summary>
        /// Includes an addition file's content into the script.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        public virtual void Include(string name, string content) { }
                

        /// <summary>
        /// Returns whether or not the name provided is exists as a variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual bool IsExistingVariable(string name)
        {
            // 1. Name from scope
            if (_scope.Contains(name))
                return true;

            // 2. Registered function
            if (_functions.ContainsKey(name))
                return true;

            return false;
        }
        

        /// <summary>
        /// Get variable value from either it's scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T Get<T>(string name)
        {
            return GetInternal<T>(name);
        }        


        /// <summary>
        /// Get variable value from either it's scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of the parameter to get</param>
        /// <param name="defaultValue">Default value if parameter is not there.</param>
        /// <returns></returns>
        public abstract T GetParam<T>(string name, T defaultValue);


        /// <summary>
        /// Get either the value of the variable name supplied if it's a variable name or returns the value converted to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">e.g. "firstname" will return value of variable named firstname. or "12" will return int 12.</param>
        /// <returns></returns>
        public virtual T GetOrConvertTo<T>(string name)
        {
            if (IsExistingVariable(name))
                return Get<T>(name);

            return (T)Convert.ChangeType(name, typeof(T));
        }

        
        /// <summary>
        /// Sets a variables
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="val">Value for the variable</param>
        public virtual void SetVariable(string name, object val)
        {
            if (val is string)
            {
                val = InterpretValue<string>(val.ToString());
            }
            _scope.SetValue(name, val);
        }


        /// <summary>
        /// Sets a functiona that can be called.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public virtual void SetFunction(string name, Func<IList<string>, object> callback)
        {
            _functions[name] = callback;
        }


        /// <summary>
        /// Interprets the value supplied by replacing parameters with variable values. e.g. "${firstname}" gets replace with the value associated with variable called firstname 
        /// </summary>
        /// <typeparam name="T">The type to convert the final value to</typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T InterpretValue<T>(string value)
        {
            value = StringHelper.Substitute(value, (val) => GetInternal<string>(val));
            return (T)Convert.ChangeType(value, typeof(T));
        }


        /// <summary>
        /// Initialize with default data.
        /// </summary>
        protected virtual void Init()
        {
            _functions = new Dictionary<string, Func<IList<string>, object>>();
            RegisterSystemVariables();
        }


        /// <summary>
        /// Run the content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual BoolMessage DoRun(string content) { return BoolMessage.False; }



        /// <summary>
        /// Gets a tuple containing the command to create and whether or not it was successfully created.
        /// </summary>
        /// <param name="name">Name of the command</param>
        /// <returns>True:Command if command name is valid, False,Null if command name does not exist.</returns>
        protected BoolMessageItem<Command> GetCommand(string name)
        {
            if (!_cmdService.Lookup.ContainsKey(name))
                return new BoolMessageItem<Command>(null, false, "Unknown command : " + name);

            var command = _cmdService.Create(name);
            command.InitScope(_scope);
            command.Script = this;
            return new BoolMessageItem<Command>(command, true, string.Empty);
        }


        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandName">The name of the command</param>
        /// <param name="paramList">The parameter list for the command</param>
        /// /// <param name="paramMap">The named parameters for the command</param>
        /// <param name="commandIndex">The index number of the command in the script</param>
        /// <param name="autoMapCallback">Callback to automap args to properties if automapping is enabled for command</param>
        /// <returns></returns>
        protected CommandResult ExecuteCommand(string commandName, List<object> paramList, object paramMap, int commandIndex, Action<Command> autoMapCallback)
        {
            // Valid command ?
            var cmdValidationResult = ValidateCommandCall(commandName, commandIndex);
            if (!cmdValidationResult.Success)
            {
                _cmdResults.Add(cmdValidationResult.Result);
                _allCommandsSuccessful = false;
                return cmdValidationResult.Result;
            }

            var command = cmdValidationResult.Cmd;
            var metaInfo = _cmdService.Lookup[cmdValidationResult.ActualCommandName];
            var commandAttribute = metaInfo.Attribute as CommandAttribute;

            // 1. Map the properties if automapping enabled.
            if (commandAttribute.AutoMap && metaInfo.Lamda == null && autoMapCallback != null)
            {
                autoMapCallback(command);
            }

            // 2. Run the command.
            var started = DateTime.Now;
            BoolMessageItem result = null;
            bool canFail = false;
            string refKey = string.Empty;
            string assignTo = string.Empty;
            try
            {
                canFail = GetParam<bool>("canFail", false);
                refKey = GetParam<string>("refkey", string.Empty);
                assignTo = GetParam<string>("assignto", string.Empty);
                result = command.Execute(new CommandContext() { Name = commandName, IsMethodCall = cmdValidationResult.IsMethodCall, ParamList = paramList, ParamMap = paramMap, Meta = commandAttribute});
            }
            catch (Exception ex)
            {
                result = new BoolMessageItem(null, false, "Error executing command : " + commandName + ". " + ex.Message);
            }

            var ended = DateTime.Now;

            // 3. Store the result of the command.
            var cmdResult = new CommandResult(started, ended, result.Success, result.Message);
            cmdResult.RefKey = refKey;
            cmdResult.Name = commandName;
            cmdResult.Index = commandIndex;
            cmdResult.Item = result.Item;
            
            // 4. Set return value to variable
            if (!string.IsNullOrEmpty(assignTo))
            {
                this._scope.SetValue(assignTo, cmdResult.Item);
            }
                
            // 5. 1 failure = not all succceeded
            if (!cmdResult.Success && !canFail) _allCommandsSuccessful = false;

            _cmdResults.Add(cmdResult);
            return cmdResult;
        }


        /// <summary>
        /// Register system level commands
        /// </summary>
        protected virtual void RegisterSystemCommands()
        {
            // Register system supplied commands.
            _cmdService.Register(typeof(CommandListCommands));            
        }


        /// <summary>
        /// System supplied variables.
        /// </summary>
        private void RegisterSystemVariables()
        {
            // ComLib version
            string version = this.GetType().Assembly.GetName().Version.ToString();
            _scope.SetValue("comlib.version", version);

            // Environment
            _scope.SetValue("env.username", Environment.UserName);
            _scope.SetValue("env.version", Environment.Version);
            _scope.SetValue("env.domain", Environment.UserDomainName);
            _scope.SetValue("env.machinename", Environment.MachineName);
            _scope.SetValue("env.osversion", Environment.OSVersion);

            // DateTime functions
            SetFunction("date.year",      (args) => DateTime.Now.Year);
            SetFunction("date.yearabbr",  (args) => DateTime.Now.ToString("yy")); 
            SetFunction("date.month",     (args) => DateTime.Now.Month);
            SetFunction("date.monthabbr", (args) => DateTime.Now.ToString("MMM"));
            SetFunction("date.monthname", (args) => DateTime.Now.ToString("MMMM"));
            SetFunction("date.day",       (args) => DateTime.Now.Day);
            SetFunction("date.dayabbr",   (args) => DateTime.Now.ToString("ddd"));
            SetFunction("date.dayname",   (args) => DateTime.Now.ToString("dddd"));
            SetFunction("time.hour",      (args) => DateTime.Now.Hour);
            SetFunction("time.minute",    (args) => DateTime.Now.Minute);
            SetFunction("time.second",    (args) => DateTime.Now.Second);
            SetFunction("time.ticks",     (args) => DateTime.Now.Ticks);
        }


        /// <summary>
        /// Gets internal variables and/or registered function variables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetInternal<T>(string name)
        {
            // 1. Name from scope
            if (_scope.Contains(name))
                return _scope.Get<T>(name);

            // 2. Registered function
            if (_functions.ContainsKey(name))
            {
                object val = _functions[name](null);
                return (T)Convert.ChangeType(val, typeof(T));
            }
            throw new ArgumentException("Variable or function name supplied : " + name + " does not exist.");
        }


        private CommandValidationResult ValidateCommandCall(string commandName, int commandIndex)
        {
            bool isMethodCall = commandName.Contains(".");
            string actualCommandName = isMethodCall
                                     ? commandName.Substring(0, commandName.IndexOf("."))
                                     : commandName;
            // Validate command exists            
            var commandCreateResult = GetCommand(actualCommandName);
            if (!commandCreateResult.Success)
            {
                var errorResult = CommandHelper.BuildUnKnownCommandResult(commandName, commandIndex, commandCreateResult.Message);
                return new CommandValidationResult(){ Success = false, Result = errorResult, ActualCommandName = actualCommandName, IsMethodCall = isMethodCall};
            }

            // Get meta data.
            var command = commandCreateResult.Item;
            var metaInfo = _cmdService.Lookup[actualCommandName];
            var commandAttribute = metaInfo.Attribute as CommandAttribute;

            // Validate. Supports multi-method calls?
            if (!commandAttribute.IsMultiMethodEnabled && isMethodCall)
            {
                var errorMethodCommand = CommandHelper.BuildMethodCallNotSupportedResult(actualCommandName, commandIndex);
                return new CommandValidationResult(){ Success = false, Result = errorMethodCommand, ActualCommandName = actualCommandName, IsMethodCall = isMethodCall};
            }
            return new CommandValidationResult(){ Success = true, Result =null, IsMethodCall = isMethodCall, ActualCommandName = actualCommandName, Cmd = command};
        }


        class CommandValidationResult
        {
            public bool Success;
            public CommandResult Result;
            public string ActualCommandName;
            public bool IsMethodCall;
            public Command Cmd;
        }
    }
}
