

namespace ComLib.Lang.Types
{
    /// <summary>
    /// The result of an evaluation of AST node. Combines the result and it's type.
    /// </summary>
    public class LTypeValue
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="result">The object result</param>
        /// <param name="type">The type of the result</param>
        public LTypeValue(object result, LType type)
        {
            this.Result = result;
            this.Type = type;
        }


        /// <summary>
        /// The result of an expression.
        /// </summary>
        public object Result;


        /// <summary>
        /// The datatype of the result.
        /// </summary>
        public LType Type;
    }
}
