using System;

namespace ComLib.Extensions
{

    /// <summary>
    /// Collection of utility extension methods related to the decimal data
    /// type.
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// Method that finds a digit at an arbirary position of a decimal.
        /// <param name="number">The number.</param>
        /// <param name="position">The position of the digit to the right of the decimal (1-n).</param>
        /// <returns>Digit at position</returns>
        /// <example>
        /// var number = 1.2459m;
        /// var digit = number.DigitAtPosition(3); // value is 5
        /// </example>
        /// <remarks>See also http://stackoverflow.com/questions/2923510/what-is-the-best-way-to-find-the-digit-at-n-position-in-a-decimal-number/2924042#2924042. </remarks>
        /// </summary>
        public static int DigitAtPosition(this decimal number, int position)
        {
            if (position <= 0)
            {
                throw new ArgumentException("Position must be positive.");
            }

            if (number < 0)
            {
                number = Math.Abs(number);
            }

            return number.SanitizedDigitAtPosition(position);
        }

        private static int SanitizedDigitAtPosition(this decimal sanitizedNumber,
            int validPosition)
        {
            sanitizedNumber -= Math.Floor(sanitizedNumber);

            if (sanitizedNumber == 0)
            {
                return 0;
            }

            if (validPosition == 1)
            {
                return (int)(sanitizedNumber * 10);
            }

            return (sanitizedNumber * 10).SanitizedDigitAtPosition(validPosition - 1);
        }

        /// <summary>
        /// Returns a specified decimal number to a specified power. This method
        /// performs slower than the Math.Pow() method, but in some circumstances
        /// it is more accurate.
        /// </summary>
        /// <param name="number">A decimal number to be raised to a power</param>
        /// <param name="exponent">A decimal number that specifies a power</param>
        /// <returns>The exponential value of the base number</returns>
        public static decimal Pow(this decimal number, decimal exponent)
        {
            // an exponent value of 0 will always return 1
            if (exponent == 0)
            {
                return 1;
            }

            /* If the base value we are multiplying against is 0 or 1, the
             * result will always be the value. */
            if (number == 0 || number == 1)
            {
                return number;
            }

            /* Test to see if the exponent is a whole number or not, 
             * if so we can calculate the exponent using higher-precision math 
             * than the Math.Pow method. */
            var result = number;

            // --== DECIMAL CASE ==--

            /* Math is hard. For fractional exponents, we just use the relatively
             * impercise Math.Pow() and cast to decimal.
             * TODO: Determine how to do this operation with greater percision */
            if (Math.Truncate(exponent) < exponent)
            {
                return new decimal(Math.Pow(Decimal.ToDouble(number),
                    Decimal.ToDouble(exponent)));
            }

            // --== POSITIVE CASE ==--
            /* In order to compute negative exponents, we need to be able
             * to compute the power as a positive number. */
            var power = exponent < 0 ? Math.Abs(exponent) : exponent;

            for (var i = 1; i < power; i++)
            {
                result *= number;
            }

            /* Return positive exponents now because we have already obtained
             * their results. */
            if (exponent > 0)
            {
                return result;
            }

            // --== NEGATIVE CASE ==--

            /* Otherwise, we are computing a negative exponent.
             * Using the following formula: x^-2 = 1/x^2. Note, we needed
             * the value from the positive case to compute the negative case. */
            return 1m / result;
        }
    }
}