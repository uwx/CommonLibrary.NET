using System;


namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array type.
    /// </summary>
    public class LDate : LObject
    {
        /// <summary>
        /// Can create from the paramelist expressions supplied.
        /// </summary>
        /// <returns></returns>
        public static bool CanCreateFrom(int paramCount)
        {            
            if (paramCount != 0 && paramCount != 1 && paramCount != 3 && paramCount != 6)
                return false;            
            return true;
        }


        /// <summary>
        /// Creates a datetime from the parameters supplied.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DateTime CreateFrom(object[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return DateTime.Now;
            }

            var result = DateTime.MinValue;
            
            // Case 1: From string
            if (parameters.Length == 1 && parameters[0] is string)
            {
                result = DateTime.Parse((string)parameters[0]);
                return result;
            }

            // Case 2: From Date
            if (parameters.Length == 1 && parameters[0] is DateTime)
            {
                var d = (DateTime)parameters[0];
                result = new DateTime(d.Ticks);
                return result;
            }
            
            // Convert all parameters to int            
            var args = new int[parameters.Length];            
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                args[ndx] = Convert.ToInt32(parameters[ndx]);
            }

            // Case 3: 3 parameters month, day, year
            if (parameters.Length == 3)
                return new DateTime(args[0], args[1], args[2]);

            // Case 4: 6 parameters
            if (parameters.Length == 6)
                return new DateTime(args[0], args[1], args[2], args[3], args[4], args[5]);

            // TODO: Need to handle this better.
            return DateTime.MinValue;    
        }
        
        
        /// <summary>
        /// Initialize with date.
        /// </summary>
        /// <param name="date"></param>
        public LDate(DateTime date)
        {
            Raw = date;
        }


        /// <summary>
        /// Initialize with date.
        /// </summary>
        /// <param name="varname">The name of the variable</param>
        /// <param name="date">The raw datetime value</param>
        public LDate(string varname, DateTime date)
        {
            _value = varname;
            Raw = date;
        }


        /// <summary>
        /// The raw datetime.
        /// </summary>
        public DateTime Raw;
    }
}
