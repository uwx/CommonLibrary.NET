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
using System.Data;


namespace ComLib.Data
{
    /// <summary>
    /// Base class for RowMapping.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IRowMapper<TSource, TResult>
    {
        /// <summary>
        /// Whether or not the callback for after rows mapped is enabled.
        /// </summary>
        bool IsCallbackEnabledForAfterRowsMapped { get; set; }


        /// <summary>
        /// Maps all the rows in TSource to list objects of type T.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        IList<TResult> MapRows(TSource dataSource);
        

        /// <summary>
        /// Maps a specific row to an item of type TResult
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="rowId"></param>
        /// <returns></returns>
        TResult MapRow(TSource dataSource, int rowId);


        /// <summary>
        /// Call back for on after rows have been mapped.
        /// </summary>
        /// <param name="items"></param>
        void OnAfterRowsMapped(IList<TResult> items);
    }
}
