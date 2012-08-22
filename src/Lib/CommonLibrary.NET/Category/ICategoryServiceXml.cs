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
using ComLib;

namespace ComLib.Categories
{
    /// <summary>
    /// Service for serializing the categories to xml
    /// </summary>
    public interface ICategoryServiceXml
    {
        /// <summary>
        /// Gets the root categories that do not have a parent category.
        /// </summary>
        /// <returns></returns>
        BoolMessage GetRootCategories();


        /// <summary>
        /// Gets the categories associated with the parentId supplied.
        /// </summary>
        /// <param name="parentId">Id of the partent</param>
        /// <returns></returns>
        BoolMessage GetCategoryiesByParent(string parentId);
    }
}
