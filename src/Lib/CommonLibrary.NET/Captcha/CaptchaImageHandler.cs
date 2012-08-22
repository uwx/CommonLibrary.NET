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
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;


namespace ComLib.CaptchaSupport
{
    /// <summary>
    /// Captcha Image Handler
    /// </summary>
    public class CaptchaImageHandler : IHttpHandler
    {
        /// <summary>
        /// IsReusable handler.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }


        /// <summary>
        /// Generate a captcha image
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["CaptchaText"] != null)
            {
                // string name = context.Request.Params["CaptchaText"];
                Bitmap bmp = Captcha.GenerateFromUrl();
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                bmp.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                bmp.Dispose();
            }
        }
    }
}
