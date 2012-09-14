using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

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
            //if (this.Value.GetType() == typeof(bool))
            //    return new LBool((bool)this.Value);

            //if (this.Value.GetType() == typeof(double))
            //    return new LNumber((bool)this.Value);

            //if (this.Value.GetType() == typeof(bool))
            //    return new LBool((bool)this.Value);
            return this.Value;
        }
    }
}
