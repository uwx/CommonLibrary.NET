using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ComLib.Web.Templating
{
    /// <summary>
    /// Class to represent an html/xml like tag with attributes.
    /// </summary>
    public class Tag
    {
        public string Name;
        public int Id;
        public OrderedDictionary Attributes = new OrderedDictionary();
        public string InnerContent = string.Empty;
        public bool IsEmpty;

        /// <summary>
        /// Starting index of the tag in the entire doc.
        /// </summary>
        public int Length;
        public int Position;
    }
}
