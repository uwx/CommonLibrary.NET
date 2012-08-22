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
using System.IO;
using System.Reflection;


namespace ComLib.Reflection
{
    /// <summary>
    /// Assembly related reflection utils.
    /// </summary>
    public class AssemblyUtils
    {
        /// <summary>
        /// Get the internal template content from the commonlibrary assembly.
        /// </summary>
        /// <param name="assemblyFolderPath">"CommonLibrary.Notifications.Templates."</param>
        /// <param name="fileName">"welcome.html"</param>
        /// <returns>String with internal template content.</returns>
        public static string GetInternalFileContent(string assemblyFolderPath, string fileName)
        {
            Assembly current = Assembly.GetExecutingAssembly();

            Stream stream = current.GetManifestResourceStream(assemblyFolderPath + fileName);
            if (stream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            return content;
        }
    }
}
