

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Class to reference instance of the basic types
    /// </summary>
    public class LTypes
    {
        /// <summary>
        /// Single instance of the array type for resusability
        /// </summary>
        public static LType Array = new LArray();


        /// <summary>
        /// Single instance of the bool type for resusability
        /// </summary>
        public static LType Bool = new LBool();


        /// <summary>
        /// Single instance of the date type for resusability
        /// </summary>
        public static LType Date = new LDate();


        /// <summary>
        /// Single instance of the function type for resusability
        /// </summary>
        public static LType Function = new LFunction();


        /// <summary>
        /// Single instanceo of the Map type for resusability
        /// </summary>
        public static LType Map = new LMap();


        /// <summary>
        /// Single instance of the Null type for resusability
        /// </summary>
        public static LType Null = new LNull();


        /// <summary>
        /// Single instance of the number type for resusability
        /// </summary>
        public static LType Number = new LNumber();


        /// <summary>
        /// Single instance of the string type for reusability
        /// </summary>
        public static LType String = new LString();


        /// <summary>
        /// Signle instance of the time type for reusability
        /// </summary>
        public static LType Time = new LTime();


        /// <summary>
        /// Object type.
        /// </summary>
        public static LType Object = new LObject(); 
    }
}
