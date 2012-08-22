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
using System.Collections;
using System.Text;
using System.Reflection;



namespace ComLib.Configuration
{
    /// <summary>
    /// Utility class for the configuration reflector.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Current configuration source based on the current
        /// Environment. <see cref="ComLib.Environments.Envs.Current"/>
        /// </summary>
        private static IConfigSource _current = new ConfigSource();

        
        /// <summary>
        /// Initialize the current config provider.
        /// </summary>
        /// <param name="inheritanceBasedConfig"></param>
        public static void Init(IConfigSource inheritanceBasedConfig)
        {
            _current = inheritanceBasedConfig;            
        }


        /// <summary>
        /// Current config.
        /// </summary>
        public static IConfigSource Current
        {
            get { return _current; }
        }


        /// <summary>
        /// Convenience method for getting typed config value from current config provider.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return _current.Get<object>(key);
        }


        /// <summary>
        /// Convenience method for getting typed config value from current config provider.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return _current.Get<string>(key);
        }


        /// <summary>
        /// Convenience method for getting typed config value from current config provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            return _current.Get<T>(key);
        }


        /// <summary>
        /// Convenience method for getting typed config value from current config provider using index position of key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexOfKey"></param>
        /// <returns></returns>
        public static T Get<T>(int indexOfKey)
        {
            // BUG: Indexing by numbers are not supported.
            return _current.Get<T>(indexOfKey.ToString());
        }


        /// <summary>
        /// Convenience method for getting section/key value from current config.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string section, string key)
        {
            return _current.Get(section, key);
        }


        /// <summary>
        /// Convenience method for getting section/key value from current config.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string section, string key)
        {
            return _current.Get<string>(section, key);
        }


        /// <summary>
        /// Convenience method for getting section/key value from current config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string section, string key)
        {
            return _current.Get<T>(section, key);
        }


        /// <summary>
        /// Convenience method for getting section/key value from current config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue">The default value to use if key is not available.</param>
        /// <returns></returns>
        public static T GetDefault<T>(string section, string key, T defaultValue)
        {
            return _current.GetDefault<T>(section, key, defaultValue);
        }


        /// <summary>
        /// Get the configuration section with the specified name.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IConfigSection GetSection(string sectionName)
        {
            return _current.GetSection(sectionName);
        }


        /// <summary>
        /// Convenience method for checking if config key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            return _current.Contains(key);
        }


        /// <summary>
        /// Convenience method for checking if config key exists.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string section, string key)
        {
            return _current.Contains(section, key);
        }


        /// <summary>
        /// The names of the sections in this config provider.
        /// </summary>
        public static List<string> Sections
        {
            get { return _current.Sections; }
        }


        /// <summary>
        /// The name of this config provider.
        /// </summary>
        public static string Name
        {
            get { return _current.Name; }
        }


        /// <summary>
        /// The full path to the source for this config provider.
        /// </summary>
        public static string SourcePath
        {
            get { return _current.SourcePath; }
        }


        /// <summary>
        /// Save the configuration.
        /// </summary>
        public static void Save()
        {
            _current.Save();
        }
    }
}
