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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

using ComLib.Extensions;


namespace ComLib.Web.HttpHandlers
{
    /// <summary>
    /// This class contains helper handler methods.
    /// </summary>
    public class HandlerHelper
    {
        /// <summary>
        /// Set the headers using the configuration data.
        /// </summary>
        /// <param name="context">Http context.</param>
        /// <param name="extensionForContent">String content extension.</param>
        /// <param name="config">Config settings.</param>
        /// <param name="configSectionName">Name of configuration section.</param>
        /// <param name="hashcode">Hash code.</param>
        public static void SetHeaders(HttpContext context, string extensionForContent, IDictionary config, string configSectionName, int hashcode)
        {
            bool isCachable = config == null ? true : config.GetOrDefault<bool>(configSectionName, "CacheEnabledOnClientSide", true);
            context.Response.ContentType = extensionForContent;
            context.Response.Cache.VaryByHeaders["Accept-Encoding"] = true;

            if (isCachable)
            {
                int maxDaysCached = config == null ? 10 : config.GetOrDefault<int>(configSectionName, "CacheDurationInDays", 10);
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.ToUniversalTime().AddDays(maxDaysCached));
                context.Response.Cache.SetMaxAge(new TimeSpan(maxDaysCached, 0, 0, 0));

                string eTag = "\"" + hashcode.ToString() + "\"";
                string existingEtag = context.Request.Headers["If-None-Match"];
                context.Response.Cache.SetETag(eTag);

                if (String.Compare(existingEtag, eTag) == 0)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                    context.Response.SuppressContent = true;
                }
            }
        }
    }
}
