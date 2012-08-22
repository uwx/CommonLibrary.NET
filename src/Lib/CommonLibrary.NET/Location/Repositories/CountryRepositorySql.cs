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



namespace ComLib.LocationSupport
{
    /// <summary>
    /// Generic repository for persisting Country.
    /// </summary>
    public partial class CountryRepository : RepositorySql<Country>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComLib.NamedQueries.NamedQueryRepository"/> class.
        /// </summary>
        public CountryRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public  CountryRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        public CountryRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="db"></param>
        public CountryRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new CountryRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Country Create(Country entity)
        {
            string sql = "insert into Countrys ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [CountryCode]"
			 + ", [Name], [Abbreviation], [AliasRefName], [IsActive], [IsAlias], [AliasRefId] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @CountryCode"
			 + ", @Name, @Abbreviation, @AliasRefName, @IsActive, @IsAlias, @AliasRefId );" + IdentityStatement;;
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
        public override Country Update(Country entity)
        {
            string sql = "update Countrys set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [CountryCode] = @CountryCode"
			 + ", [Name] = @Name, [Abbreviation] = @Abbreviation, [AliasRefName] = @AliasRefName, [IsActive] = @IsActive, [IsAlias] = @IsAlias, [AliasRefId] = @AliasRefId where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        /// <summary>
        /// Creates db parameters for entity persistence.
        /// </summary>
        /// <param name="entity">Entity for which to create db parameters.</param>
        /// <returns>Array with db parameters for entity.</returns>
        protected virtual DbParameter[] BuildParams(Country entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@CountryCode", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CountryCode) ? "" : entity.CountryCode));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Abbreviation", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Abbreviation) ? "" : entity.Abbreviation));
			dbparams.Add(BuildParam("@AliasRefName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.AliasRefName) ? "" : entity.AliasRefName));
			dbparams.Add(BuildParam("@IsActive", SqlDbType.Bit, entity.IsActive));
			dbparams.Add(BuildParam("@IsAlias", SqlDbType.Bit, entity.IsAlias));
			dbparams.Add(BuildParam("@AliasRefId", SqlDbType.Int, entity.AliasRefId));

            return dbparams.ToArray();
        }


        /// <summary>
        /// Creates a db parameter.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="dbType">Type of parameter.</param>
        /// <param name="val">Value of parameter.</param>
        /// <returns>Created db parameter.</returns>
        protected virtual DbParameter BuildParam(string name, SqlDbType dbType, object val)
        {
            var param = new SqlParameter(name, dbType);
            param.Value = val;
            return param;
        }

    }


    
    /// <summary>
    /// RowMapper for Country.
    /// </summary>
    public partial class CountryRowMapper : EntityRowMapper<Country>, IEntityRowMapper<Country>
    {
        /// <summary>
        /// Creates an instance of Country from data of a data reader.
        /// </summary>
        /// <param name="reader">Reader with data.</param>
        /// <param name="rowNumber">Number of row with data.</param>
        /// <returns>Created instance of Country.</returns>
        public override Country MapRow(IDataReader reader, int rowNumber)
        {
            Country entity =  _entityFactoryMethod == null ? new Country() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.CountryCode = reader["CountryCode"] == DBNull.Value ? string.Empty : reader["CountryCode"].ToString();
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Abbreviation = reader["Abbreviation"] == DBNull.Value ? string.Empty : reader["Abbreviation"].ToString();
			entity.AliasRefName = reader["AliasRefName"] == DBNull.Value ? string.Empty : reader["AliasRefName"].ToString();
			entity.IsActive = reader["IsActive"] == DBNull.Value ? false : (bool)reader["IsActive"];
			entity.IsAlias = reader["IsAlias"] == DBNull.Value ? false : (bool)reader["IsAlias"];
			entity.AliasRefId = reader["AliasRefId"] == DBNull.Value ? 0 : (int)reader["AliasRefId"];

            return entity;
        }
    }
}