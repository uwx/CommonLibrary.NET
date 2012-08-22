using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.TextConverter.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public class ListsPlugin : TextPlugin
    {
        private bool _isOrderedList = false;
        private string _unOrderedToken = null;
        private string _orderedToken = null;


        /// <summary>
        /// Initialize using default values.
        /// </summary>
        public ListsPlugin() : this( "#", "$" )
        {            
        }


        /// <summary>
        /// Initialize with supplied tokens represented ordered/unordered lists.
        /// </summary>
        /// <param name="orderedToken"></param>
        /// <param name="unOrderedToken"></param>
        public ListsPlugin(string orderedToken, string unOrderedToken)
        {
            // Set the tokens to be that starting tokens that trigger the execution of this plugin.
            // Case 1: # <text> indicates converting to numbered list ( ordered )
            // Case 2: * <text> indicates converting to bulleted list ( unordered )
            this.Tokens = new string[]{ orderedToken, unOrderedToken };

            // Default the block text to "ul" but this can also be "ol" depending on ordered/unordered lists.
            this.BlockText = "ul";
            this.ItemText = "li";
            this._orderedToken = orderedToken;
            this._unOrderedToken = unOrderedToken;
        }


        public override string Convert()
        {
            var items = Parse() as List<string>;
            if (items == null || items.Count == 0) return string.Empty;

            // There were list items.
            string blockText = _isOrderedList ? "ol" : "ul";
            string html = "<" + blockText + ">";

            foreach (var item in items)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</" + blockText + ">";
            return html;
        }


        /// <summary>
        /// Convert to html list.
        /// </summary>
        /// <returns></returns>
        public override object Parse()
        {            
            _isOrderedList = _parser.CurrentWord == _orderedToken;
            var items = new List<string>();
            var item = _parser.ReadLine();
            items.Add(item);

            // Any more items ?
            // Peek at the next word WITHOUT moving to the next word.
            Word nextWord = _parser.PeekWord();
            while (nextWord != Word.Empty && nextWord.Text != _unOrderedToken && nextWord.Text != _orderedToken)
            {
                // Advance to the next word. This will match the
                _parser.NextWord();
                
                // Now read the entire line again.
                item = _parser.ReadLine();
                items.Add(item);

                nextWord = _parser.PeekWord();
            }
            return items;
        }
    }
}
