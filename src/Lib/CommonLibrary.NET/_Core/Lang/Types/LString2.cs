using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Helpers;
using ComLib.Lang.Parsing;
// </lang:using>


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LString2 : LObject
    {
        /// <summary>
        /// The raw string value.
        /// </summary>
        public string Raw;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val">Value of the string</param>
        /// <param name="varName">Name of the variable</param>
        public LString2(string varName, string val)
        {   
            _varName = varName;
            DataType = typeof(string);
            Raw = val;
        }


        /// <summary>
        /// Get boolean value.
        /// </summary>
        /// <returns></returns>
        public override object ToValue()
        {
            return this.Raw;
        }
    }
}
