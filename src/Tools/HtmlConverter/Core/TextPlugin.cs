using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.TextConverter
{

    public enum TriggerMode
    {
        Word,
        Character
    }
    
    /// <summary>
    /// Base class for plugins that can handle text conversion from one format to another.
    /// </summary>
    public class TextPlugin
    {
        /// <summary>
        /// The parser that reads words/lines.
        /// </summary>
        protected TextConversionParser _parser;
        public string[] Tokens = null;
        public string BlockText = "";
        public string ItemText = "";        
        public TextConversionContext Ctx;
        public bool IsBlockSupported { get; set; }
        public TriggerMode Trigger = TriggerMode.Word;


        /// <summary>
        /// The core parser that delegates controls to each plugin.
        /// </summary>
        public TextConversionParser Parser
        {
            get { return _parser; }
            set { _parser = value; }
        }


        /// <summary>
        /// Whether or not this plugin can handle the word trigger supplied.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public virtual bool CanHandle(string word)
        {
            return true;
        }


        /// <summary>
        /// Builds the start block for the plugin... eg.. open tag.
        /// </summary>
        /// <returns></returns>
        public virtual string StartBlock()
        {
            return "<" + this.BlockText + ">";
        }
        

        /// <summary>
        /// Builds the end block for the plugin .... eg. end tag.
        /// </summary>
        /// <returns></returns>
        public virtual string EndBlock()
	    {
		    return "</" + this.BlockText + ">";
	    }


        /// <summary>
        /// Converts 
        /// </summary>
        /// <returns></returns>
        public virtual string Convert()
        {
            string text = Parse() as string;
            return "<" + this.ItemText + ">" + text + "</" + this.ItemText + ">";
        }


        /// <summary>
        /// Parse the text.... not implemented and not valid in this base class.
        /// </summary>
        /// <returns></returns>
        public virtual object Parse()
        {
            return " BASE CLASS DEFAULT PARSE METHOD ";
        }
    }
}
