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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;



namespace ComLib
{
    /// <summary>
    /// This class provides validation utility methods.
    /// </summary>
    public static partial class Validation
    {
        #region IValidatorBasic Members
        /// <summary>
        /// Determine if string is valid with regard to minimum / maximum length.
        /// </summary>
        /// <param name="text">Text to check length of.</param>
        /// <param name="allowNull">Indicate whether or not to allow null.</param>
        /// <param name="checkMaxLength">Indicate whether to check min length.</param>
        /// <param name="checkMinLength">Indicate whether to check max length.</param>
        /// <param name="minLength">-1 to not check min length, > 0 to represent min length.</param>
        /// <param name="maxLength">-1 to not check max length, > 0 to represent max length.</param>
        /// <returns>True if match based on parameter conditions, false otherwise.</returns>
        public static bool IsStringLengthMatch(string text, bool allowNull, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return allowNull;

            // Check bounds . -1 value for min/max indicates not to check.
            if (checkMinLength && minLength > 0 && text.Length < minLength)
                return false;
            if (checkMaxLength && maxLength > 0 && text.Length > maxLength)
                return false;

            return true;
        }


        /// <summary>
        /// Check if the integer value is between the min/max sizes.
        /// </summary>
        /// <param name="val">The value to check.</param>
        /// <param name="checkMinLength">True to check for the minimum bound.</param>
        /// <param name="checkMaxLength">True to check for the maximuim bound.</param>
        /// <param name="minLength">Minimum length.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <returns>True if match based on parameter conditions, false otherwise.</returns>
        public static bool IsBetween(int val, bool checkMinLength, bool checkMaxLength, int minLength, int maxLength)
        {
            // Check bounds . -1 value for min/max indicates not to check.
            if (checkMinLength && val < minLength)
                return false;
            if (checkMaxLength && val > maxLength)
                return false;

            return true;
        }


        /// <summary>
        /// Determine if the size <paramref name="val"/> in bytes is between the min/max size in kilobytes
        /// </summary>
        /// <param name="val">Value of size to check.</param>
        /// <param name="checkMinLength">True to check for the minimum bound.</param>
        /// <param name="checkMaxLength">True to check for the maximuim bound.</param>
        /// <param name="minKilobytes">Min kilobytes.</param>
        /// <param name="maxKilobytes">Max kilobytes.</param>
        /// <returns>True if match based on parameter conditions, false otherwise.</returns>
        public static bool IsSizeBetween(int val, bool checkMinLength, bool checkMaxLength, int minKilobytes, int maxKilobytes)
        {
            // convert to kilobytes.
            val = val / 1000;

            // Check bounds . -1 value for min/max indicates not to check.
            if (checkMinLength && val < minKilobytes)
                return false;
            if (checkMaxLength && val > maxKilobytes)
                return false;

            return true;
        }


        /// <summary>
        /// Determines if string matches the regex.
        /// </summary>
        /// <param name="text">Text to match.</param>
        /// <param name="allowNull">Whether or not text can be null or empty for successful match.</param>
        /// <param name="regEx">Regular expression to use.</param>
        /// <returns>True if match, false otherwise.</returns>
        public static bool IsStringRegExMatch(string text, bool allowNull, string regEx)
        {
            if (allowNull && string.IsNullOrEmpty(text))
                return true;

            return Regex.IsMatch(text, regEx);
        }

        
        /// <summary>
        /// Determines if text supplied is numeric
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <returns></returns>
        public static bool IsNumeric(string text)
        {
            return Regex.IsMatch(text, RegexPatterns.Numeric);
        }

        
        /// <summary>
        /// Determines if text supplied is numeric and within the min/max bounds.
        /// </summary>
        /// <param name="text">Text to check if it's numeric and within bounds.</param>
        /// <param name="checkMinBound">Whether or not to check the min bound.</param>
        /// <param name="checkMaxBound">Whether or not to check the max bound.</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <returns>True if the text is numeric and within the bounds.</returns>
        public static bool IsNumericWithinRange(string text, bool checkMinBound, bool checkMaxBound, double min, double max)
        {
            bool isNumeric = Regex.IsMatch(text, RegexPatterns.Numeric);
            if (!isNumeric) return false;

            double num = Double.Parse(text);
            return IsNumericWithinRange(num, checkMinBound, checkMaxBound, min, max);
        }


        /// <summary>
        /// Determines if text supplied is numeric and within the min/max bounds.
        /// </summary>
        /// <param name="num">Num to check if it's numeric and within bounds.</param>
        /// <param name="checkMinBound">Whether or not to check min</param>
        /// <param name="checkMaxBound">Whether or not to check max</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <returns>True if the number is within range.</returns>
        public static bool IsNumericWithinRange(double num, bool checkMinBound, bool checkMaxBound, double min, double max)
        {
            if (checkMinBound && num < min)
                return false;

            if (checkMaxBound && num > max)
                return false;

            return true;
        }


        /// <summary>
        /// Determines if text is either lowercase/uppercase alphabets.
        /// </summary>
        /// <param name="text">String to check.</param>
        /// <param name="allowNull">True to allow null value.</param>
        /// <returns>True if the text is alphanumeric.</returns>
        public static bool IsAlpha(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Alpha);
        }


        /// <summary>
        /// Determines if text is either lowercase/uppercase alphabets or numbers.
        /// </summary>
        /// <param name="text">String to check.</param>
        /// <param name="allowNull">True to allow null value.</param>
        /// <returns>True if the text is alphanumeric.</returns>
        public static bool IsAlphaNumeric(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.AlphaNumeric);
        }


        /// <summary>
        /// Determines if the date supplied is a date.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <returns>True if the text is a valid date.</returns>
        public static bool IsDate(string text)
        {
            DateTime result = DateTime.MinValue;
            return DateTime.TryParse(text, out result);
        }


        /// <summary>
        /// Determines if the date supplied is a date.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="checkMinBound">True to check for a minumum bound.</param>
        /// <param name="checkMaxBound">True to check for a maximum bound.</param>
        /// <param name="minDate">Min date.</param>
        /// <param name="maxDate">Max date.</param>
        /// <returns>True if the text is a valid date within bounds.</returns>
        public static bool IsDateWithinRange(string text, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate)
        {
            DateTime result = DateTime.MinValue;
            if (!DateTime.TryParse(text, out result)) return false;

            return IsDateWithinRange(result, checkMinBound, checkMaxBound, minDate, maxDate);
        }


        /// <summary>
        /// Determines if the date supplied is a date within the specified bounds.
        /// </summary>
        /// <param name="date">Date to check.</param>
        /// <param name="checkMinBound">True to check for a minumum bound.</param>
        /// <param name="checkMaxBound">True to check for a maximum bound.</param>
        /// <param name="minDate">Min date.</param>
        /// <param name="maxDate">Max date.</param>
        /// <returns>True if the date is within bounds.</returns>
        public static bool IsDateWithinRange(DateTime date, bool checkMinBound, bool checkMaxBound, DateTime minDate, DateTime maxDate)
        {
            if (checkMinBound && date.Date < minDate.Date) return false;
            if (checkMaxBound && date.Date > maxDate.Date) return false;

            return true;
        }


        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Text to check.</param>
        /// <returns>True if the text is a valid time of day.</returns>
        public static bool IsTimeOfDay(string time)
        {
            TimeSpan span = TimeSpan.MinValue;
            return TimeSpan.TryParse(time, out span);
        }


        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Text to check.</param>
        /// <param name="checkMinBound">True to check against min bound.</param>
        /// <param name="checkMaxBound">True to check against max bound.</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <returns>True if the text is a valid time and within the bounds.</returns>
        public static bool IsTimeOfDayWithinRange(string time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max)
        {
            TimeSpan span = TimeSpan.MinValue;
            if (!TimeSpan.TryParse(time, out span))
                return false;

            return IsTimeOfDayWithinRange(span, checkMinBound, checkMaxBound, min, max);
        }

        
        /// <summary>
        /// Determines if the time string specified is a time of day. e.g. 9am
        /// and within the bounds specified.
        /// </summary>
        /// <param name="time">Instance of time span to check.</param>
        /// <param name="checkMinBound">True to check against min bound.</param>
        /// <param name="checkMaxBound">True to check against max bound.</param>
        /// <param name="min">Min bound.</param>
        /// <param name="max">Max bound.</param>
        /// <returns>True if the time is within the bounds.</returns>
        public static bool IsTimeOfDayWithinRange(TimeSpan time, bool checkMinBound, bool checkMaxBound, TimeSpan min, TimeSpan max)
        {
            return true;
        }


        /// <summary>
        /// Determines if the phone number supplied is a valid US phone number.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid US phone number.</returns>
        public static bool IsPhoneUS(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.PhoneUS);
        }


        /// <summary>
        /// Determines if the phone number supplied is a valid US phone number.
        /// </summary>
        /// <param name="phone">Text to check.</param>
        /// <returns>True if the text is a valid US phone number.</returns>
        public static bool IsPhoneUS(int phone)
        {
            return Regex.IsMatch(phone.ToString(), RegexPatterns.PhoneUS);
        }


        /// <summary>
        /// Determines if ssn supplied is a valid ssn.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid ssn.</returns>
        public static bool IsSsn(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.SocialSecurity);
        }

        
        /// <summary>
        /// Determines if ssn supplied is a valid ssn.
        /// </summary>
        /// <param name="ssn">Text to check.</param>
        /// <returns>True if the text is a valid ssn.</returns>
        public static bool IsSsn(int ssn)
        {
            return Regex.IsMatch(ssn.ToString(), RegexPatterns.SocialSecurity);
        }


        /// <summary>
        /// Determines if email supplied is a valid email.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid e-mail.</returns>
        public static bool IsEmail(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Email);
        }


        /// <summary>
        /// Determines if url supplied is a valid url.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid url.</returns>
        public static bool IsUrl(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.Url);
        }


        /// <summary>
        /// Determines if text supplied is a valid zip code.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid zip code.</returns>
        public static bool IsZipCode(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.ZipCodeUS);
        }


        /// <summary>
        /// Determines if text supplied is a valid zip with 4 additional chars.
        /// e.g. 12345-2321
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <returns>True if the text is a valid zip code.</returns>
        public static bool IsZipCodeWith4Char(string text, bool allowNull)
        {
            return IsMatchRegEx(text, allowNull, RegexPatterns.ZipCodeUSWithFour);
        }


        /// <summary>
        /// Determines if items are equal.
        /// </summary>
        /// <typeparam name="T">Type of objects to check.</typeparam>
        /// <param name="obj1">First object.</param>
        /// <param name="obj2">Second object.</param>
        /// <returns>True if the objects are equal.</returns>
        public static bool AreEqual<T>(T obj1, T obj2) where T : IComparable<T>
        {
            return obj1.CompareTo(obj2) == 0;
        }


        /// <summary>
        /// Determines if objects are not equal
        /// </summary>
        /// <typeparam name="T">Type of objects to check.</typeparam>
        /// <param name="obj1">First object.</param>
        /// <param name="obj2">Second object.</param>
        /// <returns>True if the objects are not equal.</returns>
        public static bool AreNotEqual<T>(T obj1, T obj2) where T : IComparable<T>
        {
            return obj1.CompareTo(obj2) != 0;
        }
        #endregion


        /// <summary>
        /// Checks if a text matches a regular expression.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <param name="allowNull">True to allow a null value.</param>
        /// <param name="regExPattern">Regular expression to use.</param>
        /// <returns>True if the text matches the expression.</returns>
        public static bool IsMatchRegEx(string text, bool allowNull, string regExPattern)
        {
            bool isEmpty = string.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty && !allowNull) return false;

            return Regex.IsMatch(text, regExPattern);
        }


        /// <summary>
        /// Is valid - text doesn't contain any word that has
        /// more than maxChars specified.
        /// <param name="text">Text to check.</param>
        /// <param name="maxCharsInWord">Maximum characters to allow.</param>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>True if the text is valid.</returns>
        /// </summary>
        public static bool ContainsLongSingleWord(string text, int maxCharsInWord, string errorMessage)
        {            
            if (string.IsNullOrEmpty(text)) { return true; }

            bool isSpacerNewLine = false;
            int currentPosition = 0;
            int ndxSpace = StringHelper.GetIndexOfSpacer(text, currentPosition, ref isSpacerNewLine);

            //Check if single very long word ( no spaces )
            if (ndxSpace < 0 && text.Length > maxCharsInWord)
            {
                //results.Add(errorMessage + maxCharsInWord + " chars.");
                return false;
            }

            while ((currentPosition < text.Length && ndxSpace > 0))
            {
                //Lenght of word 
                int wordLength = ndxSpace - (currentPosition + 1);
                if (wordLength > maxCharsInWord)
                {
                    //results.Add(_errorMessage + _maxCharsInWord + " chars.");
                    return false;
                }
                currentPosition = ndxSpace;
                ndxSpace = StringHelper.GetIndexOfSpacer(text, (currentPosition + 1), ref isSpacerNewLine);
            }

            // Final check.. no space found but check complete length now.
            if (currentPosition < text.Length && ndxSpace < 0)
            {
                //Lenght of word 
                int wordLength = (text.Length - 1) - currentPosition;
                if (wordLength > maxCharsInWord)
                {
                    //results.Add(_errorMessage + _maxCharsInWord + " chars.");
                    return false;
                }
            }
            return true;
        }
    }

}
