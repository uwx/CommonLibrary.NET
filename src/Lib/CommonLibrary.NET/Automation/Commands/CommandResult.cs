using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Automation
{
    /// <summary>
    /// Result of a script action.
    /// </summary>
    [Serializable]
    public class CommandResult : RunResult
    {   
        /// <summary>
        /// Null object
        /// </summary>
        public static readonly CommandResult Empty = new CommandResult(DateTime.MinValue, DateTime.MaxValue, false, string.Empty);


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="started">Start time of script</param>
        /// <param name="ended">End time of script.</param>
        /// <param name="success">Whether or not the script execution was successful</param>
        /// <param name="message">A combined message of all the errors.</param>
        public CommandResult(DateTime started, DateTime ended, bool success, string message)
            : base(started, ended, success, message)
        {
        }


        /// <summary>
        /// Name of the command.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Index of the command.
        /// </summary>
        public int Index { get; set; }


        /// <summary>
        /// Key that can be used to uniquely identify a command result.
        /// </summary>
        public string RefKey { get; set; }


        /// <summary>
        /// Xml representation of result
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            return @"<commandresult"
                + " name=\"" + Name + "\""
                + " index=\"" + Index + "\""
                + " success=\"" + (Success ? "true" : "false") + "\""
                + " message=\"" + Message + "\""
                + " />";
        }
    }
}
