using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading;

using NUnit.Framework;
using ComLib;
using ComLib.Collections;
using ComLib.Caching;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class CacheManagerTest
    {
        private ICache GetCache(bool usePrefix, string prefix)
        {
            CacheAspNet cache = new CacheAspNet(new CacheSettings(){ UsePrefix = usePrefix, PrefixForCacheKeys = prefix});
            return cache;
        }


        private ICache GetCache()
        {
            return GetCache(false, string.Empty);
        }



        [Test]
        public void CanGetCacheItem()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            string val = cache.Get<string>("kishore");

            Assert.AreEqual(val, "yeahbaby");
        }


        [Test]
        public void CanGetCacheCount()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            cache.Insert("1", new IndexSpan(1, 10));

            Assert.AreEqual(2, cache.Count);
        }


        [Test]
        public void CanClearCache()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            cache.Insert("1", new IndexSpan(1, 10));

            Assert.AreEqual(2, cache.Count);

            cache.Clear();
            Assert.AreEqual(0, cache.Count);
        }


        [Test]
        public void CanRemoveItem()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            cache.Insert("1", new IndexSpan(1, 10));
            Assert.AreEqual(2, cache.Count);

            cache.Remove("kishore");

            Assert.AreEqual(1, cache.Count);

            string name = cache.Get<string>("kishore");
            Assert.IsNull(name);
        }


        [Test]
        public void CanAddPriorityBasedCacheItem()
        {
            ICache cache = GetCache();
            cache.Insert("kishore", "yeahbaby", 2000, false, CacheItemPriority.Low);
            Assert.AreEqual(1, cache.Count);            
        }


        [Test]
        public void CanGetCacheKeys()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            cache.Insert("1", new IndexSpan(1, 10));
            cache.Insert(new IndexSpan(2, 5), "indexspanUsedAsKey");

            ICollection keys = cache.Keys;
            IEnumerator enumerator = keys.GetEnumerator();
            IDictionary<string, bool> keysList = new Dictionary<string, bool>();
            while (enumerator.MoveNext())
            {
                object key = enumerator.Current;
                keysList.Add(key as string, true);                
            }

            Assert.IsTrue(keysList.ContainsKey("kishore"));
            Assert.IsTrue(keysList.ContainsKey("1"));
            Assert.IsTrue(keysList.ContainsKey("ComLib.Collections.IndexSpan"));
        }


        [Test]
        public void CanGetCacheItemDescriptions()
        {
            ICache cache = GetCache();
            cache.Clear();

            cache.Insert("kishore", "yeahbaby");
            cache.Insert("1", new IndexSpan(1, 10));
            
            IList<CacheItemDescriptor> descriptors = cache.GetDescriptors();

            IDictionary<string, CacheItemDescriptor> descriptorMap = new Dictionary<string, CacheItemDescriptor>();
            foreach (CacheItemDescriptor descriptor in descriptors)
            {
                descriptorMap.Add(descriptor.Key, descriptor );
            }
            Assert.IsTrue(descriptorMap.ContainsKey("kishore"));
            Assert.IsTrue(descriptorMap.ContainsKey("1"));
            Assert.AreEqual("System.String", descriptorMap["kishore"].ItemType);
            Assert.AreEqual("ComLib.Collections.IndexSpan", descriptorMap["1"].ItemType);
        }
    }
}
