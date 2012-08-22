
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
using ComLib.Entities;
using ComLib.Patterns;


namespace ComLib.Categories
{
    
    /// <summary>
    /// Summary description for Category
    /// </summary>
    public class Category : CompositeWithIdAndName<Category>, INodeWithIds
    {
        /// <summary>
        /// Default Initialization
        /// </summary>
        public Category() 
        {
            Id = CategoryConstants.NA;
            ParentId = CategoryConstants.NA;
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="addedDate"></param>
        /// <param name="addedBy"></param>
        /// <param name="parentCategoryId"></param>
        /// <param name="categoryTitle"></param>
        /// <param name="categoryDescription"></param>
        /// <param name="imageUrl"></param>
        /// <param name="parentTitle"></param>
        public Category(int categoryId, DateTime addedDate, string addedBy, 
            int parentCategoryId, string categoryTitle, string categoryDescription, 
            string imageUrl, string parentTitle)
        {
            this.Id = categoryId;
            this.Name = categoryTitle;
            this.Description = categoryDescription;
            this.ParentId = parentCategoryId;
            this.ImageUrl = imageUrl;
            this.ParentName = parentTitle;
            this.Url = string.Empty;
        }


        /// <summary>
        /// Description of category
        /// </summary>
        public string Description { get; set; }

                
        /// <summary>
        /// Name of parent category
        /// </summary>
        public string ParentName { get; set; }


        /// <summary>
        /// Used for ordering.
        /// </summary>
        public int OrderNum { get; set; }

        
        /// <summary>
        /// Image Url of the image associated with this category.
        /// </summary>
        public string ImageUrl { get; set; }


        /// <summary>
        /// Applicable url.
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// Group
        /// </summary>
        public string Group { get; set; }

    }
}
