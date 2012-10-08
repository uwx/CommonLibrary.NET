
using System.Collections.Generic;
using ComLib.Lang.Core;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array datatype
    /// </summary>
    public class LArray : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LArray()
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
