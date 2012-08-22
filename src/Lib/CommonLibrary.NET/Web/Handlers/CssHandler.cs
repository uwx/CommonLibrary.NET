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
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;

using ComLib.Web;
using ComLib.Extensions;


namespace ComLib.Web.HttpHandlers
{
	/// <summary>
	/// Removes whitespace in all stylesheets added to the 
	/// header of the HTML document in site.master. 
	/// </summary>
    public class CssHandler : HandlerBase, IHttpHandler
	{
        private static IDictionary<string, string> _replaceChars = new Dictionary<string, string>();
        private static IDictionary _config = new Dictionary<string, object>();
        private static string _sectionName = "WebHandlers.Css";


        /// <summary>
        /// Static initialization.
        /// </summary>
        static CssHandler()
        {
            // All the replacement chars for removing whitespace from css files.
            _replaceChars["  "] = " ";            
            _replaceChars[Environment.NewLine] = String.Empty;
            _replaceChars["\t"] = string.Empty;
            _replaceChars[" {"] = "{";
            _replaceChars[" :"] = ":";
            _replaceChars[": "] = ":";
            _replaceChars[", "] = ",";
            _replaceChars["; "] = ";";
            _replaceChars[";}"] = "}";
            _replaceChars[@"?"] = string.Empty;            
        }


        /// <summary>
        /// Set the configuration settings as an IDictionary.
        /// </summary>
        /// <param name="sectionname">Section name</param>
        /// <param name="useSection">Whether or not to use the section name.</param>
        /// <param name="config">Configuration settings.</param>
        public static void Init(IDictionary config, string sectionname, bool useSection)
        {
            _config = config;
            if (useSection)
                _sectionName = sectionname;
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        protected override void Init()
        {
            base.Init();
            this.Config = _config;
            this.ConfigSectionName = _sectionName;
            this.Extension = "css";
            this.ExtensionForContent = "text/css";
        }


        /// <summary>
        /// Get the css file.
        /// </summary>
        /// <param name="fileName">CSS file.</param>
        /// <returns>CSS file.</returns>
        protected override string GetFile(string fileName)
        {
            string contents = base.GetFile(fileName);
            if (Config.GetOrDefault<bool>(ConfigSectionName, "RemoveWhiteSpace", false))
            {
                contents = RemoveWhitespace(contents);
            }
            return contents;
        }


		/// <summary>
		/// Removes whitespace from .css file.
		/// </summary>
        private string RemoveWhitespace(string body)
		{
            // Replace all the replacement chars.
            foreach (var entry in _replaceChars)
            {
                body = body.Replace(entry.Key, entry.Value);
            }

			// Remove comments
			body = Regex.Replace(body, @"/\*[\d\D]*?\*/", string.Empty);

			return body;
		}
	}
}
