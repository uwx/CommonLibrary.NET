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



namespace ComLib.StatusUpdater
{
    /// <summary>
    /// Generic repository for persisting StatusUpdate.
    /// </summary>
    public partial class StatusUpdateRepository : RepositorySql<StatusUpdate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComLib.NamedQueries.NamedQueryRepository"/> class.
        /// </summary>
        public StatusUpdateRepository() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusUpdateRepository"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="db">Database to use.</param>
        public StatusUpdateRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
            this.RowMapper = new StatusUpdateRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Entity passed to this method.</returns>
        public override StatusUpdate Create(StatusUpdate entity)
        {
            string sql = "insert into StatusUpdates ( CreateDate, UpdateDate, CreateUser, UpdateUser, UpdateComment" +
            ", Computer, ExecutionUser, BusinessDate, BatchName" +
            ", BatchId, BatchTime, Task, Status, StartTime" +
            ", EndTime, Ref, Comment) " +
            "VALUES (" + "'" + entity.CreateDate.ToString("yyyy-MM-dd") + "'" + "," + "'" + entity.UpdateDate.ToString("yyyy-MM-dd") + "'" + "," + "'" + entity.CreateUser + "'" + "," + "'" + entity.UpdateUser + "'" + "," + "'" + entity.UpdateComment + "'"
             + "," + "'" + entity.Computer + "'" + "," + "'" + entity.ExecutionUser + "'"
             + "," + "'" + entity.BusinessDate.ToString("yyyy-MM-dd") + "'" + "," + "'" + entity.BatchName + "'" + "," + entity.BatchId + "," + "'" + entity.BatchTime.ToString("yyyy-MM-dd") + "'"
             + "," + "'" + entity.Task + "'" + "," + "'" + entity.Status + "'" + "," + "'" + entity.StartTime.ToString("yyyy-MM-dd") + "'" + "," + "'" + entity.EndTime.ToString("yyyy-MM-dd") + "'"
             + "," + "'" + entity.Ref + "'" + "," + "'" + entity.Comment + "'" + ");select scope_identity();";
            object result = _db.ExecuteScalarText(sql, null);
            entity.Id = Convert.ToInt32(result);
            return entity;
        }


        /// <summary>
        /// Update the entity using sql.
        /// </summary>
        /// <param name="entity">Entity to use.</param>
        /// <returns>Entity passed to this method.</returns>
        public override StatusUpdate Update(StatusUpdate entity)
        {
            string sql = "update StatusUpdates set CreateDate = " + "'" + entity.CreateDate.ToString("yyyy-MM-dd") + "'" + ", UpdateDate = " + "'" + entity.UpdateDate.ToString("yyyy-MM-dd") + "'" + ", CreateUser = " + "'" + entity.CreateUser + "'" + ", UpdateUser = " + "'" + entity.UpdateUser + "'" + ", UpdateComment = " + "'" + entity.UpdateComment + "'" + 
            ", Computer = " + "'" + entity.Computer + "'" + ", ExecutionUser = " + "'" + entity.ExecutionUser + "'" + ", BusinessDate = " + "'" + entity.BusinessDate.ToString("yyyy-MM-dd") + "'" + ", BatchName = " + "'" + entity.BatchName + "'" +
            ", BatchId = " + entity.BatchId + ", BatchTime = " + "'" + entity.BatchTime.ToString("yyyy-MM-dd") + "'" + ", Task = " + "'" + entity.Task + "'" + ", Status = " + "'" + entity.Status + "'" + ", StartTime = " + "'" + entity.StartTime.ToString("yyyy-MM-dd") + "'" +
            ", EndTime = " + "'" + entity.EndTime.ToString("yyyy-MM-dd") + "'" + ", Ref = " + "'" + entity.Ref + "'" + ", Comment = " + "'" + entity.Comment + "'" + " where Id = " + entity.Id; ;
            _db.ExecuteNonQueryText(sql, null);
            return entity;
        }
    }



    /// <summary>
    /// RowMapper for StatusUpdate.
    /// </summary>
    public partial class StatusUpdateRowMapper : EntityRowMapper<StatusUpdate>, IEntityRowMapper<StatusUpdate>
    {
        /// <summary>
        /// Maps entity rows to data rows.
        /// </summary>
        /// <param name="reader">Database reader with row info and data.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <returns>Instance of status update created from row.</returns>
        public override StatusUpdate MapRow(IDataReader reader, int rowNumber)
        {
            StatusUpdate entity = StatusUpdates.New();
            entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
            entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
            entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
            entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
            entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
            entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
            entity.Computer = reader["Computer"] == DBNull.Value ? string.Empty : reader["Computer"].ToString();
            entity.ExecutionUser = reader["ExecutionUser"] == DBNull.Value ? string.Empty : reader["ExecutionUser"].ToString();
            entity.BusinessDate = reader["BusinessDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["BusinessDate"];
            entity.BatchName = reader["BatchName"] == DBNull.Value ? string.Empty : reader["BatchName"].ToString();
            entity.BatchId = reader["BatchId"] == DBNull.Value ? 0 : (int)reader["BatchId"];
            entity.BatchTime = reader["BatchTime"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["BatchTime"];
            entity.Task = reader["Task"] == DBNull.Value ? string.Empty : reader["Task"].ToString();
            entity.Status = reader["Status"] == DBNull.Value ? string.Empty : reader["Status"].ToString();
            entity.StartTime = reader["StartTime"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["StartTime"];
            entity.EndTime = reader["EndTime"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["EndTime"];
            entity.Ref = reader["Ref"] == DBNull.Value ? string.Empty : reader["Ref"].ToString();
            entity.Comment = reader["Comment"] == DBNull.Value ? string.Empty : reader["Comment"].ToString();

            return entity;
        }
    }
}