using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Automation
{
    /// <summary>
    /// Helper class for commands.
    /// </summary>
    class CommandHelper
    {
        /// <summary>
        /// Builds a command result representing an unknown command
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static CommandResult BuildUnKnownCommandResult(string name, int index, string errorMessage)
        {
            var errorResult = new CommandResult(DateTime.Now, DateTime.Now, false, errorMessage);
            errorResult.Name = name;
            errorResult.Index = index;
            return errorResult;
        }


        /// <summary>
        /// Builds a command result representing an invalid command call with methods on a command that does not support methods
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static CommandResult BuildMethodCallNotSupportedResult(string name, int index)
        {
            var errorResult = new CommandResult(DateTime.Now, DateTime.Now, false, "Method call is not supported on this command");
            errorResult.Name = name;
            errorResult.Index = index;
            return errorResult;
        }
    }
}
