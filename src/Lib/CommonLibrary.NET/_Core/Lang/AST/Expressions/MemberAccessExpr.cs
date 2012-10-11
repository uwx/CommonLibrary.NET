using System;
using System.Collections;
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
            MemberAccess memberAccess = GetMemberAccess();
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
                var ltypeval = new LTypeValue(lmap, LTypes.Map);
                if(!methods.HasProperty(ltypeval, MemberName))
                    throw this.BuildRunTimeException("Property does not exist : '" + MemberName + "'"); 
                
                return methods.ExecuteMethod(ltypeval, "Get_" + MemberName, null);
            }           
            return memberAccess;
        }


        private MemberAccess GetMemberAccess()
        {
             bool isVariableExp = VariableExp is VariableExpr;
            string variableName = isVariableExp ? ((VariableExpr)VariableExp).Name : string.Empty;
            
            // CASE 1: External function call "user.create"
            if (isVariableExp && IsExternalFunctionCall(variableName))
                return new MemberAccess(MemberMode.FunctionExternal) { Name = variableName, MemberName = MemberName };
            
            // CASE 2. Static method call: "Person.Create" 
            if(isVariableExp )
            { 
                var result = IsMemberStaticAccess(variableName);
                if (result.Success)
                    return GetStaticMemberAccess(result.Item as Type);
            }

            var obj = VariableExp.Evaluate() as LTypeValue;
            var type = obj.Type;

            // Case 3: LDateType, LArrayType, String,
            bool isCoreType = obj.Type.IsBasicType();
            if ( isCoreType )
            {
                // Get the methods implementation LTypeMethods for this basic type 
                // e.g. string,  date,  time,  array , map
                // e.g. LStringType  LDateType, LTimeType, LArrayType, LMapType
                var typeMethods = Ctx.Methods.Get(type);
                var hostType = LangTypeHelper.ConvertToHostLangType(type);

                // 1. Check that the member exists.
                if (!typeMethods.HasMember(null, MemberName))
                    throw BuildRunTimeException("Property or Member : " + MemberName + " does not exist");

                // 2. Property ?
                if (typeMethods.HasProperty(null, MemberName))
                    return new MemberAccess(MemberMode.CustObjMethodInstance) { Name = type.Name, DataType = hostType, Instance = obj, MemberName = MemberName };

                // 3. Method ?
                if (typeMethods.HasMethod(null, MemberName))
                    return new MemberAccess(MemberMode.CustObjMethodInstance) { Name = type.Name, DataType = hostType, Instance = obj, MemberName = MemberName };
            }

            // CASE 4: Custom object type
            var member = GetInstanceMemberAccess(obj);
            return member;
        }
    }    
}
