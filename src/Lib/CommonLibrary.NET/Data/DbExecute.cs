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
    public static class DbExecute
    {
        /// <summary>
        /// Execute a non-query with a single output value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbHelper"></param>
        /// <param name="commandText">E.g. Storedprocedure : Posts_DeleteExpired</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="outputParamName">The name of the output parameter. E.g. @TotalRows</param>
        /// <param name="dbParameters">Input parameters.</param>
        /// <returns></returns>
        public static T Execute<T>(this IDBHelper dbHelper, string commandText, CommandType commandType, DbParameter[] dbParameters, string outputParamName )
        {
            IDictionary<string, object> results = Execute(dbHelper, commandText, commandType, dbParameters, new string[] { outputParamName });
            return (T)results[outputParamName];
        }


        /// <summary>
        /// Execute a non-query with a single input parameter.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="commandText">E.g. Storedprocedure : Posts_DeleteByUser</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="paramName">E.g. "@UserName"</param>
        /// <param name="paramType">E.g. DbType.String</param>
        /// <param name="paramValue">E.g. "kreddy"</param>
        /// <returns></returns>
        public static int Execute(this IDBHelper dbHelper, string commandText, CommandType commandType, string paramName, DbType paramType, object paramValue)
        {
            DbParameter parameter = dbHelper.BuildInParam(paramName, paramType, paramValue);
            return dbHelper.ExecuteNonQuery(commandText, CommandType.StoredProcedure, parameter);
        }


        /// <summary>
        /// Executes the non-query and returns a dictionary of all the output parameters.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="commandText">StoredProc : Posts_PerformActivation</param>
        /// <param name="commandType">StoredProc</param>
        /// <param name="dbParameters">Array of all input parameters to the proc.</param>
        /// <param name="outputParamNames">Array of output parameter names.
        /// e.g. "@TotalRecordsUpdated", @TotalProductTypesUpdated"</param>
        /// <returns></returns>
        public static IDictionary<string, object> Execute(this IDBHelper dbHelper, string commandText, CommandType commandType,
            DbParameter[] dbParameters, string[] outputParamNames)
        {
            DbConnection connection = dbHelper.GetConnection();
            DbCommand command = dbHelper.GetCommand(connection, commandText, CommandType.StoredProcedure);

            // Check if parameters supplied.
            if (dbParameters != null && dbParameters.Length > 0)
            {
                command.Parameters.AddRange(dbParameters);
            }

            // Create list of parameter results.
            IDictionary<string, object> outputResults = new Dictionary<string, object>();

            using (connection)
            {
                connection.Open();
                int queryResult = command.ExecuteNonQuery();

                // Get each output parameter name.
                for (int ndx = 0; ndx < outputParamNames.Length; ndx++)
                {
                    string outputParamName = outputParamNames[ndx];
                    object outputResult = command.Parameters[outputParamName].Value;
                    outputResults[outputParamName] = outputResult;
                }
                command.Dispose();
            }
            return outputResults;
        }   
    }
}
