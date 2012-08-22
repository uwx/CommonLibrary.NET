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
using System.Text;
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// Reflection utility class for attributes.
    /// </summary>
    public class Attributes
    {
        /// <summary>
        /// Get the description attribute from the assembly associated with <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type who's assembly's description should be obtained.</param>
        /// <param name="defaultVal">Default value to use if description is not available.</param>
        /// <returns>Assembly info description.</returns>
        public static string GetAssemblyInfoDescription(Type type, string defaultVal)
        {
            // Get the assembly object.
            Assembly assembly = type.Assembly;

            // See if the Assembly Description is defined.
            bool isDefined = Attribute.IsDefined(assembly, typeof(AssemblyDescriptionAttribute));
            string description = defaultVal;

            if (isDefined)
            {
                AssemblyDescriptionAttribute adAttr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, 
                    typeof(AssemblyDescriptionAttribute));

                if (adAttr != null) description = adAttr.Description;
            }
            return description;

        }


        /// <summary>
        /// Gets the attributes of the specified type applied to the class.
        /// </summary>
        /// <typeparam name="T">Type of attributes.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>List of attributes from the specified object.</returns>
        public static IList<T> GetClassAttributes<T>(object obj)
        {
            // Check
            if (obj == null) return new List<T>();

            object[] attributes = obj.GetType().GetCustomAttributes(typeof(T), false);

            IList<T> attributeList = new List<T>();

            // iterate through the attributes, retrieving the 
            // properties
            foreach (Object attribute in attributes)
            {                
                attributeList.Add((T)attribute);
            }
            return attributeList;
        }


        /// <summary>
        /// Loads widgets from the assembly name supplied.
        /// </summary>
        /// <typeparam name="T">Type of attributes to return.</typeparam>
        /// <param name="assemblyName">The name of the assembly to load widgets from.</param>
        /// <param name="action">A callback for the datatype and widgetattribute.</param>
        /// <returns>List of key/value pairs.</returns>
        public static IList<KeyValuePair<Type, T>> GetClassAttributesFromAssembly<T>(string assemblyName, Action<KeyValuePair<Type, T>> action)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            var types = assembly.GetTypes();
            var widgets = new List<KeyValuePair<Type, T>>();
            foreach (var type in types)
            {
                var attributes = type.GetCustomAttributes(typeof(T), false);
                if (attributes != null && attributes.Length > 0)
                {
                    var pair = new KeyValuePair<Type, T>(type, (T)attributes[0]);
                    widgets.Add(pair);
                    action(pair);
                }
            }
            return widgets;
        }        


        /// <summary>
        /// Get a list of property info's that have the supplied attribute applied to it.
        /// </summary>
        /// <typeparam name="T">Type of attribute to look for.</typeparam>
        /// <param name="obj">Object to search in.</param>
        /// <returns>Dictionary with property information.</returns>
        public static IDictionary<string, KeyValuePair<T, PropertyInfo>> GetPropsWithAttributes<T>(object obj) where T : Attribute
        {
            // Check
            if (obj == null) return new Dictionary<string, KeyValuePair<T, PropertyInfo>>();
            Dictionary<string, KeyValuePair<T, PropertyInfo>> map = new Dictionary<string, KeyValuePair<T,PropertyInfo>>();

            //IList<PropertyInfo> props = ReflectionUtils.GetAllProperties(obj, null);
            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach( PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                    map[prop.Name] = new KeyValuePair<T, PropertyInfo>(attrs[0] as T, prop);
            }
            return map;
        }


        /// <summary>
        /// Get a list of property info's that have the supplied attribute applied to it.
        /// </summary>
        /// <typeparam name="T">Type of attribute to look for.</typeparam>
        /// <param name="obj">Object to search in.</param>
        /// <returns>List with property information.</returns>
        public static List<PropertyInfo> GetPropsOnlyWithAttributes<T>(object obj) where T : Attribute
        {
            // Check
            if (obj == null) return new List<PropertyInfo>();
            List<PropertyInfo> matchedProps = new List<PropertyInfo>();

            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                    matchedProps.Add(prop);       
            }
            return matchedProps;
        }


        /// <summary>
        /// Get a list of property info's that have the supplied attribute applied to it.
        /// </summary>
        /// <typeparam name="T">Type of attribute to look for.</typeparam>
        /// <param name="obj">Object to look in.</param>
        /// <returns>List with property information.</returns>
        public static List<KeyValuePair<T, PropertyInfo>> GetPropsWithAttributesList<T>(object obj) where T : Attribute
        {
            // Check
            if (obj == null) return new List<KeyValuePair<T, PropertyInfo>>();
            List<KeyValuePair<T, PropertyInfo>> map = new List<KeyValuePair<T, PropertyInfo>>();

            PropertyInfo[] props = obj.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                    map.Add(new KeyValuePair<T, PropertyInfo>(attrs[0] as T, prop));
            }
            return map;
        }


        /// <summary>
        /// Gets all the properties associated with the supplied types that have the attribute applied to them.
        /// </summary>
        /// <typeparam name="TPropAttrib">The type of the attribute that properties should have</typeparam>
        /// <param name="types">The list of types to search properties for.</param>
        /// <param name="action">Callback</param>
        /// <returns>List with property information.</returns>
        public static IList<KeyValuePair<PropertyInfo, TPropAttrib>> GetPropertiesWithAttributesOnTypes<TPropAttrib>(IList<Type> types, Action<Type, KeyValuePair<PropertyInfo, TPropAttrib>> action) where TPropAttrib: Attribute
        {
            var propertyAttributes = new List<KeyValuePair<PropertyInfo, TPropAttrib>>();
            foreach (var type in types)
            {
                var properties = type.GetProperties();                
                foreach (var prop in properties)
                {
                    var attributes = prop.GetCustomAttributes(typeof(TPropAttrib), true);
                    if (attributes != null && attributes.Length > 0)
                    {
                        var pair = new KeyValuePair<PropertyInfo, TPropAttrib>(prop, attributes[0] as TPropAttrib);
                        propertyAttributes.Add(pair);
                        action(type, pair);
                    }
                }                
            }
            return propertyAttributes;
        }
    }
}
