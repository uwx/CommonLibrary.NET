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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using ComLib.Entities;
using ComLib.Data;
using ComLib.LocationSupport;



namespace ComLib.Tests.Products
{
    /// <summary>
    /// Generic repository for persisting Product.
    /// </summary>
    public partial class ProductRepository : RepositorySql<Product>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public ProductRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  ProductRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ProductRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ProductRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new ProductRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Product Create(Product entity)
        {
            string sql = "insert into Products ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Make], [Model], [Cost], [IsInStock], [StockDate] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Make, @Model, @Cost, @IsInStock, @StockDate );" + IdentityStatement;;
            var dbparams = BuildParams(entity);            
            object result = _db.ExecuteScalarText(sql, dbparams);
            entity.Id = Convert.ToInt32(result);
            return entity;
        }


        /// <summary>
        /// Update the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Product Update(Product entity)
        {
            string sql = "update Products set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Make] = @Make, [Model] = @Model, [Cost] = @Cost, [IsInStock] = @IsInStock, [StockDate] = @StockDate where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }

        protected virtual DbParameter[] BuildParams(Product entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Make", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Make) ? "" : entity.Make));
			dbparams.Add(BuildParam("@Model", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Model) ? "" : entity.Model));
			dbparams.Add(BuildParam("@Cost", SqlDbType.Decimal, entity.Cost));
			dbparams.Add(BuildParam("@IsInStock", SqlDbType.Bit, entity.IsInStock));
			dbparams.Add(BuildParam("@StockDate", SqlDbType.DateTime, entity.StockDate));

            return dbparams.ToArray();
        }


        protected virtual DbParameter BuildParam(string name, SqlDbType dbType, object val)
        {
            var param = new SqlParameter(name, dbType);
            param.Value = val;
            return param;
        }

    }


    
    /// <summary>
    /// RowMapper for Product.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class ProductRowMapper : EntityRowMapper<Product>, IEntityRowMapper<Product>
    {
        public override Product MapRow(IDataReader reader, int rowNumber)
        {
            Product entity = Product.New();
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Make = reader["Make"] == DBNull.Value ? string.Empty : reader["Make"].ToString();
			entity.Model = reader["Model"] == DBNull.Value ? string.Empty : reader["Model"].ToString();
			entity.Cost = reader["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Cost"]);
			entity.IsInStock = reader["IsInStock"] == DBNull.Value ? false : (bool)reader["IsInStock"];
			entity.StockDate = reader["StockDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["StockDate"];

            return entity;
        }
    }
}