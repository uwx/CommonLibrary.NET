using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


using ComLib;
using ComLib.Data;
using ComLib.Categories;
using ComLib.Patterns;


namespace CommonLibrary.Tests
{    

    [TestFixture]
    public class CompositiTests
    {

        [Test]
        public void CanAddChildren()
        {
            Category art = new Category() { Name = "art", Group = "Art" };
            Category sports = new Category() { Name = "sports", Group = "Sports" };
            Category music = new Category() { Name = "music", Group = "Music" };

            art.Add(new Category() { Name = "painting" });
            art.Add(new Category() { Id = 20, Name = "photography",  });
            sports.Add(new Category() { Name = "mma" });
            sports.Children[0].Add(new Category() { Name = "pride" });
            sports.Children[0].Add(new Category() { Name = "ufc" });

            // Test children count.
            Assert.AreEqual(art.Children.Count, 2);
            Assert.AreEqual(art.Count, 2);
            Assert.AreEqual(art.HasChildren, true);
            Assert.AreEqual(sports.HasChildren, true);
            Assert.AreEqual(sports.Children[0].HasChildren, true);
            Assert.AreEqual(sports.Children[0].Children[0].Name, "pride");

            // Test keys.
            Assert.AreEqual(art["painting"].Name, "painting");
            Assert.AreEqual(art[20].Name, "photography");
        }


        [Test]
        public void CanCheckProps()
        {
            Category art = new Category() { Name = "art", Group = "Art" };
            Category sports = new Category() { Name = "sports", Group = "Sports" };
            Category music = new Category() { Name = "music", Group = "Music" };
            Category kungfu = new Category() { Name = "Kung Fu", Group = "Martial Arts" };
            Category judo = new Category() { Name = "Judo", Group = "Martial Arts" };

            art.Add(new Category() { Name = "painting" });
            art.Add(new Category() { Id = 20, Name = "photography" });
            sports.Add(new Category() { Id = 21, Name = "Martial Arts" });
            sports.Children[0].Add(kungfu);
            sports.Children[0].Add(judo);
            
            Assert.IsTrue(art.IsRoot);
            Assert.IsTrue(art.Parent == null);
            Assert.IsTrue(kungfu.Parent != null);
            Assert.IsTrue(kungfu.ParentId == 21);
            Assert.IsFalse(kungfu.IsRoot);
            Assert.IsTrue(judo.ParentId == 21);
            Assert.IsTrue(sports[21].Name == "Martial Arts");
        }


        [Test]
        public void CanBuildLookup()
        {
            IList<Category> categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Name = "Art", Group = "Art" });
            categories.Add(new Category() { Id = 2, Name = "Painting", Group = "Painting", ParentId = 1 });
            categories.Add(new Category() { Id = 3, Name = "Oil", Group = "Oil", ParentId = 2 });
            categories.Add(new Category() { Id = 4, Name = "Water", Group = "Water", ParentId = 2 });
            categories.Add(new Category() { Id = 5, Name = "Sports", Group = "Sports" });
            categories.Add(new Category() { Id = 6, Name = "Baseball", Group = "Sports", ParentId = 5 });
            categories.Add(new Category() { Id = 7, Name = "Football", Group = "Sports", ParentId = 5 });

            CompositeLookup<Category> lookup = new CompositeLookup<Category>(categories);
            Assert.AreEqual(lookup[5].Name, "Sports");
            Assert.AreEqual(lookup["Sports"].Id, 5);
            Assert.AreEqual(lookup["Art,Painting"].Name, "Painting");
            Assert.AreEqual(lookup["Art,Painting,Oil"].Name, "Oil");
            Assert.IsTrue(lookup[2].Children.Count == 2);
            Assert.IsTrue(lookup.RootNodes[1].Name == "Sports");
            Assert.IsTrue(lookup.Children(5).Count == 2);
        }

        [Test]
        public void CanConvertToNodes()
        {
            IList<Category> categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Name = "Art", Group = "Art" });
            categories.Add(new Category() { Id = 2, Name = "Painting", Group = "Painting", ParentId = 1 });
            categories.Add(new Category() { Id = 3, Name = "Oil", Group = "Oil", ParentId = 2 });
            categories.Add(new Category() { Id = 4, Name = "Water", Group = "Water", ParentId = 2 });
            categories.Add(new Category() { Id = 5, Name = "Sports", Group = "Sports" });
            categories.Add(new Category() { Id = 6, Name = "Baseball", Group = "Sports", ParentId = 5 });
            categories.Add(new Category() { Id = 7, Name = "Football", Group = "Sports", ParentId = 5 });

            Node<Category> nodes = Node<Category>.ToNodes<Category>(categories);
            Assert.IsTrue(nodes.HasChildren);

            // art & sports.
            Assert.AreEqual(nodes.Children.Count, 2);

            // art and sports.
            Assert.AreEqual(nodes.Children[0].Item.Name, categories[0].Name);
            Assert.AreEqual(nodes.Children[1].Item.Name, categories[4].Name);

            // Painting
            Assert.AreEqual(nodes.Children[0].Children[0].Item.Name, categories[1].Name);
            Assert.AreEqual(nodes.Children[1].Children[0].Item.Name, categories[5].Name);
        }


        [Test]
        public void CanConvertToNodesWithLookups()
        {
            IList<Category> categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Name = "Art", Group = "Art" });
            categories.Add(new Category() { Id = 2, Name = "Painting", Group = "Painting", ParentId = 1 });
            categories.Add(new Category() { Id = 3, Name = "Oil", Group = "Oil", ParentId = 2 });
            categories.Add(new Category() { Id = 4, Name = "Water", Group = "Water", ParentId = 2 });
            categories.Add(new Category() { Id = 5, Name = "Sports", Group = "Sports" });
            categories.Add(new Category() { Id = 6, Name = "Baseball", Group = "Sports", ParentId = 5 });
            categories.Add(new Category() { Id = 7, Name = "Football", Group = "Sports", ParentId = 5 });

            Node<Category> nodes = Node<Category>.ToNodes<Category>(categories);

            // Get by the id using root node.
            Assert.AreEqual(nodes.Root[1].Item, categories[0]);
            Assert.AreEqual(nodes.Root[6].Item, categories[5]);

            // Get by the name using root node.
            Assert.AreEqual(nodes.Root["Art"].Item, categories[0]);
            Assert.AreEqual(nodes.Root["Sports,Baseball"].Item, categories[5]);
        }
    }
}
