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
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Entities;


namespace ComLib.Entities
{
    /// <summary>
    /// Repository factory for building/initializing a entity specific repository.
    /// </summary>
    public class RepositoryFactory
    {
        private static Action<string, IRepositoryConfigurable> _configurator;
        private static RepositoryConfiguratorDefault _defaultConfigurator = new RepositoryConfiguratorDefault();

        /// <summary>
        /// Static initializer.
        /// </summary>
        static RepositoryFactory()
        {
            Init();
        }


        /// <summary>
        /// Initialize the repository configurator.
        /// </summary>
        /// <param name="configurator">Instance of repository configurator to use.</param>
        public static void Init(Action<string, IRepositoryConfigurable> configurator)
        {
            _configurator = configurator;
        }


        /// <summary>
        /// Initialize the repository configurator using only the connection string.
        /// This results in using the RepositoryConfiguratorDefault and the default database provider "System.Data.SqlClient".
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        public static void Add(string connectionString)
        {
            Add(new ConnectionInfo(connectionString));
        }


        /// <summary>
        /// Initialize the repository configurator using only the connection provided.
        /// This results in using the RepositoryConfiguratorDefault.
        /// </summary>
        /// <param name="connection">Database connection info.</param>
        public static void Add(ConnectionInfo connection)
        {
            _defaultConfigurator.Add(connection);
        }


        /// <summary>
        /// Add connection associated w/ key.
        /// </summary>
        /// <param name="id">Connection key.</param>
        /// <param name="connectionInfo">Connection information.</param>
        public static void Add(string id, ConnectionInfo connectionInfo)
        {
            _defaultConfigurator.Add(id, connectionInfo);
        }


        /// <summary>
        /// Default initialization.
        /// </summary>
        private static void Init()
        {            
            _configurator = new Action<string, IRepositoryConfigurable>(_defaultConfigurator.Configure);
        }


        /// <summary>
        /// Initialize the repository.
        /// </summary>
        /// <typeparam name="T">Type of repository items.</typeparam>
        /// <param name="action">Action that creates a repository.</param>
        /// <returns>Created repository.</returns>
        public static IRepository<T> Create<T>(Func<IRepository<T>> action) where T: IEntity
        {
            // Allow Entity to create repository but initialize it here.
            IRepository<T> repository = action();
            _configurator(null, repository);
            return repository;
        }


        /// <summary>
        /// Initialize the repository.
        /// </summary>
        /// <param name="connectionId">Connection id.</param>
        /// <param name="repository">Repository to initialize.</param>
        public static void Configure(string connectionId, IRepositoryConfigurable repository)
        {
            // Configure the repository but initialize it here.
            _configurator(connectionId, repository);
        }
    }



    /// <summary>
    /// Default repository configurator to set the connection connection string and dbhelper.
    /// </summary>
    public class RepositoryConfiguratorDefault 
    {
        private IDictionary<string, ConnectionInfo> _connections = new Dictionary<string, ConnectionInfo>();
        private Func<string, string> _connectionFetcher = null;
        private bool _hasConnectionFetcher = false;


        /// <summary>
        /// The connection object.
        /// </summary>
        public ConnectionInfo DefaultConnection { get; set; }


        /// <summary>
        /// Initialize the connection.
        /// </summary>
        public RepositoryConfiguratorDefault()
        {
        }


        /// <summary>
        /// Initialize the connection.
        /// </summary>
        /// <param name="connection">Connection information.</param>
        public RepositoryConfiguratorDefault(ConnectionInfo connection)
        {
            Add(connection);
        }


        /// <summary>
        /// Initialize the connection.
        /// </summary>
        /// <param name="connection">Connection information.</param>
        public void Add(ConnectionInfo connection)
        {
            DefaultConnection = connection;
            _connections["default"] = connection;
        }


        /// <summary>
        /// Initialize the connection.
        /// </summary>
        /// <param name="id">Connection key.</param>
        /// <param name="connection">Connection information.</param>
        public void Add(string id, ConnectionInfo connection)
        {
            _connections[id] = connection;

            if (string.Compare(id, "default", true) == 0)
                DefaultConnection = connection;
        }


        /// <summary>
        /// Configure the repository with the connection and dbhelper.
        /// </summary>
        /// <param name="connectionId">Connection key.</param>
        /// <param name="repository">Repository to configure.</param>
        public void Configure(string connectionId, IRepositoryConfigurable repository)
        {
            ConnectionInfo con = null;

            // No connection Id? Use default.
            if (string.IsNullOrEmpty(connectionId))
            {
                con = DefaultConnection;
                
            }
            else if (_hasConnectionFetcher)
            {
                var constr = _connectionFetcher(connectionId);
                con = new ConnectionInfo(constr);
            }
            else if (!_connections.ContainsKey(connectionId))
                throw new ArgumentException("Connection Id : " + connectionId + " does not exist.");
            else
            {
                con = _connections[connectionId];
            }

            repository.Connection = con;
            repository.Database = new Database(con);
        }
    }
}
