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
using System.Linq;
using System.Text;


namespace ComLib.Web.ScriptsSupport
{
    /// <summary>
    /// Script collection.
    /// </summary>
    public class Scripts
    {
        private static ScriptService _service = new ScriptService();


        /// <summary>
        /// Gets the scripts place holder for the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="useHttp">Use HTTP.</param>
        public static void AddLocation(string location, bool useHttp = true)
        {
            _service.AddLocation(location, useHttp);
        }


        /// <summary>
        /// Gets the scripts place holder for the specified location.
        /// </summary>
        /// <param name="location">Script location.</param>
        /// <returns>Instance of scripts holder.</returns>
        public static ScriptsHolder For(string location)
        {
            return _service.For(location);
        }


        /// <summary>
        /// Add a javascript to the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="name">Script name.</param>
        /// <param name="url">Script URL.</param>
        /// <param name="dependsOn">Script dependency.</param>
        /// <param name="version">Script version.</param>
        public static void AddJavascript(string name, string url, string location = "footer", string dependsOn = "", string version = "")
        {
            _service.AddJavascript(name, url, location, dependsOn, version);
        }


        /// <summary>
        /// Adds a css to the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="name">CSS name.</param>
        /// <param name="url">CSS URL.</param>
        /// <param name="dependsOn">CSS dependency.</param>
        /// <param name="version">CSS version.</param>
        public static void AddCss(string name, string url, string location = "footer", string dependsOn = "", string version = "")
        {
            _service.AddCss(name, url, location, dependsOn, version);
        }


        /// <summary>
        /// Gets the current scripts as html tags.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Scripts as html tags.</returns>
        public static string ToHtml(string location = "footer")
        {
            return _service.For(location).ToHtml();
        }
    }
}
