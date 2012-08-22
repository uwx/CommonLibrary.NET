using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;


namespace ComLib.Automation
{
    /// <summary>
    /// Class that runs an automation script.
    /// </summary>
    public class AutomationRunner
    {
        private ScriptResult _result;
        private Script _script;


        /// <summary>
        /// Initialize.
        /// </summary>
        public AutomationRunner()
        {
            Init("xml", string.Empty);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        public AutomationRunner(string format, string commandDllsDelimited)
        {
            Init(format, commandDllsDelimited);
        }


        /// <summary>
        /// The format used in the automation script. "xml" or "json"
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// A delimited list of assemblies to load the commands contained in the automation script.
        /// </summary>
        public string Assemblies { get; set; }


        /// <summary>
        /// Gets the last run result of the last script.
        /// </summary>
        public ScriptResult Result { get { return _result; } }


        /// <summary>
        /// Gets the current script that was executed
        /// </summary>
        public Script Script { get { return _script; } }



        /// <summary>
        /// Initialize the format and assemblies for commands
        /// </summary>
        /// <param name="format"></param>
        /// <param name="assembliesForCommands"></param>
        public void Init(string format, string assembliesForCommands)
        {
            Format = format;
            Assemblies = assembliesForCommands;
            if (string.Compare(Format, "xml", true) == 0) _script = new ScriptXml(this.Assemblies);
            else if (string.Compare(Format, "js", true) == 0) _script = new ScriptCmdLine(this.Assemblies);
            else throw new ArgumentException("Format : " + Format + " not supported");
        }


        /// <summary>
        /// Run the 
        /// </summary>
        /// <returns></returns>
        public ScriptResult RunScript(string filePath)
        {            
            // Validate
            if (!File.Exists(filePath)) throw new FileNotFoundException("File : " + filePath + " does not exist.");

            var content = File.ReadAllText(filePath);
            _result = RunText(content);
            _result.Name = Path.GetFileName(filePath);
            _result.FilePath = filePath;
            return _result;
        }


        /// <summary>
        /// Runs automation commands using supplied text rather than filepath.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public ScriptResult RunText(string content)
        {
            _result = null;
            _script.Assemblies = this.Assemblies;
            _result = _script.Run(content);
            return _result;
        }


        /// <summary>
        /// Writes the results of the last script execution to the file path supplied.
        /// </summary>
        /// <param name="filepath"></param>
        public void WriteResultsToFile(string filepath)
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                var attributes = " success=\"" + (_result.Success ? "true" : "false") + "\""
                               + " message=\"" + _result.Message + "\""
                               + " file=\"" + _result.FilePath + "\"";

                writer.WriteLine("<scriptresult " + attributes + ">");
                writer.WriteLine("\t<commandresults>");
                foreach (var cmdResult in _result.CommandResults)
                {
                    var xml = cmdResult.ToXml();
                    writer.WriteLine("\t\t" + xml);
                }
                writer.WriteLine("\t</commandresults>");
                writer.WriteLine("</scriptresult>");
                writer.Flush();
            }
        }
    }
}
