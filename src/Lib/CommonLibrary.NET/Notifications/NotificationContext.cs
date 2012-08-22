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

namespace ComLib.Notifications
{
    /// <summary>
    /// Notification context.
    /// </summary>
    public class NotificationContext
    {
        /// <summary>
        /// Values.
        /// These are used to replace the place holders in the template files.
        /// e.g.
        /// key = "message.to" replaces "${message.to}" 
        /// in the template file with the value associated with "message.to" 
        /// in this dictionary.
        /// </summary>
        public IDictionary<string, string> Values;


        /// <summary>
        /// Allow default initialization.
        /// </summary>
        public NotificationContext() { Values = new Dictionary<string, string>();  }


        /// <summary>
        /// Initialize using supplied values.
        /// </summary>
        /// <param name="values">Dictionary with values to use for initialization.</param>
        public NotificationContext(IDictionary<string, string> values)
        {
            Values = values;
        }
    }
}
