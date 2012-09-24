
using System.Collections.Generic;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array datatype
    /// </summary>
    public class LArray : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val">Value of the string</param>
        public LArray(List<object> val) : this(null, val) 
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="varName">Name of this variable</param>
        /// <param name="val">Value of the string</param>
        public LArray(string varName, List<object> val) 
        {
            _value = val;
            Raw = val;
        }


        /// <summary>
        /// Raw value
        /// </summary>
        public List<object> Raw;
    }
}
