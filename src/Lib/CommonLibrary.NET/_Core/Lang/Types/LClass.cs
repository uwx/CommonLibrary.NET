using System;

using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array type.
    /// </summary>
    public class LClass : LObjectType
    {
        /// <summary>
        /// Used for now since fluentscript doesn't support classes.
        /// But this is used for using external c# classes in fluentscript.
        /// </summary>
        public Type DataType;


        /// <summary>
        /// Initialize.
        /// </summary>
        public LClass()
        {
            // To be determined during parsing phase.
            this.Name = "place_holder";
            this.FullName = "place_holder";
            this.TypeVal = TypeConstants.LClass;
            this.DataType = null;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.SetDefaultConversionMatrix(TypeConversionMode.NotSupported);
        }
    }
}
