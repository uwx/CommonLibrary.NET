using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// Tuple for success/failure flag, string error, and object.
    /// </summary>
    public class BoolMsgItem
    {
        /// <summary>
        /// Sucess/failure flag.
        /// </summary>
        public readonly bool Success;


        /// <summary>
        /// Error message
        /// </summary>
        public readonly string Message;


        /// <summary>
        /// Object associated with this success/failure result.
        /// </summary>
        public readonly object Item;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="item"></param>
        public BoolMsgItem(bool success, string message, object item)
        {
            Success = success;
            Message = message;
            Item = item;
        }
    }
}
