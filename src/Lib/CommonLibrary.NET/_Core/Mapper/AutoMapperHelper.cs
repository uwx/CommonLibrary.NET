using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// Helper class for automapping
    /// </summary>
    public class AutoMapperHelper
    {
        /// <summary>
        /// Get a dictionary of the types Public | SetProperty | Instance property names to the PropertyInfo
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isCaseSensitive"></param>
        /// <returns></returns>
        public static IDictionary<string, PropertyInfo> GetPropertyMap(Type type, bool isCaseSensitive)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
            if (props.Length == 0) return null;

            // Lowercase property map.
            var propMap = new Dictionary<string, PropertyInfo>();
            foreach (var prop in props)
            {
                if (isCaseSensitive)
                    propMap[prop.Name] = prop;
                else
                    propMap[prop.Name.ToLower()] = prop;
            }
            return propMap;
        }


        /// <summary>
        /// Get the propertyinfo of the lowest level nested object.
        /// </summary>
        /// <param name="obj">The object having the property</param>
        /// <param name="propName">"Address.Neighborhood.City" </param>
        /// <param name="createNestedObjectIfNull">Whether or not to create the nested object if it's null.</param>
        /// <returns></returns>
        public static KeyValuePair<object, PropertyInfo> GetNestedProperty(object obj, string propName, bool createNestedObjectIfNull)
        {
            int indexOfDot = propName.IndexOf(".");

            // 1. No "." top level property.
            if (indexOfDot < 0)
                return new KeyValuePair<object, PropertyInfo>(obj, obj.GetType().GetProperty(propName));

            // E.g. Address.Area.Name
            // 1. Get property name "Address" and "Coordinates"
            int startOfTrim = 0;
            string subobjectName = propName.Substring(startOfTrim, indexOfDot);
            string subPropName = propName.Substring(indexOfDot + 1);
            
            // 2. Get the propertyInfo for "Address"
            PropertyInfo prop = obj.GetType().GetProperty(subobjectName);
            object subobj = prop.GetValue(obj, null);
            if (subobj == null && createNestedObjectIfNull)
            {
                subobj = Activator.CreateInstance(prop.PropertyType);
                prop.SetValue(obj, subobj, null);
            }
            int nextIndexOfDot = propName.IndexOf(".", indexOfDot + 1);

            // 3. Keep moving forward to the next property
            while (nextIndexOfDot > 0)
            {
                // 4. Get next property info. 
                // "Area"
                subobjectName = propName.Substring(indexOfDot + 1, (nextIndexOfDot - indexOfDot) -1);

                // "Name"
                subPropName = propName.Substring(nextIndexOfDot + 1);
                prop = subobj.GetType().GetProperty(subobjectName);
                var nested = prop.GetValue(subobj, null);

                // Create if empty
                if (nested == null && createNestedObjectIfNull)
                {
                    nested = Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(subobj, nested, null);
                }
                subobj = nested;
                nextIndexOfDot = propName.IndexOf(".", nextIndexOfDot + 1);
            }

            // 5. Finally, this is the last property info.
            prop = subobj.GetType().GetProperty(subPropName);
            return new KeyValuePair<object, PropertyInfo>(subobj, prop);
        }
    }
}
