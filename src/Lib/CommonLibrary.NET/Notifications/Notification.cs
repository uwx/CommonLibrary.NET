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
using ComLib.Queue;
using ComLib.EmailSupport;


namespace ComLib.Notifications
{

    /// <summary>
    /// Notification services.
    /// </summary>
    public class Notifier 
    {
        private static INotificationAccountService _accountNotifyService;
        private static INotificationMessagingService _messageNotifyService;
        private static NotificationDefinitions _messageDefinitions;
        private static QueueProcessor<NotificationMessage> _queue;
        private static NotificationSettings _settings = new NotificationSettings();
        private static object _syncroot = new object();


        /// <summary>
        /// Initialize using default settings.
        /// </summary>
        public static void Init(IEmailService emailService, NotificationSettings settings)
        {
            _settings = settings;
            _messageDefinitions = new NotificationDefinitions(); 
            _queue = new NotificationQueueInMemory(_settings, emailService, _messageDefinitions);            
            _accountNotifyService = new NotificationAccountService(_queue, _settings);
            _messageNotifyService = new NotificationMessagingService(_queue, _settings);            
        }


        /// <summary>
        /// Send welcome email to new user.
        /// </summary>
        /// <param name="to">E-mail to.</param>
        /// <param name="subject">E-mail subject.</param>
        /// <param name="firstname">First name of user.</param>
        /// <param name="username">Username of user.</param>
        /// <param name="password">Password of user.</param>
        public static void WelcomeNewUser(string to, string subject, string firstname, string username, string password)
        {
            _accountNotifyService.WelcomeNewUser(to, subject, firstname, username, password);
        }


        /// <summary>
        /// Send the website url to a friend.
        /// </summary>
        /// <param name="toEmail">E-mail to.</param>
        /// <param name="subject">E-mail subject.</param>
        /// <param name="friendName">Friend's name.</param>
        /// <param name="fromName">From name.</param>
        /// <param name="messageFromFriend">Message text.</param>
        public static void SendToFriend(string toEmail, string subject, string friendName, string fromName, string messageFromFriend)
        {
            _messageNotifyService.SendToFriend(toEmail, subject, friendName, fromName, messageFromFriend);
        }


        /// <summary>
        /// Send an email to a friend with a link to a post on the page.
        /// </summary>
        /// <param name="toEmail">E-mail to.</param>
        /// <param name="subject">E-mail subject.</param>
        /// <param name="toFirstname">Name of recipient.</param>
        /// <param name="fromName">Name of sender.</param>
        /// <param name="messageToFriend">Message text.</param>
        /// <param name="postTitle">Title of post.</param>
        /// <param name="postUrl">Title of url.</param>
        public static void SendToFriendPost(string toEmail, string subject, string toFirstname,
            string fromName, string messageToFriend, string postTitle, string postUrl)
        {
            _messageNotifyService.SendToFriendPost(toEmail, subject, toFirstname, fromName, messageToFriend, postTitle, postUrl);
        }


        /// <summary>
        /// Send remind account/password email to user.
        /// </summary>
        /// <param name="to">E-mail to.</param>
        /// <param name="subject">E-mail subject.</param>
        /// <param name="firstname">First name of user.</param>
        /// <param name="username">Username of user.</param>
        /// <param name="password">Password of user.</param>
        public static void RemindUserPassword(string to, string subject, string firstname, string username, string password)
        {
            _accountNotifyService.RemindUserPassword(to, subject, firstname, username, password);
        }


        #region INotificationService Members
        /// <summary>
        /// Process the queue of notifications.
        /// </summary>
        public static void Process()
        {
            Queue.Process();
        }


        /// <summary>
        /// Get/Set the settings.
        /// </summary>
        public static NotificationSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        /// <summary>
        /// The notification message queue.
        /// </summary>
        public static QueueProcessor<NotificationMessage> Queue
        {
            get { return _queue; }
            set { _queue = value; }
        }


        /// <summary>
        /// Message definitions.
        /// </summary>
        public static NotificationDefinitions MessageDefs
        {
            get { return _messageDefinitions; }
        }


        /// <summary>
        /// The account services.
        /// </summary>
        public static INotificationAccountService AccountService
        {
            get { return _accountNotifyService; }
            set { _accountNotifyService = value; }
        }        


        /// <summary>
        /// Messaging service for send feedback, post, links to site.
        /// </summary>
        public static INotificationMessagingService MessageService
        {
            get { return _messageNotifyService; }
            set { _messageNotifyService = value; }
        }

        #endregion


        #region private members
        private INotificationAccountService GetAccountService()
        {
            if (_accountNotifyService == null)
            {
                lock (_syncroot)
                {
                    _accountNotifyService = new NotificationAccountService(_queue, _settings);
                    _accountNotifyService.Settings = _settings;
                }
            }
            return _accountNotifyService;
        }


        private INotificationMessagingService GetMessageService()
        {
            if (_messageNotifyService == null)
            {
                lock (_syncroot)
                {
                    _messageNotifyService = new NotificationMessagingService(_queue, _settings);
                    _messageNotifyService.Settings = _settings;
                }
            }
            return _messageNotifyService;
        }
        #endregion
    }
}
