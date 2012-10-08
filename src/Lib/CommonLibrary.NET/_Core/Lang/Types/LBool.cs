using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LBool : LObject
    {
        /// <summary>
        /// Initialize bool value.
        /// </summary>
        public LBool()
        {
            this.Name = "bool";
            this.FullName = "sys.bool";
            this.TypeVal = TypeConstants.Bool;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.AddConversionTo(TypeConstants.Array,     TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Bool,      TypeConversionMode.SameType);
            this.AddConversionTo(TypeConstants.Date,      TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Map,       TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Number,    TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Null,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.String,    TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Time,      TypeConversionMode.NotSupported);
        }
    }
}
