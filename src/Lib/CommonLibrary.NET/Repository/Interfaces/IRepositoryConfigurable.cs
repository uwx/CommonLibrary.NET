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
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Data;

using ComLib;
using ComLib.Data;


namespace ComLib.Entities
{    
    /// <summary>
    /// Interface that holds the configuration of a repository.
    /// </summary>
    public interface IRepositoryConfigurable
    {
        /// <summary>
        /// Gets or sets the db helper.
        /// </summary>
        /// <value>The helper.</value>
        IDatabase Database { get; set; }


        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>The connection.</value>
        ConnectionInfo Connection { get; set; }


        /// <summary>
        /// Gets the connection STR.
        /// </summary>
        /// <value>The connection STR.</value>
        string ConnectionString { get; }
    }
}