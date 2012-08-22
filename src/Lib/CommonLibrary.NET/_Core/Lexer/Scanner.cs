/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: ? 2009 Kishore Reddy
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
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;


namespace ComLib
{
    /// <summary>
    /// Settings for the token reader class.
    /// </summary>
    public class ScannerSettings
    {
        /// <summary>
        /// Char used to escape.
        /// </summary>
        public char EscapeChar;


        /// <summary>
        /// Tokens
        /// </summary>
        public char[] Tokens;


        /// <summary>
        /// White space tokens.
        /// </summary>
        public char[] WhiteSpaceTokens;
    }



    /// <summary>
    /// The result of a scan for a specific token
    /// </summary>
    public class ScanTokenResult
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="success"></param>
        /// <param name="text"></param>
        public ScanTokenResult(bool success, string text) : this(success, text, 0)
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="success"></param>
        /// <param name="text">The parsed text</param>
        /// <param name="totalNewLines">The total number of new lines</param>
        public ScanTokenResult(bool success, string text, int totalNewLines)
        {
            Success = success;
            Text = text;
            Lines = totalNewLines;
        }


        /// <summary>
        /// Whether or not the token was properly present
        /// </summary>
        public readonly bool Success;


        /// <summary>
        /// The text of the token.
        /// </summary>
        public readonly string Text;


        /// <summary>
        /// Number of lines parsed.
        /// </summary>
        public int Lines;
    }



    /// <summary>
    /// This class implements a token reader.
    /// </summary>
    public class Scanner
    {
        /// <summary>
        /// Stores the lexical position
        /// </summary>
        public class ScannerState
        {
            /// <summary>
            /// Current position in text.
            /// </summary>
            public int Pos;


            /// <summary>
            /// Line number
            /// </summary>
            public int Line;


            /// <summary>
            /// Line char position.
            /// </summary>
            public int LineCharPosition;


            /// <summary>
            /// The source code.
            /// </summary>
            public string Text;


            /// <summary>
            /// The current char
            /// </summary>
            public char CurrentChar;


            /// <summary>
            /// The next char.
            /// </summary>
            public char NextChar;
        }


        #region Private members
        private IDictionary<char, char> _whiteSpaceChars;
        private IDictionary<char, char> _tokens;
        private Dictionary<char, char> _readonlyWhiteSpaceMap;        
        private List<string> _errors = new List<string>();
        private char _escapeChar;
        private char END_CHAR = ' ';
        private const char QUOTE = '\"';
        private const char TICK = '\'';
        private const char SPACE = ' ';
        private const char TAB = '\t';
        private ScannerState _state;
        #endregion


        internal int LAST_POSITION;

        /// <summary>
        /// Initialize this instance with defaults.
        /// </summary>
        public Scanner() : this(string.Empty)
        {
        }


        /// <summary>
        /// Initialize with text to parse.
        /// </summary>
        /// <param name="text"></param>
        public Scanner(string text)
        {
            Init(text, '\\', new char[] { TICK, QUOTE }, new char[] { ' ', '\t' } );
        }


        /// <summary>
        /// Initialize with text to parse.
        /// </summary>
        /// <param name="text">The text to scan</param>
        /// <param name="reservedChars">Reserved chars</param>
        public Scanner(string text, char[] reservedChars)
        {
            Init(text, '\\', reservedChars, new char[] { ' ', '\t' });
        }


        /// <summary>
        /// Initialize this instance with supplied parameters.
        /// </summary>
        /// <param name="text">Text to use.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="tokenChars">Special characters</param>
        /// <param name="whiteSpaceTokens">Array with whitespace tokens.</param>
        public Scanner(string text, char escapeChar, char[] tokenChars, char[] whiteSpaceTokens)
        {
            Init(text, escapeChar, tokenChars, whiteSpaceTokens);
        }


        #region Init
        /// <summary>
        /// Initialize using settings.
        /// </summary>
        /// <param name="text">Text to use.</param>
        public void Init(string text)
        {
            Init(text, '\\', new char[] { TICK, QUOTE }, new char[] { ' ', '\t' });
        }


        /// <summary>
        /// Initialize using settings.
        /// </summary>
        /// <param name="text">Text to use.</param>
        /// <param name="settings">Instance with token reader settings.</param>
        public void Init(string text, ScannerSettings settings)
        {            
            Init(text, settings.EscapeChar, settings.Tokens, settings.WhiteSpaceTokens);
        }
        
        
        /// <summary>
        /// Initialize using the supplied parameters.
        /// </summary>
        /// <param name="text">Text to read.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="tokens">Array with tokens.</param>
        /// <param name="whiteSpaceTokens">Array with whitespace tokens.</param>
        public void Init(string text, char escapeChar, char[] tokens, char[] whiteSpaceTokens)
        {
            Reset();
            _state.Text = text;
            LAST_POSITION = _state.Text.Length - 1;
            _escapeChar = escapeChar;
            _tokens = ToDictionary(tokens);
            _whiteSpaceChars = ToDictionary(whiteSpaceTokens);
            _readonlyWhiteSpaceMap = new Dictionary<char, char>(_whiteSpaceChars);
        }
        #endregion


        #region Properties
        /// <summary>
        /// The current position.
        /// </summary>
        public int Position { get { return _state.Pos; } }


        /// <summary>
        /// Scanner Position.
        /// </summary>
        public ScannerState Pos { get { return _state; } }


        /// <summary>
        /// The text being scanned.
        /// </summary>
        public string Text { get { return _state.Text; } }


        /// <summary>
        /// Current char.
        /// </summary>
        /// <returns>Current character.</returns>
        public char CurrentChar { get { return _state.CurrentChar; } }


        /// <summary>
        /// Get the previous char that was read in.
        /// </summary>
        public char PreviousChar
        {
            get
            {
                if (_state.Pos <= 0) return char.MinValue;

                // Get the last char from the back buffer.
                // This is the last valid char that is not escaped.
                return _state.Text[_state.Pos - 1];
            }
        }
        #endregion


        #region Reset
        /// <summary>
        /// Reset reader for parsing again.
        /// </summary>
        public void Reset()
        {
            _state = new ScannerState();
            _state.Pos = -1;
            _state.Line = 1;
            _state.Text = string.Empty ;
            _whiteSpaceChars = new Dictionary<char, char>();
            _tokens = new Dictionary<char, char>();
            _escapeChar = '\\';            
        }


        /// <summary>
        /// Resets the scanner position to 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="handleNewLine"></param>
        public void ResetTo(int pos, bool handleNewLine = true)
        {
            if (pos >= 0)
            {
                _state.Pos = pos;
                _state.CurrentChar = _state.Text[pos];
                if (handleNewLine)
                {
                    if (_state.CurrentChar == '\n' && (pos - 1 >= 0) && _state.Text[pos - 1] == '\r')
                    {
                        _state.Pos--;
                        _state.CurrentChar = _state.Text[pos];
                    }
                }
            }
        }
        #endregion


        #region Peek
        /// <summary>
        /// Returns the char at current position + 1.
        /// </summary>
        /// <returns>Next char or string.empty if end of text.</returns>
        public char PeekChar()
        {
            // Validate.
            if(_state.Pos >= LAST_POSITION )
                return char.MinValue;

            _state.NextChar = _state.Text[_state.Pos + 1];
            return _state.NextChar;
        }


        /// <summary>
        /// Returns the chars starting at current position + 1 and
        /// including the <paramref name="count"/> number of characters.
        /// </summary>
        /// <param name="count">Number of characters.</param>
        /// <returns>Range of chars as string or string.empty if end of text.</returns>
        public string PeekChars(int count)
        {
            // Validate.
            if (_state.Pos + count > LAST_POSITION)
                return string.Empty;

            return _state.Text.Substring(_state.Pos + 1, count);
        }


        /// <summary>
        /// Returns the nth char from the current char index
        /// </summary>
        /// <param name="countFromCurrentCharIndex">Number of characters from the current char index</param>
        /// <returns>Single char as string</returns>
        public char PeekCharAt(int countFromCurrentCharIndex)
        {
            // Validate.
            if (_state.Pos + countFromCurrentCharIndex > LAST_POSITION)
                return END_CHAR;

            return _state.Text[_state.Pos + countFromCurrentCharIndex];
        }


        /// <summary>
        /// Peeks at the string all the way until the end of line.
        /// </summary>
        /// <returns>Current line.</returns>
        public string PeekLine()
        {
            int ndxEol = _state.Text.IndexOf(Environment.NewLine, _state.Pos + 1);
            if (ndxEol == -1)
                return _state.Text.Substring(_state.Pos + 1);

            return _state.Text.Substring(_state.Pos + 1, (ndxEol - _state.Pos));
        }
        #endregion


        #region Consume
        /// <summary>
        /// Consume the whitespace without reading anything.
        /// </summary>
        public void ConsumeWhiteSpace()
        {
            ConsumeWhiteSpace(false);
        }


        /// <summary>
        /// Consume all white space.
        /// This works by checking the next char against
        /// the chars in the dictionary of chars supplied during initialization.
        /// </summary>
        /// <param name="readFirst">True to read a character
        /// before consuming the whitespace.</param>
        public void ConsumeWhiteSpace(bool readFirst)
        {
            char currentChar = readFirst ? ReadChar() : _state.CurrentChar;

            while ( !IsEnded() && _whiteSpaceChars.ContainsKey(currentChar))
            {
                // Advance reader and next char.
                ReadChar();
                currentChar = _state.CurrentChar;
            }            
        }


        /// <summary>
        /// Consume all white space.
        /// This works by checking the next char against
        /// the chars in the dictionary of chars supplied during initialization.
        /// </summary>
        /// <param name="readFirst">True to read a character
        /// before consuming the whitepsace.</param>
        /// <param name="setPosAfterWhiteSpace">True to move position to after whitespace</param>
        public void ConsumeWhiteSpace(bool readFirst, bool setPosAfterWhiteSpace = true)
        {
            if (readFirst) ReadChar();

            bool matched = false;
            while (_state.Pos <= LAST_POSITION)
            {
                if (!_whiteSpaceChars.ContainsKey(_state.CurrentChar))
                {
                    matched = true;
                    break;
                }
                ReadChar();
            }

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterWhiteSpace) MoveChars(-1);
        }


        /// <summary>
        /// Consume new line.
        /// </summary>
        public void ConsumeNewLine()
        {
            // Check 
            if (_state.CurrentChar == '\r' )
            {
                if (PeekChar() == '\n')
                {
                    ReadChar();
                    ReadChar();
                }
                else
                    ReadChar();
            }                
        }


        /// <summary>
        /// Read text up to the eol.
        /// </summary>
        /// <returns>String read.</returns>
        public void ConsumeToNewLine(bool includeNewLine = false)
        {
            // Read until ":" colon and while not end of string.
            while (!IsEol() && _state.Pos <= LAST_POSITION)
            {
                MoveChars(1);
            }
            if(includeNewLine) ConsumeNewLine();
        }


        /// <summary>
        /// Consume until the chars found.
        /// </summary>
        /// <param name="pattern">The pattern to consume chars to.</param>
        /// <param name="includePatternInConsumption">Wether or not to consume the pattern as well.</param>
        /// <param name="movePastEndOfPattern">Whether or not to move to the ending position of the pattern</param>
        /// <param name="moveToStartOfPattern">Whether or not to move to the starting position of the pattern</param>
        public bool ConsumeUntil(string pattern, bool includePatternInConsumption, bool moveToStartOfPattern, bool movePastEndOfPattern)
        {
            int ndx = _state.Text.IndexOf(pattern, _state.Pos);
            if (ndx == -1 ) return false;
            int newCharPos = 0;

            if(!includePatternInConsumption)
                newCharPos = moveToStartOfPattern ? ndx : ndx - 1;
            else
                newCharPos = movePastEndOfPattern ? ndx + pattern.Length : (ndx + pattern.Length) - 1;

            MoveChars(newCharPos - _state.Pos);                        
            return true;                
        }


        /// <summary>
        /// Consume New Lines.
        /// </summary>
        public void ConsumeNewLines()
        {
            string combinedNewLine = _state.CurrentChar.ToString() + PeekChar();
            while (_state.CurrentChar == '\r' || combinedNewLine == "\r\n" && _state.Pos != LAST_POSITION)
            {
                ConsumeNewLine();
                combinedNewLine = _state.CurrentChar.ToString() + PeekChar();            
            }
        }
        #endregion


        #region Is Checks
        /// <summary>
        /// Determine if current char is token.
        /// </summary>
        /// <returns>True if the current char is a token.</returns>
        public bool IsToken()
        {
            if (_tokens == null || _tokens.Keys.Count == 0)
                return false;
            return _tokens.ContainsKey(_state.CurrentChar);
        }


        /// <summary>
        /// Whether or not the current sequence of chars matches the token supplied.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ignoreCase">Whether or not to check against case.</param>
        /// <returns></returns>
        public bool IsToken(string token, bool ignoreCase = false)
        {
            if (_tokens == null || _tokens.Keys.Count == 0)
                return false;
            StringComparison c = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;
            return string.Compare(_state.Text, _state.Pos, token, 0, token.Length, c) == 0;
        }


        /// <summary>
        /// Determine if current char is escape char.
        /// </summary>
        /// <returns>True if the current char is an escape char.</returns>
        public bool IsEscape()
        {
            return (_state.CurrentChar == _escapeChar || IsEscapedQuote());
        }


        /// <summary>
        /// Whether of not the current character is an escaped quote.
        /// </summary>
        /// <returns></returns>
        public bool IsEscapedQuote()
        {
            return _state.CurrentChar == QUOTE && PeekChar() == QUOTE;
        }


        /// <summary>
        /// Determine if the end of the text input has been reached.
        /// </summary>
        /// <returns>True if the end of the stream has been reached.</returns>
        public bool IsEnded()
        {
            return _state.Pos >= _state.Text.Length;
        }


        /// <summary>
        /// Determine if at last char.
        /// </summary>
        /// <returns>True if the last character is the current character.</returns>
        public bool IsAtEnd()
        {
            return _state.Pos == LAST_POSITION;
        }


        /// <summary>
        /// Determine if current char is EOL.
        /// </summary>
        /// <returns>True if the current character is an eol.</returns>
        public bool IsEol()
        {
            // Check for "\r\n"
            if (_state.CurrentChar == '\r' && PeekChar() == '\n')
                return true;

            return false;
        }


        /// <summary>
        /// Determine if current char is whitespace.
        /// </summary>
        /// <param name="whitespaceChars">Dictionary with whitespace chars.</param>
        /// <returns>True if the current character is a whitespace.</returns>
        public bool IsWhiteSpace(IDictionary whitespaceChars)
        {
            return whitespaceChars.Contains(_state.CurrentChar);
        }


        /// <summary>
        /// Determine if current char is whitespace.
        /// </summary>
        /// <returns>True if the current character is a whitespace.</returns>
        public bool IsWhiteSpace()
        {
            return this._whiteSpaceChars.ContainsKey(_state.CurrentChar);
        }
        #endregion


        #region Read API - Code, Id, Line, Number, String, Uri, Word 
        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="quoteChar">char representing quote ' or "</param>
        /// <param name="escapeChar">Escape character for quote within string.</param>
        /// <param name="advanceFirst">True to advance position first before reading string.</param>
        /// <param name="setPosAfterToken">True to move position to end quote, otherwise past end quote.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadCodeString(char quoteChar, char escapeChar = '\\', bool advanceFirst = true, bool setPosAfterToken = true)
        {
            // "name" 'name' "name\"s" 'name\'"
            var buffer = new StringBuilder();
            int totalNewLines = 0;
            char curr = advanceFirst ? ReadChar() : _state.CurrentChar;
            char next = PeekChar();
            bool matched = false;
            while (_state.Pos <= LAST_POSITION)
            {
                // End string " or '
                if (curr == quoteChar)
                {
                    matched = true;
                    MoveChars(1);
                    break;
                }
                // Not an \ for escaping so just append.
                else if (curr != escapeChar)
                {
                    if (curr == '\r') totalNewLines++;

                    buffer.Append(curr);
                }
                // Escape \
                else if (curr == escapeChar)
                {
                    if (next == quoteChar) buffer.Append(quoteChar);
                    else if (next == '\\') buffer.Append("\\");
                    else if (next == 'r') buffer.Append('\r');
                    else if (next == 'n') buffer.Append('\n');
                    else if (next == 't') buffer.Append('\t');
                    MoveChars(1);
                }

                curr = ReadChar(); next = PeekChar();
            }
            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(matched, buffer.ToString(), totalNewLines);
        }


        /// <summary>
        /// Reads an identifier where legal chars for the identifier are [$ . _ a-z A-Z 0-9]
        /// </summary>
        /// <param name="advanceFirst"></param>
        /// <param name="setPosAfterToken">True to move position to after id, otherwise 2 chars past</param>
        /// <returns></returns>
        public ScanTokenResult ReadId(bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_state.Pos <= LAST_POSITION)
            {
                if (('a' <= _state.CurrentChar && _state.CurrentChar <= 'z') ||
                        ('A' <= _state.CurrentChar && _state.CurrentChar <= 'Z') ||
                         _state.CurrentChar == '$' || _state.CurrentChar == '_' ||
                        ('0' <= _state.CurrentChar && _state.CurrentChar <= '9')
                   )
                    buffer.Append(_state.CurrentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_state.Pos < LAST_POSITION)
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Reads entire line from curr position
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="setPosAfterToken">Whether or not to move curr position to starting of new line or after</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadLine(bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool is2CharNewLine = false;
            while (_state.Pos <= LAST_POSITION)
            {
                if (_state.CurrentChar == '\r')
                {
                    char nextChar = PeekChar();
                    is2CharNewLine = nextChar == '\n';
                    matched = true;
                    break;
                }
                else
                    buffer.Append(_state.CurrentChar);

                ReadChar();
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || _state.Pos > LAST_POSITION;

            if (success)
            {
                if (is2CharNewLine)
                    MoveChars(1);
                if (setPosAfterToken)
                    MoveChars(1);
            }

            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Reads a number +/-?[0-9]*.?[0-9]*
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadNumber(bool advanceFirst, bool setPosAfterToken = true)
        {
            string sign = "";
            if (advanceFirst) ReadChar();
            if (_state.CurrentChar == '+' || _state.CurrentChar == '-') { sign = _state.CurrentChar.ToString(); ReadChar(); }
                        
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_state.Pos <= LAST_POSITION)
            {
                if ('0' <= _state.CurrentChar && _state.CurrentChar <= '9' || _state.CurrentChar == '.')
                    buffer.Append(_state.CurrentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_state.Pos < LAST_POSITION)
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);

            //return new ScanTokenResult(success, buffer.ToString());            
            var text = buffer.ToString();

            var finalresult = new ScanTokenResult(success, sign + text);
            finalresult.Lines = 0;
            return finalresult;
        }


        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="quoteChar">char representing quote ' or "</param>
        /// <param name="escapeChar">Escape character for quote within string.</param>
        /// <param name="advanceFirst">True to advance position first before reading string.</param>
        /// <param name="setPosAfterToken">True to move position to end quote, otherwise past end quote.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadString(char quoteChar, char escapeChar = '\\', bool advanceFirst = true, bool setPosAfterToken = true)
        {
            // "name" 'name' "name\"s" 'name\'"
            var buffer = new StringBuilder();
            var curr = advanceFirst ? ReadChar() : _state.CurrentChar;
            var next = PeekChar();
            bool matched = false;
            while (_state.Pos <= LAST_POSITION)
            {
                // Escape char
                if (curr == escapeChar)
                {
                    curr = ReadChar();
                    buffer.Append(curr);
                }
                else if (curr == quoteChar)
                {
                    matched = true;
                    MoveChars(1);
                    break;
                }
                else buffer.Append(curr);

                curr = ReadChar(); next = PeekChar();
            }
            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(matched, buffer.ToString());
        }


        /// <summary>
        /// Reads until the 2 chars are reached.
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="setPosAfterToken">Whether or not to advance to position after chars</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadUri(bool advanceFirst, bool setPosAfterToken = true)
        {
            var buffer = new StringBuilder();
            var currentChar = advanceFirst ? ReadChar() : _state.CurrentChar;
            if (currentChar == SPACE || currentChar == TAB)
                return new ScanTokenResult(true, string.Empty);

            var nextChar = PeekChar();
            bool matched = false;
            while (_state.Pos <= LAST_POSITION)
            {
                buffer.Append(currentChar);
                if (nextChar == SPACE || nextChar == TAB || nextChar == '(' || nextChar == ')'
                    || nextChar == ',' || nextChar == ';' || nextChar == '[' || nextChar == ']'
                    || nextChar == '\r' || nextChar == '\n' || nextChar == '\t')
                {
                    matched = true;
                    break;
                }
                currentChar = ReadChar();
                nextChar = PeekChar();
            }
            string text = buffer.ToString();
            if (matched && setPosAfterToken)
            {
                MoveChars(1);
            }

            return new ScanTokenResult(matched, text);
        }
        #endregion


        /// <summary>
        /// Store the white space chars.
        /// </summary>
        /// <param name="whitespaceChars">Dictionary with whitespace characters.</param>
        public void RegisterWhiteSpace(IDictionary<char, char> whitespaceChars)
        {
            _whiteSpaceChars = whitespaceChars;
        }


        /// <summary>
        /// Moves forward by count chars.
        /// </summary>
        /// <param name="count"></param>
        public void MoveChars(int count)
        {
            // Pos can never be more than 1 + last index position.
            // e.g. "common"
            // 1. length = 6
            // 2. LAST_POSITION = 5;
            // 3. _state can not be more than 6. 6 indicating that it's past end
            // 4. _state == 5 Indicating it's at end.
            if (_state.Pos > LAST_POSITION && count > 0) return;

            // Move past end? Move it just 1 position more than last index.
            if (_state.Pos + count > LAST_POSITION)
            {
                _state.Pos = LAST_POSITION + 1;
                _state.CurrentChar = END_CHAR;
                return;
            }

            // Can move forward count chars
            _state.Pos += count;           
            _state.CurrentChar = _state.Text[_state.Pos];
        }


        /// <summary>
        /// Read back the last char and reset
        /// </summary>
        public void ReadBackChar()
        {
            _state.Pos--;
            _state.CurrentChar = _state.Text[_state.Pos];
        }


        /// <summary>
        /// Unwinds the reader by <paramref name="count"/> chars.
        /// </summary>
        /// <param name="count">Number of characters to read.</param>
        public void ReadBackChar(int count)
        {
            // Unwind            
            _state.Pos -= count;
            _state.CurrentChar = _state.Text[_state.Pos];
        }


        /// <summary>
        /// Read the next char.
        /// </summary>
        /// <returns>Character read.</returns>
        public char ReadChar()
        {
            // NEVER GO PAST 1 INDEX POSITION AFTER CHAR
            if (_state.Pos > LAST_POSITION) return END_CHAR;

            _state.Pos++;

            // Still valid?
            if (_state.Pos <= LAST_POSITION)
            {
                _state.CurrentChar = _state.Text[_state.Pos];
                return _state.CurrentChar;
            }
            _state.CurrentChar = END_CHAR;
            return END_CHAR;
        }


        /// <summary>
        /// Read the next <paramref name="count"/> number of chars.
        /// </summary>
        /// <param name="count">Number of characters to read.</param>
        /// <returns>Characters read.</returns>
        public string ReadChars(int count)
        {
            string text = _state.Text.Substring(_state.Pos + 1, count);
            _state.Pos += count;
            _state.CurrentChar = _state.Text[_state.Pos];
            return text;
        }


        /// <summary>
        /// Read text up to the eol.
        /// </summary>
        /// <returns>String read.</returns>
        public string ReadToEol()
        {
            StringBuilder buffer = new StringBuilder();

            // Read until ":" colon and while not end of string.
            while (!IsEol() && _state.Pos <= LAST_POSITION)
            {
                buffer.Append(_state.CurrentChar);
                ReadChar();
            }
            return buffer.ToString();
        }


        /// <summary>
        /// ReadToken until one of the endchars is found
        /// </summary>
        /// <param name="endChars">List of possible end chars which halts reading further.</param>
        /// <param name="includeEndChar">True to include end character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns></returns>
        public string ReadTokenUntil(char[] endChars, bool includeEndChar = false, bool advanceFirst = false, bool readPastEndChar = false )
        {
            var buffer = new StringBuilder();
            bool found = false;
            if (advanceFirst) ReadChar();

            while (_state.Pos < LAST_POSITION && !found)
            {
                for (int ndx = 0; ndx < endChars.Length; ndx++)
                {
                    if (_state.CurrentChar == endChars[ndx])
                    {
                        found = true;
                        break;
                    }
                }
                if(!found || (found && includeEndChar))
                    buffer.Append(_state.CurrentChar);

                if(!found || (found && readPastEndChar))
                    ReadChar();
            }
            string token = buffer.ToString();
            return token;
        }


        /// <summary>
        /// Read a token.
        /// </summary>
        /// <param name="endChar">Ending character.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="includeEndChar">True to include end character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="expectEndChar">True to expect an end charachter.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns>Contens of token read.</returns>
        public string ReadToken(char endChar, char escapeChar, bool includeEndChar, bool advanceFirst, bool expectEndChar, bool readPastEndChar)
        {
            StringBuilder buffer = new StringBuilder();
            char currentChar = advanceFirst ? ReadChar() : _state.CurrentChar;

            // Read until ":" colon and while not end of string.
            while (currentChar != endChar && _state.Pos <= LAST_POSITION)
            {
                // Escape char
                if (currentChar == escapeChar)
                {
                    currentChar = ReadChar();
                    buffer.Append(currentChar);
                }
                else
                    buffer.Append(currentChar);

                currentChar = ReadChar();
            }
            bool matchedEndChar = true;

            // Error out if current char is not ":".
            if (expectEndChar && _state.CurrentChar != endChar)
            {
                _errors.Add("Expected " + endChar + " at : " + _state);
                matchedEndChar = false;
            }

            // Read past char.
            if (matchedEndChar && readPastEndChar)
                ReadChar();

            return buffer.ToString();
        }


        #region New API 
        /// <summary>
        /// Reads text up the position supplied.
        /// </summary>
        /// <param name="from1CharForward"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        public ScanTokenResult ReadToPosition(bool from1CharForward, int endPos)
        {
            int start = from1CharForward 
                      ? _state.Pos + 1
                      : _state.Pos;

            int length = (endPos - start) + 1;
            string word = _state.Text.Substring(start, length);

            // Update the position and 
            MoveChars(endPos - _state.Pos);
            return new ScanTokenResult(true, word);
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadWord(bool advanceFirst, bool setPosAfterToken = true)
        {
            return ReadChars(() => _state.CurrentChar != ' ' && _state.CurrentChar != '\t', advanceFirst, setPosAfterToken);
        }

        /// <summary>
        /// Read the next word
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance first</param>
        /// <param name="setPosAfterToken">Whether or not to set the position after the token.</param>
        /// <param name="extra1">An extra character that is allowed to be part of the word in addition to the allowed chars</param>
        /// <param name="extra2">A second extra character that is allowed to be part of word in addition to the allowed chars</param>        
        /// <returns></returns>
        public ScanTokenResult ReadWordWithExtraChars(bool advanceFirst, bool setPosAfterToken = true, char extra1 = char.MinValue, char extra2 = char.MinValue)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            bool hasExtra1 = extra1 != char.MinValue;
            bool hasExtra2 = extra2 != char.MinValue;
            
            while (_state.Pos <= LAST_POSITION)
            {
                var c = _state.CurrentChar;
                if (('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') ||
                     ('0' <= c && c <= '9') || (c == '_' || c == '-'))
                {
                    buffer.Append(c);
                }
                else if ((hasExtra1 && c == extra1) || (hasExtra2 && c == extra2))
                    buffer.Append(c);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_state.Pos < LAST_POSITION)
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);

            return new ScanTokenResult(success, buffer.ToString());
        }       


        /// <summary>
        /// Reads until the 2 chars are reached.
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="first">The first char expected</param>
        /// <param name="second">The second char expected</param>
        /// <param name="setPosAfterToken">Whether or not to advance to position after chars</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadUntilChars(bool advanceFirst, char first, char second, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            int totalNewLines = 0;
            while (_state.Pos <= LAST_POSITION)
            {
                if (_state.CurrentChar == '\r')
                    totalNewLines++;

                if (!(_state.CurrentChar == first && _state.NextChar == second))
                    buffer.Append(_state.CurrentChar);
                else
                {                    
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_state.Pos < LAST_POSITION)
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);
            
            if (setPosAfterToken)
                MoveChars(2);

            // Decrement by 1 newline if the endchars represent new line and not setting position after the newline.
            if (_state.CurrentChar == '\r' && _state.NextChar == '\n' && !setPosAfterToken)
                totalNewLines--;
            
            return new ScanTokenResult(success, buffer.ToString(), totalNewLines);
        }


        /// <summary>
        /// Reads until the 2 chars are reached.
        /// </summary>
        /// <param name="advanceFirst">Whether or not to advance curr position first </param>
        /// <param name="first">The first char expected</param>
        /// <param name="second">The second char expected</param>
        /// <param name="moveToEndChar">Whether or not to advance to last end char ( second char ) or move past it</param>
        /// <returns>String read.</returns>
        public ScanTokenResult ReadLinesUntilChars(bool advanceFirst, char first, char second, bool moveToEndChar = true)
        {
            int lineCount = 0;
            return ReadChars(() =>
            {
                // Keep track of lines meet
                if (_state.CurrentChar == '\r' && _state.NextChar == '\n') lineCount++;

                return !(_state.CurrentChar == first && _state.NextChar == second);
            }, advanceFirst, moveToEndChar);
        }
        #endregion


        /// <summary>
        /// Read token until endchar
        /// </summary>
        /// <param name="endChar">Ending character.</param>
        /// <param name="escapeChar">Escape character.</param>
        /// <param name="advanceFirst">True to advance before reading.</param>
        /// <param name="expectEndChar">True to expect an end charachter.</param>
        /// <param name="includeEndChar">True to include an end character.</param>
        /// <param name="moveToEndChar">True to move to the end character.</param>
        /// <param name="readPastEndChar">True to read past the end character.</param>
        /// <returns>Contents of token read.</returns>
        public string ReadTokenUntil(char endChar, char escapeChar, bool advanceFirst, bool expectEndChar, bool includeEndChar, bool moveToEndChar, bool readPastEndChar)
        {
            // abcd <div>
            var buffer = new StringBuilder();
            var currentChar = advanceFirst ? ReadChar() : _state.CurrentChar;
            var nextChar = PeekChar();
            while (nextChar != endChar && _state.Pos <= LAST_POSITION)
            {
                // Escape char
                if (currentChar == escapeChar)
                {
                    currentChar = ReadChar();
                    buffer.Append(currentChar);
                }
                else
                    buffer.Append(currentChar);

                currentChar = ReadChar();
                nextChar = PeekChar();
            }
            bool matchedEndChar = nextChar == endChar;
            if( expectEndChar && !matchedEndChar)
                _errors.Add("Expected " + endChar + " at : " + _state);

            if (matchedEndChar)
            {
                buffer.Append(currentChar);
                if (includeEndChar)
                    buffer.Append(nextChar);

                if (moveToEndChar)
                    ReadChar();

                else if (readPastEndChar && !IsAtEnd())
                    ReadChars(2);
            }
            
            return buffer.ToString();
        }


        /// <summary>
        /// Determines whether the current character is the expected one.
        /// </summary>
        /// <param name="charToExpect">Character to expect.</param>
        /// <returns>True if the current character is the expected one.</returns>
        public bool Expect(char charToExpect)
        {
            bool isMatch = _state.CurrentChar == charToExpect;
            if(!isMatch)
                _errors.Add("Expected " + charToExpect + " at : " + _state);
            return isMatch;
        }

        
        /// <summary>
        /// Current position in text.
        /// </summary>
        /// <returns>Current character index.</returns>
        public int CurrentCharIndex()
        {
            return _state.Pos;
        }        


        /// <summary>
        /// Sets the current position
        /// </summary>
        /// <param name="pos"></param>
        internal void SetPosition(int pos)
        {
            // Pos can never be more than 1 + last index position.
            // e.g. "common"
            // 1. length = 6
            // 2. LAST_POSITION = 5;
            // 3. _state can not be more than 6. 6 indicating that it's past end
            // 4. _state == 5 Indicating it's at end.
            if (pos >= LAST_POSITION) throw new Lang.LangException("Lexical Error", "Can not set position to : " + pos, "", -1, -1);
            if (pos < 0) throw new Lang.LangException("Lexical Error", "Can not set position before 0 : " + pos, "", -1, -1);

            _state.Pos = pos;
            _state.CurrentChar = _state.Text[_state.Pos];
        }


        #region Private methods
        private void Init(IDictionary<string, bool> tokens, string[] tokenList)
        {
            foreach (string token in tokenList)
            {
                tokens[token] = true;
            }
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="continueReadCheck">Callback function to determine whether or not to continue reading</param>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadChars(Func<bool> continueReadCheck, bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_state.Pos <= LAST_POSITION)
            {
                if (continueReadCheck())
                    buffer.Append(_state.CurrentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if(_state.Pos < LAST_POSITION )
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }
            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);
            
            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (success && !setPosAfterToken) MoveChars(-1);
            
            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Reads a word which must not have space in it and must have space/tab before and after
        /// </summary>
        /// <param name="validChars">Dictionary to check against valid chars.</param>
        /// <param name="advanceFirst">Whether or not to advance position first</param>
        /// <param name="setPosAfterToken">True to move position to end space, otherwise past end space.</param>
        /// <returns>Contents of token read.</returns>
        public ScanTokenResult ReadChars(IDictionary<char, bool> validChars, bool advanceFirst, bool setPosAfterToken = true)
        {
            // while for function
            var buffer = new StringBuilder();
            if (advanceFirst) ReadChar();

            bool matched = false;
            bool valid = true;
            while (_state.Pos <= LAST_POSITION)
            {
                if (validChars.ContainsKey(_state.CurrentChar))
                    buffer.Append(_state.CurrentChar);
                else
                {
                    matched = true;
                    valid = false;
                    break;
                }
                ReadChar();
                if (_state.Pos < LAST_POSITION)
                    _state.NextChar = _state.Text[_state.Pos + 1];
            }

            // At this point the pos is already after token.
            // If matched and need to set at end of token, move back 1 char
            if (matched && !setPosAfterToken) MoveChars(-1);

            // Either 
            // 1. Matched the token
            // 2. Did not match but valid && end_of_file
            bool success = matched || (valid && _state.Pos > LAST_POSITION);
            return new ScanTokenResult(success, buffer.ToString());
        }


        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>Dictionary of items.</returns>
        public static IDictionary<T, T> ToDictionary<T>(IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        } 
        #endregion
    }
}
