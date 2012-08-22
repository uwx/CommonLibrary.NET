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
using System.IO;


namespace CommonLibrary
{
    /// <summary>
    /// Csv parser utils.
    /// </summary>
    public class CsvParser
    {
        private StringReader _reader;
        private CsvSettings _settings;
        public const int END = -1;
        private IList<string> _tokens = new List<string>();
        private StringBuilder _tokenValueBuffer = new StringBuilder();
        private char _currentChar;
        private int _currentCharInt;
        private int _currentCharIndex;


        /// <summary>
        /// Get / set the settings.
        /// </summary>
        public CsvSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        /// <summary>
        /// Parses a line using the settings provided.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public IList<string> ParseLine(string line)
        {
            _reader = new StringReader(text);
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
            if (_nextCharInt == END)
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
            if (_currentCharInt == END)
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
        /// Consume/read the white space.
        /// </summary>
        public void ConsumeWhiteSpace(bool readFirst)
        {
            if (readFirst) Read();

            while (_currentChar == ' ')
            {
                Read();
            }
        }


        /// <summary>
        /// List of errors.
        /// </summary>
        public IList<string> Errors
        {
            get { return _errors; }
        }

        #endregion


        /// <summary>
        /// Read a multi-line value.
        /// </summary>
        /// <param name="parser"></param>
        /// <returns></returns>
        public List<string> ReadValues()
        {
            string currentChar = _currentChar.ToString();
            string lastChar = string.Empty;
            bool isEnclosed = false;
            _tokens = new List<string>();
            
            // Use cases:
            // 1. Enclosed              : 'CommonLibrary.NET'
            // 2. Enclosed & Escaped    : 'CommonLibrary\'s Csv Parser'
            // 3. Non-Enclosed, Escaped :
            while ( _currentCharInt != END )
            {
                // Hit the closing quote "abcd"
                if ( isEnclosed && currentChar == _settings.ClosingQuote )
                {
                    StoreValue();
                }
                else if (isEnclosed && currentChar == CsvConstants.Escape)
                {
                    currentChar = Read().ToString();
                    buffer.Append(currentChar);
                }
                else if (currentChar == _settings.Separator && lastChar == _settings.ClosingQuote)
                {
                    StartNewValue();
                }                
                // Non enclosed and hit separator. abcd,
                else if ( !isEnclosed && currentChar == _settings.Separator )
                {
                    StoreValue();
                    StartNewValue();
                }

                lastChar = currentChar;
                currentChar = Read().ToString();
            }

            // Check if anything is left over in the buffer.
            // Add it as a token.
            if (_tokenValueBuffer.Length > 0)
                StoreValue();

            return _tokens;
        }

        
        private bool Expect(string charValue, bool read)
        {
            Peek();
            if (_currentChar.ToString() != charValue)
            {
                _errors.Add("Expected char : " + charValue + " at position : " + _currentCharIndex);
                return false;
            }
            else
            {
                if (read) Read();
            }
        }


        private void StoreValue()
        {
            _tokens.Add(_tokenValueBuffer.ToString());
        }


        private void StartNewValue()
        {
            // Clear buffer for next value.
            _tokenValueBuffer.Remove(0, buffer.Length);            
        }

    }
}
