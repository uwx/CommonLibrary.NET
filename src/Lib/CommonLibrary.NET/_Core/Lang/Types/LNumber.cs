﻿

using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LNumber : LObject
    {   
        /// <summary>
        /// Initialize
        /// </summary>
        public LNumber()
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
