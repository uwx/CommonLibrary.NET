using System;
using System.Collections.Generic;
using System.Reflection;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.AST;
using ComLib.Lang.Types;
using ComLib.Lang.Parsing;
// </lang:using>

namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for calling functions in the script.
    /// </summary>
    public class FunctionHelper
    {   
        /// <summary>
        /// Whether or not the name/member combination supplied is a script level function or an external C# function
        /// </summary>
        /// <param name="ctx">Context of script</param>
        /// <param name="name">Object name "Log"</param>
        /// <param name="member">Member name "Info" as in "Log.Info"</param>
        /// <returns></returns>
        public static bool IsInternalOrExternalFunction(Context ctx, string name, string member)
        {
            string fullName = name;
            if (!string.IsNullOrEmpty(member))
                fullName += "." + member;

            // Case 1: getuser() script function
            if (ctx.Functions.Contains(fullName) || ctx.ExternalFunctions.Contains(fullName))
                return true;

            return false;
        }        


        /// <summary>
        /// Call internal/external script.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static object FunctionCall(Context ctx, string name, FunctionCallExpr exp)
        {
            // Case 1: Custom C# function blog.create blog.*
            if (ctx.ExternalFunctions.Contains(name))
                return ctx.ExternalFunctions.Call(name, exp);

            // Case 2: Script functions "createUser('john');" 
            return ctx.Functions.Call(exp);
        }


        /// <summary>
        /// Execute a member call.
        /// </summary>
        /// <param name="ctx">The context of the script</param>
        /// <param name="type">The type of the object</param>
        /// <param name="obj">The object to call the method on</param>
        /// <param name="varname">The name of the variable</param>
        /// <param name="memberName">The name of the member/method to call</param>
        /// <param name="methodInfo">The methodinfo(not needed for built in types )</param>
        /// <param name="paramListExpressions">The expressions to resolve as parameters</param>
        /// <param name="paramList">The list of parameters.</param>
        /// <returns></returns>
        public static object MemberCall(Context ctx, Type type, object obj, string varname, string memberName, MethodInfo methodInfo, List<Expr> paramListExpressions, List<object> paramList)
        {
            // 1. Resolve the parameters.
            if(methodInfo == null)
                ParamHelper.ResolveParameters(paramListExpressions, paramList);

            object result = null;
            if (type == null && obj != null)
                type = obj.GetType();

            // 1. DateTime
            if (type == typeof(DateTime))
            {
                var methods = ctx.Methods.Get(LTypes.Date);
                var lobj = new LDate((DateTime)obj);
                result = methods.ExecuteMethod(lobj, memberName, paramList.ToArray());
            }
            // 2. String
            else if (type == typeof(string))
            {
                var methods = ctx.Methods.Get(LTypes.String);
                var lobj = new LString((string)obj);
                result = methods.ExecuteMethod(lobj, memberName, paramList.ToArray());
            }
            // 3. Method info supplied
            else if (methodInfo != null)
            {
                result = MethodCall(ctx, obj, type, methodInfo, paramListExpressions, paramList, true);
            }
            else
            {
                methodInfo = type.GetMethod(memberName);
                if (methodInfo != null)
                    result = methodInfo.Invoke(obj, paramList.ToArray());
                else
                {
                    var prop = type.GetProperty(memberName);
                    if(prop != null)
                        result = prop.GetValue(obj, null);
                }
            }
            return result;
        }


        /// <summary>
        /// Prints to the console.
        /// </summary>
        /// /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        /// <param name="printline">Whether to print with line or no line</param>
        public static string Print(LangSettings settings, FunctionCallExpr exp, bool printline)
        {
            if (!settings.EnablePrinting) return string.Empty;

            string message = BuildMessage(exp.ParamList);
            if (printline) Console.WriteLine(message);
            else Console.Write(message);
            return message;
        }


        /// <summary>
        /// Logs severity to console.
        /// </summary>
        /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        public static string Log(LangSettings settings, FunctionCallExpr exp)
        {
            if (!settings.EnableLogging) return string.Empty;

            string severity = exp.Name.Substring(exp.Name.IndexOf(".") + 1);
            string message = BuildMessage(exp.ParamList);
            Console.WriteLine(severity.ToUpper() + " : " + message);
            return message;
        }


        /// <summary>
        /// Builds a single message from multiple arguments
        /// If there are 2 or more arguments, the 1st is a format, then rest are the args to the format.
        /// </summary>
        /// <param name="paramList">The list of parameters</param>
        /// <returns></returns>
        public static string BuildMessage(List<object> paramList)
        {
            string val = string.Empty;
            bool hasFormat = false;
            string format = string.Empty;
            if (paramList != null && paramList.Count > 0)
            {
                // Check for 2 arguments which reflects formatting the printing.
                hasFormat = paramList.Count > 1;
                if (hasFormat)
                {
                    format = paramList[0].ToString();
                    var args = paramList.GetRange(1,paramList.Count - 1);
                    val = string.Format(format, args.ToArray());
                }
                else
                    val = paramList[0].ToString();
            }
            return val;
        } 


        /// <summary>
        /// Dynamically invokes a method call.
        /// </summary>
        /// <param name="ctx">Context of the script</param>
        /// <param name="obj">Instance of the object for which the method call is being applied.</param>
        /// <param name="datatype">The datatype of the object.</param>
        /// <param name="methodInfo">The method to call.</param>
        /// <param name="paramListExpressions">List of expressions representing parameters for the method call</param>
        /// <param name="paramList">The list of values(evaluated from expressions) to call.</param>
        /// <param name="resolveParams">Whether or not to resolve the parameters from expressions to values.</param>
        /// <returns></returns>
        private static object MethodCall(Context ctx, object obj, Type datatype, MethodInfo methodInfo, List<Expr> paramListExpressions, List<object> paramList, bool resolveParams = true)
        {
            // 1. Convert language expressions to values.
            if (resolveParams) ParamHelper.ResolveParametersForMethodCall(methodInfo, paramListExpressions, paramList);

            // 2. Convert internal language types to c# code method types.
            LangTypeHelper.ConvertArgs(paramList, methodInfo);

            // 3. Now get args as an array for method calling.
            object[] args = paramList.ToArray();

            // 4. Handle  params object[];
            if (methodInfo.GetParameters().Length == 1)
            {
                if (methodInfo.GetParameters()[0].ParameterType == typeof(object[]))
                    args = new object[] { args };
            }
            object result = methodInfo.Invoke(obj, args);
            return result;
        }
    }
}
