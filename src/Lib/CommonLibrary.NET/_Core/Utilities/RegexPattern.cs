using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib
{
    /// <summary>
    /// This class contains several common regular expressions.
    /// </summary>
    public class RegexPatterns
    {
        /// <summary>
        /// Alphabetic regex.
        /// </summary>
        public const string Alpha = @"^[a-zA-Z]*$";


        /// <summary>
        /// Uppercase Alphabetic regex.
        /// </summary>
        public const string AlphaUpperCase = @"^[A-Z]*$";


        /// <summary>
        /// Lowercase Alphabetic regex.
        /// </summary>
        public const string AlphaLowerCase = @"^[a-z]*$";


        /// <summary>
        /// Alphanumeric regex.
        /// </summary>
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";


        /// <summary>
        /// Alphanumeric and space regex.
        /// </summary>
        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";


        /// <summary>
        /// Alphanumeric and space and dash regex.
        /// </summary>
        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";


        /// <summary>
        /// Alphanumeric plus space, dash and underscore regex.
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";


        /// <summary>
        /// Alphaumieric plus space, dash, period and underscore regex.
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";


        /// <summary>
        /// Numeric regex.
        /// </summary>
        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";


        /// <summary>
        /// Numeric regex.
        /// </summary>
        public const string Integer = @"^\-?[0-9]*$";


        /// <summary>
        /// Ssn regex.
        /// </summary>
        public const string SocialSecurity = @"\d{3}[-]?\d{2}[-]?\d{4}";


        /// <summary>
        /// E-mail regex.
        /// </summary>
        public const string Email = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";


        /// <summary>
        /// Url regex.
        /// </summary>
        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";


        /// <summary>
        /// US zip code regex.
        /// </summary>
        public const string ZipCodeUS = @"\d{5}";

        
        /// <summary>
        /// US zip code with four digits regex.
        /// </summary>
        public const string ZipCodeUSWithFour = @"\d{5}[-]\d{4}";


        /// <summary>
        /// US zip code with optional four digits regex.
        /// </summary>
        public const string ZipCodeUSWithFourOptional = @"\d{5}([-]\d{4})?";


        /// <summary>
        /// US phone regex.
        /// </summary>
        public const string PhoneUS = @"\d{3}[-]?\d{3}[-]?\d{4}";
    }
}
