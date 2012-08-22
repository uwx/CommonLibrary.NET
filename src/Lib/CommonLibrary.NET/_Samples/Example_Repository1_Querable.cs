using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;


using ComLib;
using ComLib.Application;
using ComLib.Data;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Extensions;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of IRepositoryQuerable - generic functionality on any table.
    /// The repository is not associated w/ any specific entity.
    /// </summary>
    public class Example_Repository1_Querable : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {               
            // First create some test data for the queries.            
            IRepository<Product> repoCrud = new RepositoryInMemory<Product>();
            var products = GetSampleProducts();
            products.ForEach( p => repoCrud.Create(p));

            // NOTES:
            // 1. The Repository<T>'s base class is RepositoryQueryable.
            // 2. We are only using a IRepository<T> here initially for adding in-memory test data.
            // 3. But this could be initialized via new RepositoryQuerable(...) constructors against a real Database.
            // 4. Notice that IRepositoryQueryable does not know anything about the entity(Product).
            IRepositoryQueryable repo = (IRepositoryQueryable)repoCrud;

            // 0. Named filters first, to show their example in each section min, max, group, etc.
            repo.AddNamedFilter("InStock Filter", Query<object>.New().Where("IsInStock").Is(true));
            repo.AddNamedFilter("Expensive", Query<object>.New().Where("Cost").MoreThan(20));

            // 1. GROUPING
            var groups = repo.Group<int>("Cost");
            var groups2 = repo.Group<int>("Cost", Query<object>.New().Where("Make").Not("Honda"));
            var groups3 = repo.Group<int>("Cost", "Expensive"); // same as groups2.

            // 2. DISTINCT
            var names = repo.Distinct<string>("Make");
            var names2 = repo.Distinct<string>("Make", Query<object>.New().Where("Cost").MoreThan(30).OrderByDescending("Make"));
            var names3 = repo.Distinct<string>("Make", "Expensive");

            // 3. COUNT
            var count1 = repo.Count();
            var count2 = repo.Count(Query<object>.New().Where("IsInStock").Is(true));
            var count3 = repo.Count("InStock Filter");

            // 4. MIN, MAX, AVG, SUM
            int count = repo.Count();
            double min = repo.Min("Cost");
            double min2 = repo.Min("Cost", Query<object>.New().Where("Make").In<string>("Bmw", "Lexus"));
            double min3 = repo.Min("Cost", "InStock Filter");

            double max = repo.Max("Cost");
            double max2 = repo.Max("Cost", Query<object>.New().Where("Make").NotIn<string>("Bmw", "Lexus"));
            double max3 = repo.Max("Cost", "InStock Filter");

            double sum = repo.Sum("Cost");
            double sum2 = repo.Sum("Cost", Query<object>.New().Where("Make").In<string>("Honda", "Toyota").And("IsInStock").Is(true));
            double sum3 = repo.Sum("Cost", "InStock Filter");

            double avg = repo.Avg("Cost");
            double avg2 = repo.Avg("Cost", Query<object>.New().Where("Make").Like("Nissan"));
            double avg3 = repo.Avg("Cost", "InStock Filter");

            // 5. To Table
            DataTable all = repo.ToTable();
            DataTable all2 = repo.ToTable(Query<object>.New().Where("Make").Is("Honda"));
            DataTable all3 = repo.ToTable("InStock Filter");

            // 6. Exists
            bool any1 = repo.Any(Query<object>.New().Where("IsInStock").Is(true));
            bool any2 = repo.Any("InStock Filter");

            // 7. Limit number of records.
            repo.ToTable(Query<object>.New().Limit(2));
            var table1 = repo.ToTable(Query<object>.New().Select("Make", "Model").Where("Cost").MoreThan(30).Limit(2));
            
            return BoolMessageItem.True;
        }


        private IList<Product> GetSampleProducts()
        {
            IList<Product> products = new List<Product>()
            {
                new Product("Honda", "Civic", 15, true, DateTime.Today.AddDays(-10)),
                new Product("Honda", "Accord", 20, true, DateTime.Today.AddDays(-9)),
                new Product("Honda", "SUV", 33, true, DateTime.Today.AddDays(-8)),
                new Product("Nissan", "Maxima", 15, true, DateTime.Today.AddDays(-7)),
                new Product("Nissan", "Altima", 20, true, DateTime.Today.AddDays(-6)),
                new Product("Nissan", "SUV", 32, true, DateTime.Today.AddDays(-5)),
                new Product("Toyota", "Solara", 10, true, DateTime.Today.AddDays(-4)),
                new Product("Toyota", "Camry", 20, false, DateTime.Today.AddDays(-3)),
                new Product("Toyota", "SUV", 30, false, DateTime.Today.AddDays(-2)),
                new Product("Ford", "Mustang", 15, true, DateTime.Today.AddDays(-1)),
                new Product("Ford", "Taurus", 20, true, DateTime.Today.AddDays(0)),
                new Product("Ford", "SUV", 28, true, DateTime.Today.AddDays(1)),
                new Product("Bmw", "M3", 45, true, DateTime.Today.AddDays(2)),
                new Product("Lexus", "G37", 40, true, DateTime.Today.AddDays(3)),
            };
            return products;
        }


        private class Product : Entity
        {
            /// <summary>
            /// Cost of the product.
            /// </summary>
            public int Cost { get; set; }


            /// <summary>
            /// Name of the product
            /// </summary>
            public string Make { get; set; }


            public string Model { get; set; }


            public bool IsInStock { get; set; }


            public DateTime StockDate { get; set; }


            public Product(string make, string model, int cost, bool isInStock, DateTime stockDate)
            {
                Make = make;
                Model = model;
                Cost = cost;
                IsInStock = isInStock;
                StockDate = stockDate;
            }
        }
    }
}
