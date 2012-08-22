/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib;


namespace ComLib
{
    /// <summary>
    /// Futher validation methods provided in an assert like manner 
    /// without throwing any exceptions.
    /// All methods take in the IStatusResults interface to collect the errors.
    /// </summary>
    public static partial class Validation
    {
        /// <summary>
        /// Determines if items are equal.
        /// </summary>
        /// <typeparam name="T">Type of objects to compare.</typeparam>
        /// <param name="obj1">First object.</param>
        /// <param name="obj2">Second object.</param>
        /// <param name="errors">Check errors.</param>
        /// <param name="tag">Tag.</param>
        /// <returns>True if items are equal.</returns>
        public static bool AreEqual<T>(T obj1, T obj2, IErrors errors, string tag) where T : IComparable<T>
        {
            return CheckError(obj1.CompareTo(obj2) == 0, errors, tag, _messages.ObjectsAreNotEqual);
        }


        /// <summary>
        /// Determines if objects are not equal
        /// </summary>
        /// <typeparam name="T">Type of objects to compare.</typeparam>
        /// <param name="obj1">First object.</param>
        /// <param name="obj2">Second object.</param>
        /// <param name="errors">Check errors.</param>
        /// <param name="tag">Tag.</param>
        /// <returns>True if items are not equal.</returns>
        public static bool AreNotEqual<T>(T obj1, T obj2, IErrors errors, string tag) where T : IComparable<T>
        {
            return CheckError(obj1.CompareTo(obj2) != 0, errors, tag, _messages.ObjectsAreEqual);
        }


        /// <summary>
        /// Checks that none of the strings in the array are null.
        /// </summary>
        /// <param name="items">Array of strings to check for null. e.g. "username1", "password1"</param>
        /// <param name="itemNames">Representative names of the strings supplied in the array.</param>
        /// <param name="errorSuffix">String to use at the end of each error ( if string is emtpy. )
        /// e.g. " is not supplied." would be suffixed to "Username is not supplied."</param>
        /// <param name="multilineSeparator">Separator to use for representing multiple errors on separate lines.</param>
        /// <returns>Comparison result.</returns>
        public static BoolMessage AreNoneNull(string[] items, string[] itemNames, string errorSuffix, string multilineSeparator)
        {
            ValidationResults errors = new ValidationResults();
            string separator = string.IsNullOrEmpty(multilineSeparator) ? Environment.NewLine : multilineSeparator;
            string suffix = string.IsNullOrEmpty(errorSuffix) ? _messages.IsNotSupplied : errorSuffix;
            for (int ndx = 0; ndx < items.Length; ndx++)
            {
                if (string.IsNullOrEmpty(items[ndx]))
                    errors.Add(itemNames[ndx] + suffix);
            }
            bool success = errors.Count == 0;
            string errorMessage = success ? string.Empty : errors.Message(separator);
            return new BoolMessage(success, errorMessage);
        }
    }
}
