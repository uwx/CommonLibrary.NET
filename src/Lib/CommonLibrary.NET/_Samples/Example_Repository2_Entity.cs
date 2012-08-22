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
    /// Example of Repository usage
    /// NOTES:
    /// 1. IRepository&lt;T&gt; is the interface where T : IEntity.
    ///
    /// 2. RepositorySql(T) is a ms-sql database implementation of IRepository&lt;T&gt;
    ///    What makes the RepositorySql(T) generic is that T must be IEntity
    ///    and all the get / delete methods are based on the field "Id".
    ///
    /// 3. The only thing you need to do to create your repository for eg. BlogRepository
    ///    is a. Extend from RepositorySql(T) and implement the Create() and Update() methods and RowMapper.
    ///    (If you use the codegenerator, this can be automated for you.)!.
    ///    
    /// 4. RepositoryInMemory(T) stores the objects in memory but still provides the CRUD methods.
    ///    This is useful for unit-tests / integration.
    /// 
    /// IEntity
    /// 1. Id
    /// 2. CreateDate
    /// 3. UpdateDate
    /// 4. CreateUser
    /// 5. UpdateUser
    /// 
    /// This convention of using IEntity ( Id ) affords some very convenient pre-built funcionality.
    /// Get, Delete, Recent, etc.
    /// </summary>
    public class Example_Repository2_Entity : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {               
            // Create In-Memory repository.
            IRepository<Product> repo = new RepositoryInMemory<Product>("Id,Name,Model,IsInStock,Cost");
            var products = GetSampleProducts();
            products.ForEach( p => repo.Create(p));

            // NOTE: I will not show the Sum, Min, Max, Distinct methods here, please refer to base class
            //       example Example_Repository1_Queryable.
            Product product = new Product("Porsche", "boxster", 10, true);

            // 1. Create            
            repo.Create(product);

            // 2. Get
            product = repo.Get(product.Id);

            // 3. Update 
            product.Name = "Ford2";
            repo.Update(product);

            // 4. Delete
            repo.Delete(product.Id);

            // 5. Get all
            IList<Product> all = repo.GetAll();

            // 6. Get first using filter.
            Product prod = repo.First("name = 'Honda'");

            // 7. Get by page using sql
            PagedList<Product> page = repo.Find("cost = 20", 1, 3);
           
            // 8. Get by page using criteria.
            var criteria1 = Query<Product>.New().Where("cost").Is(20);
            PagedList<Product> page2 = repo.Find(criteria1, 1, 3);

            // 9. Delete using criteria.
            var criteria2 = Query<Product>.New().Where(p => p.Name).Is("Skateboard");
            repo.Delete(criteria2);

            // 10. Aggregate using column name, expression, and filter. 
            var t = repo.Count(Query<Product>.New().Where(p => p.Name).Null());

            // GROUPING
            var groups1 = repo.Group<int>(e => e.Cost);

            // DISTINCT
            var names = repo.Distinct<string>(e => e.Name);

            // MIN, MAX, AVG, SUM using the Entity properties.
            double min = repo.Min(e => e.Cost);
            double max = repo.Max(e => e.Cost);
            double sum = repo.Sum(e => e.Cost);
            double avg = repo.Avg(e => e.Cost);
            
            return BoolMessageItem.True;
        }


        private IList<Product> GetSampleProducts()
        {
            IList<Product> products = new List<Product>()
            {
                new Product("Honda", "Civic", 15, true),
                new Product("Honda", "Accord", 20, true),
                new Product("Honda", "SUV", 33, true),
                new Product("Nissan", "Maxima", 15, true),
                new Product("Nissan", "Altima", 20, true),
                new Product("Nissan", "SUV", 32, true),
                new Product("Toyota", "Solara", 10, true),
                new Product("Toyota", "Camry", 20, false),
                new Product("Toyota", "SUV", 30, false),
                new Product("Ford", "Mustang", 15, true),
                new Product("Ford", "Taurus", 20, true),
                new Product("Ford", "SUV", 28, true),
                new Product("Bmw", "M3", 40, true),
                new Product("Lexus", "G37", 40, true),
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
            public string Name { get; set; }


            public string Model { get; set; }


            public bool IsInStock { get; set; }


            public Product(string name, string model, int cost, bool isInStock)
            {
                Name = name;
                Model = model;
                Cost = cost;
                IsInStock = isInStock;
            }
        }
    }
}
