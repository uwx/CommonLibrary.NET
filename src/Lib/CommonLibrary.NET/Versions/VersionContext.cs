using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace ComLib.VersionCheck
{
    /// <summary>
    /// Context data for running a version check.
    /// </summary>    
    public class VersionContext
    {
        /// <summary>
        /// Definition
        /// </summary>
        public VersionDefinition Definition { get; set; }


        /// <summary>
        /// Settings
        /// </summary>
        public VersionConfig Settings { get; set; }
    }
}
