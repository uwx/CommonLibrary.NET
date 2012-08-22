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

using ComLib;


namespace ComLib.Collections
{
    
    /// <summary>
    /// Class to serve as a base class for generic lists.
    /// This is because the Generic list <see cref="System.Collections.Generic.List&lt;T&gt;"/>
    /// is not meant to be extendable."/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericListBase<T> : IList<T>
    {
        /// <summary>
        /// List of items.
        /// </summary>
        protected IList<T> _items;


        /// <summary>
        /// Creates a new instance of this class
        /// with an empty list.
        /// </summary>
        public GenericListBase()
        {
            _items = new List<T>();
        }
   

        #region IList<T> Members
        /// <summary>
        /// Index of
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _items.IndexOf(item);
        }


        /// <summary>
        /// Insert.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public virtual void Insert(int index, T item)
        {
            _items.Insert(index, item);
        }


        /// <summary>
        /// Remove at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }


        /// <summary>
        /// Accessor.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
            }
        }

        #endregion

                
        #region ICollection<T> Members
        /// <summary>
        /// Add a list of models that should be shown in the dashboard on the sidebar.
        /// </summary>
        /// <param name="items"></param>
        public virtual void Add(params T[] items)
        {
            if (items == null || items.Length == 0)
                return;

            // Add the default definitions
            foreach (T item in items)
                this.Add(item);
        }


        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            _items.Add(item);
        }


        /// <summary>
        /// Clear
        /// </summary>
        public virtual void Clear()
        {
            _items.Clear();
        }


        /// <summary>
        /// Determines whether an item is
        /// present in the current list.
        /// </summary>
        /// <param name="item">Item to look for.</param>
        /// <returns>True if item already exists.</returns>
        public virtual bool Contains(T item)
        {
            return _items.Contains(item);
        }


        /// <summary>
        /// Copy to array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// Count of items.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }


        /// <summary>
        /// Determine if is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return _items.IsReadOnly; }
        }


        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Remove(T item)
        {
            return _items.Remove(item);
        }

        #endregion


        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator to the list.
        /// </summary>
        /// <returns>List enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }
 
}
