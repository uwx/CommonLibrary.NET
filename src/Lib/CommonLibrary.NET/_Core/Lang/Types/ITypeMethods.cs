using System;

using ComLib.Lang.Core;

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
        LType DataType { get; }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied member.
        /// </summary>
        /// <param name="type">The data type to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        bool HasMember(LTypeValue type, string memberName);


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied method.
        /// </summary>
        /// <param name="type">The data type to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        bool HasMethod(LTypeValue type, string methodName);


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied property.
        /// </summary>
        /// <param name="type">The data type to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        bool HasProperty(LTypeValue type, string propertyName);

 
        /// <summary>
        /// Validates the method call.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        BoolMsgObj ValidateCall(LTypeValue type, string methodName, object[] parameters);


        /// <summary>
        /// Executes the method supplied on the the type.
        /// </summary>
        /// <param name="type">The language type</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">The parameters to the method.</param>
        /// <returns></returns>
        object ExecuteMethod(LTypeValue type, string methodName, object[] parameters);
    }
}
