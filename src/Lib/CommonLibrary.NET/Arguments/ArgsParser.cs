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
    /// Class providing utility methods for parsing a string or collection of arguments.
    /// </summary>
    public partial class Args
    {
        private static Func<string, List<string>> _stringParser;


        /// <summary>
        /// Initialize the string parser and substutions.
        /// Args.InitServices((textargs) => ComLib.Parsing.LexArgs.ParseList(textargs), (arg) => ComLib.Subs.Substitutor.Substitute(arg));
        /// </summary>
        /// <param name="stringParser"></param>
        /// <param name="substitutor"></param>
        public static void InitServices(Func<string, List<string>> stringParser, Func<string, string> substitutor = null)
        {
            _stringParser = stringParser;
            ArgsHelper.InitServices(substitutor);
        }


        /// <summary>
        /// Parse the line into <see cref="Args"/> object.
        /// </summary>
        /// <param name="text">The line of text containing arguments to parse.</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string text)
        {
            if(string.IsNullOrEmpty(text)) return new BoolMessageItem<Args>(null, false, "No arguments provided.");
            if(_stringParser == null) return new BoolMessageItem<Args>(null, false, "Parser for text based args not initialized.");

            List<string> args = _stringParser(text);
            return TryCatch(() => Parse(args.ToArray()));
        }


        /// <summary>
        /// Parses the line into <see cref="Args"/> object using the supplied prefix and separator.
        /// </summary>
        /// <param name="text">The line of text containing arguments to parse.</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string text, string prefix, string separator)
        {
            if (string.IsNullOrEmpty(text)) return new BoolMessageItem<Args>(null, false, "No arguments provided.");
            if (_stringParser == null) return new BoolMessageItem<Args>(null, false, "Parser for text based args not initialized.");

            List<string> args = _stringParser(text);
            return TryCatch(() => Parse(args.ToArray(), prefix, separator));
        }


        /// <summary>
        /// Parses the arguments and checks for named arguments and non-named arguments.
        /// </summary>
        /// <param name="text">The line of text containing arguments to parse.</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>
        /// <param name="argReciever">The object to apply the argument values to. This must have ArgAttributes on it's properties.</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string text, string prefix, string separator, object argReciever)
        {
            if (string.IsNullOrEmpty(text)) return new BoolMessageItem<Args>(null, false, "No arguments provided.");
            if (_stringParser == null) return new BoolMessageItem<Args>(null, false, "Parser for text based args not initialized.");

            List<string> args = _stringParser(text);
            return TryCatch(() => Parse(args.ToArray(), prefix, separator, argReciever));
        }


        /// <summary>
        /// Parse the args into <see cref="Args"/> object.
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string[] args)
        {
            return Parse(args, "-", ":");
        }


        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>        
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string[] args, string prefix, string separator)
        {
            // Validate the inputs.
            BoolMessageItem<Args> validationResult = ArgsValidator.ValidateInputs(args, prefix, separator);
            if (!validationResult.Success) return validationResult;

            bool checkNamedArgs = !string.IsNullOrEmpty(separator);
            bool hasPrefix = !string.IsNullOrEmpty(prefix);

            // Name of argument can only be letter, number, (-,_,.).
            // The value can be anything.
            string patternKeyValue = @"(?<name>[a-zA-Z0-9\-_\.]+)" + separator + @"(?<value>.+)";
            string patternBool = @"(?<name>[a-zA-Z0-9\-_\.]+)";
            patternKeyValue = hasPrefix ? prefix + patternKeyValue : patternKeyValue;
            patternBool = hasPrefix ? prefix + patternBool : patternBool;

            Args resultArgs = new Args(args, prefix, separator);
            List<string> errors = new List<string>();

            // Put the arguments into the named args dictionary and/or list.
            if (checkNamedArgs)
            {
                LexHelper.ParseArgs(args, resultArgs.Named, resultArgs.Positional, patternKeyValue, patternBool);
            }
            else
            {
                resultArgs.Positional = new List<string>(args);
            }
            return new BoolMessageItem<Args>(resultArgs, true, string.Empty);
        }


        /// <summary>
        /// Parses the arguments and checks for named arguments and non-named arguments.
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>                
        /// <param name="argReciever">The object to apply the argument values to. This must have ArgAttributes on it's properties.</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string[] args, string prefix, string separator, object argReciever)
        {
            // Parse the args first.
            BoolMessageItem<Args> parseResult = Parse(args, prefix, separator);
            if (!parseResult.Success) return parseResult;
            
            Args resultArgs = parseResult.Item;
            
            // 1. Set the schema.
            List<ArgAttribute> schemaItems = ArgsHelper.GetArgsFromReciever(argReciever);
            resultArgs.Schema.Items = schemaItems;

            // 2. Parse interpreted values like ${today}.
            ArgsHelper.InterpretValues(resultArgs);

            // 3. Apply the values to the reciever and store any errors.
            var errors = new List<string>();

            // Set the named argument values on the object's properties.
            if (argReciever != null)
            {
                ArgsHelper.CheckAndApplyArgs(resultArgs, argReciever, errors);
            }
            string singleMessage = string.Empty;
            foreach (string error in errors) { singleMessage += error + Environment.NewLine; }

            return new BoolMessageItem<Args>(resultArgs, errors.Count == 0, singleMessage);
        }


        /// <summary>
        /// Parses the arguments into a <see cref="Args"/> object.
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="argsSpec">List of expected argument items(both named and positional).</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string[] args, List<ArgAttribute> argsSpec)
        {
            return Parse(args, "-", ":", argsSpec);
        }


        /// <summary>
        /// Parses the arguments into a <see cref="Args"/>
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>                
        /// <param name="argsSpec">List of expected argument items(both named and positional).</param>        
        /// <returns></returns>
        public static BoolMessageItem<Args> Parse(string[] args, string prefix, string separator, List<ArgAttribute> argsSpec)
        {
            // Parse the args first.
            BoolMessageItem<Args> parseResult = Parse(args, prefix, separator);
            if (!parseResult.Success) return parseResult;

            Args resultArgs = parseResult.Item;

            // 1. Set the schema.
            resultArgs.Schema.Items = argsSpec;

            // 2. Parse interpreted values like ${today}.
            ArgsHelper.InterpretValues(resultArgs);

            // 3. Validate the values.
            var errors = new List<string>();
                
            if (argsSpec != null && argsSpec.Count > 0)
            {
                ArgsValidator.Validate(resultArgs, argsSpec, errors, null);
            }
            string singleMessage = string.Empty;
            foreach (string error in errors) { singleMessage += error + Environment.NewLine; }

            return new BoolMessageItem<Args>(resultArgs, errors.Count == 0, singleMessage);
        }


        /// <summary>
        /// Parses the arguments into a <see cref="Args"/> and prints any errors to the console.
        /// </summary>
        /// <param name="rawArgs">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>                
        /// <param name="argsReciever">The object to apply the argument values to. This must have ArgAttributes on it's properties.</param>
        /// <returns>True if arguments are valid, false otherwise. argsReciever contains the arguments.</returns>
        public static bool Accept(string[] rawArgs, string prefix, string separator, object argsReciever)
        {
            Args args = new Args(rawArgs, prefix, separator);
            if (args.IsHelp)
            {
                Console.WriteLine(ArgsUsage.BuildUsingReciever(argsReciever, prefix, separator));
                return false;
            }

            // Parse the command line arguments.
            BoolMessageItem<Args> results = Args.Parse(rawArgs, prefix, separator, argsReciever);
            if (!results.Success)
            {
                Console.WriteLine("ERROR: Invalid arguments supplied.");
                Console.WriteLine(results.Message);
                Console.WriteLine(ArgsUsage.BuildUsingReciever(argsReciever, prefix, separator));
                return false;
            }
            return true;
        }


        /// <summary>
        /// Parses the arguments into a <see cref="Args"/> and prints any errors to the console.
        /// </summary>
        /// <param name="rawArgs">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>                
        /// <param name="minArgs">The minimum number of arguments expected.</param>
        /// <param name="supported">List of expected argument items(both named and positional).</param>                        
        /// <param name="examples">List of examples of how to use the examples.</param>
        /// <returns></returns>
        public static BoolMessageItem<Args> Accept(string[] rawArgs, string prefix, string separator,
            int minArgs, List<ArgAttribute> supported, List<string> examples)
        {
            Args args = new Args(rawArgs, supported);
            if( args.IsEmpty )
            {
                Console.WriteLine("No arguments were supplied." + Environment.NewLine);
                ArgsUsage.Show(supported, examples);
                return new BoolMessageItem<Args>(args, false, "No arguments were supplied.");
            }

            // Try to validate and accept the arguments.
            BoolMessageItem<Args> result = Args.Parse(rawArgs, prefix, separator, supported);
            return result;
        }


        #region Private members
        /// <summary>
        /// Wrap the parsing of the arguments into a trycatch.
        /// </summary>
        /// <param name="executor"></param>
        /// <returns></returns>
        private static BoolMessageItem<Args> TryCatch(Func<BoolMessageItem<Args>> executor)
        {
            try
            {
                return executor();
            }
            catch (Exception ex)
            {
                return new BoolMessageItem<Args>(null, false, ex.Message);
            }
        }
        #endregion
    }   
}
