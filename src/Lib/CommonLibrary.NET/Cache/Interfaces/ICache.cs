
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
using System.Collections;


namespace ComLib.Caching
{
    /// <summary>
    /// Contract for Caching provider
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the number of items in the cache.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Gets a collection of all cache item keys.
        /// </summary>
        ICollection Keys { get; }


        /// <summary>
        /// Whether or not there is a cache entry for the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);


        /// <summary>
        /// Retrieves an item from the cache.
        /// </summary>
        object Get(object key);


        /// <summary>
        /// Retrieves an item from the cache of the specified type.
        /// </summary>
        T Get<T>(object key);


        /// <summary>
        /// Retrieves an item from the cache of the specified type and key and 
        /// inserts by getting it using the lamda it if isn't there
        /// </summary>
        T GetOrInsert<T>(object key, int timeToLiveInSeconds, bool slidingExpiration, Func<T> fetcher);

        
        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        void Remove(object key);


        /// <summary>
        /// Removes collection of items from the cache.
        /// </summary>
        void RemoveAll(ICollection keys);


        /// <summary>
        /// Removes all items from the cache.
        /// </summary>
        void Clear();


        /// <summary>
        /// Inserts an item into the cache.
        /// </summary>
        void Insert(object key, object value);


        /// <summary>
        /// Inserts an item into the cache.
        /// </summary>
        void Insert(object key, object value, int timeToLive, bool slidingExpiration);


        /// <summary>
        /// Inserts an item into the cache.
        /// </summary>
        void Insert(object key, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority);


        /// <summary>
        /// Get description of the cache entries.
        /// </summary>
        /// <returns></returns>
        IList<CacheItemDescriptor> GetDescriptors();
    }
}
