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
    /// Interface for any calculator to process the page action.
    /// </summary>
    public interface IPagerCalculator
    {
        /// <summary>
        /// Calculate pages
        /// </summary>
        /// <param name="pager">Instance of pager.</param>
        /// <param name="settings">Pager settings.</param>
        void Calculate(Pager pager, PagerSettings settings);
    }


    /// <summary>
    /// Default implementation of a pager.
    /// This uses a calculation scheme where the rules are the following :
    /// 
    /// 1. All possible links include ( first, previous, page1, page2, pageX, next, last )
    /// 2. The number of pages in the middle ( page1, page2 ) can be configured.
    /// 3. If current page = 1 ( links first and previous are not applicable )
    /// 4. If current page = last ( links last and next are not applicable )
    /// 5. The number of pages in the middle is cycled.
    /// </summary>
    public class PagerCalculator : IPagerCalculator
    {
        #region IPagerCalculator Members
        /// <summary>
        /// Calculate the pager properties such as starting, ending, next, previous pages.
        /// </summary>
        /// <param name="pagerData">Pager data.</param>
        /// <param name="pagerSettings">Pager settings.</param>
        public void Calculate(Pager pagerData, PagerSettings pagerSettings)
        {
            // Check bounds.
            if (pagerData.CurrentPage < 0) pagerData.CurrentPage = 1;
            if (pagerData.CurrentPage > pagerData.TotalPages) pagerData.CurrentPage = 1;

            int currentPage = pagerData.CurrentPage;

            // Calculate the starting and ending page.
            pagerData.StartingPage = GetStartingPage(pagerData, pagerSettings);
            pagerData.EndingPage = GetEndingPage(pagerData, pagerSettings);

            // Calculate next page.
            if (currentPage + 1 <= pagerData.TotalPages)
                pagerData.NextPage = currentPage + 1;
            else
                pagerData.NextPage = currentPage;

            // Calculate previous page number.
            if (currentPage - 1 >= 1)
                pagerData.PreviousPage = currentPage - 1;
            else
                pagerData.PreviousPage = currentPage;
        }
        #endregion


        #region Private methods
        private static int GetStartingPage(Pager pagerData, PagerSettings settings)
        {
            // Total pages = 5 = number of pages to display.. so starting page is 1;
            if (pagerData.CurrentPage <= settings.NumberPagesToDisplay)
            {
                return 1;
            }
            int range = GetRange(pagerData.CurrentPage, settings.NumberPagesToDisplay);
            int totalRanges = GetTotalRanges(pagerData.TotalPages, settings.NumberPagesToDisplay);

            // Current page = 17
            // Total pages = 18
            // Pages to show = 5
            // Range = 4
            // Starting page = 14, 15, 16, 17, 18
            if (range == totalRanges)
            {
                return (pagerData.TotalPages - settings.NumberPagesToDisplay) + 1;
            }
            // current page = 7
            // Total pages = 18
            // Pages to show = 5
            // Range = 2
            // starting page = 6
            range--;
            return (range * settings.NumberPagesToDisplay) + 1;
        }


        private static int GetEndingPage(Pager pagerData, PagerSettings settings)
        {
            // Total pages = 5 = Number to display 1 - 5
            if (pagerData.TotalPages <= settings.NumberPagesToDisplay)
            {
                return pagerData.TotalPages;
            }

            int range = GetRange(pagerData.CurrentPage, settings.NumberPagesToDisplay);
            int totalRanges = GetTotalRanges(pagerData.TotalPages, settings.NumberPagesToDisplay);

            if (range == totalRanges)
            {
                return pagerData.TotalPages;
            }

            // if page = 8, ending page is 10.
            return range * settings.NumberPagesToDisplay;
        }


        /// <summary>
        /// Get the total number of ranges.
        /// </summary>
        /// <param name="totalPages"></param>
        /// <param name="numberPagesToDisplay"></param>
        /// <returns></returns>
        private static int GetTotalRanges(int totalPages, int numberPagesToDisplay)
        {
            return GetRange(totalPages, numberPagesToDisplay);
        }


        /// <summary>
        /// Get the total ranges.
        /// e.g.
        /// Total pages = 18
        /// 
        /// 1  - 5  = Range 1
        /// 6  - 10 = Range 2
        /// 11 - 15 = Range 3
        /// 14 - 18 = Range 4
        /// </summary>
        /// <param name="currentPage">Current/Selected page.</param>
        /// <param name="numberPagesToDisplay">Number of pages to display in the middle.
        /// This does not include the first page link and last page link.</param>
        /// <returns></returns>
        private static int GetRange(int currentPage, int numberPagesToDisplay)
        {
            // First range.
            if (currentPage <= numberPagesToDisplay) return 1;

            // Find range.
            // Current Page     = 9
            // Pages to display = 5
            // Range            = 2
            // 9 / 5 = 1
            // 9 mod 5 = 4
            int range = currentPage / numberPagesToDisplay;
            int remainingPages = currentPage % numberPagesToDisplay;

            // If there is any left over, then increment the range.            
            // this means range 2
            if (remainingPages > 0)
            {
                range++;
            }

            return range;
        }
        #endregion
    }
}