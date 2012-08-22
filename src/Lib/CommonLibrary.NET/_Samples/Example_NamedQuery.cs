using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;


using ComLib.Entities;
using ComLib.Account;
using ComLib;
using ComLib.Application;
using ComLib.NamedQueries;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the NamedQueries namespace.
    /// </summary>
    public class Example_NamedQuery : App
    {
        //private string _creationType = "factory_service";


        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_NamedQuery()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            var query = new NamedQuery();
            query.Name = "Top 10 blog posts";
            query.Description = "Popular blog posts";
            query.ItemType = "text";
            query.Roles = "admin";
            query.Sql = "select top 10 from blogposts order by AvgRating desc";
            query.AddParameters(new List<NamedQueryParam>() { new NamedQueryParam(){ Name = "Count", DefaultValue = "10", IsRequired = false, Typ = typeof(int).Name }});

            NamedQuery.Create(query); 
          
            // To Run the query and get the results table.
            // TO_DO: Not implemented yet.
            // NamedQuery.Execute("top 10 blog posts");

            return BoolMessageItem.True;
        }


        /// <summary>
        /// Configure the ActiveRecord for Accounts
        /// NOTES: - All service objects can be constructed with a repository, validator, settings object.            
        ///        - All service objects can be constructed with a fake repository ( in memory ) for testing.
        /// </summary>
        /// <param name="context"></param>
        public override void Init(object context)
        {
            string columnsToIndex = "Id,UserName,UserNameLowered,Email,EmailLowered,Password";
            IRepository<NamedQuery> repository = new RepositoryInMemory<NamedQuery>(columnsToIndex);
            NamedQuery.Init(() => repository, () => new NamedQueryValidator(), new EntitySettings<NamedQuery>(), false, null);
        }        
    }
}
