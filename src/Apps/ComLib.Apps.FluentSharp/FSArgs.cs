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
        /// Path to the template or code file to run.
        /// </summary>
        [Arg(Name = "filepath", Description = "Path to the file to execute", DataType = typeof(string), IsRequired = true, DefaultValue = "", Example = "c:\\tests\\file1.js")]
        public string FilePath { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        [Arg(Name = "logfolder", Description = "Path to logfolder", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\logs")]
        public string LogFolder { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        [Arg(Name = "outputfolder", Description = "Path to the output folder for rendering templates", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\output")]
        public string OutPutFolder { get; set; }

        
        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "istemplate", Description = "Whether or not the file is a template file to render", DataType = typeof(bool), IsRequired = false, DefaultValue = false, Example = "true")]
        public bool IsTemplate { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        [Arg(Name = "tokenize", Description = "Whether or not to convert to script into set of tokens for debugging purposes", DataType = typeof(bool), IsRequired = false, DefaultValue = false, Example = "true")]
        public bool Tokenize { get; set; }
    }
}
