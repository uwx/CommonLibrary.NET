using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodePlexDocGenerator
{
    /// <summary>
    /// Enumeration for the possible wiki styles.
    /// </summary>
    public enum WikiStyle
    {
        /// <summary>
        /// Use and parse xml markers.
        /// </summary>
        Xml_Markers = 0,


        /// <summary>
        /// Use the full file and remove the xml markers.
        /// </summary>
        Full_File = 1,


        /// <summary>
        /// Show a standard message instead of code.
        /// </summary>
        Not_Available = 2
    }
}
