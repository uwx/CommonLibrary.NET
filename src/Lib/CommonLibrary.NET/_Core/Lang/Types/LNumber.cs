

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LNumber : LObject
    {   
        /// <summary>
        /// Initialize bool value.
        /// </summary>
        /// <param name="value"></param>
        public LNumber(double value)
        {
            _value = value;
            DataType = typeof(double);
        }


        /// <summary>
        /// Get boolean value.
        /// </summary>
        /// <returns></returns>
        public double ToNumber()
        {
            return (double)_value;
        }
    }
}
