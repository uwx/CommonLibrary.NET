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

namespace ComLib
{

    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Execute action on each item in enumeration.
        /// </summary>
        /// <typeparam name="T">Type of item to use with the method.</typeparam>
        /// <param name="items">List of items.</param>
        /// <param name="action">Action to call for each item.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }


        /// <summary>
        /// Joins the specified items.
        /// </summary>
        /// <typeparam name="T">Type of item to use with the method.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <returns>String with joined items.</returns>
        public static string Join<T>(this IList<T> items, string delimeter)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return items[0].ToString();

            StringBuilder buffer = new StringBuilder();
            buffer.Append(items[0].ToString());

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                string append = items[ndx].ToString();
                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Join values using a delimeter.
        /// </summary>
        /// <typeparam name="T">Type of item to use with the method.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <param name="appender">The function to call when appending info.</param>
        /// <returns>String with joined items.</returns>
        public static string JoinDelimited<T>(this IList<T> items, string delimeter, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            string val = appender == null ? items[0].ToString() : appender(items[0]);
            buffer.Append(val);

            for(int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                val = appender == null ? item.ToString() : appender(item);                
                buffer.Append(delimeter + val);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Join values using a delimeter.
        /// </summary>
        /// <typeparam name="T">Type of item to use with the method.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <param name="newLineAfterCount">Number of lines after which a new line will be inserted.</param>
        /// <param name="newLineText">New line text to be inserted after each new line is inserted.</param>
        /// <param name="appender">The function to call when appending info.</param>
        /// <returns>String with joined items.</returns>
        public static string JoinDelimitedWithNewLine<T>(this IList<T> items, string delimeter, int newLineAfterCount, string newLineText, Func<T, string> appender)
        {
            if (items == null || items.Count == 0)
                return string.Empty;

            if (items.Count == 1)
                return appender(items[0]);

            StringBuilder buffer = new StringBuilder();
            buffer.Append(appender(items[0]));

            for (int ndx = 1; ndx < items.Count; ndx++)
            {
                T item = items[ndx];
                string append = appender(item);
                if (ndx % newLineAfterCount == 0)
                    buffer.Append(newLineText);

                buffer.Append(delimeter + append);
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Converts an enumerable collection to an delimited string.
        /// </summary>
        /// <typeparam name="T">Type of item to use with the method.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="delimeter">The delimeter.</param>
        /// <returns>Delimited string.</returns>
        public static string AsDelimited<T>(this IEnumerable<T> items, string delimeter)
        {
            List<string> itemList = new List<string>();
            foreach (T item in items)
            {
                itemList.Add(item.ToString());
            }
            return String.Join(delimeter, itemList.ToArray());
        }


        #region Conditional Checks
        /// <summary>
        /// Determines whether the specified enumerable collection is empty.
        /// Note: This method has the side effect of moving the position of
        /// the enumerator back to the starting position. Normally, this
        /// shouldn't effect anything unless you have a non-standard IEnumerable
        /// implementation.
        /// See also <a href="http://stackoverflow.com/questions/41319/checking-if-a-list-is-empty-with-linq"></a>.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">Enumerable to test</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsEmpty<T>(this IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            var isEmpty = !items.GetEnumerator().MoveNext();
            /* Reset the enumerator back to the starting position in the off
             * chance that we have a very poorly implemented IEnumerable
             * that does not return a *new* enumerator with every invocation
             * of the GetEnumerator method. */
            try
            {
                items.GetEnumerator().Reset();
            }
            // If this method is not supported, just skip the operation
            catch (NotSupportedException)
            {
            }

            return isEmpty;
        }

        /// <summary>
        /// Determines whether the specified enumerable collection is empty.
        /// Note: This method has the side effect of moving the position of
        /// the enumerator back to the starting position.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">Enumerable to test</param>
        /// <returns>
        /// 	<c>true</c> if the specified collection is empty; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            return items == null || items.IsEmpty();
        }

        /// <summary>
        /// Check for any nulls.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>True if a null is present in the list.</returns>
        public static bool HasAnyNulls<T>(this IEnumerable<T> items)
        {
            return IsTrueForAny<T>(items, t => t == null);
        }


        /// <summary>
        /// Check if any of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items to use.</param>
        /// <param name="executor">Function to call for each item.</param>
        /// <returns>True if the executor returned True for at least one item.</returns>
        public static bool IsTrueForAny<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {

            foreach (T item in items)
            {
                bool result = executor(item);
                if (result)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Check if all of the items in the collection satisfied by the condition.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <param name="executor">Function to call for each item.</param>
        /// <returns>True if the executor returned true for all items.</returns>
        public static bool IsTrueForAll<T>(this IEnumerable<T> items, Func<T, bool> executor)
        {            
            foreach (T item in items)
            {
                bool result = executor(item);
                if (!result)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Converts a list of items to a dictionary with the items.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>Converted list as dictionary.</returns>
        public static IDictionary<T, T> ToDictionary<T>(this IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }        
        #endregion
    }
}
