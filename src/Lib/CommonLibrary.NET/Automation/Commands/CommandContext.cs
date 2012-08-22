using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace ComLib.Automation
{
    /// <summary>
    /// Contextual information supplied to a command to execute
    /// </summary>
    public class CommandContext
    {
        /// <summary>
        /// Name of the command. This can be either in format "Name" or "Name"."Function". 
        /// This corresponds to basically either the name parameter of the CommandAttribute on the class or 
        /// the Name.Function to support 1 command handling multiple methods calls.
        /// e.g. The following commands/methods calls
        /// 1. Blog.Delete(1);
        /// 2. Blog.Create({title: "testing", content: "blog content"});
        /// 
        /// Can all be handled by 1 command called "Blog" where that CommandAttribute has "IsMultiMethodEnabled" set to true.
        /// </summary>
        public string Name;


        /// <summary>
        /// Whether or not the command is a method call as in Class.Method rather than a function call.
        /// </summary>
        public bool IsMethodCall;


        /// <summary>
        /// The parameter list to the command.
        /// </summary>
        public List<object> ParamList;


        /// <summary>
        /// Named parameters to the command
        /// </summary>
        public object ParamMap;


        /// <summary>
        /// The attribute metadata
        /// </summary>
        public CommandAttribute Meta;
    }
}
