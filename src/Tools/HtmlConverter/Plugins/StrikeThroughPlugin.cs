using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.TextConverter.Plugins
{
    public class StrikeThroughPlugin : TextPlugin
    {
        public string strikeThroughTag = "s";

        /// <summary>
        /// Initialize using default values.
        /// </summary>
        public StrikeThroughPlugin()
            : this(new string[] { "~" })
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderedToken"></param>
        /// <param name="unOrderedToken"></param>
        public StrikeThroughPlugin(string[] token)
        {
            this.Tokens = token;
            this.Trigger = TriggerMode.Character;
        }


        public override string Convert()
        {
            var item = Parse() as string;
            if (item == null || item == "") return string.Empty;

            string html = "<" + strikeThroughTag + ">";
            html += item;
            html += "</" + strikeThroughTag + ">";

            return html;
        }


        /// <summary>
        /// Convert to html list.
        /// </summary>
        /// <returns></returns>
        public override object Parse()
        {
            // Parse until "~"
            var text = _parser.ReadUntil('~', _parser.Text, _parser.CurrentPosition);
            return text;
        }
    }
}
