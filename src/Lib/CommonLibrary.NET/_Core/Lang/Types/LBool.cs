using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LBool : LObject
    {
        /// <summary>
        /// Initialize bool value.
        /// </summary>
        /// <param name="value"></param>
        public LBool(bool value)
        {
            _value = value;
            DataType = typeof(bool);
        }


        /// <summary>
        /// Get boolean value.
        /// </summary>
        /// <returns></returns>
        public bool ToValue()
        {
            return (bool)_value;
        }
    }
}
