using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Xml;


namespace ComLib
{
    
    /// <summary>
    /// Class to automatically map values from one source to another source.
    /// </summary>
    public class AutoMapper
    {
        /// <summary>
        /// Default settings
        /// </summary>
        private static readonly AutoMapperSettings DefaultSettings = new AutoMapperSettings()
        {
            CatchErrors = false,
            IsCaseSensitive = false,
            MapNestedObjects = false
        };


        /// <summary>
        /// Map item from IDictionary to the type supplied.
        /// </summary>
        /// <typeparam name="T">The type to map</typeparam>
        /// <param name="map">The dictionary of key/value pairs to map from</param>
        /// <param name="item">The destination object to map values to</param>
        public static T Map<T>(IDictionary map, T item) where T : class
        {
            return Map<T>(map, item, DefaultSettings, null);
        }


        /// <summary>
        /// Map item from IDictionary to the type supplied using the settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="settings"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static T Map<T>(IDictionary source, T item, AutoMapperSettings settings, IList<string> errors) where T : class
        {
            // Map of properties
            var propMap = AutoMapperHelper.GetPropertyMap(item.GetType(), settings.IsCaseSensitive);
            bool hasNameFilter = !string.IsNullOrEmpty(settings.NameFilter);
            string nameFilter = !hasNameFilter ? string.Empty : settings.NameFilter;
            if(!settings.IsCaseSensitive) nameFilter = nameFilter.ToLower();

            // 1. Iterate through all the keys
            foreach (DictionaryEntry keyvalue in source)
            {
                var key = keyvalue.Key.ToString();
                var value = keyvalue.Value;

                // 2. Convert key to lowercase if not case sensitive.
                if( !settings.IsCaseSensitive ) key = key.ToLower();

                // 3. Check if name prefix filter exists.
                if (!hasNameFilter || (hasNameFilter && key.StartsWith(nameFilter)))
                {
                    // 4. Convert key name to actual destination prop name.
                    var destinationKeyName = !hasNameFilter ? key : key.Replace(nameFilter, string.Empty);

                    // 5. Check if destination property exists.
                    if (propMap.ContainsKey(destinationKeyName))
                    {
                        var prop = propMap[destinationKeyName];

                        // Finally set the value.
                        SetValue(prop, settings, item, destinationKeyName, value, errors);
                    }
                    // Check if mapping nested objects and key is Address.City (example).
                    else if (settings.MapNestedObjects && destinationKeyName.Contains("."))
                    {
                        var nested = AutoMapperHelper.GetNestedProperty(item, keyvalue.Key.ToString(), settings.AutoCreateNestedObjects);
                        SetValue(nested.Value, settings, nested.Key, nested.Value.Name, value, errors);
                    }
                }
            }
            return item;
        }


        /// <summary>
        /// Map item from IDictionary to the type supplied using the settings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <param name="settings"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static T Map<T>(XmlNode source, T item, AutoMapperSettings settings, IList<string> errors) where T : class
        {
            // Map of properties
            var propMap = AutoMapperHelper.GetPropertyMap(item.GetType(), settings.IsCaseSensitive);
            bool hasNameFilter = !string.IsNullOrEmpty(settings.NameFilter);
            string nameFilter = !hasNameFilter ? string.Empty : settings.NameFilter;
            if (!settings.IsCaseSensitive) nameFilter = nameFilter.ToLower();

            // 1. Iterate through all the keys
            for(var ndx = 0; ndx < source.Attributes.Count; ndx++)
            {

                var key = source.Attributes[ndx].Name;
                var value = source.Attributes[ndx].Value;

                // 2. Convert key to lowercase if not case sensitive.
                if (!settings.IsCaseSensitive) key = key.ToLower();

                // 3. Check if name prefix filter exists.
                if (!hasNameFilter || (hasNameFilter && key.StartsWith(nameFilter)))
                {
                    // 4. Convert key name to actual destination prop name.
                    var destinationKeyName = !hasNameFilter ? key : key.Replace(nameFilter, string.Empty);

                    // 5. Check if destination property exists.
                    if (propMap.ContainsKey(destinationKeyName))
                    {
                        var prop = propMap[destinationKeyName];

                        // Finally set the value.
                        SetValue(prop, settings, item, destinationKeyName, value, errors);
                    }
                    // Check if mapping nested objects and key is Address.City (example).
                    else if (settings.MapNestedObjects && destinationKeyName.Contains("."))
                    {
                        var nested = AutoMapperHelper.GetNestedProperty(item, source.Attributes[ndx].Name, settings.AutoCreateNestedObjects);
                        SetValue(nested.Value, settings, nested.Key, nested.Value.Name, value, errors);
                    }
                }
            }
            return item;
        }


        private static void SetValue(PropertyInfo prop, AutoMapperSettings settings, 
            object item, string destinationKeyName, object value, IList<string> errors)
        {   
            // Finally map the source to the destination value.
            if (settings.CatchErrors)
                try
                {
                    var newVal = Convert.ChangeType(value, prop.PropertyType);
                    prop.SetValue(item, newVal, null);
                }
                catch { if (errors != null) errors.Add("Unable to map source " + destinationKeyName + " to destination " + destinationKeyName); }
            else
            {
                var newVal = Convert.ChangeType(value, prop.PropertyType);
                prop.SetValue(item, newVal, null);
            }
        }
    }
}
