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
using ComLib.Reflection;


namespace ComLib.Notifications
{
    /// <summary>
    /// This class provides helper methods
    /// for the Notifications namespace.
    /// </summary>
    public class NotificationUtils
    {
        /// <summary>
        /// Get the internal template content from the commonlibrary assembly.
        /// </summary>
        /// <param name="fileName">e.g. welcome.html</param>
        /// <returns>String with internal templace.</returns>
        public static string GetInternalNotificationTemplate(string fileName)
        {
            return AssemblyUtils.GetInternalFileContent("CommonLibrary.Notifications.Templates.", fileName);
        }
    }
}
