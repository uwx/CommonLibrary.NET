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
    public class Templater
    {
        private string _tagPrefix;
        private Func<Tag, string> _tagBuilder;
        private TemplateParser _parser;
            

        /// <summary>
        /// Initialize the tag lookup
        /// </summary>
        /// <param name="components"></param>
        public void Init(string tagPrefix, Func<Tag, string> tagBuilder)
        {
            _tagPrefix = tagPrefix;
            _tagBuilder = tagBuilder;
            _parser = new TemplateParser(_tagPrefix);
        }


        /// <summary>
        /// Builds up the template by using the existing text in the template and replacing 
        /// the tags with their respective text.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Process(string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            var tags = _parser.Parse(content);
            if (tags == null || tags.Count == 0)
                return content;

            var buffer = new StringBuilder();
            int start = 0;
            int tagIndex = 0;

            while (start < content.Length && tagIndex < tags.Count)
            {
                var tag = tags[tagIndex];
                string html = content.Substring(start, tag.Position - start);
                buffer.Append(html);

                var attribute = tag.Attributes["id"] as KeyValue<string, string>;                
                var widgetContent = _tagBuilder(tag);
                buffer.Append(widgetContent);

                start = tag.Position + tag.Length;
                tagIndex++;
            }
            if (start < content.Length)
                buffer.Append(content.Substring(start));
            string finalhtml = buffer.ToString();
            return finalhtml;
        }
    }
}
