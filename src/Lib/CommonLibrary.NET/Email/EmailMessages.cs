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



namespace ComLib.EmailSupport
{
    /// <summary>
    /// Notification message.
    /// </summary>
    public class NotificationMessage
    {
        /// <summary>
        /// Who the message is to
        /// </summary>
        public string To;


        /// <summary>
        /// Who the message is from.
        /// </summary>
        public string From;


        /// <summary>
        /// Subject of the message
        /// </summary>
        public string Subject;


        /// <summary>
        /// Body of the message
        /// </summary>
        public string Body;


        /// <summary>
        /// Template id to link the message to the template for the message
        /// </summary>
        public string MessageTemplateId;


        /// <summary>
        /// Whether or not this message body contains html.
        /// </summary>
        public bool IsHtml = true;


        /// <summary>
        /// String of values to supply to the message.
        /// </summary>
        public IDictionary<string, string> Values;
        

        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="messageTemplateId"></param>
        public NotificationMessage(IDictionary<string, string> values, string to, string from, string subject, string messageTemplateId)
        {
            To = to;
            From = from;
            Subject = subject;
            Values = values;
            MessageTemplateId = messageTemplateId;
        }
    }



    /// <summary>
    /// Basic email message.
    /// </summary>
    public class EmailMessage
    {

        /// <summary>
        /// From email address.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// To email address.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Subject of email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of email.
        /// </summary>
        public string Body { get; set; }


        /// <summary>
        /// Whether or not the body message contains html.
        /// </summary>
        public bool IsHtml { get; set; }
    }
}
