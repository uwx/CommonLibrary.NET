using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array datatype
    /// </summary>
    public class LJSMapMethods : LTypeMethods
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LJSMapMethods()
        {
            DataType = new LMapType();
            AddProperty(true, true,     "length",       "Length",           typeof(double),     "Sets or returns the number of elements in an array");
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied member.
        /// </summary>
        /// <param name="type">The data type to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        public override bool HasMember(LTypeValue type, string memberName)
        {
            return HasProperty(type, memberName);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied method.
        /// </summary>
        /// <param name="type">The data type to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        public override bool HasMethod(LTypeValue type, string methodName)
        {
            return HasProperty(type, methodName);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied property.
        /// </summary>
        /// <param name="type">The data type to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        public override bool HasProperty(LTypeValue target, string propertyName)
        {
            var map = target.Result as IDictionary;
            if (map == null) return false;

            return map.Contains(propertyName);
        }




        #region Javascript API methods
        /// <summary>
        /// Lenght of the array.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        public int Length(LTypeValue target)
        {
            var map = target.Result as IDictionary;
            return map.Count;
        }
        #endregion



        #region Helpers
        /// <summary>
        /// Get the value of a property.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="key">The name of the map key/property to get</param>
        /// <returns></returns>
        public T IndexerGetAs<T>(LTypeValue target, string name)
        {
            object result = IndexerGet(target, name);
            T returnVal = (T)result;
            return returnVal;
        }


        /// <summary>
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="key">The name of the map key/property to get</param>
        /// <returns></returns>
        public object IndexerGet(LTypeValue target, string key)
        {
            if (target == null ) return LNullType.NullResult;
            var map = target.Result as IDictionary;
            if (map == null || map.Count == 0) return LNullType.NullResult;
            if (string.IsNullOrEmpty(key)) throw new IndexOutOfRangeException("Property does not exist : " + key);
            return map[key];
        }


        /// <summary>
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="key">The name of the map key/property to set</param>
        /// <param name="value">The vlaue to set</param>
        /// <returns></returns>
        public void IndexerSet(LTypeValue target, string key, object value)
        {
            if (target == null ) return;
            var map = target.Result as IDictionary;
            if (map == null) return;
            if (string.IsNullOrEmpty(key)) throw new IndexOutOfRangeException("Property does not exist : " + key);
            map[key] = value;
        }
        #endregion
    }
}
