using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// Helper class for fluentscript
    /// </summary>
    public class FSHelper
    {
        /// <summary>
        /// Writes out a line indicating success/failure in different colors.
        /// </summary>
        /// <param name="success"></param>
        public static void WriteScriptStatus(bool success, string message)
        {
            var color = success ? ConsoleColor.Green : ConsoleColor.Red;
            string text = success ? "SUCCESS" : "FAILURE(S)";
            Console.WriteLine();
            WriteText(color, text);
            if (!success)
            {
                WriteText(ConsoleColor.Red, "Failed with error: " + message);
            }
        }


        /// <summary>
        /// Writes out a line indicating success/failure in different colors.
        /// </summary>
        /// <param name="success"></param>
        public static void WriteText(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }


        /// <summary>
        /// Validates the args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static BoolMsgItem Validate(FSArgs args)
        {
            var isOutFileNeeded = (args.Tokenize || args.Nodes);

            if (string.IsNullOrEmpty(args.FilePath))
                return new BoolMsgItem(false, "File path was not supplied", null);

            if (!File.Exists(args.FilePath))
                return new BoolMsgItem(false, "File path : " + args.FilePath + " does NOT exist.", null);

            if (!string.IsNullOrEmpty(args.Logs) && !Directory.Exists(args.Logs))
                return new BoolMsgItem(false, "Log directory : " + args.Logs + " does NOT exist.", null);

            if (!string.IsNullOrEmpty(args.Plugins) && !IsValidPluginGroup(args.Plugins))
                return new BoolMsgItem(false, "Plugin group : " + args.Plugins + " is NOT valid. e.g. 'sys', 'all', 'explicit:<pluginname1>,<pluginname2>..etc'", null);

            if (isOutFileNeeded && string.IsNullOrEmpty(args.Out))
                return new BoolMsgItem(false, "Out file path was not supplied", null);

            return new BoolMsgItem(true, string.Empty, null);
        }


        private static bool IsValidPluginGroup(string group)
        {
            if (string.Compare(group, "sys", true) == 0) return true;
            if (string.Compare(group, "all", true) == 0) return true;
            return false;
        }
    }
}
