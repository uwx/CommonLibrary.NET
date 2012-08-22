using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;


namespace ComLib.Lang
{    

    /// <summary>
    /// Member access expressions for "." property or "." method.
    /// </summary>
    public class MemberValueExpr : MemberExpr
    {
        private static LDate _dateTypeForMethodCheck = new LDate(null, null);


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="variableExp">The variable expression to use instead of passing in name of variable.</param>
        /// <param name="memberName">Name of member, this could be a property or a method name.</param>
        public MemberValueExpr(Expr variableExp, string memberName) : base(variableExp, memberName)
        {
        }


        /// <summary>
        /// Either external function or member name.
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            MemberAccess memberAccess = GetMemberAccess();
            bool IsAssignment = false;
            if (!IsAssignment)
                return memberAccess;

            // Get property value ( either static or instance )
            if (memberAccess.Property != null)
                return memberAccess.Property.GetValue(memberAccess.Instance, null);

            return null;
        }


        private MemberAccess GetMemberAccess()
        {
            Type type = null;
            string variableName = string.Empty;            

            // CASEE 1: "user.create" -> external or internal script.
            if (VariableExp is VariableExpr )
            {
                var exp = VariableExp as VariableExpr;
                variableName = exp.Name;

                // 1. External function : Log.Error
                if (IsExternalFunctionCall(variableName))
                    return new MemberAccess(MemberMode.FunctionExternal) { Name = variableName, MemberName = MemberName };
                

                // 2. Static method : "Person.Create" -> static method call on custom object?
                var result = IsMemberStaticAccess(variableName);
                if (result.Success)
                    return GetStaticMemberAccess(result.Item as Type);  
            }

            object obj = VariableExp.Evaluate();
            type = obj.GetType();

            // CASE 2: Core type ( Larray, Lmap, string, datetime )
            if (IsCoreType(obj))
            {
                // 1. Map or Array
                if (obj is LMap) 
                {
                    return new MemberAccess(MemberMode.CustObjMethodInstance) 
                    { Name = type.Name, DataType = type, Instance = obj, MemberName = MemberName };
                }
                if (obj is LArray)
                {
                    MemberName = LArray.MapMethod(MemberName);
                    return new MemberAccess(MemberMode.CustObjMethodInstance) 
                    { Name = type.Name, DataType = type, Instance = obj, MemberName = MemberName };
                }
                // Case 2a: string.Method
                if (obj is string)
                {
                    return new MemberAccess(MemberMode.CustObjMethodInstance) 
                    { Name = type.Name, DataType = type, Instance = obj, MemberName = MemberName };
                }
                // Case 2b: date.Method
                if (obj is DateTime)
                {
                    return new MemberAccess(MemberMode.CustObjMethodInstance) 
                    { Name = type.Name, DataType = type, Instance = obj, MemberName = MemberName };
                }
            }

            // CASE 3: Custom object type
            var member = GetInstanceMemberAccess(obj);
            return member;
        }
    }    
}
