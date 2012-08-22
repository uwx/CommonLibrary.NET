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
using System.Text;

using System.Net;
using System.Net.Mail;

namespace ComLib.EmailSupport
{

    /// <summary>
    /// Email service.
    /// Simplly sends an email.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Configuration information needed for sending emails.
        /// </summary>
        IEmailSettings Settings { get; set; }


        /// <summary>
        /// Emails the message using default values from Settings.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Send(EmailMessage message);


        /// <summary>
        /// Emails the message using the connection strings supplied.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="useCredentials"></param>
        /// <param name="credentialsUser"></param>
        /// <param name="credentialsPassword"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        BoolMessageEx Send(EmailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword, string host, int port);


        /// <summary>
        /// Sends an email using the data from the message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Send(NotificationMessage message);


        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="credentialsUser">The credentials user.</param>
        /// <param name="credentialsPassword">The credentials password.</param>
        /// <returns></returns>
        bool Send(NotificationMessage message, string credentialsUser, string credentialsPassword);


        /// <summary>
        /// Send the mailmessage.
        /// </summary>
        /// <param name="message">The mail message</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>        
        bool Send(MailMessage message, bool useCredentials, string credentialsUser, string credentialsPassword);


        /// <summary>
        /// Mail the message using the native MailMessage class.
        /// </summary>
        /// <param name="from">Who the email is from.</param>
        /// <param name="to">Who the email is being sent to.</param>
        /// <param name="subject">Subject of email.</param>
        /// <param name="body">Email body.</param>
        /// <param name="useCredentials">Whether or not to use credentials for security when sending emails.</param>
        /// <param name="credentialsUser">User name when using credentials.</param>
        /// <param name="credentialsPassword">Password when using credentials.</param>
        /// <returns></returns>        
        bool Send(string from, string to, string subject, string body, bool useCredentials, string credentialsUser, string credentialsPassword);


        /// <summary>
        /// Mail the message using the native MailMessage class and the credentials from the current configuration.
        /// </summary>
        /// <param name="mailMessage">The mail message</param>
        /// <returns></returns>
        bool Send(MailMessage mailMessage);
    }



    /// <summary>
    /// Interface for email settings.
    /// </summary>
    public interface IEmailSettings
    {
        /// <summary>
        /// Gets or sets the SMPT server.
        /// </summary>
        /// <value>The SMPT server.</value>
        string SmptServer { get; set; }


        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        string From { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [use port].
        /// </summary>
        /// <value><c>true</c> if [use port]; otherwise, <c>false</c>.</value>
        bool UsePort { get; set; }


        /// <summary>
        /// Gets or sets the name of the authentication user.
        /// </summary>
        /// <value>The name of the authentication user.</value>
        string AuthenticationUserName { get; set; }


        /// <summary>
        /// Gets or sets the authentication password.
        /// </summary>
        /// <value>The authentication password.</value>
        string AuthenticationPassword { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is authentication required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authentication required; otherwise, <c>false</c>.
        /// </value>
        bool IsAuthenticationRequired { get; set; }


        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        int Port { get; set; }
    }
}
