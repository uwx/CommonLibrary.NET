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
using System.Collections.Specialized;


namespace ComLib.Web.ScriptsSupport
{
    /// <summary>
    /// Script collection.
    /// </summary>
    public class ScriptsHolder
    {
        private bool _useHttp = true;
        private IDictionary _scripts = new OrderedDictionary();


        /// <summary>
        /// Default initialization
        /// </summary>
        public ScriptsHolder() : this(true)
        {   
        }


        /// <summary>
        /// Initialize flag to use http context or in-memory.
        /// </summary>
        /// <param name="useHttp">True to use HTTP context.</param>
        public ScriptsHolder(bool useHttp)
        {
            _useHttp = useHttp;
        }


        /// <summary>
        /// Adds the javascript to the scripts.
        /// </summary>
        /// <param name="name">Script name.</param>
        /// <param name="url">Script URL.</param>
        /// <param name="dependsOn">Script dependency.</param>
        /// <param name="version">Script version.</param>
        public void AddJavascript(string name, string url, string dependsOn = "", string version = "")
        {
            var script = new Script(name, url, dependsOn, version);
            string format = "<script src=\"{0}\" type=\"text/javascript\"></script>";
            script.Tag = string.Format(format, url);
            var scripts = GetScripts();
            scripts[name] = script;
        }


        /// <summary>
        /// Adds the css to the scripts collection.
        /// </summary>
        /// <param name="name">CSS name.</param>
        /// <param name="url">CSS URL.</param>
        /// <param name="dependsOn">CSS dependency.</param>
        /// <param name="version">CSS version.</param>
        public void AddCss(string name, string url, string dependsOn = "", string version = "")
        {
            var script = new Script(name, url, dependsOn, version);
            string format = "<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />";
            script.Tag = string.Format(format, url);
            var scripts = GetScripts();
            scripts[name] = script;
        }


        /// <summary>
        /// Html representation of all the scripts, javascript followed by css.
        /// </summary>
        /// <returns>HTML representation of scripts.</returns>
        public string ToHtml()
        {
            var scripts = GetScripts();
            if (scripts == null || scripts.Count == 0) return string.Empty;
            var buffer = new StringBuilder();
            foreach (DictionaryEntry pair in scripts)
            {
                var script = pair.Value as Script;
                buffer.AppendLine(script.Tag);
            }
            string html = buffer.ToString();
            return html;
        }


        private IDictionary GetScripts()
        {
            if (!_useHttp) return _scripts;

            var scriptsMap = System.Web.HttpContext.Current.Items["scripts"] as IDictionary;
            if (scriptsMap == null)
            {
                scriptsMap = new OrderedDictionary();
                System.Web.HttpContext.Current.Items["scripts"] = scriptsMap;
            }
            return scriptsMap;
        }
    }
}
