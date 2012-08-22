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
    /// 2 Level Tree like dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DictionarySet<T> : IDictionary<T, T>, ISet<T>
    {
        private IDictionary<T, T> _map;


        /// <summary>
        /// Constructor requiring the generic dictionary being wrapped.
        /// </summary>
        public DictionarySet()
        {
            _map = new Dictionary<T, T>();
        }


        #region IDictionary<T,T> Members
        /// <summary>
        /// Determine if the underlying collection contains the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(T key)
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
        public T this[T key]
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
        public ICollection<T> Keys
        {
            get { return _map.Keys; }
        }


        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T key, T value)
        {
            _map.Add(key, value);
        }


        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(T key)
        {
            return _map.Remove(key);
        }


        /// <summary>
        /// Try to get the value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(T key, out T value)
        {
            value = default(T);

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
        public ICollection<T> Values
        {
            get { return _map.Values; }
        }
        #endregion


        #region ICollection<KeyValuePair<T,T>> Members
        /// <summary>
        /// Not-supported.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<T, T> item)
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
        public bool Contains(KeyValuePair<T, T> item)
        {
            return _map.Contains(item);
        }


        /// <summary>
        /// Copy items to the array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<T, T>[] array, int arrayIndex)
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
        public bool Remove(KeyValuePair<T, T> item)
        {
            return _map.Remove(item);
        }

        #endregion


        #region IEnumerable<KeyValuePair<T,T>> Members
        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, T>> GetEnumerator()
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


        #region ISet<T> Members
        /// <summary>
        /// Unions the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public ISet<T> Union(ISet<T> other)
        {
            return SetHelper<T>.Union(this, other);
        }


        /// <summary>
        /// Intersects the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public ISet<T> Intersect(ISet<T> other)
        {
            return SetHelper<T>.Intersect(this, other);
        }


        /// <summary>
        /// Exclusives the or.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public ISet<T> ExclusiveOr(ISet<T> other)
        {
            return SetHelper<T>.ExclusiveOr(this, other);
        }


        /// <summary>
        /// Minuses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public ISet<T> Minus(ISet<T> other)
        {
            return SetHelper<T>.Minus(this, other);
        }
        #endregion


        #region ICollection<T> Members
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(T item)
        {
            _map.Add(item, item);
        }


        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return _map.ContainsKey(item);
        }


        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// 	<paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
        
        }
        #endregion


        #region IEnumerable<T> Members
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _map.Keys.GetEnumerator();
        }
        #endregion
    }
}
