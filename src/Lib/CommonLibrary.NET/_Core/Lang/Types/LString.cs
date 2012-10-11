


using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Used to store a string value.
    /// </summary>
    public class LString : LObjectValue
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="val"></param>
        public LString(string val)
        {
            this.Value = val;
            this.Type = LTypes.String;
        }


        /// <summary>
        /// The raw type value.
        /// </summary>
        public string Value;
    }



    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LStringType : LObjectType
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LStringType()
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
