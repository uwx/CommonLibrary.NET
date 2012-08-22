using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.VersionCheck
{
    public class VersionConfig
    {
        /// <summary>
        /// Whether to log the version check.
        /// </summary>
        public bool Log { get; set; }


        /// <summary>
        /// Whether or not to send an email after component.
        /// </summary>
        public bool SendEmail { get; set; }


        public VersionConfig()
        {
            Log = true;
            SendEmail = true;
        }
    }
}
