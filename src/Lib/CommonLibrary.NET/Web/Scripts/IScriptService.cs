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
using System.Reflection;

using ComLib.Reflection;


namespace ComLib.Web.ScriptsSupport
{
    /// <summary>
    /// Information Service
    /// </summary>
    public interface IScriptService
    {
        /// <summary>
        /// Add a scripts holder for the specified location.
        /// </summary>
        /// <param name="location">Script location.</param>
        /// <param name="useHttp">Use HTTP for script.</param>
        void AddLocation(string location, bool useHttp = true);


        /// <summary>
        /// Get the scripts for the name.
        /// </summary>
        /// <param name="name">Script name.</param>
        /// <returns></returns>
        ScriptsHolder For(string name);


        /// <summary>
        /// Add a javascript to the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="name">Script name.</param>
        /// <param name="url">Script URL.</param>
        /// <param name="dependsOn">Script dependency.</param>
        /// <param name="version">Script version.</param>
        void AddJavascript(string name, string url, string location = "footer", string dependsOn = "", string version = "");


        /// <summary>
        /// Adds a css to the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="name">CSS name.</param>
        /// <param name="url">CSS URL.</param>
        /// <param name="dependsOn">CSS dependency.</param>
        /// <param name="version">CSS version.</param>
        void AddCss(string name, string url, string location = "footer", string dependsOn = "", string version = "");


        /// <summary>
        /// Gets the current scripts as html tags.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Current scripts as html tags.</returns>
        string ToHtml(string location = "footer");
    }
}
