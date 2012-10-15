using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Helpers;
using ComLib.Lang.Types;
// </lang:using>

namespace ComLib.Lang.AST
{   
    /// <summary>
    /// Represents the member access
    /// </summary>
    public class MemberAccess
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="mode"></param>
        public MemberAccess(MemberMode mode)
        {
            Mode = mode;
        }


        /// <summary>
        /// The mode of access
        /// </summary>
        public MemberMode Mode;


        /// <summary>
        /// The name of the member.
        /// </summary>
        public string Name;


        /// <summary>
        /// The name of the member being accessed
        /// </summary>
        public string MemberName;


        /// <summary>
        /// Instance of the member
        /// </summary>
        public object Instance;


        /// <summary>
        /// The datatype of the member being accessed.
        /// </summary>
        public Type DataType;


        /// <summary>
        /// The type if the member access is on a built in a fluentscript type.
        /// </summary>
        public LType Type;


        /// <summary>
        /// The method represetning the member.
        /// </summary>
        public MethodInfo Method;


        /// <summary>
        /// The property representing the member.
        /// </summary>
        public PropertyInfo Property;


        /// <summary>
        /// The full member name.
        /// </summary>
        public string FullMemberName
        {
            get { return Name + "." + MemberName; }
        }


        /// <summary>
        /// Whether or not this represents an external or internal function call.
        /// </summary>
        /// <returns></returns>
        public bool IsInternalExternalFunctionCall()
        {
            return Mode == MemberMode.FunctionExternal || Mode == MemberMode.FunctionScript;
        }
    }


    /// <summary>
    /// Member access expressions for "." property or "." method.
    /// </summary>
    public class MemberAccessExpr : MemberExpr
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="variableExp">The variable expression to use instead of passing in name of variable.</param>
        /// <param name="memberName">Name of member, this could be a property or a method name.</param>
        /// <param name="isAssignment">Whether or not this is part of an assigment</param>
        public MemberAccessExpr(Expr variableExp, string memberName, bool isAssignment) : base(variableExp, memberName)
        {
            this.IsAssignment = isAssignment;
        }


        /// <summary>
        /// Whether or not this member access is part of an assignment.
        /// </summary>
        public bool IsAssignment;


        /// <summary>
        /// Either external function or member name.
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            var memberAccess = MemberHelper.GetMemberAccess(this, this.Ctx, this.VariableExp, this.MemberName);
            if (IsAssignment)
                return memberAccess;

            // Get property value ( either static or instance )
            if (memberAccess.Property != null)
            {
                return memberAccess.Property.GetValue(memberAccess.Instance, null);
            }
            if (memberAccess.DataType == typeof(IDictionary))
            {
                // 2. Non-Assignment - Validate property exists.
                var lmap = memberAccess.Instance;
                var methods = this.Ctx.Methods.Get(LTypes.Map);
                var ltypeval = new LMap((IDictionary<string, object>)lmap);
                if(!methods.HasProperty(ltypeval, MemberName))
                    throw this.BuildRunTimeException("Property does not exist : '" + MemberName + "'"); 
                
                return methods.ExecuteMethod(ltypeval, "Get_" + MemberName, null);
            }           
            return memberAccess;
        }
    }    
}
