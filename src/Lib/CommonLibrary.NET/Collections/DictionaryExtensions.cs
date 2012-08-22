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
using System.Collections;
using System.Linq;
using System.Text;
using ComLib;

namespace ComLib.Collections
{
    /*
    /// <summary>
    /// Extensions to Non-Generic Dictionary
    /// </summary>
    public static class DictionaryNonGenericExtensions
    {
        /// <summary>
        /// Get typed value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary d, object key)
        {
            object result = d[key];
            T converted = (T)ConvertTo<T>(result);
            return converted;
        }


        /// <summary>
        /// Get using default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary d, object key, T defaultValue)
        {
            if (!d.Contains(key)) return defaultValue;

            object result = d[key];
            T converted = (T)ConvertTo<T>(result);
            return converted;
        }


        /// <summary>
        /// Convert to correct type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(object input)
        {
            object result = default(T);
            if (typeof(T) == typeof(int))
                result = System.Convert.ToInt32(input);
            else if (typeof(T) == typeof(long))
                result = System.Convert.ToInt64(input);
            else if (typeof(T) == typeof(string))
                result = System.Convert.ToString(input);
            else if (typeof(T) == typeof(bool))
                result = System.Convert.ToBoolean(input);
            else if (typeof(T) == typeof(double))
                result = System.Convert.ToDouble(input);
            else if (typeof(T) == typeof(DateTime))
                result = System.Convert.ToDateTime(input);
            else
                result = input;

            return (T)result;
        }
    }

    */

    /// <summary>
    /// Extensions to Non-Generic Dictionary
    /// </summary>
    public static class DictionaryStringExtensions
    {

        #region Public dictionary value conversion methods
        /// <summary>
        /// Get the value associated with the key as a int.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetInt<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToInt32(d[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a bool.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetBool<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToBoolean(d[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a string.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToString(d[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a double.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static double GetDouble<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToDouble(d[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a datetime.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime GetDateTime<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToDateTime(d[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a long.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetLong<TKey>(this IDictionary<TKey, string> d, TKey key)
        {
            return Convert.ToInt64(d[key]);
        }
        #endregion
    }
}
