using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace ComLib.Automation
{  
    /// <summary>
    /// Attribute used to define a command class used in automation.
    /// </summary>
    public class CommandAttribute : ExtensionAttribute
    {
        /// <summary>
        /// Initialize defaults
        /// </summary>
        public CommandAttribute()
        {
            AutoHandleMethodCalls = true;
        }


        /// <summary>
        /// Whether or not this command can handle multiple methods calls.
        /// e.g. Command named "Blog" can handle method calls such as Blog.Delete(1); Blog.Create("test"); etc.
        /// </summary>
        public bool IsMultiMethodEnabled { get; set; }


        /// <summary>
        /// Handle method calls automatically
        /// </summary>
        public bool AutoHandleMethodCalls { get; set; }


        /// <summary>
        /// Whether or not to automap arguments from input to public instance settable properties on the command class.
        /// </summary>
        public bool AutoMap { get; set; }
    }
}
