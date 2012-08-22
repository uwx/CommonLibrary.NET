
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
using System.Data;
using System.Collections.Generic;
using ComLib.Entities;


namespace ComLib.Categories
{

    /// <summary>
    /// Interface for the Category data access object.
    /// </summary>
    public interface ICategoryDao : IRepository<Category>
    {
        /// <summary>
        /// Get the root categories.
        /// </summary>
        /// <returns></returns>
        IList<Category> GetRootCategories();


        /// <summary>
        /// Get the root categories directly from the datasource without using any cache.
        /// </summary>
        /// <returns></returns>
        IList<Category> GetRootCategoriesNonCache();


        /// <summary>
        /// Get the categories by the parent id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        IList<Category> GetByParentId(int categoryId);


        /// <summary>
        /// Get the category lookup component.
        /// </summary>
        /// <returns></returns>
        CategoryLookUp GetLookUp();
    }
}
