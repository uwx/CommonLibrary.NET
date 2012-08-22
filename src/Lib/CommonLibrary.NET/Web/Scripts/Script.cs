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

namespace ComLib.Web.ScriptsSupport
{
    /// <summary>
    /// Script object
    /// </summary>
    public class Script
    {
        /// <summary>
        /// Default
        /// </summary>
        public Script() { }


        /// <summary>
        /// Initalize 
        /// </summary>
        /// <param name="name">Script name.</param>
        /// <param name="url">Script URL.</param>
        /// <param name="dependsOn">Script dependency.</param>
        /// <param name="version">Script version.</param>
        public Script(string name, string url, string dependsOn, string version)
        {
            Name = name;
            Url = url;
            DependsOn = dependsOn;
            Version = version;
        }


        /// <summary>
        /// Name of the script.
        /// </summary>
        public string Name;


        /// <summary>
        /// Url of the script.
        /// </summary>
        public string Url;


        /// <summary>
        /// Version of the script.
        /// </summary>
        public string Version;


        /// <summary>
        /// The other scripts this depends on.
        /// </summary>
        public string DependsOn;


        /// <summary>
        /// The html tag.
        /// </summary>
        public string Tag;
    }
}
