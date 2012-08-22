using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace ComLib.VersionCheck
{
    /// <summary>
    /// Match condition
    /// </summary>    
    public enum VersionMatch
    {
        /// <summary>
        /// Target Component must equal ( match exactly ) against definition.
        /// </summary>
        Equal,


        /// <summary>
        /// Target component must be equal to or greater than definition.
        /// </summary>
        GreaterEqual,


        /// <summary>
        /// Target component must be equal to or less than definition.
        /// </summary>
        LessEqual,


        /// <summary>
        /// Target component must not be equal to the definition.
        /// </summary>
        NotEqual,


        /// <summary>
        /// Indicates that the match can be any of the above.
        /// Useful just for checking if files exist.
        /// </summary>
        Any
    };



    /// <summary>
    /// Check mode.
    /// </summary>
    public enum VersionRequirement
    { 
        /// <summary>
        /// Indicates that the definition component is required ( on target computer )
        /// </summary>
        Required,


        /// <summary>
        /// Indicates that the definition component can be optional.
        /// </summary>
        Optional
    };
}
