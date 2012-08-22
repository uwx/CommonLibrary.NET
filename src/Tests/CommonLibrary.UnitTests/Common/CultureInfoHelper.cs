using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary.Tests
{

    /// <summary>
    /// Helper class providing culture info methods. 
    /// </summary>
    /// <remarks>Some tests deal with numbers and dates, the format of which may change 
    /// depending on the culture info used by a system. This class provides methods that
    /// return culture info information and perform other changes on test data.
    /// </remarks>
    public class CultureInfoHelper
    {

        /// <summary>
        /// Returns the decimal separator used in the system.
        /// </summary>
        /// <returns>String with decimal separator used in the system.</returns>
        public static string GetDecimalSeparator()
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        }


        /// <summary>
        /// Fixes dates present in CSV test files according to the system date format.
        /// </summary>
        /// <param name="csvText"></param>
        /// <returns></returns>
        public static string FixDates(string csvText)
        {
            try
            {
                // See if this makes sense. If so, return everything unchanged.
                DateTime.Parse("4/13/2009");
                return csvText;
            }
            catch
            {
                // If it doesn't the date format is different.
                // Replace with what is universally understood.
                return csvText.Replace("4/10/2009","2009/04/10")
                              .Replace("4/11/2009","2009/04/11")
                              .Replace("4/12/2009","2009/04/12")
                              .Replace("4/13/2009","2009/04/13");
            }

        }
    }
}
