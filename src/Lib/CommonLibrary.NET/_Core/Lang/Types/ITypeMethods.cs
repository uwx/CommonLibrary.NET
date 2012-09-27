using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Interface for supporting different methods on a datatype.
    /// </summary>
    public interface ITypeMethods
    {
        /// <summary>
        /// The datatype this methods class supports.
        /// </summary>
        Type DataType { get; }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied member.
        /// </summary>
        /// <param name="type">The data type to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        bool HasMember(LObject type, string memberName);


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied method.
        /// </summary>
        /// <param name="type">The data type to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        bool HasMethod(LObject type, string methodName);


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied property.
        /// </summary>
        /// <param name="type">The data type to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        bool HasProperty(LObject type, string propertyName);

 
        /// <summary>
        /// Validates the method call.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ComLib.Lang.Core.BoolMsgObj ValidateCall(LObject type, string methodName, object[] parameters);


        /// <summary>
        /// Executes the method supplied on the the type.
        /// </summary>
        /// <param name="type">The language type</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">The parameters to the method.</param>
        /// <returns></returns>
        object ExecuteMethod(LObject type, string methodName, object[] parameters);
    }
}
