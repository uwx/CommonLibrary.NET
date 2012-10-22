using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Tests.Common
{
    /// <summary>
    /// Used to store a collection of testcases.
    /// </summary>
    public class HostTypes
    {
        public static Type Double = typeof(double);
        public static Type String = typeof(string);
        public static Type Bool   = typeof(bool);
        public static Type Date   = typeof(DateTime);
        public static Type Time   = typeof(TimeSpan);
        public static Type Object = typeof(object);
        public static Type Null =   typeof(Nullable);

    }
}
