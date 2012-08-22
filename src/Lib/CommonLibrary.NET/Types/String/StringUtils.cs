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
using System.IO;
using System.Linq;
using System.Collections.Specialized;
using System.Collections;
using ComLib;
using ComLib.Collections;

namespace ComLib
{
   
    /// <summary>
    /// String helper methods.
    /// </summary>
    public class StringHelpers
    {
        /// <summary>
        /// Read all the lines in the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IList<string> ReadLines(string text)
        {
            // Check for empty and return empty list.
            if (string.IsNullOrEmpty(text))
                return new List<string>();

            StringReader reader = new StringReader(text);
            string currentLine = reader.ReadLine();
            IList<string> lines = new List<string>();

            // More to read.
            while (currentLine != null)
            {
                lines.Add(currentLine);
                currentLine = reader.ReadLine();
            }
            return lines;
        }


        /// <summary>
        /// Get delimited chars from a string.
        /// </summary>
        /// <param name="rawText">search-classes-workshops-4-1-1-6</param>
        /// <param name="excludeText">search-classes-workshops</param>
        /// <param name="delimiter">-</param>
        /// <returns></returns>
        public static string[] GetDelimitedChars(string rawText, string excludeText, char delimiter)
        {
            int indexOfDelimitedData = rawText.IndexOf(excludeText);
            string delimitedData = rawText.Substring(indexOfDelimitedData + excludeText.Length);
            string[] separatedChars = delimitedData.Split(delimiter);
            return separatedChars;
        }


        /// <summary>
        /// Truncates the string.
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string Truncate(string txt, int maxChars)
        {
            if(string.IsNullOrEmpty(txt) )
                return txt;
            
            if(txt.Length <= maxChars)
                return txt;
            
            return txt.Substring(0, maxChars);
        }


        /// <summary>
        /// Truncate the text supplied by number of characters specified by <paramref name="maxChars"/>
        /// and then appends the suffix.
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="maxChars"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string TruncateWithText(string txt, int maxChars, string suffix)
        {
            if (string.IsNullOrEmpty(txt))
                return txt;

            if (txt.Length <= maxChars)
                return txt;

            // Now do the truncate and more.
            string partial = txt.Substring(0, maxChars);
            return partial + suffix;
        }


        /// <summary>
        /// Join string enumeration items.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string Join(IList<string> items, string delimeter)
        {
            string joined = "";
            int ndx;
            for (ndx = 0; ndx < items.Count - 2; ndx++)
            {
                joined += items[ndx] + delimeter;
            }
            joined += items[ndx];
            return joined;
        }


        /// <summary>
        /// If null returns empty string.
        /// Else, returns original.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetOriginalOrEmptyString(string text)
        {
            if (text == null) { return string.Empty; }
            return text;
        }


        /// <summary>
        /// Returns the defaultval if the val string is null or empty.
        /// Returns the val string otherwise.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        public static string GetDefaultStringIfEmpty(string val, string defaultVal)
        {
            if (string.IsNullOrEmpty(val)) return defaultVal;

            return val;
        }        


        /// <summary>
        /// Convert the word(s) in the sentence to sentence case.
        /// UPPER = Upper
        /// lower = Lower
        /// MiXEd = Mixed
        /// </summary>
        /// <param name="s"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ConvertToSentanceCase(string s, char delimiter)
        {
            // Check null/empty
            if (string.IsNullOrEmpty(s))
                return s;

            s = s.Trim();
            if (string.IsNullOrEmpty(s))
                return s;

            // Only 1 token
            if (s.IndexOf(delimiter) < 0)
            {
                s = s.ToLower();
                s = s[0].ToString().ToUpper() + s.Substring(1);
                return s;
            }

            // More than 1 token.
            string[] tokens = s.Split(delimiter);
            StringBuilder buffer = new StringBuilder();

            foreach (string token in tokens)
            {
                string currentToken = token.ToLower();
                currentToken = currentToken[0].ToString().ToUpper() + currentToken.Substring(1);
                buffer.Append(currentToken + delimiter);
            }

            s = buffer.ToString();
            return s.TrimEnd(delimiter);
        }


        /// <summary>
        /// Get the index of a spacer ( space" " or newline )
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        public static int GetIndexOfSpacer(string txt, int currentPosition, ref bool isNewLine)
        {
            // Take the first spacer that you find. it could be eithr
            // space or newline, if space is before the newline take space
            // otherwise newline.            
            int ndxSpace = txt.IndexOf(" ", currentPosition);
            int ndxNewLine = txt.IndexOf(Environment.NewLine, currentPosition);
            bool hasSpace = ndxSpace > -1;
            bool hasNewLine = ndxNewLine > -1;
            isNewLine = false;

            // Found both space and newline.
            if (hasSpace && hasNewLine)
            {
                if (ndxSpace < ndxNewLine) { return ndxSpace; }
                isNewLine = true;
                return ndxNewLine;
            }

            // Found space only.
            if (hasSpace && !hasNewLine) { return ndxSpace; }

            // Found newline only.
            if (!hasSpace && hasNewLine) { isNewLine = true; return ndxNewLine; }

            // no space or newline.
            return -1;
        }


        /// <summary>
        /// Convert boolean value to "Yes" or "No"
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ConvertBoolToYesNo(bool b)
        {
            if (b) { return "Yes"; }

            return "No";
        }


        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static string ConvertToString(object[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            StringBuilder buffer = new StringBuilder();
            foreach (object arg in args)
            {
                if (arg != null)
                    buffer.Append(arg.ToString());
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Parses a delimited list of items into a readonly dictionary.
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static DictionaryReadOnly<string, string> ToMapReadonly(string delimitedText, char delimeter)
        {
            return new DictionaryReadOnly<string, string>(ToMap(delimitedText, delimeter));
        }

        /// <summary>
        /// Convert to delimited text to a dictionary.
        /// </summary>
        /// <param name="delimitedText">"1,2,3,4,5"</param>
        /// <param name="delimeter">','</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToMap(string delimitedText, char delimeter)
        {
            IDictionary<string, string> items = new Dictionary<string, string>();
            string[] tokens = delimitedText.Split(delimeter);

            // Check
            if (tokens == null) return new Dictionary<string, string>(items);

            foreach (string token in tokens)
            {
                items[token] = token;
            }
            return new Dictionary<string, string>(items);
        }


        /// <summary>
        /// Map a set of keyvalue pairs to a dictionary.
        /// </summary>
        /// <param name="delimitedText">e.g. "city=Queens, state=Ny, zipcode=12345, country=usa"</param>
        /// <param name="keyValuePairDelimiter">","</param>
        /// <param name="keyValueDelimeter">"="</param>
        /// <param name="makeKeysCaseSensitive">Flag to make the keys case insensitive, which
        /// converts the keys to lowercase if set to true.</param>
        /// <param name="trimValues">Flag to trim the values in the key-value pairs.</param>
        /// <returns></returns>
        public static IDictionary<string, string> ToMap(string delimitedText, char keyValuePairDelimiter, char keyValueDelimeter,
            bool makeKeysCaseSensitive, bool trimValues)
        {
            IDictionary<string, string> map = new Dictionary<string, string>();
            string[] tokens = delimitedText.Split(keyValuePairDelimiter);

            // Check
            if (tokens == null) return map;

            // Each pair
            foreach (string token in tokens)
            {
                // Split city=Queens to "city", "queens"
                string[] pair = token.Split(keyValueDelimeter);

                string key = pair[0];
                string value = pair[1];

                if (makeKeysCaseSensitive)
                {
                    key = key.ToLower();
                }
                if (trimValues)
                {
                    key = key.Trim();
                    value = value.Trim();
                }
                map[key] = value;
            }
            return map;
        }


        /// <summary>
        /// Parses a delimited list of items into a string[].
        /// </summary>
        /// <param name="delimitedText">"1,2,3,4,5,6"</param>
        /// <param name="delimeter">','</param>
        /// <returns></returns>
        public static string[] ToStringArray(string delimitedText, char delimeter)
        {
            if (string.IsNullOrEmpty(delimitedText))
                return null;

            string[] tokens = delimitedText.Split(delimeter);
            return tokens;
        }


        /// <summary>
        /// Substitute the placeholders ${name} where name is the key in <paramref name="subsitutions"/>
        /// and replace it with the value associated with the key.
        /// </summary>
        /// <param name="subsitutions">The subsitutions.</param>
        /// <param name="contentPlaceholders">The content placeholders.</param>
        /// <returns></returns>
        public static string Substitute(IDictionary<string, string> subsitutions, string contentPlaceholders)
        {
            if (string.IsNullOrEmpty(contentPlaceholders))
                return contentPlaceholders;

            if (subsitutions == null || subsitutions.Count == 0)
                return contentPlaceholders;

            string replacedValues = contentPlaceholders;
            subsitutions.ForEach<KeyValuePair<string, string>>(kv => replacedValues = replacedValues.Replace("${" + kv.Key + "}", kv.Value));
            
            return replacedValues;
        }


        /// <summary>
        /// Finds the maximum length of a key and padd all the rest of the keys to be 
        /// the same fixed length, and calls the delegate supplied.
        /// </summary>
        /// <param name="keyValues">The key value pairs.</param>
        /// <param name="extraPadding">Additional number of chars to pad to the keys.</param>
        /// <param name="printer">The delegate to call to print to.</param>
        public static void DoFixedLengthPrinting(IDictionary keyValues, int extraPadding,  Action<string, object> printer)
        {
            // Get the length of the longest named argument.
            int maxLength = 0;
            foreach (DictionaryEntry entry in keyValues)
            {
                int keyLen = ((string)entry.Key).Length;
                if (keyLen > maxLength)
                    maxLength = keyLen;
            }
            maxLength += extraPadding;

            // Iterate through all the keys and build a fixed length key.
            // e.g. if key is 4 chars and maxLength is 6, add 2 chars using space(' ').
            foreach (DictionaryEntry entry in keyValues)
            {
                string newKeyWithPadding = GetFixedLengthString((string)entry.Key, maxLength, " ");
                printer(newKeyWithPadding, entry.Value);
            }
        }


        /// <summary>
        /// Builds a fixed length string with the maxChars provided.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxChars"></param>
        /// <returns></returns>
        public static string GetFixedLengthString(string text, int maxChars, string paddingChar)
        {
            int leftOver = maxChars - text.Length;
            string finalText = text;
            for (int ndx = 0; ndx < leftOver; ndx++)
                finalText += paddingChar;
            return finalText;
        }

        #region LineSeparator Conversions
        /// <summary>
        /// A liberal list of line breaking characters used in unicode. Typically,
        /// LF and CR are the only characters supported in C#.
        /// </summary>
        public static readonly char[] LineBreakingCharacters = new char[]
        {
            '\x000a', // LF \n
            '\x000d', // CR \r
            '\x000c', // FF (form feed)
            '\x2028', // LS (unicode line separator)
            '\x2029', // paragraph separator
            '\x0085', // NEL (next line)
        };

        /// <summary>
        /// DOS/Windows style line breaks (CR+LF)
        /// </summary>
        public static readonly char[] DosLineSeparator = { '\x000d', '\x000a' };
        /// <summary>
        /// Unix style line breaks (LF)
        /// </summary>
        public static readonly char[] UnixLineSeparator = { '\x000a' };
        /// <summary>
        /// Commodore, TRS-80, Apple II, Apple MacOS9 style line breaks (CR)
        /// </summary>
        public static readonly char[] MacOs9Separator = { '\x000d' };
        /// <summary>
        /// Unicode line separator - not widely supported
        /// </summary>
        public static readonly char[] UnicodeSeparator = {'\x2028'};

        /// <summary>
        /// Converts from a liberal list of unicode line separators to the
        /// specified line separator.
        /// </summary>
        /// <param name="reader">TextReader to read from</param>
        /// <param name="writer">TextReader to write to</param>
        /// <param name="separator">Line break separator.</param>
        public static void ConvertLineSeparators(TextReader reader,
            TextWriter writer, char[] separator)
        {
            for (var c = reader.Read(); c != -1; c = reader.Read())
            {
                // One new line
                if (LineBreakingCharacters.Contains((char)c))
                {
                    // If a windows style new line, skip the next char
                    if (c == '\r' && reader.Peek() == '\n')
                    {
                        reader.Read();
                    }

                    writer.Write(separator);
                    continue;
                }
                writer.Write((char)c);
            }
        }

        /// <summary>
        /// Converts from a liberal list of unicode line separators to the
        /// specified line separator.
        /// </summary>
        /// <example>
        /// // Convert line breaks to current environment's default
        /// var text = "blah blah...";
        /// ConvertLineSeparators(text, Environment.NewLine.ToCharArray());
        /// </example>
        /// <param name="text">Source text</param>
        /// <param name="separator">Line break separator.</param>
        /// <returns>String with normalized line separators</returns>
        public static String ConvertLineSeparators(String text, char[] separator)
        {
            var reader = new StringReader(text);
            var writer = new StringWriter();
            ConvertLineSeparators(reader, writer, separator);
            return writer.ToString();
        }

        #endregion
    }
}
