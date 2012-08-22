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

namespace ComLib
{
    /// <summary>
    /// Paged list to represent the page index, size, total records, and total pages
    /// in addition to the items.
    /// </summary>
    /// <typeparam name="T">Type of items to hold in the paged list.</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Empty/ null object.
        /// </summary>
        public readonly static PagedList<T> Empty = new PagedList<T>(1, 1, 0, null);


        /// <summary>
        /// Get/set the size of a page.
        /// </summary>
        public int PageSize { get; private set; }


        /// <summary>
        /// Get/set the page index.
        /// </summary>
        public int PageIndex { get; private set; }


        /// <summary>
        /// Get/set the total number of items in the list.
        /// </summary>
        public int TotalCount { get; private set; }

        
        /// <summary>
        /// Get/set the total number of pages.
        /// </summary>
        public int TotalPages { get; private set; }        
        

        /// <summary>
        /// Initialize w/ items, page index, size and total records.
        /// </summary>
        /// <param name="items">The items representing the list.</param>
        /// <param name="pageIndex">Page index to start at.</param>
        /// <param name="pageSize">Page size to start with.</param>
        /// <param name="totalRecords">Total number of records to start with.</param>
        public PagedList(int pageIndex, int pageSize, int totalRecords, IList<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalRecords;
            TotalPages = (int) Math.Ceiling(TotalCount / (double)PageSize);
            if (items != null && items.Count > 0)
            {
                this.AddRange(items);
            }
        }
    }
}
