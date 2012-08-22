using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace ComLib.Automation
{
    /// <summary>
    /// Executes commands in a script that are in the format of command line arguments. e.g.
    /// 1. HelloWorldAutoMap user=kishore isactive=true age=30 birthdate=2/2/1979
    /// 2. HelloWorldAutoMap user="kishore1" isactive="true" age="31" birthdate="2/2/1980"
    /// </summary>
    public class ScriptCmdLine : Script
    {
        private IDictionary _currentLineArgs;


        /// <summary>
        /// Initialize with assembly list.
        /// </summary>
        /// <param name="assemblies">Delimited list of assemblies.</param>
        public ScriptCmdLine(string assemblies) : base(assemblies) {}


        /// <summary>
        /// Registers an external script that may be referenced in the script to run.
        /// </summary>
        /// <param name="name">The name of the external file to include</param>
        /// <param name="content">The content of the external file</param>
        public override void Include(string name, string content)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns whether or not the name provided is exists as a variable.
        /// </summary>
        /// <param name="name">The name of the variable to check if it's an existing variable.</param>
        /// <returns></returns>
        public override bool IsExistingVariable(string name)
        {
            if (_currentLineArgs != null && _currentLineArgs.Contains(name))
                return true;

            return base.IsExistingVariable(name);
        }


        /// <summary>
        /// Sets a variables
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="val">Value for the variable</param>
        public override void SetVariable(string name, object val)
        {
            if (IsExistingVariable(val.ToString()))
            {
                object actual = Get<object>(val.ToString());
                _scope.SetValue(name, actual);
                return;
            }
            _scope.SetValue(name, val);
        }


        /// <summary>
        /// Get variable value from either it's scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T Get<T>(string name)
        {
            if (_currentLineArgs != null && _currentLineArgs.Contains(name))
                return InterpretValue<T>((string)_currentLineArgs[name]);

            return base.Get<T>(name);
        }


        /// <summary>
        /// Gets a parameter value from the current command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of the parameter to get</param>
        /// <param name="defaultVal">Default value to get if parameter does not exist</param>
        /// <returns></returns>
        public override T GetParam<T>(string name, T defaultVal)
        {
            if (!_currentLineArgs.Contains(name))
                return defaultVal;
            
            return InterpretValue<T>((string)_currentLineArgs[name]);
        }


        /// <summary>
        /// Run the script content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override BoolMessage DoRun(string content)
        {
            var interpreter = new JsLite(_scope, ProcessTokenType);
            interpreter.Interpret(content);
            return new BoolMessage(_allCommandsSuccessful, string.Empty);
        }


        private bool ProcessTokenType(JsLiteArgs args)
        {
            // Function
            if (args.TokenType == JsLiteToken.FuncCall)
            {
                _currentLineArgs = args.ParamMap as IDictionary;
                if(args.ParamMap != null && args.ParamMap.Count > 0)                
                    ProcessFunctionCallParameters(_currentLineArgs);

                var result = ExecuteCommand(args.Name, args.ParamList, args.ParamMap, args.LineNumber, (command) => AutoMapper.Map<object>(_currentLineArgs, command, AutoMapperSettings.Default, null));
                if (!result.Success) return false;
            }
            // Variable
            else if (args.TokenType == JsLiteToken.Var)
            {
                SetVariable(args.Name, args.Context);
            }
            // For loop
            else if (args.TokenType == JsLiteToken.ForLoop)
            {

            }
            return true;
        }


        private void ProcessFunctionCallParameters(IDictionary args)
        {
            List<string> keys = new List<string>();
            foreach (var k in args.Keys) keys.Add(k.ToString());

            foreach (var key in keys)
            {
                string val = args[key].ToString();
                if (IsExistingVariable(val))
                {
                    object actual = GetInternal<object>(val);
                    args[key] = actual;
                }
            }
        }
    }
}
