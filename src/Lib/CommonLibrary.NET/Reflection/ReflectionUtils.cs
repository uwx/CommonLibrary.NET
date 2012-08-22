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
using System.Collections.Specialized;
using System.Collections;
using System.Linq;

namespace ComLib.Reflection
{
    
    /// <summary>
    /// Various reflection based utility methods.
    /// </summary>
    public class ReflectionUtils
    {
        /// <summary>
        /// Set object properties on T using the properties collection supplied.
        /// The properties collection is the collection of "property" to value.
        /// </summary>
        /// <typeparam name="T">A class type.</typeparam>
        /// <param name="obj">Object whose properties will be set.</param>
        /// <param name="properties">List of key/value pairs with property names and values.</param>
        public static void SetProperties<T>(T obj, IList<KeyValuePair<string, string>> properties) where T : class        
        {
            // Validate
            if (obj == null) { return; }

            foreach (KeyValuePair<string, string> propVal in properties)
            {
                SetProperty<T>(obj, propVal.Key, propVal.Value);
            }
        }


        /// <summary>
        /// Set the object properties using the prop name and value.
        /// </summary>
        /// <typeparam name="T">A class type.</typeparam>
        /// <param name="obj">Object whose property will be set.</param>
        /// <param name="propName">Property name to set.</param>
        /// <param name="propVal">Property value to set.</param>
        public static void SetProperty<T>(object obj, string propName, object propVal) where T : class        
        {
            Guard.IsNotNull(obj, "Object containing properties to set is null");
            Guard.IsTrue(!string.IsNullOrEmpty(propName), "Property name not supplied.");

            // Remove spaces.
            propName = propName.Trim();
            if (string.IsNullOrEmpty(propName)) { throw new ArgumentException("Property name is empty."); }

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propName);

            // Correct property with write access 
            if (propertyInfo != null && propertyInfo.CanWrite )
            {
                // Check same Type
                if (ReflectionTypeChecker.CanConvertToCorrectType(propertyInfo, propVal))
                {
                    object convertedVal = ReflectionTypeChecker.ConvertToSameType(propertyInfo, propVal);
                    propertyInfo.SetValue(obj, convertedVal, null);
                }
            }
        }


        /// <summary>
        /// Set the object properties using the prop name and value.
        /// </summary>
        /// <param name="obj">Object whose property will be set.</param>
        /// <param name="propName">Property name to set.</param>
        /// <param name="propVal">Property value to set.</param>
        public static void SetProperty(object obj, string propName, object propVal) 
        {
            // Remove spaces.
            propName = propName.Trim();
            if (string.IsNullOrEmpty(propName)) { throw new ArgumentException("Property name is empty."); }

            Type type = obj.GetType();
            PropertyInfo propertyInfo = type.GetProperty(propName);

            // Correct property with write access 
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(obj, propVal, null);
            }
        }


        /// <summary>
        /// Set the object properties using the prop name and value.
        /// </summary>
        /// <param name="obj">Object whose property will be set.</param>
        /// <param name="prop">Property information.</param>
        /// <param name="propVal">Property value to set.</param>
        /// <param name="catchException">Try to catch any exception and
        /// not throw it to the caller.</param>
        public static void SetProperty(object obj, PropertyInfo prop, object propVal, bool catchException)
        {
            // Correct property with write access 
            if (prop != null && prop.CanWrite)
            {
                if (!catchException)
                    prop.SetValue(obj, propVal, null);
                else
                    Try.Catch(() => prop.SetValue(obj, propVal, null), null);
            }
        }


        /// <summary>
        /// Set the property value using the string value.
        /// </summary>
        /// <param name="obj">Object whose property will be set.</param>
        /// <param name="prop">Property information.</param>
        /// <param name="propVal">Property value to set.</param>
        public static void SetProperty(object obj, PropertyInfo prop, string propVal)
        {
            Guard.IsNotNull(obj, "Object containing properties to set is null");
            Guard.IsNotNull(prop, "Property not supplied.");

            // Correct property with write access 
            if (prop != null && prop.CanWrite)
            {
                // Check same Type
                if (ReflectionTypeChecker.CanConvertToCorrectType(prop, propVal))
                {
                    object convertedVal = ReflectionTypeChecker.ConvertToSameType(prop, propVal);
                    prop.SetValue(obj, convertedVal, null);
                }
            }
        }


        /// <summary>
        /// Get the property value.
        /// </summary>
        /// <param name="obj">Object whose property will be retrieved.</param>
        /// <param name="propName">Name of property to retrieve.</param>
        /// <returns>Property value.</returns>
        public static object GetPropertyValue(object obj, string propName)
        {
            Guard.IsNotNull(obj, "Must provide object to get it's property.");
            Guard.IsTrue(!string.IsNullOrEmpty(propName), "Must provide property name to get property value.");

            propName = propName.Trim();

            PropertyInfo property = obj.GetType().GetProperty(propName);
            if (property == null) return null;

            return property.GetValue(obj, null);
        }

        
        /// <summary>
        /// Get all the property values.
        /// </summary>
        /// <param name="obj">Object whose properties will be retrieved.</param>
        /// <param name="properties">List of properties to retrieve.</param>
        /// <returns>List with property values.</returns>
        public static IList<object> GetPropertyValues(object obj, IList<string> properties)
        {
            IList<object> propertyValues = new List<object>();

            foreach(string property in properties)
            {
                PropertyInfo propInfo = obj.GetType().GetProperty(property);
                object val = propInfo.GetValue(obj, null);
                propertyValues.Add(val);
            }
            return propertyValues;
        }


        /// <summary>
        /// Get all the properties.
        /// </summary>
        /// <param name="obj">Object whose properties will be retrieved.</param>
        /// <param name="propsDelimited">Delimited list with properties to retrieve.</param>
        /// <returns>List of property values.</returns>
        public static IList<PropertyInfo> GetProperties(object obj, string propsDelimited)
        {
            return GetProperties(obj.GetType(), propsDelimited.Split(','));            
        }


        /// <summary>
        /// Get property information for a type.
        /// </summary>
        /// <param name="type">Type whose property names to retrieve.</param>
        /// <param name="props">Array with property names to look for.</param>
        /// <returns>List with property information of found properties.</returns>
        public static IList<PropertyInfo> GetProperties(Type type, string[] props)
        {
            PropertyInfo[] allProps = type.GetProperties();
            List<PropertyInfo> propToGet = new List<PropertyInfo>();
            IDictionary<string, string> propsMap = props.ToDictionary<string>();
            foreach (PropertyInfo prop in allProps)
            {
                if (propsMap.ContainsKey(prop.Name))
                    propToGet.Add(prop);
            }
            return propToGet;
        }


        /// <summary>
        /// Get all the properties.
        /// </summary>
        /// <param name="type">Type whose property names to retrieve.</param>
        /// <param name="props">Array with property names to look for.</param>
        /// <param name="flags">Flags to use when searching for properties.</param>
        /// <returns>List with property information of found properties.</returns>
        public static IList<PropertyInfo> GetProperties(Type type, string[] props, BindingFlags flags)
        {
            PropertyInfo[] allProps = type.GetProperties(flags);            
            List<PropertyInfo> propToGet = new List<PropertyInfo>();
            IDictionary<string, string> propsMap = props.ToDictionary<string>();
            foreach (PropertyInfo prop in allProps)
            {
                if (propsMap.ContainsKey(prop.Name))
                    propToGet.Add(prop);
            }
            return propToGet;
        }


        /// <summary>
        /// Gets the property value safely, without throwing an exception.
        /// If an exception is caught, null is returned.
        /// </summary>
        /// <param name="obj">Object to look into.</param>
        /// <param name="propInfo">Information of property to retrieve.</param>
        /// <returns>Retrieved property value.</returns>
        public static object GetPropertyValueSafely(object obj, PropertyInfo propInfo)
        {
            Guard.IsNotNull(obj, "Must provide object to get it's property.");
            if (propInfo == null) return null;

            object result = null;
            try
            {
                result = propInfo.GetValue(obj, null);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }


        /// <summary>
        /// Gets all the properties of an object.
        /// </summary>
        /// <param name="obj">Object to look into.</param>
        /// <param name="criteria">Matching criteria.</param>
        /// <returns>List with information of matched properties.</returns>
        public static IList<PropertyInfo> GetAllProperties(object obj, Predicate<PropertyInfo> criteria)
        {
            if (obj == null) { return null; }
            return GetProperties(obj.GetType(), criteria);
        }


        /// <summary>
        /// Get the properties of a type.
        /// </summary>
        /// <param name="type">Type to look into.</param>
        /// <param name="criteria">Matching criteria.</param>
        /// <returns>List of information of matched properties.</returns>
        public static IList<PropertyInfo> GetProperties(Type type, Predicate<PropertyInfo> criteria)
        {
            IList<PropertyInfo> allProperties = new List<PropertyInfo>();
            PropertyInfo[] properties = type.GetProperties();
            if (properties == null || properties.Length == 0) { return null; }

            // Now check for all writeable properties.
            foreach (PropertyInfo property in properties)
            {
                // Only include writable properties and ones that are not in the exclude list.
                bool okToAdd = (criteria == null) ? true : criteria(property);
                if (okToAdd)
                {
                    allProperties.Add(property);
                }
            }
            return allProperties;
        }


        /// <summary>
        /// Gets all the writable properties of an object.
        /// </summary>
        /// <param name="obj">Object to look into.</param>
        /// <param name="propsToExclude">Dictionary with properties to exclude.</param>
        /// <returns>List with information of matched properties.</returns>
        public static IList<PropertyInfo> GetWritableProperties(object obj, StringDictionary propsToExclude)
        {
            IList<PropertyInfo> props = ReflectionUtils.GetWritableProperties(obj.GetType(),
                 delegate(PropertyInfo property)
                 {
                     bool okToAdd = propsToExclude == null ? property.CanWrite : (property.CanWrite && !propsToExclude.ContainsKey(property.Name));                
                     return okToAdd;
                 });
            return props;
        }


        /// <summary>
        /// Gets all the properties of a type.
        /// </summary>
        /// <param name="propsToExclude">Dictionary with properties to exclude.</param>
        /// <param name="typ">Type to look into.</param>
        /// <returns>List with information of matched properties.</returns>
        public static IList<PropertyInfo> GetProperties(StringDictionary propsToExclude, Type typ)
        {
            IList<PropertyInfo> props = ReflectionUtils.GetWritableProperties(typ,
                 delegate(PropertyInfo property)
                 {
                     bool okToAdd = propsToExclude == null ? true : (!propsToExclude.ContainsKey(property.Name));
                     return okToAdd;
                 });
            return props;
        }


        /// <summary>
        /// Gets all the properties of the object as dictionary of property names to propertyInfo.
        /// </summary>
        /// <param name="obj">Object to look into.</param>
        /// <param name="criteria">Matching criteria.</param>
        /// <returns>Dictionary with property name and information of matched properties.</returns>
        public static IDictionary<string, PropertyInfo> GetPropertiesAsMap(object obj, Predicate<PropertyInfo> criteria)
        {
            IList<PropertyInfo> matchedProps = GetProperties(obj.GetType(), criteria);
            IDictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();

            // Now check for all writeable properties.
            foreach (PropertyInfo prop in matchedProps)
            {
                props.Add(prop.Name, prop);
            }
            return props;
        }


        /// <summary>
        /// Get all the properties.
        /// </summary>
        /// <param name="type">Type to look into.</param>
        /// <param name="flags">Flags to use when looking for properties.</param>
        /// <param name="isCaseSensitive">True to use the property name in the
        /// dictionary with its lower-cased value.</param>
        /// <returns>Dictionary with property name and information of found properties.</returns>
        public static IDictionary<string, PropertyInfo> GetPropertiesAsMap(Type type, BindingFlags flags, bool isCaseSensitive)
        {
            PropertyInfo[] allProps = type.GetProperties(flags);
            IDictionary<string, PropertyInfo> propsMap = new Dictionary<string, PropertyInfo>();
            foreach (PropertyInfo prop in allProps)
            {
                if (isCaseSensitive)
                    propsMap[prop.Name] = prop;
                else
                    propsMap[prop.Name.Trim().ToLower()] = prop;
            }

            return propsMap;
        }


        /// <summary>
        /// Get all the properties.
        /// </summary>
        /// <typeparam name="T">Type to look into.</typeparam>
        /// <param name="flags">Flags to use when looking for properties.</param>
        /// <param name="isCaseSensitive">True to use the property name in the
        /// dictionary with its lower-cased value.</param>
        /// <returns>Dictionary with property name and information of found properties.</returns>
        public static IDictionary<string, PropertyInfo> GetPropertiesAsMap<T>(BindingFlags flags, bool isCaseSensitive)
        {
            Type type = typeof(T);
            return GetPropertiesAsMap(type, flags, isCaseSensitive);
        }


        /// <summary>
        /// Get the propertyInfo of the specified property name.
        /// </summary>
        /// <param name="type">Type to look into.</param>
        /// <param name="propertyName">Name of property.</param>
        /// <returns>Information of property.</returns>
        public static PropertyInfo GetProperty(Type type, string propertyName)
        {
            IList<PropertyInfo> props = GetProperties(type, 
                delegate(PropertyInfo property)
                {
                    return property.Name == propertyName;
                });
            return props[0];
        }        


        /// <summary>
        /// Gets a list of all the writable properties of the class associated with the object.
        /// </summary>
        /// <param name="type">Type to look into.</param>
        /// <param name="criteria">Matching criteria.</param>
        /// <remarks>This method does not take into account, security, generics, etc.
        /// It only checks whether or not the property can be written to.</remarks>
        /// <returns>List with information of matching properties.</returns>
        public static IList<PropertyInfo> GetWritableProperties(Type type, Predicate<PropertyInfo> criteria )
        {
            IList<PropertyInfo> props = ReflectionUtils.GetProperties(type,
                 delegate(PropertyInfo property) 
                 {
                     // Now determine if it can be added based on criteria.
                     bool okToAdd = (criteria == null) ? property.CanWrite : (property.CanWrite && criteria(property));
                     return okToAdd;
                 });
            return props;
        }



        /// <summary>
        /// Invokes the method on the object provided.
        /// </summary>
        /// <param name="obj">The object containing the method to invoke</param>
        /// <param name="methodName">arguments to the method.</param>
        /// <param name="parameters">Parameters to pass when invoking the method.</param>
        public static object InvokeMethod(object obj, string methodName, object[] parameters)
        {
            Guard.IsNotNull(methodName, "Method name not provided.");
            Guard.IsNotNull(obj, "Can not invoke method on null object");

            methodName = methodName.Trim();
            
            // Validate.
            if (string.IsNullOrEmpty(methodName)) { throw new ArgumentException("Method name not provided."); }

            MethodInfo method = obj.GetType().GetMethod(methodName);
            object output = method.Invoke(obj, parameters);
            return output;
        }

        
        /// <summary>
        /// Copies the property value from the source to destination.
        /// </summary>
        /// <param name="source">Source object.</param>
        /// <param name="destination">Destination object.</param>
        /// <param name="prop">Information of property whose value
        /// is to be copied from the source to the target object.</param>
        public static void CopyPropertyValue(object source, object destination, PropertyInfo prop)
        {
            object val = prop.GetValue(source, null);
            prop.SetValue(destination, val, null);
        }
    }
}
