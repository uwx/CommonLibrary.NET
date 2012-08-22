using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Automation
{
    /// <summary>
    /// Result of a script execution.
    /// </summary>
    public class ScriptResult : RunResult
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="started">Start time of script</param>
        /// <param name="ended">End time of script.</param>
        /// <param name="success">Whether or not the script execution was successful</param>
        /// <param name="message">A combined message of all the errors.</param>
        public ScriptResult(DateTime started, DateTime ended, bool success, string message)
            : base(started, ended, success, message)
        {
        }


        /// <summary>
        /// Name of the script.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Filepath of the script.
        /// </summary>
        public string FilePath { get; set; }


        /// <summary>
        /// List of results for each command.
        /// </summary>
        public List<CommandResult> CommandResults { get; set; }


        /// <summary>
        /// Get the return value of a specific command.
        /// </summary>
        /// <param name="refkey"></param>
        /// <returns></returns>
        public object ValueForRefKey(string refkey)
        {
            var cmdOutput = (from result in CommandResults 
                           where string.Compare(result.RefKey, refkey) == 0 
                           select result.Item).SingleOrDefault<object>();
            return cmdOutput;
        }


        /// <summary>
        /// Gets the combined output of each command result
        /// </summary>
        /// <returns></returns>
        public string Messages(string newLine = "")
        {
            var buffer = new StringBuilder();
            if (string.IsNullOrEmpty(newLine))
                newLine = Environment.NewLine;

            foreach (var result in CommandResults)
            {
                buffer.Append(result.Name + " : " + result.Message.ToString() + newLine);                    
            }
            return buffer.ToString();
        }
    }
}
