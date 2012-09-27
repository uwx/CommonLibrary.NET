
using System.Collections.Generic;

using ComLib.Lang.Parsing;

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
        /// <param name="val">Values of the map</param>
        public LMap(Dictionary<string, object> val) //: this(null, val) 
        {
            Raw = val;
        }


        /// <summary>
        /// Raw value
        /// </summary>
        public Dictionary<string, object> Raw;
    }
}
