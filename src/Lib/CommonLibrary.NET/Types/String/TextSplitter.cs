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



namespace ComLib.Types
{
    /// <summary>
    /// Helper class to split a long word into a single one.
    /// Alternative to possibly using Regular expression.
    /// </summary>
    public class TextSplitter
    {
        /// <summary>
        /// Check the single line of text for long word that exceeds the
        /// maximum allowed.
        /// If found, splits the word.
        /// </summary>
        /// <param name="text">Line of text.</param>
        /// <param name="maxCharsInWord">Maximum characters allowed in a word.</param>
        /// <returns>Splitted text.</returns>
        public static string CheckAndSplitText(string text, int maxCharsInWord)
        {
            // Validate.
            if (string.IsNullOrEmpty(text)) return text;            

            bool isSpacerNewLine = false; 
            int currentPosition = 0;
            int ndxSpace = StringHelper.GetIndexOfSpacer(text, currentPosition, ref isSpacerNewLine);

            // Case 1: Single long word.
            if (ndxSpace < 0 && text.Length > maxCharsInWord) return SplitWord(text, maxCharsInWord, " ");

            StringBuilder buffer = new StringBuilder();           

            // Now go through all the text and check word and split.
            while ((currentPosition < text.Length && ndxSpace > 0))
            {
                //Lenght of word 
                int wordLength = ndxSpace - (currentPosition );
                string currentWord = text.Substring(currentPosition, wordLength);
                string spacer = isSpacerNewLine ? Environment.NewLine : " ";

                if (wordLength > maxCharsInWord)
                {
                    string splitWord = SplitWord(currentWord, maxCharsInWord, " ");
                    buffer.Append(splitWord + spacer);
                }
                else
                {
                    buffer.Append(currentWord + spacer);
                }

                currentPosition = (isSpacerNewLine) ? ndxSpace + 2 : ndxSpace + 1;
                ndxSpace = StringHelper.GetIndexOfSpacer(text, (currentPosition), ref isSpacerNewLine);
            }

            // Final check.. no space found but check complete length now.
            if (currentPosition < text.Length && ndxSpace < 0)
            {
                //Lenght of word 
                int wordLength = (text.Length) - currentPosition;
                string currentWord = text.Substring(currentPosition, wordLength);
                string spacer = isSpacerNewLine ? Environment.NewLine : " ";

                if (wordLength > maxCharsInWord)
                {
                    string splitWord = SplitWord(currentWord, maxCharsInWord, " ");
                    buffer.Append(splitWord);
                }
                else
                {
                    buffer.Append(currentWord);
                }
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Split the word, N number of times.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="charsPerWord">40 chars in each word.</param>
        /// <param name="spacer">" "</param>
        /// <returns>Splitted word.</returns>
        internal static string SplitWord(string text, int charsPerWord, string spacer)
        {
            // Validate.
            if (string.IsNullOrEmpty(text)) { return text; }

            // Determine how many times we have to split.
            int splitCount = GetNumberOfTimesToSplit(text.Length, charsPerWord);

            // Validate.
            if (splitCount == 0) return text;

            // Use buffer instead of string concatenation.
            StringBuilder buffer = new StringBuilder();
            int currentPosition = 0;

            // Split N number of times.
            for (int count = 1; count <= splitCount; count++)
            {
                string word = (count < splitCount) ? text.Substring(currentPosition, charsPerWord) : text.Substring(currentPosition);
                
                buffer.Append(word);

                // Condition to prevent adding spacer at the end.
                // This is to leave the supplied text the same except for splitting ofcourse.
                if (count < splitCount) buffer.Append(spacer);

                // Move to next split start position.
                currentPosition += charsPerWord;
            }

            return buffer.ToString();
        }


        /// <summary>
        /// Determine how many times the word has to be split.
        /// </summary>
        /// <param name="wordLength">Length of word.</param>
        /// <param name="maxCharsInWord">Maximum characters in word.</param>
        /// <returns>Number of times to split the word.</returns>
        internal static int GetNumberOfTimesToSplit(int wordLength, int maxCharsInWord)
        {
            // Validate.
            if (wordLength <= maxCharsInWord) return 0;

            // Now calc.
            int splitCount = wordLength / maxCharsInWord;
            int leftOver = wordLength % maxCharsInWord;

            if (leftOver > 0) splitCount++;

            return splitCount;
        }        
    }
}
