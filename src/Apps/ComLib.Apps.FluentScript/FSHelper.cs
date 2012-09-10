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
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string group, string key)
        {
            var section = ConfigurationManager.GetSection(group);
            var namevalSection = section as NameValueCollection;
            var val = namevalSection[key];
            return val;
        }


        /// <summary>
        /// Parses the arguments into a map.
        /// </summary>
        /// <returns></returns>
        public static FSArgs LoadSettings()
        {
            var fsargs = new FSArgs();
            fsargs.LogFolder = ConfigurationManager.AppSettings["logfolder"];
            fsargs.OutPutFolder = ConfigurationManager.AppSettings["outputfolder"];
            fsargs.PluginGroup = ConfigurationManager.AppSettings["plugins"];
            return fsargs;
        }



        /// <summary>
        /// Parses the arguments into a map.
        /// </summary>
        /// <returns></returns>
        public static FSArgs ParseArgs(FSArgs fsargs, string[] args)
        {
            if(fsargs == null)
                fsargs = new FSArgs();

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
                else if (name == "istemplate") fsargs.IsTemplate = true;
                else if (name == "plugins") fsargs.PluginGroup = val;
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
            if (string.IsNullOrEmpty(args.FilePath))
                return new BoolMsgItem(false, "File path was not supplied", null);

            if (!File.Exists(args.FilePath))
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
