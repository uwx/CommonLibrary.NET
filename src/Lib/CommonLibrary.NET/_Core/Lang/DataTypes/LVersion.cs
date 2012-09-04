using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// Boolean datatype.
    /// </summary>
    public class LVersion : LObject
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="v"></param>
        public LVersion(Version v)
        {
            this.Major = v.Major;
            this.Minor = v.Minor;
            this.Build = v.Build;
            this.Revision = v.Revision;
        }


        /// <summary>
        /// The major part of the version ( first )
        /// </summary>
        public int Major    { get; set; }
                 
        
        /// <summary>
        /// The minor part of the version ( second )
        /// </summary> 
        public int Minor    { get; set; }
                      
        
        /// <summary>
        /// The build part of the version ( third )
        /// </summary>
        public int Build    { get; set; }
                                   

        /// <summary>
        /// The revision part of the version ( fourth )
        /// </summary>
        public int Revision { get; set; }


        /// <summary>
        /// The text representation of the version.
        /// </summary>
        /// <returns></returns>
        public string Text()
        {
            var text = this.Major + "." + this.Minor + "." + this.Build;
            if(this.Revision < 0 )
                return text;

            return text + "." + this.Revision.ToString();
        }
    }
}
