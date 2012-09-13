using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            this._value = null;
            this.DataType = typeof(Nullable);
        }


        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static readonly LNull Instance = new LNull();
    }
}
