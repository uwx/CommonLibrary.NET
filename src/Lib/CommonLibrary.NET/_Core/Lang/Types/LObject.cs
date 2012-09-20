using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Base type for all data types.
    /// </summary>
    public class LObject
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        protected string _varName;


        /// <summary>
        /// Object value.
        /// </summary>
        protected object _value;


        /// <summary>
        /// The datatype.
        /// </summary>
        public Type DataType;


        /// <summary>
        /// Value of the type.
        /// </summary>
        public virtual object ToValue()
        {
            return _value;
        }
    }
}
