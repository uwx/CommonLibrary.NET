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

namespace CommonLibrary
{
    /// <summary>
    /// Dictionary for bidirectional lookup.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryMultiKey<TKey1, TKey2, TValue> : IDictionary<TKey1, TValue>
    {
        #region Private Data members
        private IDictionary<TKey1, TValue> _key1Map = new Dictionary<TKey1, TValue>();
        private IDictionary<TKey2, TValue> _key2Map = new Dictionary<TKey2, TValue>();
        #endregion


        #region Constructors
        /// <summary>
        /// Create new instance with empty bi-directional lookups.
        /// </summary>
        public DictionaryBidirectional() { }


        /// <summary>
        /// Initialize using existing forward and reverse lookups.
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="reverse"></param>
        public DictionaryBidirectional(IDictionary<TKey, TValue> forward, IDictionary<TValue, TKey> reverse)
        {
            _key1Map = forward;
            _key2Map = reverse;
        }
        #endregion Constructors


        #region IDictionary<TKey,TValue> Members
        /// <summary>
        /// Add to key/value for both forward and reverse lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            _key1Map.Add(key, value);
            _key2Map.Add(value, key);
        }


        /// <summary>
        /// Determine if the key is contain in the forward lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _key1Map.ContainsKey(key);
        }


        /// <summary>
        /// Get a list of all the keys in the forward lookup.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _key1Map.Keys; }
        }


        /// <summary>
        /// Remove the key for both forward and reverse lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            _key2Map.Remove(_key1Map[key]);
            return _key1Map.Remove(key);
        }


        /// <summary>
        /// Try to get the value from the forward lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _key1Map.TryGetValue(key, out value);
        }


        /// <summary>
        /// Get the collection of values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return _key1Map.Values; }
        }


        /// <summary>
        /// Set the key / value for bi-directional lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get
            {
                return _key1Map[key];
            }
            set
            {
                _key1Map[key] = value;
                _key2Map[value] = key;
            }
        }
        #endregion


        #region ICollection<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Add to bi-directional lookup.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _key1Map.Add(item);
            _key2Map.Add(new KeyValuePair<TValue, TKey>(item.Value, item.Key));
        }


        /// <summary>
        /// Clears keys/value for bi-directional lookup.
        /// </summary>
        public void Clear()
        {
            _key1Map.Clear();
            _key2Map.Clear();
        }


        /// <summary>
        /// Determine if the item is in the forward lookup.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _key1Map.Contains(item);
        }


        /// <summary>
        /// Copies the array of key/value pairs for both bi-directionaly lookups.
        /// TO_DO: This needs to be unit-tested since, I don't think I'm handling
        /// the _key2Map correctly.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _key1Map.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Get number of entries.
        /// </summary>
        public int Count
        {
            get { return _key1Map.Count; }
        }


        /// <summary>
        /// Get whether or not this is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _key1Map.IsReadOnly; }
        }


        /// <summary>
        /// Remove the item from bi-directional lookup.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            _key2Map.Remove(item.Value);
            return _key1Map.Remove(item);
        }
        #endregion


        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Get the enumerator for the forward lookup.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _key1Map.GetEnumerator();
        }
        #endregion


        #region IEnumerable Members
        /// <summary>
        /// Get the enumerator for the forward lookup.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _key1Map.GetEnumerator();
        }
        #endregion


        #region Public Reverse Lookup methods
        /// <summary>
        /// Determine whether or not the reverse lookup contains the key
        /// represented by the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(TValue value)
        {
            return _key2Map.ContainsKey(value);
        }

        /// <summary>
        /// Determine whether or the reverse lookup ( value ) exists.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TKey ContainsReverseLookup(TValue value)
        {
            return _key2Map[value];
        }
        #endregion
    } 
}
