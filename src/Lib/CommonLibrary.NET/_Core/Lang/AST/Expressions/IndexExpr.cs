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
            
            // 1. Check for null
            if (ndxVal == null)
                throw BuildRunTimeException("Unable to index with null value");

            if(!this.IsAssignment)
            {
                // Is the index value a number ? Indicates that the object is an array.
                var ndxNum = (LObject)ndxVal;
                if (ndxNum.Type == LTypes.Number)
                {
                    var ndx = ((LNumber)ndxNum).Value;
                    result = GetArrayValue(Convert.ToInt32(ndx));
                    return result;    
                }
                // If the index is a string. Then object is a map/dictionary.
                else if (ndxVal is string)
                {
                    string memberName = ndxVal as string;
                    var methods = this.Ctx.Methods.Get(LTypes.Map);
                    var ltypeval = new LMap((IDictionary<string, object>)ListObject);
                    if (!methods.HasProperty(ltypeval, memberName))
                        throw this.BuildRunTimeException("Property does not exist : '" + memberName + "'");
                    return methods.ExecuteMethod(ltypeval, "Get_" + memberName, null);
                }       
            }
            if (ndxVal is int || ndxVal is double)
            {
                return new Tuple<object, int>(ListObject, Convert.ToInt32(ndxVal));
            }
            return new Tuple<LMapType, string>((LMapType)ListObject, (string)ndxVal);          
        }


        private object GetArrayValue(int ndx)
        {
            MethodInfo method = null;
            object result = null;
            var list = (LArray)ListObject;
            
            // 1. Array
            if (list.Type == LTypes.Array)
            {
                method = ListObject.GetType().GetMethod("GetValue", new Type[] { typeof(int) });
            }
            // 2. LArrayType
            else if (ListObject is LArrayType)
            {
                method = ListObject.GetType().GetMethod("GetByIndex");
            }
            // 3. IList
            else
            {
                method = ListObject.GetType().GetMethod("get_Item");
            }
            // Getting value?                
            try
            {
                result = method.Invoke(ListObject, new object[] { ndx });
            }
            catch (Exception)
            {
                throw BuildRunTimeException("Access of list item at position " + ndx + " is out of range");
            }
            return result;
        }
    }    
}
