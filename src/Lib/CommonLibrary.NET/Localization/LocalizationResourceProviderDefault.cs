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

namespace ComLib.Locale
{
    /// <summary>
    /// Default localization manager.
    /// This should only be used as the default provider.
    /// </summary>
    public class LocalizationResourceProviderDefault : ILocalizationResourceProvider
    {
        #region Singleton Instance provider
        /// <summary>
        /// Used as singleton.
        /// </summary>
        private static readonly ILocalizationResourceProvider _instance = new LocalizationResourceProviderDefault();

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static ILocalizationResourceProvider Instance
        {
            get { return _instance; }
        }
        #endregion


        private IDictionary<string, string> _resourceStrings = new Dictionary<string, string>();


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
            if (_resourceStrings.ContainsKey(name))
                return _resourceStrings[name];

            return defaultValue;
        }


        /// <summary>
        /// Check if resource string exists with the specified name.
        /// If exists, return resource string, otherwise return string value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetString(string name, System.Globalization.CultureInfo cultureInfo, string defaultValue)
        {
            if (_resourceStrings.ContainsKey(name))
                return _resourceStrings[name];

            return defaultValue;
        }

        #endregion
    }
}
