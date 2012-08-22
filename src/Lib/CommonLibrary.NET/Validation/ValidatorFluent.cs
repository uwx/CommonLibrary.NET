using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;


namespace ComLib.ValidationSupport
{
    /// <summary>
    /// This class implements a fluent-style validator.
    /// </summary>
    public class ValidatorFluent
    {
        private object _target;
        private string _objectName;
        private bool _appendObjectNameToError;
        private string _propertyName;
        private bool _checkCondition;
        private IValidationResults _errors;
        private int _initialErrorCount;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="typeToCheck">Type to check.</param>
        public ValidatorFluent(Type typeToCheck) : this(typeToCheck, false, null)
        {
        }


        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="typeToCheck">Type to check.</param>
        /// <param name="errors">Validation results.</param>
        public ValidatorFluent(Type typeToCheck, IValidationResults errors)
            : this(typeToCheck, false, errors)
        {
        }


        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="typeToCheck">Type to check.</param>
        /// <param name="appendTypeToError">True to append the type to an error.</param>
        /// <param name="errors">Validation results.</param>
        public ValidatorFluent(Type typeToCheck, bool appendTypeToError, IValidationResults errors)
        {
            _objectName = typeToCheck.Name;
            _appendObjectNameToError = appendTypeToError;
            _errors = errors == null ? new ValidationResults() : errors;
            _initialErrorCount = _errors.Count;
        }


        /// <summary>
        /// True if there are errors.
        /// </summary>
        public bool HasErrors
        {
            get { return _errors.Count > _initialErrorCount; }
        }


        /// <summary>
        /// Get/set the validation results.
        /// </summary>
        public IValidationResults Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }


        /// <summary>
        /// Set the validation target object.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Check(object target)
        {
            // Reset the check condition flag.
            _checkCondition = true;
            _propertyName = string.Empty;
            _target = target;
            return this;
        }


        /// <summary>
        /// Set the validation expression.
        /// </summary>
        /// <param name="exp">Validation expression.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Check(Expression<Func<object>> exp)
        {
            _target = ExpressionHelper.GetPropertyNameAndValue(exp, ref _propertyName);
            _checkCondition = true;
            return this;
        }


        /// <summary>
        /// Sets the property name and the target object.
        /// </summary>
        /// <param name="propName">Property name.</param>
        /// <param name="target">Target object.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Check(string propName, object target)
        {
            // Reset the check condition flag.            
            _checkCondition = true;
            _propertyName = propName;
            _target = target;
            return this;
        }


        /// <summary>
        /// Sets a check condition.
        /// </summary>
        /// <param name="isOkToCheckNext">Check condition.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent If(bool isOkToCheckNext)
        {
            _checkCondition = isOkToCheckNext;
            return this;
        }


        /// <summary>
        /// Validates against a specified value.
        /// </summary>
        /// <param name="val">Value to validate against.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Is(object val)
        {
            if (!_checkCondition) return this;

            if(_target == null && val == null)
                return this;

            if(_target == null && val != null)
                return IsValid(false, "must equal : " + val.ToString());

            return IsValid(val.Equals(_target), "must equal : " + val.ToString());
        }


        /// <summary>
        /// Determines if the validation is different than the specified value.
        /// </summary>
        /// <param name="val">Specified value.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsNot(object val)
        {
            if (!_checkCondition) return this;
            
            if (_target == null && val == null)
                return this;

            if (_target == null && val != null)
                return this;

            return IsValid(!val.Equals(_target), "must not equal : " + val.ToString());
        }


        /// <summary>
        /// Determines if the value is null.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsNull()
        {
            if (!_checkCondition) return this;
            
            return IsValid(_target == null, "must be null");
        }


        /// <summary>
        /// Determines if the value is not null.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsNotNull()
        {
            if (!_checkCondition) return this;

            return IsValid(_target != null, "must be not null");
        }


        /// <summary>
        /// Determines if the value is in a list of values.
        /// </summary>
        /// <typeparam name="T">Type of values in list.</typeparam>
        /// <param name="vals">List of values.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent In<T>(params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals == null || vals.Length == 0)
                return this;

            T checkVal = Converter.ConvertTo<T>(_target);
            bool isValid = false;
            foreach (object val in vals)
            {
                T validVal = Converter.ConvertTo<T>(val);
                if (checkVal.Equals(validVal))
                {
                    isValid = true;
                    break;
                }
            }
            return IsValid(isValid, "is not a valid value");
        }


        /// <summary>
        /// Determines if the value is not in a list of values.
        /// </summary>
        /// <typeparam name="T">Type of values in list.</typeparam>
        /// <param name="vals">List of values.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent NotIn<T>(params object[] vals)
        {
            if (!_checkCondition) return this;

            if (vals == null || vals.Length == 0)
                return this;

            T checkVal = Converter.ConvertTo<T>(_target);
            bool isValid = true;
            foreach (object val in vals)
            {
                T validVal = Converter.ConvertTo<T>(val);
                if (checkVal.Equals(validVal))
                {
                    isValid = false;
                    break;
                }
            }
            return IsValid(isValid, "is not a valid value");
        }


        /// <summary>
        /// Determines if a regular expression produces a match.
        /// </summary>
        /// <param name="regex">Regular expression.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Matches(string regex)
        {
            if (!_checkCondition) return this;

            return IsValid(Regex.IsMatch((string)_target, regex), "does not match pattern : " + regex);
        }


        /// <summary>
        /// Determines if a value is between a minimum and a maximum.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsBetween(int min, int max)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target);
            if( isNumeric )
            {
                double val = Convert.ToDouble(_target);
                return IsValid(min <= val && val <= max, "must be between : " + min + ", " + max);
            }
            else
            {
                // can only be string.
                string strVal = _target as string;
                if(min > 0 && string.IsNullOrEmpty(strVal))
                    return IsValid(false, "length must be between : " + min + ", " + max);

                return IsValid(min <= strVal.Length && strVal.Length <= max, "length must be between : " + min + ", " + max);
            } 
        }


        /// <summary>
        /// Determines if a value contains a specified string.
        /// </summary>
        /// <param name="val">Specified string.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Contains(string val)
        {
            if (!_checkCondition) return this;

            if (string.IsNullOrEmpty((string)_target))
                return IsValid(false, "does not contain : " + val);

            string valToCheck = (string)_target;
            return IsValid(valToCheck.Contains(val), "must contain : " + val);
        }


        /// <summary>
        /// Determines if a value does not contain a specified string.
        /// </summary>
        /// <param name="val">Specified string.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent NotContain(string val)
        {
            if (!_checkCondition) return this;

            if (string.IsNullOrEmpty((string)_target))
                return this;

            string valToCheck = (string)_target;
            return IsValid(!valToCheck.Contains(val), "should not contain : " + val);
        }


        /// <summary>
        /// Determines if a value is equal or above a specified minimum.
        /// </summary>
        /// <param name="min">Specified minimum.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Min(int min)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target);
            if (!isNumeric) return IsValid(false, "must be numeric value");

            double val = Convert.ToDouble(_target);
            return IsValid(val >= min, "must have minimum value of : " + min);
        }


        /// <summary>
        /// Determines if a value is equal or below a specified maximum.
        /// </summary>
        /// <param name="max">Specified maximum.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent Max(int max)
        {
            if (!_checkCondition) return this;

            bool isNumeric = TypeHelper.IsNumeric(_target);
            if (!isNumeric) return IsValid(false, "must be numeric value");

            double val = Convert.ToDouble(_target);
            return IsValid(val <= max, "must have maximum value of : " + max);
        }


        /// <summary>
        /// Determines if a value is true.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsTrue()
        {
            if (!_checkCondition) return this;

            bool isBool = _target is bool;
            if (!isBool) return IsValid(false, "must be bool(true/false)");

            return IsValid(((bool)_target) == true, "must be true");
        }


        /// <summary>
        /// Determines if a value is false.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsFalse()
        {
            if (!_checkCondition) return this;

            bool isBool = _target is bool;
            if (!isBool) return IsValid(false, "must be bool(true/false)");

            return IsValid(((bool)_target) == false, "must be false");
        }


        /// <summary>
        /// Determines if a day is after today.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsAfterToday()
        {
            if (!_checkCondition) return this;

            IsAfter(DateTime.Today);
            _checkCondition = false;
            return this;
        }


        /// <summary>
        /// Determines if a day is before today.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsBeforeToday()
        {
            if (!_checkCondition) return this;

            IsBefore(DateTime.Today);
            _checkCondition = false;
            return this;
        }


        /// <summary>
        /// Determines if a day is after a specified date.
        /// </summary>
        /// <param name="date">Specified date.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsAfter(DateTime date)
        {
            if (!_checkCondition) return this;

            DateTime checkVal = (DateTime)_target;
            return IsValid(checkVal.Date.CompareTo(date.Date) > 0, "must be after date : " + date.ToShortDateString());
        }


        /// <summary>
        /// Determines if a day is before a specified date.
        /// </summary>
        /// <param name="date">Specified date.</param>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsBefore(DateTime date)
        {
            if (!_checkCondition) return this;

            DateTime checkVal = (DateTime)_target;
            return IsValid(checkVal.Date.CompareTo(date.Date) < 0, "must be before date : " + date.ToShortDateString());
        }


        /// <summary>
        /// Determines if a value is a valid e-mail.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsValidEmail()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsEmail((string)_target, false), "must be a valid email.");
        }


        /// <summary>
        /// Determines if a value is a valid US phone.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsValidPhoneUS()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsPhoneUS((string)_target, false), "must be a valid U.S phone.");
        }


        /// <summary>
        /// Determines if a value is a valid URL.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsValidUrl()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsUrl((string)_target, false), "must be a valid url.");
        }


        /// <summary>
        /// Determines if a value is a valid zip code.
        /// </summary>
        /// <returns>The current instance of this class.</returns>
        public ValidatorFluent IsValidZip()
        {
            if (!_checkCondition) return this;

            return IsValid(Validation.IsZipCode((string)_target, false), "must be a valid zip.");
        }


        /// <summary>
        /// Returns the current instance of this class.
        /// </summary>
        /// <returns></returns>
        public ValidatorFluent End()
        {
            return this;
        }


        #region Check
        private ValidatorFluent IsValid(bool isValid, string error)
        {
            if (!isValid)
            {
                string prefix = string.IsNullOrEmpty(_propertyName) ? "Property " : _propertyName + " ";
                _errors.Add(prefix + error);
            }
            return this;
        }
        #endregion
    }
}
