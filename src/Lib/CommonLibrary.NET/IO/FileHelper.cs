using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using ComLib;
using ComLib.Arguments;
using ComLib.Application;


namespace ComLib.IO
{
    /// <summary>
    /// Stores command line arguments for LicenseApplier
    /// </summary>
    public class LicenseArgs : FileCleanArgs
    {
        /// <summary>
        /// Path to the license file
        /// </summary>
        [Arg("licensefile", "l", "license.txt - Name of the file to get text to append to next one.", typeof(string), true, "license.txt", "license.txt")]
        public string LicenseFile { get; set; }
        
        
        /// <summary>
        /// Whether or not the check files for existing license
        /// </summary>
        [Arg("checklicense", "l", "Check for existing license before applying.", typeof(string), true, true, "true", "true|false")]
        public bool CheckLicenseFirst { get; set; }
    }



    /// <summary>
    /// Applies a license by prepending it to each *.cs file.
    /// </summary>
    public class LicenseApplier : FileAppBase
    {
        private LicenseArgs _args = null;
        private string _license;
        private StringBuilder _buffer = new StringBuilder();


        /// <summary>
        /// File Cleaner.
        /// </summary>
        public LicenseApplier()
        {
            Settings.ArgsReciever  = new LicenseArgs();
            Settings.ArgsRequired = true;
            Settings.ArgsAppliedToReciever = true;
            _license = string.Empty;
        }


        #region IShellCommand Members
        /// <summary>
        /// Execute cleaning of files/directories.
        /// Doesn't actually delete anything but generates a file
        /// containing the commands.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override BoolMessageItem  Execute(object context)
        {
           // Initialize the starting directory.
            InitializeDir(_args.RootDir);

            StringBuilder buffer = new StringBuilder();
            bool handleFiles = true;
            if (_args.FileType == "dir")
                handleFiles = false;

            _license = File.ReadAllText(_args.LicenseFile);
            FileSearcher searcher = new FileSearcher(new Action<FileInfo>(HandleFile), new Action<DirectoryInfo>(HandleDirectory), _args.Pattern, handleFiles);
            searcher.Init();
            searcher.Search(_rootDirectory);
            File.WriteAllText(_args.OutputFile, _buffer.ToString());
            return new BoolMessageItem(null, true, string.Empty);
        }
        #endregion


        /// <summary>
        /// Handle the directory.
        /// </summary>
        /// <param name="directory"></param>
        protected virtual void HandleDirectory(DirectoryInfo directory)
        {
            Console.WriteLine("In directory : " + directory.Name);
        }


        /// <summary>
        /// Handle the file.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void HandleFile(FileInfo file)
        {
            _buffer.Append(Environment.NewLine + " Writing to file : " + file.FullName);
            //FileUtils.PrependText(_license, file);
        }
    }
}
