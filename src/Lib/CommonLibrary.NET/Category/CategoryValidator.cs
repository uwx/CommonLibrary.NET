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
using ComLib.ValidationSupport;


namespace ComLib.Categories
{
    /// <summary> 
    /// Category validator. 
    /// </summary> 
    public class CategoryValidator : Validator
    {
        private Category _category;


        /// <summary>
        /// Initializer.
        /// </summary>
        /// <param name="category"></param>
        public CategoryValidator(Category category)
        {
            _category = category;
        }


        #region IValidator<Category> Members
        /// <summary> 
        /// Validate the category. 
        /// </summary> 
        /// <param name="validationEvent"></param>
        /// <returns></returns> 
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results;
            bool useTarget = validationEvent.Target != null;
            
            int initialErrorCount = results.Count;
            Category category = useTarget ? (Category)target : _category;
            ValidationUtils.Validate(string.IsNullOrEmpty(category.Name), results, "Title", "Category title can not be empty");
            return initialErrorCount == results.Count;
        }
        #endregion
    }
}
