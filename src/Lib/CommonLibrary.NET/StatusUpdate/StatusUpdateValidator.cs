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
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.StatusUpdater
{
    /// <summary>
    /// Validator for StatusUpdate
    /// </summary>
    public partial class StatusUpdateValidator : EntityValidator
    {
        /// <summary>
        /// Validation method for the entity.
        /// </summary>
        /// <param name="validationEvent">Validation event to use.</param>
        /// <returns>True if the validation succeeds.</returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results; 

            int initialErrorCount = results.Count;
            StatusUpdate entity = (StatusUpdate)target;
			Validation.IsStringLengthMatch(entity.Computer, false, false, true, -1, 30, results, "Computer" );
			Validation.IsStringLengthMatch(entity.ExecutionUser, false, true, true, 1, 30, results, "ExecutionUser" );
			Validation.IsDateWithinRange(entity.BusinessDate, false, false, DateTime.MinValue, DateTime.MaxValue, results, "BusinessDate" );
			Validation.IsStringLengthMatch(entity.BatchName, false, true, true, 1, 30, results, "BatchName" );
			Validation.IsNumericWithinRange(entity.BatchId, false, false, -1, -1, results, "BatchId");
			Validation.IsDateWithinRange(entity.BatchTime, false, false, DateTime.MinValue, DateTime.MaxValue, results, "BatchTime" );
			Validation.IsStringLengthMatch(entity.Task, false, true, true, 1, 50, results, "Task" );
			Validation.IsStringLengthMatch(entity.Status, false, true, true, 1, 30, results, "Status" );
			Validation.IsDateWithinRange(entity.StartTime, false, false, DateTime.MinValue, DateTime.MaxValue, results, "StartTime" );
			Validation.IsDateWithinRange(entity.EndTime, false, false, DateTime.MinValue, DateTime.MaxValue, results, "EndTime" );
			Validation.IsStringLengthMatch(entity.Ref, true, false, true, -1, 30, results, "Ref" );
			Validation.IsStringLengthMatch(entity.Comment, true, false, true, -1, 150, results, "Comment" );

            return initialErrorCount == results.Count;
        }
    }
}
