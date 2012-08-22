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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Collections;
using System.Text;


namespace ComLib.Collections
{

    /// <summary>
    /// Dictionary based class to allow multiple values for a specific key.
    /// e.g. "searchsettings" = list{ setting1, setting2, setting3, .. settingN }
    /// where setting1 and setting2 both are associated with keys.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryMultiValue<TKey, TValue>
    {
        private IDictionary<TKey, IList<TValue>> _dict;


        /// <summary>
        /// Constructor requiring the generic dictionary being wrapped.
        /// </summary>
        public DictionaryMultiValue()
        {
            _dict = new Dictionary<TKey, IList<TValue>>();
        }


        /// <summary>
        /// Constructor requiring the generic dictionary being wrapped.
        /// </summary>
        /// <param name="dict"></param>
        public DictionaryMultiValue(IDictionary<TKey, IList<TValue>> dict)
        {
            _dict = dict;
        }


        #region IDictionary<TKey,TValue> Members
        /// <summary>
        /// Determine if the underlying collection contains the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }


        /// <summary>
        /// Number of items in the dictionary.
        /// </summary>
        public int Count
        {
            get { return _dict.Count; }
        }


        /// <summary>
        /// Returns the first of the multiple values associated with the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get 
            {
                if (_dict.ContainsKey(key))
                    return _dict[key][0];

                return default(TValue);
            }
            set
            {
                // Add to existing.
                if (_dict.ContainsKey(key))
                {
                    IList<TValue> valList = _dict[key];
                    valList.Add(value);
                }
                else
                {
                    IList<TValue> list = new List<TValue>();
                    list.Add(value);
                    _dict.Add(key, list);
                }
            }
        }


        /// <summary>
        /// Returns the entire list associated with the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<TValue> Get(TKey key)
        {
            if (!_dict.ContainsKey(key))
                return new List<TValue>();

            return _dict[key];
        }


        /// <summary>
        /// Return keys.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _dict.Keys; }
        }


        /// <summary>
        /// Add the key-value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            this[key] = value;
        }


        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }


        /// <summary>
        /// Try to get the value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);

            if (_dict.ContainsKey(key))
            {
                value = _dict[key][0];
                return true;
            }
            return false;
        }


        /// <summary>
        /// Get the values.
        /// </summary>
        public ICollection<IList<TValue>> Values
        {
            get 
            { 
                return _dict.Values; 
            }
        }
        #endregion


        #region ICollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }


        /// <summary>
        /// Not-Supported.
        /// </summary>
        public void Clear()
        {
            _dict.Clear();
        }


        /// <summary>
        /// Determine whether key value pair is in dictionary.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dict.ContainsKey(item.Key);
        }


        /// <summary>
        /// Copy items to the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            // TO_DO: Not supported presently.
            //this._dict.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Indicate read-only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Non-supported action.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _dict.Remove(item.Key);
        }

        #endregion


        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        #endregion
    }
}
