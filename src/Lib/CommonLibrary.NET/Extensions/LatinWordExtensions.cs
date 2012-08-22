using System;
using System.Linq;
using System.Text;

namespace ComLib.Extensions
{
    /// <summary>
    /// Collection of extension utility methods useful for processing latin
    /// text.
    /// </summary>
    public static class LatinWordExtensions
    {
        /// <summary>
        /// A very liberal list of whitespace characters used in unicode.
        /// Char.isWhitespace() only checks for latin1 values, this list is
        /// more comprehensive and should not be used unintelligently for
        /// stripping whitespace because it may make the target language
        /// unreadable.
        /// </summary>
        public static readonly char[] WhiteSpaceCharacters = new char[]
        {
            '\x0020', // standard space
            '\x0009', // horizontal tab
            '\x000a', // LF
            '\x000b', // line tabulation
            '\x000c', // FF (form feed)
            '\x000d', // CR
            '\x00a0', // non-breaking space
            '\x0085', // NEL (next line)
            '\x1680', // OGHAM space mark
            '\x2000', // En quad
            '\x2001', // Em quad
            '\x2002', // En space
            '\x2003', // Em space
            '\x2004', // three-per-em space
            '\x2005', // four-per-em space
            '\x2006', // six-per-em space
            '\x2007', // figure space
            '\x2008', // punctuation space
            '\x2009', // thin space
            '\x200a', // hair space
            '\x200b', // zero width space
            '\x2028', // LS (unicode line separator)
            '\x2029', // paragraph separator
            '\x202f', // narrow no-break space
            '\x205f', // medium mathematical space
            '\x3000', // ideographic space (CJKV)
        };

        /// <summary>
        /// Small list of english words that commonly excluded from abbreviations
        /// and initials.
        /// </summary>
        public static readonly string[] EnglishInitialsExclusionWords = new string[] 
        { 
            "a", "an", "as", "at", "by", "for", "from", "in", "into", "of", "on",
            "to", "the", "with"
        };

        /// <summary>
        /// Determines whether the specified char is delimiter.
        /// </summary>
        /// <param name="c">Character to match</param>
        /// <param name="delimiters">A array of delimiters or null for default whitespace lookup</param>
        /// <returns>
        /// 	<c>true</c> if the specified char is delimiter; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsDelimiter(char c, char[] delimiters)
        {
            // if no delimators are provided, use unicode list
            if (delimiters == null)
            {
                delimiters = WhiteSpaceCharacters;
            }

            var size = delimiters.Length;
            
            for (var i=0; i < size; i++)
            {
                if (c.Equals(delimiters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates initials from the first characters in each word (as separated
        /// by whitespace) in the specified phrase.
        /// </summary>
        /// <example>
        /// <code>
        /// var initials = "end of file".Initials(); // "eof"
        /// </code>
        /// </example>
        /// <param name="phrase">Phrase to initialize</param>
        /// <returns>string containing the first initials of the specified phrase</returns>
		public static string Initials(this string phrase)
		{
			return phrase.Initials(WhiteSpaceCharacters, null);
		}

        /// <summary>
        /// Creates initials from the first characters in each word (as separated
        /// by delimiters) in the specified phrase.
        /// </summary>
        /// <example>
        /// <code>
        /// var initials = "end,of,file".Initials(new char[] {','}); // "eof"
        /// </code>
        /// </example>
        /// <param name="phrase">Phrase to initialize</param>
        /// <param name="delimiters">Array of characters used to delimite words, if null then default whitespace matching is used.</param>
        /// <returns>string containing the first initials of the specified phrase</returns>
		public static string Initials(this string phrase, char[] delimiters)
		{
			return phrase.Initials(delimiters, null);
		}
				
        /// <summary>
        /// Creates initials from the first characters in each word (as separated
        /// by delimiters) in the specified phrase.
        /// </summary>
        /// <example>
        /// <code>
        /// var initials = "end of file".Initials(null, new string[] {"of"}); // ef
        /// </code>
        /// </example>
        /// <param name="phrase">Phrase to initialize</param>
        /// <param name="delimiters">Array of characters used to delimite words, if null then default whitespace matching is used.</param>
        /// <param name="exclusions">Words to excluded from initials (e.g. 'the', 'an', etc)</param>
        /// <returns>string containing the first initials of the specified phrase</returns>
        public static string Initials(this string phrase, char[] delimiters,
            string[] exclusions)
        {
            if (phrase == null)
            {
                var msg = "Can't process null string";
                throw new ArgumentNullException("phrase", msg);
            }
            // Use the default value for delimiters - whitespace
            if (delimiters == null)
            {
                delimiters = WhiteSpaceCharacters;
            }

            var buffer = new StringBuilder();

            foreach (string word in phrase.Split(delimiters))
            {
                // Skip all excluded words
                if (exclusions != null && exclusions.Contains(word)) { continue; }
                // Append the first character of every word to the buffer
                if (word.Length > 0) { buffer.Append(word[0]); }
            }

            // remove all excluded words

            return buffer.ToString();
        }

        /// <summary>
        /// Word wraps string to specified line length without justification.
        /// Words that exceed line length will not have line break added.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="lineLength">Number of characters in line</param>
        /// <returns>Word wrapped string</returns>
        public static string Wrap(this string text, int lineLength)
		{
			return text.Wrap(lineLength, null);
		}
		
        /// <summary>
        /// Word wraps string to specified line length without justification.
        /// Words that exceed line length will not have line break added.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="lineLength">Number of characters in line</param>
        /// <param name="newLineMarker">Character to use for new lines, if null Environment.NewLine</param>
        /// <returns>Word wrapped string</returns>
        public static string Wrap(this string text, int lineLength, 
            string newLineMarker)
        {
            if (text == null)
            {
                var msg = "Can't process null string";
                throw new ArgumentNullException("text", msg);
            }

            // Don't wrap when line length is less than or equal to zero
            if (lineLength <= 0)
            {
                return text;
            }

            // Use default line breaking scheme if parameter is blank
            if (newLineMarker == null)
            {
                newLineMarker = Environment.NewLine;
            }

            var lastWhitespacePos = 0;
            var capacity = (int)Math.Round(text.Length*1.2m);
            var buffer = new StringBuilder(capacity);

            var size = text.Length;
            for (var i = 0; i < size; i++)
            {
                var c = text[i];
                var pos = buffer.Length;

                if (IsDelimiter(c, WhiteSpaceCharacters))
                {
                    lastWhitespacePos = pos;
                }

                buffer.Append(c);

                // Add line break if when we are at border
                if ( (pos > 0 && pos % lineLength == 0))
                {
                    buffer.Remove(lastWhitespacePos, 1);
                    buffer.Insert(lastWhitespacePos, newLineMarker);
                }
            }

            // Test to see if the last line exceeds our border
            if (text.Length % lineLength > 0)
            {
                buffer.Remove(lastWhitespacePos, 1);
                buffer.Insert(lastWhitespacePos, newLineMarker);
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Normalizes the whitespace in a string by transforming all whitespace
        /// characters that repeat in succession to a single default whitespace
        /// character.
        /// </summary>
        /// <param name="text">The text to normalize</param>
        /// <returns>Normalized string</returns>
		public static string NormalizeWhitespace(this string text)
		{
            return text.NormalizeWhitespace(' ', WhiteSpaceCharacters);
		}

        /// <summary>
        /// Normalizes the whitespace in a string by transforming all whitespace
        /// characters that repeat in succession to a single default whitespace
        /// character.
        /// </summary>
        /// <param name="text">The text to normalize</param>
        /// <param name="whitespaceChar">The whitespace character to replace all other whitespace with</param>
        public static string NormalizeWhitespace(this string text,
            char whitespaceChar)
        {
            return text.NormalizeWhitespace(whitespaceChar, WhiteSpaceCharacters);
        }

        /// <summary>
        /// Normalizes the whitespace in a string by transforming all whitespace
        /// characters that repeat in succession to a single default whitespace
        /// character.
        /// </summary>
        /// <param name="text">The text to normalize</param>
        /// <param name="whitespaceChar">The whitespace character to replace all other whitespace with</param>
        /// <param name="whitespace">Array of whitespace characters to match</param>
        /// <returns>Normalized string</returns>
		public static string NormalizeWhitespace(this string text, 
            char whitespaceChar, char[] whitespace)
        {
            if (text == null)
            {
                var msg = "Can't process null string";
                throw new ArgumentNullException("text", msg);
            }

            var buffer = new StringBuilder(text.Length);
            var wasWhitespace = false;
            foreach (var c in text.ToCharArray())
            {
                if (IsDelimiter(c, whitespace))
                {
                    if (!wasWhitespace)
                    {
                        buffer.Append(whitespaceChar);
                    }
                    wasWhitespace = true;
                }
                else
                {
                    buffer.Append(c);
                    wasWhitespace = false;
                }
            }

            return buffer.ToString();
        }
    }
}
