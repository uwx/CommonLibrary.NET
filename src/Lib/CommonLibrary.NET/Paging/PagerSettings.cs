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
    /// Pager settings.
    /// </summary>
    public class PagerSettings
    {
        /// <summary>
        /// Default settings
        /// </summary>
        public static readonly PagerSettings Default = new PagerSettings(7, "current", "");


        /// <summary>
        /// Default construction
        /// </summary>
        public PagerSettings() { }


        /// <summary>
        /// Initialize fields.
        /// </summary>
        /// <param name="numberPagesToDisplay">Number of paged to display.</param>
        /// <param name="cssClassForCurrentPage">Css for current page.</param>
        /// <param name="cssClassForPage">Css for pages.</param>
        public PagerSettings(int numberPagesToDisplay, string cssClassForCurrentPage, string cssClassForPage)
        {
            NumberPagesToDisplay = numberPagesToDisplay;
            CssCurrentPage = cssClassForCurrentPage;
            CssClass = cssClassForPage;
        }        

        
        /// <summary>
        /// How many pages to display in a row at once.
        /// </summary>
        public int NumberPagesToDisplay = 5;


        /// <summary>
        /// Name of css class used for currently displayed page.
        /// </summary>
        public string CssCurrentPage = string.Empty;

        
        /// <summary>
        /// Name of css class used for showing normal non-current pages.
        /// </summary>
        public string CssClass = string.Empty;
    }
}