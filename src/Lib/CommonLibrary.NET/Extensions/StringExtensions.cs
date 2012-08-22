using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace ComLib.Extensions
{
    /// <summary>
    /// This static class contains several common String extension methods.
    /// </summary>
    public static class StringExtensions
    {
        #region Appending
        /// <summary>
        /// Multiply a string N number of times.
        /// </summary>
        /// <param name="str">String to multiply.</param>
        /// <param name="times">Number of times to multiply the string.</param>
        /// <returns>Original string multiplied N times.</returns>
        public static string Times(this string str, int times)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            if (times <= 1) return str;

            string strfinal = string.Empty;
            for (int ndx = 0; ndx < times; ndx++)
                strfinal += str;

            return strfinal;
        }


        /// <summary>
        /// Increases the string to the maximum length specified.
        /// If the string is already greater than maxlength, it is truncated if the flag truncate is true.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="truncate">if set to <c>true</c> [truncate].</param>
        /// <returns>Increased string.</returns>
        public static string IncreaseTo(this string str, int maxLength, bool truncate)
        {
            if (string.IsNullOrEmpty(str)) return str;
            if (str.Length == maxLength) return str;
            if (str.Length > maxLength && truncate) return str.Truncate(maxLength);

            string original = str;

            while (str.Length < maxLength)
            {
                // Still less after appending by original string.
                if (str.Length + original.Length < maxLength)
                {
                    str += original;
                }
                else // Append partial.
                {
                    str += str.Substring(0, maxLength - str.Length);
                }
            }
            return str;
        }


        /// <summary>
        /// Increases the string to the maximum length specified.
        /// If the string is already greater than maxlength, it is truncated if the flag truncate is true.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="minLength">String minimum length.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="truncate">if set to <c>true</c> [truncate].</param>
        /// <returns>Randomly increased string.</returns>
        public static string IncreaseRandomly(this string str, int minLength, int maxLength, bool truncate)
        {
            Random random = new Random(minLength);
            int randomMaxLength = random.Next(minLength, maxLength);
            return IncreaseTo(str, randomMaxLength, truncate);
        }
        #endregion


        #region Truncation
        /// <summary>
        /// Truncates the string.
        /// </summary>
        /// <param name="txt">String to truncate.</param>
        /// <param name="maxChars">Maximum string length.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this string txt, int maxChars)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            return txt.Substring(0, maxChars);
        }


        /// <summary>
        /// Truncate the text supplied by number of characters specified by <paramref name="maxChars"/>
        /// and then appends the suffix.
        /// </summary>
        /// <param name="txt">String to truncate.</param>
        /// <param name="maxChars">Maximum string length.</param>
        /// <param name="suffix">Suffix to append to string.</param>
        /// <returns>Truncated string with suffix.</returns>
        public static string TruncateWithText(this string txt, int maxChars, string suffix)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            // Now do the truncate and more.
            string partial = txt.Substring(0, maxChars);
            return partial + suffix;
        }
        #endregion


        #region Conversion
        /// <summary>
        /// Convert the text  to bytes.
        /// </summary>
        /// <param name="txt">Text to convert to bytes.</param>
        /// <returns>ASCII bytes representing the string.</returns>
        public static byte[] ToBytesAscii(this string txt)
        {
            return txt.ToBytesEncoding(new System.Text.ASCIIEncoding());
        }


        /// <summary>
        /// Convert the text to bytes using the system default code page.
        /// </summary>
        /// <param name="txt">Text to convert to bytes.</param>
        /// <returns>Bytes representing the string.</returns>
        public static byte[] ToBytes(this string txt)
        {
            return txt.ToBytesEncoding(System.Text.Encoding.Default);
        }


        /// <summary>
        /// Convert the text to bytes using a specified encoding.
        /// </summary>
        /// <param name="txt">Text to convert to bytes.</param>
        /// <param name="encoding">Encoding to use during the conversion.</param>
        /// <returns>Bytes representing the string.</returns>
        public static byte[] ToBytesEncoding(this string txt, Encoding encoding)
        {
            if (string.IsNullOrEmpty(txt))
                return new byte[] { };

            return encoding.GetBytes(txt);
        }


        /// <summary>
        /// Converts an ASCII byte array to a string.
        /// </summary>
        /// <param name="bytes">ASCII bytes.</param>
        /// <returns>String representation of ASCII bytes.</returns>
        public static string StringFromBytesASCII(this byte[] bytes)
        {
            return bytes.StringFromBytesEncoding(new System.Text.ASCIIEncoding());
        }


        /// <summary>
        /// Converts a byte array to a string using the system default code page.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <returns>String representation of bytes.</returns>
        public static string StringFromBytes(this byte[] bytes)
        {
            return bytes.StringFromBytesEncoding(System.Text.Encoding.Default);
        }

        
        /// <summary>
        /// Converts a byte array to a string using a specified encoding.
        /// </summary>
        /// <param name="bytes">Byte array.</param>
        /// <param name="encoding">Encoding to use during the conversion.</param>
        /// <returns>String representation of bytes.</returns>
        public static string StringFromBytesEncoding(this byte[] bytes, Encoding encoding)
        {
            if (0 == bytes.GetLength(0))
                return null;

            return encoding.GetString(bytes);
        }


        /// <summary>
        /// Converts "yes/no/true/false/0/1"
        /// </summary>
        /// <param name="txt">String to convert to boolean.</param>
        /// <returns>Boolean converted from string.</returns>
        public static object ToBoolObject(this string txt)
        {
            return ToBool(txt) as object;
        }


        /// <summary>
        /// Converts "yes/no/true/false/0/1"
        /// </summary>
        /// <param name="txt">String to convert to boolean.</param>
        /// <returns>Boolean converted from string.</returns>
        public static bool ToBool(this string txt)
        {            
            if (string.IsNullOrEmpty(txt))
                return false;

            string trimed = txt.Trim().ToLower();
            if (trimed == "yes" || trimed == "true" || trimed == "1")
                return true;

            return false;
        }


        /// <summary>
        /// Converts a string to an integer and returns it as an object.
        /// </summary>
        /// <param name="txt">String to convert to integer.</param>
        /// <returns>Integer converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static object ToIntObject(this string txt)
        {
            return ToInt(txt) as object;
        }


        /// <summary>
        /// Converts a string to an integer.
        /// </summary>
        /// <param name="txt">String to convert to integer.</param>
        /// <returns>Integer converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static int ToInt(this string txt)
        {
            return ToNumber<int>(txt, (s) => Convert.ToInt32(Convert.ToDouble(s)), 0);
        }


        /// <summary>
        /// Converts a string to a long and returns it as an object.
        /// </summary>
        /// <param name="txt">String to convert to long.</param>
        /// <returns>Long converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static object ToLongObject(this string txt)
        {
            return ToLong(txt) as object;
        }


        /// <summary>
        /// Converts a string to a long.
        /// </summary>
        /// <param name="txt">String to convert to long.</param>
        /// <returns>Long converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static long ToLong(this string txt)
        {
            return ToNumber<long>(txt, (s) => Convert.ToInt64(s), 0);
        }


        /// <summary>
        /// Converts a string to a double and returns it as an object.
        /// </summary>
        /// <param name="txt">String to convert to double.</param>
        /// <returns>Double converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static object ToDoubleObject(this string txt)
        {
            return ToDouble(txt) as object;
        }


        /// <summary>
        /// Converts a string to a double.
        /// </summary>
        /// <param name="txt">String to convert from double.</param>
        /// <returns>Double converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static double ToDouble(this string txt)
        {
            return ToNumber<double>(txt, (s) => Convert.ToDouble(s), 0);
        }


        /// <summary>
        /// Converts a string to a float and returns it as an object.
        /// </summary>
        /// <param name="txt">String to convert to float.</param>
        /// <returns>Float converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static object ToFloatObject(this string txt)
        {
            return ToFloat(txt) as object;
        }


        /// <summary>
        /// Converts a string as a float and returns it.
        /// </summary>
        /// <param name="txt">String to convert to float.</param>
        /// <returns>Float converted from string.</returns>
        /// <remarks>The method takes into starting monetary symbols like $.</remarks>
        public static float ToFloat(this string txt)
        {
            return ToNumber<float>(txt, (s) => Convert.ToSingle(s), 0);
        }


        /// <summary>
        /// Converts to a number using the callback.
        /// </summary>
        /// <typeparam name="T">Type to convert to.</typeparam>
        /// <param name="txt">String to convert.</param>
        /// <param name="callback">Conversion callback method.</param>
        /// <param name="defaultValue">Default conversion value.</param>
        /// <returns>Instance of type converted from string.</returns>
        public static T ToNumber<T>(string txt, Func<string, T> callback, T defaultValue)
        {            
            if (string.IsNullOrEmpty(txt))
                return defaultValue;

            string trimed = txt.Trim().ToLower();
            // Parse $ or the system currency symbol.
            if (trimed.StartsWith("$") || trimed.StartsWith(Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol))
            {
                trimed = trimed.Substring(1);
            }
            return callback(trimed);
        }


        /// <summary>
        /// Converts a string to a timespan instance and returns it as an object.
        /// </summary>
        /// <param name="txt">String to convert to a timespan instance.</param>
        /// <returns>Timespan instance converted from string.</returns>
        public static object ToTimeObject(this string txt)
        {
            return ToTime(txt) as object;
        }


        /// <summary>
        /// Converts a string to a timespan instance.
        /// </summary>
        /// <param name="txt">String to convert to timespan instance.</param>
        /// <returns>Timespan instance converted from string.</returns>
        public static TimeSpan ToTime(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return TimeSpan.MinValue;

            string trimmed = txt.Trim().ToLower();
            return TimeHelper.Parse(trimmed).Item;
        }


        /// <summary>
        /// Converts a string to a datetime instance.
        /// </summary>
        /// <param name="txt">String to conevert to datetime instance.</param>
        /// <returns>Datetime instance converted from string.</returns>
        public static object ToDateTimeObject(this string txt)
        {
            return ToDateTime(txt) as object;
        }


        /// <summary>
        /// Converts a string to a datetime instance.
        /// </summary>
        /// <param name="txt">String to convert to datetime instance.</param>
        /// <returns>Datetime instance converted from string.</returns>
        public static DateTime ToDateTime(this string txt)
        {
            if (string.IsNullOrEmpty(txt))
                return DateTime.MinValue;

            string trimmed = txt.Trim().ToLower();
            if (trimmed.StartsWith("$"))
            {
                if (trimmed == "${today}") return DateTime.Today;
                if (trimmed == "${yesterday}") return DateTime.Today.AddDays(-1);
                if (trimmed == "${tommorrow}") return DateTime.Today.AddDays(1);
                if (trimmed == "${t}") return DateTime.Today;
                if (trimmed == "${t-1}") return DateTime.Today.AddDays(-1);
                if (trimmed == "${t+1}") return DateTime.Today.AddDays(1);
                if (trimmed == "${today+1}") return DateTime.Today.AddDays(1);
                if (trimmed == "${today-1}") return DateTime.Today.AddDays(-1);

                // Handles ${t+4} or ${t-9}
                string internalVal = trimmed.Substring(2, (trimmed.Length -1 ) - 2);
                DateTime result = DateParser.ParseTPlusMinusX(internalVal);
                return result;
            }
            DateTime parsed = DateTime.Parse(trimmed);
            return parsed;
        }
        #endregion


        #region Hex and Binary
        /// <summary>
        /// Determines whether the string contains valid hexadecimal characters only.
        /// </summary>
        /// <param name="txt">String to check.</param>
        /// <returns>True if the string contains valid hexadecimal characters.</returns>
        /// <remarks>An empty or null string is considered to <b>not</b> contain
        /// valid hexadecimal characters.</remarks>
        public static bool IsHex(this string txt)
        {
            return (!txt.IsNullOrEmpty()) && (txt.ReplaceChars("0123456789ABCDEFabcdef", "                      ").Trim().IsNullOrEmpty());
        }


        /// <summary>
        /// Determines whether the string contains valid binary characters only.
        /// </summary>
        /// <param name="txt">String to check.</param>
        /// <returns>True if the string contains valid binary characters.</returns>
        /// <remarks>An empty or null string is considered to <b>not</b> contain
        /// valid binary characters.</remarks>
        public static bool IsBinary(this string txt)
        {
            return (!txt.IsNullOrEmpty()) && (txt.ReplaceChars("01", "  ").Trim().IsNullOrEmpty());
        }

        /// <summary>
        /// Returns the hexadecimal representation of a decimal number.
        /// </summary>
        /// <param name="txt">Hexadecimal string to convert to decimal.</param>
        /// <returns>Decimal representation of string.</returns>
        public static string DecimalToHex(this string txt)
        {
            return Convert.ToInt32(txt).ToHex();
        }


        /// <summary>
        /// Returns the binary representation of a binary number.
        /// </summary>
        /// <param name="txt">Decimal string to convert to binary.</param>
        /// <returns>Binary representation of string.</returns>
        public static string DecimalToBinary(this string txt)
        {
            return Convert.ToInt32(txt).ToBinary();
        }


        /// <summary>
        /// Returns the decimal representation of a hexadecimal number.
        /// </summary>
        /// <param name="txt">Hexadecimal string to convert to decimal.</param>
        /// <returns>Decimal representation of string.</returns>
        public static string HexToDecimal(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 16));
        }


        /// <summary>
        /// Returns the binary representation of a hexadecimal number.
        /// </summary>
        /// <param name="txt">Binary string to convert to hexadecimal.</param>
        /// <returns>Hexadecimal representation of string.</returns>
        public static string HexToBinary(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 16), 2);
        }


        /// <summary>
        /// Converts a hexadecimal string to a byte array representation.
        /// </summary>
        /// <param name="txt">Hexadecimal string to convert to byte array.</param>
        /// <returns>Byte array representation of the string.</returns>
        /// <remarks>The string is assumed to be of even size.</remarks>
        public static byte[] HexToByteArray(this string txt)
        {
            byte[] b = new byte[txt.Length / 2];
            for (int i = 0; i < txt.Length; i += 2)
            {
                b[i / 2] = Convert.ToByte(txt.Substring(i, 2), 16);
            }
            return b;
        }


        /// <summary>
        /// Converts a byte array to a hexadecimal string representation.
        /// </summary>
        /// <param name="b">Byte array to convert to hexadecimal string.</param>
        /// <returns>String representation of byte array.</returns>
        public static string ByteArrayToHex(this byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "");
        }


        /// <summary>
        /// Returns the hexadecimal representation of a binary number.
        /// </summary>
        /// <param name="txt">Binary string to convert to hexadecimal.</param>
        /// <returns>Hexadecimal representation of string.</returns>
        public static string BinaryToHex(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 2), 16);
        }


        /// <summary>
        /// Returns the decimal representation of a binary number.
        /// </summary>
        /// <param name="txt">Binary string to convert to decimal.</param>
        /// <returns>Decimal representation of string.</returns>
        public static string BinaryToDecimal(this string txt)
        {
            return Convert.ToString(Convert.ToInt32(txt, 2));
        }
        #endregion


        #region Replacement
        /// <summary>
        /// Replaces the characters in the originalChars string with the
        /// corresponding characters of the newChars string.
        /// </summary>
        /// <param name="txt">String to operate on.</param>
        /// <param name="originalChars">String with original characters.</param>
        /// <param name="newChars">String with replacement characters.</param>
        /// <example>For an original string equal to "123456654321" and originalChars="35" and
        /// newChars "AB", the result will be "12A4B66B4A21".</example>
        /// <returns>String with replaced characters.</returns>
        public static string ReplaceChars(this string txt, string originalChars, string newChars)
        {
            string returned = "";

            for (int i = 0; i < txt.Length; i++)
            {
                int pos = originalChars.IndexOf(txt.Substring(i, 1));
                
                if (-1 != pos)
                    returned += newChars.Substring(pos, 1);
                else
                    returned += txt.Substring(i, 1);
            }
            return returned;
        }
        #endregion


        #region Lists
        /// <summary>
        /// Prefixes all items in the list w/ the prefix value.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>List with prefixes.</returns>
        public static List<string> PreFixWith(this List<string> items, string prefix)
        {
            for(int ndx = 0; ndx < items.Count; ndx++)
            {
                items[ndx] = prefix + items[ndx];
            }
            return items;
        }
        #endregion


        #region Matching
        /// <summary>
        /// Determines whether or not the string value supplied represents a "not applicable" string value by matching on na, n.a., n/a etc.
        /// </summary>
        /// <param name="val">String to check.</param>
        /// <param name="useNullOrEmptyStringAsNotApplicable">True to use null or empty string check.</param>
        /// <returns>True if the string value represents a "not applicable" string.</returns>
        public static bool IsNotApplicableValue(this string val, bool useNullOrEmptyStringAsNotApplicable = false)
        {
            bool isEmpty = string.IsNullOrEmpty(val);
            if(isEmpty && useNullOrEmptyStringAsNotApplicable) return true;
            if(isEmpty && !useNullOrEmptyStringAsNotApplicable) return false;
            val = val.Trim().ToLower();

            if (val == "na" || val == "n.a." || val == "n/a" || val == "n\\a" || val == "n.a" || val == "not applicable")
                return true;
            return false;
        }


        /// <summary>
        /// Use the Levenshtein algorithm to determine the similarity between
        /// two strings. The higher the number, the more different the two
        /// strings are.
        /// TODO: This method needs to be rewritten to handle very large strings
        /// See <a href="http://www.merriampark.com/ld.htm"></a>.
        /// See <a href="http://en.wikipedia.org/wiki/Levenshtein_distance"></a>.
        /// </summary>
        /// <param name="source">Source string to compare</param>
        /// <param name="comparison">Comparison string</param>
        /// <returns>0 if both strings are identical, otherwise a number indicating the level of difference</returns>
        public static int Levenshtein(this string source, string comparison)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Can't parse null string");
            }
            if (comparison == null)
            {
                throw new ArgumentNullException("comparison", "Can't parse null string");
            }

            var s = source.ToCharArray();
            var t = comparison.ToCharArray();
            var n = source.Length;
            var m = comparison.Length;
            var d = new int[n + 1, m + 1];

            // shortcut calculation for zero-length strings
            if (n == 0) { return m; }
            if (m == 0) { return n; }

            for (var i = 0; i <= n; d[i, 0] = i++) {}
            for (var j = 0; j <= m; d[0, j] = j++) {}

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var cost = t[j - 1].Equals(s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,
                        d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }


        /// <summary>
        /// Calculate the simplified soundex value for the specified string.
        /// See <a href="http://en.wikipedia.org/wiki/Soundex"></a>.
        /// See <a href="http://west-penwith.org.uk/misc/soundex.htm"></a>.
        /// </summary>
        /// <param name="source">String to calculate</param>
        /// <returns>Soundex value of string</returns>
        public static string SimplifiedSoundex(this string source)
        {
            return source.SimplifiedSoundex(4);
        }

        /// <summary>
        /// Calculate the simplified soundex value for the specified string.
        /// See <a href="http://en.wikipedia.org/wiki/Soundex"></a>.
        /// See <a href="http://west-penwith.org.uk/misc/soundex.htm"></a>.
        /// </summary>
        /// <param name="source">String to calculate</param>
        /// <param name="length">Length of soundex value (typically 4)</param>
        /// <returns>Soundex value of string</returns>
        public static string SimplifiedSoundex(this string source, int length)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (source.Length < 3)
            {
                throw new ArgumentException(
                    "Source string must be at least two characters", "source");
            }

            var t = source.ToUpper().ToCharArray();
            var buffer = new StringBuilder();

            short prev = -1;

            foreach (var c in t)
            {
                short curr = 0;
                switch (c)
                {
                    case 'A':
                    case 'E':
                    case 'I':
                    case 'O':
                    case 'U':
                    case 'H':
                    case 'W':
                    case 'Y':
                        curr = 0;
                        break;
                    case 'B':
                    case 'F':
                    case 'P':
                    case 'V':
                        curr = 1;
                        break;
                    case 'C':
                    case 'G':
                    case 'J':
                    case 'K':
                    case 'Q':
                    case 'S':
                    case 'X':
                    case 'Z':
                        curr = 2;
                        break;
                    case 'D':
                    case 'T':
                        curr = 3;
                        break;
                    case 'L':
                        curr = 4;
                        break;
                    case 'M':
                    case 'N':
                        curr = 5;
                        break;
                    case 'R':
                        curr = 6;
                        break;
                    default:
                        throw new ApplicationException(
                            "Invalid state in switch statement");
                }

                /* Change all consecutive duplicate digits to a single digit
                 * by not processing duplicate values. 
                 * Ignore vowels (i.e. zeros). */
                if (curr != prev)
                {
                    buffer.Append(curr);
                }

                prev = curr;
            }

            // Prefix value with first character
            buffer.Remove(0, 1).Insert(0, t.First());
            
            // Remove all vowels (i.e. zeros) from value
            buffer.Replace("0", "");
            
            // Pad soundex value with zeros until output string equals length))))
            while (buffer.Length < length) { buffer.Append('0'); }
            
            // Truncate values that are longer than the supplied length
            return buffer.ToString().Substring(0, length);
        }

        #endregion
    }
}
