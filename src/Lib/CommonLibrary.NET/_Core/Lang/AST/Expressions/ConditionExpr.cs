using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Types;
// </lang:using>

namespace ComLib.Lang.AST
{
    /// <summary>
    /// Condition expression less, less than equal, more, more than equal etc.
    /// </summary>
    public class ConditionExpr : Expr
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="left">Left hand expression</param>
        /// <param name="op">Operator</param>
        /// <param name="right">Right expression</param>
        public ConditionExpr(Expr left, Operator op, Expr right)
        {
            Left = left;
            Right = right;
            Op = op;
        }


        /// <summary>
        /// Left hand expression
        /// </summary>
        public Expr Left;


        /// <summary>
        /// Operator > >= == != less less than
        /// </summary>
        public Operator Op;


        /// <summary>
        /// Right hand expression
        /// </summary>
        public Expr Right;


        /// <summary>
        /// Evaluate > >= != == less less than
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            // Validate
            if (Op != Operator.And && Op != Operator.Or)
                throw new ArgumentException("Only && || supported");

            var result = false;
            var left = Left.EvaluateAs<LBool>();
            var right = Right.EvaluateAs<LBool>();

            if (Op == Operator.Or)
            {
                result = left.Value || right.Value;
            }
            else if (Op == Operator.And)
            {
                result = left.Value && right.Value;
            }
            return new LBool(result);
        }
    }    
}
