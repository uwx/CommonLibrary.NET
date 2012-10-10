

using System.Collections.Generic;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// A lookup class for types.
    /// </summary>
    public class LTypesLookup
    {
        private static Dictionary<string, LType> _types = new Dictionary<string, LType>();


        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public static void Init()
        {
            Register(LTypes.Array);
            Register(LTypes.Bool);
            Register(LTypes.Date);
            Register(LTypes.Function);
            Register(LTypes.Map);
            Register(LTypes.Null);
            Register(LTypes.Number);
            Register(LTypes.String);
            Register(LTypes.Time);
        }


        /// <summary>
        /// Register the type
        /// </summary>
        /// <param name="type"></param>
        public static void Register(LType type)
        {
            _types[type.FullName] = type;
        }

        
        /// <summary>
        /// Check whether or nor the fulltype name supplied is a basic type
        /// </summary>
        /// <param name="fullName">The full name of the type. e.g. sys.string.</param>
        /// <returns></returns>
        public static bool IsBasicType(string fullName)
        {
            if (!_types.ContainsKey(fullName))
                return false;
            var type = _types[fullName];
            return type.IsBasicType();
        }


        /// <summary>
        /// Gets the type of the fullname supplied.
        /// </summary>
        /// <param name="fullName">The full name of the type. e.g. sys.string.</param>
        /// <returns></returns>
        public static LType GetLType(string fullName)
        {
            if (!_types.ContainsKey(fullName))
                return null;

            return _types[fullName];
        }
    }
}
