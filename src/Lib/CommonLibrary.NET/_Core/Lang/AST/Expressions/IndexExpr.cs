using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Types;
using ComLib.Lang.Helpers;
// </lang:using>

namespace ComLib.Lang.AST
{
    /// <summary>
    /// Member access expressions for "." property or "." method.
    /// </summary>
    public class IndexExpr : Expr
    {
       
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="variableExp">The variable expression to use instead of passing in name of variable.</param>
        /// <param name="indexExp">The expression representing the index value to get</param>
        /// <param name="isAssignment">Whether or not this is part of an assigment</param>
        public IndexExpr(Expr variableExp, Expr indexExp, bool isAssignment)
        {
            InitBoundary(true, "]");
            VariableExp = variableExp;
            IndexExp = indexExp;
            IsAssignment = isAssignment;
        }


        /// <summary>
        /// Expression representing the index
        /// </summary>
        public Expr IndexExp;


        /// <summary>
        /// The variable expression representing the list.
        /// </summary>
        public Expr VariableExp;


        /// <summary>
        /// The object to get the index value from. Used if ObjectName is null or empty.
        /// </summary>
        public object ListObject;


        /// <summary>
        /// Whether or not this member access is part of an assignment.
        /// </summary>
        public bool IsAssignment;


        /// <summary>
        /// Evaluate object[index]
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            object result = null;
            var ndxVal = IndexExp.Evaluate();
            
            // Either get from scope or from exp.
            if (VariableExp is VariableExpr)
                this.ListObject = Ctx.Memory.Get<object>(((VariableExpr)VariableExp).Name);
            else
                this.ListObject = VariableExp.Evaluate();

            var lobj = LObjects.Empty;
            if (this.ListObject != null)
                lobj = (LObject)this.ListObject;

            // Check for null
            if (ndxVal == null)
                throw BuildRunTimeException("Unable to index with null value");

            // 1. Access
            if(!this.IsAssignment)
            {
                result = EvalHelper.AccessIndex(this.Ctx.Methods, this, lobj, (LObject)ndxVal);
                return result;
            }
            // 2. Assignment
            if (ndxVal is int || ndxVal is double)
            {
                return new Tuple<object, int>(ListObject, Convert.ToInt32(ndxVal));
            }
            return new Tuple<LMapType, string>((LMapType)ListObject, (string)ndxVal);
        }
    }    
}
