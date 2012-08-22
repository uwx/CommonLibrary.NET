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
using System.Globalization;


namespace ComLib.Locale
{
    /// <summary>
    /// Simple interface for a localized resource manager.
    /// </summary>
    public interface ILocalizationResourceProvider
    {
        /// <summary>
        /// Get / set the localized resource string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; set; }


        /// <summary>
        /// Get the localized string given the key name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetString(string name);


        /// <summary>
        /// Get the localized string given the key name, if it doesn't exist, use the default value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        string GetString(string name, string defaultValue);


        /// <summary>
        /// Get the localized string given the key name and culture info.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        string GetString(string name, CultureInfo cultureInfo);


        /// <summary>
        /// Get the localized string given the key name and culture info.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        string GetString(string name, CultureInfo cultureInfo, string defaultName);
    }  
}
