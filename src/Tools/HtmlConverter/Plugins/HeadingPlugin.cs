using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.TextConverter.Plugins
{
    /// <summary>
    /// 
    /// </summary>
    public class HeadingPlugin : TextPlugin
    {
        private Dictionary<string, string> _conversionMappings;
        private string _currentItemHeading = null;


        /// <summary>
        /// Initialize using default values.
        /// </summary>
        public HeadingPlugin()
            : this(null, null, new string[] { "!", "!!", "!!!", "!!!!", "!!!!!", "!!!!!!" })
        {            
        }


        /// <summary>
        /// Initialize with parameters.
        /// </summary>
        /// <param name="blockText">The text to use to surround this in a block.</param>
        /// <param name="itemWrapText">Text to use to wrap a single item</param>
        /// <param name="tokens">The tokens that trigger the execution of this plugin.</param>
        public HeadingPlugin(string blockText, string itemWrapText, string[] tokens)
        {
            // Set the tokens to be that starting tokens that trigger the execution of this plugin.
            // Case 1: # <text> indicates converting to numbered list ( ordered )
            // Case 2: * <text> indicates converting to bulleted list ( unordered )
            this.Tokens = tokens;
            this.IsBlockSupported = false;
            this._conversionMappings = new Dictionary<string, string>();

            // _conversionMappings["!"] = "h1";
            // _conversionMappings["!!"] = "h2";
            // _conversionMappings["!!!"] = "h3";
            // _conversionMappings["!!!!"] = "h4";
            // _conversionMappings["!!!!!"] = "h5";
            // _conversionMappings["!!!!!!"] = "h6";
            for (int ndx = 0; ndx < tokens.Length; ndx++)
            {
                string token = tokens[ndx];
                string replacement = "h" + ( ndx + 1 ).ToString();
                this._conversionMappings[token] = replacement;
            }            
        }


        /// <summary>
        /// Converts from one heading format ! to another e.g. h1.
        /// </summary>
        /// <returns></returns>
        public override string Convert()
        {
            string text = Parse() as string;

            // Map to h1 h2 etc.
            string replacement = _conversionMappings[_currentItemHeading];

            // <h1>some text</h1>
            string html = "<" + replacement + ">" + text + "</" + replacement + ">";
            return html;
        }



        /// <summary>
        /// Convert to html list.
        /// </summary>
        /// <returns></returns>
        public override object Parse()
        {
            // Current word is either ! !! !!! !!!! !!!!!
            _currentItemHeading = _parser.CurrentWord;
            return _parser.ReadLine();
        }
    }
}
