using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ComLib.Macros
{
    /// <summary>
    /// Class to represent an html/xml like tag with attributes.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// The name of the macro tag.
        /// </summary>
        public string Name;


        /// <summary>
        /// The id of the macro tag if present
        /// </summary>
        public int Id;


        /// <summary>
        /// An ordered dictionary of the all the attributes of the tag
        /// </summary>
        public OrderedDictionary Attributes = new OrderedDictionary();


        /// <summary>
        /// The inner content of the tag ( between the start/end tags )
        /// </summary>
        public string InnerContent = string.Empty;


        /// <summary>
        /// Whether or not the tag is an empty tag.
        /// </summary>
        public bool IsEmpty;

        /// <summary>
        /// Starting index of the tag in the entire document/text/string.
        /// </summary>
        public int Length;


        /// <summary>
        /// The total length in characters of the macro in the original document/text/string.
        /// </summary>
        public int Position;
    }
}
