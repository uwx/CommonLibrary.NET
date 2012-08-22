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

namespace CommonLibrary
{
    /// <summary>
    /// Csv reader.
    /// </summary>
    public class CsvDictionaryReader
    {        
    }




    public interface ICsvReader
    {
        /// <summary>
        /// The name of all the fields/columns.
        /// </summary>
        IList<string> FieldNames { get; }


        /// <summary>
        /// Number of columns.
        /// </summary>
        int FieldCount { get; }


        /// <summary>
        /// Number of records.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Get the list of column/values representing at the specified record number.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        IList<string> this[int row] { get; }


        /// <summary>
        /// Get value at the specified row/column intersection.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        string this[int row, int column] { get; }


        /// <summary>
        /// Get the string value at the specified row/column name intersection.
        /// if column names are not available in from the datasource, the
        /// default columns names will be
        /// "column1", "column2", "column3", etc. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string this[int row, string column] { get; }


        /// <summary>
        /// Read the csv file.
        /// </summary>
        /// <returns></returns>
        BoolMessage ReadAll();
    }
}
