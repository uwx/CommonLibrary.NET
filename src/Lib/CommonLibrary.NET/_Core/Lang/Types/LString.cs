


using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LString : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LString()
        {
            this.Name = "string";
            this.FullName = "sys.string";
            this.TypeVal = TypeConstants.String;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.AddConversionTo(TypeConstants.Array,     TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Bool,      TypeConversionMode.RunTimeCheck);
            this.AddConversionTo(TypeConstants.Date,      TypeConversionMode.RunTimeCheck);
            this.AddConversionTo(TypeConstants.Map,       TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Number,    TypeConversionMode.RunTimeCheck);
            this.AddConversionTo(TypeConstants.Null,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.String,    TypeConversionMode.SameType);
            this.AddConversionTo(TypeConstants.Time,      TypeConversionMode.RunTimeCheck);
        }
    }
}
