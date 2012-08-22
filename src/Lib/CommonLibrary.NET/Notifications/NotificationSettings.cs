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

using ComLib.Collections;


namespace ComLib.Notifications
{
    /// <summary>
    /// Notification configuration.
    /// </summary>
    //[Configuration("appGlobal", "notifyConfig", true)]
    public class NotificationSettings
    {
        private Dictionary<string, string> _settings = new Dictionary<string, string>();


        /// <summary>
        /// Default constructor to support dependency injection of properties by Spring.
        /// </summary>
        public NotificationSettings()
        {
            IntervalSchedule = 10000;
            DebugSleepIfNotEnabled = false;
            DebugSleepTimeIfNotEnabled = 1000;
            NumberOfMessagesToProcessAtOnce = 10;
            LogMessage = false;
        }


        /// <summary>
        /// initialize using supplied settings.
        /// </summary>
        /// <param name="enableNotifications">True to enable notifications.</param>
        /// <param name="from">Sender.</param>
        /// <param name="intervalSchedule">Process interval.</param>
        public NotificationSettings(bool enableNotifications, string from, int intervalSchedule)
        {
            EnableNotifications = enableNotifications;
            From = from;
            NumberOfMessagesToProcessAtOnce = 10;
            IntervalSchedule = intervalSchedule;
        }


        /// <summary>
        /// Get/set additional values.
        /// </summary>
        /// <param name="key">Key to settings item.</param>
        /// <returns>Value of settings item.</returns>
        public string this[string key]
        {
            get { return _settings[key]; }
            set { _settings[key] = value; }
        }


        /// <summary>
        /// Provide read-only access to settings.
        /// </summary>
        public DictionaryReadOnly<string, string> Settings
        {
            get { return new DictionaryReadOnly<string, string>(_settings); }
        }


        /// <summary>
        /// The folder location for the templates.
        /// </summary>
        public string TemplateFolderPath { get; set;  }


        /// <summary>
        /// From.
        /// </summary>
        public string From { get; set; }


        /// <summary>
        /// Url of the website.
        /// http://www.knowledgedrink.com
        /// </summary>
        public string WebSiteUrl { get; set; }


        /// <summary>
        /// Url for looking at a specific post 
        /// </summary>
        public string WebSiteUrlForPost { get; set; }


        /// <summary>
        /// Flag to enable/disable notifications.
        /// </summary>
        public bool EnableNotifications { get; set; }


        /// <summary>
        /// Number of messages to send per batch.
        /// </summary>
        public int NumberOfMessagesToProcessAtOnce { get; set; }


        /// <summary>
        /// How often to check for and send messages.
        /// </summary>
        public int IntervalSchedule { get; set; }


        /// <summary>
        /// The email to use to send user feedback to.
        /// </summary>
        public string FeedBackEmail { get; set; }


        /// <summary>
        /// For debugging purposes, whether or not to debug messages to file.
        /// </summary>
        public bool DebugOutputMessageToFile { get; set; }


        /// <summary>
        /// Location of debug messages.
        /// </summary>
        public string DebugOutputMessageFolderPath { get; set; }


        /// <summary>
        /// Flag to simulate sending emails by sleeping thread.
        /// </summary>
        public bool DebugSleepIfNotEnabled { get; set; }


        /// <summary>
        /// Amount of time to simulate sending.
        /// </summary>
        public int DebugSleepTimeIfNotEnabled { get; set; }


        /// <summary>
        /// Log message for errors.
        /// </summary>
        public bool LogMessage { get; set; }
    }
}
