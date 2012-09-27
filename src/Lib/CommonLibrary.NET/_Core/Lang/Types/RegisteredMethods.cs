using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ComLib.Lang.Types
{

    /// <summary>
    /// Support the mapping of methods to dataypes.
    /// </summary>
    public class RegisteredMethods
    {        
        private IDictionary<Type, ITypeMethods> _typeToMethods = new Dictionary<Type, ITypeMethods>();


        /// <summary>
        /// Register methods on a specific type.
        /// </summary>
        /// <param name="type">The datatype for which the methods implementation are applicable</param>
        /// <param name="methods">The method implementations</param>
        public void Register(Type type, ITypeMethods methods)
        {
            _typeToMethods[type] = methods;
        }


        /// <summary>
        /// Registers methods on a specific type if no existing methods implementation are already
        /// registered for the type.
        /// </summary>
        /// <param name="type">The datatype for which the methods implementation are applicable</param>
        /// <param name="methods">The method implementations</param>
        public void RegisterIfNotPresent(Type type, ITypeMethods methods)
        {
            if (!_typeToMethods.ContainsKey(type))
                _typeToMethods[type] = methods;
        }


        /// <summary>
        /// Whether or not there are methods for the supplied type.
        /// </summary>
        /// <param name="type"></param>
        public bool Contains(Type type)
        {
            return _typeToMethods.ContainsKey(type);
        }


        /// <summary>
        /// Get the methods implementation for the supplied type.
        /// </summary>
        /// <param name="type"></param>
        public ITypeMethods Get(Type type)
        {
            return _typeToMethods[type];
        }
    }
}
