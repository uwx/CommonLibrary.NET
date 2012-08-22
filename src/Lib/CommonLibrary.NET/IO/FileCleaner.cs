/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using ComLib;
using ComLib.Arguments;
using ComLib.Application;


namespace ComLib.IO
{
    /// <summary>
    /// Arguments for cleaning directories/files.
    /// </summary>
    public class FileCleanArgs
    {
        /// <summary>
        /// Get/set directory recurse flag.
        /// </summary>
        [Arg("recurse", "r", "Recurse into subdirectories", typeof(bool), false, false, "true", "true|false")]
        public bool Recurse { get; set; }


        /// <summary>
        /// Get/set the file matching pattern.
        /// </summary>
        [Arg("pattern", "p", "Only process files with name matching pattern", typeof(string), false, ".svn", ".svn", ".svn|.obj")]
        public string Pattern { get; set; }


        /// <summary>
        /// Get/set the type of entities to handle.
        /// </summary>
        [Arg("filetype", "f", "Indicate whether to handle only files/directories or both", typeof(string), false, "dir", "dir", "dir|file|all")]
        public string FileType { get; set; }


        /// <summary>
        /// Get/set whether execution will happen.
        /// </summary>
        [Arg("dryrun", "d", "Indicate only showing what will happen without running.", typeof(bool), false, false, "true", "true|false")]
        public bool DryRun { get; set; }


        /// <summary>
        /// Get/set the root directory.
        /// </summary>
        [Arg("rootdir", "rd", "c:\\temp\" -Starting directory where cleaning should happen. If not specified, current directory is assumed.", typeof(string), false, ".", ".", @".|..\|c:\temp")]
        public string RootDir { get; set; }


        /// <summary>
        /// Get/set the output file.
        /// </summary>
        [Arg("outfile", "o", "fileclean.txt - Name of the file to write the output to.", typeof(string), false, "FileClean.txt", "FileClean.txt", @"fileClean.txt|c:\temp\fileclean.txt")]
        public string OutputFile { get; set; }
    }


    /// <summary>
    /// Abstract class for file-based applications.
    /// </summary>
    public abstract class FileAppBase : App
    {
        /// <summary>
        /// Root directory.
        /// </summary>
        protected DirectoryInfo _rootDirectory;

        /// <summary>
        /// Initialize the starting/root directory for this command.
        /// </summary>
        /// <param name="rootDir"></param>
        public void InitializeDir(string rootDir)
        {
            string location = Assembly.GetExecutingAssembly().Location;
            FileInfo info = new FileInfo(location);
            _rootDirectory = info.Directory;
            Console.WriteLine(location);
            if (!string.IsNullOrEmpty(rootDir) && rootDir != ".")
            {
                _rootDirectory = new DirectoryInfo(rootDir);
            }
        }
    }

    
    /// <summary>
    /// Cleans directories / files.
    /// </summary>
    public class FileCleaner : FileAppBase
    {
        private StringBuilder _buffer;


        /// <summary>
        /// File Cleaner.
        /// </summary>
        public FileCleaner()
        {            
            Settings.ArgsReciever = new FileCleanArgs();
            Settings.ArgsRequired = true;
            Settings.ArgsAppliedToReciever = true;
            _buffer = new StringBuilder();
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
            FileCleanArgs args = Settings.ArgsReciever as FileCleanArgs;   
            // Initialize the starting directory.
            InitializeDir(args.RootDir);

            StringBuilder buffer = new StringBuilder();
            bool handleFiles = true;
            if (args.FileType == "dir")
                handleFiles = false;

            FileSearcher searcher = new FileSearcher(new Action<FileInfo>(HandleFile), new Action<DirectoryInfo>(HandleDirectory), "**/**", handleFiles);
            searcher.Search(_rootDirectory);
            File.WriteAllText(args.OutputFile, _buffer.ToString());
            return new BoolMessageItem(null, true, string.Empty);
        }


        /// <summary>
        /// Handle the directory.
        /// </summary>
        /// <param name="directory"></param>
        protected virtual void HandleDirectory(DirectoryInfo directory)
        {
            FileCleanArgs args = Settings.ArgsReciever as FileCleanArgs;
            if (!args.DryRun)
            {
                _buffer.Append("rmdir \"" + directory.FullName + "\" /s /q" + Environment.NewLine);
            }
            else
            {
                _buffer.Append("Dry run - cleaning : " + directory.FullName + " /s /q " + Environment.NewLine);
            }
        }


        /// <summary>
        /// Handle the file.
        /// </summary>
        /// <param name="file"></param>
        protected virtual void HandleFile(FileInfo file)
        {
            FileCleanArgs args = Settings.ArgsReciever as FileCleanArgs;
            if (!args.DryRun)
            {
                _buffer.Append("del \"" + file.FullName + "\" /f /q" + Environment.NewLine);
            }
            else
            {
                _buffer.Append("Dry run - cleaning : " + file.FullName + " /f /q" + Environment.NewLine);
            }
        }
        #endregion
    }
}
