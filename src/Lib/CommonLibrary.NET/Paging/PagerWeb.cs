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
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;



namespace ComLib.Paging
{

    /// <summary>
    /// Pager url mode builder interface.
    /// </summary>
    public interface IPagerBuilderWeb
    {
        /// <summary>
        /// Builds the entire html for the specified page index.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        /// <returns>Created html.</returns>
        string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder);


        /// <summary>
        /// Builds the entire html for the specified page index.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="settings">The settings for the pager</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        /// <returns>Created html.</returns>
        string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder);


        /// <summary>
        /// Build the entire html for the pager.
        /// </summary>
        /// <param name="pager">Instance of pager.</param>
        /// <param name="settings">Pager settings.</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        /// <returns>Created html.</returns>
        string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder);
    }



    /// <summary>
    /// Buider class that builds the pager in Url mode.
    /// </summary>
    public class PagerBuilderWeb : IPagerBuilderWeb
    {
        private static IPagerBuilderWeb _instance;
        private static readonly object _syncRoot = new object();


        /// <summary>
        /// Get singleton instance.
        /// </summary>
        /// <returns>Returns the single instance of this class.</returns>
        public static IPagerBuilderWeb Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PagerBuilderWeb();
                        }
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Builds the entire html for the specified page index / total pages.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="urlBuilder">The URL builder.</param>
        /// <returns>Created html.</returns>
        public string Build(int pageIndex, int totalPages, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, PagerSettings.Default);
            return Build(pager, PagerSettings.Default, urlBuilder);
        }


        /// <summary>
        /// Builds the entire html for the specified page index / total pages.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="totalPages">The total pages.</param>
        /// <param name="settings">The settings for the pager.</param>
        /// <param name="urlBuilder">The URL builder.</param>
        /// <returns>Created html.</returns>
        public string Build(int pageIndex, int totalPages, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pager = Pager.Get(pageIndex, totalPages, settings);
            return Build(pager, pager.Settings, urlBuilder);
        }


        /// <summary>
        /// Build the entire html for the pager.
        /// </summary>
        /// <param name="pager">Instance of pager.</param>
        /// <param name="settings">Pager settings.</param>
        /// <param name="urlBuilder">The lamda to build the url for a specific page</param>
        /// <returns>Created html.</returns>
        public string Build(Pager pager, PagerSettings settings, Func<int, string> urlBuilder)
        {
            Pager pagerData = pager;

            // Get reference to the default or custom url builder for this pager.
            StringBuilder buffer = new StringBuilder();
            string cssClass = string.Empty;
            string urlParams = string.Empty;
            string url = string.Empty;

            // Build the previous page link.
            if (pagerData.CanShowPrevious)
            {
                // Previous
                url = urlBuilder(pagerData.CurrentPage - 1);
                buffer.Append("<a class=\"" + settings.CssClass + "\" href=\"" + url + "\">" + "&#171;" + "</a>");
            }

            // Build the starting page link.            
            if (pagerData.CanShowFirst)
            {
                // First
                url = urlBuilder(1);
                buffer.Append("<a class=\"" + settings.CssClass + "\" href=\"" + url + "\">" + 1 + "</a>");

                // This is to avoid putting ".." between 1 and 2 for example.
                // If 1 is the starting page and we want to display 2 as starting page.
                if (pagerData.CanShowPrevious)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
            }

            // Each page number.
            for (int ndx = pagerData.StartingPage; ndx <= pagerData.EndingPage; ndx++)
            {
                cssClass = (ndx == pagerData.CurrentPage) ? settings.CssCurrentPage : string.Empty;
                url = urlBuilder(ndx);

                // Build page number link. <a href="<%=Url %>" class="<%=cssClass %>" ><%=ndx %></a>                
                buffer.Append("<a class=\"" + cssClass + "\" href=\"" + url + "\">" + ndx + "</a>");
            }

            // Build the  ending page link.
            if (pagerData.CanShowLast)
            {
                url = urlBuilder(pagerData.TotalPages);

                // This is to avoid putting ".." between 7 and 8 for example.
                // If 7 is the ending page and we want to display 8 as total pages.
                if (pagerData.CanShowNext)
                {
                    buffer.Append("&nbsp;&nbsp;&nbsp;");
                }
                buffer.Append("<a class=\"" + settings.CssClass + "\" href=\"" + url + "\">" + pagerData.TotalPages + "</a>");
            }

            // Build the next page link.
            if (pagerData.CanShowNext)
            {
                // Previous
                url = urlBuilder(pagerData.CurrentPage + 1);
                buffer.Append("<a class=\"" + settings.CssClass + "\" href=\"" + url + "\">" + "&#187;" + "</a>");
            }
            string content = buffer.ToString();
            return content;
        }
    }
}