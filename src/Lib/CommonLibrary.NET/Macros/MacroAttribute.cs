using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ComLib;


namespace ComLib.Macros
{  
    /// <summary>
    /// Attribute used to define a widget.
    /// </summary>
    public class MacroAttribute : ExtensionAttribute
    {
        /// <summary>
        /// Example of the inner content.
        /// </summary>
        public string ExampleInnerContent { get; set; }


        /// <summary>
        /// Whether or not this macro supports inner content.
        /// </summary>
        public bool HasInnerContent { get; set; }
    }
}
