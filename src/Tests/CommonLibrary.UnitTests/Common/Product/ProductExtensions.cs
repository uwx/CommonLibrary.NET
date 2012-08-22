using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Entities;
using ComLib.Data;


namespace ComLib.Tests.Products
{
    public partial class Product : ActiveRecordBaseEntity<Product>, IEntity
    {
        public Product()
        {
        }


        public Product(string make, string model, int cost, bool isInStock, DateTime stockDate)
        {
            Make = make;
            Model = model;
            Cost = cost;
            IsInStock = isInStock;
            StockDate = stockDate;

            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;
            CreateUser = "kishore";
            UpdateUser = "kishore";
        }
    }



    public class Post : ActiveRecordBaseEntity<Post>, IEntity
    {
        public string Title { get; set; }
        public Category Category { get; set; }
        public IList<Comment> Comments { get; set; }
    }



    public class Category : ActiveRecordBaseEntity<Post>, IEntity
    {
        public string Title { get; set; }
        public int RefId { get; set; }
    }



    public class Comment : ActiveRecordBaseEntity<Post>, IEntity
    {
        public string Content { get; set; }
        public int RefId { get; set; }
    }
}
