using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using NUnit.Framework;

using ComLib.Extensions;
using ComLib;
using ComLib.Logging;
using ComLib.Collections;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class CountableEnumeratorTest
    {
        [Test]
        public void CanIterate()
        {
            Logger.Error("testing", null, null);

            IList<string> names = new List<string>() { "kishore", "sush", "govini" };
            IEnumeratorCountable<string> enumerator = new EnumeratorCountable<string>(names);
            IList<string> names2 = new List<string>();

            // Check that it's not empty.
            Assert.IsFalse(enumerator.IsEmpty);

            // Check that current item is currently empty.
            // Because we havn't advanced the iterator.
            Assert.IsNull(enumerator.Current);
            
            // Iterate.
            while (enumerator.MoveNext())
            {                
                if (enumerator.CurrentIndex == 0)
                    Assert.IsTrue(enumerator.IsFirst());

                if (enumerator.CurrentIndex == enumerator.Count - 1)
                    Assert.IsTrue(enumerator.IsLast());

                names2.Add(enumerator.Current);
            }


            // Confirm getting the values from the .Current property.
            for (int ndx = 0; ndx < names.Count; ndx++)
            {
                Assert.AreEqual(names[ndx], names2[ndx]);
            }
        }
    }


    [TestFixture]
    public class EnumeratorMultiTests
    {
        [Test]
        public void CanIterate()
        {
            IList<string> names = new List<string>() { "kishore", "sush", "govini" };
            IList<string> names2 = new List<string>() { "1", "2", "3" };
            IEnumerator<string> enumerator = new EnumeratorMulti<string>(new List<IEnumerator<string>>() { names.GetEnumerator(), names2.GetEnumerator() });

            // Check that current item is currently empty.
            // Because we havn't advanced the iterator.
            Assert.IsNull(enumerator.Current);

            // Iterate.
            // keep track of item count
            int index = 0;
            while (enumerator.MoveNext())
            {
                
                if (index == 2)
                {
                    Assert.AreEqual("govini", enumerator.Current);
                }
                if (index == 5)
                {
                    Assert.AreEqual("3", enumerator.Current);
                }
                index++;
            }
            Assert.AreEqual(index, 6);
        }
    }


    [TestFixture]
    public class DictionaryReadOnlyTests
    {

        public IDictionary<string, string> CreateDefault()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("pageSize", "1");
            dict.Add("theme", "2");

            return new DictionaryReadOnly<string, string>(dict);
        }


        [Test]
        [ExpectedException()]
        public void CanNotAdd()
        {
            IDictionary<string, string> readOnlyDict = CreateDefault();
            readOnlyDict.Add("new", "value");
        }


        [Test]
        [ExpectedException()]
        public void CanNotChangeViaIndexer()
        {
            IDictionary<string, string> readOnlyDict = CreateDefault();
            readOnlyDict["pageSize"] = "3";
        }


        [Test]
        [ExpectedException()]
        public void CanNotRemove()
        {
            IDictionary<string, string> readOnlyDict = CreateDefault();
            readOnlyDict.Remove("pageSize");
        }
    }



    [TestFixture]
    public class DictionaryOrderedTests
    {

        public DictionaryOrdered<string, string> CreateDefault()
        {
            DictionaryOrdered<string, string> dict = new DictionaryOrdered<string, string>();
            dict.Add("1", "a");
            dict.Add("2", "b");
            dict.Add("3", "c");
            dict.Add("4", "d");

            return dict;
        }


        [Test]
        public void CanGetIndexOfKey()
        {
            DictionaryOrdered<string, string> dict = CreateDefault();
            int ndx = dict.IndexOfKey("3");
            Assert.AreEqual(2, ndx);
        }


        [Test]
        public void CanInsert()
        {
            DictionaryOrdered<string, string> dict = CreateDefault();
            dict.Insert(2, "2a", "b2");
            string val = dict["2a"];
            int ndx = dict.IndexOfKey("2a");
            
            Assert.AreEqual("b2", val);
            Assert.AreEqual(2, ndx);            
        }


        [Test]
        public void CanRemove()
        {
            DictionaryOrdered<string, string> dict = CreateDefault();
            dict.Remove("3");
            bool containsKey = dict.ContainsKey("3");
            int ndx = dict.IndexOfKey("3");
            string valAtIndex = dict[2];

            Assert.IsFalse(containsKey);
            Assert.AreEqual(-1, ndx);
            Assert.AreEqual("d", valAtIndex);            
        }
    }



    [TestFixture]
    public class DictionaryMultiValueTests
    {
        
        public DictionaryMultiValue<string, string> CreateDefault()
        {
            DictionaryMultiValue<string, string> multiValDict = new DictionaryMultiValue<string, string>();
            multiValDict.Add("searchSettings", "pageSize");
            multiValDict.Add("searchSettings", "displayFull");
            multiValDict.Add("globalSettings", "theme");
            multiValDict.Add("globalSettings", "allowRegistration");

            return multiValDict;
        }


        [Test]
        public void CanAdd()
        {
            DictionaryMultiValue<string, string> multiValDict = CreateDefault();
            Assert.AreEqual(2, multiValDict.Count);
        }


        [Test]
        public void CanRetrieveFirstItemUsingIndexer()
        {
            DictionaryMultiValue<string, string> multiValDict = CreateDefault();
            Assert.AreEqual("pageSize", multiValDict["searchSettings"]);
            Assert.AreEqual("theme", multiValDict["globalSettings"]);
        }


        [Test]
        public void CanRetrieveListValues()
        {
            DictionaryMultiValue<string, string> multiValDict = CreateDefault();
            Assert.AreEqual("pageSize", multiValDict.Get("searchSettings")[0]);
            Assert.AreEqual("allowRegistration", multiValDict.Get("globalSettings")[1]);
        }


        [Test]
        public void DoesKeyExist()
        {
            DictionaryMultiValue<string, string> multiValDict = CreateDefault();
            Assert.IsTrue(multiValDict.ContainsKey("searchSettings"));
        }


        [Test]
        public void CanRemoveKey()
        {
            DictionaryMultiValue<string, string> multiValDict = CreateDefault();
            Assert.IsTrue(multiValDict.ContainsKey("searchSettings"));

            // Remove
            multiValDict.Remove("searchSettings");

            Assert.IsFalse(multiValDict.ContainsKey("searchSettings"));
        }
    }


    [TestFixture]
    public class DictionarySetTests
    {
        [Test]
        public void CanDiff()
        {
            DictionarySet<string> set1 = new DictionarySet<string>();
            set1.Add("a");
            set1.Add("b");
            set1.Add("c");

            DictionarySet<string> set2 = new DictionarySet<string>();
            set2.Add("b");
            set2.Add("d");
            set2.Add("e");

            ComLib.Collections.ISet<string> diff = set1.Minus(set2);
            Assert.IsFalse(diff.Contains("d"));
            Assert.IsFalse(diff.Contains("e"));
            Assert.IsFalse(diff.Contains("b"));
            Assert.IsTrue(diff.Contains("a"));
            Assert.IsTrue(diff.Contains("c"));

        }
    }


    [TestFixture]
    public class DictionaryExtensions
    {
        [Test]
        public void CanUseExtensions()
        {
            IDictionary dic = new Dictionary<string, object>();
            dic["name"] = "kishore";
            dic["age"] = 30;
            dic["date"] = DateTime.Today.Date;
            dic["isover21"] = true;
            dic["salary"] = 20.5;
            dic["job"] = new Dictionary<string, object>();
            dic.Section("job")["salary"] = 20.5;
            dic.Section("job")["title"] = "lead dev";

            Assert.AreEqual(dic.Get<string>("name"), "kishore");
            Assert.AreEqual(dic.Get<int>("age"), 30);
            Assert.AreEqual(dic.Get<bool>("isover21"), true);
            Assert.AreEqual(dic.Get<double>("salary"), 20.5);
            Assert.AreEqual(dic.Get<DateTime>("date"), DateTime.Today.Date);

            Assert.AreEqual(dic.Get<double>("job", "salary"), 20.5);
            Assert.AreEqual(dic.Get<string>("job", "title"), "lead dev");

            Assert.AreEqual(dic.Section("job").GetOrDefault<int>("sal", 200000), 200000);
            Assert.AreEqual(dic.Section("job").Get<double>("salary"), 20.5);
        }
    }




    [TestFixture]
    public class PropertyKeyTests
    {
        [Test]
        public void CanParseGroupAndKey()
        {
            PropertyKey key = PropertyKey.Parse("a.b");

            Assert.IsNotNull(key);
            Assert.AreEqual("a", key.Group);
            Assert.AreEqual("b", key.Key);
        }


        [Test]
        public void CanParseGroupSubGroupKey()
        {
            PropertyKey key = PropertyKey.Parse("searchSettings.global.pageSize");

            Assert.IsNotNull(key);
            Assert.AreEqual("searchSettings", key.Group);
            Assert.AreEqual("global", key.SubGroup);
            Assert.AreEqual("pageSize", key.Key);
        }


        [Test]
        public void CanBuildKey()
        {
            PropertyKey key = new PropertyKey("a", string.Empty, "b");            
            Assert.AreEqual("a.b", key.ToString());

            key = new PropertyKey("a", "b", "c");
            Assert.AreEqual("a.b.c", key.ToString());
        }
    }
    
}
