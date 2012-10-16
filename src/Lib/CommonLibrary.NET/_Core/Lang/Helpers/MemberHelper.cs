using System;
using System.Reflection;
using ComLib.Lang.Core;
using ComLib.Lang.Parsing;
using ComLib.Lang.Types;
using ComLib.Lang.AST;


namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for member access on internal fluentscript types and external c# types.
    /// </summary>
    public class MemberHelper
    {
        /// <summary>
        /// Gets a member access object representing the a member access.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="ctx"></param>
        /// <param name="varExp"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static MemberAccess GetMemberAccess(AstNode node, Context ctx, Expr varExp, string memberName)
        {
            bool isVariableExp = varExp is VariableExpr;
            string variableName = isVariableExp ? ((VariableExpr) varExp).Name : string.Empty;

            // CASE 1: External function call "user.create"
            if (isVariableExp && MemberHelper.IsExternalFunctionCall(ctx.ExternalFunctions, variableName, memberName))
                return new MemberAccess(MemberMode.FunctionExternal) {Name = variableName, MemberName = memberName};

            // CASE 2. Static method call: "Person.Create" 
            if (isVariableExp)
            {
                var result = MemberHelper.IsExternalTypeName(ctx.Memory, ctx.Types, variableName);
                if (result.Success)
                    return MemberHelper.GetExternalTypeMember(node, (Type) result.Item, variableName, null, memberName, true);
            }

            var obj = varExp.Evaluate() as LObject;
            var type = obj.Type;

            // Case 3: Method / Property on FluentScript type
            bool isCoreType = obj.Type.IsBasicType();
            if (isCoreType)
            {
                var result = MemberHelper.GetLangBasicTypeMember(node, ctx.Methods, obj, memberName);
                return result;
            }

            // CASE 4: Method / Property on External/Host language type (C#)
            var member = MemberHelper.GetExternalTypeMember(node, null, variableName, null, memberName, true);
            return member;
        }


        /// <summary>
        /// Gets a see MemberAccess object that represents the instance, member name and other information on the member access expression.
        /// </summary>
        /// <param name="node">The Ast node associated with the member access operation</param>
        /// <param name="methods">The collection of registered methods on various types</param>
        /// <param name="obj">The object on which the member access is being performed.</param>
        /// <param name="memberName">The name of the member to get.</param>
        /// <returns></returns>
        public static MemberAccess GetLangBasicTypeMember(AstNode node, RegisteredMethods methods, LObject obj, string memberName)
        {
            var type = obj.Type;
            
            // Get the methods implementation LTypeMethods for this basic type 
            // e.g. string,  date,  time,  array , map
            // e.g. LStringType  LDateType, LTimeType, LArrayType, LMapType
            var typeMethods = methods.Get(type);

            // 1. Check that the member exists.
            if (!typeMethods.HasMember(null, memberName))
                throw MemberHelper.BuildRunTimeException(node, "Property or Member : " + memberName + " does not exist");

            // 2. It's either a Property or method
            var isProp = typeMethods.HasProperty(obj, memberName);
            var mode = isProp ? MemberMode.PropertyMember : MemberMode.MethodMember;
            var maccess = new MemberAccess(mode);
            maccess.Name = type.Name;
            maccess.Instance = obj;
            maccess.MemberName = memberName;
            maccess.Type = type;
            return maccess;
        }


        /// <summary>
        /// Gets a see MemberAccess object that represents the instance, member name and other information on the member access expression.
        /// </summary>
        /// <param name="node">The Ast node associated with the member access operation</param>
        /// <param name="type">The data type of the external object c#.</param>
        /// <param name="varName">The name associated with the object. e.g. user.FirstName, "user".</param>
        /// <param name="obj">The object on which the member access is being performed.</param>
        /// <param name="memberName">The name of the member to get.</param>
        /// <param name="isStatic">Whether or not this is a static access.</param>
        /// <returns></returns>
        public static MemberAccess GetExternalTypeMember(AstNode node, Type type, string varName, object obj, string memberName, bool isStatic)
        {
            // 1. Get all the members on the type that match the name.
            var members = type.GetMember(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.IgnoreCase);

            // 2. Check that there were members with matching name.
            if (members == null || members.Length == 0)
                throw BuildRunTimeException(node, "Property does not exist : '" + memberName + "' ");

            // 3. Get the first member that matches the memberName.
            var matchingMember = members[0];
            var memberNameCaseIgnorant = matchingMember.Name;
            var mode = isStatic ? MemberMode.CustObjMethodStatic : MemberMode.CustObjMethodInstance;

            // 4. Store information about the member. instance, type, membername.
            var member = new MemberAccess(mode);
            member.Name = isStatic ? type.Name : varName;
            member.DataType = type;
            member.Instance = obj;
            member.MemberName = memberName;

            // 1. Property.
            if (matchingMember.MemberType == MemberTypes.Property)
                member.Property = type.GetProperty(memberNameCaseIgnorant);

            // 2. Method
            else if (matchingMember.MemberType == MemberTypes.Method)
                member.Method = type.GetMethod(matchingMember.Name);

            return member;
        }


        /// <summary>
        /// Whether or not this variable + member name maps to an external function call.
        /// Note: In fluentscript you can setup "Log.*" and allow all method calls to "Log" to map to that external call.
        /// </summary>
        /// <param name="funcs">The collection of external functions.</param>
        /// <param name="varName">The name of the external object e.g. "Log" as in "Log.Error"</param>
        /// <param name="memberName">The name of the method e.g. "Error" as in "Log.Error"</param>
        /// <returns></returns>
        public static bool IsExternalFunctionCall(ExternalFunctions funcs, string varName, string memberName)
        {
            string funcName = varName + "." + memberName;
            if (funcs.Contains(funcName))
                return true;
            return false;
        }


        /// <summary>
        /// Whether or not the variable name provided is an external host language( C# ) type name.
        /// </summary>
        /// <param name="memory">The memory space of the runtime.</param>
        /// <param name="types">The collection of registered external types.</param>
        /// <param name="varName">The name associated with the member access.</param>
        /// <returns></returns>
        public static BoolMsgObj IsExternalTypeName(Memory memory, RegisteredTypes types, string varName)
        {
            Type type = null;
            var isStatic = false;
            
            // 1. Class name : "Person" as in "Person.Create" -> so definitely a static method call on custom object.
            if (types.Contains(varName))
            {
                type = types.Get(varName);
            }
            // 2. Case insensitive Class name: "person" as in "Person.Create" but "person" variable doesn't exist.
            else if (!memory.Contains(varName))
            {
                // Only do this check for "user" -> "User" class / static method.
                var first = Char.ToUpper(varName[0]);
                var name = first + varName.Substring(1);
                if (types.Contains(name))
                {
                    type = types.Get(name);
                }
            }
            if (type != null) isStatic = true;
            return new BoolMsgObj(type, isStatic, string.Empty);
        }


        /// <summary>
        /// Build a language exception due to the current token being invalid.
        /// </summary>
        /// <returns></returns>
        public static LangException BuildRunTimeException(AstNode node, string message)
        {
            return new LangException("Runtime Error", message, node.Ref.ScriptName, node.Ref.Line, node.Ref.CharPos);
        }
    }
}
