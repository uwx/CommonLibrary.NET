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
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

using ComLib;
using ComLib.Data;
using ComLib.Entities;


namespace ComLib.Entities
{
    /// <summary>
    /// Repository base class.
    /// </summary>
    /// <typeparam name="T">Type of repository items.</typeparam>
    public abstract class RepositoryBase<T> : RepositoryQueryable, IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// Row mapper used to map objects from datastore back to object.
        /// </summary>
        protected IEntityRowMapper<T> _rowMapper;


        /// <summary>
        /// Callback handler for after rows have been mapped.
        /// </summary>
        protected Action<IList<T>> _onRowsMappedHandler;
        

        #region Properties
        /// <summary>
        /// Entity row mapper.
        /// </summary>
        public virtual IEntityRowMapper<T> RowMapper
        {
            get { return _rowMapper; }
            set { _rowMapper = value; }
        }


        /// <summary>
        /// Callback to use for OnRowsMapped
        /// </summary>
        public virtual Action<IList<T>> OnRowsMappedCallBack { get { return _onRowsMappedHandler; } set { _onRowsMappedHandler = value; } }


        /// <summary>
        /// Stmt used to get the identity of the last inserted row.
        /// </summary>
        public string IdentityStatement
        {
            get { return "select scope_identity();"; }
        }
        #endregion


        #region Crud - Create, Get, Update, Delete
        /// <summary>
        /// Create the entity in the datastore.
        /// </summary>
        /// <param name="entity">Instance of entity.</param>
        /// <returns>Created entity.</returns>
        public abstract T Create(T entity);        


        /// <summary>
        /// Create list of entities.
        /// </summary>
        /// <param name="entities">List of entities.</param>
        public virtual void Create(IList<T> entities)
        {
            foreach (var entity in entities)
                Create(entity);
        }


        /// <summary>
        /// Creates the entities supplied conditionally based on whether it exists in the datastore.
        /// Existance in the datastore is done by finding any entities w/ matching values for the 
        /// <paramref name="checkFields"/> supplied.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="checkFields">Matching filters.</param>
        public virtual void Create(T entity, params Expression<Func<T, object>>[] checkFields)
        {
            if(checkFields == null || checkFields.Length == 0)
            {
                Create(entity);
                return;
            }
                           
            IQuery<T> criteria = Query<T>.New();
            string propName = ExpressionHelper.GetPropertyName<T>(checkFields[0]);
            PropertyInfo prop = entity.GetType().GetProperty(propName);
            object propVal = Reflection.ReflectionUtils.GetPropertyValueSafely(entity, prop);
            criteria.Where(propName).Is(propVal);

            if (checkFields.Length > 1)
            {
                for (int ndx = 1; ndx < checkFields.Length; ndx++)
                {
                    propName = ExpressionHelper.GetPropertyName<T>(checkFields[ndx]);
                    prop = entity.GetType().GetProperty(propName);
                    propVal = Reflection.ReflectionUtils.GetPropertyValueSafely(entity, prop);
                    criteria.And(propName).Is(propVal);
                }
            }
            // Only create if it doesn't exist.
            int count = Count(criteria);
            if (count == 0)
                Create(entity);
        }


        /// <summary>
        /// Copies the entity and returns the copy.
        /// </summary>
        /// <param name="entity">Copy of entity.</param>
        public virtual T Copy(T entity)
        {
            T copy = (T)entity.Clone();
            Create(copy);
            return copy;
        }


        /// <summary>
        /// Get item by id.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        /// <returns>Matching entity.</returns>
        public virtual T Get(object id)
        {
            var sql = "select * from " + _tableName + " where Id = " + id;
            IList<T> entities = _db.QueryNoParams<T>(sql, CommandType.Text, RowMapper);
            if (entities == null || entities.Count == 0)
                return default(T);

            OnRowsMapped(entities);
            return entities[0];
        }


        /// <summary>
        /// Get all the entities from the datastore.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public virtual IList<T> GetAll()
        {
            string sql = string.Format("select * from {0} ", TableName);
            IList<T> result = _db.QueryNoParams<T>(sql, CommandType.Text, RowMapper);
            OnRowsMapped(result);
            return result;
        }


        /// <summary>
        /// Get all the items.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public virtual IList GetAllItems()
        {
            ArrayList list = new ArrayList();
            IList<T> allItems = GetAll();
            foreach (T item in allItems)
                list.Add(item);

            return list;
        }


        /// <summary>
        /// Update the entity in the datastore.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        public abstract T Update(T entity);


        /// <summary>
        /// Create list of entities.
        /// </summary>
        /// <param name="entities">List of entities to create.</param>
        public virtual void Update(IList<T> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }


        /// <summary>
        /// Delete by the entity id.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        public virtual void Delete(object id)
        {
            string sql = string.Format("delete from {0} where Id = {1}", TableName, id);
            _db.ExecuteNonQueryText(sql, null);
        }


        /// <summary>
        /// Delete by the entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        public virtual void Delete(T entity)
        {
            int id = entity.Id;
            Delete(id);
        }


        /// <summary>
        /// Delete all entities from the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            string sql = string.Format("delete from {0}", TableName);
            _db.ExecuteNonQueryText(sql, null);
        }


        /// <summary>
        /// Delete using the expression.
        /// e.g. entity.LogLevel == 1
        /// </summary>
        /// <param name="expression">Expression to use for deletion.</param>
        public virtual void Delete(Expression<Func<T, bool>> expression)
        {
            string filter = RepositoryExpressionHelper.BuildSinglePropertyCondition<T>(expression);
            string sql = string.Format("delete from {0} where {1}", TableName, filter);
            _db.ExecuteNonQueryText(sql, null);
        }
        

        /// <summary>
        /// Delete using the Criteria object.
        /// </summary>
        /// <param name="criteria">Criteria for deletion.</param>
        public virtual void Delete(IQuery criteria)
        {
            var sql = criteria.Builder.BuildConditions(false);
            string fullSql = string.Format("delete from {0} where {1}", TableName, sql);            
            _db.ExecuteNonQueryText(fullSql, null);
        }


        /// <summary>
        /// Delete by named filter
        /// </summary>
        /// <param name="filterName">Criteria for deletion.</param>
        public void DeleteByNamedFilter(string filterName)
        {
            IQuery criteria = GetNamedFilter(filterName, true);
            Delete(criteria);
        }


        /// <summary>
        /// Save the entity.
        /// </summary>
        /// <param name="entity">Entity to save/update.</param>
        /// <returns>Updated entity.</returns>
        public virtual T Save(T entity)
        {
            if (entity.IsPersistant())
                return Update(entity);
            else
                return Create(entity);
        }


        /// <summary>
        /// Create list of entities.
        /// </summary>
        /// <param name="entities">List of entities to create/update.</param>
        public virtual void Save(IList<T> entities)
        {
            foreach (var entity in entities)
                Save(entity);
        }
        #endregion
        

        #region Aggregate - Sum, Min, Max, Count, Avg, Count, Distinct, Group By
        /// <summary>
        /// Sum using expression. e.g. p => p.FirstName;
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Sum of entities.</returns>
        public virtual double Sum(Expression<Func<T, object>> exp)
        {
            return Sum(RepositoryExpressionHelper.GetPropertyName<T>(exp));
        }
        
        
        /// <summary>
        /// Min using expression. e.g. p => p.FirstName;
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Sum of entities.</returns>
        public virtual double Min(Expression<Func<T, object>> exp)
        {
            return Min(RepositoryExpressionHelper.GetPropertyName<T>(exp));
        }


        /// <summary>
        /// Max using expression. e.g. p => p.FirstName;
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Max of entities.</returns>
        public virtual double Max(Expression<Func<T, object>> exp)
        {
            return Max(RepositoryExpressionHelper.GetPropertyName<T>(exp));
        }


        /// <summary>
        /// Sum using expression. e.g. p => p.FirstName;
        /// </summary>
        /// <param name="exp">Expression representing the column name via property name</param>
        /// <returns>Average of entities.</returns>
        public virtual double Avg(Expression<Func<T, object>> exp)
        {
            return Avg(RepositoryExpressionHelper.GetPropertyName<T>(exp));
        }


        /// <summary>
        /// Sum using expression. e.g. p => p.FirstName;
        /// </summary>
        /// <typeparam name="TVal">Type of values to return.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List of distinct values.</returns>
        public virtual List<TVal> Distinct<TVal>(Expression<Func<T, object>> exp)
        {
            string propName = RepositoryExpressionHelper.GetPropertyName<T>(exp);
            return InternalDistinct<TVal>(propName, null);
        }


        /// <summary>
        /// Group(date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List of key/value pairs.</returns>
        public virtual List<KeyValuePair<TGroup, int>> Group<TGroup>(Expression<Func<T, object>> exp)
        {
            string propName = RepositoryExpressionHelper.GetPropertyName<T>(exp);
            return InternalGroup<TGroup>(propName, null);
        }
        #endregion


        #region Increment / Decrement
        /// <summary>
        /// Increments the specified field name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id of the row to increment.</param>
        public virtual void Increment(string fieldName, int by, int id)
        {
            string sql = string.Format("update {0} set {1} = (select {2} from {3} where id = {4}) + {5}", _tableName, fieldName, fieldName, _tableName, id);
            _db.ExecuteNonQueryText(sql);
        }


        /// <summary>
        /// Decrements the specified field name.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id of the row to increment.</param>
        public virtual void Decrement(string fieldName, int by, int id)
        {
            string sql = string.Format("update {0} set {1} = (select {2} from {3} where id = {4}) - {5}", _tableName, fieldName, fieldName, _tableName, id);
            _db.ExecuteNonQueryText(sql);
        }


        /// <summary>
        /// Increments the field specified by the expression.
        /// </summary>
        /// <param name="exp">The fieldname as an expression.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public void Increment(Expression<Func<T, object>> exp, int by, int id)
        {
            string fieldName = ExpressionHelper.GetPropertyName<T>(exp);
            Increment(fieldName, by, id);
        }


        /// <summary>
        /// Decrements the field specified by the expression.
        /// </summary>
        /// <param name="exp">The exp.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public void Decrement(Expression<Func<T, object>> exp, int by, int id)
        {
            string fieldName = ExpressionHelper.GetPropertyName<T>(exp);
            Decrement(fieldName, by, id);
        }
        #endregion


        #region Lookups
        /// <summary>
        /// Create dictionary of all entities using the Id.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        public virtual IDictionary<int, T> ToLookUp()
        {
            return ToLookUp(string.Empty);
        }


        /// <summary>
        /// Create dictionary of all entities using the Id.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        public virtual IDictionary<int, T> ToLookUp(string namedFilter)
        {
            IList<T> all = string.IsNullOrEmpty(namedFilter) ? GetAll() : FindByNamedFilter(namedFilter);
            IDictionary<int, T> lookup = new Dictionary<int, T>();
            foreach (var item in all)
                lookup[item.Id] = item;

            return lookup;
        }


        /// <summary>
        /// Create dictionary of all entities using the Id.
        /// </summary>
        /// <returns>Dictionary with entities.</returns>
        public virtual IDictionary<int, T> ToLookUp(IQuery<T> query)
        {
            IList<T> all = query == null ? Find(string.Empty) : Find(query);
            IDictionary<int, T> lookup = new Dictionary<int, T>();
            foreach (var item in all)
                lookup[item.Id] = item;

            return lookup;
        }


        /// <summary>
        /// Create dictionary of all entities using the Id and the Field name supplied by <paramref name="propName"/>
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="propName">Name of propery.</param>
        /// <returns>List with lookups.</returns>
        public virtual LookupMulti<T> ToLookUpMulti<Id2>(string propName)
        {
            return ToLookUpMulti<Id2>(propName, string.Empty);
        }


        /// <summary>
        /// Lookup on 2 values. Creates lookup on Id and Id2
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List with lookups.</returns>
        public LookupMulti<T> ToLookUpMulti<Id2>(Expression<Func<T, object>> exp)
        {
            string propName = ExpressionHelper.GetPropertyName<T>(exp);
            return ToLookUpMulti<Id2>(propName);
        }


        /// <summary>
        /// Create dictionary of all entities using the Id and the Field name supplied by <paramref name="propName"/>
        /// </summary>
        /// <typeparam name="Id2">Type of id.</typeparam>
        /// <param name="propName">Name of property.</param>
        /// <param name="filterName">Filter name.</param>
        /// <returns>List with lookups.</returns>
        public virtual LookupMulti<T> ToLookUpMulti<Id2>(string propName, string filterName)
        {
            IList<T> all = string.IsNullOrEmpty(filterName) ? GetAll() : FindByNamedFilter(filterName);
            var lookup = new LookupMulti<T>();
            lookup.Init(all, null, item => item.Id, item2 => Reflection.ReflectionUtils.GetPropertyValue(item2, propName) as string, null);
            return lookup;
        }
        #endregion


        #region Find
        /// <summary>
        /// Find all records matching the query string.
        /// </summary>
        /// <param name="sql">SQL to use.</param>
        /// <returns>List of items.</returns>
        public virtual IList<T> Find(string sql)
        {
            return Find(sql, true);
        }


        /// <summary>
        /// Find by query
        /// </summary>
        /// <param name="query">The query used in the find.</param>
        /// <returns>List of matching items.</returns>
        public virtual IList<T> Find(IQuery query)
        {
            query.Data.From = this.TableName;
            string sql = query.Builder.Build();
            return Find(sql, true);
        }


        /// <summary>
        /// Find by named filter : e.g. FindByNamedFilter("PopularPosts");
        /// </summary>
        /// <param name="namedFilter">e.g. "Most Popular Posts"</param>
        /// <returns>List of matching items.</returns>
        public IList<T> FindByNamedFilter(string namedFilter)
        {
            IQuery criteria = GetNamedFilter(namedFilter, true);
            return Find(criteria);
        }


        /// <summary>
        /// Find by filter.
        /// </summary>
        /// <param name="queryString">The query, this can be either just a filter
        /// after the where clause or the entire sql</param>
        /// <param name="isFullSql">Whether or not the query supplid contains select and from clauses.</param>
        /// <returns>List of matching items.</returns>
        public virtual IList<T> Find(string queryString, bool isFullSql)
        {
            string sql = queryString;
            if (!isFullSql)
            {
                string tableName = this.TableName;
                queryString = string.IsNullOrEmpty(queryString) ? string.Empty : " where " + queryString;
                sql = "select * from " + tableName + " " + queryString;
            }
            IList<T> results = _db.QueryNoParams<T>(sql, CommandType.Text, RowMapper);
            OnRowsMapped(results);
            return results;
        }


        /// <summary>
        /// Get items by page
        /// </summary>
        /// <param name="criteria">Criteria object representing filter</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        public virtual PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize)
        {
            string sql = criteria.Builder.BuildConditionsAndOrdering(false, true);
            return Find(sql, pageNumber, pageSize);
        }


        /// <summary>
        /// Get items by page using named filter
        /// </summary>
        /// <param name="filterName">Named filter</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        public PagedList<T> FindByNamedFilter(string filterName, int pageNumber, int pageSize)
        {
            IQuery criteria = GetNamedFilter(filterName, true);
            return Find(criteria, pageNumber, pageSize);
        }


        /// <summary>
        /// Get items by page
        /// </summary>
        /// <param name="sql">Sql to use as filter.</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of matched items by page.</returns>
        public abstract PagedList<T> Find(string sql, int pageNumber, int pageSize);


        /// <summary>
        /// Get items by page based on latest / most recent create date.
        /// </summary>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>List of items in page.</returns>
        public abstract PagedList<T> FindRecent(int pageNumber, int pageSize);


        /// <summary>
        /// Find the recent items and convert them to a different type.
        /// </summary>
        /// <typeparam name="T2">The type to convert to.</typeparam>
        /// <param name="pageNumber">The page number to get.</param>
        /// <param name="pageSize">Number of items in each page.</param>
        /// <returns>List of items in page.</returns>
        public IList<T2> FindRecentAs<T2>(int pageNumber, int pageSize)
        {
            PagedList<T> items = FindRecent(pageNumber, pageSize);
            IList<T2> converted = new List<T2>();
            foreach (object item in items)
            {
                converted.Add((T2)item);
            }
            return converted;
        }
        #endregion


        #region Misc
        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="filter">Filter to match.</param>
        /// <returns>First matched item.</returns>
        public virtual T First(string filter)
        {
            IList<T> result = Find(filter, false);
            if (result == null || result.Count == 0)
                return default(T);

            return result[0];
        }


        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>First matched item.</returns>
        public virtual T First(IQuery criteria)
        {
            criteria.Data.From = this.TableName;
            IList<T> items = Find(criteria.Builder.Build(), true);
            if (items == null || items.Count == 0)
                return default(T);

            return items[0];
        }


        /// <summary>
        /// Get the first item after applying the named filter.
        /// </summary>
        /// <param name="filterName">Named filter.</param>
        /// <returns>First matched item.</returns>
        public T FirstByNamedFilter(string filterName)
        {
            IQuery criteria = GetNamedFilter(filterName, true);
            return First(criteria.Builder.Build());
        }
        #endregion


        #region Helper Methods - Relationships
        /// <summary>
        /// Provides hook for doing additional processing after items have been mapped.
        /// </summary>
        /// <param name="items">List of items.</param>
        protected virtual void OnRowsMapped(IList<T> items)
        {
            if (_onRowsMappedHandler != null)
                _onRowsMappedHandler(items);
        }


        /// <summary>
        /// Get one item in the 1-to-1 relationship.
        /// </summary>
        /// <typeparam name="TRelation">The type of the item in the 1-to-1 relationship.</typeparam>
        /// <param name="filter">Named filter.</param>
        /// <returns>The 1-to-1 relationship.</returns>
        protected virtual TRelation GetOne<TRelation>(string filter)
        {
            var repo = EntityRegistration.GetRepository<TRelation>();
            var item = repo.First(filter);
            return item;
        }


        /// <summary>
        /// Get all the items in the 1-to-many relation
        /// </summary>
        /// <typeparam name="TRelation">The type of the item in the many relationship.</typeparam>
        /// <param name="filter">Named filter.</param>
        /// <returns>List of relations.</returns>
        protected virtual IList<TRelation> GetMany<TRelation>(string filter)
        {
            var repo = EntityRegistration.GetRepository<TRelation>();
            var items = repo.Find(filter, false);
            return items;
        }
        #endregion
    }

}
