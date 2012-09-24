using System;

// <lang:using>
using ComLib.Lang.Types;
// </lang:using>

namespace ComLib.Lang.AST
{
    /// <summary>
    /// Variable expression data
    /// </summary>
    public class ConstantExpr : ValueExpr
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val"></param>
        public ConstantExpr(object val)
        {
            this.Value = val;
            this.DataType = val.GetType();
        }


        /// <summary>
        /// Evaluate value.
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            /*
            if (this.Value.GetType() == typeof(string))
                return new LString((string)this.Value);

            if (this.Value.GetType() == typeof(double))
                return new LNumber((double)this.Value);

            if (this.Value.GetType() == typeof(bool))
                return new LBool((bool)this.Value);

            if (this.Value.GetType() == typeof(DateTime))
                return new LDate((DateTime) this.Value);
            */
            return this.Value;
        }
    }
}
