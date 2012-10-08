

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
        /// Single instance of the array type for resusability
        /// </summary>
        public static LType ArrayType = new LArray();


        /// <summary>
        /// Single instance of the bool type for resusability
        /// </summary>
        public static LType BoolType = new LBool();


        /// <summary>
        /// Single instance of the date type for resusability
        /// </summary>
        public static LType DateType = new LDate();


        /// <summary>
        /// Single instance of the function type for resusability
        /// </summary>
        public static LType FunctionType = new LFunction();


        /// <summary>
        /// Single instanceo of the Map type for resusability
        /// </summary>
        public static LType MapType = new LMap();


        /// <summary>
        /// Single instance of the Null type for resusability
        /// </summary>
        public static LType NullType = new LNull();


        /// <summary>
        /// Single instance of the number type for resusability
        /// </summary>
        public static LType NumberType = new LNumber();


        /// <summary>
        /// Single instance of the string type for reusability
        /// </summary>
        public static LType StringType = new LString();


        /// <summary>
        /// Signle instance of the time type for reusability
        /// </summary>
        public static LType TimeType = new LTime();


        /// <summary>
        /// Object type.
        /// </summary>
        public static LType ObjectType = new LObject();


        /// <summary>
        /// Initialize with defaults
        /// </summary>
        public static void Init()
        {
            Register(ArrayType);
            Register(BoolType);
            Register(DateType);
            Register(FunctionType);
            Register(MapType);
            Register(NullType);
            Register(NumberType);
            Register(StringType);
            Register(TimeType);
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
