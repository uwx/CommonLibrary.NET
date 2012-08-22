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


namespace CommonLibrary
{
    /// <summary>
    /// Parser constants.
    /// </summary>
    public class IniParserConstants
    {
        public const string DoubleQuote = "\"";
        public const string SingleQuote = "'";
        public const string WhiteSpace = " ";
        public const string Escape = "\\";
        public const string Colon = ":";
        public const string BracketLeft = "[";
        public const string BracketRight = "]";
        public const string CommentChar = ";";
        public static string Eol = Environment.NewLine;
    }



    /// <summary>
    /// Settings for the parser.
    /// </summary>
    public class IniParserSettings
    {
        public int MaxLenghtOfComment = 500;
        public int MaxLengthOfGroup = 40;
        public int MaxLengthOfKey = 40;
        public int MaxLenghtOfValueSingleLine = 500;
    }



    /// <summary>
    /// The various states that the parser can be in.
    /// </summary>
    public enum IniParserState
    {
        None, Group, KeyValue, Comment
    };



    /// <summary>
    /// IniNode
    /// </summary>
    public class IniNode
    {
        public string Name;
        public IList<KeyValuePair<string, string>> Pairs = new List<KeyValuePair<string, string>>();
    }


    
    public interface IIniParser
    {
        /// <summary>
        /// Settings for the parser.
        /// </summary>
        IniParserSettings Settings { get; set; }

        /// <summary>
        /// Current char.
        /// </summary>
        string CurrentChar { get; }

        /// <summary>
        /// Get the index of the current char in the entire string.
        /// </summary>
        int CurrentCharIndex { get; }

        /// <summary>
        /// Read some token until the end-of-line.
        /// </summary>
        /// <returns></returns>
        string ReadTokenToEndOfLine();

        /// <summary>
        /// Consume any white space.
        /// </summary>
        void ConsumeWhiteSpace();

        /// <summary>
        /// Read the next character
        /// </summary>
        /// <returns></returns>
        char Read();

        /// <summary>
        /// Peek to see what the next character is.
        /// </summary>
        /// <returns></returns>
        char Peek();

        /// <summary>
        /// List of errors
        /// </summary>
        IList<string> Errors { get; }
    }



    /// <summary>
    /// Parser.
    /// Terms:
    /// 1. Char - a single char whether it's space, doublequote, singlequote, etc.
    /// 2. Token - a collection of chars that make up a valid word/word-boundary.
    ///     e.g.
    ///     1. abc, 
    ///     2. -format:csv
    ///     3. -file:"c:/my files/file1.txt"
    ///     4. loc:'c:/my files/file1.txt'
    ///     5. -format:csv
    /// </summary>
    public class IniParser : IIniParser
    {
        #region Private DataMembers
        private string _inputText = string.Empty;
        private StringReader _reader = null;
        private IniLineType _lastLineType = IniLineType.None;
        private char _nextChar;
        private int _nextCharInt;
        private char _currentChar;
        private int _currentCharInt;
        private int _currentCharIndex;
        private List<string> _errors = new List<string>();
        List<IniSection> _sections = new List<IniSection>();
        IniSection _currentSection = new IniSection(string.Empty, null);
        #endregion


        public IniParser()
        {
            Settings = new IniParserSettings();
        }


        private IniParserSettings _settings;
        public IniParserSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        #region IParser Members
        /// <summary>
        /// Parse the text and convert into list of params.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<IniSection> Parse(string text)
        {
            // Initialize for parsing.
            Init(text);

            // Validate the input text.
            if (!ValidateText().Success) return null;

            // Parse the text.
            Process();

            // Close any resources after parsing is complete.
            Close();

            return _sections;
        }


        /// <summary>
        /// The current char.
        /// </summary>
        public string CurrentChar
        {
            get { return _currentChar.ToString(); }
        }


        /// <summary>
        /// The index position of the current char.
        /// </summary>
        public int CurrentCharIndex
        {
            get { return _currentCharIndex; }
        }


        /// <summary>
        /// Peek at the next character.
        /// </summary>
        /// <returns></returns>
        public char Peek()
        {
            _nextCharInt = _reader.Peek();

            // Check for End of line.
            if (_nextCharInt == -1)
            {
                _nextChar = ' ';
                return _nextChar;
            }

            // Convert from char from int to char.
            string nextChar = Char.ConvertFromUtf32(_nextCharInt);
            _nextChar = nextChar[0];
            return _nextChar;
        }


        /// <summary>
        /// Reads/consumes the next char.
        /// </summary>
        public char Read()
        {
            _currentCharInt = _reader.Read();

            // Check for End of line.
            if (_currentCharInt == -1)
            {
                _currentChar = ' ';
                return _currentChar;
            }

            // Convert from char from int to char.
            string nextChar = Char.ConvertFromUtf32(_currentCharInt);
            _currentChar = nextChar[0];
            _currentCharIndex++;
            return _currentChar;
        }


        /// <summary>
        /// Read a single line value.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public string ReadTokenToEndOfLine()
        {
            StringBuilder buffer = new StringBuilder();

            string lastChar = _currentChar.ToString();
            string currentChar = this.Read().ToString();
            string combinedLast2Chars = lastChar + currentChar;

            // Handle check for empty value.
            if (combinedLast2Chars == Environment.NewLine) return string.Empty;

            // Append the last chars.
            buffer.Append(lastChar);

            bool parsedNewLine = false;

            // Read until "\r\n" endofline.
            while (combinedLast2Chars != IniParserConstants.Eol
                && _currentCharInt != -1)
            {                
                // Advance the parser to next char.
                lastChar = currentChar;
                currentChar = this.Read().ToString();
                combinedLast2Chars = lastChar + currentChar;

                // Check here to prevent \r being added to token.
                if (combinedLast2Chars == IniParserConstants.Eol)
                {
                    parsedNewLine = true;
                    break;
                }
                else
                    buffer.Append(lastChar);
            }

            // Error out if current char is not ":".
            if (!parsedNewLine && _currentCharInt != -1)
            {
                this.Errors.Add("Expected end of line at : " + this.CurrentCharIndex);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Consume/read the white space.
        /// </summary>
        public void ConsumeWhiteSpace()
        {
            while (_currentChar == ' ')
            {
                Read();
            }
        }


        /// <summary>
        /// Expect the next token to be the supplied token.
        /// </summary>
        /// <param name="token"></param>
        public void ExpectToken(string token)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        /// <summary>
        /// List of errors.
        /// </summary>
        public IList<string> Errors
        {
            get { return _errors; }
        }

        #endregion


        #region Private members
        private void Process()
        {
            // Move to first char.
            Read();

            // Consume End Of Line.
            ConsumeEndOfLine();

            // While not end of content.
            while (_currentCharInt != -1)
            {
                // Get the current char.
                string currentChar = _currentChar.ToString();

                if (currentChar == IniParserConstants.BracketLeft)
                {
                    StoreGroup();
                }
                else if (currentChar == IniParserConstants.CommentChar)
                {
                    StoreComment();
                }
                else if (IsEndOfLine())
                {
                    ConsumeEndOfLine();
                }
                else
                {
                    StoreKeyValue();   
                }

                // Read the next char.
                Read();

                // Check and consume End Of Line.
                ConsumeEndOfLine();
            }
        }


        private void StoreGroup()
        {
            // Push the last one into the list.
            if (_currentSection.Count > 0)
                _sections.Add(_currentSection);

            string group = TokenParser.ReadGroup(this);
            this.ReadTokenToEndOfLine();

            // Create a new section using the name of the group.
            _currentSection = new IniSection(group, null);
            _lastLineType = IniLineType.Group;
        }


        private void StoreKeyValue()
        {
            string key = TokenParser.ReadKey(this);
            string value = TokenParser.ReadValue(this);
            _currentSection.Add(new KeyValueItem<string, string>(key, value));
            _lastLineType = IniLineType.KeyValue;
        }


        private void StoreComment()
        {
            string comment = ReadTokenToEndOfLine();
            _lastLineType = IniLineType.Comment;
        }


        private bool IsEndOfLine()
        {
            string nextChar = Peek().ToString();
            string combinedChars = _currentChar.ToString() + nextChar;

            // If the first two chars are EndOfLine. Read past it.
            if (combinedChars == IniParserConstants.Eol)
            {
                return true;
            }
            return true;
        }


        private void ConsumeEndOfLine()
        {
            // Move past new line.
            this.Read();
            this.Read();
        }


        /// <summary>
        /// Confirm that the input text is valid text
        /// </summary>
        /// <returns></returns>
        private BoolMessage ValidateText()
        {
            string message = string.Empty;
            bool success = true;

            if (string.IsNullOrEmpty(_inputText))
            {
                success = false;
                message = "Empty String";
            }

            // Close resource if not valid so program can return.
            if (!success) { Close(); }

            return new BoolMessage(success, message);
        }


        /// <summary>
        /// Initialize the reader and the stack.
        /// </summary>
        /// <param name="text"></param>
        private void Init(string text)
        {
            _inputText = text;
            _reader = new StringReader(text);
        }


        /// <summary>
        /// Close the reader.
        /// </summary>
        private void Close()
        {
            _reader.Close();
        }
        #endregion
    }
    


    public class TokenParser
    {
        /// <summary>
        /// Read the IniGroup name.
        /// [group1]
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string ReadGroup(IIniParser parser)
        {
            StringBuilder buffer = new StringBuilder();
            
            // Exclude the "[" bracket from consuming the token("group name").
            if (parser.CurrentChar == IniParserConstants.BracketLeft) 
                parser.Read();

            int iterationCount = 0;

            // Read until "]" right bracket.
            while (parser.CurrentChar != IniParserConstants.BracketRight 
                && iterationCount <= parser.Settings.MaxLengthOfGroup)
            {
                buffer.Append(parser.CurrentChar);
                iterationCount++;

                // Advance the parser to next char.
                parser.Read();
            }

            // Error out if current char is not "]".
            if (parser.CurrentChar != IniParserConstants.BracketRight)
                parser.Errors.Add("Expected closing bracket ']' at : " + parser.CurrentCharIndex);
            else
                // Advance the parser to after the "]" closing bracket.
                parser.Read();

            return buffer.ToString();
        }


        /// <summary>
        /// Read the IniGroup name.
        /// [group1]
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string ReadComment(IIniParser parser)
        {
            StringBuilder buffer = new StringBuilder();
            
            // Exclude the "[" bracket from consuming the token("group name").
            string comment = parser.ReadTokenToEndOfLine();
            return comment;            
        }


        /// <summary>
        /// Read the IniGroup name.
        /// [group1]
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string ReadKey(IIniParser parser)
        {
            StringBuilder buffer = new StringBuilder();

            int iterationCount = 0;
            // Read until ":" colon.
            while (parser.CurrentChar != IniParserConstants.Colon.ToString()
               && iterationCount <= parser.Settings.MaxLengthOfKey)
            {
                buffer.Append(parser.CurrentChar);
                iterationCount++;

                // Advance the parser to next char.
                parser.Read();
            }

            // Error out if current char is not ":".            
            if (parser.CurrentChar != IniParserConstants.Colon.ToString())
                parser.Errors.Add("Expected colon ':' at : " + parser.CurrentCharIndex);
            else
                // Advance the parser to after the ":" colon.
                parser.Read();

            return buffer.ToString();
        }


        /// <summary>
        /// Read the IniGroup name.
        /// [group1]
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string ReadValue(IIniParser parser)
        {
            StringBuilder buffer = new StringBuilder();
            
            // Get rid of starting whitespace.
            parser.ConsumeWhiteSpace();

            if (parser.CurrentChar != IniParserConstants.DoubleQuote)
                return parser.ReadTokenToEndOfLine();

            return ReadValueMultiLine(parser);
        }


        /// <summary>
        /// Read a multi-line value.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public static string ReadValueMultiLine(IIniParser parser)
        {
            StringBuilder buffer = new StringBuilder();            
            string currentChar = string.Empty;

            // Read until ":" colon.
            while (currentChar != IniParserConstants.DoubleQuote)
            {
                // Escape char
                if (currentChar == IniParserConstants.Escape)
                {
                    currentChar = parser.Read().ToString();
                    buffer.Append(currentChar);
                }
                else
                    buffer.Append(currentChar);

                currentChar = parser.Read().ToString();
            }

            // Error out if current char is not ":".
            if (parser.CurrentChar != IniParserConstants.DoubleQuote)
                parser.Errors.Add("Expected double quote '\"' to end multi-line value at : " + parser.CurrentCharIndex);

            return buffer.ToString();
        }
    }
}
