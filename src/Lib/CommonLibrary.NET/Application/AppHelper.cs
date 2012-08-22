using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

using ComLib;
using ComLib.Logging;
using ComLib.Environments;
using ComLib.Arguments;
using ComLib.Authentication;


namespace ComLib.Application
{
    /// <summary>
    /// Utility class with methods that provide application information.
    /// </summary>
    public class AppHelper
    {
        /// <summary>
        /// <para>
        /// Handle possible "meta data" options of the application.
        /// 1. -help
        /// 2. -version
        /// 3. -pause
        /// </para>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static BoolMessageItem<Args> HandleOptions(IApp app, Args args)
        {
            BoolMessageItem<Args> result = new BoolMessageItem<Args>(args, true, string.Empty);

            // Pause the execution of application to allow attaching of debugger.
            if (args.IsPause)
            {
                Console.WriteLine("Paused for debugging ....");
                Console.ReadKey();
                result = new BoolMessageItem<Args>(args, true, string.Empty); 
            }
            else if (args.IsVersion || args.IsInfo)
            {
                AppHelper.ShowAppInfo(app);
                result = new BoolMessageItem<Args>(args, false, "Displaying description/version.");
            }
            else if (args.IsHelp)
            {
                // -help or ?
                args.ShowUsage(app.Name);
                result = new BoolMessageItem<Args>(args, false, "Displaying usage");
            }
            return result;
        }


        /// <summary>
        /// Show the application info.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static void ShowAppInfo(IApp app)
        {
            Console.WriteLine(GetAppInfo(app));
        }


        /// <summary>
        /// Get application run summary.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="isStart"></param>
        /// <param name="summaryInfo"></param>
        /// <returns></returns>
        public static string GetAppRunSummary(IApp app, bool isStart, IDictionary summaryInfo)
        {
            BuildAppRunSummary(app, isStart, summaryInfo);
            string runSummary = GetAppRunSummaryAsString(isStart, summaryInfo);
            return runSummary;
        }


        /// <summary>
        /// Get application information as string.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static string GetAppInfo(IApp app)
        {
            StringBuilder buffer = new StringBuilder();
            IDictionary info = new OrderedDictionary();
            info["Company"] = app.Name;
            info["Name"] = app.Name;
            info["Version"] = app.Version;
            info["Description"] = app.Description;
            buffer.Append("===============================================================" + Environment.NewLine);
            StringHelper.DoFixedLengthPrinting(info, 4, (key, val) => buffer.Append(key + " : " + val + Environment.NewLine));
            buffer.Append("===============================================================" + Environment.NewLine);
            string summary = buffer.ToString();
            return summary;
        }


        /// <summary>
        /// Creates a runtime summary for the application.
        /// </summary>
        /// <param name="app">Application to get summary for.</param>
        /// <param name="isStart">True if application is starting.</param>
        /// <param name="summaryInfo">Dictionary to use when storing runtime summary information.</param>
        public static void BuildAppRunSummary(IApp app, bool isStart, IDictionary summaryInfo)
        {
            if (summaryInfo == null) summaryInfo = new OrderedDictionary();
            FileInfo fileInfo = new FileInfo(Assembly.GetEntryAssembly().Location);
            string envName = Env.Selected == null ? "" : Env.Name;
            string envPath = Env.Selected == null ? "" : Env.RefPath;
            string envType = Env.Selected == null ? "" : Env.EnvType.ToString();

            // Version/Machine/Evnrionment summary.
            summaryInfo["Location"] = fileInfo.Directory.FullName;
            summaryInfo["Version"] = app.Version;
            summaryInfo["Machine"] = Environment.MachineName;
            summaryInfo["User"] = Auth.UserName;
            summaryInfo["StartTime"] = app.StartTime.ToString();
            summaryInfo["EndTime"] = DateTime.Now.ToString();
            summaryInfo["Duration"] = (DateTime.Now - app.StartTime).ToString();
            summaryInfo["Diagnostics"] = "Diagnostics.log";
            summaryInfo["Args"] = Environment.CommandLine;
            summaryInfo["Env Type"] = envType;
            summaryInfo["Env Name"] = envName;
            summaryInfo["Config"] = envPath;

            // Get the log summary.
            List<string> logSummary = Logger.GetLogInfo();
            for (int ndx = 0; ndx < logSummary.Count; ndx++)
                summaryInfo["Log " + ndx + 1] = logSummary[ndx];

            // Get arguments that were applied to the argument reciever.
            if (app.Settings.ArgsReciever != null
                && app.Settings.ArgsRequired && app.Settings.ArgsAppliedToReciever)
                ArgsHelper.GetArgValues(summaryInfo, app.Settings.ArgsReciever);
        }


        /// <summary>
        /// Returns a string with the application summary information..
        /// </summary>
        /// <param name="isStart">True if the application is starting.</param>
        /// <param name="summaryInfo">Dictionary with summary info.</param>
        /// <returns>String with application summary.</returns>
        public static string GetAppRunSummaryAsString(bool isStart, IDictionary summaryInfo)
        {
            string header = isStart ? "Application Start" : "Application End";            
            StringBuilder buffer = new StringBuilder();
            // Print the summary.
            // - Version          : 1.0.0.0
            // - Machine          : KISHORE1
            // - User             : kishore1\kishore
            buffer.Append("=================================================== " + Environment.NewLine);
            buffer.Append("=========== " + header + " Information ============= " + Environment.NewLine);
            StringHelper.DoFixedLengthPrinting(summaryInfo, 4, (key, val) => buffer.Append(" - " + key + " : " + val + Environment.NewLine));
            buffer.Append("=================================================== " + Environment.NewLine);
            buffer.Append("=================================================== " + Environment.NewLine);
            return buffer.ToString();
        }

    }
}
