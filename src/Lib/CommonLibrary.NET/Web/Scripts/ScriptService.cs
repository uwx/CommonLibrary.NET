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


namespace ComLib.Web.ScriptsSupport
{

    /// <summary>
    /// Information Service
    /// </summary>
    public class ScriptService : IScriptService
    {
        private IDictionary<string, ScriptsHolder> _holders = new Dictionary<string, ScriptsHolder>();


        /// <summary>
        /// Add a new scripts holder for the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <param name="useHttp">Use HTTP.</param>
        public void AddLocation(string location, bool useHttp = true)
        {
            _holders[location] = new ScriptsHolder(useHttp);
        }


        /// <summary>
        /// Get the scriptsholder for the specified location.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Instance of scripts holder.</returns>
        public ScriptsHolder For(string location)
        {
            return _holders[location];
        }


        /// <summary>
        /// Add a javascript to the specified location.
        /// </summary>
        /// <param name="name">Script name.</param>
        /// <param name="url">Script URL.</param>
        /// <param name="location">Location.</param>
        /// <param name="dependsOn">Script dependency.</param>
        /// <param name="version">Script version.</param>
        public void AddJavascript(string name, string url, string location = "footer", string dependsOn = "", string version = "")
        {
            _holders[location].AddJavascript(name, url, dependsOn, version);
        }


        /// <summary>
        /// Adds a css to the specified location.
        /// </summary>
        /// <param name="name">CSS name.</param>
        /// <param name="url">CSS URL.</param>
        /// <param name="location">Location.</param>
        /// <param name="dependsOn">CSS dependency.</param>
        /// <param name="version">CSS version.</param>
        public void AddCss(string name, string url, string location = "footer", string dependsOn = "", string version = "")
        {
            _holders[location].AddCss(name, url, dependsOn, version);
        }


        /// <summary>
        /// Gets the current scripts as html tags.
        /// </summary>
        /// <param name="location">Location.</param>
        /// <returns>Scripts as html tags.</returns>
        public string ToHtml(string location = "footer")
        {
            return _holders[location].ToHtml();
        }
    }
}