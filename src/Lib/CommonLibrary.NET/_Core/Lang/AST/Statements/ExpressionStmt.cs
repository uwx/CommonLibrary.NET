using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace ComLib.Lang
{
    /// <summary>
    /// A statement that actually executes an expression.
    /// Used in cases where an expression can be both an expression and run as a statement.
    /// </summary>
    public class ExpressionStmt : Stmt
    {
        private Expr _exp;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="exp"></param>
        public ExpressionStmt(Expr exp)
        {
            _exp = exp;
        }


        /// <summary>
        /// Execute the statement.
        /// </summary>
        public override void DoExecute()
        {
            if (_exp != null)
                _exp.Evaluate();
        }
    }
}
