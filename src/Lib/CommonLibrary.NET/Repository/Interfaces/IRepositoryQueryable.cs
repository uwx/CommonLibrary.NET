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
    /// Repository that is Queryable withOUT CRUD/persistance methods.
    /// This allows some basic functionality across any table, regardless
    /// of the entities it stores.
    /// </summary>
    public interface IRepositoryQueryable : IRepositoryConfigurable
    {
        /// <summary>
        /// The name of the table associated w/ this repository.
        /// </summary>
        string TableName { get; set; }


        /// <summary>
        /// Prefix to use for parameterized queries.
        /// e.g. @ for sql server.
        /// </summary>
        string ParamPrefix { get; set; }


        #region Named Filters
        /// <summary>
        /// Dictionary of named filters/criteria.
        /// Useful when doing Count("ActiveUsers");
        /// Or Avg("Rating", "PopularPosts")
        /// </summary>
        IDictionary<string, IQuery> NamedFilters { get; }


        /// <summary>
        /// Add named filter.
        /// e.g. "Published last week" => Criteria.Where("CreateDate").Between(1.Week.Ago, DateTime.Now)
        /// </summary>
        /// <param name="filterName">Name of filter.</param>
        /// <param name="criteria">Filter criteria.</param>
        void AddNamedFilter(string filterName, IQuery criteria);
        #endregion


        #region Sum, Min, Max, Count, Avg, Count, Distinct, Group By
        /// <summary>
        /// Gets the sum of values in the column.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Sum of values.</returns>
        double Sum(string columnName);


        /// <summary>
        /// Gets the sum of the values in the column using criteria supplied.
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <returns>Sum of values.</returns>
        double Sum(string columnName, IQuery criteria);


        /// <summary>
        /// Gets the sum of the values in the column using the named filter
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <param name="namedFilter">Criteria for inclusion.</param>
        /// <returns>Sum of values.</returns>
        double Sum(string columnName, string namedFilter);
        

        /// <summary>
        /// Gets the minimum of the values in the column
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Min of values.</returns>
        double Min(string columnName);


        /// <summary>
        /// Gets the minimum of the values in the column using criteria
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="criteria">The criteria to filter on first</param>
        /// <returns>Min of values.</returns>
        double Min(string columnName, IQuery criteria);


        /// <summary>
        /// Gets the minimum of the values in the column using the named filter.
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="namedFilter">The name of the filter</param>
        /// <returns>Min of values.</returns>
        double Min(string columnName, string namedFilter);


        /// <summary>
        /// Gets the maximum of the values in the column
        /// </summary>
        /// <param name="columnName">Column name.</param>
        /// <returns>Max of values.</returns>
        double Max(string columnName);


        /// <summary>
        /// Gets the maximum of the values in the column using the criteria.
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="criteria">The criteria to filter on first</param>
        /// <returns>Max of values.</returns>
        double Max(string columnName, IQuery criteria);


        /// <summary>
        /// Gets the maximum of the values in the column using the named filter.
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="namedFilter">The name of the filter</param>
        /// <returns>Max of values.</returns>
        double Max(string columnName, string namedFilter);


        /// <summary>
        /// Gets the avergage of the values in the column
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <returns>Average of values.</returns>
        double Avg(string columnName);
        
        /// <summary>
        /// Gets the average of the values in the column using the criteria
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="criteria">The criteria to filter on first</param>
        /// <returns>Average of values.</returns>
        double Avg(string columnName, IQuery criteria);
        

        /// <summary>
        /// Gets the average of the values in the column using the named filter.
        /// </summary>
        /// <param name="columnName">The name of the column</param>
        /// <param name="namedFilter">The name of the filter</param>
        /// <returns>Average of values.</returns>
        double Avg(string columnName, string namedFilter);


        /// <summary>
        /// Gets the total items of records in this repository
        /// </summary>
        /// <returns>Number of values.</returns>
        int Count();


        /// <summary>
        /// Gets the number of items matching the criteria
        /// </summary>
        /// <param name="criteria">The criteria to apply to get count</param>
        /// <returns>Number of matching values.</returns>
        int Count(IQuery criteria);


        /// <summary>
        /// Gets the number of items using the named filter
        /// </summary>
        /// <param name="namedFilter">The name of the filter</param>
        /// <returns>Number of matching values.</returns>
        int Count(string namedFilter);
        
        
        /// <summary>
        /// Gets the number of items using the sql text supplied as the search criteria.
        /// </summary>
        /// <param name="sql">The sql to use as the filter</param>
        /// <returns>Number of matching values.</returns>
        int CountBySql(string sql);


        /// <summary>
        /// Get the distinct values in the column supplied.
        /// </summary>
        /// <typeparam name="TVal">Type of value to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <returns>List with distinct values.</returns>
        List<TVal> Distinct<TVal>(string columnName);


        /// <summary>
        /// Get the distinct values in the oclumn using the criteria
        /// </summary>
        /// <typeparam name="TVal">Type of value to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <returns>List with distinct values.</returns>
        List<TVal> Distinct<TVal>(string columnName, IQuery criteria);


        /// <summary>
        /// Get the distinct values in the column using the named filter for search criteria.
        /// </summary>
        /// <typeparam name="TVal">Type of value to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <param name="namedFilter">Criteria for inclusion.</param>
        /// <returns>List with distinct values.</returns>
        List<TVal> Distinct<TVal>(string columnName, string namedFilter);

        /// <summary>
        /// Get the stored key/value pairs.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <returns>List with key/value pairs.</returns>
        List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName);


        /// <summary>
        /// Get the stored key/value pairs after applying matching criteria.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <returns>List with key/value pairs.</returns>
        List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName, IQuery criteria);


        /// <summary>
        /// Get the stored key/value pairs after applying matching criteria.
        /// </summary>
        /// <typeparam name="TGroup">Type of group to use.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <param name="namedFilter">Criteria for inclusion.</param>
        /// <returns>List with key/value pairs.</returns>
        List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName, string namedFilter);

        /// <summary>
        /// Get a datatable with stored values based on criteria.
        /// </summary>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <param name="columnNames">Array with column names to include.</param>
        /// <returns>Datatable with matching values.</returns>
        DataTable Group(IQuery criteria, params string[] columnNames);


        /// <summary>
        /// Get a datatable with stored values based on criteria.
        /// </summary>
        /// <param name="namedFilter">Criteria for inclusion.</param>
        /// <param name="columnNames">Array with column names to include.</param>
        /// <returns>Datatable with matching values.</returns>
        DataTable GroupNamedFilter(string namedFilter, params string[] columnNames);
        #endregion


        #region Exists
        /// <summary>
        /// Checks whether there are any records that the named filter
        /// </summary>
        /// <param name="filtername">Criteria for inclusion.</param>
        /// <returns>True if there are records matching the criteria.</returns>
        bool Any(string filtername);


        /// <summary>
        /// Chekcs where there are any records that match the criteria filter.
        /// </summary>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <returns>True if there are records matching the criteria.</returns>
        bool Any(IQuery criteria);


        /// <summary>
        /// Get datatable using the sql supplied.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>True if there are any records matching the criteria.</returns>
        bool AnyBySql(string sql);
        #endregion


        #region To Table
        /// <summary>
        /// Get Table containing all the records.
        /// </summary>
        /// <returns>Table with all records.</returns>
        DataTable ToTable();


        /// <summary>
        /// Get datatable using the IQuery filter
        /// </summary>
        /// <param name="criteria">Criteria for inclusion.</param>
        /// <returns>Table with all matching records.</returns>
        DataTable ToTable(IQuery criteria);


        /// <summary>
        /// Get datatable using the filtername.
        /// </summary>
        /// <param name="filterName">Criteria for inclusion.</param>
        /// <returns>Table with all matching records.</returns>
        DataTable ToTable(string filterName);


        /// <summary>
        /// Get datatable using the sql filter.
        /// </summary>
        /// <param name="sql">SQL filter.</param>
        /// <returns>Table with all matching records.</returns>
        DataTable ToTableBySql(string sql);
        #endregion
    }
}