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

using ComLib;
using ComLib.Entities;
using ComLib.Data;



namespace ComLib.Entities
{
    /// <summary>
    /// Repository that versions the Entities on updates.
    /// </summary>
    /// <remarks>
    /// This works via the following:
    /// 1. Decorates any existing Repository you want to use.
    /// 2. The entity T of the repository must implement IEntityVersionable
    /// 3. The Update method will create a new version of the entity without changing it's primary key.
    /// 4. The VersionRefId of historic/older versions becomes the reference to the primary key.
    ///    and the historic/older versions have a different primary key.
    /// </remarks>
    /// <typeparam name="T">Type of items to store.</typeparam>
    public class RepositoryVersioned<T> : RepositoryBase<T>, IRepositoryVersionable<T> where T : class, IEntity
    {
        /// <summary>
        /// Repository of items.
        /// </summary>
        protected IRepository<T> _repo;


        /// <summary>
        /// Versioned flag.
        /// </summary>
        protected bool _isVersioned;


        /// <summary>
        /// Strict versioned flag.
        /// </summary>
        protected bool _isVersionedStrict;


        private bool _useOptimizedQuery;


        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="repo">Repository to use.</param>
        public RepositoryVersioned(IRepository<T> repo) : this(repo, true, true)
        {
        }


        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <param name="repo">Repository to use.</param>
        /// <param name="isVersionedStrict">True to use strict version checking.</param>
        /// <param name="useOptimizedQueries">True to optimize queries.</param>
        public RepositoryVersioned(IRepository<T> repo, bool isVersionedStrict, bool useOptimizedQueries)
        {
            _repo = repo;
            _isVersioned = true;
            _isVersionedStrict = isVersionedStrict;
            _useOptimizedQuery = useOptimizedQueries;
        }


        /// <summary>
        /// Get/set whether this repository is versioned.
        /// </summary>
        public bool IsVersioned { get { return _isVersioned; } set { _isVersioned = value; } }


        /// <summary>
        /// Get/set whether this repository is strictly versioned.
        /// </summary>
        public bool IsVersioningStrict { get { return _isVersionedStrict; } }


        /// <summary>
        /// Roll back the entity w/ the specified id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        public void RollBack(int id)
        {
            var entity = _repo.Get(id);
            RollBack(entity);
        }


        /// <summary>
        /// Rollback the entity to the prior version.
        /// </summary>
        /// <param name="entity">Entity to rollback.</param>
        public void RollBack(T entity)
        {
            // Validation versioning conditions.
            if (!(entity is IEntityVersioned))
                throw new InvalidOperationException("Entity to rollback is not a versioned entity, does not support IEntityVersioned interface");

            if (_isVersioned && _isVersionedStrict && !((IEntityVersioned)entity).IsLatestVersion())
                throw new InvalidOperationException("Entity to rollback is not the latest version.");

            // Use case not supported.
            if (!((IEntityVersioned)entity).IsLatestVersion())
                return;

            int id = ((IEntityVersioned)entity).Id;

            // sql for the next to latest version.
            string optimizedQuery = "VersionRefId = " + id
                       + " and version = select max(version) from " + _repo.TableName
                                       + " where VersionRefId = " + id;

            string sql = optimizedQuery;
            if (!_useOptimizedQuery)
            {
                double versionId = _repo.Max("version", Query<IEntityVersioned>.New().Where(e => e.VersionRefId).Is(id));
                sql = "VersionRefId = " + id
                    + " and version = " + versionId;
            }
            var lastEntity = _repo.First(sql) as IEntityVersioned;
            var lastId = lastEntity.Id;

            // Update the entity by setting it's id ot the original.
            lastEntity.Id = id;
            lastEntity.VersionRefId = -1;
            _repo.Update((T)lastEntity);

            // Delete the last entity.
            _repo.Delete(lastId);
        }


        #region IRepositoryWithId<int,T> Members
        /// <summary>
        /// Get/set the database to use.
        /// </summary>
        public override IDatabase Database
        {
            get { return _repo.Database; }
            set { _repo.Database = value; }
        }


        /// <summary>
        /// Get/set the connection info to use.
        /// </summary>
        public override ConnectionInfo Connection
        {
            get { return _repo.Connection; }
            set { _repo.Connection = value; }
        }

        
        /// <summary>
        /// Get the connection string to use.
        /// </summary>
        public override string ConnectionString
        {
            get { return _repo.ConnectionString; }
        }


        /// <summary>
        /// Get the table name used.
        /// </summary>
        public override string TableName
        {
            get { return _repo.TableName; }
        }


        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override T Create(T entity)
        {
            if (entity is IEntityVersioned)
                ((IEntityVersioned)entity).VersionRefId = -1;

            return _repo.Create(entity);
        }


        /// <summary>
        /// Return an entity by its id.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        /// <returns>Corresponding entity.</returns>
        public override T Get(object id)
        {
            return _repo.Get(id);
        }


        /// <summary>
        /// Returns the latest versions of all entities.
        /// </summary>
        /// <returns>List with latest version of all entities.</returns>
        public override IList<T> GetAll()
        {
            var criteria = Query<IEntityVersioned>.New();
            criteria.Where(e => e.VersionRefId).Is(-1).End();
            return _repo.Find(criteria.Builder.Build(), false);
        }


        /// <summary>
        /// Returns the latest versions of all entities.
        /// </summary>
        /// <returns>List with latest version of all entities.</returns>
        public virtual IList<T> GetAllVersions(int id)
        {
            var criteria = Query<IEntityVersioned>.New();
            criteria.Where(e => e.Id).Is(id).Or( e => e.VersionRefId).Is(id);
            return Find(criteria, false); 
        }


        /// <summary>
        /// Updates the entity while storing historic versions.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        public override T Update(T entity)
        {
            if (!_isVersioned) return _repo.Update(entity);

            // Validation versioning conditions.
            if (!(entity is IEntityVersioned))
                throw new InvalidOperationException("Entity to rollback is not a versioned entity, does not support IEntityVersioned interface");

            if (_isVersionedStrict && !((IEntityVersioned)entity).IsLatestVersion())
                throw new InvalidOperationException("Entity to rollback is not the latest version.");

            var ventity = entity as IEntityVersioned;

            // Check if this is latest version.
            if (!ventity.IsLatestVersion()) return _repo.Update(entity);

            //  The original id.
            var id = ventity.Id;

            // Create historic version by making the VersionRefId = id 
            // instead of -1 on the current version in the database.
            var current = Get(id);
            ((IEntityVersioned)current).VersionRefId = id;
            _repo.Create(current);

            // Now increment the version and make this the lastest version.
            ventity.Version++;
            ventity.VersionRefId = -1;

            // Update as usual.
            return _repo.Update(entity);	
        }


        /// <summary>
        /// Deletes the entity and all it's versions.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        public override void Delete(object id)
        {
            var criteria = Query<IEntityVersioned>.New();
            criteria.Where(e => e.Id).Is(id).Or(e => e.VersionRefId).Is(id);
            _repo.Delete(criteria);
        }


        /// <summary>
        /// Delete an entity that matches an expression.
        /// </summary>
        /// <param name="expression">Matching expression.</param>
        public override void Delete(Expression<Func<T, bool>> expression)
        {
            _repo.Delete(expression);
        }


        /// <summary>
        /// Delete an entity that matches criteria.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        public override void Delete(IQuery criteria)
        {
            _repo.Delete(criteria);
        }


        /// <summary>
        /// Delete all entities.
        /// </summary>
        public override void DeleteAll()
        {
            _repo.DeleteAll();
        }


        /// <summary>
        /// Get the sum of a value of all entities.
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <returns>Sum of entities.</returns>
        public override double Sum(string columnName)
        {
            return _repo.Sum(columnName);
        }


        /// <summary>
        /// Get the sum of a value of entities 
        /// that match an expression.
        /// </summary>
        /// <param name="exp">Matching expression.</param>
        /// <returns>Sum of matching entities.</returns>
        public override double Sum(Expression<Func<T, object>> exp)
        {
            return _repo.Sum(exp);
        }


        /// <summary>
        /// Get the min of a value of all entities.
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <returns>Min value.</returns>
        public override double Min(string columnName)
        {
            return _repo.Min(columnName);
        }


        /// <summary>
        /// Get the min of a value of entities 
        /// that match an expression.
        /// </summary>
        /// <param name="exp">Matching expression.</param>
        /// <returns>Min of matching entities.</returns>
        public override double Min(Expression<Func<T, object>> exp)
        {
            return _repo.Min(exp);
        }


        /// <summary>
        /// Get the max of a value of all entities.
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <returns>Max value.</returns>
        public override double Max(string columnName)
        {
            return _repo.Max(columnName);
        }


        /// <summary>
        /// Get the max of a value of entities 
        /// that match an expression.
        /// </summary>
        /// <param name="exp">Matching expression.</param>
        /// <returns>Max of matching entities.</returns>
        public override double Max(Expression<Func<T, object>> exp)
        {
            return _repo.Max(exp);
        }


        /// <summary>
        /// Get the average of a value of all entities.
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <returns>Average value.</returns>
        public override double Avg(string columnName)
        {
            return _repo.Avg(columnName);
        }


        /// <summary>
        /// Get the average of a value of entities 
        /// that match an expression.
        /// </summary>
        /// <param name="exp">Matching expression.</param>
        /// <returns>Average of matching entities.</returns>
        public override double Avg(Expression<Func<T, object>> exp)
        {
            return _repo.Avg(exp);
        }


        /// <summary>
        /// Get the number of items.
        /// </summary>
        /// <returns></returns>
        public override int Count()
        {
            return _repo.Count();
        }


        /// <summary>
        /// Returns the distinct values of all items.
        /// </summary>
        /// <typeparam name="TVal">Type of value to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <returns>List of distinct values.</returns>
        public override List<TVal> Distinct<TVal>(string columnName)
        {
            return _repo.Distinct<TVal>(columnName);
        }


        /// <summary>
        /// Returns the distinct values of items
        /// that match an expression.
        /// </summary>
        /// <typeparam name="TVal">Type of value to return.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List of distinct values of matching items.</returns>
        public override List<TVal> Distinct<TVal>(Expression<Func<T, object>> exp)
        {
            return _repo.Distinct<TVal>(exp);
        }


        /// <summary>
        /// Returns a grouping of values of items.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <returns>List with key/value pairs of values.</returns>
        public override List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName)
        {
            return _repo.Group<TGroup>(columnName);
        }


        /// <summary>
        /// Returns a grouping of values of items
        /// that match an expression.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="exp">Matching expression.</param>
        /// <returns>List with key/value pairs of values.</returns>
        public override List<KeyValuePair<TGroup, int>> Group<TGroup>(Expression<Func<T, object>> exp)
        {
            return _repo.Group<TGroup>(exp);
        }


        /// <summary>
        /// Finds a list of latest versions of items by using an sql.
        /// </summary>
        /// <param name="sql">Sql to use.</param>
        /// <returns>List of items found.</returns>
        public override IList<T> Find(string sql)
        {
            return _repo.Find(sql);
        }


        /// <summary>
        /// Finds the latest versions, excluding historic ones.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List of matching items.</returns>
        public override IList<T> Find(IQuery criteria)
        {
            return Find(criteria, true);
        }


        /// <summary>
        /// Finds the latest versions, excluding historic ones.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <param name="onlyLatestVersions">True to get only the latest versions.</param>
        /// <returns>List of matching items.</returns>
        public virtual IList<T> Find(IQuery criteria, bool onlyLatestVersions)
        {
            if (onlyLatestVersions)
                criteria.AddCondition("VersionRefId", ExpressionType.Equal, -1);

            return _repo.Find(criteria);
        }


        /// <summary>
        /// Finds the latest versions.
        /// </summary>
        /// <param name="sql">Sql to use.</param>
        /// <param name="isFullSql">True if the sql is complete (not criteria only).</param>
        /// <returns>List of matched items.</returns>
        public override IList<T> Find(string sql, bool isFullSql)
        {
            return _repo.Find(sql, isFullSql);
        }


        /// <summary>
        /// Returns a paged list of items.
        /// </summary>
        /// <param name="sql">Sql to use.</param>
        /// <param name="pageNumber">Page of number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list of items.</returns>
        public override PagedList<T> Find(string sql, int pageNumber, int pageSize)
        {
            return _repo.Find(sql, pageNumber, pageSize);
        }


        /// <summary>
        /// Finds latest versions, excluding historic ones.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list of items.</returns>
        public override PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize)
        {
            criteria.AddCondition("VersionRefId", ExpressionType.Equal, -1);
            return _repo.Find(criteria, pageNumber, pageSize);
        }


        /// <summary>
        /// Get a paged list of most recent items.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list of most recent items.</returns>
        public override PagedList<T> FindRecent(int pageNumber, int pageSize)
        {
            return _repo.FindRecent(pageNumber, pageSize);
        }


        /// <summary>
        /// Gets the first item that matches a filter.
        /// </summary>
        /// <param name="filter">Filter to use.</param>
        /// <returns>Matched item.</returns>
        public override T First(string filter)
        {
            return _repo.First(filter);
        }

        #endregion
    }
}
