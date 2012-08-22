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
    public class ValidatorWithRules : Validator, IValidatorWithRules
    {
        /// <summary>
        /// List with validation rules.
        /// </summary>
        protected List<ValidationRuleDef> _rules;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorWithRules"/> class.
        /// </summary>
        public ValidatorWithRules() : this(null)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorWithRules"/> class
        /// with a lamda validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public ValidatorWithRules(Func<ValidationEvent, bool> validator) : base( validator)
        {
            _rules = new List<ValidationRuleDef>();
        }


        /// <summary>
        /// Add new validation rule.
        /// </summary>
        /// <param name="rule">Rule function.</param>
        public void Add(Func<ValidationEvent, bool> rule)
        {
            Add(string.Empty, rule);
        }


        /// <summary>
        /// Add new validation rule.
        /// </summary>
        /// <param name="ruleName">Rule name.</param>
        /// <param name="rule">Rule function.</param>
        public void Add(string ruleName, Func<ValidationEvent, bool> rule)
        {
            var ruleDef = new ValidationRuleDef() { Name = ruleName, Rule = rule };
            _rules.Add(ruleDef);
        }


        /// <summary>
        /// Remove at the specified index.
        /// </summary>
        /// <param name="ndx">The NDX.</param>
        public void RemoveAt(int ndx)
        {
            if (ndx < 0 || ndx >= _rules.Count) return;

            _rules.RemoveAt(ndx);
        }


        /// <summary>
        /// Remove with the specified name.
        /// </summary>
        /// <param name="name">Rule name.</param>
        public void Remove(string name)
        {
            List<int> indexesToRemove = new List<int>();
            for (int ndx = 0; ndx < _rules.Count; ndx++)
            {
                var ruleDef = _rules[ndx];
                if (string.Compare(ruleDef.Name, name, true) == 0)
                    indexesToRemove.Add(ndx);
            }
            if (indexesToRemove.Count > 0)
            {
                indexesToRemove.Reverse();
                foreach (int ndx in indexesToRemove)
                    _rules.RemoveAt(ndx);
            }
        }


        /// <summary>
        /// Gets the validation rule at the specified index.
        /// </summary>
        /// <value></value>
        /// <param name="ndx">Rule index.</param>
        /// <returns>Validation rule.</returns>
        public Func<ValidationEvent, bool> this[int ndx]
        {
            get
            {
                if (ndx < 0 || ndx >= _rules.Count)
                    return null;

                return _rules[ndx].Rule;
            }
        }


        /// <summary>
        /// Clear all the rules.
        /// </summary>
        public override void Clear()
        {
            _lastValidationResults = new ValidationResults();
            _rules.Clear();
        }


        /// <summary>
        /// Number of rules.
        /// </summary>
        /// <value>The count.</value>
        public int Count 
        { 
            get { return _rules.Count; } 
        }


        /// <summary>
        /// Validates all the rules in the internal rule list.
        /// </summary>
        /// <param name="validationEvent">Validation event.</param>
        /// <returns>Validation result.</returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            // Should this even be here?
            if (_validatorLamda != null)
                return _validatorLamda(validationEvent);

            // Run the validations against all the rules.
            int initialErrorCount = validationEvent.Results.Count;
            foreach (var rule in _rules)
            {
                rule.Rule(validationEvent);
            }
            return validationEvent.Results.Count == initialErrorCount;
        }
    }
}
