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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using ComLib;


namespace ComLib.Collections
{
    /// <summary>
    /// Semi-Generic based dictionary where the values are always strings which 
    /// can be converted to any type using the public conversion methods such 
    /// as GetInt(key), GetBool(key) etc.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class DictionaryTypeConversion<TKey> : IDictionary<TKey, string>
    {
        private IDictionary<TKey, string> _map;


        /// <summary>
        /// Constructor requiring the generic dictionary being wrapped.
        /// </summary>
        public DictionaryTypeConversion()
        {
            _map = new Dictionary<TKey, string>();
        }


        #region Public dictionary value conversion methods
        /// <summary>
        /// Get the value associated with the key as a int.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(TKey key)
        {
            return Convert.ToInt32(this[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a bool.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetBool(TKey key)
        {
            return Convert.ToBoolean(this[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(TKey key)
        {
            return this[key];
        }


        /// <summary>
        /// Get the value associated with the key as a double.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double GetDouble(TKey key)
        {
            return Convert.ToDouble(this[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a datetime.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DateTime GetDateTime(TKey key)
        {
            return Convert.ToDateTime(this[key]);
        }


        /// <summary>
        /// Get the value associated with the key as a long.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetLong(TKey key)
        {
            return Convert.ToInt64(this[key]);
        }
        #endregion


        #region IDictionary<TKey,TValue> Members
        /// <summary>
        /// Determine if the underlying collection contains the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _map.ContainsKey(key);
        }


        /// <summary>
        /// Number of items in the dictionary.
        /// </summary>
        public int Count
        {
            get { return _map.Count; }
        }


        /// <summary>
        /// Returns the value associated with the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[TKey key]
        {
            get { return _map[key]; }
            set
            {
                _map[key] = value;
            }
        }


        /// <summary>
        /// Return keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _map.Keys; }
        }


        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, string value)
        {
            _map.Add(key, value);
        }


        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            return _map.Remove(key);
        }


        /// <summary>
        /// Try to get the value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out string value)
        {
            value = string.Empty;

            if (_map.ContainsKey(key))
            {
                value = _map[key];
                return true;
            }
            return false;
        }


        /// <summary>
        /// Get the values.
        /// </summary>
        public ICollection<string> Values
        {
            get { return _map.Values; }
        }
        #endregion


        #region ICollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, string> item)
        {
            _map.Add(item);
        }


        /// <summary>
        /// Not-Supported.
        /// </summary>
        public void Clear()
        {
            _map.Clear();
        }


        /// <summary>
        /// Determine whether key value pair is in dictionary.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, string> item)
        {
            return _map.Contains(item);
        }


        /// <summary>
        /// Copy items to the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, string>[] array, int arrayIndex)
        {
            this._map.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Indicate read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }


        /// <summary>
        /// Non-supported action.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, string> item)
        {
            return _map.Remove(item);
        }

        #endregion


        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, string>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        #endregion


        #region IEnumerable Members
        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        #endregion
    }
}
