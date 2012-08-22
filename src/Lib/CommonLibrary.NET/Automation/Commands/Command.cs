using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;


namespace ComLib.Automation
{
    /// <summary>
    /// Base class for an individual command.
    /// </summary>
    public class Command
    {        
        private Scope _scope;
        private Script _script;

        /// <summary>
        /// Whether or not scope is supported for the command which indicates to the script to enter into a new scope.
        /// </summary>
        protected bool _isScopeSupported; 
  

        /// <summary>
        /// Reference id used in scripts to uniquely refer to a specific command.
        /// This can be supplied in the script.
        /// </summary>
        public string RefKey { get; set; }


        /// <summary>
        /// The script this command is a part of.
        /// </summary>
        public Script Script { get { return _script; } set { _script = value; } }


        /// <summary>
        /// Whether or not scope is supported for the command which indicates to the script to enter into a new scope.
        /// </summary>
        public bool IsScopeSupported { get { return _isScopeSupported; } }


        /// <summary>
        /// Initialize the scope for variables.
        /// </summary>
        /// <param name="scope"></param>
        internal void InitScope(Scope scope)
        {
            _scope = scope;
        }


        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public BoolMessageItem Execute(CommandContext context)
        {
            // IsMethod call and auto execute method call?
            if(!context.IsMethodCall)
                return DoExecute(context);
            
            if(context.IsMethodCall && !context.Meta.AutoHandleMethodCalls)
                return DoExecute(context);

            // Handle method calls
            string methodName = context.Name.Substring(context.Name.IndexOf(".") + 1);

            object[] args = null;
            MethodInfo method = this.GetType().GetMethod(methodName);
            if (context.ParamMap is XmlAttributeCollection)
                return new BoolMessageItem(null, false, "Auto Calling methods on command is only supported in JS scripts, not xml scripts");

            // Parameters are named in json style ( { id : 12, isActive: true, name: "kishore" });
            // Index based parameters ( 12, true, "kishore");
            if (context.ParamList != null && context.ParamList.Count > 0)
            {
                args = context.ParamList.ToArray();
            }
            else if (context.ParamMap is Dictionary<string, object>)
            {   
                var listmap = context.ParamMap as Dictionary<string, object>;
                        
                if (listmap.Count > 0)
                {
                    args = new object[listmap.Count];                
                    var allParams = method.GetParameters();
                    for(int ndx = 0; ndx < allParams.Length; ndx++)
                    {
                        args[ndx] = listmap[allParams[ndx].Name];
                    }
                }
            }
            // Else = 0 parameters to method so args = null;

            // Call the method.
            object result = method.Invoke(this, args);
            return new BoolMessageItem(result, true, string.Empty);
        }
        

        /// <summary>
        /// Get variable value from either it's scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual T Get<T>(string name)
        {
            return _script.Get<T>(name);
        }


        /// <summary>
        /// Get either the value of the variable name supplied if it's a variable name or returns the value converted to the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">e.g. "firstname" will return value of variable named firstname. or "12" will return int 12.</param>
        /// <returns></returns>
        protected virtual T GetOrConvertTo<T>(string name)
        {
            return _script.GetOrConvertTo<T>(name);
        }
        

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual BoolMessageItem DoExecute(CommandContext context)
        {
            return new BoolMessageItem(null, true, string.Empty);
        }
    }
}
