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
using System.Collections.Generic;
using System.Text;
using System;
using ComLib;


namespace ComLib.ValidationSupport
{

    /// <summary>
    /// This class provides validation utility methods.
    /// </summary>
    public class ValidationUtils
    {
        /// <summary>
        /// Check the parameter isError for validation condition.
        /// If it is not valid, adds the errmessage to the list of errors.
        /// </summary>
        /// <param name="isError">True if is a validation condition..</param>
        /// <param name="errors">List with errors.</param>
        /// <param name="message">Error message.</param>
        /// <returns>True if isError is false.</returns>
        public static bool Validate(bool isError, IList<string> errors, string message )
        {
            if (isError) { errors.Add(message); }
            return !isError;
        }


        /// <summary>
        /// Validates the bool condition and adds the string error
        /// to the error list if the condition is invalid.
        /// </summary>
        /// <param name="isError">Flag indicating if invalid.</param>
        /// <param name="errors">Error message collection</param>
        /// <param name="key">Key associated with the message.</param>
        /// <param name="message">The error message to add if isError is true.</param>
        /// <returns>True if isError is false, indicating no error.</returns>
        public static bool Validate(bool isError, IErrors errors, string key, string message)
        {
            if (isError) 
            {
                errors.Add(key, message);
            }
            return !isError;
        }



        /// <summary>
        /// Validates the bool condition and adds the string error
        /// to the error list if the condition is invalid.
        /// </summary>
        /// <param name="isError">Flag indicating if invalid.</param>
        /// <param name="errors">Error message collection</param>
        /// <param name="message">The error message to add if isError is true.</param>
        /// <returns>True if isError is false, indicating no error.</returns>
        public static bool Validate(bool isError, IErrors errors, string message)
        {
            if (isError)
            {
                errors.Add(string.Empty, message);
            }
            return !isError;
        }


        /// <summary>
        /// Transfers all the messages from the source to the validation results.
        /// </summary>
        /// <param name="messages">List of messages.</param>
        /// <param name="errors">Storage for errors.</param>
        public static void TransferMessages(IList<string> messages, IErrors errors)
        {
            foreach (string message in messages)
            {
                errors.Add(string.Empty, message);
            }
        }
        

        /// <summary>
        /// Valdiates all the validation rules in the list.
        /// </summary>
        /// <param name="validators">List of validations to validate</param>
        /// <param name="destinationResults">List of validation results to populate.
        /// This list is populated with the errors from the validation rules.</param>
        /// <returns>True if all rules passed, false otherwise.</returns>
        public static bool Validate(IList<IValidator> validators, IValidationResults destinationResults)
        {
            if (validators == null || validators.Count == 0)
                return true;

            // Get the the initial error count;		
            int initialErrorCount = destinationResults.Count;

            // Iterate through all the validation rules and validate them.
            foreach(IValidator validator in validators)
            {
                validator.Validate(destinationResults);
            }

            // Determine validity if errors were added to collection.
            return initialErrorCount == destinationResults.Count;
        }


        /// <summary>
        /// Validates the rule and returns a boolMessage.
        /// </summary>
        /// <param name="validator">Validator to use.</param>
        /// <returns>Validation result.</returns>
        public static BoolMessage Validate(IValidator validator)
        {
            IValidationResults results = validator.Validate() as IValidationResults;
            
            // Empty message if Successful.
            if (results.IsValid) return new BoolMessage(true, string.Empty);

            // Error            
            string multiLineError = results.Message();
            return new BoolMessage(false, multiLineError);
        }


        /// <summary>
        /// Validates the rule and returns a boolMessage.
        /// </summary>
        /// <param name="validator">Validator to use.</param>
        /// <param name="results">Validation results.</param>
        /// <returns>True if validation is successful.</returns>
        public static bool ValidateAndCollect(IValidator validator, IValidationResults results)
        {
            IValidationResults validationResults = validator.Validate(results);
            return validationResults.IsValid;
        }
    }
}