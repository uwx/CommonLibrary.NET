
using ComLib.Lang.Core;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Datatype for map
    /// </summary>
    public class LMap : LObject
    {        

        /// <summary>
        /// Initialize
        /// </summary>
        public LMap()
        {
            this.Name = "map";
            this.FullName = "sys.map";
            this.TypeVal = TypeConstants.Map;
            // Dictionary<string, object>
        }


        /// <summary>
        /// Sets up the matrix of possible conversions from one type to another type.
        /// </summary>
        public override void SetupConversionMatrix()
        {
            this.SetDefaultConversionMatrix(TypeConversionMode.NotSupported);
            this.AddConversionTo(TypeConstants.Map,   TypeConversionMode.SameType);
        }
    }
}
