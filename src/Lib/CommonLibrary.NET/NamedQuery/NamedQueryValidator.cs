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
using ComLib.ValidationSupport;
using ComLib.Entities;



namespace ComLib.NamedQueries
{
    /// <summary>
    /// Validator for NamedQuery
    /// </summary>
    public partial class NamedQueryValidator : EntityValidator
    {       

        /// <summary>
        /// Validation method for the entity.
        /// </summary>
        /// <param name="validationEvent">Validator.</param>
        /// <returns>True if the validation was successful.</returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results; 

            int initialErrorCount = results.Count;
            NamedQuery entity = (NamedQuery)target;
			Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 100, results, "Name" );
			Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 200, results, "Description" );
			Validation.IsStringLengthMatch(entity.Sql, false, true, true, 1, 500, results, "Sql" );
			Validation.IsStringLengthMatch(entity.Parameters, true, false, true, -1, 250, results, "Parameters" );
			Validation.IsNumericWithinRange(entity.OrderId, false, false, -1, -1, results, "OrderId");
			Validation.IsStringLengthMatch(entity.ItemType, true, false, true, -1, 50, results, "ItemType" );
			Validation.IsStringLengthMatch(entity.Roles, true, false, true, -1, 50, results, "Roles" );

            return initialErrorCount == results.Count;
        }
    }
}
