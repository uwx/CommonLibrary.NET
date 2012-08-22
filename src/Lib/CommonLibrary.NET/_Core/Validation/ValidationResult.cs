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


namespace ComLib
{
    /// <summary>
    /// Encapsulate all the various inputs for performing validation on an object.
    /// </summary>
    public class ValidationEvent
    {
        /// <summary>
        /// The object to validate.
        /// </summary>
        public object Target;


        /// <summary>
        /// The results to store validation errors.
        /// </summary>
        public IValidationResults Results;


        /// <summary>
        /// Other contextual data that could be supplied.
        /// </summary>
        public object Context;


        /// <summary>
        /// Return the object to validate.
        /// </summary>
        /// <typeparam name="T">Type of object to return.</typeparam>
        /// <returns>Object to validate.</returns>
        public T TargetT<T>()
        {
            if (Target == null)
                return default(T);

            return (T)Target;
        }

        /// <summary>
        /// Initialize data.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="results">Validation results.</param>
        /// <param name="context">Context.</param>
        public ValidationEvent(object target, IValidationResults results, object context)
        {
            Target = target;
            Results = results;
            Context = context;
        }


        /// <summary>
        /// Initialize data.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="results">Validation results.</param>
        public ValidationEvent(object target, IValidationResults results)
        {
            Target = target;
            Results = results;
        }
    }



    /// <summary>
    /// Interface for validation results.
    /// </summary>
    public interface IValidationResults : IErrors
    {
        /// <summary>
        /// Returns the validation results.
        /// </summary>
        bool IsValid { get; }
    }



    /// <summary> 
    /// Stores validation results. 
    /// </summary> 
    /// <remarks>NOTE : Errors could be a read-only collection. 
    /// </remarks> 
    public class ValidationResults : Errors, IValidationResults
    {
        /// <summary>
        /// Null object.
        /// </summary>
        public static readonly ValidationResults Empty = new ValidationResults();


        /// <summary> 
        /// Passed validation ? 
        /// </summary> 
        public bool IsValid
        {
            get { return base.Count == 0; } 
        }
    }



    /// <summary>
    /// Interface for a validator that does not store any stateful instance data.
    /// This allows for the entire validation to be done in one method.
    /// </summary>
    public interface IValidatorNonStateful
    {
        /// <summary>
        /// Validates <paramref name="target"/> and returns a new <see cref="ValidationResults"/> with
        /// all the <see cref="ValidationResults"/> entries that represent the validation failures.
        /// </summary>
        /// <param name="target">The object to validate.</param>
        /// <returns>The results of the validation.</returns>
        IValidationResults ValidateTarget(object target);


        /// <summary>
        /// Validates <paramref name="target"/> and adds <see cref="ValidationResults"/> entires 
        /// representing failures to the supplied <paramref name="results"/>.
        /// </summary>
        /// <param name="target">The object to validate.</param>
        /// <param name="results">The <see cref="ValidationResults"/> where the validation failures
        /// should be collected.</param>
        bool Validate(object target, IValidationResults results);


        /// <summary>
        /// Validate using the supplied validation event that encapsulates both the target and results.
        /// </summary>
        /// <param name="validationEvent">Validation event.</param>
        /// <returns>Validation result.</returns>
        bool Validate(ValidationEvent validationEvent);
    }



    /// <summary>
    /// Interface for a class that can serve as a validator.
    /// This interface is stateful which means it stores the last validation result,
    /// the object being validated.
    /// </summary>
    public interface IValidatorStateful
    {
        /// <summary>
        /// The object to validate.
        /// </summary>
        object Target { get; set; }


        /// <summary>
        /// Message to use for a validation failure.
        /// </summary>
        string Message { get; }


        /// <summary>
        /// method to determine if validation has passed
        /// or failed for the object(s) being validated.
        /// </summary>
        /// <returns>True if the validation is passed.</returns>
        bool IsValid { get; }


        /// <summary>
        /// The last validation results.
        /// </summary>
        IValidationResults Results { get; }


        /// <summary>
        /// Performs validation on data provided during initialization/construction.
        /// </summary>
        /// <returns>Validation results.</returns>
        IValidationResults Validate();


        /// <summary>
        /// Validate using the results collection provided.
        /// </summary>
        /// <param name="results">Validation results.</param>
        /// <returns>Validation results.</returns>
        IValidationResults Validate(IValidationResults results);


        /// <summary>
        /// Clear the results.
        /// </summary>
        void Clear();
    }    



    /// <summary>
    /// Interface for a validator that supports both stateful and non-stateful
    /// validation.
    /// </summary>
    public interface IValidator : IValidatorStateful, IValidatorNonStateful
    {
    }


    /// <summary>
    /// Interface for a validator that supports validation rules.
    /// </summary>
    public interface IValidatorWithRules : IValidator
    {
        /// <summary>
        /// Adds a validation rule.
        /// </summary>
        /// <param name="rule">Validation rule to add.</param>
        void Add(Func<ValidationEvent, bool> rule);


        /// <summary>
        /// Adds a named validation rule.
        /// </summary>
        /// <param name="ruleName">Rule name.</param>
        /// <param name="rule">Validation rule to add.</param>
        void Add(string ruleName, Func<ValidationEvent, bool> rule);


        /// <summary>
        /// Removes a validation rule by its index.
        /// </summary>
        /// <param name="ndx">Validation rule index.</param>
        void RemoveAt(int ndx);


        /// <summary>
        /// Removes a validation rule by its name.
        /// </summary>
        /// <param name="ruleNname">Name of rule to remove.</param>
        void Remove(string ruleNname);


        /// <summary>
        /// Gets the number of validation rules.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Gets a validation rule.
        /// </summary>
        /// <param name="ndx">Index of validation rule.</param>
        /// <returns>Validation rule.</returns>
        Func<ValidationEvent, bool> this[int ndx] { get; }        
    }


    /// <summary>
    /// This class defines a validation rule.
    /// </summary>
    public class ValidationRuleDef
    {
        /// <summary>
        /// Description of the validation rule.
        /// </summary>
        public string Name;


        /// <summary>
        /// Validation function.
        /// </summary>
        public Func<ValidationEvent, bool> Rule;
    }
}