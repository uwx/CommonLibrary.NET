/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: Please refer to site for license
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
using System.IO;


namespace ComLib
{
    /// <summary>
    /// This class contains utility methods for saving an object state.
    /// </summary>
    public class SimpleStateHelper
    {
        private static IDictionary<string, SimpleStateMap> _stateMap = new Dictionary<string, SimpleStateMap>();


        /// <summary>
        /// Load the state into the properties of <paramref name="objToSave"/>
        /// </summary>
        /// <param name="objToSave">The obj to save.</param>
        /// <param name="state">The state as a string.
        /// e.g.
        /// line #: propertyname,value
        /// line1 : Title,Learn how to program in asp.net mvc./r/n
        /// line2 : Cost,30/r/n
        /// line3 : Content,learn how to program in asp.net using commonlibrary.net CMS which is an
        ///       : ASP.NET MVC CMS using commonlibrary.net/r/n
        /// line6 : Url,1,http://commonlibrarynetcms.codeplex.com/r/n
        /// </param>
        /// <param name="includeProps">The include props.</param>
        /// <param name="excludeProps">The exclude props.</param>
        /// <param name="stringClobProps">The string clob props.</param>
        public static void LoadState(object objToSave, string state, string includeProps, string excludeProps, string stringClobProps)
        {
            if (objToSave == null) throw new ArgumentNullException("Object to save is null");
            if (string.IsNullOrEmpty(includeProps) && string.IsNullOrEmpty(includeProps) && string.IsNullOrEmpty(includeProps))
                return;

            SimpleStateMap propStateMap = null;
            if (_stateMap.ContainsKey(objToSave.GetType().FullName))
                propStateMap = _stateMap[objToSave.GetType().FullName];
            else
            {
                propStateMap = SimpleStateMap.ToPropStateMap(objToSave.GetType(), includeProps, excludeProps, stringClobProps);
                _stateMap[objToSave.GetType().FullName] = propStateMap;
            }

            using (StringReader reader = new StringReader(state))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Format is <propertyname>,<numberoflines>,<value>
                    int ndxComma = line.IndexOf(',');
                    string propName = line.Substring(0, ndxComma);
                    if (propStateMap.AllPropMap.ContainsKey(propName))
                    {
                        string val = line.Substring(ndxComma + 1);

                        // Now set the value.
                        var prop = propStateMap.AllPropMap[propName];
                        object convertedVal = Converter.ConvertTo(prop.PropertyType, val);
                        prop.SetValue(objToSave, convertedVal, null);
                    }
                }
            }
        }


        /// <summary>
        /// Saves all the extended settings into a simple multi-line key/value pair.
        /// This also handles the case for stringclobs.
        /// </summary>
        /// <param name="objToSave">The obj to save.</param>
        /// <param name="includeProps">The include props.</param>
        /// <param name="excludeProps">The exclude props.</param>
        /// <param name="stringClobProps">The string clob props.</param>
        /// <returns>String with property keys and values.</returns>
        public static string CreateState(object objToSave, string includeProps, string excludeProps, string stringClobProps)
        {
            if (objToSave == null) throw new ArgumentNullException("Object to save is null");
            if (string.IsNullOrEmpty(includeProps) && string.IsNullOrEmpty(includeProps) && string.IsNullOrEmpty(includeProps))
                return string.Empty;

            SimpleStateMap propStateMap = null;
            if (_stateMap.ContainsKey(objToSave.GetType().FullName))
                propStateMap = _stateMap[objToSave.GetType().FullName];
            else
            {
                propStateMap = SimpleStateMap.ToPropStateMap(objToSave.GetType(), includeProps, excludeProps, stringClobProps);
                _stateMap[objToSave.GetType().FullName] = propStateMap;
            }

            // 4. Now save state for each property.
            var buffer = new StringBuilder();
            foreach (var propPair in propStateMap.IncludeProps)
            {
                // Save state for each property.
                PropertyInfo prop = propStateMap.AllPropMap[propPair.Key];
                object val = prop.GetValue(objToSave, null);
                bool isStringClob = propStateMap.StringClobProps.ContainsKey(prop.Name);
                string valString = string.Empty;
                if (isStringClob && val != null)
                    valString = ((string)val).Replace(Environment.NewLine, "<br />");
                else if (val != null)
                    valString = val.ToString();
                string state = string.Format("{0},{1}" + Environment.NewLine, prop.Name, valString);
                buffer.Append(state);
            }

            string completeState = buffer.ToString();
            return completeState;
        }
    }



    /// <summary>
    /// Container for storing properties that should be included/excluded during saving of an objects state.
    /// </summary>
    public class SimpleStateMap
    {
        /// <summary>
        /// Properties that should be included when saving state.
        /// </summary>
        public IDictionary<string, string> IncludeProps;
        
        
        /// <summary>
        /// Properties that should be excluded when saving state.
        /// </summary>
        public IDictionary<string, string> ExcludeProps;
        

        /// <summary>
        /// Properties that represent long strings.
        /// </summary>
        public IDictionary<string, string> StringClobProps;
        
        
        /// <summary>
        /// All instance/get/set properties.
        /// </summary>
        public PropertyInfo[] Props;


        /// <summary>
        /// All instance/get/set properties as a map.
        /// </summary>
        public IDictionary<string, PropertyInfo> AllPropMap;


        /// <summary>
        /// Toes the prop state map.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includePropsDelimeted">The include props delimeted.</param>
        /// <param name="excludePropsDelimted">The exclude props delimted.</param>
        /// <param name="stringClobPropsDelimited">The string clob props delimited.</param>
        /// <returns>Instance of simple state map with property configuration.</returns>
        public static SimpleStateMap ToPropStateMap(Type type, string includePropsDelimeted, string excludePropsDelimted, string stringClobPropsDelimited)
        {
            // 1. Map of columns to include.
            string[] colsToInclude = string.IsNullOrEmpty(includePropsDelimeted) ? new string[0] : includePropsDelimeted.Split(',');
            var colsToIncludeMap = colsToInclude.ToDictionary();

            // 2. Map of columns to exclude.
            string[] colsToExclude = string.IsNullOrEmpty(excludePropsDelimted) ? new string[0] : excludePropsDelimted.Split(',');
            var colsToExcludeMap = colsToExclude.ToDictionary();

            // 2. Get map of string clob props.
            string[] stringClobProps = string.IsNullOrEmpty(stringClobPropsDelimited) ? new string[0] : stringClobPropsDelimited.Split(',');
            var stringClobMap = stringClobProps.ToDictionary();

            // 3. Get all the props as both 1(list) and 2(map).
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty);
            props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            IDictionary<string, PropertyInfo> propMap = new Dictionary<string, PropertyInfo>();
            foreach (var prop in props) propMap[prop.Name] = prop;

            return new SimpleStateMap() { ExcludeProps = colsToExcludeMap, IncludeProps = colsToIncludeMap, StringClobProps = stringClobMap, Props = props, AllPropMap = propMap };
        }
    }
}
