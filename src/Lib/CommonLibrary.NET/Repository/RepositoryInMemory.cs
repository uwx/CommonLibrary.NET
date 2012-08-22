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
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Specialized;

using ComLib;
using ComLib.Reflection;
using ComLib.Entities;
using ComLib.Data;


namespace ComLib.Entities
{
    /// <summary>
    /// UNIT - Test  Implementation.
    /// 
    /// NOTE: This is only used for UNIT-TESTS:
    /// The real repository is RepositorySql which actually connects to a database.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class RepositoryInMemory<T> : RepositoryBase<T> where T : class, IEntity
    {
        #region Private data
        private static int _nextId;
        //private DataSet _database;
        private DataTable _table;
        private string[] _columnsToIndex;
        private const string ColumnEntity = "Entity";
        private bool _cloneEntityOnGet;
        #endregion


        #region Constructors and Initialization
        /// <summary>
        /// Initializes a new instance with default settings and settings public instance properties excluding "IsValid", "Errors", and "Settings" as searchable.
        /// </summary>
        public RepositoryInMemory()
        {
            // Store columns for all properties.
            PropertyInfo[] props = typeof(T).GetProperties();
            string propsToStoreAsColumns = props[0].Name;
            for (int ndx = 1; ndx < props.Length; ndx++)
            {
                if(props[ndx].Name != "IsValid" && props[ndx].Name != "Errors" && props[ndx].Name != "Settings")
                    propsToStoreAsColumns += "," + props[ndx].Name;
            }
            Init(true, propsToStoreAsColumns);
        }


        /// <summary>
        /// Initializes a new instance using comma delimited list of properties that can be indexed(searched)
        /// </summary>
        /// <param name="columnsToIndexDelimited">Delimited list of columns.</param>
        public RepositoryInMemory(string columnsToIndexDelimited) 
        {
            Init(false, columnsToIndexDelimited);
        }


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="cloneEntityOnGet">True to return cloned entity on retrieval as opposed to simply returning the entity since it's in memory</param>
        /// <param name="columnsToIndexDelimited">Delimited list of columns that can be searched/indexed.</param>
        public RepositoryInMemory(bool cloneEntityOnGet, string columnsToIndexDelimited)
        {
            Init(cloneEntityOnGet, columnsToIndexDelimited);
        }


        /// <summary>
        /// Initializes using settings supplied
        /// </summary>
        /// <param name="cloneEntityOnGet">True to return cloned entity on retrieval as opposed to simply returning the entity since it's in memory</param>
        /// <param name="columnsToIndexDelimited">Delimited list of columns.</param>
        public void Init(bool cloneEntityOnGet, string columnsToIndexDelimited)
        {
            string[] colsToIndex = columnsToIndexDelimited.Split(',');
            _cloneEntityOnGet = cloneEntityOnGet;
            _columnsToIndex = colsToIndex;
            _table = new DataTable();
            _tableName = typeof(T).Name + "s";
            IList<PropertyInfo> props = ReflectionUtils.GetProperties(typeof(T), colsToIndex);

            foreach(PropertyInfo prop in props)
            {
                _table.Columns.Add(new DataColumn(prop.Name, prop.PropertyType));
            }
            // Add the object itself.
            _table.Columns.Add(new DataColumn(ColumnEntity, typeof(object)));

        }
        #endregion


        #region Crud
        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="entity">Instance of entity.</param>
        /// <returns>Created entity.</returns>
        public override T Create(T entity)
        {
            // Create id.
            entity.Id = GetNextId();            
            DataRow row = _table.NewRow();
            TransferData(row, entity);
            _table.Rows.Add(row);
            return entity;
        }


        /// <summary>
        /// Retrieve the entity by it's key/id.
        /// </summary>
        /// <param name="id">Id to entity.</param>
        /// <returns>Matching entity.</returns>
        public override T Get(object id)
        {
            DataRow[] rows = _table.Select("Id = " + id);
            if (rows == null || rows.Length == 0)
                return default(T);

            T entity = (T)rows[0][ColumnEntity];
            if (entity is ICloneable)
                entity = (T)((ICloneable)entity).Clone();

            OnRowsMapped(new List<T>(){ entity });
            return entity;
        }


        /// <summary>
        /// Retrieve all the entities.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public override IList<T> GetAll()
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in _table.Rows)
            {
                entities.Add((T)row[ColumnEntity]);
            }
            OnRowsMapped(entities);
            return entities;
        }


        /// <summary>
        /// Retrieve all the entities into a non-generic list.
        /// </summary>
        /// <returns>List with all entities.</returns>
        public override System.Collections.IList GetAllItems()
        {
            ArrayList entities = new ArrayList();
            foreach (DataRow row in _table.Rows)
            {
                entities.Add((T)row[ColumnEntity]);
            }
            return entities;
        }


        /// <summary>
        /// Update the entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <returns>Updated entity.</returns>
        public override T Update(T entity)
        {
            DataRow row = GetRow(entity.Id);
            TransferData(row, entity);
            return entity;
        }


        /// <summary>
        /// Delete the entitiy by it's key/id.
        /// </summary>
        /// <param name="id">Id of entity to delete.</param>
        public override void Delete(object id)
        {
            DataRow row = GetRow(id);
            _table.Rows.Remove(row);
        }


        /// <summary>
        /// Delete all entities from the repository.
        /// </summary>
        public override void DeleteAll()
        {
            _table.Rows.Clear();
        }


        /// <summary>
        /// Delete using the Criteria object.
        /// </summary>
        /// <param name="criteria">Criteria for deletion.</param>
        public override void Delete(IQuery criteria)
        {
            var filter = criteria.Builder.BuildConditions(false);
            DataRow[] rows = _table.Select(filter);
            if (rows != null && rows.Length > 0)
            {
                foreach (var row in rows)
                    _table.Rows.Remove(row);
            }
        }


        /// <summary>
        /// Delete using the expression.
        /// e.g. entity.LogLevel == 1
        /// </summary>
        /// <param name="expression">Expression to use for deletion.</param>
        public override void Delete(Expression<Func<T, bool>> expression)
        {
            string filter = RepositoryExpressionHelper.BuildSinglePropertyCondition<T>(expression);
            DataRow[] rows = _table.Select(filter);
            foreach (var row in rows)
                _table.Rows.Remove(row);
        }
        #endregion


        #region Find
        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="criteria">Filter to match.</param>
        /// <returns>First matched item.</returns>
        public override T First(IQuery criteria)
        {
            IList<T> items = Find(criteria);
            if (items == null || items.Count == 0)
                return default(T);

            return items[0];
        }


        /// <summary>
        /// Find by query
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>List of matching items.</returns>
        public override IList<T> Find(IQuery criteria)
        {
            DataRow[] rows = GetRows(criteria);
            return Map(rows);
        }


        /// <summary>
        /// Find entities by the query. 
        /// </summary>
        /// <param name="queryString">"Id = 23"</param>
        /// <param name="isFullSql">Whether or not the query contains "select from {table} "
        /// This shuold be removed from this datatable implementation.</param>
        /// <returns>List of found items.</returns>
        public override IList<T> Find(string queryString, bool isFullSql)
        {
            DataRow[] rows = _table.Select(queryString);
            return Map(rows);
        }


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="criteria">Criteria object representing filter</param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>Paged list with found items.</returns>
        public override PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize)
        {
            DataRow[] rows = GetRows(criteria);
            return Map(rows, pageNumber, pageSize);
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter">Filter to use.</param>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns>Paged list with found items.</returns>
        public override PagedList<T> Find(string filter, int pageNumber, int pageSize)
        {
            DataRow[] rows = string.IsNullOrEmpty(filter) ?  _table.Select() : _table.Select(filter);
            return Map(rows, pageNumber, pageSize);
        }


        /// <summary>
        /// Get items by page based on latest / most recent create date.
        /// </summary>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15 ( records per page )</param>
        /// <returns>Paged list with found items.</returns>
        public override PagedList<T> FindRecent(int pageNumber, int pageSize)
        {
            return Find(string.Empty, pageNumber, pageSize);
        }
        #endregion


        #region Aggregate - Sum, Min, Max, Count, Avg methods
        /// <summary>
        /// Group(date)(CreateDate)
        /// </summary>
        /// <typeparam name="TGroup">Type of group.</typeparam>
        /// <param name="columnName">Name of column to group by.</param>
        /// <param name="criteria">Inclusion criteria.</param>
        /// <returns>List with key/value pairs.</returns>
        protected override List<KeyValuePair<TGroup, int>> InternalGroup<TGroup>(string columnName, IQuery criteria)
        {
            columnName = DataUtils.Encode(columnName);
            DataRow[] rows = GetRows(criteria);
            var map = new OrderedDictionary();
            foreach (DataRow row in rows)
            {
                string val = row[columnName].ToString();
                if (map.Contains(val))
                {
                    var count = map[val] as KeyValue<TGroup, int>;
                    count.Value++;
                }
                else
                {
                    TGroup groupVal = Converter.ConvertTo<TGroup>(row[columnName]);
                    map[val] = new KeyValue<TGroup, int>(groupVal, 1);
                }
            }
            var groups = new List<KeyValuePair<TGroup, int>>();
            foreach(DictionaryEntry entry in map)
            {
                KeyValue<TGroup, int> result = entry.Value as KeyValue<TGroup,int>;
                groups.Add(new KeyValuePair<TGroup, int>(result.Key, result.Value));
            }
            return groups;
        }


        /// <summary>
        /// Get datatable using mutliple columns in group by and criteria/filter.
        /// </summary>
        /// <param name="criteria">Group criteria.</param>
        /// <param name="columnNames">Array with names of columns to include in the result.</param>
        /// <returns>Data table with data of matching items.</returns>
        public override DataTable Group(IQuery criteria, params string[] columnNames)
        {
            if (columnNames == null || columnNames.Length == 0) return ToTable(criteria);
            DataRow[] rows = GetRows(criteria);

            DataTable groupings = new DataTable();
            foreach (string columnName in columnNames)
                groupings.Columns.Add(columnName);

            groupings.Columns.Add("Count");

            // Add the first one.
            DataRow first = groupings.NewRow();
            Func<DataRow, DataRow> checker = (existing) =>
            {
                // Check 0 rows.
                if (groupings.Rows.Count == 0) return null;

                // Build filter to get matching row.                
                IQuery<object> groupCheck = Query<object>.New().Where(columnNames[0]).Is(existing[columnNames[0]]).End();
                for (int ndx = 1; ndx < columnNames.Length; ndx++)
                    groupCheck.And(columnNames[ndx]).Is(existing[columnNames[ndx]]);

                string filter = groupCheck.Builder.BuildConditions(false);
                DataRow[] matched = groupings.Select(filter);
                if (matched != null && matched.Length > 0)
                    return matched[0];

                return null;
            };

            // Create the grouping data.
            foreach (var row in rows)
            {
                DataRow groupMatch = checker(row);
                if (groupMatch == null)
                {
                    // Add grouping.
                    DataRow newRow = groupings.NewRow();
                    columnNames.ForEach(col => newRow[col] = row[col]);
                    newRow["Count"] = 1;
                    groupings.Rows.Add(newRow);
                }
                else
                {
                    int count = Convert.ToInt32(groupMatch["Count"]);
                    count++;
                    groupMatch["Count"] = count;
                }
            }

            return groupings;
        }


        /// <summary>
        /// Get the distinct values in the specified column.
        /// </summary>
        /// <typeparam name="TVal">Type of values to return.</typeparam>
        /// <param name="columnName">Column name.</param>
        /// <param name="criteria">Filter to apply before getting distinct columns.</param>
        /// <returns>List of distinct values.</returns>
        protected override List<TVal> InternalDistinct<TVal>(string columnName, IQuery criteria)
        {
            Hashtable map = new Hashtable();
            DataRow[] rows = GetRows(criteria);
            List<TVal> uniqueVals = new List<TVal>();
            foreach (DataRow row in rows)
            {
                if (!map.ContainsKey(row[columnName]))
                {
                    map.Add(row[columnName], string.Empty);
                    uniqueVals.Add(Converter.ConvertTo<TVal>(row[columnName]));
                }
            }
            return uniqueVals;
        }
        #endregion


        #region Increment / Decrement
        /// <summary>
        /// Increments the field specified by the expression.
        /// </summary>
        /// <param name="fieldName">The fieldname.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public override void Increment(string fieldName, int by, int id)
        {
            DataRow row = GetRow(id);
            if(row != null)
            {
                int counter = 0;
                if (_table.Columns.Contains(fieldName))
                {
                    counter = Convert.ToInt32(row[fieldName]);
                    counter += by;
                    row[fieldName] = counter;
                }
                else
                {
                    // Update the entity Propertyname.
                    object entity = row["Entity"];
                    PropertyInfo propInfo = entity.GetType().GetProperty(fieldName);                    
                    if (propInfo != null)
                    {
                        counter = Convert.ToInt32(propInfo.GetValue(entity, null));
                        counter += by;
                        propInfo.SetValue(entity, counter, null);
                    }
                }
            }
        }


        /// <summary>
        /// Decrements the field specified by the expression.
        /// </summary>
        /// <param name="fieldName">The fieldname.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public override void Decrement(string fieldName, int by, int id)
        {
            Increment(fieldName, by * -1, id);
        }
        #endregion


        #region To Table
        /// <summary>
        /// Get Table containing all the records.
        /// </summary>
        /// <returns>Data table with all records.</returns>
        public override DataTable ToTable()
        {
            return ToTableBySql(string.Empty);
        }


        /// <summary>
        /// Get datatable using the IQuery filter
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <returns>Data table with matching records.</returns>
        public override DataTable ToTable(IQuery criteria)
        {
            DataRow[] rows = GetRows(criteria);
            return CopyRows(rows);
        }


        /// <summary>
        /// Get datatable using the IQuery filter
        /// </summary>
        /// <param name="sql">Sql criteria to match.</param>
        /// <returns>Data table with matching records.</returns>
        public override DataTable ToTableBySql(string sql)
        {
            DataRow[] filtered = _table.Select(sql);
            return CopyRows(filtered);
        }
        #endregion


        #region Helper methods
        /// <summary>
        /// Retrieve the entity by it's key/id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Data row with entity data.</returns>
        protected DataRow GetRow(object id)
        {
            DataRow[] rows = _table.Select("Id = " + id);
            return rows[0];
        }


        private DataRow[] GetRows(IQuery criteria)
        {
            DataRow[] rows = null;
            if (criteria != null)
            {
                string filter = criteria.Builder.BuildConditions(false);
                string orderby = criteria.Builder.BuildOrderBy(false);
                rows = _table.Select(filter, orderby);

                if (!criteria.Data.IsRecordLimitEnabled)
                    return rows;

                if(rows.Length <= criteria.Data.RecordLimit - 1)
                    return rows;

                DataRow[] limit = new DataRow[criteria.Data.RecordLimit];
                for (int ndx = 0; ndx < criteria.Data.RecordLimit; ndx++)
                    limit[ndx] = rows[ndx];
                return limit;
            }
            else
            {
                var rowList = new List<DataRow>();
                foreach (DataRow row in _table.Rows)
                    rowList.Add(row);
                rows = rowList.ToArray();
            }
            return rows;
        }
        

        /// <summary>
        /// Just change how the query is used. The existing Count, Min, Max base methods can stay the same.
        /// </summary>
        /// <typeparam name="TResult">Type of result to return.</typeparam>
        /// <param name="funcName">Name of aggregation function to run.</param>
        /// <param name="columnName">Column name.</param>
        /// <param name="filter">Filter to apply before running aggregate function.</param>
        /// <returns>Matching result.</returns>
        protected override TResult ExecuteAggregateWithFilter<TResult>(string funcName, string columnName, string filter)
        {
            if (string.Compare(funcName, "count", true) == 0)
            {
                object count = _table.Rows.Count;
                if (!string.IsNullOrEmpty(filter))
                {
                    count = _table.Select(filter).Length;
                }
                return Converter.ConvertTo<TResult>(count);
            }
            string sql = string.Format("{0}({1})",
                         DataUtils.Encode(funcName), DataUtils.Encode(columnName), _tableName);
            object result = _table.Compute(sql, filter);
            if (result == DBNull.Value)
                return default(TResult);

            TResult resultTyped = Converter.ConvertTo<TResult>(result);
            return resultTyped;
        }


        private DataTable CopyRows(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0)
                return _table.Clone();

            DataTable copy = _table.Clone();
            foreach (var row in rows)
                copy.ImportRow(row);

            return copy;
        }


        /// <summary>
        /// Gets the next id.
        /// </summary>
        /// <returns></returns>
        private static int GetNextId()
        {
            int id = 0;
            if (_nextId.GetType() == typeof(int))
            {
                id = Convert.ToInt32(_nextId);
                id++;
                object obj = id;
                _nextId = (int)obj;
            }
            return _nextId;
        }


        /// <summary>
        /// Transfer the entity data into the DataRow and the entity itself.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="entity"></param>
        private void TransferData(DataRow row, T entity)
        {
            BindingFlags flags = BindingFlags.Public|BindingFlags.Instance;
            IList<PropertyInfo> props = ReflectionUtils.GetProperties(entity.GetType(), _columnsToIndex, flags);
            foreach (PropertyInfo prop in props)
            {
                if (prop.CanRead)
                {
                    object val = ReflectionUtils.GetPropertyValueSafely(entity, prop);
                    row[prop.Name] = val;
                }
            }
            row["Entity"] = entity;
        }


        /// <summary>
        /// Maps the DataRows into a list of the typed Entities.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private List<T> Map(DataRow[] rows)
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in rows)
            {
                entities.Add((T)row[ColumnEntity]);
            }
            OnRowsMapped(entities);
            return entities;
        }


        private PagedList<T> Map(DataRow[] rows, int pageNumber, int pageSize)
        {
            // Calculate rows
            IEnumerable<DataRow> pagedRows = null;
            if (pageNumber == 1)
                pagedRows = rows.Take(pageSize);
            else
                pagedRows = rows.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            List<T> items = Map(pagedRows);
            return new PagedList<T>(pageNumber, pageSize, rows.Length, items);
        }


        /// <summary>
        /// Maps the enumerable collection of datarows into a List of entities.
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private List<T> Map(IEnumerable<DataRow> rows)
        {
            List<T> entities = new List<T>();
            foreach (DataRow row in rows)
            {
                entities.Add((T)row[ColumnEntity]);
            }
            OnRowsMapped(entities);
            return entities;
        }
        #endregion
    }
}
