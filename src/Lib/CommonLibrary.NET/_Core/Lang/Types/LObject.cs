using System;

using ComLib.Lang.Core;


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
        /// The value of the type.
        /// </summary>
        public readonly int TypeVal { get; set; }


        /// <summary>
        /// Whether or not this is a basic type e.g. bool, date.
        /// </summary>
        /// <returns></returns>
        public bool IsBasicType()
        {
            return this.TypeVal >= TypeConstants.Bool 
                && this.TypeVal <= TypeConstants.Time;
        }
        

        /// <summary>
        /// Value of the type.
        /// </summary>
        public virtual object ToValue()
        {
            return _value;
        }
    }
}
