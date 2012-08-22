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
using System.Data;
using System.Globalization;
using System.Text;

using System.Xml;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;



namespace ComLib.Data
{
    /// <summary>
    /// Class containing various helper methods for accessing data.
    /// </summary>
    public class Database : IDatabase
    {
        /// <summary>
        /// Stores connection info.
        /// </summary>
        protected ConnectionInfo _connection;


        /// <summary>
        /// Database provider factory.
        /// </summary>
        protected DbProviderFactory _factory;


        /// <summary>
        /// Factory creation flag.
        /// </summary>
        protected bool _createFactory = true;


        /// <summary>
        /// Database settings.
        /// </summary>
        protected DbSettings _settings;


        /// <summary>
        /// Default construction
        /// </summary>
        public Database()
            : this(ConnectionInfo.Default, new DbSettings(), null)
        {
        }


        /// <summary>
        /// Construct using only the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public Database(string connectionString)
            : this(new ConnectionInfo(connectionString))
        {
        }


        /// <summary>
        /// Initialize w/ connection string and provider.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="provider"></param>
        public Database(string connectionString, string provider)
            : this(new ConnectionInfo(connectionString, provider))
        {
        }


        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="connection"></param>
        public Database(ConnectionInfo connection)
            : this(connection, new DbSettings(), null)
        {
        }


        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="factory"></param>
        public Database(string connectionString, DbProviderFactory factory)
        {
            ConnectionInfo conn = new ConnectionInfo(connectionString);
            Init(conn, new DbSettings(), factory);
        }


        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="settings"></param>
        /// <param name="factory"></param>
        public Database(ConnectionInfo connection, DbSettings settings, DbProviderFactory factory)
        {
            Init(connection, settings, factory);
        }


        /// <summary>
        /// Initialize this dbhelper.
        /// </summary>
        /// <param name="connection">The connection information.</param>
        /// <param name="settings">Optional settings for this instance of DbHelper.</param>
        /// <param name="factory">Optional instance of the provider factory.
        /// e.g. If using oracle, supply this as OracleClientFactory.Instance</param>
        public void Init(ConnectionInfo connection, DbSettings settings, DbProviderFactory factory)
        {
            _connection = connection;
            _settings = settings == null ? new DbSettings() : settings;
            _createFactory = factory == null;
            _factory = factory;
            InitFactory();
        }


        /// <summary>
        /// Database settings
        /// </summary>
        public DbSettings Settings
        {
            get { return _settings; }
        }


        #region Public database execute methods
        /// <summary>
        /// Execute the datareader.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="action"></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// IDataReader = db.ExecuteReader("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public void ExecuteReaderText(string commandText, Action<IDataReader> action, params DbParameter[] dbParameters)
        {
            ExecuteReader(commandText, CommandType.Text, action, dbParameters);
        }


        /// <summary>
        /// Execute the datareader.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="action">The lamda to call with the reader as a parameter.</param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// IDataReader = db.ExecuteReader("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public void ExecuteReaderProc(string commandText, Action<IDataReader> action, params DbParameter[] dbParameters)
        {
            ExecuteReader(commandText, CommandType.StoredProcedure, action, dbParameters);
        }

        /// <summary>
        /// Execute the datareader.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="action"></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// IDataReader = db.ExecuteReader("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public void ExecuteReader(string commandText, CommandType commandType, Action<IDataReader> action, params DbParameter[] dbParameters)
        {
            Execute<bool>(commandText, commandType, dbParameters, command =>
            {
                IDataReader reader = command.ExecuteReader();
                action(reader);
                reader.Close();
                return true;
            });
        }


        /// <summary>
        /// Executes a non-query using sql text
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.ExecuteNonQuery("update users set isactive =1", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public int ExecuteNonQueryText(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, dbParameters);
        }


        /// <summary>
        /// Executes a non-query using command
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.ExecuteNonQuery("update users set isactive =1", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public int ExecuteNonQueryProc(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteNonQuery(commandText, CommandType.StoredProcedure, dbParameters);
        }


        /// <summary>
        /// Executes a non-query using command
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.ExecuteNonQuery("update users set isactive =1", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, CommandType commandType, params DbParameter[] dbParameters)
        {
            return Execute<int>(commandText, commandType, dbParameters, command => { return command.ExecuteNonQuery(); });
        }


        /// <summary>
        /// Executes a scalar query using sql text.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// object obj = db.ExecuteScalar("select count(*) from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public object ExecuteScalarText(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteScalar(commandText, CommandType.Text, dbParameters);
        }


        /// <summary>
        /// Executes a scalar query using a stored procedure.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// object obj = db.ExecuteScalar("select count(*) from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public object ExecuteScalarProc(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteScalar(commandText, CommandType.StoredProcedure, dbParameters);
        }


        /// <summary>
        /// Executes a scalar query.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// object obj = db.ExecuteScalar("select count(*) from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType, params DbParameter[] dbParameters)
        {
            return Execute<object>(commandText, commandType, dbParameters, command => command.ExecuteScalar());
        }


        /// <summary>
        /// executes
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataTable tbl = db.ExecuteDataTable("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataTable ExecuteDataTableText(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteDataTable(commandText, CommandType.Text, dbParameters);
        }


        /// <summary>
        /// executes
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataTable tbl = db.ExecuteDataTable("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataTable ExecuteDataTableProc(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteDataTable(commandText, CommandType.StoredProcedure, dbParameters);
        }


        /// <summary>
        /// executes
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataTable tbl = db.ExecuteDataTable("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, params DbParameter[] dbParameters)
        {
            return Execute<DataTable>(commandText, commandType, dbParameters,
                (command) =>
                {
                    IDataReader reader = command.ExecuteReader();
                    DataTable table = new DataTable();
                    table.Load(reader);
                    return table;
                });
        }


        /// <summary>
        /// Executes the stored proc command and returns a dataset.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataSet set = db.ExecuteDataSet("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataSet ExecuteDataSetText(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteDataSet(commandText, CommandType.Text, dbParameters);
        }


        /// <summary>
        /// Executes the stored proc command and returns a dataset.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataSet set = db.ExecuteDataSet("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataSet ExecuteDataSetProc(string commandText, params DbParameter[] dbParameters)
        {
            return ExecuteDataSet(commandText, CommandType.StoredProcedure, dbParameters);
        }


        /// <summary>
        /// Executes the stored proc command and returns a dataset.
        /// </summary>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// DataSet set = db.ExecuteDataSet("select * from users", CommandType.Text, null);
        /// </example>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, params DbParameter[] dbParameters)
        {
            return Execute<DataSet>(commandText, commandType, dbParameters,
                (command) =>
                {
                    DbDataAdapter adapter = _factory.CreateDataAdapter();
                    adapter.SelectCommand = command;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    return dataSet;
                });
        }
        #endregion


        #region Public getter/setters
        /// <summary>
        /// Get/Set the connection object.
        /// This is specifically made as a setter to
        /// 1. Allow dependency injection
        /// 2. Allow connections to multiple database with multiple instances of DbHelper.
        /// </summary>
        public ConnectionInfo Connection
        {
            get { return _connection; }
            set
            {
                _connection = value;

                // Set the DbFactory.
                InitFactory();
            }
        }
        #endregion


        #region public Connection helpers
        /// <summary>
        /// Get database provider factory so caller can create dbparams, etc.
        /// </summary>
        /// <returns></returns>
        public DbProviderFactory GetFactory()
        {            
            DbProviderFactory factory = DbProviderFactories.GetFactory(_connection.ProviderName);
            return factory;
        }


        /// <summary>
        /// Create a connection to the database.
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory(_connection.ProviderName);
            DbConnection con = factory.CreateConnection();
            con.ConnectionString = _connection.ConnectionString;
            return con;
        }


        /// <summary>
        /// Create a dbCommand given the DbConnection.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbCommand GetCommand(DbConnection con, string commandText, CommandType commandType)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.CommandType = commandType;
            cmd.CommandText = commandText;
            return cmd;
        }
        #endregion


        #region Database Parameter building
        /// <summary>
        /// Creates a DbParameter object from the arguments and adds it to the ctx command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="val"></param>
        public DbParameter BuildInParam(string paramName, DbType dbType, object val)
        {
            // Parameter.
            DbParameter param = _factory.CreateParameter();
            param.ParameterName = _settings.ParamPrefix + paramName;
            param.DbType = dbType;
            param.Value = val;
            return param;
        }


        /// <summary>
        /// Creates a DbParameter object from the arguments and adds it to the ctx command.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        /// <param name="val"></param>
        public void AddInParam(IList<DbParameter> parameters, string paramName, DbType dbType, object val)
        {
            parameters.Add(BuildInParam(paramName, dbType, val));
        }


        /// <summary>
        /// Creates a DbParameter object from the arguments and adds it to the ctx command.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        public DbParameter BuildOutParam(string paramName, DbType dbType)
        {
            // Parameter.
            DbParameter param = _factory.CreateParameter();
            param.ParameterName = _settings.ParamPrefix + paramName;
            param.DbType = dbType;
            param.Direction = ParameterDirection.Output;
            return param;
        }


        /// <summary>
        /// Creates a DbParameter object from the arguments and adds it to the ctx command.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="paramName"></param>
        /// <param name="dbType"></param>
        public void BuildOutParam(IList<DbParameter> parameters, string paramName, DbType dbType)
        {
            parameters.Add(BuildOutParam(paramName, dbType));
        }


        /// <summary>
        /// Creates a DbParameter object from the arguments and adds it to the ctx command.
        /// </summary>        
        /// <param name="parameters">List of parameters.</param>
        /// <param name="dbParam"></param>
        public void BuildOutParam(IList<DbParameter> parameters, DbParameter dbParam)
        {
            dbParam.Direction = ParameterDirection.Output;
            parameters.Add(dbParam);
        }
        #endregion


        #region Execute Methods
        /// <summary>
        /// A template method to execute any command action.
        /// This is made virtual so that it can be extended to easily include Performance profiling.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <param name="executor"></param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.Execute(int)("Users_GrantAccessToAllUsers", CommandType.StoredProcedure, 
        ///                               null, delegate(DbCommand cmd) { cmd.ExecuteNonQuery(); } );
        /// </example>
        /// <returns></returns>
        public virtual TResult Execute<TResult>(string commandText, CommandType commandType, DbParameter[] dbParameters, Func<DbCommand, TResult> executor)
        {
            return Execute<TResult>(commandText, commandType, dbParameters, false, executor);
        }


        /// <summary>
        /// A template method to execute any command action that is Stored Procedure based.
        /// This is made virtual so that it can be extended to easily include Performance profiling.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="dbParameters">List of parameters</param>
        /// <param name="executor"></param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.Execute(int)("Users_GrantAccessToAllUsers", CommandType.StoredProcedure, 
        ///                               null, delegate(DbCommand cmd) { cmd.ExecuteNonQuery(); } );
        /// </example>
        /// <returns></returns>
        public virtual TResult ExecuteProc<TResult>(string commandText, DbParameter[] dbParameters, Func<DbCommand, TResult> executor)
        {
            return Execute<TResult>(commandText, CommandType.StoredProcedure, dbParameters, false, executor);
        }


        /// <summary>
        /// A template method to execute any command action.
        /// This is made virtual so that it can be extended to easily include Performance profiling.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText">Sql text or StoredProcedure Name. </param>
        /// <param name="commandType"><see cref="System.Data.CommandType"/></param>
        /// <param name="dbParameters">List of parameters</param>
        /// <param name="useTransaction"></param>
        /// <param name="executor"></param>
        /// <example>
        /// IDatabase db = new Database("connectionString value");
        /// int result = db.Execute(int)("Users_GrantAccessToAllUsers", CommandType.StoredProcedure, 
        ///                               null, delegate(DbCommand cmd) { cmd.ExecuteNonQuery(); } );
        /// </example>
        /// <returns></returns>
        public virtual TResult Execute<TResult>(string commandText, CommandType commandType, DbParameter[] dbParameters, bool useTransaction, Func<DbCommand, TResult> executor)
        {
            TResult result = default(TResult);
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connection.ConnectionString;

            try
            {
                using (connection)
                {
                    connection.Open();
                    DbCommand command = connection.CreateCommand();
                    DbTransaction transaction = useTransaction ? connection.BeginTransaction() : null;
                    command.Connection = connection;
                    command.CommandType = commandType;
                    command.CommandText = commandText;
                    command.Transaction = transaction;
                    if (dbParameters != null && dbParameters.Length > 0)
                    {
                        command.Parameters.AddRange(dbParameters);
                    }
                    result = executor(command);

                    // Commit transaction if enabled.
                    if (useTransaction) transaction.Commit();
                }
            }
            finally
            {
                connection.Close();
            }
            return result;
        }


        /// <summary>
        /// Gets a single value from the reader and closes it after reading
        /// the value if the close is true.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        public static object GetSingleValue(IDataReader reader, bool close)
        {
            reader.Read();
            object result = reader.GetValue(0);
            if (close) reader.Close();
            return result;
        }


        /// <summary>
        /// Initialize the database factory.
        /// </summary>
        protected void InitFactory()
        {
            if (_createFactory)
            {
                _factory = DbProviderFactories.GetFactory(_connection.ProviderName);
            }
        }
        #endregion
    }
}
