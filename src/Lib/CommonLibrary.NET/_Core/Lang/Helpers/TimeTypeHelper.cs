using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for time types.
    /// </summary>
    public class TimeTypeHelper
    {
        /// <summary>
        /// Whether or not a timespan can be created from the number of arguments supplied.
        /// </summary>
        /// <param name="paramCount">The number of parameters</param>
        /// <returns></returns>
        public static bool CanCreateTimeFrom(int paramCount)
        {            
            // 1. 0 args = new TimeSpan()
            // 2. 3 args = new TimeSpan(hours, mins, secs)
            // 3. 4 args = new TimeSpan(days, hours, mins, secs)
            if (paramCount == 0 || paramCount == 3 || paramCount == 4)
                return true;

            return false;
        }


        /// <summary>
        /// Whether or not a timespan can be created from the number of arguments supplied.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static TimeSpan CreateTimeFrom(object[] args)
        {
            // Validate
            if (!CanCreateTimeFrom(args.Length)) 
                throw new ArgumentException("Incorrect number of inputs for creating time");

            // Convert object into ints
            int[] timeArgs = LangHelper.ConvertToInts(args);
            int len = args.Length;

            // 1. 0 args = new TimeSpan()
            if (len == 0) return new TimeSpan();

            // 2. 3 args = new TimeSpan(hours, mins, secs)
            if (len == 3) return new TimeSpan(timeArgs[0], timeArgs[1], timeArgs[2]);

            // 3. 4 args = new TimeSpan(days, hours, mins, secs)
            return new TimeSpan(timeArgs[0], timeArgs[1], timeArgs[2], timeArgs[3]);
        }
    }
}
