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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Linq.Expressions;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Reflection;


namespace ComLib.Entities
{
    /// <summary>
    /// Repository pattern providing CRUD( Create / Retrieve / Update / Delete ) and other methods
    /// using Linq 2 Sql.
    /// 
    /// NOTES:
    /// 1. This is slightly Hybrid version of plain dynamic sql, Linq2Sql, and stored procs
    /// 2. This requires a RowMapper and stored procedures if using the GetByFilter and GetRecent methods
    /// 3. This uses dynamic sql for the Find methods
    /// 4. Linq 2 Sql is used for Create, Update, Get, Delete methods.
    /// 5. Linq 2 Sql itself is a limited solution with the following restrictions:
    /// 
    ///     -a. Can only be used for SQL Server
    ///     -b. Only supports a 1-1 object to table mapping.
    ///     -c. The Update( T entity ) method is a workaround to the inability to attach POCO's( detached entities ) to a different DataContext.
    ///    
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryLinq2Sql<T> : RepositoryBase<T> where T : class, IEntity
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public RepositoryLinq2Sql()
        {
            Init(null, null);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionInfo">Connection information.</param>
        public RepositoryLinq2Sql(ConnectionInfo connectionInfo)
        {
            var db = new Database(connectionInfo);
            Init(connectionInfo, db);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionInfo">Connection information.</param>
        /// <param name="db">Database.</param>
        public RepositoryLinq2Sql(ConnectionInfo connectionInfo, IDatabase db)
        {
            Init(connectionInfo, db);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionInfo">Connection information.</param>
        /// <param name="db">Database.</param>
        public virtual void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            _connectionInfo = connectionInfo;
            _db = db == null ? new Database() : db; 
            
            if( _connectionInfo != null)
                _db.Connection = _connectionInfo;

            _tableName = typeof(T).Name + "s";
        }


        #region Crud
        /// <summary>
        /// Create the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override T Create(T entity)
        {
            Execute((ctx, table) =>
            {
                table.InsertOnSubmit(entity);
                ctx.SubmitChanges(ConflictMode.ContinueOnConflict);
            });
            return entity;
        }


        /// <summary>
        /// Create the entity.
        /// </summary>
        /// <param name="entities">List of entities to create.</param>
        public override void Create(IList<T> entities)
        {
            Execute((ctx, table) =>
            {
                table.InsertAllOnSubmit<T>(entities);
                ctx.SubmitChanges(ConflictMode.ContinueOnConflict);
            });
        }


        /// <summary>
        /// Update the entity.
        /// </summary>
        /// <param name="updatedEntity">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        public override T Update(T updatedEntity)
        {           
            Execute((ctx, table) =>
            {
                // Get the original.
                T original = table.First<T>(m => m.Id.Equals(updatedEntity.Id));

                // Now copy over all updated values from the updated to original.
                // This is the FIX / WORKAROUND for the missing feature in Linq2Sql where you can not attach
                // an entity to the Table.
                CopyValues(updatedEntity, original);

                // Now save the oringal.
                ctx.SubmitChanges(ConflictMode.ContinueOnConflict);                
            });
            return updatedEntity;
        }


        /// <summary>
        /// Get the entity by the id.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        /// <returns>Corresponding entity.</returns>
        public override T Get(object id)
        {
            T first = default(T);
            Execute((ctx, table) => first = table.First<T>(m => m.Id.Equals(id)));
            return first;
        }


        /// <summary>
        /// Get all the entities in the database as a List.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public override IList<T> GetAll()
        {
            IList<T> all = null;
            Execute((ctx, table) => all = table.ToList<T>());
            return all;
        }


        /// <summary>
        /// Delete the entity from the repository.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        public override void Delete(object id)
        {
            Execute((ctx, table) =>
            {
                T entity = table.First<T>(m => m.Id.Equals(id));
                table.DeleteOnSubmit(entity);
                ctx.SubmitChanges(ConflictMode.ContinueOnConflict);
            });
        }
        #endregion


        #region Find
        /// <summary>
        /// Get page of records using filter.
        /// </summary>
        /// <param name="filter">Filter to apply.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list with matching records.</returns>
        public override PagedList<T> Find(string filter, int pageNumber, int pageSize)
        {
            string procName = TableName + "_GetByFilter";
            List<DbParameter> dbParams = new List<DbParameter>();
            dbParams.Add(_db.BuildInParam("@Filter", System.Data.DbType.String, filter));
            dbParams.Add(_db.BuildInParam("@PageIndex", System.Data.DbType.Int32, pageNumber));
            dbParams.Add(_db.BuildInParam("@PageSize", System.Data.DbType.Int32, pageSize));
            dbParams.Add(_db.BuildOutParam("@TotalRows", System.Data.DbType.Int32));

            Tuple2<IList<T>, IDictionary<string, object>> result = _db.Query<T>(
                procName, System.Data.CommandType.StoredProcedure, dbParams.ToArray(), _rowMapper, new string[] { "@TotalRows" });

            // Set the total records.
            int totalRecords = (int)result.Second["@TotalRows"];
            PagedList<T> pagedList = new PagedList<T>(pageNumber, pageSize, totalRecords, result.First);
            return pagedList;
        }


        /// <summary>
        /// Get recents posts by page.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>List with matching records.</returns>
        public override PagedList<T> FindRecent(int pageNumber, int pageSize)
        {
            string procName = TableName + "_GetRecent";
            List<DbParameter> dbParams = new List<DbParameter>();

            // Build input params to procedure.
            dbParams.Add(_db.BuildInParam("@PageIndex", System.Data.DbType.Int32, pageNumber));
            dbParams.Add(_db.BuildInParam("@PageSize", System.Data.DbType.Int32, pageSize));
            dbParams.Add(_db.BuildOutParam("@TotalRows", System.Data.DbType.Int32));

            Tuple2<IList<T>, IDictionary<string, object>> result = _db.Query<T>(
                procName, System.Data.CommandType.StoredProcedure, dbParams.ToArray(), _rowMapper, new string[] { "@TotalRows" });

            // Set the total records.
            int totalRecords = (int)result.Second["@TotalRows"];
            PagedList<T> pagedList = new PagedList<T>(pageNumber, pageSize, totalRecords, result.First);
            return pagedList;
        }
        #endregion


        #region Helpers
        /// <summary>
        /// Execute the executor providing it the datacontext and table.
        /// </summary>
        /// <param name="executor">Action to execute.</param>
        protected void Execute(Action<DataContext, Table<T>> executor)
        {
            var conn = _db.GetConnection();
            var ctx = new DataContext(conn);
            using (ctx)
            {
                conn.Open();
                // Get the LogEventEntity table
                Table<T> table = ctx.GetTable<T>();             
                executor(ctx, table);
                conn.Close();
            }
        }


        /// <summary>
        /// Execute the executor providing it the datacontext only.
        /// </summary>
        /// <param name="executor">Action to execute.</param>
        protected void Execute(Action<DataContext> executor)
        {
            var conn = _db.GetConnection();
            var ctx = new DataContext(conn);
            using (ctx)
            {
                conn.Open();
                executor(ctx);
                conn.Close();
            }
        }


        /// <summary>
        /// Copies all the mapped column property values from the updatedEntity to the original.
        /// </summary>
        /// <param name="updatedEntity">Updated entity.</param>
        /// <param name="original">Original entity.</param>
        protected void CopyValues(T updatedEntity, T original)
        {
            var props = AttributeHelper.GetPropsOnlyWithAttributes<ColumnAttribute>(updatedEntity);
            foreach (var prop in props)
            {
                ReflectionUtils.CopyPropertyValue(updatedEntity, original, prop);
            }
        }
        #endregion
    }
}
