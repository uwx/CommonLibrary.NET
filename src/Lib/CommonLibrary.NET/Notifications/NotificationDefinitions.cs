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
    /// Define the location of notification content.
    /// </summary>
    public class NotificationDef
    {
        /// <summary>
        /// Identifies a message template.
        /// </summary>
        public readonly string Name;


        /// <summary>
        /// Location of the template definition.
        /// </summary>
        public readonly string Location;


        /// <summary>
        /// Just the file name from the full location.
        /// </summary>
        public readonly string FileName;


        /// <summary>
        /// Flag indicates whether or not the template definition
        /// is embedded in an assembly.
        /// </summary>
        public readonly bool IsAssemblyEmbedded;


        /// <summary>
        /// Name of the assembly if embedded.
        /// </summary>
        public readonly string AssemblyName;


        /// <summary>
        /// Whether or not this is xsl based.
        /// TO_DO: NOT_USED currently.
        /// </summary>
        public readonly bool IsXsl;


        /// <summary>
        /// Notifcation content.
        /// </summary>
        /// <param name="name">Name of notification.</param>
        /// <param name="fulllocation">Location of template.</param>
        /// <param name="fileName">Full name of notification.</param>
        /// <param name="isAssemblyEmbedded">True if the template is 
        /// embedded in an assembly.</param>
        /// <param name="assemblyName">Name of assembly.</param>
        /// <param name="isXsl">True if the notification is xsl based.</param>
        public NotificationDef(string name, string fulllocation, string fileName, bool isAssemblyEmbedded, string assemblyName, bool isXsl)
        {
            Name = name;
            Location = fulllocation;
            FileName = fileName;
            IsAssemblyEmbedded = isAssemblyEmbedded;
            AssemblyName = assemblyName;
            IsXsl = isXsl;
        }
    }



    /// <summary>
    /// Xsl file paths for various emails.
    /// </summary>
    public class NotificationDefinitions
    {
        /// <summary>
        /// Storage of the notification templates.
        /// </summary>
        private IDictionary<string, NotificationDef> _defs = new Dictionary<string, NotificationDef>();


        /// <summary>
        /// Default the message templates to the internal ones provided by commonlibrary.net.
        /// </summary>
        public NotificationDefinitions()
        {
            _defs["WelcomeNewUser"] = new NotificationDef("WelcomeNewUser", "Notifications.Templates.welcome.html", "welcome.html", true, "CommonLibrary", false);
            _defs["ForgotPassword"] = new NotificationDef("ForgotPassword", "Notifications.Templates.password_reminder.html", "password_reminder.html", true, "CommonLibrary", false);
            _defs["SendWebSitePost"] = new NotificationDef("SendWebSitePost", "Notifications.Templates.sendtofriend_post.html", "sendtofriend_post.html", true, "CommonLibrary", false);
            _defs["SendWebSite"] = new NotificationDef("SendWebSite", "Notifications.Templates.sendtofriend_site.html", "sendtofriend_site.html", true, "CommonLibrary", false);
            _defs["SubmitFeedback"] = new NotificationDef("SubmitFeedback", "Notifications.Templates.feedback.html", "feedback.html", true, "CommonLibrary", false);
        }


        /// <summary>
        /// Get all the keys.
        /// </summary>
        public string[] Keys
        {
            get
            {
                string[] keys = new string[_defs.Keys.Count];
                _defs.Keys.CopyTo(keys, 0);
                return keys;
            }
        }


        /// <summary>
        /// Remove entry associated with the key.
        /// </summary>
        /// <param name="key">Key of entry to remove.</param>
        public void Remove(string key)
        {
            _defs.Remove(key);
        }


        /// <summary>
        /// Get / set the value.
        /// </summary>
        /// <param name="key">Key of entry to get/set.</param>
        /// <returns></returns>
        public NotificationDef this[string key]
        {
            get { return _defs[key]; }
            set { _defs[key] = value; }
        }
    }    
}
