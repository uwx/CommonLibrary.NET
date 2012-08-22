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
using System.Text;
using System.Threading;
using System.Data;
using System.Data.Common;


namespace ComLib.Data
{
    /// <summary>
    /// Class to encapsulate a connection string.
    /// </summary>
    public class ConnectionInfo
    {
        private string _connectionString;
        private string _providerName;

        
        /// <summary>
        /// Default instance.
        /// </summary>
        public static readonly ConnectionInfo Empty = new ConnectionInfo(
            "Server=server1;Database=database1;User=user1;Password=password;", "System.Data.SqlClient");


        /// <summary>
        /// Default instance 2.
        /// </summary>
        public static readonly ConnectionInfo Default = new ConnectionInfo(
            "Server=server1;Database=database1;User=user1;Password=password;", "System.Data.SqlClient");


        /// <summary>
        /// Initialize using the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public ConnectionInfo(string connectionString)
        {
            _connectionString = connectionString;
            _providerName = "System.Data.SqlClient";
        }


        /// <summary>
        /// Initialize using connection string and provider name.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerName"></param>
        public ConnectionInfo(string connectionString, string providerName)
        {
            _connectionString = connectionString;
            _providerName = providerName;
        }


        /// <summary>
        /// Get the connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }
        

        /// <summary>
        /// THe name of the database provider. e.g. "System.Data.SqlClient"
        /// </summary>
        public string ProviderName
        {
            get { return _providerName; }
        }
    }
}
