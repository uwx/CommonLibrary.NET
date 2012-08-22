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

using ComLib;
using ComLib.Configuration;
using ComLib.IO;


namespace ComLib.Entities
{
    /// <summary>
    /// Localization resource provider for the entity.
    /// </summary>
    public class EntityResources : IEntityResources
    {
        /// <summary>
        /// Value of configuration source for strings.
        /// </summary>
        protected IConfigSource _resourceStrings = null;


        /// <summary>
        /// Entity name of localization.
        /// </summary>
        protected string _entityName = string.Empty;

        /// <summary>
        /// Default initialization.
        /// </summary>
        public EntityResources()
        {
            // Multi-line enabled ini document.
            _resourceStrings = new IniDocument();
        }


        /// <summary>
        /// Initialize w/ resource file.
        /// </summary>
        /// <param name="filepath"></param>
        public EntityResources(string filepath)
        {
            _resourceStrings = new IniDocument(filepath, true);
        }


        /// <summary>
        /// Initialize w/ config source containing the resource strings.
        /// </summary>
        /// <param name="configSource"></param>
        public EntityResources(IConfigSource configSource)
        {
            _resourceStrings = configSource;
        }


        /// <summary>
        /// Get the name of the entity.
        /// </summary>
        public virtual string EntityName { get { return _entityName; } }


        #region ILocalizationResourceProvider Members
        /// <summary>
        /// Get / set the resource string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get { return GetString(key, key); }
            set
            {
                _resourceStrings[key] = value;
            }
        }


        /// <summary>
        /// Get the resource string associated with the key.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetString(string name)
        {
            return GetString(name, name);
        }


        /// <summary>
        /// Get the resource string associated with the key and culture info.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public string GetString(string name, System.Globalization.CultureInfo cultureInfo)
        {
            return GetString(name, cultureInfo, name);
        }


        /// <summary>
        /// Check if resource string exists with the specified name.
        /// If exists, return resource string, otherwise return string value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string name, string defaultValue)
        {
            return GetString(name, null, defaultValue);
        }


        /// <summary>
        /// Check if resource string exists with the specified name.
        /// If exists, return resource string, otherwise return string value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public virtual string GetString(string name, System.Globalization.CultureInfo cultureInfo, string defaultValue)
        {
            if (_resourceStrings.Contains(name))
                return _resourceStrings.Get<string>(name);

            return defaultValue;
        }

        #endregion
    }
}
