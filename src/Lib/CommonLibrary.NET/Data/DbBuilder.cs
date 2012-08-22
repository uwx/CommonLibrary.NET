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
    public static class DbBuilder
    {

        #region Command builder
        /// <summary>
        /// Builds a command object with a multiple parameters.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DbCommand BuildCommand(this IDBHelper dbHelper, string commandText, CommandType commandType, DbParameter[] parameters)
        {
            DbConnection connection = dbHelper.GetConnection();
            DbCommand command = dbHelper.GetCommand(connection, commandText, commandType);
            command.Connection = connection;
            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }


        /// <summary>
        /// Builds a command object with a single parameter.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="paramName"></param>
        /// <param name="paramType"></param>
        /// <param name="paramValue"></param>
        /// <returns></returns>
        public static DbCommand BuildCommand(this IDBHelper dbHelper, string commandText, CommandType commandType, string paramName, DbType paramType, object paramValue)
        {
            DbConnection connection = dbHelper.GetConnection();
            DbCommand command = dbHelper.GetCommand(connection, commandText, commandType);
            command.Connection = connection;
            command.Parameters.Add(dbHelper.BuildInParam(paramName, paramType, paramValue));
            return command;
        }
        #endregion
    }
}
