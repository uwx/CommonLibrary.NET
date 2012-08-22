using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace ComLib.Macros
{

    /// <summary>
    /// Enums used for keeping state of parsing when parsing text containing macros.
    /// </summary>
    enum TokenType
    {
        /// <summary>
        /// No type.
        /// </summary>
        None,


        /// <summary>
        /// The start tag of the macro
        /// </summary>
        StartTag,


        /// <summary>
        /// The end tag of the macro
        /// </summary>
        EndTag,


        /// <summary>
        /// An attribute of the macro
        /// </summary>
        Attr,


        /// <summary>
        /// An attribute name
        /// </summary>
        AttrKey,


        /// <summary>
        /// An attribute value
        /// </summary>
        AttrVal,


        /// <summary>
        /// The inner html inside a macro start and end tag.
        /// </summary>
        InnerHtml,


        /// <summary>
        /// A comment
        /// </summary>
        Comment
    } ;



    /// <summary>
    /// Class to represent a Macro Document after having been parsed for macros.
    /// </summary>
    public class MacroDoc
    {
        /// <summary>
        /// The raw content of the document before parsing for macros.
        /// </summary>
        public string RawContent;


        /// <summary>
        /// The content of the document after marcros have been removed
        /// </summary>
        public string Content;


        /// <summary>
        /// The list of macro tags in the document. Each tag has a reference to it starting position.
        /// </summary>
        public IList<Tag> Tags = new List<Tag>();
    }

    

    /// <summary>
    /// Parses a string / text for macros.
    /// </summary>
    public class MacroDocParser
    {
        private bool _hasTagPrefix;
        private char _tagPrefix;
        private string _content = "";
        private TokenType _lastTokenType = TokenType.None;
        private List<string> _errors = new List<string>();
        private Stack<Tag> _tags = new Stack<Tag>();
        private Scanner _reader;

        /// <summary>
        /// The character representing the opening bracket for a macro.
        /// </summary>
        protected char _openBracket = '<';


        /// <summary>
        /// The character representing the closing bracket for a macro.
        /// </summary>
        protected char _closeBracket = '>';


        /// <summary>
        /// Buffer to store the content of the text without macros.
        /// </summary>
        protected StringBuilder _innerContent = new StringBuilder();


        /// <summary>
        /// Initialize using defaults.
        /// </summary>
        public MacroDocParser() : this(char.MinValue, '<', '>')
        {
        }


        /// <summary>
        /// Initialize a new instance of this class.
        /// </summary>
        /// <param name="tagPrefix">$</param>
        /// <param name="openBracket">[</param>
        /// <param name="closeBracket">]</param>
        public MacroDocParser(char tagPrefix, char openBracket, char closeBracket)
        {
            _tagPrefix = tagPrefix;
            _hasTagPrefix = _tagPrefix != char.MinValue;
            _openBracket = openBracket;
            _closeBracket = closeBracket;
        }


        /// <summary>
        /// Parses the content.
        /// </summary>
        /// <param name="content">Content to parse.</param>
        /// <returns>Parsed content.</returns>
        public MacroDoc Parse(string content)
        {
            _content = content;
            var reader = new Scanner(_content, '/', new char[] { }, new char[] { ' ' } );
            return Parse(reader, false);
        }


        /// <summary>
        /// Parse using the supplied token reader.
        /// </summary>
        /// <param name="reader">Instance of token reader.</param>
        /// <param name="parseTagsOnly">True to parse tags only.</param>
        /// <returns>Parsed content.</returns>
        public MacroDoc Parse(Scanner reader, bool parseTagsOnly)
        {
            _reader = reader;
            _reader.ReadChar();
            while(!_reader.IsEnded())
            {
                char current = _reader.CurrentChar;
                char nextchar = _reader.PeekChar();

                // Escaping tag via prefix. $$
                if (_hasTagPrefix && current == _tagPrefix && nextchar == _tagPrefix)
                { _innerContent.Append(current); _reader.ReadChar(); }

                // Escaping tag with open bracket - no tag prefix. [[
                else if (!_hasTagPrefix && current == _openBracket && nextchar == _openBracket)
                { _innerContent.Append(current); _reader.ReadChar(); }

                // Start tag. $[div
                else if (_hasTagPrefix && current == _tagPrefix && nextchar == _openBracket)
                { _reader.ReadChar(); ParseTagStart(true); }

                //  Weird scenario w/ []
                else if (current == _openBracket && nextchar == _closeBracket)
                { _innerContent.Append(current); _innerContent.Append(_reader.ReadChar()); }

                // Start tag no prefix
                else if (!_hasTagPrefix && current == _openBracket && nextchar != '/')
                    ParseTagStart(true);

                // Empty tag
                else if (current == '/' && nextchar == _closeBracket
                    && (_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr))
                { 
                    var lastTag = _tags.Peek();
                    lastTag.IsEmpty = true;
                    _reader.ReadChar();
                    lastTag.Length = (_reader.Position - lastTag.Position) + 1;
                    _lastTokenType = TokenType.EndTag; 
                }

                // End tag [/div]
                else if (current == _openBracket && nextchar == '/')
                    ParseTagEnd(true);

                // End of start tag ]
                else if (current == _closeBracket && (_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr))
                    ParseInnerHtml(true);

                // Remove empty space between starttag and attributes and between attributes themselves.
                else if ((_lastTokenType == TokenType.Attr || _lastTokenType == TokenType.StartTag) && current == ' ')
                    _reader.ConsumeWhiteSpace(false, false);

                // Character for antoher attribute.
                else if ((_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr) && Char.IsLetter(current))
                    ParseAttribute();

                // Other non tag related content.
                else if (_lastTokenType == TokenType.EndTag || _lastTokenType == TokenType.None)
                    _innerContent.Append(current);

                else Console.WriteLine(current);

                _reader.ReadChar();
            }

            var doc = new MacroDoc();

            // The tags are in a stack ( most recent is index 0 )
            // So reverse the order when building the list of tags so they come in sequential order.
            var tags = _tags.ToArray();
            for(int ndx = tags.Length - 1; ndx >= 0; ndx--) doc.Tags.Add(tags[ndx]);
            doc.RawContent = _content;
            doc.Content = _innerContent.ToString();
            return doc;
        }


        /// <summary>
        /// Parses the start tag.
        /// </summary>
        /// <param name="advance">True to advance a character
        /// before parsing.</param>
        /// <returns>Parsed tag.</returns>
        private string ParseTagStart(bool advance)
        {
            var tag = new Tag();       
            var pos = _reader.Position;
            tag.Position = _hasTagPrefix ? pos - 1 : pos;

            if (advance) _reader.ReadChar();

            bool read = true;
            var buffer = new StringBuilder();
            var next = _reader.PeekChar();
            while (read && !_reader.IsAtEnd())
            {
                buffer.Append(_reader.CurrentChar);
                
                // slash indicates empty tag and space indicates possible attributes.
                if (next == ' ' || next == '/' || next == _closeBracket) read = false;

                if (read)
                {
                    _reader.ReadChar();
                    next = _reader.PeekChar();
                }
            }
            string name = buffer.ToString();
            tag.Name = name.Trim();   
            _tags.Push(tag);
            _lastTokenType = TokenType.StartTag;
            return name;
        }


        /// <summary>
        /// Parses the end tag.
        /// </summary>
        /// <param name="advance">True to advance a character
        /// before parsing.</param>
        /// <returns>Parsed tag.</returns>
        private string ParseTagEnd(bool advance)
        {
            string name = _reader.ReadTokenUntil(_closeBracket, '/', advance, true, false, true, false);
            var lastTag = _tags.Peek();
            if (string.Compare(name, lastTag.Name, true) != 0)
                _errors.Add("Expected end tag : " + lastTag.Name + ", but found : " + name);
            _lastTokenType = TokenType.EndTag;
            lastTag.Length = (_reader.Position - lastTag.Position) + 1;
            return name;
        }


        /// <summary>
        /// Parses an attribute key/value pair.
        /// </summary>
        /// <returns></returns>
        private KeyValue<string, string> ParseAttribute()
        {
            var key = _reader.ReadTokenUntil('=', '/', false, true, false, false, true);
            _reader.ConsumeWhiteSpace(false);
            var quote = _reader.CurrentChar;
            var value = _reader.ReadTokenUntil(quote, '/', true, true, false, true, false);
            var attr = new KeyValue<string, string>(key, value);
            _tags.Peek().Attributes.Add(key, value);
            _lastTokenType = TokenType.Attr;
            return attr;
        }


        /// <summary>
        /// Parses the innerhtml.
        /// </summary>
        /// <returns></returns>
        private string ParseInnerHtml(bool advance)
        {
            if(advance) _reader.ReadChar();

            bool read = true;
            var buffer = new StringBuilder();
            var next = _reader.PeekChar();
            while(read && !_reader.IsAtEnd())
            {
                buffer.Append(_reader.CurrentChar);
                if (next == _openBracket)
                {
                    string next1 = _reader.PeekChars(2);
                    if (next1[1] == '/')
                        read = false;
                }
                if (read)
                {
                    _reader.ReadChar();
                    next = _reader.PeekChar();
                }
            }
            string html = buffer.ToString();

            _tags.Peek().InnerContent = html;
            _lastTokenType = TokenType.InnerHtml;
            return html;
        }
    }
}
