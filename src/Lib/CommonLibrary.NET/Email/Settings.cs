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
    /// Email Settings
    /// </summary>
    public class EmailServiceSettings : IEmailSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceSettings"/> class.
        /// </summary>
        public EmailServiceSettings()
        {
            UsePort = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="EmailServiceSettings"/> class.
        /// </summary>
        /// <param name="smtpService">The SMTP service.</param>
        /// <param name="port">The port.</param>
        public EmailServiceSettings(string smtpService, int port)
        {
            SmptServer = smtpService;
            Port = port;
        }


        #region Settings Members
        /// <summary>
        /// Gets or sets the SMPT server.
        /// </summary>
        /// <value>The SMPT server.</value>
        public string SmptServer { get; set; }


        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        public string From { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [use port].
        /// </summary>
        /// <value><c>true</c> if [use port]; otherwise, <c>false</c>.</value>
        public bool UsePort { get; set; }


        /// <summary>
        /// Gets or sets the name of the authentication user.
        /// </summary>
        /// <value>The name of the authentication user.</value>
        public string AuthenticationUserName { get; set; }


        /// <summary>
        /// Gets or sets the authentication password.
        /// </summary>
        /// <value>The authentication password.</value>
        public string AuthenticationPassword { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is authentication required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authentication required; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticationRequired { get; set; }


        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }
        #endregion
    }
}
