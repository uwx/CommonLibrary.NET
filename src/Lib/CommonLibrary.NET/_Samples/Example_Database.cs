using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Reflection;
using System.Collections.Specialized;

//<doc:using>
using ComLib;
using ComLib.Data;
//</doc:using>
using ComLib.Application;
using ComLib.Account;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Data namespace.
    /// </summary>
    public class Example_Database : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
			//<doc:example>
            var db = new Database("Server=kishore_pc1;Database=testdb;User=testuser1;Password=password;", "System.Data.SqlClient");
            
            // 1. Get Datatable using sql text.
            var table = db.ExecuteDataTableText("select * from users", null);

            // 2. Get scalar value using sql text
            var scalar1 = db.ExecuteScalarText("select count(*) from users", null);

            // 3. Get dataset using sql text
            var dataset1 = db.ExecuteDataSetText("select * from users");

            // 4. Non-Query using sql
            db.ExecuteNonQueryText("update users set IsActive = 1");
                        
            // 5. Get data reader.
            db.ExecuteReaderText("select * from users", reader => Console.WriteLine("process reader"));

            // 6. Get a connection
            var con1 = db.GetConnection();

            // 7. Get a command
            var cmd1 = db.GetCommand(db.GetConnection(), "GetUser", CommandType.StoredProcedure);

            // 8. Run execute method with lamda
            var scalar2 = db.Execute<object>("select max(id) from users", CommandType.Text, null, false, cmd => cmd.ExecuteScalar());

            // 9. Query and Map users using RowMapper.
            var userList = db.QueryNoParams<User>("select * from users", CommandType.Text, new UserRowMapper());
            
            // 10. Run in transaction
            
            // 11. Get table using proc and parameters.
            List<DbParameter> args = new List<DbParameter>();
            args.Add(db.BuildInParam("IsActive", DbType.Boolean, 1));
            args.Add(db.BuildInParam("Role", DbType.StringFixedLength, "Moderator"));            
            var table2 = db.ExecuteDataTableProc("GetUsersInRole", args.ToArray());
            
			//</doc:example>
            return BoolMessageItem.True;
        }
    }
}
