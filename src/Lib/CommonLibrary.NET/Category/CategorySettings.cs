
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

//using ComLib.Modules;


namespace ComLib.Categories
{
    /// <summary>
    /// Settings for the comments module.
    /// </summary>
    public class CategorySettings //: ModuleSettings
    {
        /// <summary>
        /// Whether or not the show the add/collapse image for the parent categories.
        /// </summary>
        public bool AddCollapseExpand { get; set; }


        /// <summary>
        /// Whether or not the show the expand / collapse all links
        /// </summary>
        public bool ShowExpandCollapseAll { get; set; }


        /// <summary>
        /// Whether or not the show the suggest category link
        /// </summary>
        public bool SuggestCategoryShowLink { get; set; }


        /// <summary>
        /// The text to display when showing suggest category.
        /// </summary>
        public string SuggestCategoryText { get; set; }


        /// <summary>
        /// Url for the suggest category link
        /// </summary>
        public string SuggestCategoryUrl { get; set; }


        /// <summary>
        /// Link action: Url , PostBack ,Javascript
        /// </summary>
        public string LinkAction { get; set; }


        /// <summary>
        /// Get/set the number of categories to auto expand.
        /// </summary>
        public int NumberOfCategoriesToAutoExpand { get; set; }


        /// <summary>
        /// Initialize the settings.
        /// </summary>
        public CategorySettings()
        {
            AddCollapseExpand = true;
            ShowExpandCollapseAll = true;
            SuggestCategoryShowLink = true;
            SuggestCategoryText = "not here?<br/>suggest a category";
            SuggestCategoryUrl = "~/ContactUs.aspx";
            LinkAction = "Url";
            NumberOfCategoriesToAutoExpand = 2;
        }
    }
}
