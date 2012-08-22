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
using ComLib;


namespace ComLib.ValidationSupport
{
   
    /// <summary>
    /// Base class for any validator.
    /// </summary>
    public class Validator : IValidator
    {
        /// <summary>
        /// Validator to use.
        /// </summary>
        public static readonly IValidator Empty = new Validator();


        /// <summary>
        /// Message to use as description for an error.
        /// </summary>
        protected string _message;


        /// <summary>
        /// The target object to validate
        /// </summary>
        protected object _target;


        /// <summary>
        /// The results of the validation.
        /// </summary>
        protected IValidationResults _lastValidationResults;


        /// <summary>
        /// A lamda to call for validation.
        /// </summary>
        protected Func<ValidationEvent, bool> _validatorLamda;


        /// <summary>
        /// The number of errors in the error collection before validation runs.
        /// </summary>
        protected int _initialErrorCount;


        /// <summary>
        /// Create validation event flag.
        /// </summary>
        protected bool _creatValidationEvent;


        /// <summary>
        /// Default construction
        /// </summary>
        public Validator()
        {
        }


        /// <summary>
        /// Create using the lambda that does the actual validation.
        /// </summary>
        /// <param name="validator">Validator function.</param>
        public Validator(Func<ValidationEvent, bool> validator)
        {
            _validatorLamda = validator;
        }


        #region IValidator Members
        /// <summary>
        /// The object to validate.
        /// </summary>
        public virtual object Target
        {
            get { return _target; }
            set { _target = value; }
        }


        /// <summary>
        /// Message to use for the description of an error.
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }


        /// <summary>
        /// Simple true/false to indicate if validation passed.
        /// </summary>
        public bool IsValid
        {
            get { return Validate().IsValid; }
        }


        /// <summary>
        /// The results of the last validation.
        /// </summary>
        public IValidationResults Results
        {
            get { return _lastValidationResults; }
        }


        /// <summary>
        /// Clears the validation results.
        /// </summary>
        public virtual void Clear()
        {
            _lastValidationResults = new ValidationResults();
        }

        /// <summary>
        /// Validate data using data provided during initialization/construction.
        /// </summary>
        /// <returns>Validation results.</returns>
        public virtual IValidationResults Validate()
        {
            _lastValidationResults = new ValidationResults() as IValidationResults;
            Validate(Target, _lastValidationResults);
            return _lastValidationResults;
        }


        /// <summary>
        /// Validate using the object provided.
        /// </summary>
        /// <param name="target">Validation target.</param>
        /// <returns>Validation results.</returns>
        public virtual IValidationResults ValidateTarget(object target)
        {
            _lastValidationResults = new ValidationResults() as IValidationResults;
            Validate(new ValidationEvent(target, _lastValidationResults));
            return _lastValidationResults;
        }


        /// <summary>
        /// Validate using the results collection provided.
        /// </summary>
        /// <param name="results">Validation results.</param>
        /// <returns>Validation results.</returns>
        public virtual IValidationResults Validate(IValidationResults results)
        {
            Validate(new ValidationEvent(Target, results));
            return results;
        }


        /// <summary>
        /// Validate using the object provided, and add errors to the results list provided.
        /// </summary>
        /// <param name="target">Validation target.</param>
        /// <param name="results">Validation results.</param>
        /// <returns>Validation result.</returns>
        public bool Validate(object target, IValidationResults results)
        {
            return Validate(new ValidationEvent(target, results));
        }
        #endregion


        /// <summary>
        /// This Method will call the ValidateInternal method of this validator.
        /// </summary>
        /// <remarks>
        /// The reason that the ValidateInternal method is NOT called directly by the
        /// other Validate methods is because the CodeGenerator generates the Validation
        /// code inside of the ValidateInternal method.
        /// If a client wants to override the validation while sill leveraging the autogenerated
        /// validation code, it can be done by overrideing this method and calling the
        /// ValidateInternal method.
        /// This allows a lot of flexibility for codegeneration.
        /// </remarks>
        /// <param name="validationEvent">Validation event.</param>
        /// <returns>Validation result.</returns>
        public virtual bool Validate(ValidationEvent validationEvent)
        {
            return ValidateInternal(validationEvent);
        }


        /// <summary>
        /// Implement this method.
        /// </summary>
        /// <param name="validationEvent">Validation event.</param>
        /// <returns>Validation result.</returns>
        protected virtual bool ValidateInternal(ValidationEvent validationEvent)
        {
            if (_validatorLamda != null)
                return _validatorLamda(validationEvent);

            return true;
        }


        /// <summary>
        /// Add a new result to the list of errors.
        /// </summary>
        /// <param name="results">Validation results.</param>
        /// <param name="key">Result key.</param>
        /// <param name="message">Result message.</param>
        protected void AddResult(IValidationResults results, string key, string message)
        {
            results.Add(key, message);
        }
    }
}
