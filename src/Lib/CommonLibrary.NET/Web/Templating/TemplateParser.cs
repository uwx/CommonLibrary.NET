using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

using ComLib;


namespace ComLib.Web.Templating
{
    /// <summary>
    /// Parse to parse html/xml like tags in text.
    /// CAUTION!!! 
    /// This is a non-strict html light-weight implementation, instead of having a full lexical parser for html.
    /// By "non-strict" it does NOT take into account whether the parsed tags are nested inside an html comment or xml cdata
    /// <widget id="2" />
    /// </summary>
    public class TemplateParser
    {
        private string _text;
        private string _tagPrefix;
        private List<Tag> _tags;
        

        /// <summary>
        /// Parser for the tags.
        /// </summary>
        /// <param name="tagname"></param>
        public TemplateParser(string tagPrefix)
        {
            _tagPrefix = tagPrefix;
        }


        /// <summary>
        /// Parses the text and obtains a collection of the tags to parse.
        /// </summary>
        /// <param name="text"></param>
        public IList<Tag> Parse(string text)
        {
            _text = text;
            _tags = new List<Tag>();
            // \<pch\:(?<tag>[\w]+)\s{1}\s*(?<name>[\w]+)=\"(?<val>[\w]+)\"\s*/\>
            string pattern = "\\<" + _tagPrefix + "\\:(?<tag>[\\w]+)\\s{1}\\s*(?<name>[\\w]+)=\\\"(?<val>[\\w]+)\\\"\\s*/\\>";
            var matches = Regex.Matches(_text, pattern, RegexOptions.IgnoreCase);

            if (matches != null && matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    Tag tag = new Tag();
                    tag.Attributes = new OrderedDictionary();
                    tag.Position = match.Index;
                    tag.Length = match.Length;
                    Group taggroup = match.Groups["tag"];
                    if (tag != null)
                    {
                        tag.Name = taggroup.Value;
                        Group name = match.Groups["name"];
                        if (name != null)
                        {                            
                            Group val = match.Groups["val"];
                            if (val != null)
                            {
                                if (string.Compare(name.Value, "id", true) == 0)
                                    tag.Id = Convert.ToInt32(val.Value);

                                tag.Attributes[name.Value] = new KeyValue<string, string>(name.Value, val.Value);
                                _tags.Add(tag);
                            }
                        }
                    }
                }
            }
            return _tags;
        }
    }
}
