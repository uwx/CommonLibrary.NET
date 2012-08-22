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
using Val = System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using ComLib;


namespace ComLib
{
    /// <summary>
    /// This class provides validation utility methods.
    /// </summary>
    public static partial class Validation
    {
        /// <summary>
        /// Error messages to return to callers.
        /// </summary>
        static ValidationExtensionMessages _messages = new ValidationExtensionMessages();


        /// <summary>
        /// Resets the error messages to the default values.
        /// </summary>
        public static void SetDefaultMessages()
        {
            _messages = new ValidationExtensionMessages();
        }


        /// <summary>
        /// Loads custom error messages to override the default values.
        /// </summary>
        /// <param name="messages">Custom validation error messages.</param>
        public static void SetCustomMessages(ValidationExtensionMessages messages)
        {
            _messages = messages;
        }

        #region IValidatorBasic Members
        /// <summary>
        /// Validates the object using System.ComponentModel.DataAnnotations, and puts any errors into the errors object.
        /// </summary>
        /// <param name="objectToValidate">Object to validate</param>
        /// <param name="errors">The collection to put validation errors into.</param>
        /// <returns>True if the validation succeeds.</returns>
        public static bool ValidateObject(object objectToValidate, IErrors errors = null)
        {

            var validationResults = new List<Val.ValidationResult>();
            var ctx = new Val.ValidationContext(objectToValidate, null, null);
            var isValid = Val.Validator.TryValidateObject(objectToValidate, ctx, validationResults, true);
            
            if(!isValid && errors != null)
                foreach (var result in validationResults)
                    errors.Add(result.ErrorMessage);
            
            return isValid;
        }


        /// <summary>
        /// Determine if string is valid with regard to minimum / maximum length.
        /// </summary>
        /// <param name="text">Text to check length of.</param>
        /// <param name="allowNull">Indicate whether or not to allow null.</param>
        /// <param name="checkMinLength">Whether or not to check the min length</param>
        /// <param name="checkMaxLength">Whether or not to check the max length</param>
        /// <param name="minLength">-1 to not check min length, > 0 to represent min length.</param>
        /// <param name="maxLength">-1 to not check max length, > 0 to represent max length.</param>
        /// <param name="errors">The error collection to populate if any validation errors occur</param>
        /// <param name="tag">The tag to use when populating any errors into the error collection.</param>
        /// <returns>True if match based on parameter conditions, false otherwise.</returns>
        public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength, IErrors errors, string tag)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (!allowNull)
                    errors.Add(tag + _messages.IsNotSupplied);
                return allowNull;
            }

            int textLength = text == null ? 0 : text.Length;  
              
            // Check bounds . -1 value for min/max indicates not to check.
            if (checkMinLength && minLength > 0 && textLength < minLength)
                return CheckError(false, errors, tag, string.Format(_messages.TextLessThanMinLength, minLength));

            if (checkMaxLength && maxLength > 0 && textLength > maxLength)
                return CheckError(false, errors, tag, string.Format(_messages.TextMoreThanMaxLength, maxLength));            

            return true;
        }


        /// <summary>
        /// Determines if string matches the regex.
        /// </summary>
        /// <param name="text">Text to match.</param>
        /// <param name="allowNull">Whether or not text can be null or empty for successful match.</param>
        /// <param name="regEx">Regular expression to use.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if match, false otherwise.</returns>
        public static bool IsStringRegExMatch(string text, bool allowNull, string regEx, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, regEx, errors, tag, _messages.TextNotMatchPattern);
        }


        /// <summary>
        /// Determines whether the text [is string in] [the specified values].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="allowNull">if set to <c>true</c> [allow null].</param>
        /// <param name="compareCase">if set to <c>true</c> [compare case].</param>
        /// <param name="allowedValues">The allowed values.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>
        /// 	<c>true</c> if [is string in] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStringIn(string text, bool allowNull, bool compareCase, string[] allowedValues, IErrors errors, string tag)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty && !allowNull)
            {
                string vals = allowedValues.JoinDelimited(",", (val) => val);
                errors.Add(tag, string.Format(_messages.TextMustBeIn, vals));
                return false;
            }
            bool isValid = false;
            foreach (string val in allowedValues)
            {
                if (string.Compare(text, val, compareCase) == 0)
                {
                    isValid = true;
                    break;
                }
            }
            if (!isValid)
            {
                string vals = allowedValues.JoinDelimited(",", (val) => val);
                errors.Add(tag, string.Format(_messages.TextMustBeIn, vals));
                return false;
            }
            return true;
        }

        
        /// <summary>
        /// Determines if text supplied is numeric
        /// </summary>
        /// <param name="text"></param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is numeric.</returns>
        public static bool IsNumeric(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Numeric, errors, tag, _messages.TextNotValidNumber);
        }

        
        /// <summary>
        /// Determines if text supplied is numeric and within the min/max bounds.
        /// </summary>
        /// <param name="text">Text to check if it's numeric and within bounds.</param>
        /// <param name="checkMinBound">Whether or not to check the min</param>
        /// <param name="checkMaxBound">Whether or not to check the max</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is numeric and within range.</returns>
        public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string tag)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Numeric);
            if (!isNumeric)
            {
                errors.Add(tag, _messages.TextNotNumeric);
                return false;
            }

            double num = Double.Parse(text);
            return IsNumericWithinRange(num, checkMinBound, checkMaxBound, min, max, errors, tag);
        }


        /// <summary>
        /// Determines if text supplied is numeric and within the min/max bounds.
        /// </summary>
        /// <param name="num">Number to check if it's numeric and within bounds.</param>
        /// <param name="checkMinBound">Whether or not to check the min</param>
        /// <param name="checkMaxBound">Whether or not to check the max</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is numeric and within range.</returns>
        public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max, IErrors errors, string tag)
        {
            if (checkMinBound && num < min)
            {
                errors.Add(tag, string.Format(_messages.NumberLessThan, min));
                return false;
            }

            if (checkMaxBound && num > max)
            {
                errors.Add(tag, string.Format(_messages.NumberMoreThan, max));
                return false;
            }

            return true;
        }


        /// <summary>
        /// Determines if text is either lowercase/uppercase alphabets.
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is alphanumeric.</returns>
        public static bool IsAlpha(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Alpha, errors, tag, string.Format(_messages.TextMustContainOnlyChars, RegexPatterns.Alpha));
        }


        /// <summary>
        /// Determines if text is either lowercase/uppercase alphabets or numbers.
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>Ture if the text is alphanumeric.</returns>
        public static bool IsAlphaNumeric(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.AlphaNumeric, errors, tag, string.Format(_messages.TextMustContainOnlyCharsAndNumbers, RegexPatterns.AlphaNumeric));
        }


        /// <summary>
        /// Determines if the date supplied is a date.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid date.</returns>
        public static bool IsDate(string text, IErrors errors, string tag)
        {
            DateTime result = DateTime.MinValue;
            return CheckError(DateTime.TryParse(text, out result), errors, tag, _messages.TextInvalidDate);
        }


        /// <summary>
        /// Determines if the date supplied is a date.
        /// </summary>
        /// <param name="text">Text to check if it's date and within bounds specified.</param>
        /// <param name="checkMinBound">Whether or not to check the min</param>
        /// <param name="checkMaxBound">Whether or not to check the max</param>
        /// <param name="minDate">Min date.</param>
        /// <param name="maxDate">Max date.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid date within the range.</returns>
        public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string tag)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(text, out result))
            {
                errors.Add(tag, _messages.TextInvalidDate);
                return false;
            }

            return IsDateWithinRange(result, checkMinBound, checkMaxBound, minDate, maxDate, errors, tag);
        }


        /// <summary>
        /// Determines if the date supplied is a date within the specified bounds.
        /// </summary>
        /// <param name="date">Date to check if within bounds specified.</param>
        /// <param name="checkMinBound">Whether or not to check the min</param>
        /// <param name="checkMaxBound">Whether or not to check the max</param>
        /// <param name="minDate">Min date.</param>
        /// <param name="maxDate">Max date.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid date within the range.</returns>
        public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate, IErrors errors, string tag)
        {
            if (checkMinBound && date.Date < minDate.Date)
            {
                errors.Add(tag, string.Format(_messages.DateLessThanMinDate, minDate.ToShortDateString()));
                return false;
            }
            if (checkMaxBound && date.Date > maxDate.Date)
            {
                errors.Add(tag, string.Format(_messages.DateMoreThanMaxDate, maxDate.ToShortDateString()));
                return false;
            }

            return true;
        }


        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Text to check.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid time.</returns>
        public static bool IsTimeOfDay(string time, IErrors errors, string tag)
        {
            TimeSpan span = TimeSpan.MinValue;
            bool isMatch = TimeSpan.TryParse(time, out span);
            return CheckError(isMatch, errors, tag, _messages.TextInvalidTime);
        }


        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Text to check.</param>
        /// <param name="checkMinBound">True to check min time span.</param>
        /// <param name="checkMaxBound">True to check max time span.</param>
        /// <param name="min">Min time span.</param>
        /// <param name="max">Max time span.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid time within the range.</returns>
        public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string tag)
        {
            TimeSpan span = TimeSpan.MinValue;
            if (!TimeSpan.TryParse(time, out span))
                return CheckError(false, errors, tag, _messages.TextInvalidTime);

            return IsTimeOfDayWithinRange(span, checkMinBound, checkMaxBound, min, max, errors, tag);
        }

        
        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Text to check.</param>
        /// <param name="checkMinBound">True to check min bound.</param>
        /// <param name="checkMaxBound">True to check max bound.</param>
        /// <param name="min">Min time span.</param>
        /// <param name="max">Max time span.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid time within the range.</returns>
        public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max, IErrors errors, string tag)
        {
            return true;
        }


        /// <summary>
        /// Determines if the phone number supplied is a valid US phone number.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid US phone number.</returns>
        public static bool IsPhoneUS(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.PhoneUS, errors, tag, _messages.TextInvalidUSPhone);
        }


        /// <summary>
        /// Determines if the phone number supplied is a valid US phone number.
        /// </summary>
        /// <param name="phone">Text to check.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid US phone number.</returns>
        public static bool IsPhoneUS(int phone, IErrors errors, string tag)
        {
            return CheckErrorRegEx(phone.ToString(), false, RegexPatterns.PhoneUS, errors, tag, _messages.TextInvalidUSPhone);
        }


        /// <summary>
        /// Determines if ssn supplied is a valid ssn.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <returns>True if the text is a valid ssn.</returns>
        public static bool IsSsn(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.SocialSecurity, errors, tag, _messages.TextInvalidSSN);
        }

        
        /// <summary>
        /// Determines if ssn supplied is a valid ssn.
        /// </summary>
        /// <param name="ssn">Text to check.</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is a valid ssn.</returns>
        public static bool IsSsn(int ssn, IErrors errors, string tag)
        {
            return CheckErrorRegEx(ssn.ToString(), false, RegexPatterns.SocialSecurity, errors, tag, _messages.TextInvalidSSN);
        }


        /// <summary>
        /// Determines if email supplied is a valid email.
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is a valid e-mail.</returns>
        public static bool IsEmail(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Email, errors, tag, _messages.TextInvalidEmail);
        }


        /// <summary>
        /// Determines if url supplied is a valid url.
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is a valid url.</returns>
        public static bool IsUrl(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.Url, errors, tag, _messages.TextInvalidUrl);
        }


        /// <summary>
        /// Determines if text supplied is a valid zip code.
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is a valid zip code.</returns>
        public static bool IsZipCode(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.ZipCodeUS, errors, tag, _messages.TextInvalidUSZip);
        }


        /// <summary>
        /// Determines if text supplied is a valid zip with 4 additional chars.
        /// e.g. 12345-2321
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is valid.</returns>
        public static bool IsZipCodeWith4Char(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.ZipCodeUSWithFour, errors, tag, _messages.TextInvalidUSZip);
        }


        /// <summary>
        /// Determines if text supplied is a valid zip with 4 additional chars.
        /// e.g. 12345-2321
        /// </summary>
        /// <param name="text">The text check</param>
        /// <param name="allowNull">Whether or not the text can be null</param>
        /// <param name="errors">List of errors.</param>
        /// <param name="tag">Tag used to identify the error.</param>
        /// <returns>True if the text is valid.</returns>
        public static bool IsZipCodeWith4CharOptional(string text, bool allowNull, IErrors errors, string tag)
        {
            return CheckErrorRegEx(text, allowNull, RegexPatterns.ZipCodeUSWithFourOptional, errors, tag, _messages.TextInvalidUSZip);
        }
        #endregion


        /// <summary>
        /// Check the condition and add the error.
        /// </summary>
        /// <param name="isValid">True to add the error.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <param name="error">The error text to add</param>
        /// <returns>isValid.</returns>
        public static bool CheckError(bool isValid, IErrors errors, string tag, string error)
        {
            if (!isValid)
            {
                errors.Add(tag, error);                
            }
            return isValid;
        }


        /// <summary>
        /// Check the text for the regex pattern and adds errors in incorrect.
        /// </summary>
        /// <param name="inputText">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <param name="regExPattern">Pattern to use for checking.</param>
        /// <param name="errors">The error collection to populate with any errors.</param>
        /// <param name="tag">Tag used in an error message.</param>
        /// <param name="error">Error string.</param>
        /// <returns>True if the checking succeeds.</returns>
        public static bool CheckErrorRegEx(string inputText, bool allowNull, string regExPattern, IErrors errors, string tag, string error)
        {
            bool isEmpty = string.IsNullOrEmpty(inputText);
            if (allowNull && isEmpty)
                return true;

            if (!allowNull && isEmpty)
            {
                errors.Add(tag, error);
                return false;
            }

            bool isValid = Regex.IsMatch(inputText, regExPattern);
            if (!isValid) errors.Add(tag, error);

            return isValid;
        }
    }
}
