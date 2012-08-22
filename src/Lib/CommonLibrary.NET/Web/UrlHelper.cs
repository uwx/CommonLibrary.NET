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
using System.Web;
using ComLib;


namespace ComLib.Web
{
    /// <summary>
    /// Url helper to provide various utitlity methods.
    /// 
    /// 1. Provide valid url's for either local development maching running
    ///    a website from visual studio OR IIS.
    /// 
    /// 2. Get url's that have been rewritten.
    /// </summary>
    public class UrlHelper
    {
        private static bool _isRewritingEnabled = false;
        private static UrlMapper _urlMapper;


        #region Properties
        /// <summary>
        /// Get or set flag indicating if url-rewriting is enabled.
        /// </summary>        
        public static bool IsRewritingEnabled
        {
            get { return _isRewritingEnabled; }
            set
            {
                _isRewritingEnabled = value;
                if (_isRewritingEnabled && _urlMapper == null)
                {
                    throw new InvalidOperationException("Must provide url mapping if performing url-rewriting.");
                }
            }
        }


        /// <summary>
        /// Instance of the url mapper.
        /// </summary>
        public UrlMapper UrlMapperProvider
        {
            get { return _urlMapper; }
        }
        #endregion


        /// <summary>
        /// Gets the relative site url.
        /// </summary>
        /// <remarks>
        /// Use this method for getting relative site urls for resource files such as
        /// 1. Javascript
        /// 2. Images
        /// 3. Xml files
        /// </remarks>
        /// <param name="url">"~/Scripts/Javascript/UI.js"</param>
        /// <returns>
        /// For IIS PRODUCTION: /Scripts/Javascript/UI.js
        /// For VS  LOCAL DEV : /MyApp.WebSite/Scripts/Javascript/UI.js
        /// </returns>
        public static string GetRelativeSiteUrl(string url)
        {
            return GetRelativeSiteUrl(HttpContext.Current.Request.ApplicationPath, url);
        }


        /// <summary>
        /// Gets the relative site url for possibly mapped/rewritten urls.
        /// </summary>
        /// <remarks>
        /// Use this method for getting relative site urls for rewritten urls such as
        /// 1. Web pages ( .aspx .html .htm etc. )
        /// </remarks>
        /// <param name="url">"~/Modules/Home.aspx"</param>
        /// <returns>
        /// If there are mappings:
        /// For IIS PRODUCTION: /Home.aspx
        /// For VS  LOCAL DEV : /Workshops.WebSite/Home.aspx
        /// </returns>        
        public static string GetMappedRelativeSiteUrl(string url)
        {
            return GetMappedRelativeSiteUrl(HttpContext.Current.Request.ApplicationPath, url); ;
        }


        /// <summary>
        /// Configure the url-rewriting flag and set the url mapper.
        /// </summary>
        /// <param name="isUrlRewritingEnabled">True if URL rewriting is enabled.</param>
        /// <param name="urlMapper">Instance of URL mapper.</param>
        public static void ConfigureUrlRewriting(bool isUrlRewritingEnabled, UrlMapper urlMapper)
        {
            _isRewritingEnabled = isUrlRewritingEnabled;
            if (_isRewritingEnabled && urlMapper == null)
            {
                throw new InvalidOperationException("Must provide urlmapper if performing url-rewriting.");
            }

            _urlMapper = urlMapper;
        }


        /// <summary>
        /// Gets the root of the website. http: or https: plus the appropriate port.
        /// </summary>
        /// <returns>Root of the website.</returns>
        public static string GetSiteRoot()
        {
            string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string siteRoot = protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + HttpContext.Current.Request.ApplicationPath;
            return siteRoot;
        }


        /// <summary>
        /// Returns the name of the requested file.
        /// </summary>
        /// <param name="rawUrl">Raw URL.</param>
        /// <param name="includeExtension">Flag indicating if extension of the file should also be 
        /// included.</param>
        /// <returns>Requested file name.</returns>
        public static string GetRequestedFileName(string rawUrl, bool includeExtension)
        {
            string file = rawUrl.Substring(rawUrl.LastIndexOf("/") + 1);
            
            if (includeExtension)
                return file;

            return file.Substring(0, file.IndexOf("."));
        }


        /// <summary>
        /// These are exposed to the unit tests in CommonLibrary.Tests.
        /// </summary>
        /// <param name="applicationPath">Application path.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>Relative site URL.</returns>
        internal static string GetRelativeSiteUrl(string applicationPath, string url)
        {
            // Get proper application path ending with "/"
            if (!applicationPath.EndsWith("/"))
            {
                applicationPath += "/";
            }

            // Remove the "~/" from the url since we are using application path.
            if (!string.IsNullOrEmpty(url) && url.StartsWith("~/"))
            {
                url = url.Substring(2, url.Length - 2);
            }
            return applicationPath + url;
        }


        /// <summary>
        /// These are exposed to the unit tests in ServiceGoFor.CommonLibrary.Tests.
        /// </summary>
        /// <param name="applicationPath">Application path.</param>
        /// <param name="url">Request URL.</param>
        /// <returns>Mapped relative site URL.</returns>
        internal static string GetMappedRelativeSiteUrl(string applicationPath, string url)
        {
            // Check if rewriting enabled.
            if (!IsRewritingEnabled || _urlMapper == null) { return GetRelativeSiteUrl(applicationPath, url); }

            // Get the mapped url and return the valid relative site url.
            url = _urlMapper.GetUrl(url);
            return GetRelativeSiteUrl(applicationPath, url);
        }
    }



    /// <summary>
    /// Url mapper to assist in mapping physical urls to possible rewritten urls.
    /// </summary>
    public class UrlMapper
    {
        private IDictionary<string, string> _urlMappings;


        /// <summary>
        /// Url mapper with mappings provided as IDictionary.
        /// </summary>
        /// <example>
        ///           Physical Url        Rewritten url
        ///     key = "~/Classes.aspx"    value = "~/SearchClasses.aspx"
        /// </example>
        /// <param name="urlMappings">Mappings from physical url to logical urls.</param>
        public UrlMapper(IDictionary<string, string> urlMappings)
        {
            Init(urlMappings);
        }


        /// <summary>
        /// Get the real url.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns>Mapped url if mapping present, original url otherwise</returns>
        public string GetUrl(string url)
        {
            string urlLowerCase = url.ToLower();

            // Not mapped ? Return original
            if (_urlMappings.ContainsKey(urlLowerCase))
            {
                return _urlMappings[urlLowerCase];
            }

            // No mapping, return original
            return url;
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="urlMappings">mappings to rewritten urls.</param>
        private void Init(IDictionary<string, string> urlMappings)
        {
            Guard.IsNotNull(urlMappings, "Url rewrite mappings were not provided.");
            _urlMappings = new Dictionary<string, string>();

            // Store the mappings with the physical url in lowercase.
            IEnumerator<KeyValuePair<string, string>> enumerator = urlMappings.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key.ToLower();
                _urlMappings.Add(key, enumerator.Current.Value);
            }
        }
    }
}
