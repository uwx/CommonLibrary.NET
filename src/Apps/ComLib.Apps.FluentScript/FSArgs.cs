using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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
        //[Arg(Name = "filepath", Description = "Path to the file to execute", DataType = typeof(string), IsRequired = true, DefaultValue = "", Example = "c:\\tests\\file1.js")]
        public string FilePath { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        //[Arg(Name = "logfolder", Description = "Path to logfolder", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\logs")]
        public string LogFolder { get; set; }


        /// <summary>
        /// The folder to generate the output to.
        /// </summary>
        //[Arg(Name = "outputfolder", Description = "Path to the output folder for rendering templates", DataType = typeof(string), IsRequired = false, DefaultValue = "", Example = "c:\\tests\\output")]
        public string OutPutFolder { get; set; }

        
        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        //[Arg(Name = "istemplate", Description = "Whether or not the file is a template file to render", DataType = typeof(bool), IsRequired = false, DefaultValue = false, Example = "true")]
        public bool IsTemplate { get; set; }


        /// <summary>
        /// Whether or not the filepath supplied is a template or just code.
        /// </summary>
        //[Arg(Name = "tokenize", Description = "Whether or not to convert to script into set of tokens for debugging purposes", DataType = typeof(bool), IsRequired = false, DefaultValue = false, Example = "true")]
        public bool Tokenize { get; set; }


        /// <summary>
        /// The plugin group to register. e.g. "sys", "all", "explicit:var,if"
        /// </summary>
        public string PluginGroup { get; set; }


        /// <summary>
        /// Gets the help text for the args.
        /// </summary
        /// <returns></returns>
        public string ToHelpString()
        {
            var args = new List<List<object>>()
            {
                                    // name         required    
                new List<object>(){ "file",         "REQUIRED",      "string",     @"c:\tests\file.js",    "The path to the fluentscript file" },
                new List<object>(){ "logfolder",    "optional",      "string",     @"c:\fs\logs",          "The log folder of fluentscript for any error" },
                new List<object>(){ "outfolder",    "optional",      "string",     @"c:\fs\out",           "The output folder to write out the processed template file if script is a template" },
                new List<object>(){ "tokenize",     "optional",      "bool",       @"true",                "Whether or not to print out the tokens in the script rather than executing script" },
                new List<object>(){ "istemplate",   "optional",      "bool",       @"true",                "Whether or not the script file is a template like asp.net aspx files" },
                new List<object>(){ "plugins",      "optional",      "string",     @"sys | all",           "The plugin group to use" }
            };
            var text = "\r\nFLUENTSCRIPT command line arguments:\r\n\r\n";
            foreach (var argInfo in args)
            {
                text += string.Format(" {0}\r\n\t{1}\r\n\t{2} - {3}\r\n\texample: {4}\r\n\r\n", argInfo[0].ToString(), argInfo[4].ToString(), argInfo[1].ToString(), argInfo[2].ToString(), argInfo[3].ToString());
            }             
            return text;
        }


        /// <summary>
        /// The version.
        /// </summary>
        /// <returns></returns>
        public string ToVersion() { return "0.9.8.8 Beta"; }


        /// <summary>
        /// Examples
        /// </summary>
        /// <returns></returns>
        public string ToExamples()
        {
            var eg = Environment.NewLine + "fs file:c:\\fs\\samples\\helloworld.js"
                   + Environment.NewLine + "fs file:\\samples\\helloworld.js  \t tokenize:true"
                   + Environment.NewLine + "fs file:\\samples\\helloworld.js  \t plugins:all"
                   + Environment.NewLine + "fs file:\\samples\\helloworld.jst \t istemplate:true";
            return eg + "\r\n\r\n";
        }


        /// <summary>
        /// THe full help text.
        /// </summary>
        /// <returns></returns>
        public string ToFullHelpText()
        {
            var text = ToHelpString();
            var examples = ToExamples();
            var finalText = text + "\r\n\r\nVERSION:\t" + ToVersion() + "\r\n\r\n\r\nEXAMPLES:\r\n\r\n" + examples;
            finalText += "\r\nMORE INFO:\r\n\r\nCompany : CodeHelixSolutions Inc."
                      + Environment.NewLine + "Site    : www.codehelixsolutions.com"
                      + Environment.NewLine + "Demo    : www.fluentscript.com"
                      + Environment.NewLine + "Docs    : http://fluentscript.codeplex.com/documentation"
                      + Environment.NewLine;
            return finalText;
        }
    }
}
