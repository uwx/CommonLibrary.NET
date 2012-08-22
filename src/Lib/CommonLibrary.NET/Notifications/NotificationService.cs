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
    /// Base class for notifications.
    /// </summary>
    public class NotificationServiceBase
    {
        /// <summary>
        /// The queue the stores the notification messages.
        /// </summary>
        protected QueueProcessor<NotificationMessage> _queue = null;


        /// <summary>
        /// The notification settings.
        /// </summary>
        protected NotificationSettings _settings;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="queue">Instance of queue processor for notification messages.</param>
        /// <param name="settings">Notification settings.</param>
        public NotificationServiceBase(QueueProcessor<NotificationMessage> queue, NotificationSettings settings)
        {
            _settings = settings;
            _queue = queue; 
        }


        /// <summary>
        /// Notification settings.
        /// </summary>
        public NotificationSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        /// <summary>
        /// The notification queue.
        /// </summary>
        public QueueProcessor<NotificationMessage> Queue
        {
            get { return _queue; }
            set { _queue = value; }
        }


        /// <summary>
        /// Queue the notification.
        /// Don't just send it directly.
        /// </summary>
        /// <param name="message">Notification message to put to queue.</param>
        protected void Notify(NotificationMessage message)
        {
            _queue.Enqueue(message);
        }
    }



    /// <summary>
    /// The notification service for sending feedback, sending to a friend.
    /// </summary>
    public class NotificationAccountService : NotificationServiceBase, INotificationAccountService
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="queue">Instance of queue processor for notification messages.</param>
        /// <param name="settings">Notification settings.</param>
        public NotificationAccountService(QueueProcessor<NotificationMessage> queue, NotificationSettings settings)
            : base(queue, settings)
        {
        }


        #region INotificationAccountService Members
        /// <summary>
        /// Queues the notification to email to a friend.
        /// ${message.to}
        /// ${message.from}
        /// ${message.subject}
        /// ${message.briefmessage}
        /// </summary>
        /// <param name="firstname">first name</param>
        /// <param name="password">Password</param>
        /// <param name="subject">Subject of the message.</param>
        /// <param name="to">The email address to send to</param>
        /// <param name="username">User name</param>
        public void WelcomeNewUser(string to, string subject, string firstname, string username, string password)
        {
            NotificationContext ctx = new NotificationContext();
            ctx.Values["message.to"] = to;
            ctx.Values["message.subject"] = subject;
            ctx.Values["firstname"] = firstname;
            ctx.Values["username"] = username;
            ctx.Values["password"] = password;
            WelcomeNewUser(ctx);
        }


        /// <summary>
        /// Welcome new user using Notification context.
        /// </summary>
        /// <param name="ctx">Instance of notification context.</param>
        public void WelcomeNewUser(NotificationContext ctx)
        {
            Notify(new NotificationMessage(ctx.Values, ctx.Values["message.to"], string.Empty, 
                ctx.Values["message.subject"], "WelcomeNewUser"));            
        }


        /// <summary>
        /// Send remind account/password email to user.
        /// </summary>
        /// <param name="firstname">first name</param>
        /// <param name="password">Password</param>
        /// <param name="subject">Subject of the message.</param>
        /// <param name="to">The email address to send to</param>
        /// <param name="username">User name</param>
        public void RemindUserPassword(string to, string subject, string firstname, string username, string password)
        {
            NotificationContext ctx = new NotificationContext();
            ctx.Values["message.to"] = to;
            ctx.Values["message.subject"] = subject;
            ctx.Values["firstname"] = firstname;
            ctx.Values["username"] = username;
            ctx.Values["password"] = password;
            RemindUserPassword(ctx);
        }

        /// <summary>
        /// Submits the feedback notification.
        /// ${message.to}
        /// ${message.from}
        /// ${message.subject}
        /// ${message.briefmessage}
        /// ${from.name}
        /// ${post.url}
        /// ${post.title}
        /// ${friend.name}
        /// ${friend.email}
        /// </summary>
        /// <param name="ctx">Instance of notification context.</param>
        public void RemindUserPassword(NotificationContext ctx)
        {
            Notify(new NotificationMessage(ctx.Values, ctx.Values["message.to"], string.Empty,
                ctx.Values["message.subject"], "ForgotPassword"));             
        }
        #endregion
    }
    


    /// <summary>
    /// The notification service for sending feedback, sending to a friend.
    /// </summary>
    public class NotificationMessagingService : NotificationServiceBase, INotificationMessagingService
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="queue">Instance of queue processor for notification messages.</param>
        /// <param name="settings">Notification settings.</param>
        public NotificationMessagingService(QueueProcessor<NotificationMessage> queue, NotificationSettings settings ) 
            : base(queue, settings)
        {
        }


        #region INotificationMessagingService Members
        /// <summary>
        /// Send the website url to a friend.
        /// </summary>
        /// <param name="toEmail">E-mail to.</param>
        /// <param name="subject">E-mail subject.</param>
        /// <param name="friendName">Friend's name.</param>
        /// <param name="fromName">From name.</param>
        /// <param name="messageFromFriend">Message text.</param>
        public void SendToFriend(string toEmail, string subject, string friendName, string fromName, string messageFromFriend)
        {
            NotificationContext ctx = new NotificationContext();
            ctx.Values["message.to"] = toEmail;
            ctx.Values["message.subject"] = subject;
            ctx.Values["firstname"] = friendName;
            ctx.Values["from.name"] = fromName;
            ctx.Values["message.briefmessage"] = messageFromFriend;
            SendToFriend(ctx);
        }


        /// <summary>
        /// Queues the notification to email to a friend.
        /// ${message.to}
        /// ${message.from}
        /// ${message.subject}
        /// ${message.briefmessage}
        /// ${from.name}
        /// ${post.url}
        /// ${post.title}
        /// ${friend.name}
        /// ${friend.email}
        /// </summary>
        /// <param name="ctx">Instance of notification context.</param>
        public void SendToFriend(NotificationContext ctx)
        {
            Notify(new NotificationMessage(
                ctx.Values, ctx.Values["message.to"], string.Empty,
                ctx.Values["message.subject"], "SendWebSite"));
        }


        /// <summary>
        /// Submits the feedback notification.
        /// ${message.to}
        /// ${message.from}
        /// ${message.subject}
        /// ${message.briefmessage}
        /// ${from.name}
        /// ${post.url}
        /// ${post.title}
        /// ${friend.name}
        /// ${friend.email}
        /// </summary>
        /// <param name="ctx">Instance of notification context.</param>
        public void SubmitFeedBack(NotificationContext ctx)
        {
            Notify(new NotificationMessage(
                ctx.Values, ctx.Values["message.to"], string.Empty,
                ctx.Values["message.subject"], "SubmitFeedback"));
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
        public void SendToFriendPost(string toEmail, string subject, string toFirstname,
            string fromName, string messageToFriend, string postTitle, string postUrl)
        {
            NotificationContext ctx = new NotificationContext();            
            ctx.Values["firstname"] = toFirstname;
            ctx.Values["post.title"] = postTitle;
            ctx.Values["post.url"] = postUrl;
            ctx.Values["from.name"] = fromName;
            ctx.Values["message.briefmessage"] = messageToFriend;
            ctx.Values["message.subject"] = subject;
            ctx.Values["message.to"] = toEmail;
            SendToFriendPost(ctx);
        }


        /// <summary>
        /// Sends a link to a post to a friend.
        /// ${message.to}
        /// ${message.from}
        /// ${message.subject}
        /// ${message.briefmessage}
        /// ${from.name}
        /// ${post.url}
        /// ${post.title}
        /// ${friend.name}
        /// ${friend.email}
        /// </summary>
        /// <param name="ctx">Instance of notification context.</param>
        public void SendToFriendPost(NotificationContext ctx)
        {
            Notify(new NotificationMessage(
                ctx.Values, ctx.Values["message.to"], string.Empty,
                ctx.Values["message.subject"], "SendWebSitePost"));
        }
        #endregion
    }
}
