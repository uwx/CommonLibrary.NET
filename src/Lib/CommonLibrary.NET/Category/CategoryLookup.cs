
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
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ComLib.Patterns;


namespace ComLib.Categories
{

    /// <summary>
    /// Category look up class.
    /// This contains all the categories / subcategories available.
    /// </summary>
    public class CategoryLookUp : CompositeLookup<Category>
    {
        /// <summary>
        /// Initialzie the lookup
        /// </summary>
        /// <param name="allCategories"></param>
        public CategoryLookUp(IList<Category> allCategories) : base(allCategories)
        {
        }
    }



    internal class CategoryKey
    {
        /// <summary>
        /// Build key for category.
        /// 
        /// If root, just the the category name.
        ///     e.g. "Art"
        /// 
        /// If sub-category, parent/sub-cat name
        ///     e.g. "Art,Sculpture"
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static string BuildKey(Category category)
        {
            // Root category
            if (category.ParentId == CategoryConstants.RootCategoryParentCategoryId)
            {
                return category.Name.ToLower();
            }

            return category.ParentName.ToLower() + "," + category.Name.ToLower();
        }


        /// <summary>
        /// Build category key.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="subCategoryName"></param>
        /// <returns></returns>
        public static string BuildKey(string categoryName, string subCategoryName)
        {
            return categoryName.Trim().ToLower() + "," + subCategoryName.Trim().ToLower();
        }
    }
}
