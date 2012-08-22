using System;
using System.Collections.Generic;
using System.Text;


using NUnit.Framework;
using ComLib.Collections;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class IndexedLookUpTests
    {        
        private IndexedLookUp<int, PageData> CreateLookUp(IList<PageData> items)
        {            
            return new IndexedLookUp<int,PageData>(items);
        }


        private IList<PageData> BuildList()
        {
            IList<PageData> items = new List<PageData>();
            items.Add(new PageData(1, "News.aspx"));
            items.Add(new PageData(2, "AboutUs.aspx"));
            items.Add(new PageData(3, "FAQ.aspx"));
            return items;
        }


        [Test]
        public void CanInitialize()
        {
            IList<PageData> items = BuildList();
            IndexedLookUp<int, PageData> lookup = CreateLookUp(items);
            Assert.IsTrue(lookup.Count == 3);
        }


        [Test]
        public void CanLookUpById()
        {
            IList<PageData> items = BuildList();
            IndexedLookUp<int, PageData> lookup = CreateLookUp(items);

            PageData page = lookup[2];
            Assert.AreEqual(2, page.Id);
        }


        [Test]
        public void CanLookUpByNamedKey()
        {
            IList<PageData> items = BuildList();
            IndexedLookUp<int, PageData> lookup = CreateLookUp(items);

            string key = items[2].BuildKey();
            PageData page = lookup[key];
            Assert.AreEqual(items[2], page);
        }
    }



    public class PageData : IIndexedComponent<int>
    {
        private int _id;
        private string _name;


        public PageData(int id, string name)
        {
            _id = id;
            _name = name;
        }


        public int Id
        {
            get { return _id; }
        }


        public string BuildKey()
        {
            return _name.ToLower();
        }
    }
}
