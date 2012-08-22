/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Data;

using ComLib;
using ComLib.Data;


namespace ComLib.Entities
{    
    /// <summary>
    /// Interface for a Repository pattern to support CRUD and Find operations on some underlying datastore(sql database).
    /// </summary>
    /// <typeparam name="T">Type of entities.</typeparam>
    public interface IRepository<T> : IRepositoryQueryable, IRepositoryConfigurable
    {

        #region CRUD - Create, Get, Update, Delete
        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="entity">Instance of entity.</param>
        /// <returns>Created entity.</returns>
        T Create(T entity);


        /// <summary>
        /// Create a list of entities.
        /// </summary>
        /// <param name="entities">List of entities.</param>
        void Create(IList<T> entities);


        /// <summary>
        /// Creates the entity conditionally based on whether they exists in the datastore.
        /// Existance in the datastore is done by finding any entities w/ matching values for the 
        /// <paramref name="checkFields"/> supplied.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="checkFields">Matching filters.</param>
        void Create(T entity, params Expression<Func<T, object>>[] checkFields);


        /// <summary>
        /// Copies the entity and returns the copy.
        /// </summary>
        /// <param name="entity">Copy of entity.</param>
        T Copy(T entity);


        /// <summary>
        /// Retrieve the entity by it's key/id.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        /// <returns>Matching entity.</returns>
        T Get(object id);


        /*
        /// <summary>
        /// Gets the specified entity and loads the TRelation. Defaults the foreign key to typeof(TRelation).Name + Id.
        /// Defaults the Property name to typeof(TRelation).
        /// </summary>
        /// <typeparam name="TRelation">The type of the relation.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T Get<TRelation>(int id) where TRelation : class;


        /// <summary>
        /// Gets the specified entity and loads the 1-to-Many TRelation. Defaults the foreign key to typeof(TRelation).Name + Id.
        /// Defaults the Property name to typeof(TRelation)"s".
        /// </summary>
        /// <typeparam name="TRelation">The type of the relation.</typeparam>
        /// <param name="id">The id.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        T Get<TRelation>(int id, int pageIndex, int pageSize) where TRelation : class;
        */

        /// <summary>
        /// Retrieve all the entities.
        /// </summary>
        /// <returns>List with all entities.</returns>
        IList<T> GetAll();


        /// <summary>
        /// Retrieve all the entities into a non-generic list.
        /// </summary>
        /// <returns>List with all entities.</returns>
        IList GetAllItems();


        /// <summary>
        /// Update the entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        T Update(T entity);


        /// <summary>
        /// Create a list of entities.
        /// </summary>
        /// <param name="entities">List of entities to create.</param>
        void Update(IList<T> entities);


        /// <summary>
        /// Delete the entitiy by it's key/id.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        void Delete(object id);


        /// <summary>
        /// Delete the entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        void Delete(T entity);


        /// <summary>
        /// Delete using the expression.
        /// e.g. entity.LogLevel == 1
        /// </summary>
        /// <param name="expression">Expression to use for deletion.</param>
        void Delete(Expression<Func<T, bool>> expression);


        /// <summary>
        /// Delete by criteria.
        /// </summary>
        /// <param name="criteria">Criteria for deletion.</param>
        void Delete(IQuery criteria);


        /// <summary>
        /// Delete by criteria.
        /// </summary>
        /// <param name="filterName">Criteria for deletion.</param>
        void DeleteByNamedFilter(string filterName);


        /// <summary>
        /// Delete all entities from the repository.
        /// </summary>
        void DeleteAll();


        /// <summary>
        /// Create or update an entity.
        /// </summary>
        /// <param name="entity">Entity to save/update.</param>
        /// <returns>Updated entity.</returns>
        T Save(T entity);


        /// <summary>
        /// Create or update all entities.
        /// </summary>
        /// <param name="entities">List of entities to create/update.</param>
        void Save(IList<T> entities);
        #endregion


        #region Sum, Min, Max, Count, Avg, Count, Distinct, Group By
        /// <summary>
        /// Get the Sum of the values in the column name represented by the supplied expression
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Sum of entities.</returns>
        double Sum(Expression<Func<T, object>> exp);


        /// <summary>
        /// Get the Min of the values in the column name represented by the supplied expression
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Sum of entities.</returns>
        double Min(Expression<Func<T, object>> exp);


        /// <summary>
        /// Get the Max of the values in the column name represented by the supplied expression
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Max of entities.</returns>
        double Max(Expression<Func<T, object>> exp);


        /// <summary>
        /// Get the Avg of the values in the column name represented by the supplied expression.
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Average of entities.</returns>
        double Avg(Expression<Func<T, object>> exp);
        
        
        /// <summary>
        /// Gets a list of distinct values matched by the supplied expression.
        /// </summary>
        /// <typeparam name="TVal">Type of values to return.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List of distinct values.</returns>
        List<TVal> Distinct<TVal>(Expression<Func<T, object>> exp);
        
        
        /// <summary>
        /// Gets a list of key/value pairs matched by the supplied expression.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List of key/value pairs.</returns>
        List<KeyValuePair<TGroup, int>> Group<TGroup>(Expression<Func<T, object>> exp);        
        #endregion


        #region Increment / Decrement
        /// <summary>
        /// Increments an entity's field.
        /// </summary>
        /// <param name="fieldName">Field to increment.</param>
        /// <param name="by">Increment amount.</param>
        /// <param name="id">Id of entity.</param>
        void Increment(string fieldName, int by, int id);


        /// <summary>
        /// Decrements an entity's field.
        /// </summary>
        /// <param name="fieldName">Field to decrement.</param>
        /// <param name="by">Decrement amount.</param>
        /// <param name="id">Id of entity.</param>
        void Decrement(string fieldName, int by, int id);


        /// <summary>
        /// Increment an entity's field.
        /// </summary>
        /// <param name="exp">Field matching expression.</param>
        /// <param name="by">Increment amount.</param>
        /// <param name="id">Id of entity.</param>
        void Increment(Expression<Func<T, object>> exp, int by, int id);


        /// <summary>
        /// Decrement an entity's field.
        /// </summary>
        /// <param name="exp">Field matching expression.</param>
        /// <param name="by">Decrement amount.</param>
        /// <param name="id">Id of entity.</param>
        void Decrement(Expression<Func<T, object>> exp, int by, int id);
        #endregion


        #region Lookup
        /// <summary>
        /// Create dictionary of all entities using the Id.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        IDictionary<int, T> ToLookUp();


        /// <summary>
        /// Create dictionary of all entities using the Id after applying named filter.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        IDictionary<int, T> ToLookUp(string namedFilter);


        /// <summary>
        /// Create dictionary of all entities using the Id after applying named filter.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        IDictionary<int, T> ToLookUp(IQuery<T> query);


        /// <summary>
        /// Lookup on 2 values. Creates lookup on Id and Id2.
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="propName">Name of propery.</param>
        /// <returns>List with lookups.</returns>
        LookupMulti<T> ToLookUpMulti<Id2>(string propName);


        /// <summary>
        /// Lookup on 2 values. Creates lookup on Id and Id2.
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List with lookups.</returns>
        LookupMulti<T> ToLookUpMulti<Id2>(Expression<Func<T, object>> exp);


        /// <summary>
        /// Lookup on 2 values. Creates lookup on Id and Id2 after applying the named Filter.
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="propName">Name of property.</param>
        /// <param name="filterName">Filter name.</param>
        /// <returns>List with lookups.</returns>
        LookupMulti<T> ToLookUpMulti<Id2>(string propName, string filterName);
        #endregion


        #region Find
        /// <summary>
        /// Find by using the sql string.
        /// </summary>
        /// <param name="sql">SQL to use.</param>
        /// <returns>List of items.</returns>
        IList<T> Find(string sql);


        /// <summary>
        /// Find by query.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List of matching items.</returns>
        IList<T> Find(IQuery criteria);


        /// <summary>
        /// Find by named filter.
        /// </summary>
        /// <param name="namedFilter">e.g. "Most Popular Posts"</param>
        /// <returns>List of matching items.</returns>
        IList<T> FindByNamedFilter(string namedFilter);


        /// <summary>
        /// Find using the sql provided.
        /// </summary>
        /// <param name="sql">SQL to use.</param>
        /// <param name="isFullSql">If true, assumes that the sql contains "select * from table ..."</param>
        /// <returns>List of matching items.</returns>
        IList<T> Find(string sql, bool isFullSql);


        /// <summary>
        /// Find items by page using sql.
        /// </summary>        
        /// <param name="sql">Sql to use for filter</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        PagedList<T> Find(string sql, int pageNumber, int pageSize);
        
        
        /// <summary>
        /// Find items by page using criteria.
        /// </summary>
        /// <param name="criteria">Criteria object representing filter.</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize);


        /// <summary>
        /// Find items by page using named filter.
        /// </summary>
        /// <param name="filterName">Filter to apply.</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        PagedList<T> FindByNamedFilter(string filterName, int pageNumber, int pageSize);


        /// <summary>
        /// Get items by page based on latest / most recent create date.
        /// </summary>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of items in page.</returns>
        PagedList<T> FindRecent(int pageNumber, int pageSize);


        /// <summary>
        /// Find the recent items and convert them to a different type.
        /// </summary>
        /// <typeparam name="T2">Type to convert to.</typeparam>
        /// <param name="pageNumber">Number of page.</param>
        /// <param name="pageSize">Size of page.</param>
        /// <returns>List of items in page.</returns>
        IList<T2> FindRecentAs<T2>(int pageNumber, int pageSize);
        #endregion


        #region Misc
        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="filter">Filter to match.</param>
        /// <returns>First matched item.</returns>
        T First(string filter);


        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="filter">Filter to match.</param>
        /// <returns>First matched item.</returns>
        T First(IQuery filter);


        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="filterName">Filter to match.</param>
        /// <returns>First matched item.</returns>
        T FirstByNamedFilter(string filterName);
        #endregion
    }
}