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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;

using ComLib;

namespace ComLib.Arguments
{
    /// <summary>
    /// Helper class for argument parsing.
    /// </summary>
    public class ArgsHelper
    {
        private static Func<string, string> _substitutor;


        /// <summary>
        /// Initialize the string parser. e.g. Args.InitParser( (textargs) => LexArgs.ParseList(text) );
        /// </summary>
        /// <param name="substitutor">The substitutor lamda</param>
        public static void InitServices(Func<string, string> substitutor)
        {
            _substitutor = substitutor;
        }


        /// <summary>
        /// Gets a list of all the argument definitions that are applied
        /// (via attributes) on the argument reciever object supplied.
        /// </summary>
        /// <param name="argsReciever">Object containing args attributes.</param>
        /// <returns></returns>
        public static List<ArgAttribute> GetArgsFromReciever(object argsReciever)
        {
            // Get all the properties that have arg attributes.
            List<KeyValuePair<ArgAttribute, PropertyInfo>> args = Attributes.GetPropsWithAttributesList<ArgAttribute>(argsReciever);
            List<ArgAttribute> argsList = new List<ArgAttribute>();
            args.ForEach((pair) => argsList.Add(pair.Key));
            return argsList;
        }


        /// <summary>
        /// Get the all argument names / values from the object that recievers the arguments.
        /// </summary>
        /// <param name="argsReciever"></param>
        /// <returns></returns>
        public static IDictionary GetArgValues(object argsReciever)
        {
            var dict = new OrderedDictionary();
            GetArgValues(dict, argsReciever);
            return dict;
        }


        /// <summary>
        /// Get the all argument names / values from the object that recievers the arguments.
        /// </summary>
        /// <param name="argsReciever"></param>
        /// <param name="argsValueMap"></param>
        /// <returns></returns>
        public static void GetArgValues(IDictionary argsValueMap, object argsReciever)
        {
            var args = Attributes.GetPropsWithAttributesList<ArgAttribute>(argsReciever);
            args.ForEach(argPair =>
            {
                object val = ReflectionHelper.GetPropertyValueSafely(argsReciever, argPair.Value);
                argsValueMap[argPair.Value.Name] = val;
            });
        }


        /// <summary>
        /// Applies the argument values to the object argument reciever.
        /// </summary>
        /// <param name="parsedArgs"></param>
        /// <param name="argReciever"></param>
        /// <param name="errors"></param>
        public static void CheckAndApplyArgs(Args parsedArgs, object argReciever, IList<string> errors)
        {
            List<KeyValuePair<ArgAttribute, PropertyInfo>> mappings = Attributes.GetPropsWithAttributesList<ArgAttribute>(argReciever);
            List<ArgAttribute> argSpecs = new List<ArgAttribute>();
            mappings.ForEach((pair) => argSpecs.Add(pair.Key));

            // Set the supplied argument value on the object that should recieve the value.
            ArgsValidator.Validate(parsedArgs, argSpecs, errors, (argAttr, argVal, ndx) =>
            {
                SetValue(argReciever, mappings[ndx], argVal);
            });
        }


        /// <summary>
        /// Set the argument value from command line on the property of the object
        /// recieving the value.
        /// </summary>
        /// <param name="argReciever"></param>
        /// <param name="val"></param>
        /// <param name="rawArgValue"></param>
        private static void SetValue(object argReciever, KeyValuePair<ArgAttribute, PropertyInfo> val, string rawArgValue)
        {
            ArgAttribute argAttr = val.Key;

            // First interpret.
            string argValue = rawArgValue;
            if (argAttr.Interpret)
            {
                argValue = Substitute(argValue);
            }
            ReflectionHelper.SetProperty(argReciever, val.Value, argValue);
        }


        /// <summary>
        /// Set the argument value from command line on the property of the object
        /// recieving the value.
        /// </summary>
        /// <param name="args"></param>
        public static void InterpretValues(Args args)
        {
            if (args.Schema.IsEmpty) return;

            foreach (var arg in args.Schema.Items)
            {
                if (arg.Interpret && arg.IsNamed)
                {
                    string rawval = string.Empty;
                    string argName = string.Empty;
                    if (args.Contains(arg.Name))
                    {
                        rawval = args.Named[arg.Name];
                        argName = arg.Name;
                    }
                    else if (args.Contains(arg.Alias))
                    {
                        rawval = args.Named[arg.Alias];
                        argName = arg.Alias;
                    }
                    else if (args.Contains(arg.NameLowered))
                    {
                        rawval = args.Named[arg.NameLowered];
                        argName = arg.NameLowered;
                    }
                    else if (args.Contains(arg.AliasLowered))
                    {
                        rawval = args.Named[arg.AliasLowered];
                        argName = arg.AliasLowered;
                    }
                    if (!string.IsNullOrEmpty(argName))
                    {
                        string interpreted = Substitute(rawval);
                        args.Named[argName] = interpreted;
                    }
                }
                else if (arg.Interpret && !arg.IsNamed && args.Positional != null && arg.IndexPosition < args.Positional.Count)
                {
                    string rawval = args.Positional[arg.IndexPosition];
                    string interpreted = Substitute(rawval);
                    args.Positional[arg.IndexPosition] = interpreted;
                }
            }
        }


        private static string Substitute(string original)
        {
            if (_substitutor == null) return original;
            string converted = _substitutor(original);
            return converted;
        }
    }
}
