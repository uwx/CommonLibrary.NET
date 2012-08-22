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
    /// Dictionary for bidirectional lookup.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class DictionaryBidirectional<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Private Data members
        private IDictionary<TKey, TValue> _forwardMap = new Dictionary<TKey, TValue>();
        private IDictionary<TValue, TKey> _reverseMap = new Dictionary<TValue, TKey>();
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
            _forwardMap = forward;
            _reverseMap = reverse;
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
            _forwardMap.Add(key, value);
            _reverseMap.Add(value, key);
        }


        /// <summary>
        /// Determine if the key is contain in the forward lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return _forwardMap.ContainsKey(key);
        }


        /// <summary>
        /// Get a list of all the keys in the forward lookup.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return _forwardMap.Keys; }
        }


        /// <summary>
        /// Remove the key for both forward and reverse lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            _reverseMap.Remove(_forwardMap[key]);
            return _forwardMap.Remove(key);
        }


        /// <summary>
        /// Try to get the value from the forward lookup.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _forwardMap.TryGetValue(key, out value);
        }


        /// <summary>
        /// Get the collection of values.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return _forwardMap.Values; }
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
                return _forwardMap[key];
            }
            set
            {
                _forwardMap[key] = value;
                _reverseMap[value] = key;
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
            _forwardMap.Add(item);
            _reverseMap.Add(new KeyValuePair<TValue, TKey>(item.Value, item.Key));
        }


        /// <summary>
        /// Clears keys/value for bi-directional lookup.
        /// </summary>
        public void Clear()
        {
            _forwardMap.Clear();
            _reverseMap.Clear();
        }


        /// <summary>
        /// Determine if the item is in the forward lookup.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _forwardMap.Contains(item);
        }


        /// <summary>
        /// Copies the array of key/value pairs for both bi-directionaly lookups.
        /// TO_DO: This needs to be unit-tested since, I don't think I'm handling
        /// the _reverseMap correctly.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _forwardMap.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Get number of entries.
        /// </summary>
        public int Count
        {
            get { return _forwardMap.Count; }
        }


        /// <summary>
        /// Get whether or not this is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return _forwardMap.IsReadOnly; }
        }


        /// <summary>
        /// Remove the item from bi-directional lookup.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            _reverseMap.Remove(item.Value);
            return _forwardMap.Remove(item);
        }
        #endregion


        #region IEnumerable<KeyValuePair<TKey,TValue>> Members
        /// <summary>
        /// Get the enumerator for the forward lookup.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _forwardMap.GetEnumerator();
        }
        #endregion


        #region IEnumerable Members
        /// <summary>
        /// Get the enumerator for the forward lookup.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _forwardMap.GetEnumerator();
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
            return _reverseMap.ContainsKey(value);
        }

        
        /// <summary>
        /// Determine whether or the reverse lookup ( value ) exists.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TKey ContainsReverseLookup(TValue value)
        {
            return _reverseMap[value];
        }


        /// <summary>
        /// Determine whether or the reverse lookup ( value ) exists.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TKey KeyFor(TValue value)
        {
            return _reverseMap[value];
        }
        #endregion
    } 
}
