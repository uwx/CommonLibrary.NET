using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.TextConverter.Plugins;


namespace ComLib.TextConverter
{
    /// <summary>
    /// Service to convert text from one format to another.
    /// </summary>
    public class TextConversionService
    {
        private IDictionary<string, TextPlugin> _plugins;
        private TextConversionParser _parser;


        /// <summary>
        /// Initialize
        /// </summary>
        public TextConversionService()
        {
            _plugins = new Dictionary<string, TextPlugin>();
            _parser = new TextConversionParser();
        }


        /// <summary>
        /// Register all text plugins.
        /// </summary>
        public void RegisterAll()
        {
            Register(new ListsPlugin());
            Register(new HeadingPlugin());
            Register(new BoldPlugin());
            Register(new ItalicsPlugin());
            Register(new UnderlinePlugin());
            Register(new StrikeThroughPlugin());
            Register(new SuperScriptPlugin());
            Register(new SubscriptPlugin());
        }


        /// <summary>
        /// Register a single text plugin.
        /// </summary>
        /// <param name="plugin"></param>
        public void Register(TextPlugin plugin)
        {
            plugin.Parser = _parser;
            foreach (var token in plugin.Tokens)
                _plugins[token] = plugin;
        }


        /// <summary>
        /// Parses the text.
        /// </summary>
        public string Convert(string text)
        {
            _parser.Init(text, _plugins);
            string converted = _parser.Parse();
            return converted;
        }
    }
}
