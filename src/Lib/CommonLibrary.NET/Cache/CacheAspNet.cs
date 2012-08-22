
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
using System.Web.Caching;
using System.Web;
using ComLib;


namespace ComLib.Caching
{
    /// <summary>
    /// Cache manager using AspNet which groups all the keys with a prefix.
    /// </summary>
    public class CacheAspNet : ICache
    {
        private Cache _cache;        
        private CacheSettings _settings = new CacheSettings();


        /// <summary>
        /// Initialize with spring cache.
        /// </summary>
        public CacheAspNet()
        {
            Init(new CacheSettings());
        }


        /// <summary>
        /// Initialize using cache settings.
        /// </summary>
        /// <param name="settings"></param>
        public CacheAspNet(CacheSettings settings)
        {
            Init(settings);                      
        }


        /// <summary>
        /// Get the cache settings.
        /// </summary>
        public CacheSettings Settings
        {
            get { return _settings; }
        }


        #region ICache Members
        /// <summary>
        /// Get the number of items in the cache that are 
        /// associated with this instance.
        /// </summary>
        public int Count
        {
            get 
            {
                // If not using prefix. return all.
                if ( !_settings.UsePrefix)
                    return _cache.Count;

                int keyCount = 0;
                foreach (DictionaryEntry entry in _cache)
                {
                    string key = (string)entry.Key;
                    if (key.StartsWith(_settings.PrefixForCacheKeys))
                        keyCount++;
                }
                return keyCount;
            }
        }



        /// <summary>
        /// Whether or not there is a cache entry for the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            string exactKey = BuildKey(key);
            object entry = _cache.Get(exactKey);
            if (entry == null) return false;

            return true;
        }


        /// <summary>
        /// Get the collection of cache keys associated with this instance of
        /// cache ( using the cachePrefix ).
        /// </summary>
        public ICollection Keys
        {
            get
            {                
                IList<string> keys = new List<string>();                
                foreach (DictionaryEntry entry in _cache)
                {
                    string key = (string)entry.Key;

                    if (!_settings.UsePrefix)
                    {
                        keys.Add(key);
                    }
                    else if (key.StartsWith(_settings.PrefixForCacheKeys))
                    {
                        // Add 1 to length to include the separator key "."
                        keys.Add(key.Substring(_settings.PrefixForCacheKeys.Length + 1));
                    }
                }

                return keys as ICollection;
            }
        }


        /// <summary>
        /// Retrieves an item from the cache.
        /// </summary>
        public object Get(object key)
        {
            string actualKey = BuildKey(key);
            return _cache.Get(actualKey);
        }


        /// <summary>
        /// Get the object associated with the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(object key)
        {
            string actualKey = BuildKey(key);
            object obj = _cache.Get(actualKey);
            if (obj == null) { return default(T); }

            return (T)obj;
        }


        /// <summary>
        /// Retrieves an item from the cache of the specified type.
        /// </summary>
        public T GetOrInsert<T>(object key, int timeToLiveInSeconds, bool slidingExpiration, Func<T> fetcher)
        {
            object obj = Get(key);
            if (obj == null)
            {
                T result = fetcher();
                Insert(key, result, timeToLiveInSeconds, slidingExpiration);
                return result;
            }

            return (T)obj;
        }


        /// <summary>
        /// Remove from cache.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            string actualKey = BuildKey(key);
            _cache.Remove(actualKey);
        }


        /// <summary>
        /// Remove all the items associated with the keys specified.
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveAll(ICollection keys)
        {
            foreach (object keyItem in keys)
            {
                string key = BuildKey((string)keyItem);
                _cache.Remove(key);
            }
        }


        /// <summary>
        /// Clear all the items in the cache.
        /// </summary>
        public void Clear()
        {
            ICollection keys = Keys;
            foreach (string key in keys)
            {
                string actualKey = BuildKey(key);
                _cache.Remove(actualKey);
            }
        }


        /// <summary>
        /// Insert an item into the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Insert(object key, object value)
        {
            Insert(key, value, _settings.DefaultTimeToLive, _settings.DefaultSlidingExpirationEnabled, _settings.DefaultCachePriority);
        }


        /// <summary>
        /// Insert an item into the cache with the specified sliding expiration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeToLive"></param>
        /// <param name="slidingExpiration"></param>
        public void Insert(object key, object value, int timeToLive, bool slidingExpiration)
        {
            Insert(key, value, timeToLive, slidingExpiration, _settings.DefaultCachePriority);
        }


        /// <summary>
        /// Insert an item into the cache with the specified time to live, 
        /// sliding expiration and priority.
        /// </summary>
        /// <param name="keyName">The cache key</param>
        /// <param name="value">The cache value</param>
        /// <param name="timeToLive">How long in seconds the object should be cached.</param>
        /// <param name="slidingExpiration">Whether or not to reset the time to live if the object is touched.</param>
        /// <param name="priority">Priority of the cache entry.</param>
        public void Insert(object keyName, object value, int timeToLive, bool slidingExpiration, CacheItemPriority priority)
        {            
            TimeSpan timeToLiveAsTimeSpan = TimeSpan.FromSeconds(timeToLive);
            Guard.IsNotNull(keyName, "key");            
            Guard.IsTrue(TimeSpan.Zero <= timeToLiveAsTimeSpan, "timeToLive");
            System.Web.Caching.CacheItemPriority aspNetPriority = ConvertToAspNetPriority(priority);

            string key = BuildKey(keyName);
            if (TimeSpan.Zero < timeToLiveAsTimeSpan)
            {
                if (slidingExpiration)
                {
                    _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, timeToLiveAsTimeSpan, aspNetPriority, null);
                }
                else
                {
                    DateTime absoluteExpiration = DateTime.Now.AddSeconds(timeToLive);
                    _cache.Insert(key, value, null, absoluteExpiration, Cache.NoSlidingExpiration, aspNetPriority, null);
                }
            }
            else
            {
                _cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, aspNetPriority, null);
            }
        }


        /// <summary>
        /// Returns descriptors for all items in the cache.
        /// </summary>
        /// <returns></returns>
        public IList<CacheItemDescriptor> GetDescriptors()
        {
            IList<CacheItemDescriptor> descriptorList = new List<CacheItemDescriptor>();
            ICollection keys = Keys;
            IEnumerator enumerator = keys.GetEnumerator();

            while (enumerator.MoveNext())
            {
                string key = enumerator.Current as string;
                object cacheItem = Get(key);
                descriptorList.Add(new CacheItemDescriptor(key, cacheItem.GetType().FullName));
            }

            // Sort the cache items by their name
            ((List<CacheItemDescriptor>)descriptorList).Sort(
              delegate(CacheItemDescriptor c1, CacheItemDescriptor c2)
              {
                  return c1.Key.CompareTo(c2.Key);
              });
            return descriptorList;
        }
        #endregion


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="settings"></param>
        private void Init(CacheSettings settings)
        {
            _cache = HttpRuntime.Cache;
            _settings = settings;  
        }


        private string BuildKey(object key)
        {
            if(_settings.UsePrefix )
                return _settings.PrefixForCacheKeys + "." + key.ToString();

            return key.ToString();
        }


        private System.Web.Caching.CacheItemPriority ConvertToAspNetPriority(ComLib.Caching.CacheItemPriority priority)
        {
            if (priority == ComLib.Caching.CacheItemPriority.Default) { return System.Web.Caching.CacheItemPriority.Default; }
            if (priority == ComLib.Caching.CacheItemPriority.High) { return System.Web.Caching.CacheItemPriority.High; }
            if (priority == ComLib.Caching.CacheItemPriority.Low) { return System.Web.Caching.CacheItemPriority.Low; }
            if (priority == ComLib.Caching.CacheItemPriority.Normal) { return System.Web.Caching.CacheItemPriority.Normal; }

            return System.Web.Caching.CacheItemPriority.NotRemovable;
        }
    }
}
