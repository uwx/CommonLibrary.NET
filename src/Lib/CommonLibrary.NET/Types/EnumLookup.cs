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
using System.Linq;
using System.Text;
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// Enum lookup extensions.
    /// </summary>
    public static class EnumLookupExtensions
    {
        /// <summary>
        /// Parses a string after validating it and returns 
        /// the value of the enumeration it represents.
        /// </summary>
        /// <param name="lookup">Enumeration lookup.</param>
        /// <param name="enumType">Type of enumeration.</param>
        /// <param name="val">Value of string.</param>
        /// <param name="results">Validation results.</param>
        /// <returns>Enumeration value.</returns>
        public static object GetValue(this EnumLookup lookup, Type enumType, string val, IValidationResults results)
        {
            return GetValue(lookup, enumType, val, results, string.Empty);
        }


        /// Parses a string after validating it and returns 
        /// the value of the enumeration it represents.
        /// <param name="lookup">Enumeration lookup.</param>
        /// <param name="enumType">Type of enumeration.</param>
        /// <param name="val">Value of string.</param>
        /// <param name="results">Validation results.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Enumeration value.</returns>
        public static object GetValue(this EnumLookup lookup, Type enumType, string val, IValidationResults results, string defaultValue)
        {
            // Invalid enum value.
            if (!EnumLookup.IsValid(enumType, val))
            {
                results.Add("Invalid value '" + val + "' for " + enumType.Name);
                return false;
            }

            return EnumLookup.GetValue(enumType, val, defaultValue);
        }
    }



    /// <summary>
    /// Class to parse/lookup the value of enums.
    /// </summary>
    public class EnumLookup
    {
        /// <summary>
        ///  Stores the possible values for various Enum types.
        /// </summary>
        private static IDictionary<string, Dictionary<string, string>> _enumMap;


        /// <summary>
        /// Static initializer.
        /// </summary>
        static EnumLookup()
        {
            _enumMap = new Dictionary<string, Dictionary<string, string>>();
        }


        /// <summary>
        /// Register enum mappings.
        /// </summary>
        /// <param name="enumType">Type of enumeration.</param>
        /// <param name="aliasValuesDelimited">String with delimited values of enumeration.</param>
        public static void Register(Type enumType, string aliasValuesDelimited)
        {
            Dictionary<string, string> enumValues = enumValues = new Dictionary<string, string>();
            _enumMap[enumType.FullName] = enumValues;

            SetupMappings(enumType, enumValues, aliasValuesDelimited);
        }


        /// <summary>
        /// Determines if the string based enum value is valid.
        /// </summary>
        /// <param name="enumType">Type of enumeration.</param>
        /// <param name="val">String based enum value.</param>
        /// <returns>True if the string represents a valid value.</returns>
        public static bool IsValid(Type enumType, string val)
        {
            ConfirmRegistration(enumType);

            val = val.ToLower().Trim();            
            return _enumMap[enumType.FullName].ContainsKey(val);
        }


        /// <summary>
        /// Get the enum Value.
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object GetValue(Type enumType, string val)
        {
            return GetValue(enumType, val, string.Empty);
        }


        /// <summary>
        /// Get the enum Value
        /// </summary>
        /// <param name="enumType">Type of enumeration.</param>
        /// <param name="val">String with value.</param>
        /// <param name="defaultVal">Default value.</param>
        /// <returns>Enumeration with corresponding value.</returns>
        public static object GetValue(Type enumType, string val, string defaultVal)
        {            
            if (!IsValid(enumType, val))
            {
                // Can't do anything if a default value was not supplied.
                if (string.IsNullOrEmpty(defaultVal))
                    throw new ArgumentException("Value '" + val + "' is not a valid value for " + enumType.GetType().Name);
                else
                    return Enum.Parse(enumType, defaultVal, true);
            }
            val = val.ToLower().Trim();
            string actualValue = _enumMap[enumType.FullName][val];
            return Enum.Parse(enumType, actualValue, true);
        }


        private static void ConfirmRegistration(Type enumType)
        {
            // The type of enum is not registered.. so register it.
            if (!_enumMap.ContainsKey(enumType.FullName))
                Register(enumType, string.Empty);
        }


        /// <summary>
        /// Set the various mappings for an enum value.
        /// </summary>
        /// <param name="type">Type of enumeration.</param>
        /// <param name="enumValues">Dictionary with enumeration values.</param>
        /// <param name="aliasValuesDelimeted">Delimited string with alias values.</param>
        private static void SetupMappings(Type type, Dictionary<string, string> enumValues, string aliasValuesDelimeted)
        {
            // Put each value of the num in the map.
            foreach (FieldInfo fInfo in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string name = fInfo.Name;
                enumValues.Add(name.ToLower(), name);
            }

            // Store the alias vlues.
            if (!string.IsNullOrEmpty(aliasValuesDelimeted))
            {
                // Get an alias / value pair.
                // E.g. pro=professional,guru=professional,master=professional,blackbelt=professional
                // You get the idea.
                string[] aliasValuePairs = aliasValuesDelimeted.Split(',');

                // For each pair.
                foreach (string aliasValuePair in aliasValuePairs)
                {
                    // Get the alias name and it's value.
                    string[] tokens = aliasValuePair.Split('=');

                    // guru=professional
                    string alias = tokens[0].Trim().ToLower();
                    string aliasValue = tokens[1].Trim().ToLower();
                    enumValues[alias] = aliasValue;
                }
            }
        }        
    }
}
