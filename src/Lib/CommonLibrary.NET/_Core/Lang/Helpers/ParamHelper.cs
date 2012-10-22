
using System;
using System.Collections.Generic;
using System.Reflection;

using ComLib.Lang.AST;
using ComLib.Lang.Core;
using ComLib.Lang.Types;


namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for function parameters.
    /// </summary>
    public class ParamHelper
    {
        /// <summary>
        /// Whether or not the parametlist of expressions contains a named parameter with the name supplied.
        /// </summary>
        /// <param name="paramListExpressions">List of parameter list expressions.</param>
        /// <param name="paramName">Name of the parameter to search for</param>
        /// <returns></returns>
        public static bool HasNamedParameter(List<Expr> paramListExpressions, string paramName)
        {
            if (paramListExpressions == null || paramListExpressions.Count == 0)
                return false;

            foreach (var paramExpr in paramListExpressions)
                if (paramExpr is NamedParamExpr)
                    if (((NamedParamExpr)paramExpr).Name == paramName)
                        return true;
            return false;
        }


        /// <summary>
        /// Resolve the parameters in the function call.
        /// </summary>
        public static void ResolveParameters(List<Expr> paramListExpressions, List<object> paramList)
        {
            if (paramListExpressions == null || paramListExpressions.Count == 0)
                return;

            paramList.Clear();
            foreach (var exp in paramListExpressions)
            {
                object val = exp.Evaluate();
                paramList.Add(val);
            }
        }


        /// <summary>
        /// Resolve the parameters in the function call.
        /// </summary>
        public static void ResolveParametersToHostLangValues(List<Expr> paramListExpressions, List<object> paramList)
        {
            if (paramListExpressions == null || paramListExpressions.Count == 0)
                return;

            paramList.Clear();
            foreach (var exp in paramListExpressions)
            {
                var val = exp.Evaluate();
                if(val is LObject)
                {
                    var converted = ((LObject)val).GetValue();
                    paramList.Add(converted);
                }
                else
                    paramList.Add(val);
            }
        }
        

        /// <summary>
        /// Resolves the parameter expressions to actual values.
        /// </summary>
        /// <param name="totalParams"></param>
        /// <param name="paramListExpressions"></param>
        /// <param name="paramList"></param>
        /// <param name="indexLookup"></param>
        public static void ResolveParameters(int totalParams, List<Expr> paramListExpressions, List<object> paramList, Func<NamedParamExpr, int> indexLookup)
        {
            if (paramListExpressions == null || paramListExpressions.Count == 0)
                return;

            paramList.Clear();
            bool hasNamedParams = false;
            foreach (var param in paramListExpressions)
            {    
                if (param is NamedParamExpr)
                {
                    hasNamedParams = true;
                    break;
                }
            }

            // If there are no named params. Simply evaluate and return.
            if (!hasNamedParams)
            {
                foreach (var exp in paramListExpressions)
                {
                    object val = exp.Evaluate();
                    paramList.Add(val);
                }

                return;
            }

            // 1. Set all args to null. [null, null, null, null, null]
            for (int ndx = 0; ndx < totalParams; ndx++)
                paramList.Add(null);

            // 2. Now go through each argument and replace the nulls with actual argument values.
            // Each null should be replaced at the correct index.
            // [true, 20.68, new Date(2012, 8, 10), null, 'fluentscript']
            for (int ndx = 0; ndx < paramListExpressions.Count; ndx++)
            {
                var exp = paramListExpressions[ndx];

                // 3. Named arg? Evaluate and put its value into the appropriate index of the args list.           
                if (exp is NamedParamExpr)
                {
                    var namedParam = exp as NamedParamExpr;
                    object val = namedParam.Evaluate();
                    int argIndex = indexLookup(namedParam);
                    paramList[argIndex] = val;
                }
                else
                {
                    // 4. Expect the position of non-named args should be valid.
                    // TODO: Semantic analysis is required here once Lint check feature is added.
                    object val = exp.Evaluate();
                    paramList[ndx] = val;
                }
            }
        }

        /// <summary>
        /// Resolve the parameters in the function call.
        /// </summary>
        public static void ResolveParametersForScriptFunction(FunctionMetaData meta, List<Expr> paramListExpressions, List<object> paramList)
        {
            int totalParams = meta.Arguments == null ? 0 : meta.Arguments.Count;
            ResolveParameters(totalParams, paramListExpressions, paramList,
                namedParam => meta.ArgumentsLookup[namedParam.Name].Index);
        }


        /// <summary>
        /// Resolve the parameters in the function call.
        /// </summary>
        public static void ResolveParametersForMethodCall(MethodInfo method, List<Expr> paramListExpressions, List<object> paramList)
        {
            var parameters = method.GetParameters();
            if (parameters == null || parameters.Length == 0) return;

            // Convert parameters to map.
            var map = System.Linq.Enumerable.ToDictionary(parameters, p => p.Name);
            
            ResolveParameters(parameters.Length, paramListExpressions, paramList,
                namedParam => map[namedParam.Name].Position);
        }

    }
}
