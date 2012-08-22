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
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Web.Caching;

using ComLib.Web;
using ComLib.Extensions;


namespace ComLib.Web.HttpHandlers
{
	/// <summary>
	/// Removes whitespace in all stylesheets added to the 
	/// header of the HTML document in site.master. 
	/// </summary>
    public class ImageHandler : HandlerBase, IHttpHandler
	{
        private static IDictionary<string, string> _supportedImageTypes = new Dictionary<string, string>();
        private static IDictionary _config = new Dictionary<string, object>();
        private static string _sectionName = "WebHandlers.Images";
        private static Action<HttpContext> _externalImageHandler;
        private static Func<HttpContext, bool> _externalImageHandlerCheck;
        private static bool _enableLogging = false;


        /// <summary>
        /// Static initialization.
        /// </summary>
        static ImageHandler()
        {
            // All the replacement chars for removing whitespace from css files.
            _supportedImageTypes["jpeg"] = "jpeg";
            _supportedImageTypes["jpg"] = "jpeg";
            _supportedImageTypes["gif"] = "gif";
            _supportedImageTypes["png"] = "png";
            _supportedImageTypes["tiff"] = "tiff";
        }


        /// <summary>
        /// Set the configuration settings as an IDictionary.
        /// </summary>
        /// <param name="sectionname">Section name</param>
        /// <param name="useSection">Whether or not to use the section name.</param>
        /// <param name="config">Configuration settings.</param>
        public static void Init(IDictionary config, string sectionname, bool useSection)
        {
            _config = config;
            if (useSection)
                _sectionName = sectionname;
        }


        /// <summary>
        /// Enable logging.
        /// </summary>
        public static bool EnableLogging { get { return _enableLogging; } set { _enableLogging = value; } }


        /// <summary>
        /// Initialize the external handler.
        /// </summary>
        /// <param name="externalImageHandlerCheck">Image handler check method.</param>
        /// <param name="externalImageHandler">Image handler.</param>
        public static void InitExternalHandler(Func<HttpContext, bool> externalImageHandlerCheck, Action<HttpContext> externalImageHandler)
        {
            _externalImageHandlerCheck = externalImageHandlerCheck;
            _externalImageHandler = externalImageHandler;
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        protected override void Init()
        {
            base.Init();
            this.Config = _config;
            this.ConfigSectionName = _sectionName;
            this.Extension = "jpeg";
            this.ExtensionForContent = "image/jpeg";
        }


        /// <summary>
        /// Handle the request for the image file.
        /// </summary>
        /// <param name="context">Http context.</param>
        public override void ProcessRequest(HttpContext context)
        {
            if (_externalImageHandlerCheck != null && _externalImageHandlerCheck(context))
            {
                if(_enableLogging) Logging.Logger.Debug("CommonLibrary ImageHandler ProcessRequest using external handler for : " + context.Request.Url.AbsolutePath);
                _externalImageHandler(context);
                return;
            }

            string fileName = context.Request.Url.AbsolutePath;
			try
			{
                string physicalpath = context.Server.MapPath(fileName);
                FileInfo fi = new FileInfo(physicalpath);
                if (_enableLogging) Logging.Logger.Debug("CommonLibrary ImageHandler Processing image : " + fileName + ", physical path : " + physicalpath);
                if (fi.Exists)
                {
                    string extension = fi.Extension.Substring(1);
                    string contentType = string.Empty;

					if (string.Compare(extension, "JPG") == 0)
                        contentType = "image/jpeg";
					else
						contentType = "image/" + extension;

                    int hashCode = fi.FullName.GetHashCode();
                    SetHeaders(context, null, hashCode);
					context.Response.TransmitFile(fi.FullName);
                    if (_enableLogging) Logging.Logger.Debug("Image file exists : " + fileName + ", physical file : " + physicalpath);
				}
				else
				{
                    if (_enableLogging) Logging.Logger.Debug("CommonLibrary ImageHandler ProcessRequest ImageFile : " + fileName + " not found.");
					context.Response.Redirect("/NotFound");
				}
			}
			catch (Exception ex)
			{
                if (_enableLogging) Logging.Logger.Debug("CommonLibrary ImageHandler ProcessRequest : unable to process request for file : " + fileName, ex);
				context.Response.Redirect("/NotFound");
			}
		}
	}
}
