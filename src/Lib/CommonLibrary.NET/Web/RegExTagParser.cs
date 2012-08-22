using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

using ComLib;

namespace ComLib.Web
{
    /// <summary>
    /// Parse to parse html/xml like tags in text.
    /// CAUTION!!! 
    /// This is a non-strict html light-weight implementation, instead of having a full lexical parser for html.
    /// By "non-strict" it does NOT take into account whether the parsed tags are nested inside an html comment or xml cdata
    /// <widget id="2" />
    /// </summary>
    public class RegExTagParser
    {
        private string _text;
        private string _tagName;
        private List<Tag> _tags;
        

        /// <summary>
        /// Parser for the tags.
        /// </summary>
        /// <param name="tagname">Name of tag.</param>
        public RegExTagParser(string tagname)
        {
            _tagName = tagname;
        }


        /// <summary>
        /// Parses the text and obtains a collection of the tags to parse.
        /// </summary>
        /// <param name="text">List of parsed tags.</param>
        public IList<Tag> Parse(string text)
        {
            _text = text;
            _tags = new List<Tag>();
            // \<component\s{1}\s*(?<name>[\w]+)=\"(?<val>[\w]+)\"\s*/\>
            string pattern = "\\<" + _tagName + "\\s{1}\\s*(?<name>[\\w]+)=\\\"(?<val>[\\w]+)\\\"\\s*/\\>";
            var matches = Regex.Matches(_text, pattern, RegexOptions.IgnoreCase);

            if (matches != null && matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    Tag tag = new Tag();
                    tag.Attributes = new OrderedDictionary();
                    tag.Name = _tagName;
                    tag.Position = match.Index;
                    tag.Length = match.Length;
                    Group name = match.Groups["name"];
                    if (name != null)
                    {   
                        Group val = match.Groups["val"];
                        if(val != null)
                        {
                            tag.Attributes[name.Value] = new KeyValue<string, string>(name.Value, val.Value);
                            _tags.Add(tag);
                        }
                    }                    
                }
            }
            return _tags;
        }




        /// <summary>
        /// Class to represent an html/xml like tag with attributes.
        /// </summary>
        public class Tag
        {
            /// <summary>
            /// Tag name.
            /// </summary>
            public string Name;


            /// <summary>
            /// Tag attributes.
            /// </summary>
            public OrderedDictionary Attributes;


            /// <summary>
            /// Tag inner content.
            /// </summary>
            public string InnerContent;

            /// <summary>
            /// Length of the tag.
            /// </summary>
            public int Length;


            /// <summary>
            /// Starting index of the tag in the entire doc.
            /// </summary>
            public int Position;
        }
    }
}
