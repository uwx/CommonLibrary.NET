


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LString : LObject
    {
        

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val">The raw string value</param>
        public LString(string val) : this(null, val)
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val">Value of the string</param>
        /// <param name="varName">Name of the variable</param>
        public LString(string varName, string val)
        {
            _varName = varName;
            DataType = typeof(string);
            Raw = val;
        }


        /// <summary>
        /// The raw string value.
        /// </summary>
        public string Raw;


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
