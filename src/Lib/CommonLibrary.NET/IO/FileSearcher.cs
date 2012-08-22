using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;


namespace ComLib.IO
{

    /// <summary>
    /// Search for directories and files using a pattern.
    /// </summary>
    public class FileSearcher
    {
        private Action<FileInfo> _fileHandler;
        private Action<DirectoryInfo> _directoryHandler = null;
        private string _directoryPattern = "**";
        private string _filePattern = "**";
        private string _pattern = "**/**";
        private bool _handleFiles = true;


        /// <summary>
        /// Initialize with file handler
        /// </summary>
        /// <param name="directoryHandler">Handler for each directory.</param>
        /// <param name="fileHandler">Handler for each file.</param>
        /// <param name="handleFiles">Flag indicating to handle files.</param>
        /// <param name="pattern">Search pattern for directories files.
        /// e.g. **/**.</param>
        public FileSearcher(Action<FileInfo> fileHandler, Action<DirectoryInfo> directoryHandler, 
            string pattern, bool handleFiles)
        {
            _pattern = pattern;
            _fileHandler = fileHandler;
            _handleFiles = handleFiles;
            Init();
        }


        /// <summary>
        /// Initialize directory and file pattern.
        /// </summary>
        public void Init()
        {
            // Determine directory pattern and file pattern.
            if (!string.IsNullOrEmpty(_pattern))
            {
                int ndxSlash = _pattern.IndexOf("/");
                if (ndxSlash < 0)
                    ndxSlash = _pattern.IndexOf("\\");

                _directoryPattern = _pattern.Substring(0, ndxSlash);
                _filePattern = _pattern.Substring(ndxSlash + 1);
            }
        }

        
        /// <summary>
        /// Search directory for directories/files using pattern.
        /// </summary>
        /// <param name="startDir"></param>
        public void Search(DirectoryInfo startDir)
        {
            DirectoryInfo[] directories = null;
            if (_directoryPattern == "**")
                directories = startDir.GetDirectories();
            else
                directories = startDir.GetDirectories(_directoryPattern);

            if ((directories != null) && (directories.Length > 0))
            {
                foreach (DirectoryInfo currentDir in directories)
                {
                    if (_handleFiles)
                    {
                        SearchFiles(currentDir);
                    }
                    Search(currentDir);

                    // Call the directory handler.
                    if (_directoryHandler != null)
                        _directoryHandler(currentDir);
                }
            }
        }


        /// <summary>
        /// Search all the files.
        /// </summary>
        /// <param name="directory"></param>
        private void SearchFiles(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles(_filePattern);
            if (files == null || files.Length == 0)
                return;

            foreach (FileInfo file in files)
            {
                if (_fileHandler != null)
                {
                    _fileHandler(file);
                }
            }
        }
    }
}
