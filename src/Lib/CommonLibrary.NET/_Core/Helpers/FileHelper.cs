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
using System.Collections;
using System.IO;
using System.Diagnostics;


namespace ComLib
{
	/// <summary>
	/// This class provides various file helper methods.
	/// </summary>      
    public class FileHelper
    {
        /// <summary>
        /// Gets the orginal extension from a renamed extension. e.g. file.xml.config return .xml instead of .config. file.xml returns .xml.
        /// </summary>
        /// <param name="path">/config/users.csv.config</param>
        /// <param name="appendedExtension">The extra extension appended to the file. e.g. ".config"</param>
        /// <returns>String with original extension.</returns>
        public static string GetOriginalExtension(string path, string appendedExtension)
        {
            // Example /configfiles/users.csv.config
            FileInfo file = new FileInfo(path);
            string extension = file.Extension.ToLower();

            // None supplied ?
            if (string.IsNullOrEmpty(appendedExtension))
                return extension;

            // Now check that file ends w/ the extra extension
            appendedExtension = appendedExtension.ToLower();

            if (string.Compare(extension, appendedExtension, true) != 0)
                return extension;

            // Now get .csv from users.csv.config
            path = file.Name.Substring(0, file.Name.LastIndexOf(appendedExtension, StringComparison.InvariantCultureIgnoreCase));
            file = new FileInfo(path);
            return file.Extension.ToLower();
        }


        /// <summary>
        /// Prepend some text to a file.
        /// </summary>
        /// <param name="text">Text to prepend to a file.</param>
        /// <param name="file">File where text will be prepended.</param>
        public static void PrependText(string text, FileInfo file)
        {
            string content = File.ReadAllText(file.FullName);
            content = text + content;
            File.WriteAllText(file.FullName, content);
        }


        /// <summary>
        /// Get the file version information.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>String with the file version.</returns>
        public static string GetVersion(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            // Get the file version for the notepad.
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            return versionInfo.FileVersion;
        }


        /// <summary>
        /// Get file size in bytes.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>File size in bytes.</returns>
        public static int GetSizeInBytes(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            return (int)f.Length;
        }


        /// <summary>
        /// Get file size in kilobytes.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>File size in kilobytes.</returns>
        public static int GetSizeInKilo(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            float size = f.Length / 1000;
            return (int)size;
        }


        /// <summary>
        /// Get file size in megs.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        /// <returns>File size in megabytes.</returns>
        public static int GetSizeInMegs(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            float size = f.Length / 1000000;
            return (int)size;
        }  
    
    }
   
}
