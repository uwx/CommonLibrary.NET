using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;


namespace ComLib.Automation
{
    /// <summary>
    /// Executes command in a script that are in the format of xml. similar to NAnt.
    /// Features Include:
    /// 
    /// # GENERAL
    /// Scenario: Can use variables in script
    /// Scenario: Can map commands to c# classes
    /// Scenario: Can automap command parameters to c# class properties 
    /// Scenario: C# Command classes have access to use inner xml element for data
    /// Scenario: Command can be configured to fail but allow script to continue running
    /// Scenario: Script result has access to each command result ( command Index, return value, success / fail, message )
    /// Scenario: Script result can build up text combining all command result messages
    /// Scenario: Can specify a "RefKey" on a command to uniquey identify and retrieve that command result value
    /// Scenario: Can include file into the script
    /// Scenario: Can add script variables programmatically
    /// 
    /// # FAILURE
    /// Scenario: Script can gracefully handle invalid xml 
    /// Scenario: Script can gracefully handle invalid command
    /// Scenario: Script can gracefully handle command exception
    /// 
    /// # SUBSTITUTIONS
    /// Scenario: Variables can reference other variables using format ${variable_name}
    /// Scenario: Command parameters can reference variables using format ${variable_name}
    /// Scenario: Command can assign it's return value to a variable via "assignto" parameter assignto="${hello_world_result}
    /// Scenario: Registered default variables
    /// Scenario: Register functions ${date.hour} using name/lamda pairs
    /// </summary>
    public class ScriptXml : Script
    {
        private Dictionary<string, XmlDocument> _includedDocs = new Dictionary<string, XmlDocument>();
        private XmlNode _xmlCommandCurrent;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ScriptXml()
        {
        }


        /// <summary>
        /// Initialize with assembly list.
        /// </summary>
        /// <param name="assemblies">Delimited list of assemblies.</param>
        public ScriptXml(string assemblies) : base(assemblies) {}


        /// <summary>
        /// Registers an external script that may be referenced in the script to run.
        /// </summary>
        /// <param name="name">The name of the external file to include</param>
        /// <param name="content">The content of the external file</param>
        public override void Include(string name, string content)
        {
            var doc = new XmlDocument();
            doc.LoadXml(content);
            _includedDocs[name] = doc;
        }
        

        /// <summary>
        /// Run the script content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected override BoolMessage DoRun(string content)
        {
            // Load xml file.
            var xmlDocLoadResult = ScriptHelper.LoadXml(content);
            if (!xmlDocLoadResult.Success)
            {
                _allCommandsSuccessful = false;
                return new BoolMessage(false, xmlDocLoadResult.Message);
            }
            var doc = xmlDocLoadResult.Item;

            ProcessScript(doc.DocumentElement.ChildNodes);

            return new BoolMessage(_allCommandsSuccessful, string.Empty);
        }


        /// <summary>
        /// Returns whether or not the name provided is exists as a variable.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool IsExistingVariable(string name)
        {
            // Override level
            // 1. Name from Command
            if (_xmlCommandCurrent != null && _xmlCommandCurrent.Attributes.GetNamedItem(name) != null)
                return true;

            return base.IsExistingVariable(name);
        }


        /// <summary>
        /// Get variable value from either it's scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public override T Get<T>(string name)
        {
            // Override level
            // 1. Name from Command
            if (_xmlCommandCurrent != null && _xmlCommandCurrent.Attributes.GetNamedItem(name) != null)
                return InterpretValue<T>(_xmlCommandCurrent.Attributes.GetNamedItem(name).Value);

            return base.Get<T>(name);
        }


        /// <summary>
        /// Get parameter value from the current command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of the parameter to get</param>
        /// <param name="defaultVal">Default value to get if parameter does not exist</param>
        /// <returns></returns>
        public override T GetParam<T>(string name, T defaultVal)
        {
            // The xml command attributes are effectively like a functions parameters.
            if (_xmlCommandCurrent.Attributes.GetNamedItem(name) == null)
                return defaultVal;

            return InterpretValue<T>(_xmlCommandCurrent.Attributes.GetNamedItem(name).Value);
        }


        /// <summary>
        /// Process the content of the script.
        /// </summary>
        /// <param name="allTags">All the tags to process</param>
        /// <returns></returns>
        internal BoolMessage ProcessScript(XmlNodeList allTags)
        {            
            int commandIndex = 0;
            // for each xml command
            for (int ndx = 0; ndx < allTags.Count; ndx++)
            {
                XmlNode tag = allTags[ndx];

                if (tag.Name == "var")
                {
                    ProcessVariable(tag);
                }
                else if (tag.Name == "include")
                {
                    ProcessInclude(tag);
                }
                else if (tag.Name == "for")
                {
                    ProcessForLoop(tag);
                }
                else if (tag.Name == "command")
                {
                    var result = ProcessCommand(tag, commandIndex);
                    commandIndex++;
                }
            }
            return new BoolMessage(_allCommandsSuccessful, string.Empty);
        }


        /// <summary>
        /// Process the xml tag "var" representing a variable.
        /// </summary>
        /// <param name="xmlVar"></param>
        private void ProcessVariable(XmlNode xmlVar)
        {
            var name = xmlVar.Attributes.GetNamedItem("name").Value;
            var val = xmlVar.Attributes.GetNamedItem("value").Value;
            SetVariable(name, val);
        }


        /// <summary>
        /// Process the xml tag "include" which now only supports "var" tags.
        /// </summary>
        /// <param name="xmlInclude"></param>
        private void ProcessInclude(XmlNode xmlInclude)
        {
            var name = xmlInclude.Attributes.GetNamedItem("name").Value;
            var doc = _includedDocs[name];

            ProcessScript(doc.DocumentElement.ChildNodes);
        }


        /// <summary>
        /// Process the xml tag for which loops through commands inside it.
        /// </summary>
        /// <param name="xmlFor">The xml for loop element</param>
        private void ProcessForLoop(XmlNode xmlFor)
        {
            // Set current command.
            _xmlCommandCurrent = xmlFor;

            // Parse loop
            string loopline = Get<string>("loop");
            var interpreter = new Interpreter(this.Scope);
            var forloop = interpreter.TokenizeForLoop("for( " + loopline + ")", 1);

            this.Scope.Push();

            // Get values.
            int start = GetOrConvertTo<int>(forloop.StartExpression);
            int upto = GetOrConvertTo<int>(forloop.CheckExpression);
            int increment = GetOrConvertTo<int>(forloop.IncrementExpression);

            // Initialie new scope for for
            this.Scope.SetValue(forloop.Variable, start);

            // Run loop.
            for (var ndx = start; ndx < upto; ndx += increment)
            {
                ProcessScript(xmlFor.ChildNodes);

                // Update the variable value
                this.Scope.SetValue(forloop.Variable, ndx);
            }
        }


        /// <summary>
        /// Processs the xml tag "command".
        /// </summary>
        /// <param name="xmlCommand">The xml node representing the command</param>
        /// <param name="commandIndex">The index number of the xml command representing a command</param>
        /// <returns></returns>
        private CommandResult ProcessCommand(XmlNode xmlCommand, int commandIndex)
        {
            // Set current command.
            _xmlCommandCurrent = xmlCommand;

            var name = xmlCommand.Attributes.GetNamedItem("name").Value;

            // Need to interpret the values of all the attributes.
            var atts = xmlCommand.Attributes;
            for (int ndx = 0; ndx < atts.Count; ndx++)
            {
                string rawValue = atts[ndx].Value;
                atts[ndx].Value = InterpretValue<string>(rawValue);
            }

            return ExecuteCommand(name, null, xmlCommand, commandIndex, 
                (command) => AutoMapper.Map<object>(xmlCommand, command, AutoMapperSettings.Default, null));
        }
    }
}
