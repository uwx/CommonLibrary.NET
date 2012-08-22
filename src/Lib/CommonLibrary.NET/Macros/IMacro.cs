using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Reflection;


namespace ComLib.Macros
{
    /// <summary>
    /// Interface for a macro
    /// </summary>
    public interface IMacro
    {
        /// <summary>
        /// Processes a tag.
        /// </summary>
        /// <param name="tag">Instance of tag to process.</param>
        /// <returns>Content for this tag.</returns>
        string Process(Tag tag);
    }
}
