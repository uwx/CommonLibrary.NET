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
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

using ComLib;
using ComLib.Data;
using ComLib.Entities;


namespace ComLib.Entities
{
    /// <summary>
    /// Repository base class.
    /// </summary>
    public class RepositoryQueryable : IRepositoryConfigurable, IRepositoryQueryable
    {
        /// <summary>
        /// Connection information.
        /// </summary>
        protected ConnectionInfo _connectionInfo;


        /// <summary>
        /// Database table name.
        /// </summary>
        protected string _tableName;


        /// <summary>
        /// Database instance.
        /// </summary>
        protected IDatabase _db;


        /// <summary>
        /// List of filters.
        /// </summary>
        protected IDictionary<string, IQuery> _namedFilters;


        /// <summary>
        /// Prefix used for sql parameters.
        /// </summary>
        protected string _paramPrefix = "@";


        #region IRepositoryConfigurable
        /// <summary>
        /// The database db.
        /// </summary>
        public virtual IDatabase Database
        {
            get { return _db; }
            set { _db = value; }
        }


        /// <summary>
        /// The connection info object.
        /// </summary>
        public virtual ConnectionInfo Connection
        {
            get { return _connectionInfo; }
            set { _connectionInfo = value; }
        }


        /// <summary>
        /// Connection info object.
        /// </summary>
        public virtual string ConnectionString
        {
            get { return _connectionInfo.ConnectionString; }
        }
        #endregion


        #region Properties
        /// <summary>
        /// Get / Set the table name.
        /// </summary>
        public virtual string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }


        /// <summary>
        /// Get/set the parameter prefix.
        /// </summary>
        public string ParamPrefix
        {
            get { return _paramPrefix; }
            set { _paramPrefix = value; }
        }
        #endregion


        #region Sum, Min, Max, Count, Avg, Count, Distinct, Group By
        /// <summary>
        /// Total number of records.
        /// </summary>
        /// <returns>Total number of records.</returns>
        public virtual int Count()
        {
            return ExecuteAggregate<int>("count", "*", null);
        }


        /// <summary>
        /// Total number of records with filter.
        /// </summary>
        /// <param name="criteria">Filter to apply before getting totalrecords.</param>
        /// <returns>Number of matched records.</returns>
        public virtual int Count(IQuery criteria)
        {
            return ExecuteAggregate<int>("count", "*", criteria);
        }


        /// <summary>
        /// Total number of records with named filter.
        /// </summary>
        /// <param name="namedFilter">Filter to apply before getting totalrecords.</param>
        /// <returns>Number of matched records.</returns>
        public int Count(string namedFilter)
        {            
            return ExecuteAggregate<int>("count", "*", GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Total number of records with sql expression.
        /// </summary>
        /// <param name="sql">Sql expression.</param>
        /// <returns>Number of matched records.</returns>
        public virtual int CountBySql(string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return Count();

            string fullsql = string.Format("select count(*) as count from {0}", _tableName);
            fullsql += " where " + sql;
            int numRecords = Convert.ToInt32(_db.ExecuteScalarText(sql));
            return numRecords;
        }


        /// <summary>
        /// Sum("Cost");
        /// </summary>
        /// <param name="columnName">Name of the column to perform the Sum on</param>
        /// <returns>Sum of items.</returns>
        public virtual double Sum(string columnName)
        {
            return ExecuteAggregate<double>("sum", columnName, null);
        }


        /// <summary>
        /// Sum("cost", filter).
        /// </summary>
        /// <param name="columnName">Name of column to use.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>Sum of matching items.</returns>
        public virtual double Sum(string columnName, IQuery criteria)
        {
            return ExecuteAggregate<double>("sum", columnName, criteria);
        }


        /// <summary>
        /// Sum("cost", "Popular Products").
        /// </summary>
        /// <param name="columnName">Name of column to use.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>Sum of matching items.</returns>
        public virtual double Sum(string columnName, string namedFilter)
        {
            return ExecuteAggregate<double>("sum", columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Min("Cost");
        /// </summary>
        /// <param name="columnName">Name of the column to perform the Min on</param>
        /// <returns>Min of items.</returns>
        public virtual double Min(string columnName)
        {
            return ExecuteAggregate<double>("min", columnName, null);
        }


        /// <summary>
        /// Min("cost", filter).
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>Min of items matching the criteria.</returns>
        public virtual double Min(string columnName, IQuery criteria)
        {
            return ExecuteAggregate<double>("min", columnName, criteria);
        }


        /// <summary>
        /// Min("cost", "Popular Products").
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>Min of items matching the criteria.</returns>
        public virtual double Min(string columnName, string namedFilter)
        {
            return ExecuteAggregate<double>("min", columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Max("Cost");
        /// </summary>
        /// <param name="columnName">Name of the column to perform the Max on</param>
        /// <returns>Max of items.</returns>
        public virtual double Max(string columnName)
        {
            return ExecuteAggregate<double>("max", columnName, null);
        }


        /// <summary>
        /// Max("cost", filter).
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>Max of matched items.</returns>
        public virtual double Max(string columnName, IQuery criteria)
        {
            return ExecuteAggregate<double>("max", columnName, criteria);
        }


        /// <summary>
        /// Max("cost", "Popular Products").
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>Max of matched items.</returns>
        public virtual double Max(string columnName, string namedFilter)
        {
            return ExecuteAggregate<double>("max", columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Avg("Cost");
        /// </summary>
        /// <param name="columnName">Name of the column to perform the Avg on</param>
        /// <returns>Average of items.</returns>
        public virtual double Avg(string columnName)
        {
            return ExecuteAggregate<double>("avg", columnName, null);
        }


        /// <summary>
        /// Avg("cost", filter).
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>Average of matched items.</returns>
        public virtual double Avg(string columnName, IQuery criteria)
        {
            return ExecuteAggregate<double>("avg", columnName, criteria);
        }


        /// <summary>
        /// Avg("cost", "Popular Products").
        /// </summary>
        /// <param name="columnName">Name of column.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>Average of matched items.</returns>
        public virtual double Avg(string columnName, string namedFilter)
        {
            return ExecuteAggregate<double>("avg", columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Get distinct values from the specified column.
        /// </summary>
        /// <typeparam name="TVal">Type of value to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <returns>List of distinct values.</returns>
        public virtual List<TVal> Distinct<TVal>(string columnName)
        {
            return InternalDistinct<TVal>(columnName, null);
        }


        /// <summary>
        /// Get distinct values from the specified column.
        /// </summary>
        /// <typeparam name="TVal">Type of value to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List of distinct values matching the criteria.</returns>
        public virtual List<TVal> Distinct<TVal>(string columnName, IQuery criteria)
        {
            return InternalDistinct<TVal>(columnName, criteria);
        }        

                
        /// <summary>
        /// Get distinct values from the specified column.
        /// </summary>
        /// <typeparam name="TVal">Type of value to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>List of distinct values matching the criteria.</returns>
        public virtual List<TVal> Distinct<TVal>(string columnName, string namedFilter)
        {
            return InternalDistinct<TVal>(columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// GroupBy (date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of group to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <returns>List with key/value pairs.</returns>
        public virtual List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName)
        {
            return InternalGroup<TGroup>(columnName, null);
        }


        /// <summary>
        /// GroupBy (date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of group to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List with key/value pairs that match.</returns>
        public virtual List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName, IQuery criteria)
        {
            return InternalGroup<TGroup>(columnName, criteria);
        }


        /// <summary>
        /// GroupBy (date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of group to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <returns>List of key/value pairs that match.</returns>
        public virtual List<KeyValuePair<TGroup, int>> Group<TGroup>(string columnName, string namedFilter)
        {
            return InternalGroup<TGroup>(columnName, GetNamedFilter(namedFilter, true));
        }


        /// <summary>
        /// Get datatable using named filter and groupby on multiple columns.
        /// </summary>
        /// <param name="namedFilter">Criteria to match.</param>
        /// <param name="columnNames">Array with column names.</param>
        /// <returns>Data table with information of matching items.</returns>
        public DataTable GroupNamedFilter(string namedFilter, params string[] columnNames)
        {
            return Group(GetNamedFilter(namedFilter, true), columnNames);
        }


        /// <summary>
        /// Get datatable using mutliple columns in group by and criteria/filter.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <param name="columnNames">Array with column names.</param>
        /// <returns>Data table with information of matching items.</returns>
        public virtual DataTable Group(IQuery criteria, params string[] columnNames)
        {            
            if (columnNames == null || columnNames.Length == 0) return ToTable(criteria);

            string columns = DataUtils.Encode(columnNames[0]);
            string groupBy = DataUtils.Encode(columnNames[0]);

            if (columnNames.Length > 1)
                for (int ndx = 1; ndx < columnNames.Length; ndx++)
                {
                    columns += ", " + DataUtils.Encode(columnNames[ndx]);
                    groupBy += ", " + DataUtils.Encode(columnNames[ndx]);
                }

            var sql = BuildSqlSelect(criteria, columns + ", count(*) as count", _tableName, null, groupBy, true);
            return _db.ExecuteDataTableText(sql);
        }
        #endregion


        #region To Table
        /// <summary>
        /// Get Table containing all the records.
        /// </summary>
        /// <returns>Data table with information of all items.</returns>
        public virtual DataTable ToTable()
        {
            string sql = string.Format("select * from {0}", _tableName);
            return ToTableBySql(sql);
        }


        /// <summary>
        /// Get datatable using the IQuery filter.
        /// </summary>
        /// <param name="criteria">Inclusion criteria.</param>
        /// <returns>Data table with information of matched items.</returns>
        public virtual DataTable ToTable(IQuery criteria)
        {
            criteria.Data.From = _tableName;
            string sql = criteria.Builder.Build();
            return ToTableBySql(sql);            
        }


        /// <summary>
        /// Get datatable using the filtername.
        /// </summary>
        /// <param name="filterName">Inclusion criteria.</param>
        /// <returns>Data table with information of matched items.</returns>
        public DataTable ToTable(string filterName)
        {
            IQuery filter = GetNamedFilter(filterName, true);
            return ToTable(filter);
        }


        /// <summary>
        /// Get datatable using the sql filter.
        /// </summary>
        /// <param name="sql">Sql to execute.</param>
        /// <returns>Resulting data table.</returns>
        public virtual DataTable ToTableBySql(string sql)
        {
            return _db.ExecuteDataTableText(sql);
        }
        #endregion


        #region Exists
        /// <summary>
        /// Checks whether there are any records that match the sql.
        /// </summary>
        /// <param name="filterName">Filter to apply.</param>
        /// <returns>True if at least one record matches the filter criteria.</returns>
        public virtual bool Any(string filterName)
        {
            return Count(filterName) > 0;
        }


        /// <summary>
        /// Checks whether there are any records that match the sql.
        /// </summary>
        /// <param name="sql">Sql filter.</param>
        /// <returns>True if at least one record matches the filter criteria.</returns>
        public virtual bool AnyBySql(string sql)
        {
            return CountBySql(sql) > 0;
        }


        /// <summary>
        /// Chekcs where there are any records that match the criteria filter.
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>True if at least one record matches the criteria.</returns>
        public virtual bool Any(IQuery criteria)
        {
            return Count(criteria) > 0;
        }
        #endregion


        #region Named Filters
        /// <summary>
        /// Dictionary of named filters/criteria.
        /// Useful when doing Count("ActiveUsers");
        /// Or Avg("Rating", "PopularPosts")
        /// </summary>
        public IDictionary<string, IQuery> NamedFilters { get { return _namedFilters; } }


        /// <summary>
        /// Add named filter.
        /// e.g. "Published last week" => Criteria.Where("CreateDate").Between(1.Week.Ago, DateTime.Now)
        /// </summary>
        /// <param name="filterName">Filter to apply.</param>
        /// <param name="criteria">Criteria to match.</param>
        public void AddNamedFilter(string filterName, IQuery criteria)
        {
            if(_namedFilters == null )
                _namedFilters = new Dictionary<string, IQuery>();

            NamedFilters[filterName] = criteria;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Retrieves distinct items after applying the supplied criteria.
        /// </summary>
        /// <typeparam name="TVal">Type of items to return.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List with distinct items of records that match the criteria.</returns>
        protected virtual List<TVal> InternalDistinct<TVal>(string columnName, IQuery criteria)
        {
            string encodedColumnName = string.Format("distinct([{0}])", DataUtils.Encode(columnName));
            string sql = BuildSqlSelect(criteria, encodedColumnName, _tableName, null, null, false);
            List<TVal> uniqueItems = new List<TVal>();

            _db.ExecuteReaderText(sql, reader =>
            {
                while (reader.Read())
                {
                    TVal val = Converter.ConvertTo<TVal>(reader[columnName]);
                    uniqueItems.Add(val);
                }
            });
            return uniqueItems;
        }


        /// <summary>
        /// GroupBy (date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of grouping.</typeparam>
        /// <param name="columnName">Name of column.</param>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List fo key/value pairs of matching items.</returns>
        protected virtual List<KeyValuePair<TGroup, int>> InternalGroup<TGroup>(string columnName, IQuery criteria)
        {
            string encodedColName = DataUtils.Encode(columnName);
            string cols = string.Format("[{0}], count(*) as \"count\"", encodedColName);
            string sql = BuildSqlSelect(criteria, cols, _tableName, null, encodedColName, true);
            DataTable table = _db.ExecuteDataTableText(sql, null);
            var results = new List<KeyValuePair<TGroup, int>>();
            foreach (DataRow row in table.Rows)
            {
                TGroup val = Converter.ConvertTo<TGroup>(row[columnName]);
                int count = Convert.ToInt32(row["count"]);
                results.Add(new KeyValuePair<TGroup, int>(val, count));
            }
            return results;
        }


        /// <summary>
        /// Returns a named filter by its name.
        /// </summary>
        /// <param name="filterName">Name of filter to search for.</param>
        /// <param name="throwIfNotFound">True to throw a <see cref="ArgumentException"/> if 
        /// the filter is not found.</param>
        /// <returns>Named filter.</returns>
        protected IQuery GetNamedFilter(string filterName, bool throwIfNotFound)
        {
            if (string.IsNullOrEmpty(filterName))
                return null;

            if (_namedFilters.ContainsKey(filterName))
                return _namedFilters[filterName];

            if (throwIfNotFound) throw new ArgumentException("Named filter : " + filterName + " does not exist.");

            return null;
        }


        /// <summary>
        /// Sum,Min,Max,Count,Avg execution method.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="funcName">sum | min | max | avg | count</param>
        /// <param name="columnName">name of the column to perform the function on.</param>
        /// <param name="criteria">Criteria for filters.</param>
        /// <returns>Result.</returns>
        protected virtual TResult ExecuteAggregate<TResult>(string funcName, string columnName, IQuery criteria)
        {
            string filter = criteria == null ? string.Empty : criteria.Builder.BuildConditions(false);
            return ExecuteAggregateWithFilter<TResult>(funcName, columnName, filter);
        }


        /// <summary>
        /// Sum,Min,Max,Count,Avg execution method.
        /// </summary>
        /// <typeparam name="TResult">Type of result.</typeparam>
        /// <param name="funcName">sum | min | max | avg | count</param>
        /// <param name="columnName">name of the column to perform the function on.</param>
        /// <param name="filter">Filter to apply in where clause.</param>
        /// <returns>Result.</returns>
        protected virtual TResult ExecuteAggregateWithFilter<TResult>(string funcName, string columnName, string filter)
        {
            string sql = string.Format("select {0}({1}) from {2}",
                         DataUtils.Encode(funcName), DataUtils.Encode(columnName), _tableName);
            if (!string.IsNullOrEmpty(filter))
                sql = sql + " where " + filter;

            object result = _db.ExecuteScalarText(sql, null);
            TResult resultTyped = Converter.ConvertTo<TResult>(result);
            return resultTyped;
        }


        /// <summary>
        /// Builds a select SQL based on parameters.
        /// </summary>
        /// <param name="criteria">Criteria to apply to result.</param>
        /// <param name="cols">Delimited list of columns to include.</param>
        /// <param name="fromTable">Table to select from.</param>
        /// <param name="whereFilter">Sql Where filter.</param>
        /// <param name="groupByCols">Sql GroupBy expression.</param>
        /// <param name="useLimit">Rows to limit to.</param>
        /// <returns>Constructed SQL select statement.</returns>
        protected virtual string BuildSqlSelect(IQuery criteria, string cols, string fromTable, string whereFilter, string groupByCols, bool useLimit)
        {
            string select = "*";

            // Allow the select cols to override the criteria
            bool hasCriteria = criteria != null;
            if(!string.IsNullOrEmpty(cols)) select = cols;
            else if( hasCriteria) select = criteria.Builder.BuildSelect(false);

            // Allow where filter to override the criteria.
            string where = "";
            if (!string.IsNullOrEmpty(whereFilter)) where = " where " + whereFilter;
            else if (hasCriteria) where = criteria.Builder.BuildConditions();

            string limit = criteria != null && useLimit ? criteria.Builder.BuildLimit() : string.Empty;            
            string orderby = criteria == null ? string.Empty : criteria.Builder.BuildOrderBy();
            string groupby = string.IsNullOrEmpty(groupByCols) ? string.Empty : " group by " + groupByCols;

            string sql = string.Empty;
       
            if ( string.IsNullOrEmpty(groupby) )
                sql = string.Format("select {0} {1} from {2} {3} {4}", limit, select, fromTable, where, orderby);

            else
                sql = string.Format("select {0} {1} from {2} {3} {4} {5}", limit, select, fromTable, where, groupby, orderby);

            return sql;
        }
        #endregion
    }

}
