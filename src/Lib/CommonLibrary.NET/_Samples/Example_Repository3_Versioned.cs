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
    /// Example of an Repository that versions the entities it stores.
    /// This is a decorated that can be used with any underlying repository
    /// </summary>
    public class Example_Repository3_Versioned : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {               
            // Create In-Memory repository.
            IRepository<Product> underlyingRepo = new RepositoryInMemory<Product>(true, "Id,Name,Cost,Version,VersionRefId");
            
            // NOTES:
            // 1. Now create the versionable repo, via decoration.
            // 2. Need to use flag : useOptimizedQueries = false, to enable use of In-Memory Repository.
            // 3. The Entity T must implement the IEntityVersionable interface, see entity below.
            IRepositoryVersionable<Product> repo = new RepositoryVersioned<Product>(underlyingRepo, true, false);
            ((int)4).Times(ndx =>
            {
                // Create more than 1 item w/ same cost, this is for group by functionality.
                int cost = ndx % 2 == 0 ? 10 : 20;
                var newProduct = new Product("Product" + ndx, cost);
                repo.Create(newProduct);
            });

            // 1. Just get the id of the first entity for testing.            
            int id = repo.GetAll()[0].Id;

            // 2. Update it 2 times to create 3 versions
            //   v1 = original, v2 = update 1, v3 = update 2
            var product = repo.Get(id);
            product.Name = product.Name + " update 1";
            repo.Update(product);

            product = repo.Get(id);
            product.Name = product.Name + " update 2";
            repo.Update(product);

            // 3. Get all the versions of the entity.
            var allVersions = repo.GetAllVersions(id);

            // 4. Now rollback the last version.
            repo.RollBack(id);
            var allVersionsAfterRollback = repo.GetAllVersions(id);

            return BoolMessageItem.True;
        }


        private class Product : EntityVersioned
        {
            /// <summary>
            /// Cost of the product.
            /// </summary>
            public int Cost { get; set; }


            /// <summary>
            /// Name of the product
            /// </summary>
            public string Name { get; set; }


            public Product(string name, int cost)
            {
                Name = name;
                Cost = cost;
            }
        }
    }
}
