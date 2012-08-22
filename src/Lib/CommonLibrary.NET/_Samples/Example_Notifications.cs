using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using ComLib;
using ComLib.Logging;
using ComLib.Application;
using ComLib.EmailSupport;
using ComLib.Notifications;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Notifications namespace.
    /// </summary>
    public class Example_Notifications : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // Configure the notification service.
            // Since emailing is disabled, the EmailServiceSettings are not configured.
            // The emails are not sent as "EnableNotifications" = false above.
            // Debugging is turned on so you can see the emails in the folder "Notifications.Tests".
            Notifier.Init(new EmailService(new EmailServiceSettings()), new NotificationSettings());
            Notifier.Settings["website.name"] = "KnowledgeDrink.com";
            Notifier.Settings["website.url"] = "http://www.KnowledgeDrink.com";
            Notifier.Settings["website.urls.contactus"] = "http://www.KnowledgeDrink.com/contactus.aspx";
            Notifier.Settings.EnableNotifications = false;
            Notifier.Settings.DebugOutputMessageToFile = true;
            Notifier.Settings.DebugOutputMessageFolderPath = @"Notifications.Tests";

            Logger.Info("====================================================");
            Logger.Info("NOTIFICATION EMAILS ");
            Logger.Info("Emails are generated to folder : " + Notifier.Settings.DebugOutputMessageFolderPath);
            Notifier.WelcomeNewUser("user1@mydomain.com", "Welcome to www.knowledgedrink.com", "batman", "user1", "password");
            Notifier.RemindUserPassword("user1@mydomain.com", "Welcome to www.knowledgedrink.com", "batman", "user1", "password");
            Notifier.SendToFriend("batman@mydomain.com", "Check out www.knowledgedrink.com", "superman", "bruce", "Learn to fight.");
            Notifier.SendToFriendPost("superman@mydomain.com", "Check out class at www.knowledgedrink.com", "batman", "clark", "Punk, learn to fly.",
                "Learn to fly", "http://www.knowledgedrink.com/classes/learn-to-fly.aspx");
            Notifier.Process();
            return BoolMessageItem.True;
        }
    }
}
