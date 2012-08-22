using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace ComLib
{
    /// <summary>
    /// Helper class for parsing strings in the format of parameters ( key / value pairs or simple keys )
    /// </summary>
    public class LexHelper
    {
        /// <summary>
        /// Parses the line into a dictionary of key/value pairs.
        /// </summary>
        /// <param name="line">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>        
        /// <returns></returns>
        public static IDictionary<string, string> Parse(string line, string prefix, string separator)
        {
            var lexer = new LexArgs();
            var tokens = lexer.ParseText(line);
            return Parse(tokens.ToArray(), prefix, separator);                
        }


        /// <summary>
        /// Parses the arguments into a dictionary of key/value pairs.
        /// </summary>
        /// <param name="args">e.g. "-env:prod", "-config:prod.xml", "-date:T-1", "20"</param>
        /// <param name="prefix">Prefix used for named arguments. E.g. "-" as in "-env:prod"</param>
        /// <param name="separator">Separator used between name and value of named arguments. E.g. ":" as in "-env:prod"</param>        
        /// <returns></returns>
        public static IDictionary<string, string> Parse(string[] args, string prefix, string separator)
        {
            // Validate the inputs.            
            bool checkNamedArgs = !string.IsNullOrEmpty(separator);
            bool hasPrefix = !string.IsNullOrEmpty(prefix);

            // Name of argument can only be letter, number, (-,_,.).
            // The value can be anything.
            string patternKeyValue = @"(?<name>[a-zA-Z0-9\-_\.]+)" + separator + @"(?<value>.+)";
            string patternBool = @"(?<name>[a-zA-Z0-9\-_\.]+)";
            patternKeyValue = hasPrefix ? prefix + patternKeyValue : patternKeyValue;
            patternBool = hasPrefix ? prefix + patternBool : patternBool;

            var map = new Dictionary<string, string>();
            LexHelper.ParseArgs(args, map, null, patternKeyValue, patternBool);
            return map;
        }


        /// <summary>
        /// Checks for named args and gets the name and corresponding value.
        /// </summary>
        /// <param name="args">The arguments to parse</param>
        /// <param name="namedArgs">Dictionary to populate w/ named arguments.</param>
        /// <param name="unnamedArgs">List to populate with unamed arguments.</param>
        /// <param name="regexPatternWithValue">Regex pattern for key/value pair args.
        /// e.g. -env:prod where key=env value=prod</param>
        /// <param name="regexPatternBool">Regex pattern for boolean based key args.
        /// -sendemail key=sendemail the value is automatically set to true.
        /// This is useful for enabled options e.g. -sendemail -recurse </param>
        public static void ParseArgs(string[] args, IDictionary<string, string> namedArgs, List<string> unnamedArgs,
            string regexPatternWithValue, string regexPatternBool)
        {
            foreach (string arg in args)
            {
                Match matchKeyValue = Regex.Match(arg, regexPatternWithValue);
                Match matchBool = Regex.Match(arg, regexPatternBool);

                if (matchKeyValue != null && matchKeyValue.Success)
                {
                    string name = matchKeyValue.Groups["name"].Value;
                    string val = matchKeyValue.Groups["value"].Value;
                    namedArgs[name] = val;
                }
                else if (matchBool != null && matchBool.Success)
                {
                    string name = matchBool.Groups["name"].Value;
                    namedArgs[name] = "true";
                }
                else if(unnamedArgs != null)
                {
                    unnamedArgs.Add(arg);
                }
            }
        }
    }
}
