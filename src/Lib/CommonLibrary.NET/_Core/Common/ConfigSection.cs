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
using System.Collections;
using System.Collections.Specialized;


namespace ComLib
{

    /// <summary> 
    /// Simple class to lookup stored configuration settings by key. 
    /// Also provides type conversion methods. 
    /// GetInt("PageSize"); 
    /// GetBool("IsEnabled"); 
    /// </summary> 
    public class ConfigSection : OrderedDictionary, IConfigSection
    {
        /// <summary> 
        /// Allow default constructor. 
        /// </summary> 
        public ConfigSection() { }


        /// <summary>
        /// Initialize the config section w/ the name.
        /// </summary>
        /// <param name="name"></param>
        public ConfigSection(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Name of config section.
        /// </summary>
        public string Name { get; set; }        
        

        /// <summary>
        /// Get typed root setting by string key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T Get<T>(string key)
        {
            object result = this[key];
            T converted = Converter.ConvertTo<T>(result);
            return converted;
        }


        /// <summary>
        /// Validate and return the default value if the key is not present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetDefault<T>(string key, T defaultValue)
        {
            // Validate and return default value.
            if (!Contains(key)) return defaultValue;
            return Get<T>(key);
        }


        /// <summary>
        /// Get section key value.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string section, string key)
        {
            return this[section, key];
        }


        /// <summary>
        /// Get typed section key value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string section, string key)
        {
            object result = this[section, key];
            T converted = (T)Converter.ConvertObj<T>(result);
            return converted;
        }


        /// <summary>
        /// Get section/key value if present, default value otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetDefault<T>(string section, string key, T defaultValue)
        {
            // Validate and return default value.
            if (!Contains(section, key)) return defaultValue;
            return Get<T>(section, key);
        }


        /// <summary> 
        /// Get a section. 
        /// </summary> 
        /// <param name="sectionName"></param> 
        /// <returns></returns> 
        public IConfigSection GetSection(string sectionName)
        {
            return this[sectionName] as IConfigSection;
        }


        /// <summary> 
        /// Get a section associated with the specified key at the specified index.
        /// </summary> 
        /// <param name="sectionName"></param>
        /// <param name="ndx"></param>
        /// <returns></returns> 
        public IConfigSection GetSection(string sectionName, int ndx)
        {
            return ConfigSectionUtils.Get<IConfigSection>(this, sectionName, ndx);
        }


        /// <summary> 
        /// Get / set the value using both the section name and key. 
        /// e.g. "globalsettings", "pageSize"
        /// </summary> 
        /// <param name="sectionName"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public object this[string sectionName, string key]
        {
            get
            {
                object section = this[sectionName];
                if (section is IDictionary)
                {
                    return ((IDictionary)section)[key];
                }
                return null;
            }
            set
            {
                object section = this[sectionName];
                if (section is IDictionary)
                {
                    ((IDictionary)section)[key] = value;
                }
                else if ( section == null)
                {
                    Add(sectionName, key, value);
                }
            }
        }
        

        /// <summary>
        /// Checks whether or not the key exists in the section.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string section, string key)
        {
            IConfigSection configSection = GetSection(section);
            if (configSection == null) return false;

            return configSection.Contains(key);
        }


        /// <summary>
        /// The names of all the sections.
        /// </summary>
        public List<string> Sections
        {
            get
            {
                List<string> sections = new List<string>();
                foreach (DictionaryEntry entry in this)
                {
                    // Single entry section associated w/ key.
                    if (entry.Value is IConfigSection)
                        sections.Add(entry.Key.ToString());
                    
                    // Check for list of sections with same key.
                    else if (entry.Value is List<object>)
                    {
                        List<object> items = (List<object>)entry.Value;
                        if (items[0] is IConfigSection)
                            sections.Add(entry.Key.ToString());
                    }
                }
                return sections;
            }
        }


        /// <summary>
        /// Add the key value to the section specified.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="overWrite"></param>
        public virtual void Add(string sectionName, string key, object val, bool overWrite)
        {
            ConfigSectionUtils.Add(this, sectionName, key, val, overWrite);
        }


        /// <summary>
        /// Add the key value to the section specified.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void Add(string sectionName, string key, object val)
        {
            ConfigSectionUtils.Add(this, sectionName, key, val, false);
        }


        /// <summary>
        /// Add key value with option of overwriting value of existing key
        /// or adding to a list of values associated w/ the same key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="overWrite"></param>
        public virtual void AddMulti(string key, object val, bool overWrite)
        {
            ConfigSectionUtils.Add(this, key, val, overWrite);
        }
    }



    /// <summary>
    /// Config section utils for adding/removing from both a map and list.
    /// </summary>
    internal class ConfigSectionUtils
    {
        /// <summary>
        /// Add item to map checking for duplicate keys.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="overWrite"></param>
        public static void Add(IDictionary dictionary, string key, object val, bool overWrite)
        {
            object result = dictionary[key];
            if (result == null || overWrite)
            {
                dictionary[key] = val;
                return;
            }
            // Make this a list.
            List<object> valueList = null;
            if (result is List<object>)
            {
                valueList = (List<object>)result;
                valueList.Add(val);
            }
            else
            {
                valueList = new List<object>();
                valueList.Add(result);
                valueList.Add(val);
                dictionary[key] = valueList;
            }            
        }


        /// <summary>
        /// Add key / value pair to the section specified.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="sectionName">E.g. "server"</param>
        /// <param name="key">"name"</param>
        /// <param name="val">"myserver01"</param>
        /// <param name="overWrite">true</param>
        public static void Add(IDictionary dictionary, string sectionName, 
            string key, object val, bool overWrite)
        {
            object result = dictionary[sectionName];
            IConfigSection section = null;

            // Handle null.
            if (result is IConfigSection)
            {
                section = (IConfigSection)result;
            }
            else if (result == null)
            {
                section = new ConfigSection(sectionName);
                dictionary.Add(sectionName, section);
            }
            else if (result is List<object>)
            {
                section = (IConfigSection)((List<object>)result)[0];
            }
            Add(section, key, val, overWrite);
        }


        /// <summary>
        /// Get the entry at the specified index of the multivalue list 
        /// associated with the key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key">"post"</param>
        /// <param name="ndx">1</param>
        /// <returns></returns>
        public static T Get<T>(IDictionary dictionary, string key, int ndx)
        {
            object result = dictionary[key];
            if (result == null) return default(T);

            if (result is T) return (T)result;

            if (result is List<object>)
            {
                List<object> valueList = result as List<object>;
                if (ndx < 0 || ndx >= valueList.Count)
                    return default(T);

                return (T)valueList[ndx];
            }
            return default(T);
        }
    }
}
