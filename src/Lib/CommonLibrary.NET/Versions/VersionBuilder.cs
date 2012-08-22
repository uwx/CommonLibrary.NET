using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using ComLib.Application;
using ComLib.Arguments;


namespace ComLib.VersionCheck
{
    public class VersionBuilder : App
    {
        #region Private data
        private bool _hasFilter;
        private Dictionary<string, string> _filteredTypes = new Dictionary<string, string>();
        private BoolMessageItem _checkResult;
        #endregion


        /// <summary>
        /// Create using arguments, version definition, config settings.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="ctx">Version Definition context</param>
        /// <param name="checkResultHandler">Callback</param>
        public VersionBuilder()
        {
        }


        /// <summary>
        /// Check the result.
        /// </summary>
        public BoolMessageItem Result { get { return _checkResult; } }


        /// <summary>
        /// Get the list of arguments that are supported.
        /// Type sojara.commodities.exe --help to display all the options.
        /// </summary>
        /// <returns></returns>
        public override List<ArgAttribute> OptionsSupported
        {
            get
            {
                List<ArgAttribute> options = new List<ArgAttribute>()
                {           
                    new ArgAttribute("name",    "Name representing this check", typeof(string), true, "", "ComJane_1.1.3, LimMds_1.3"),
                    new ArgAttribute("definition", "Path to xml definition file to generate", typeof(string), true, "", @"CommoditiesJane_1.1.3.xml"),
                    new ArgAttribute("directory",  "The directory to generate definition from.", typeof(string), true, @"",  @"C:\CS\RAD\ComJane\1.1.3.14\bin"),
                    new ArgAttribute("include",    "List of file extensions to include", typeof(string), false, "xml,dll", ".xml,.dll,.config,.exe"),
                    new ArgAttribute("useSize",    "Flag to include filesize in defintion", typeof(bool), false, false, "true | false"),
                    new ArgAttribute("pause",   false, "Pause sojara for debugging", typeof(bool), false, false, false, true, "true | false")                    
                };
                return options;
            }
        }


        /// <summary>
        /// Get examples of the sojara command line.
        /// </summary>
        /// <returns></returns>
        public override List<string> OptionsExamples
        {
            get
            {
                List<string> examples = new List<string>()
                {
                    @"VersionChecker.exe -action=build -include=xml,dll,exe,config -name=CommoditiesJane_1.1.2 -definition=config\CommoditiesJane.xml -directory=c:\CS\RAD\ComJane\1.1.3.14"
                };
                return examples;
            }
        }


        /// <summary>
        /// Accept or reject the arguments supplied on the command line.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override BoolMessageItem<Args> AcceptArgs(string[] args)
        {            
            BoolMessageItem<Args> result = base.AcceptArgs(args, "-", "=");
            if (result.Success)
            {
               // Check directory exists.
                string directory = result.Item.NamedArgs["directory"];
                if (!Directory.Exists(directory))
                    result = new BoolMessageItem<Args>(result.Item, false, "Directory : " + directory + " does NOT exist.");
            }
            return result;
        }


        /// <summary>
        /// Initialize the check.
        /// </summary>
        public override void  Initialize()
        {
            _filteredTypes.Clear();
            string filterPattern = !_parsedArgs.NamedArgs.ContainsKey("include") ? string.Empty : _parsedArgs.NamedArgs["include"];
            _hasFilter = !string.IsNullOrEmpty(filterPattern);
            string[] filteredExtensions = filterPattern.Split(',');
            foreach (string filter in filteredExtensions)
            {
                string filterLowercase = filter.ToLower().Trim();
                _filteredTypes[filterLowercase] = filterLowercase;
            }
        }
        

        /// <summary>
        /// Execute the check.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            try
            {
                bool useFileSize = Convert.ToBoolean(_parsedArgs.NamedArgs["useSize"]);
                string[] files = Directory.GetFiles(_parsedArgs.NamedArgs["directory"]);
                List<VersionDefItem> definitionComponents = new List<VersionDefItem>();
                foreach (string filename in files)
                {                    
                    FileInfo file = new FileInfo(filename);
                    if (IsPartOfDefinition(file))
                    {
                        VersionDefFile fileDef = new VersionDefFile();
                        fileDef.Path = file.FullName;
                        fileDef.IsRequired = true;
                        fileDef.IsWritable = false;
                        fileDef.Match = VersionMatch.Equal;
                        fileDef.VersionReq = FileUtils.GetVersion(filename);
                        if(useFileSize) fileDef.SizeReq = file.Length.ToString();
                        fileDef.Requirement = VersionRequirement.Required;
                        fileDef.FailOnError = false;
                        fileDef.DateReq = file.LastWriteTime.ToShortDateString();
                        definitionComponents.Add(fileDef);
                    }
                }
                var def = new VersionDefinition();
                def.Name = _parsedArgs.NamedArgs["name"];
                def.Components = definitionComponents;
                VersionImport.Save<VersionDefinition>(_parsedArgs.NamedArgs["definition"], def);
            }
            catch (Exception ex)
            {
                _checkResult = new BoolMessageItem(null, false, ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return _checkResult;
        }



        private bool IsPartOfDefinition(FileInfo file)
        {
            if (!_hasFilter) return true;

            string extension = file.Extension.ToLower().Trim();
            return _filteredTypes.ContainsKey(extension);
        }
    }
}
