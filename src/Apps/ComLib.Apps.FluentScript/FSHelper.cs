using System;
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
        /// Parses the arguments into a map.
        /// </summary>
        /// <returns></returns>
        public static FSArgs ParseArgs(string[] args)
        {
            var fsargs = new FSArgs();
            var map = new Dictionary<string, object>();
            foreach (var arg in args)
            {
                // Split on ":" e.g. /folder:
                var tokens = arg.Split(':');
                var name = tokens[0].ToLower();
                var val = tokens[1];

                if (name == "file") fsargs.FilePath = val;
                else if (name == "logfolder") fsargs.LogFolder = val;
                else if (name == "outfolder") fsargs.OutPutFolder = val;
                else if (name == "tokenize") fsargs.Tokenize = true;
                else if (name == "register") fsargs.PluginGroup = val;
            }
            return fsargs;
        }



        /// <summary>
        /// Validates the args.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static BoolMsgItem Validate(FSArgs args)
        {
            if (!string.IsNullOrEmpty(args.FilePath) && !File.Exists(args.FilePath))
                return new BoolMsgItem(false, "File path : " + args.FilePath + " does NOT exist.", null);

            if (!string.IsNullOrEmpty(args.LogFolder) && !Directory.Exists(args.LogFolder))
                return new BoolMsgItem(false, "Log directory : " + args.LogFolder + " does NOT exist.", null);

            if (!string.IsNullOrEmpty(args.OutPutFolder) && !Directory.Exists(args.OutPutFolder))
                return new BoolMsgItem(false, "Out directory : " + args.OutPutFolder + " does NOT exist.", null);

            if (!string.IsNullOrEmpty(args.PluginGroup) && !IsValidPluginGroup(args.PluginGroup))
                return new BoolMsgItem(false, "Plugin group : " + args.PluginGroup + " is NOT valid. e.g. 'sys', 'all', 'explicit:<pluginname1>,<pluginname2>..etc'", null);

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
