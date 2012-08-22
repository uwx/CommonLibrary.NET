using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace CodePlexDocGenerator.Lib
{
    public class DocSettings
    {
        /// <summary>
        /// Default template location.
        /// </summary>
        public string Template = "..\\..\\template.txt";


        /// <summary>
        /// Default location of CommonLibrary source code.
        /// </summary>
        public string ComLib = "..\\..\\..\\..\\Lib\\CommonLibrary.Net";


        /// <summary>
        /// Default wiki output file.
        /// </summary>
        public string DocFile = "wiki.txt";


        /// <summary>
        /// Change set number to use.
        /// </summary>
        public string ChangeSet = "71515";


        /// <summary>
        /// Path to _Examples.xml with details about wiki creation.
        /// </summary>
        public string XML = "\\_Samples\\_Examples.xml";


        /// <summary>
        /// Relative path to the samples folder.
        /// </summary>
        public string Examples = "\\_Samples";


        /// <summary>
        /// Loads the settings from the config file.
        /// </summary>
        /// <returns></returns>
        public static DocSettings LoadFromConfig()
        {
            var settings = new DocSettings();
            settings.ChangeSet = ConfigurationManager.AppSettings["changeset"];
            settings.ComLib   = ConfigurationManager.AppSettings["comlibPath"];
            settings.Examples = ConfigurationManager.AppSettings["examplesPath"];
            settings.Template = ConfigurationManager.AppSettings["templatePath"];
            settings.XML = ConfigurationManager.AppSettings["xmlDocInstructionsPath"];
            settings.DocFile = ConfigurationManager.AppSettings["docFile"];
            return settings;
        }
    }
}
