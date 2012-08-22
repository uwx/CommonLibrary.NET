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
using ComLib.Collections;


namespace ComLib.Web.HttpHandlers
{
	/// <summary>
	/// Removes whitespace in all stylesheets added to the 
	/// header of the HTML document in site.master. 
	/// </summary>
	public class HandlerBase : IHttpHandler
	{
        /// <summary>
        /// Default construction. Initialize.
        /// </summary>
        public HandlerBase()
        {
            Init();
        }


        /// <summary>
        /// The content to write back to the response.
        /// </summary>
        protected string _content;


        /// <summary>
        /// The extension for this handler.
        /// e.g. .css.
        /// </summary>
        public string Extension { get; set; }


        /// <summary>
        /// Used to set content type.
        /// </summary>
        public string ExtensionForContent { get; set; }


        /// <summary>
        /// The name of the section to use for this specific handler.
        /// </summary>
        public string ConfigSectionName { get; set; }


        /// <summary>
        /// Configuration settings as IDictionary.
        /// </summary>
        public IDictionary Config { get; set; }
                
       
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom 
        /// HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
        /// references to the intrinsic server objects 
        /// (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public virtual void ProcessRequest(HttpContext context)
        {
            string fileName = context.Request.Url.AbsolutePath;

            // Check for only this extension files
            if (!fileName.EndsWith(Extension, StringComparison.OrdinalIgnoreCase))
                throw new System.Security.SecurityException("Invalid " + Extension + " file extension");

            _content = string.Empty;

            // Get the css file.
            _content = GetFile(fileName);


            // Validate .css file.
            if (string.IsNullOrEmpty(_content))
            {
                context.Response.Status = Extension + " not found";
                context.Response.StatusCode = 404;
                return;
            }

            // Have valid css file at this point.
            // Now set the headers for caching the .css on the client side.
            SetHeaders(context, null, _content.GetHashCode());
            WriteContent(context);
        }


        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public virtual bool IsReusable
        {
            get { return false; }
        }


        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <param name="contextbag">Context bag.</param>
        /// <param name="hashcode">Hash code.</param>
        protected virtual void SetHeaders(HttpContext context, object contextbag, int hashcode)
        {
            HandlerHelper.SetHeaders(context, ExtensionForContent, Config, ConfigSectionName, hashcode);
        }


        /// <summary>
        /// Allows for initialization after construction.
        /// </summary>
        protected virtual void Init()
        {
        }


        /// <summary>
        /// Write the response content.
        /// </summary>
        /// <param name="context">Http context.</param>
        protected virtual void WriteContent(HttpContext context)
        {
            context.Response.Write(_content);
        }


        /// <summary>
        /// Get the css file.
        /// </summary>
        /// <param name="fileName">CSS file.</param>
        /// <returns>CSS file.</returns>
        protected virtual string GetFile(string fileName)
        {
            string content = string.Empty;
            if (fileName.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                content = WebUtils.GetFileContentsRemote(fileName);
            }
            else
            {
                content = WebUtils.GetFileContentsLocal(fileName);
            }

            return content;
        }
	}
}
