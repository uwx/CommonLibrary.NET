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
            DataType = typeof(LMap);
            AddProperty(true, true,     "length",       "Length",           typeof(double),     "Sets or returns the number of elements in an array");
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied member.
        /// </summary>
        /// <param name="type">The data type to check for the member</param>
        /// <param name="memberName">The name of the member to check for.</param>
        /// <returns></returns>
        public virtual bool HasMember(LObject type, string memberName)
        {
            return HasProperty(type, memberName);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied method.
        /// </summary>
        /// <param name="type">The data type to check for the method</param>
        /// <param name="methodName">The name of the method to check for.</param>
        /// <returns></returns>
        public virtual bool HasMethod(LObject type, string methodName)
        {
            return HasProperty(type, methodName);
        }


        /// <summary>
        /// Whether or not the associted type of this methods class has the supplied property.
        /// </summary>
        /// <param name="type">The data type to check for the property</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns></returns>
        public virtual bool HasProperty(LObject type, string propertyName)
        {            
            var target = type as LMap;
            if (target == null) return false;

            return target.Raw.ContainsKey(propertyName);
        }




        #region Javascript API methods
        /// <summary>
        /// Lenght of the array.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        public int Length(LMap target)
        {
            return target.Raw.Count;
        }
        #endregion



        #region Helpers
        /// <summary>
        /// Get the value of a property.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="key">The name of the map key/property to get</param>
        /// <returns></returns>
        public T IndexerGetAs<T>(LMap target, string name)
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
        public object IndexerGet(LMap target, string key)
        {
            if (target == null || target.Raw == null || target.Raw.Count == 0) return LNull.Instance;
            var map = target.Raw;
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
        public void IndexerSet(LMap target, string key, object value)
        {
            if (target == null || target.Raw == null || target.Raw.Count == 0) return;
            var list = target.Raw;
            var map = target.Raw;
            if (string.IsNullOrEmpty(key)) throw new IndexOutOfRangeException("Property does not exist : " + key);
            map[key] = value;
        }
        #endregion
    }
}
