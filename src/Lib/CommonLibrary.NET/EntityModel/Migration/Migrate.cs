using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace ComLib.Entities.Migrations
{
    /*
    public class Migrate
    {
        private static IDataMigrator _dataMigrator;
        private static ISchemaMigrator _schemaMigrator;


        public static IDataMigrator Data
        {
            get { return _dataMigrator; }
        }


        public static ISchemaMigrator Schema 
        {
            get { return _schemaMigrator; }
        }
    }



    public interface IDataMigrator
    {
        void DeleteAdd<T>(IList<T> items, params Expression<Func<T, object>>[] condition);       
        void Add<T>(IList<T> items, params Expression<Func<T,object>>[] condition);
        void Up<T>(IList<T> items);
        void Up<T>(IList<T> items, Expression<Func<T, object>> onColumnPredicate);
        void Up<T>(IList<T> items, params Expression<Func<T, object>>[] onColumnsPredicates);
        void Down<T>(Expression<Func<T, object>> onColumn, params T[] inValues);
    }



    public interface ISchemaMigrator
    {
        void Up<T>();
        void Up<T>(IList<T> items, Expression<Func<T, object>> onColumnPredicate);
        void Up<T>(IList<T> items, params Expression<Func<T, object>>[] onColumnsPredicates);
    }
    */
}
