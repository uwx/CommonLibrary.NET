using System;

using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array type.
    /// </summary>
    public class LTime : LObject
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public LTime()
        {
            this.Name = "time";
            this.FullName = "sys.time";
            this.TypeVal = TypeConstants.Time;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.AddConversionTo(TypeConstants.Array,     TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Bool,      TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Date,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Map,       TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Number,    TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Null,      TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.String,    TypeConversionMode.Supported);
            this.AddConversionTo(TypeConstants.Time,      TypeConversionMode.SameType);
        }
    }
}
