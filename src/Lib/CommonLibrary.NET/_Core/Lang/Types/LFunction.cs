using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// function in script
    /// </summary>
    public class LFunction : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LFunction()
        {
            this.Name = "function";
            this.FullName = "sys.function";
            this.TypeVal = TypeConstants.Function;
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
