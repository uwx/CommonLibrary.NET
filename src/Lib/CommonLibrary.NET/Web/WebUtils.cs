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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Globalization;
using System.Drawing.Imaging;


namespace ComLib.Web
{
    /// <summary>
    /// This class provides helper methods for the <see cref="ComLib.Web"/> namespace.
    /// </summary>
    public class WebUtils
    {
        private static IDictionary<string, ImageFormat> _imageFormatsLookup;

        /// <summary>
        /// Static initializer.
        /// </summary>
        static WebUtils()
        {
            _imageFormatsLookup = new Dictionary<string, ImageFormat>();
            _imageFormatsLookup.Add("jpeg", ImageFormat.Jpeg);
            _imageFormatsLookup.Add("jpg", ImageFormat.Jpeg);
            _imageFormatsLookup.Add("gif", ImageFormat.Gif);
            _imageFormatsLookup.Add("tiff", ImageFormat.Tiff);
            _imageFormatsLookup.Add("png", ImageFormat.Png);
        }


        /// <summary>
        /// Get a remote web file.
        /// </summary>
        /// <param name="file">The remote URL</param>
        public static string GetFileContentsRemote(string file)
        {
            string content = string.Empty;
            try
            {
                Uri url = new Uri(file, UriKind.Absolute);
                using (WebClient client = new WebClient())
                {
                    // Load CSS content
                    client.Credentials = CredentialCache.DefaultNetworkCredentials;
                    content = client.DownloadString(url);
                }
                return content;
            }
            catch (System.Net.Sockets.SocketException)
            { return string.Empty; }
        }


        /// <summary>
        /// Retrieve local file contents.
        /// </summary>
        public static string GetFileContentsLocal(string file)
        {
            string path = HttpContext.Current.Server.MapPath(file);
            string content = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    content = reader.ReadToEnd();                    
                }
            }
            catch
            { }
            return content;
        }


        /// <summary>
        /// Get the content of an upload file as a string.
        /// </summary>
        /// <param name="inputFile">Path to input file.</param>
        /// <returns>String with file contents.</returns>
        public static string GetContentOfFile(HtmlInputFile inputFile)
        {
            byte[] data = new byte[inputFile.PostedFile.ContentLength];
            int contentLength = inputFile.PostedFile.ContentLength;

            inputFile.PostedFile.InputStream.Read(data, 0, contentLength);
            MemoryStream stream = new MemoryStream(data);
            StreamReader reader = new StreamReader(stream);
            string importText = reader.ReadToEnd();
            return importText;
        }


        /// <summary>
        /// Get the content of an upload file as a string.
        /// </summary>
        /// <param name="inputFile">Path to input file.</param>
        /// <returns>Byte array with file contents.</returns>
        public static byte[] GetContentOfFileAsBytes(HtmlInputFile inputFile)
        {
            byte[] data = new byte[inputFile.PostedFile.ContentLength];
            int contentLength = inputFile.PostedFile.ContentLength;

            inputFile.PostedFile.InputStream.Read(data, 0, contentLength);
            return data;
        }


        /// <summary>
        /// Get the content of an upload file as a string.
        /// </summary>
        /// <param name="inputFile">Path to input file.</param>
        /// <returns>Byte array with file contents.</returns>
        public static byte[] GetContentOfFileAsBytes(HttpPostedFileBase inputFile)
        {
            byte[] data = new byte[inputFile.ContentLength];
            int contentLength = inputFile.ContentLength;

            inputFile.InputStream.Read(data, 0, contentLength);
            return data;
        }


        /// <summary>
        /// Gets the file extension of the file.
        /// </summary>
        /// <param name="inputFile">Path to file.</param>
        /// <returns>File extension.</returns>
        public static string GetFileExtension(HtmlInputFile inputFile)
        {
            if (inputFile == null || string.IsNullOrEmpty(inputFile.PostedFile.FileName))
                return string.Empty;

            string fileName = inputFile.PostedFile.FileName;

            int ndxExtensionPeriod = fileName.LastIndexOf(".");
            if (ndxExtensionPeriod < 0) { return string.Empty; }

            // Error could occurr with file name = test. (ok for now)
            // Check for .txt extension.
            string fileExtension = fileName.Substring(ndxExtensionPeriod + 1);
            fileExtension = fileExtension.Trim().ToLower();
            return fileExtension;
        }


        /// <summary>
        /// Get the file extension as a image format.
        /// </summary>
        /// <param name="inputFile">Path to image file.</param>
        /// <returns>The format of the image file.</returns>
        public static ImageFormat GetFileExtensionAsFormat(HtmlInputFile inputFile)
        {
            string extension = GetFileExtension(inputFile);
            if (string.IsNullOrEmpty(extension)) return null;

            if (!_imageFormatsLookup.ContainsKey(extension)) return null;
            return _imageFormatsLookup[extension];
        }
    }



    /// <summary>
    /// Security util.
    /// </summary>
    public class WebSecurityUtils
    {
        /// <summary>
        /// Determines if the request being made is from the same host.
        /// Otherwise, most likely someone is leeching the image.
        /// </summary>
        /// <param name="requestDeniedImagePath">"~/images/backoff.gif"</param>
        /// <param name="ctx">Current http contenxt.</param>
        /// <param name="path">Physical path.</param>
        /// <returns>True of is being made from the same host.</returns>
        public static bool IsSelfRequest(HttpContext ctx, ref string path, string requestDeniedImagePath)
        {
            HttpRequest req = ctx.Request;
            path = req.PhysicalPath;

            if (req.UrlReferrer != null && req.UrlReferrer.Host.Length > 0)
            {
                if ( CultureInfo.InvariantCulture.CompareInfo.Compare(req.Url.Host,
                    req.UrlReferrer.Host, CompareOptions.IgnoreCase) != 0)
                {
                    path = ctx.Server.MapPath(requestDeniedImagePath);
                    return false;
                }
            }
            return true;
        }
    }
}