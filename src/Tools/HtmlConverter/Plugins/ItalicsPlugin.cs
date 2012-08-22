using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.TextConverter;

namespace ComLib.TextConverter.Plugins
{
    public class ItalicsPlugin : TextPlugin
    {
        public string italicsTag = "i";

        /// <summary>
        /// Initialize using default values.
        /// </summary>
        public ItalicsPlugin()
            : this(new string[] { "_" })
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderedToken"></param>
        /// <param name="unOrderedToken"></param>
        public ItalicsPlugin(string[] token)
        {
            this.Tokens = token;
            this.Trigger = TriggerMode.Character;
        }


        public override string Convert()
        {
            var item = Parse() as string;
            if (item == null || item == "") return string.Empty;

            string html = "<" + italicsTag + ">";
            html += item;
            html += "</" + italicsTag + ">";

            return html;
        }


        /// <summary>
        /// Convert to html list.
        /// </summary>
        /// <returns></returns>
        public override object Parse()
        {
            // Parse until "_"
            var text = _parser.ReadUntil('_', _parser.Text, _parser.CurrentPosition);
            return text;
        }
    }
}
