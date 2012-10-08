using System;
using System.Collections.Generic;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.AST;
using ComLib.Lang.Parsing;
using ComLib.Lang.Types;
// </lang:using>


namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class
    /// </summary>
    public class LangHelper
    {
        /// <summary>
        /// Executes the statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="parent"></param>
        public static void Evaluate(List<Expr> statements, AstNode parent)
        {
            if (statements != null && statements.Count > 0)
            {
                foreach (var stmt in statements)
                {
                    stmt.Evaluate();
                }
            }
        }


        /// <summary>
        /// The shunting yard algorithm that processes a postfix list of expressions/operators.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parser"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        public static Expr ProcessShuntingYardList(Context context, Parser parser, List<object> stack)
        {
            int index = 0;
            Expr finalExp = null;

            // Shunting yard algorithm handles POSTFIX operations.
            while (index < stack.Count && stack.Count > 0)
            {
                // Keep moving forward to the first operator * - + / && that is found  
                // This is a postfix algorithm so it works by creating an expression
                // from the last 2 items behind an operator.
                if (!(stack[index] is TokenData))
                {
                    index++;
                    continue;
                }

                // At this point... we hit an operator 
                // So get the last 2 items on the stack ( they have to be expressions )
                // left  is 2 behind current position
                // right is 1 behind current position
                var left = stack[index - 2] as Expr;
                var right = stack[index - 1] as Expr;
                TokenData tdata = stack[index] as TokenData;
                Token top = tdata.Token;
                Operator op = Operators.ToOp(top.Text);
                Expr exp = null;

                if (Operators.IsMath(op))
                    exp = new BinaryExpr(left, op, right);
                else if (Operators.IsConditional(op))
                    exp = new ConditionExpr(left, op, right);
                else if (Operators.IsCompare(op))
                    exp = new CompareExpr(left, op, right);

                exp.Ctx = context;
                parser.SetScriptPosition(exp, tdata);
                stack.RemoveRange(index - 2, 2);
                index = index - 2;
                stack[index] = exp;
                index++;

            }
            finalExp = stack[0] as Expr;
            return finalExp;
        }
    }
}
