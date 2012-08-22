using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Macros
{
    /// <summary>
    /// Attributes.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true)]
    public class MacroParameterAttribute : ExtensionArgAttribute
    {      

        /// <summary>
        /// Allow initialize via named property initializers.
        /// </summary>
        public MacroParameterAttribute() { }
    }
}
