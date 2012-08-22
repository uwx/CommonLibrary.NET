using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using ComLib;


namespace ComLib.Automation
{
    /// <summary>
    /// Service that loads all the commands ( that have CommandAttribute ) on classes.
    /// This loads commands from multiple dlls that are supplied by the caller.
    /// </summary>
    public class CommandService : ExtensionService<CommandAttribute, Command>
    {
        private const string DefaultParamFormat =
                                     "Name: {0} " + "{newline}"
                                   + "Desc: {1} " + "{newline}"
                                   + "Type: {2} " + "{newline}"
                                   + "Req : {3} " + "{newline}"
                                   + "E.g.: {4} " + "{newline}";


        /// <summary>
        /// Initialize command service
        /// </summary>
        public CommandService()
        {
            _onLoadCompleteCallback = () => LoadAdditionalAttributes<CommandParameterAttribute>();
        }


        /// <summary>
        /// Gets the help text for the command name.
        /// </summary>
        /// <param name="commandName">Name of the command to get help on</param>
        /// <param name="newLine">New line to use, defaults to Environment.NewLine</param>
        /// <returns></returns>
        public string GetHelpOn(string commandName, string newLine = "")
        {
            var info = _lookup[commandName];

            // Determine newline.
            if (string.IsNullOrEmpty(newLine)) newLine = Environment.NewLine;
            string format = DefaultParamFormat.Replace("{newline}", newLine);

            string text = info.Attribute.Name + newLine;
            if (info.AdditionalAttributes != null && info.AdditionalAttributes.Count > 0)
            {
                // Sort them by order number
                var orderedParams = new List<CommandParameterAttribute>();
                foreach (var param in info.AdditionalAttributes)
                    orderedParams.Add(param as CommandParameterAttribute);

                orderedParams = orderedParams.OrderBy(p => p.OrderNum).ToList();

                foreach (CommandParameterAttribute param in orderedParams)
                {
                    var req = param.IsRequired ? "required" : "optional";
                    text += string.Format(format, param.Name, param.Description, param.DataType.Name, req, param.Example);
                    text += newLine;
                }
            }
            return text;
        }


        /// <summary>
        /// Builds up a sample call for the command.
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public string GetSampleRun(string commandName)
        {
            // - HelloWorld({name:"kishore", age:35, ..... });
            ExtensionMetaData info = null;
            List<CommandParameterAttribute> args = null;
            GetInfo(commandName, (ext, paramList) => { info = ext; args = paramList; });

            string sample = info.Attribute.Name;
            if(args == null || args.Count == 0)
                return sample + "();";

            sample += "( {";
            // Build up individual parameters.
            foreach(var arg in args)
            {
                if (arg.DataType == typeof(string))
                {
                    sample += " " + arg.Name + " : \"" + arg.Example + "\",";
                }
                else if (arg.DataType == typeof(DateTime))
                {
                    sample += " " + arg.Name + " : \"" + arg.Example + "\",";
                }
                else
                    sample += " " + arg.Name + " : " + arg.Example + ",";
            }
            sample = sample.Remove(sample.Length - 1, 1);
            return sample += "} );";
        }


        private void GetInfo(string commandName, Action<ExtensionMetaData, List<CommandParameterAttribute>> callback)
        {
            var info = _lookup[commandName];
            List<CommandParameterAttribute> parameters = null;

            if (info.AdditionalAttributes != null && info.AdditionalAttributes.Count > 0)
            {
                // Sort them by order number
                var orderedParams = new List<CommandParameterAttribute>();
                foreach (var param in info.AdditionalAttributes)
                    orderedParams.Add(param as CommandParameterAttribute);

                parameters = orderedParams.OrderBy(p => p.OrderNum).ToList();
            }
            callback(info, parameters);    
        }
    }
}
