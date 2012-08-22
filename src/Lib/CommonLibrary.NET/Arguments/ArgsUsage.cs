/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;


namespace ComLib.Arguments
{
    /// <summary>
    /// Arguments utility class.
    /// </summary>
    public class ArgsUsage
    {
        /// <summary>
        /// Build a string showing what arguments are expected.
        /// This is done by inspecting the argattributes on all the
        /// properties of the supplied object.
        /// </summary>
        /// <param name="argsReciever"></param>
        /// <returns></returns>
        public static string BuildUsingReciever(object argsReciever)
        {
            return BuildUsingReciever(argsReciever, "-", ":");
        }


        /// <summary>
        /// Build a string showing what arguments are expected.
        /// This is done by inspecting the argattributes on all the
        /// properties of the supplied object.
        /// </summary>
        /// <param name="argsReciever"></param>
        /// <param name="prefix"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string BuildUsingReciever(object argsReciever, string prefix, string separator)
        {
            // Get all the properties that have arg attributes.
            List<ArgAttribute> argsList = ArgsHelper.GetArgsFromReciever(argsReciever);
            return Build("app", argsList, prefix, separator);
        }


        /// <summary>
        /// Build usage of the arguments.
        /// </summary>
        /// <param name="argsList"></param>
        /// <returns></returns>
        public static string Build(IList<ArgAttribute> argsList)
        {
            return Build("app", argsList, "-", ":");
        }


        /// <summary>
        /// Build usage of the arguments using the schema
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public static string Build(ArgsSchema schema)
        {
            return Build("app", schema.Items, "-", ":");
        }


        /// <summary>
        /// Build descriptive usage showing arguments and sample runs.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="argAttributes"></param>
        /// <param name="prefix"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Build(string appName, IList<ArgAttribute> argAttributes, string prefix, string separator)
        {
            var samples = BuildSampleRuns(appName, argAttributes, prefix, separator);
            return BuildDescriptive(argAttributes, samples, prefix, separator);
        }


        /// <summary>
        /// Build a sample run.
        /// </summary>
        /// <param name="appName">Application name</param>
        /// <param name="argAttributes">List of args</param>
        /// <param name="prefix">Prefix used for the arguments</param>
        /// <param name="separator">Separator used between arg and it's value</param>
        /// <returns></returns>
        public static List<string> BuildSampleRuns(string appName, IList<ArgAttribute> argAttributes, string prefix, string separator)
        {
            // Get all the required named args.
            var requiredNamed = from a in argAttributes where !string.IsNullOrEmpty(a.Name) && a.IsRequired select a;
            var optionalNamed = from a in argAttributes where !string.IsNullOrEmpty(a.Name) && !a.IsRequired select a;
            var requiredIndex = from a in argAttributes where string.IsNullOrEmpty(a.Name) && a.IsRequired select a;
            var optionalIndex = from a in argAttributes where string.IsNullOrEmpty(a.Name) && !a.IsRequired select a;

            // Required.
            string requiredNamedSample = "", requiredIndexSample = "", optionalNamedSample = "", optionalIndexSample = "";
            if( string.IsNullOrEmpty(prefix)) prefix = "-";
            if( string.IsNullOrEmpty(separator)) separator = ":";

            requiredNamed.ForEach(argAttr => requiredNamedSample += string.Format("{0}{1}{2}{3} ", prefix, argAttr.Name, separator, argAttr.Example));
            optionalNamed.ForEach(argAttr => optionalNamedSample += string.Format("{0}{1}{2}{3} ", prefix, argAttr.Name, separator, argAttr.Example));
            requiredIndex.ForEach(argAttr => requiredIndexSample += string.Format("{0} ", argAttr.Example));
            optionalIndex.ForEach(argAttr => optionalIndexSample += string.Format("{0} ", argAttr.Example));

            // Get all the examples.
            var examples = new List<string>()
            {
                appName + " " + requiredNamedSample + " " + requiredIndexSample,
                appName + " " + requiredNamedSample + " " + optionalNamedSample + " " + requiredIndexSample + " " + optionalIndexSample
            };
            return examples;
        }


        /// <summary>
        /// Build a sample runs using examples provided rather from the schema.
        /// </summary>
        /// <param name="examples"></param>
        /// <returns></returns>
        public static string BuildSampleRuns(IList<string> examples)
        {
            string exampleText = string.Empty;
                
            // Now add examples.           
            if (examples != null && examples.Count > 0)
            {
                for (int count = 0; count < examples.Count; count++)
                {
                    string example = examples[count];
                    exampleText += (count + 1).ToString() + ". " + example + Environment.NewLine;
                }                
            }
            return exampleText;
        }


        /// <summary>
        /// Build a string showing argument usage.
        /// </summary>
        /// <param name="argAttributes">The argument definitions.</param>
        /// <param name="examples">Examples of command line usage.</param>
        /// <param name="prefix">Prefix for the command line named args. e.g. "-"</param>
        /// <param name="separator">Separator for named args key/value pairs. e.g. ":".</param>
        /// <returns></returns>
        public static string BuildDescriptive(IList<ArgAttribute> argAttributes, IList<string> examples, string prefix, string separator)
        {
            // Check Args.
            if (string.IsNullOrEmpty(prefix)) prefix = "-";
            if (string.IsNullOrEmpty(separator)) separator = ":";

            // Get the length of the longest named argument.
            int maxLength = 0;
            foreach (ArgAttribute argAtt in argAttributes)
                if (!string.IsNullOrEmpty(argAtt.Name) && argAtt.Name.Length > maxLength)
                    maxLength = argAtt.Name.Length;
            maxLength += 4;

            string argsUsage = BuildArgs(maxLength, argAttributes, prefix, separator);
            string exampleRuns = BuildSampleRuns(examples);
            string usage = Environment.NewLine + "OPTIONS" + Environment.NewLine
                         + argsUsage + Environment.NewLine + Environment.NewLine
                         + "SAMPLES:" + Environment.NewLine + Environment.NewLine
                         + exampleRuns;
            return usage;
        }


        /// <summary>
        /// Build the usage for arguments only.
        /// </summary>
        /// <param name="paddingLength"></param>
        /// <param name="argAttributes"></param>
        /// <param name="prefix"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string BuildArgs(int paddingLength, IList<ArgAttribute> argAttributes, string prefix, string separator)
        {
            string usage = Build(argAttributes, (arg) =>
            {
                string padding = StringHelper.GetFixedLengthString(string.Empty, paddingLength, " ");
                string info = string.Empty;
                string name = " " + prefix + arg.Name;
                string alias = (arg.IsAliased) ? prefix + arg.Alias : "";
                string description = string.IsNullOrEmpty(arg.Description) ? string.Empty : arg.Description;
                string required = arg.IsRequired ? "Required" : "Optional";
                string defaultValue = arg.DefaultValue == null ? "\"\"" : arg.DefaultValue.ToString();
                string caseSensitivity = arg.IsCaseSensitive ? "Case Sensitive" : "Not CaseSensitive";
                if (string.IsNullOrEmpty(arg.Name))
                    name = "   [" + arg.IndexPosition.ToString() + "]";

                name = StringHelper.GetFixedLengthString(name, paddingLength, " ");
                if (!arg.IsAliased)
                    info += name + description + Environment.NewLine;
                else
                {
                    info += name + alias + Environment.NewLine;
                    info += padding + description + Environment.NewLine;
                }
                info += string.Format(padding + "{0}, {1}, {2}, default: {3}", required, arg.DataType.Name, caseSensitivity, defaultValue) + Environment.NewLine;
                info += padding + "Example: " + arg.ExampleMultiple + Environment.NewLine;
                if (arg.IsUsedOnlyForDevelopment) info += padding + "DEVELOPMENT USE ONLY" + Environment.NewLine;
                info += Environment.NewLine;
                return info;
            });
            return usage;
        }


        /// <summary>
        /// Build a string showing what arguments are expected.
        /// This is done by inspecting the argattributes on all the
        /// properties of the supplied object.
        /// </summary>
        /// <param name="argAttributes"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static string Build(IList<ArgAttribute> argAttributes, Func<ArgAttribute, string> formatter)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(Environment.NewLine);

            // For each property.
            foreach (ArgAttribute arg in argAttributes)
            {
                string argInfo = formatter(arg);
                buffer.Append(argInfo);
            }
            string usage = buffer.ToString();
            return usage;
        }


        /// <summary>
        /// Return a string that shows how this should be used.
        /// </summary>
        /// <returns></returns>
        public static void Show(List<ArgAttribute> argdefs, List<string> examples)
        {
            string usage = BuildDescriptive(argdefs, examples, "-", ":");
            Console.WriteLine(usage);
        }


        /// <summary>
        /// Show usage using reciever.
        /// </summary>
        /// <param name="reciever"></param>
        /// <param name="prefix"></param>
        /// <param name="separator"></param>
        public static void ShowUsingReciever(object reciever, string prefix, string separator)
        {
            string usage = BuildUsingReciever(reciever, prefix, separator);
            Console.WriteLine(usage);
        }


        /// <summary>
        /// Shows the error on the console.
        /// </summary>
        /// <param name="message"></param>
        public static void ShowError(string message)
        {
            Console.WriteLine("================================================");
            Console.WriteLine("=================== ERRORRS ====================");
            Console.WriteLine();
            Console.WriteLine("Command line arguments are NOT valid.");
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("================================================");
            Console.WriteLine("================================================");
            Console.WriteLine();
        }
    }
}
