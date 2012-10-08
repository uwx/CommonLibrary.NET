
using ComLib.Lang.Core;

namespace ComLib.Lang.Types
{

    /// <summary>
    /// Class to represent null
    /// </summary>
    public class LNull : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LNull()
        {
            this.Name = "null";
            this.FullName = "sys.null";
            this.TypeVal = TypeConstants.Null;
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.SetDefaultConversionMatrix(TypeConversionMode.NotSupported);
        }


        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static readonly LNull Instance = new LNull();


        /// <summary>
        /// Used for null values returned from ast evaulations.
        /// </summary>
        public static LTypeValue NullResult = new LTypeValue(null, new LNull());
    }
}
