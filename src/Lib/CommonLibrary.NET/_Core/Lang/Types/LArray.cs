
using System.Collections.Generic;
using ComLib.Lang.Core;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Used to store a array
    /// </summary>
    public class LArray : LObjectValue
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val"></param>
        public LArray(List<object> val)
        {
            this.Value = val;
            this.Type = LTypes.Array;
        }


        /// <summary>
        /// The raw type value.
        /// </summary>
        public List<object> Value;


        /// <summary>
        /// Gets the value of this object.
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return this.Value;
        }
    }


    /// <summary>
    /// Array datatype
    /// </summary>
    public class LArrayType : LObjectType
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LArrayType()
        {
            this.Name = "array";
            this.FullName = "sys.array";
            this.TypeVal = TypeConstants.Array;
            // List<object>
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.SetDefaultConversionMatrix(TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Array,   TypeConversionMode.SameType);
        }
    }
}
