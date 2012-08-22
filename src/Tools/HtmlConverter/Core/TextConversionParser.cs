using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.TextConverter.Plugins;

namespace ComLib.TextConverter
{
    /// <summary>
    /// Text conversion parser.
    /// </summary>
    public class TextConversionParser
    {
        private string _currentWord;
        private char _currentChar;
        private string _text;
        private int _currentPosition;
        private int _textLength;
        private IDictionary<string, TextPlugin> _plugins;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="plugins"></param>
        public void Init(string text, IDictionary<string, TextPlugin> plugins)
        {
            _text = text;
            _textLength = _text.Length;
            _currentPosition = 0;
            _plugins = plugins;
        }


        /// <summary>
        /// Parses the text and converts to 
        /// </summary>
        public string Parse()
        {
            var buffer = new StringBuilder();
            
            // Move to first word.
            NextWord();

            BasicFormatting();
            
            // Keep reading words to give to plugins to handle.
            while (_currentPosition < _textLength)
            {
                if (_plugins.ContainsKey(_currentWord))
                {
                    var plugin = _plugins[_currentWord];
                    string text = plugin.Convert();
                    buffer.Append(text);
                }
                // Advance to the next word.
                NextWord();
            }
            string convertedText = buffer.ToString();
            return convertedText;
        }

        private void BasicFormatting()
        {
            if (_currentWord[0] == '*' && _currentWord[_currentWord.Length - 1] == '*' && _currentWord.Length > 1)
            {
                _currentWord = "*";
            }

            if (_currentWord[0] == '_' && _currentWord[_currentWord.Length - 1] == '_' && _currentWord.Length > 1)
            {
                _currentWord = "_";
            }

            if (_currentWord[0] == '+' && _currentWord[_currentWord.Length - 1] == '+' && _currentWord.Length > 1)
            {
                _currentWord = "+";
            }

            if (_currentWord[0] == '~' && _currentWord[_currentWord.Length - 1] == '~' && _currentWord.Length > 1)
            {
                _currentWord = "~";
            }

            if (_currentWord[0] == '^' && _currentWord[_currentWord.Length - 1] == '^' && _currentWord.Length > 1)
            {
                _currentWord = "^";
            }

            if (_currentWord[0] == ',' && _currentWord[_currentWord.Length - 1] == ',' && _currentWord.Length > 1)
            {
                _currentWord = ",";
            }
        }


        /// <summary>
        /// Whether or not the current word is the start of a plugin.
        /// </summary>
        /// <returns></returns>
        public bool IsCurrentWordStartOfPlugin()
        {
            return _plugins.ContainsKey(_currentWord);
        }


        /// <summary>
        /// Whether or not the current word is the start of a plugin.
        /// </summary>
        /// <returns></returns>
        public bool IsCurrentCharStartOfPlugin()
        {
            return _plugins.ContainsKey(_currentChar.ToString());
        }


        /// <summary>
        /// Gets the current word.
        /// </summary>
        /// <returns></returns>
        public string CurrentWord { get { return _currentWord; } }


        /// <summary>
        /// The current Char.
        /// </summary>
        public char CurrentChar { get { return _currentChar; } }


        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns></returns>
        public string Text { get { return _text; } }


        /// <summary>
        /// Gets the current position.
        /// </summary>
        /// <returns></returns>
        public int CurrentPosition { get { return _currentPosition; } }
        

        /// <summary>
        /// Reads the next word and the next word becomes current.
        /// </summary>
        /// <returns></returns>
        public Word NextWord(bool movePastWhiteSpace = true)
        {
            bool found = false;
            string word = string.Empty;

            // At end of text ?
            if (_currentPosition >= _textLength) return Word.Empty;

            // Make sure the position stops at the end of the text
            while (_currentPosition < _textLength)
            {
                char currentChar = _text[_currentPosition];
                if (currentChar != ' ' && currentChar != '\t')
                {
                    word += currentChar;
                }
                else
                {
                    if (movePastWhiteSpace)
                        _currentPosition++;

                    found = true;
                    break;
                }
                _currentPosition++;
            }
            if (!found && !IsBasicFormatting(found, word))//word[0] != '*' && word[0] != '_' && word[0] != '+' && word[0] != '~' && word[0] != '^' && word[0] != ',')
                _currentWord = string.Empty;
            else if (IsBasicFormatting(found, word))
            {
                _currentWord = word;
                _currentPosition = 1;
                _currentChar = _text[_currentPosition];
            }
            else
            {
                _currentWord = word;
                _currentChar = _text[_currentPosition];
            }            
            return new Word(_currentWord);
        }

        private static bool IsBasicFormatting(bool found, string word)
        {
            return (!found && word[0] == '*' && word[word.Length - 1] == '*') ||
                             (word[0] == '_' && word[word.Length - 1] == '_') ||
                             (word[0] == '+' && word[word.Length - 1] == '+') ||
                             (word[0] == '~' && word[word.Length - 1] == '~') ||
                             (word[0] == '^' && word[word.Length - 1] == '^') ||
                             (word[0] == ',' && word[word.Length - 1] == ',') ;
        }


        /// <summary>
        /// Reads a line of text.
        /// </summary>
        /// <returns></returns>
        public string ReadLine(bool advance1CharFirst = false, bool includeCarriageReturnInText = false)
        {
            // At end of text ?
            if (_currentPosition >= _textLength) return string.Empty;

            // while for function
            var buffer = new StringBuilder();
            if (advance1CharFirst)
            {
                _currentPosition++;
                _currentChar = _text[_currentPosition];
            }

            bool matched = false;
            bool is2CharNewLine = false;
            while (_currentPosition < _textLength)
            {
                _currentChar = _text[_currentPosition];
                if (_currentChar == '\r')
                {
                    char nextChar = _text[_currentPosition + 1];
                    is2CharNewLine = nextChar == '\n';
                    matched = true;
                    break;
                }
                else
                    buffer.Append(_currentChar);

                _currentPosition++;                
            }
            if (matched)
            {   
                // \r\n or \r
                if (is2CharNewLine) _currentPosition += 2;
                else _currentPosition += 1;
                _currentChar = _text[_currentPosition];
            }
            string text = buffer.ToString();
            return text;
        }


        /// <summary>
        /// Reads until the next looking symbol
        /// </summary>
        /// <returns></returns>
        public string ReadUntil(char endChar, string text, int currPosition)
        {
            var buffer = new StringBuilder();
            for (int i = currPosition; i < text.Length - 1; i++)
            {
                if (text[i] != endChar)
                    buffer.Append(text[i]);    
            }
            
            string txt = buffer.ToString();
            return txt;
        }


        /// <summary>
        /// Peeks at the next word, but the next word does not become current.
        /// </summary>
        /// <returns></returns>
        public Word PeekWord()
        {
            // At end of text ?
            if (_currentPosition >= _textLength) return Word.Empty;

            bool found = false;
            int pos = _currentPosition;
            string text = string.Empty;

            // Make sure the position stops at the end of the text
            while (pos < _textLength)
            {
                char currentChar = _text[pos];
                if (currentChar == ' ' || currentChar == '\t')
                {
                    found = true;
                    break;
                }
                else
                {
                    text += currentChar;
                }
                pos++;
            }
            if (!found) return Word.Empty;

            return new Word(text);
        }
    }
}
