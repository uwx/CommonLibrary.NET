

using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Used to store a number value.
    /// </summary>
    public class LNumber : LObjectValue
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val"></param>
        public LNumber(double val)
        {
            this.Value = val;
            this.Type = LTypes.Number;
        }


        /// <summary>
        /// The raw type value.
        /// </summary>
        public double Value;
    }



    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LNumberType : LObjectType
    {   
        /// <summary>
        /// Initialize
        /// </summary>
        public LNumberType()
        {
            this.Name = "number";
            this.FullName = "sys.number";
            this.TypeVal = TypeConstants.Number;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.AddConversionTo(TypeConstants.Array,     TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Bool,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Date,      TypeConversionMode.RunTimeCheck);
            this.AddConversionTo(TypeConstants.Map,       TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Number,    TypeConversionMode.SameType);
            this.AddConversionTo(TypeConstants.Null,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.String,    TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Time,      TypeConversionMode.RunTimeCheck);
        }
    }
}
