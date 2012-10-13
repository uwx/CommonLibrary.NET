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
        /// Whether or not the associted obj of this methods class has the supplied member.
        /// </summary>
        /// <param name="obj">The data obj to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        bool HasMember(LObject obj, string memberName);


        /// <summary>
        /// Whether or not the associted obj of this methods class has the supplied method.
        /// </summary>
        /// <param name="obj">The data obj to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        bool HasMethod(LObject obj, string methodName);


        /// <summary>
        /// Whether or not the associted obj of this methods class has the supplied property.
        /// </summary>
        /// <param name="obj">The data obj to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        bool HasProperty(LObject obj, string propertyName);

 
        /// <summary>
        /// Validates the method call.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        BoolMsgObj ValidateCall(LObject obj, string methodName, object[] parameters);


        /// <summary>
        /// Executes the method supplied on the the obj.
        /// </summary>
        /// <param name="obj">The language obj</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">The parameters to the method.</param>
        /// <returns></returns>
        object ExecuteMethod(LObject obj, string methodName, object[] parameters);


        /// <summary>
        /// Set a value by the index.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ndx"></param>
        object GetByNumericIndex(LObject obj, int index);


        /// <summary>
        /// Set a value by the index.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        object GetByStringMember(LObject obj, string member);
        
        
        /// <summary>
        /// Set a value by the index.
        /// </summary>
        /// <param name="obj">The object whose index value is being set.</param>
        /// <param name="index">The index position to set the value</param>
        /// <param name="val">The value to set at the index</param>
        void SetByNumericIndex(LObject obj, int index, LObject val);


        /// <summary>
        /// Set a value by the index.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ndx"></param>
        void SetByStringMember(LObject obj, string member, LObject val);
    }
}
