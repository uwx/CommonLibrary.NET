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
using System.Text;

using ComLib;


namespace ComLib.Collections
{
    /// <summary>
    /// Interface that any component must implement
    /// so that it can be indexed by an id or by a name.
    /// </summary>
    /// <typeparam name="TNumericKey"></typeparam>
    public interface IIndexedComponent<TNumericKey>
    {
        /// <summary>
        /// Get the key id.
        /// </summary>
        TNumericKey Id { get; }


        /// <summary>
        /// Creates a string key based on the id.
        /// </summary>
        /// <returns>String key.</returns>
        string BuildKey();
    }



    /// <summary>
    /// Interface for storing a collection of objects of type T,
    /// such that the objects can be looked up by either the
    /// id of the object T or by creating a distinct name for the object
    /// based on it's hashcode.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TNumericKey"></typeparam>
    public interface IIndexedLookUp<TNumericKey, T> where T : IIndexedComponent<TNumericKey>
    {
        /// <summary>
        /// Gets an object based on its id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <returns>Object corresponding to id.</returns>
        T this[TNumericKey id] { get; }


        /// <summary>
        /// Gets an object based on its name.
        /// </summary>
        /// <param name="name">Name of object.</param>
        /// <returns>Object corresponding to name.</returns>
        T this[string name] { get; }


        /// <summary>
        /// Gets the number of items stored in the collection.
        /// </summary>
        int Count { get; }
    }



    /// <summary>
    /// Indexed lookup class for storing objects of type T which can
    /// be retrieved by either an id or name.
    /// </summary>
    /// <typeparam name="TNumericKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class IndexedLookUp<TNumericKey, T> : IIndexedLookUp<TNumericKey, T> where T : class, IIndexedComponent<TNumericKey>
    {
        /// <summary>
        /// Dictionary with all items ordered by id.
        /// </summary>
        protected IDictionary<TNumericKey, T> _allItemsById;


        /// <summary>
        /// Dictionary with all items order by name.
        /// </summary>
        protected IDictionary<string, T> _allItemsByName;


        /// <summary>
        /// List with all items.
        /// </summary>
        protected IList<T> _allItems;

        
        /// <summary>
        /// Generic based lookup.
        /// </summary>
        /// <param name="allItems"></param>
        public IndexedLookUp(IList<T> allItems)
        {
            _allItems = allItems;
            _allItemsById = new Dictionary<TNumericKey, T>();
            _allItemsByName = new Dictionary<string, T>();
            Initialize(allItems);
        }


        /// <summary>
        /// Returns the location item given the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T this[TNumericKey id]
        {
            get
            {
                if (!_allItemsById.ContainsKey(id))
                {
                    return null;
                }
                return _allItemsById[id];
            }
        }


        /// <summary>
        /// Returns the location item given the full name ("New York") or abbr ( "NY" )
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual T this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name)) { return null; }

                string lowerCaseName = name.Trim().ToLower();
                
                if (_allItemsByName.ContainsKey(lowerCaseName))
                {
                    return _allItemsByName[lowerCaseName];
                }

                return null;   
            }
        }


        /// <summary>
        /// Get the number of items in this lookup.
        /// </summary>
        public int Count
        {
            get { return _allItemsById.Count; }
        }


        /// <summary>
        /// Initialize the internal lookup tables with the items.
        /// Store them by id and name.
        /// </summary>
        /// <param name="allItems"></param>
        protected virtual void Initialize(IList<T> allItems)
        {
            foreach (T item in allItems)
            {
                // Store by id.
                _allItemsById[item.Id] = item;

                // Now store by name.
                string namedKey = item.BuildKey();
                _allItemsByName[namedKey] = item;
            }
        }
    }
}
