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

using ComLib;
using ComLib.Entities;
using ComLib.Data;


namespace ComLib.Account
{
    /// <summary>
    /// Generic repository for persisting User.
    /// </summary>
    public partial class UserRepository : RepositorySql<User>  
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UserRepository() { }


        /// <summary>
        /// Initializes a new instance using connection string supplied.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public UserRepository(string connectionString)
            : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance using a connection info.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        public UserRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {
        }


        /// <summary>
        /// Initializes a new instance using a connection info and database helper.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="db">The helper.</param>
        public UserRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Set the rowmapper
        /// </summary>
        /// <param name="connectionInfo">connection info</param>
        /// <param name="db">database helper</param>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new UserRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity">Instance of user.</param>
        /// <returns>Passed instance of user with updated id.</returns>
        public override User Create(User entity)
        {
            string sql = "insert into Users ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [UserName]"
             + ", [UserNameLowered], [Email], [EmailLowered], [Password], [Roles], [MobilePhone]"
             + ", [SecurityQuestion], [SecurityAnswer], [Comment], [IsApproved], [IsLockedOut], [LockOutReason]"
             + ", [LastLoginDate], [LastPasswordChangedDate], [LastPasswordResetDate], [LastLockOutDate] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @UserName"
             + ", @UserNameLowered, @Email, @EmailLowered, @Password, @Roles, @MobilePhone"
             + ", @SecurityQuestion, @SecurityAnswer, @Comment, @IsApproved, @IsLockedOut, @LockOutReason"
             + ", @LastLoginDate, @LastPasswordChangedDate, @LastPasswordResetDate, @LastLockOutDate );" + IdentityStatement; ;
            var dbparams = BuildParams(entity);
            object result = _db.ExecuteScalarText(sql, dbparams);
            entity.Id = Convert.ToInt32(result);
            return entity;
        }


        /// <summary>
        /// Update the entity using sql.
        /// </summary>
        /// <param name="entity">Instance of user.</param>
        /// <returns>Passed instance of user.</returns>
        public override User Update(User entity)
        {
            string sql = "update Users set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [UserName] = @UserName"
             + ", [UserNameLowered] = @UserNameLowered, [Email] = @Email, [EmailLowered] = @EmailLowered, [Password] = @Password, [Roles] = @Roles, [MobilePhone] = @MobilePhone"
             + ", [SecurityQuestion] = @SecurityQuestion, [SecurityAnswer] = @SecurityAnswer, [Comment] = @Comment, [IsApproved] = @IsApproved, [IsLockedOut] = @IsLockedOut, [LockOutReason] = @LockOutReason"
             + ", [LastLoginDate] = @LastLoginDate, [LastPasswordChangedDate] = @LastPasswordChangedDate, [LastPasswordResetDate] = @LastPasswordResetDate, [LastLockOutDate] = @LastLockOutDate where Id = " + entity.Id;
            var dbparams = BuildParams(entity);
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        /// <summary>
        /// Builds the params.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Array with database parameters for user.</returns>
        protected virtual DbParameter[] BuildParams(User entity)
        {
            var dbparams = new List<DbParameter>();
            dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
            dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
            dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
            dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
            dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
            dbparams.Add(BuildParam("@UserName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UserName) ? "" : entity.UserName));
            dbparams.Add(BuildParam("@UserNameLowered", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UserNameLowered) ? "" : entity.UserNameLowered));
            dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
            dbparams.Add(BuildParam("@EmailLowered", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.EmailLowered) ? "" : entity.EmailLowered));
            dbparams.Add(BuildParam("@Password", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Password) ? "" : entity.Password));
            dbparams.Add(BuildParam("@Roles", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Roles) ? "" : entity.Roles));
            dbparams.Add(BuildParam("@MobilePhone", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.MobilePhone) ? "" : entity.MobilePhone));
            dbparams.Add(BuildParam("@SecurityQuestion", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.SecurityQuestion) ? "" : entity.SecurityQuestion));
            dbparams.Add(BuildParam("@SecurityAnswer", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.SecurityAnswer) ? "" : entity.SecurityAnswer));
            dbparams.Add(BuildParam("@Comment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Comment) ? "" : entity.Comment));
            dbparams.Add(BuildParam("@IsApproved", SqlDbType.Bit, entity.IsApproved));
            dbparams.Add(BuildParam("@IsLockedOut", SqlDbType.Bit, entity.IsLockedOut));
            dbparams.Add(BuildParam("@LockOutReason", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.LockOutReason) ? "" : entity.LockOutReason));
            dbparams.Add(BuildParam("@LastLoginDate", SqlDbType.DateTime, entity.LastLoginDate));
            dbparams.Add(BuildParam("@LastPasswordChangedDate", SqlDbType.DateTime, entity.LastPasswordChangedDate));
            dbparams.Add(BuildParam("@LastPasswordResetDate", SqlDbType.DateTime, entity.LastPasswordResetDate));
            dbparams.Add(BuildParam("@LastLockOutDate", SqlDbType.DateTime, entity.LastLockOutDate));

            return dbparams.ToArray();
        }


        /// <summary>
        /// Creates a parameter.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="dbType">Type of parameter.</param>
        /// <param name="val">Value of parameter.</param>
        /// <returns>Created parameter.</returns>
        protected virtual DbParameter BuildParam(string name, SqlDbType dbType, object val)
        {
            var param = new SqlParameter(name, dbType);
            param.Value = val;
            return param;
        }
    }


    
    /// <summary>
    /// RowMapper for User.
    /// </summary>
    public partial class UserRowMapper : EntityRowMapper<User>, IEntityRowMapper<User>
    {
        /// <summary>
        /// Creates a new entity from data of a reader.
        /// </summary>
        /// <param name="reader">Reader with data.</param>
        /// <param name="rowNumber">Row number to use.</param>
        /// <returns>Created instance of entity.</returns>
        public override User MapRow(IDataReader reader, int rowNumber)
        {
            User entity = User.New();
            entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
            entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
            entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
            entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
            entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
            entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
            entity.UserName = reader["UserName"] == DBNull.Value ? string.Empty : reader["UserName"].ToString();
            entity.UserNameLowered = reader["UserNameLowered"] == DBNull.Value ? string.Empty : reader["UserNameLowered"].ToString();
            entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
            entity.EmailLowered = reader["EmailLowered"] == DBNull.Value ? string.Empty : reader["EmailLowered"].ToString();
            entity.Password = reader["Password"] == DBNull.Value ? string.Empty : reader["Password"].ToString();
            entity.Roles = reader["Roles"] == DBNull.Value ? string.Empty : reader["Roles"].ToString();
            entity.MobilePhone = reader["MobilePhone"] == DBNull.Value ? string.Empty : reader["MobilePhone"].ToString();
            entity.SecurityQuestion = reader["SecurityQuestion"] == DBNull.Value ? string.Empty : reader["SecurityQuestion"].ToString();
            entity.SecurityAnswer = reader["SecurityAnswer"] == DBNull.Value ? string.Empty : reader["SecurityAnswer"].ToString();
            entity.Comment = reader["Comment"] == DBNull.Value ? string.Empty : reader["Comment"].ToString();
            entity.IsApproved = reader["IsApproved"] == DBNull.Value ? false : (bool)reader["IsApproved"];
            entity.IsLockedOut = reader["IsLockedOut"] == DBNull.Value ? false : (bool)reader["IsLockedOut"];
            entity.LockOutReason = reader["LockOutReason"] == DBNull.Value ? string.Empty : reader["LockOutReason"].ToString();
            entity.LastLoginDate = reader["LastLoginDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["LastLoginDate"];
            entity.LastPasswordChangedDate = reader["LastPasswordChangedDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["LastPasswordChangedDate"];
            entity.LastPasswordResetDate = reader["LastPasswordResetDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["LastPasswordResetDate"];
            entity.LastLockOutDate = reader["LastLockOutDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["LastLockOutDate"];

            return entity;
        }
    }
}