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
using System.Threading;
using System.IO;
using ComLib;
using ComLib.Logging;
using ComLib.EmailSupport;
using ComLib.Queue;



namespace ComLib.Notifications
{
    /// <summary>
    /// Queue to store the notification messages.
    /// This is the default implementation to store them in memory
    /// rather than in some persistant storage (e.g. database ).
    /// </summary>
    public class NotificationQueueInMemory : QueueProcessor<NotificationMessage>
    {
        private NotificationSettings _settings;
        private IEmailService _emailService;
        private NotificationItemProcessor _notificationProcessor;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="config">Instance of notification settings.</param>
        /// <param name="emailService">Instance of email service.</param>
        /// <param name="messageDefs">Instance of notification definitions.</param>
        public NotificationQueueInMemory(NotificationSettings config, IEmailService emailService, NotificationDefinitions messageDefs = null)            
        {
            _settings = config;
            _emailService = emailService;
            _notificationProcessor = new NotificationItemProcessor(config, _emailService, messageDefs);
            NumberToProcessPerDequeue = _settings.NumberOfMessagesToProcessAtOnce;            
        }


        /// <summary>
        /// Override the process method to handle notification messages.
        /// </summary>
        /// <param name="itemsToProcess">List of notification messages to process.</param>
        protected override void Process(IList<NotificationMessage> itemsToProcess)
        {            
            // Now send each message.
            foreach (NotificationMessage message in itemsToProcess)
            {
                _notificationProcessor.Process(message);
            }
            if (_settings.LogMessage)
            {
                Logger.Info("Finished processing " + itemsToProcess.Count + " email notifications.");
            }
        }        
    }



    /// <summary>
    /// Class that actually generates the messages and
    /// sends them out.
    /// </summary>
    public class NotificationItemProcessor
    {
        private IEmailService _emailService;
        private NotificationSettings _settings;
        private NotificationDefinitions _messageDefs;
        private int _debugOutputId = 0;


        /// <summary>
        /// Notification processor.
        /// </summary>
        /// <param name="config">Instance of notification settings.</param>
        /// <param name="emailService">Instance of email service.</param>
        /// <param name="messageDefs">Instance of notification definitions.</param>
        public NotificationItemProcessor(NotificationSettings config, IEmailService emailService, NotificationDefinitions messageDefs = null)
        {
            _settings = config;
            _emailService = emailService;
            _messageDefs = messageDefs;
        }


        /// <summary>
        /// Processes the notification message. Either just sends the data or transforms the
        /// data and then sends it.
        /// </summary>
        /// <param name="currentMessage">Notification message to process.</param>
        public void Process(NotificationMessage currentMessage)
        {
            try
            {
                string messageBody = string.Empty;

                // Currently the templates are hardcoded to get the files from this assembly.
                messageBody = GetMessageDef(currentMessage.MessageTemplateId);               
                messageBody = StringHelper.Substitute(currentMessage.Values, messageBody);
                
                // Now replace using the global notification substitution settings.
                messageBody = StringHelper.Substitute(_settings.Settings, messageBody);
                currentMessage.Body = messageBody;


                /*
                if (currentMessage.PerformTransform)
                {
                    string xslFilePath = _xmlDirectoryLocation + "\\Xsl\\" + currentMessage.XslFilePath + ".xsl";
                    string xml = XmlSerializerUtil.XmlSerialize(currentMessage.Adaptor);
                    messageBody = XmlUtils.TransformXml(xml, xslFilePath);
                    currentMessage.Body = messageBody;
                }
                else
                {
                    messageBody = currentMessage.Body;
                }
                */
                // Output the transform to debug file.
                if (_settings.DebugOutputMessageToFile)
                {
                    OutputContent(currentMessage, messageBody);
                }

                // Send the notification if enabled.
                if (_settings.EnableNotifications)
                {                
                    // Notify
                    _emailService.Send(currentMessage);                   
                }                
                else if(_settings.DebugSleepIfNotEnabled)
                {
                    Thread.Sleep(_settings.DebugSleepTimeIfNotEnabled);
                    if (_settings.LogMessage)
                    {
                        Logger.Info("Notifications are not enabled. Simulating single email notification by sleeping.");
                    }
                }

            }
            catch (Exception ex)
            {
                string error = string.Empty;
                if (currentMessage != null)
                {
                    error = currentMessage.To + " " + currentMessage.MessageTemplateId;
                }
                Logger.Error("Unable to send notification : " + error, ex);
            }
        }


        /// <summary>
        /// Simply used for generating unique filenames for testing purposes.
        /// </summary>
        /// <returns>Next id.</returns>
        private int GetNextId()
        {
            Interlocked.Increment(ref _debugOutputId);
            return _debugOutputId;
        }


        private string GetMessageDef(string templateName)
        {
            NotificationDef def = null;
            if (_messageDefs != null)
                def = _messageDefs[templateName];

            string contents = string.Empty;
            // Can use configurable template in folder.
            if (def != null )
            {
                bool useEmbeddedFile = false;
                if (!string.IsNullOrEmpty(_settings.TemplateFolderPath))
                {
                    string templatefilePath = Path.Combine(_settings.TemplateFolderPath, templateName);
                    if (File.Exists(templatefilePath))
                    {
                        contents = File.ReadAllText(templatefilePath);
                    }
                    else
                        useEmbeddedFile = true;
                }
                if (useEmbeddedFile)
                {                    
                    // Now get and replace values.                    
                    contents = NotificationUtils.GetInternalNotificationTemplate(def.FileName);
                }
            }
            return contents;
        }


        /// <summary>
        /// Write the message to file for debugging purposes.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="htmlBody"></param>
        private void OutputContent(NotificationMessage message, string htmlBody)
        {
            string filePath = _settings.DebugOutputMessageFolderPath;
            try
            {                
                int messageId = GetNextId();
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string messageName = message.MessageTemplateId + messageId;
                filePath += "\\message_" + messageName + ".html";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(htmlBody);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Unable to write notification message to file : " + filePath );
                Logger.Error("Error : " + ex.Message);
            }
        }
    }
}
