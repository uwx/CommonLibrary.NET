using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Application
{
    /// <summary>
    /// Settings for the application template.
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Prefix for named args. "-"
        /// </summary>
        public string ArgsPrefix { get; set; }


        /// <summary>
        /// Key / value separator for Named args. ":".
        /// </summary>
        public string ArgsSeparator { get; set; }


        /// <summary>
        /// Object to apply the arguments to.
        /// </summary>
        public object ArgsReciever { get; set; }


        /// <summary>
        /// Whether or not to apply the arguments to the reciever.
        /// </summary>
        public bool   ArgsAppliedToReciever { get; set; }


        /// <summary>
        /// Whether or not any arguments are required at all.
        /// </summary>
        public bool   ArgsRequired { get; set; }


        /// <summary>
        /// Whether or not to send email on completion.
        /// </summary>
        public bool SendEmailOnCompletion { get; set; }


        /// <summary>
        /// Whether or not config files are required.
        /// </summary>
        public bool RequireConfigs { get; set; }


        /// <summary>
        /// Initialize defaults.
        /// </summary>
        public AppConfig()
        {
            ArgsPrefix = "-";
            ArgsSeparator = ":";
            RequireConfigs = true;
        }
    }
}
