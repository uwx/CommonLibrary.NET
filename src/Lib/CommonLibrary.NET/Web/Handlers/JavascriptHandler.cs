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
	public class JavascriptHandler : HandlerBase, IHttpHandler
	{
        private static IDictionary _config = new Dictionary<string, object>();
        private static string _sectionName = "WebHandlers.Js";
        

        /// <summary>
        /// Set the configuration settings as an IDictionary.
        /// </summary>
        /// <param name="sectionname">Section name</param>
        /// <param name="useSection">Whether or not to use the section name.</param>
        /// <param name="config">Config settings.</param>
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
            this.Extension = "js";
            this.ExtensionForContent = "text/javascript";
        }
	}
}
