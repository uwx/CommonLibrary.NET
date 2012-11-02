using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Arguments;


namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// Arguments on commandline.
    /// </summary>
    public class FSArgs
    {
        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "tokens", Alias = "t", Description = "Converts the script into set of tokens( for debugging purposes )", DataType = typeof(bool), IsRequired = false, DefaultValue = false, ExampleMultiple = "true | false")]
        public bool Tokenize { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "nodes", Alias = "n", Description = "Converts the script into a tree of nodes( for debugging purposes )", DataType = typeof(bool), IsRequired = false, DefaultValue = false, ExampleMultiple = "true | false")]
        public bool Nodes { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "exec", Alias = "e", Description = "Executes the script", DataType = typeof(bool), IsRequired = false, DefaultValue = false, ExampleMultiple = "true | false")]
        public bool Execute { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "translate", Alias = "tr", Description = "Translates the script into another language( not fully supported yet )", DataType = typeof(bool), IsRequired = false, DefaultValue = false, ExampleMultiple = "true | false")]
        public bool Translate { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "interactive", Alias = "i", Description = "Runs fluentscript in interactive mode( read eval print loop )", DataType = typeof(bool), IsRequired = false, DefaultValue = false, ExampleMultiple = "true | false")]
        public bool Interactive { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        [Arg(Name = "out", Alias = "o", Description = "Output path for a file( for flag tokens/nodes/translate )", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\output")]
        public string Out { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        [Arg(Name = "logs", Alias = "l", Description = "Log folder path( for debugging purposes )", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\output")]
        public string Logs { get; set; }

        
        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "istemplate", Alias = "templ", Description = "Whether or not the file is a template file to render", DataType = typeof(bool), IsRequired = false, DefaultValue = false, Example = "true")]
        public bool IsTemplate { get; set; }

                
        /// <summary>
        /// The plugin group to register. e.g. "sys", "all", "explicit:var,if"
        /// </summary>
        [Arg(Name = "plugins", Description = "The group of plugins to use", DataType = typeof(bool), IsRequired = false, DefaultValue = "all", Example = "true")]
        public string Plugins { get; set; }


        /// <summary>
        /// Path to the template or code file to run.
        /// </summary>
        [Arg(IndexPosition = 0, Description = "Path to the file to execute", DataType = typeof(string), IsRequired = true, DefaultValue = "", Example = "c:\\tests\\file1.js")]
        public string FilePath { get; set; }

    }
}
