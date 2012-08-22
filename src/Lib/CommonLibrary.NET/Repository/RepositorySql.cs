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
    /// Repository for a relational database, base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositorySql<T> : RepositoryBase<T> where T : class, IEntity
    {        
        /// <summary>
        /// Initialize.
        /// </summary>
        public RepositorySql()
        {
            Init(ConnectionInfo.Empty, new Database());
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionString">Initialization connection string.</param>
        public RepositorySql(string connectionString) : this(new ConnectionInfo(connectionString))
        {            
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionInfo">Initialization connection information.</param>
        public RepositorySql(ConnectionInfo connectionInfo)
        {
            var db = new Database(connectionInfo);
            Init(connectionInfo, db);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="connectionInfo">Connection information.</param>
        /// <param name="db">Database.</param>
        public RepositorySql(ConnectionInfo connectionInfo, IDatabase db)
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
            _db = db == null ? new Database(_connectionInfo) : db;
            _db.Connection = _connectionInfo;
            _tableName = typeof(T).Name + "s";
        }        


        #region Crud
        /// <summary>
        /// Create the entity in the datastore.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override T Create(T entity)
        {
            return entity;
        }


        /// <summary>
        /// Update the entity in the datastore.
        /// </summary>
        /// <param name="entity">Entity to create/update.</param>
        /// <returns>Created/updated entity.</returns>
        public override T Update(T entity)
        {
            return entity;
        }
        #endregion


        #region Find 
        /// <summary>
        /// Get page of records using filter.
        /// </summary>
        /// <param name="filter">Filter to apply.</param>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list with matching entities.</returns>
        public override PagedList<T> Find(string filter, int pageNumber, int pageSize)
        {
            string procName = TableName + "_GetByFilter";
            List<DbParameter> dbParams = new List<DbParameter>();
            dbParams.Add(_db.BuildInParam("Filter", System.Data.DbType.String, filter));
            dbParams.Add(_db.BuildInParam("PageIndex", System.Data.DbType.Int32, pageNumber));
            dbParams.Add(_db.BuildInParam("PageSize", System.Data.DbType.Int32, pageSize));
            dbParams.Add(_db.BuildOutParam("TotalRows", System.Data.DbType.Int32));

            Tuple2<IList<T>, IDictionary<string, object>> result = _db.Query<T>(
                procName, System.Data.CommandType.StoredProcedure, dbParams.ToArray(), _rowMapper, new string[] { "@TotalRows" });

            // Set the total records.
            int totalRecords = (int)result.Second["@TotalRows"];
            PagedList<T> pagedList = new PagedList<T>(pageNumber, pageSize, totalRecords, result.First);
            OnRowsMapped(result.First);
            return pagedList;
        }


        /// <summary>
        /// Get recents posts by page.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Paged list with matched entities.</returns>
        public override PagedList<T> FindRecent(int pageNumber, int pageSize)
        {
            string procName = TableName + "_GetRecent";
            List<DbParameter> dbParams = new List<DbParameter>();

            // Build input params to procedure.
            dbParams.Add(_db.BuildInParam("PageIndex", System.Data.DbType.Int32, pageNumber));
            dbParams.Add(_db.BuildInParam("PageSize", System.Data.DbType.Int32, pageSize));
            dbParams.Add(_db.BuildOutParam("TotalRows", System.Data.DbType.Int32));

            Tuple2<IList<T>, IDictionary<string, object>> result = _db.Query<T>(
                procName, System.Data.CommandType.StoredProcedure, dbParams.ToArray(), _rowMapper, new string[] { "@TotalRows" });

            // Set the total records.
            int totalRecords = (int)result.Second["@TotalRows"];
            PagedList<T> pagedList = new PagedList<T>(pageNumber, pageSize, totalRecords, result.First);
            OnRowsMapped(result.First);
            return pagedList;
        }
        #endregion
    }
}
