using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;
using System.Data;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Account;
using ComLib.Tests.Products;

namespace CommonLibrary.Tests
{
    [TestFixture]
    public class RepositoryFactoryTests
    {
        [Test]
        public void CanDisableConfiguration()
        {
            // Default connection.
            RepositoryFactory.Add(new ConnectionInfo("conn_1"));

            User.Init(() => new UserRepository(), false);

            var repo = User.Repository;
            Assert.AreNotEqual(repo.Database.Connection.ConnectionString, "conn_1");
            Assert.AreNotEqual(repo.Connection.ConnectionString, "conn_1");
            Assert.AreNotEqual(repo.ConnectionString, "conn_1");
        }


        [Test]
        public void CanEnableDefaultConfiguration()
        {
            RepositoryFactory.Add(new ConnectionInfo("conn_1"));
            User.Init(() => new UserRepository(), true);

            var repo = User.Repository;
            Assert.AreEqual(repo.Database.Connection.ConnectionString, "conn_1");
            Assert.AreEqual(repo.Connection.ConnectionString, "conn_1");
            Assert.AreEqual(repo.ConnectionString, "conn_1");
        }


        [Test]
        public void CanEnableCustomConnectionIdConfiguration()
        {
            RepositoryFactory.Add("conn_2", new ConnectionInfo("membership_db"));
            User.Init(() => new UserRepository(), true, "conn_2");

            var repo = User.Repository;
            Assert.AreEqual(repo.Database.Connection.ConnectionString, "membership_db");
            Assert.AreEqual(repo.Connection.ConnectionString, "membership_db");
            Assert.AreEqual(repo.ConnectionString, "membership_db");
        }
    }



    [TestFixture]
    public class RepositoryTests
    {
        bool _useRealRepo = false;

        private IRepository<Product> GetRepo()
        {
            ConnectionInfo conn = new ConnectionInfo("Server=kishore_pc1;Database=testdb;User=testuser1;Password=password;", "System.Data.SqlClient");
            
            IRepository<Product> repo = new RepositoryInMemory<Product>();
            if (_useRealRepo)
                repo = new ProductRepository(conn);

            return repo;
        }


        [Test]
        public void CanCRUD()
        {
            // Create In-Memory repository.
            IRepository<Product> repo = GetRepo();
            var products = GetSampleProducts();
            repo.DeleteAll();
            products.ForEach(p => repo.Create(p));

            // NOTE: I will not show the Sum, Min, Max, Distinct methods here, please refer to base class
            //       example Example_Repository1_Queryable.
            Product product = new Product("Porsche", "boxster", 60, true, DateTime.Now);

            // 1. Create            
            repo.Create(product);
            Assert.IsTrue(product.Id != 0);

            // 2. Get
            product = repo.Get(product.Id);
            Assert.IsNotNull(product);

            // 3. Update 
            product.Make = "Ford2";
            repo.Update(product);
            Assert.IsTrue(repo.Get(product.Id).Make == "Ford2");

            // 4. Delete
            repo.Delete(product.Id);
            Assert.IsNull(repo.Get(product.Id));

            // 5. Get all
            IList<Product> all = repo.GetAll();

            // 6. Get first using filter.
            Product prod = repo.First("Make = 'Honda'");
            Assert.IsTrue(prod != null && prod.Make == "Honda");

            // 7. Get by page using sql
            PagedList<Product> page = repo.Find("Cost = 20", 1, 3);
            foreach (var p in page) Assert.AreEqual(p.Cost, 20);

            // 8. Get by page using criteria.
            var criteria1 = Query<Product>.New().Where("cost").Is(15);
            PagedList<Product> page2 = repo.Find(criteria1, 1, 3);
            foreach (var p in page2) Assert.AreEqual(p.Cost, 15);

            // 9. Delete using criteria.
            var criteria2 = Query<Product>.New().Where(p => p.Make).Is("Toyota");
            var toyotaBefore = repo.Find(criteria2);
            Assert.AreEqual(toyotaBefore.Count, 4);
            repo.Delete(criteria2);
            var toyotaAfter = repo.Find(criteria2);
            Assert.AreEqual(toyotaAfter.Count, 0);
        }


        [Test]
        public void CanAggregate()
        {
            // First create some test data for the queries.            
            IRepository<Product> repoCrud = GetRepo();
            var products = GetSampleProducts();
            repoCrud.DeleteAll();
            products.ForEach(p => repoCrud.Create(p));

            // NOTES:
            // 1. The Repository<T>'s base class is RepositoryQueryable.
            // 2. We are only using a IRepository<T> here initially for adding in-memory test data.
            // 3. But this could be initialized via new RepositoryQuerable(...) constructors against a real Database.
            // 4. Notice that IRepositoryQueryable does not know anything about the entity(Product).
            IRepositoryQueryable repo = (IRepositoryQueryable)repoCrud;

            // 0. Named filters first, to show their example in each section min, max, group, etc.
            repo.AddNamedFilter("InStock Filter", Query.New().Where("IsInStock").Is(true));
            repo.AddNamedFilter("Expensive", Query.New().Where("Cost").MoreThan(20));

            // 1. GROUPING
            var groups = repo.Group<int>("Cost");
            var groups2 = repo.Group<int>("Cost", Query.New().Where("Make").Not("Honda"));
            var groups3 = repo.Group<int>("Cost", "Expensive"); // same as groups2.

            // 2. DISTINCT
            var names = repo.Distinct<string>("Make");
            var names2 = repo.Distinct<string>("Make", Query.New().Where("Cost").MoreThan(30).OrderByDescending("Make"));
            var names3 = repo.Distinct<string>("Make", "Expensive");

            // 3. COUNT
            var count1 = repo.Count();
            var count2 = repo.Count(Query<object>.New().Where("Make").Is("Toyota").And("IsInStock").Is(true));
            var count3 = repo.Count("InStock Filter");

            // 4. MIN, MAX, AVG, SUM
            int count = repo.Count();
            double min = repo.Min("Cost");
            double min2 = repo.Min("Cost", Query.New().Where("Make").In<string>("Bmw", "Lexus"));
            double min3 = repo.Min("Cost", "InStock Filter");

            double max = repo.Max("Cost");
            double max2 = repo.Max("Cost", Query.New().Where("Make").NotIn<string>("Bmw", "Mercedes", "Lexus"));
            double max3 = repo.Max("Cost", "InStock Filter");

            double sum = repo.Sum("Cost");
            double sum2 = repo.Sum("Cost", Query.New().Where("Make").In<string>("Honda", "Toyota").And("IsInStock").Is(true));
            double sum3 = repo.Sum("Cost", "InStock Filter");

            double avg = repo.Avg("Cost");
            double avg2 = repo.Avg("Cost", Query.New().Where("Make").Like("Nissan"));
            double avg3 = repo.Avg("Cost", "InStock Filter");

            // 5. To Table
            DataTable all = repo.ToTable();
            DataTable all2 = repo.ToTable(Query.New().Where("Make").Is("Honda"));
            DataTable all3 = repo.ToTable("InStock Filter");

            // 6. Exists
            bool any1 = repo.Any(Query.New().Where("IsInStock").Is(true));
            bool any2 = repo.Any("InStock Filter");

            // 7. Limit number of records.
            repo.ToTable(Query.New().Limit(2));
            var table1 = repo.ToTable(Query.New().Select("Make", "Model").Where("Cost").MoreThan(30).Limit(2));

            // Check min
            double minLinq = products.Min(p => p.Cost);
            double min2Linq = products.Where(p => (p.Make == "Bmw" || p.Make == "Lexus")).Min(p => p.Cost);
            double min3Linq = products.Where(p => p.IsInStock).Min(p => p.Cost);

            double maxLinq = products.Max(p => p.Cost);
            double max2Linq = products.Where(p => (p.Make != "Bmw" && p.Make != "Mercedes" && p.Make != "Lexus")).Max(p => p.Cost);
            double max3Linq = products.Where(p => p.IsInStock).Max(p => p.Cost);

            double sumLinq = products.Sum(p => p.Cost);
            double sum2Linq = products.Where(p => (p.Make == "Honda" || p.Make == "Toyota") && p.IsInStock == true).Sum(p => p.Cost);
            double sum3Linq = products.Where(p => p.IsInStock).Sum(p => p.Cost);            
            
            double avgLinq = products.Average(p => p.Cost);
            double avg2Linq = products.Where(p => p.Make == "Nissan").Average(p => p.Cost);
            double avg3Linq = products.Where(p => p.IsInStock).Average(p => p.Cost);

            int countLinq = products.Count;
            int count2Linq = products.Where(p => p.Make == "Toyota" && p.IsInStock).Count();
            int count3Linq = products.Where(p => p.IsInStock).Count();

            // Check count
            Assert.AreEqual(count1, countLinq);
            Assert.AreEqual(count2, count2Linq);
            Assert.AreEqual(count3, count3Linq);

            // Check min
            Assert.AreEqual(min, minLinq);
            Assert.AreEqual(min2, min2Linq);
            Assert.AreEqual(min3, min3Linq);

            // Check max
            Assert.AreEqual(max, maxLinq);
            Assert.AreEqual(max2, max2Linq);
            Assert.AreEqual(max3, max3Linq);

            // Check sum
            Assert.AreEqual(sum, sumLinq);
            Assert.AreEqual(sum2, sum2Linq);
            Assert.AreEqual(sum3, sum3Linq);

            // Check avg
            Assert.AreEqual(avg.ToString().Substring(0, 7), avgLinq.ToString().Substring(0, 7));
            Assert.AreEqual(avg2.ToString().Substring(0, 7), avg2Linq.ToString().Substring(0, 7));
            Assert.AreEqual(avg3.ToString().Substring(0, 7), avg3Linq.ToString().Substring(0, 7));
        }


        public void CanRunPostMappingEvents()
        {
            IRepository<Product> repo = new RepositoryInMemory<Product>(){ OnRowsMappedCallBack = (products) =>
            {
                foreach(var product in products) product.Model = "after mapped " + product.Model;
            }};

            Action<IList<Product>> reset = (products) =>
            {
                foreach (var p in products)
                    p.Model = p.Model.Replace("after mapped ", "");
            };

            var allproducts = GetSampleProducts();
            foreach (var product in allproducts)
                repo.Create(product);

            var first = repo.Get(1);
            Assert.IsTrue(first.Model.StartsWith("after mapped"));
            reset(allproducts);

            var all = repo.GetAll();
            foreach (var item in all) Assert.IsTrue(item.Model.StartsWith("after mapped"));
            reset(allproducts);

            var lookup = repo.ToLookUp();
            foreach (var pair in lookup) Assert.IsTrue(pair.Value.Model.StartsWith("after mapped"));
            reset(allproducts);

            var matches = repo.Find(Query<Product>.New().Where(p => p.Cost).Is(15));
            foreach (var match in matches) Assert.IsTrue(match.Model.StartsWith("after mapped"));
        }


        private IList<Product> GetSampleProducts()
        {
            IList<Product> products = new List<Product>()
            {
                new Product("Honda", "Civic",   15, true, DateTime.Today.AddDays(-10)),
                new Product("Honda", "Accord",  20, true, DateTime.Today.AddDays(-9)),
                new Product("Honda", "SUV",     33, true, DateTime.Today.AddDays(-8)),
                new Product("Nissan", "Maxima", 15, true, DateTime.Today.AddDays(-7)),
                new Product("Nissan", "Altima", 20, true, DateTime.Today.AddDays(-6)),
                new Product("Nissan", "SUV",    32, true, DateTime.Today.AddDays(-5)),
                new Product("Toyota", "Solara", 10, true, DateTime.Today.AddDays(-4)),
                new Product("Toyota", "Camry",  20, false, DateTime.Today.AddDays(-3)),
                new Product("Toyota", "SUV",    30, false, DateTime.Today.AddDays(-2)),
                new Product("Toyota", "Prius",  8,  false, DateTime.Today.AddDays(-2)),
                new Product("Mercedes", "Benz", 70, false, DateTime.Today.AddDays(-2)),
                new Product("Ford", "Mustang",  15, true, DateTime.Today.AddDays(-1)),
                new Product("Ford", "Taurus",   20, true, DateTime.Today.AddDays(0)),
                new Product("Ford", "SUV",      28, true, DateTime.Today.AddDays(1)),
                new Product("Bmw", "M3",        45, true, DateTime.Today.AddDays(2)),
                new Product("Lexus", "G37",     40, true, DateTime.Today.AddDays(3)),
            };
            return products;
        }
    }



    [TestFixture]
    public class RepositoryRelationTests
    {



        [Test]
        public void CanLoad1To1()
        {
        }
    }
}
