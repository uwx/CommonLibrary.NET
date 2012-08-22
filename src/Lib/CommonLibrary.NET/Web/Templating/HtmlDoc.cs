using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ComLib.Parsing;


namespace ComLib.Web.Templating
{
    public enum TokenType
    {
        None,
        StartTag,
        EndTag,
        Attr,
        AttrKey,
        AttrVal,
        InnerHtml,
        Comment
    } ;


    public class DomDoc
    {
        public string RawContent;
        public string Content;
        public IList<Tag> Tags = new List<Tag>();
    }

    
    public class DomDocParser
    {
        private bool _hasTagPrefix;
        private string _tagPrefix;
        private string _content = "";
        private TokenType _lastTokenType = TokenType.None;
        private List<string> _errors = new List<string>();
        private Stack<Tag> _tags = new Stack<Tag>();
        private ITokenReader _reader;
        protected string _openBracket = "<";
        protected string _closeBracket = ">";
        protected StringBuilder _innerContent = new StringBuilder();


        /// <summary>
        /// Initialize using defaults.
        /// </summary>
        public DomDocParser() : this(null, "<", ">")
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="tagPrefix">$</param>
        /// <param name="openBracket">[</param>
        /// <param name="closeBracket">]</param>
        public DomDocParser(string tagPrefix, string openBracket, string closeBracket)
        {
            _tagPrefix = tagPrefix;
            _hasTagPrefix = !string.IsNullOrEmpty(_tagPrefix);
            _openBracket = openBracket;
            _closeBracket = closeBracket;
        }


        /// <summary>
        /// Pareses the content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public DomDoc Parse(string content)
        {
            _content = content;
            var reader = new TokenReader(_content, "/", new string[] { }, new string[] { " " }, new string[] { Environment.NewLine });
            return Parse(reader, false);
        }


        /// <summary>
        /// Parse using the supplied token reader.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public DomDoc Parse(ITokenReader reader, bool parseTagsOnly)
        {
            _reader = reader;
            _reader.ReadChar();
            while(!_reader.IsEnd())
            {
                string current = _reader.CurrentChar;
                var nextchar = _reader.PeekChar();

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
                else if (!_hasTagPrefix && current == _openBracket && nextchar != "/")
                    ParseTagStart(true);

                // Empty tag
                else if (current == "/" && nextchar == _closeBracket
                    && (_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr))
                { _tags.Peek().IsEmpty = true; _reader.ReadChar(); _lastTokenType = TokenType.EndTag; }

                // End tag [/div]
                else if (current == _openBracket && nextchar == "/")
                    ParseTagEnd(true);

                // End of start tag ]
                else if (current == _closeBracket && (_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr))
                    ParseInnerHtml(true);

                // Remove empty space between starttag and attributes and between attributes themselves.
                else if ((_lastTokenType == TokenType.Attr || _lastTokenType == TokenType.StartTag) && current == " ")
                    _reader.ConsumeWhiteSpace(false, false);

                // Character for antoher attribute.
                else if ((_lastTokenType == TokenType.StartTag || _lastTokenType == TokenType.Attr) && Char.IsLetter(current[0]))
                    ParseAttribute();

                // Other non tag related content.
                else if (_lastTokenType == TokenType.EndTag || _lastTokenType == TokenType.None)
                    _innerContent.Append(current);

                else Console.WriteLine(current);

                _reader.ReadChar();
            }

            var doc = new DomDoc();
            doc.Tags = new List<Tag>(_tags);
            doc.RawContent = _content;
            doc.Content = _innerContent.ToString();
            return doc;
        }


        /// <summary>
        /// Parses the start tag.
        /// </summary>
        /// <param name="advance"></param>
        /// <returns></returns>
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
                if (next == " " || next == "/") read = false;

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
        /// <param name="advance"></param>
        /// <returns></returns>
        private string ParseTagEnd(bool advance)
        {
            string name = _reader.ReadTokenUntil(_closeBracket, "/", advance, true, false, true, false);
            var lastTag = _tags.Peek();
            if (string.Compare(name, lastTag.Name, true) != 0)
                _errors.Add("Expected end tag : " + lastTag.Name + ", but found : " + name);
            _lastTokenType = TokenType.EndTag;
            return name;
        }


        /// <summary>
        /// Parses an attribute key/value pair.
        /// </summary>
        /// <returns></returns>
        private KeyValue<string, string> ParseAttribute()
        {
            var key = _reader.ReadTokenUntil("=", "/", false, true, false, false, true);
            _reader.ConsumeWhiteSpace(false);
            var quote = _reader.CurrentChar;
            var value = _reader.ReadTokenUntil(quote, "/", true, true, false, true, false);
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
