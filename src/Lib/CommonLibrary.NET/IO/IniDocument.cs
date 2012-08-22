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
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using ComLib;




namespace ComLib.IO
{
    /// <summary>
    /// IniDocument to handle loading and writing(not yet done)
    /// of ini files.
    /// 
    /// This class can load a MULTI-LINE ini file into a dictionary like data structure.
    /// 
    /// [BlogPost1]
    /// Title : Introduction to Oil painting class.
    /// Description : "Learn how to paint using
    /// oil, in this beginners class for painting enthusiats."
    /// Url : http://www.knowledgedrink.com
    /// </summary>
    public class IniDocument : ConfigSource
    {
        private string _iniContent = "";
        private string _iniFilePath = "";
        private bool _isFileBased = false;
        private bool _isCaseSensitive = false;


        /// <summary>
        /// Default initialization.
        /// </summary>
        public IniDocument() { }


        /// <summary>
        /// Initialize using IniSections.
        /// </summary>
        /// <param name="sections"></param>
        public IniDocument(IList<ConfigSection> sections)
        {
            sections.ForEach(s => Add(s.Name, s));
        }


        /// <summary>
        /// Initialize using IniSections.
        /// </summary>
        /// <param name="iniContentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        public IniDocument(string iniContentOrFilePath, bool isFilePath)
            : this("", iniContentOrFilePath, isFilePath, true)
        {
        }


        /// <summary>
        /// Initialize using IniSections.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iniContentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        public IniDocument(string name, string iniContentOrFilePath, bool isFilePath)
            : this(name, iniContentOrFilePath, isFilePath, true)
        {
        }


        /// <summary>
        /// Initialize the ini document with the string or file path.
        /// </summary>
        /// <param name="iniContentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        /// <param name="isCaseSensitive"></param>
        public IniDocument(string iniContentOrFilePath, bool isFilePath, bool isCaseSensitive)
            : this("", iniContentOrFilePath, isFilePath, isCaseSensitive)
        {            
        }


        /// <summary>
        /// Initialize the ini document with the string or file path.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iniContentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        /// <param name="isCaseSensitive"></param>
        public IniDocument(string name, string iniContentOrFilePath, bool isFilePath, bool isCaseSensitive)
        {
            Init(name, iniContentOrFilePath, isFilePath, isCaseSensitive);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="iniContentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        /// <param name="isCaseSensitive"></param>
        public void Init(string name, string iniContentOrFilePath, bool isFilePath, bool isCaseSensitive)
        {
            Name = name;
            _iniContent = iniContentOrFilePath;
            _isCaseSensitive = isCaseSensitive;
            if (isFilePath)
            {
                _iniFilePath = iniContentOrFilePath;
                _iniContent = File.ReadAllText(iniContentOrFilePath);
                _isFileBased = true;
            }
            Load();
        }


        /// <summary>
        /// Source path of this file.
        /// </summary>
        public override string SourcePath
        {
            get
            {
                if( _isFileBased ) return _iniFilePath;

                return string.Empty;
            }
        }


        /// <summary>
        /// Load settings.
        /// </summary>
        public override void Load()
        {
            // Handle empty ini file.
            // This is possible and very possible for inherited configs.
            if (string.IsNullOrEmpty(_iniContent))
                return;

            IIniParser parser = new IniParser();
            parser.Settings.IsCaseSensitive = _isCaseSensitive;
            List<IniSection> sections = null;
            sections = parser.Parse(_iniContent);
            sections.ForEach(s => AddMulti(s.Name, s, false));
        }


        /// <summary>
        /// Save the document to file.
        /// </summary>
        public override void Save()
        {
            this.Save(this._iniFilePath);
        }


        /// <summary>
        /// Save the document to the filepath specified.
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            StringBuilder buffer = new StringBuilder();
            List<string> sections = this.Sections;
            for(int ndx = 0; ndx < sections.Count; ndx++)
            {
                string sectionName = sections[ndx];
                IniSection section = GetSection(sectionName) as IniSection;
                string sectionContent = section.ToString();
                buffer.Append(sectionContent);
                buffer.Append(Environment.NewLine);
            }
            string fullContent = buffer.ToString();
            File.WriteAllText(filePath, fullContent);
        }
    }



    /// <summary>
    /// Class to represent an IniSection/Group and which also stores the entries
    /// associated under the section/group.
    /// e.g. 
    /// [group1]
    /// key1 = value1
    /// key2 = value2
    /// </summary>
    public class IniSection : ConfigSection
    {

        #region ICloneable Members
        /// <summary>
        /// Create shallow copy.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion


        /// <summary>
        /// Return the ini format of the contents.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            string name = string.IsNullOrEmpty(Name) ? "section" : Name;

            buffer.Append("[" + name + "]" + Environment.NewLine);
            foreach (DictionaryEntry pair in this)
            {
                buffer.Append(pair.Key.ToString() + " : ");
                string val = pair.Value as string;
                if (!val.Contains("\""))
                {
                    buffer.Append(val);
                }
                else
                {
                    string encoded = val.Replace("\"", "\\\"");
                    buffer.Append(encoded);
                }
                buffer.Append(Environment.NewLine);
            }
            return buffer.ToString();
        }
    }



    /// <summary>
    /// The type of the ini line type.
    /// </summary>
    enum IniLineType { Group, KeyValue, Comment, Other, EmptyLine, None }



    /// <summary>
    /// Parser constants.
    /// </summary>
    class IniParserConstants
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
        /// <summary>
        /// The maximum length of a comment.
        /// </summary>
        public int MaxLenghtOfComment = 500;


        /// <summary>
        /// The maximum length of a group name [group]
        /// </summary>
        public int MaxLengthOfGroup = 40;


        /// <summary>
        /// The maximum length of a key key:.
        /// </summary>
        public int MaxLengthOfKey = 40;


        /// <summary>
        /// The maximum length of a value in a single line.
        /// </summary>
        public int MaxLenghtOfValueSingleLine = 500;


        /// <summary>
        /// Whether or not the groups/keys are case-sensitive
        /// </summary>
        public bool IsCaseSensitive = false;
    }



    /// <summary>
    /// Interface for an IniParser, This is a parser that supports
    /// parsing multiple lines for the values.
    /// e.g. 
    /// 
    /// [post1]
    /// title: My first post
    /// description: " This is a ini parser that can handle
    /// parsing multiples lines for the value. "
    /// 
    /// </summary>
    public interface IIniParser
    {
        /// <summary>
        /// Parse the ini content.
        /// </summary>
        /// <param name="iniContent"></param>
        /// <returns></returns>
        List<IniSection> Parse(string iniContent);
        
        /// <summary>
        /// Settings for the parser.
        /// </summary>
        IniParserSettings Settings { get; set; }

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
        private Scanner _reader = null;
        //private IniLineType _lastLineType = IniLineType.None;
        private List<string> _errors = new List<string>();
        List<IniSection> _sections = new List<IniSection>();
        IniSection _currentSection = new IniSection();
        #endregion


        /// <summary>
        /// Create new instance with default settings
        /// </summary>
        public IniParser()
        {
            Settings = new IniParserSettings();
        }


        private IniParserSettings _settings;


        /// <summary>
        /// The settings for the parser.
        /// </summary>
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

            return _sections;
        }


        /// <summary>
        /// Initialize the reader and the stack.
        /// </summary>
        /// <param name="text"></param>
        private void Init(string text)
        {
            _inputText = text;
            _reader = new Scanner(text, new char[] { '[', ']', ':' });
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
            _reader.ReadChar();

            // Consume End Of Line.
            _reader.ConsumeNewLines();
            bool readNextChar = true;
            // While not end of content.
            while ( !_reader.IsEnded() )
            {
                // Get the current char.
                char currentChar = _reader.CurrentChar;

                if (currentChar == '[')
                {
                    StoreGroup();
                    readNextChar = false;
                }
                else if (currentChar == ';' )
                {
                    string comment = _reader.ReadToEol();                   
                }
                else if (_reader.IsEol())
                {
                    _reader.ConsumeNewLines();
                    readNextChar = false;
                }
                else
                {
                    StoreKeyValue();
                    _reader.ConsumeNewLines();
                    readNextChar = false;
                }

                // Read the next char.
                if(readNextChar) _reader.ReadChar();
            }

            // Add the last section.
            // Handle null/emtpy sections.
            if(_currentSection.Count > 0 && !string.IsNullOrEmpty(_currentSection.Name))
                _sections.Add(_currentSection);
        }


        /// <summary>
        /// Store the current group
        /// </summary>
        protected virtual void StoreGroup()
        {
            // Push the last one into the list.
            if (_currentSection.Count > 0)
                _sections.Add(_currentSection);

            string group = _reader.ReadToken(']', '\\', false, true, true, true);
            if (!_settings.IsCaseSensitive)
                group = group.ToLower();

            // Create a new section using the name of the group.
            _currentSection = new IniSection() { Name = group };
            //_lastLineType = IniLineType.Group;
        }


        /// <summary>
        /// Store the current key value.
        /// </summary>
        protected virtual void StoreKeyValue()
        {
            string key = _reader.ReadToken(':', '\\', false, false, true, true);
            string val = "";

            _reader.ConsumeWhiteSpace();
            // If starting with " then possibly multi-line.
            if (_reader.CurrentChar == '"')
                val = _reader.ReadToken('"', '\\', false, true, true, true);
            else
                val = _reader.ReadToEol();

            if (!_settings.IsCaseSensitive)
                key = key.ToLower();

            // This allow multiple values for the same key.
            // Multiple values are stored using List<object>.
            _currentSection.AddMulti(key, val, false);
            //_lastLineType = IniLineType.KeyValue;
        }


        /// <summary>
        /// Confirm that the input text is valid text
        /// </summary>
        /// <returns></returns>
        private BoolMessage ValidateText()
        {
            if (string.IsNullOrEmpty(_inputText))
                return new BoolMessage(false, "Empty text");

            return BoolMessage.True;
        }
        #endregion
    }
}
