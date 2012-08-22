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


namespace ComLib.Account
{
    /// <summary>
    /// Validator for User
    /// </summary>
    public partial class UserValidator : EntityValidator
    {
        /// <summary>
        /// Validation method for the entity.
        /// </summary>
        /// <param name="validationEvent">The event containing the object to validate.</param>
        /// <returns>True if the validation is successful.</returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results; 

            int initialErrorCount = results.Count;
            User entity = (User)target;
            Validation.IsStringLengthMatch(entity.UserName, false, true, true, 3, 20, results, "UserName");
            Validation.IsStringLengthMatch(entity.UserNameLowered, false, true, true, 3, 20, results, "UserNameLowered");
            Validation.IsEmail(entity.Email, false, results, "Email");
            Validation.IsStringLengthMatch(entity.Email, false, true, true, 6, 50, results, "Email");
            Validation.IsStringLengthMatch(entity.Password, false, true, true, 5, 100, results, "Password");
            Validation.IsStringLengthMatch(entity.Roles, true, false, false, -1, 50, results, "Roles");
            Validation.IsStringLengthMatch(entity.MobilePhone, true, true, true, 10, 20, results, "MobilePhone");
            Validation.IsStringLengthMatch(entity.SecurityQuestion, true, false, true, -1, 150, results, "SecurityQuestion");
            Validation.IsStringLengthMatch(entity.SecurityAnswer, true, false, true, -1, 150, results, "SecurityAnswer");
            Validation.IsStringLengthMatch(entity.Comment, true, false, true, -1, 50, results, "Comment");
            Validation.IsStringLengthMatch(entity.LockOutReason, true, false, true, -1, 50, results, "LockOutReason");
            return initialErrorCount == results.Count;
        }
    }
}
