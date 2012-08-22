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

using ComLib;
using ComLib.Entities;
using ComLib.Data;


namespace ComLib.NamedQueries
{
    /// <summary>
    /// Generic repository for persisting NamedQuery.
    /// </summary>
    public partial class NamedQueryRepository : RepositorySql<NamedQuery> //EntityRepositoryInMemory<NamedQuery>  //
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public NamedQueryRepository() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="db">Database instance.</param>
        public NamedQueryRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }
    }


    
    /// <summary>
    /// RowMapper for NamedQuery.
    /// </summary>
    public partial class NamedQueryRowMapper : EntityRowMapper<NamedQuery>, IEntityRowMapper<NamedQuery>
    {
        /// <summary>
        /// Mapes reader information to named query properties.
        /// </summary>
        /// <param name="reader">Reader with data.</param>
        /// <param name="rowNumber">Data row number.</param>
        /// <returns>Instance of named query with data from reader.</returns>
        public override NamedQuery MapRow(IDataReader reader, int rowNumber)
        {
            NamedQuery entity = NamedQuery.New();
            entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
            entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
            entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
            entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
            entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
            entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
            entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
            entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
            entity.Sql = reader["Sql"] == DBNull.Value ? string.Empty : reader["Sql"].ToString();
            entity.Parameters = reader["Parameters"] == DBNull.Value ? string.Empty : reader["Parameters"].ToString();
            entity.IsStoredProcedure = reader["IsStoredProcedure"] == DBNull.Value ? false : (bool)reader["IsStoredProcedure"];
            entity.IsPagingSuppored = reader["IsPagingSuppored"] == DBNull.Value ? false : (bool)reader["IsPagingSuppored"];
            entity.IsScalar = reader["IsScalar"] == DBNull.Value ? false : (bool)reader["IsScalar"];
            entity.OrderId = reader["OrderId"] == DBNull.Value ? 0 : (int)reader["OrderId"];
            entity.ItemType = reader["ItemType"] == DBNull.Value ? string.Empty : reader["ItemType"].ToString();
            entity.Roles = reader["Roles"] == DBNull.Value ? string.Empty : reader["Roles"].ToString();

            return entity;
        }
    }
}