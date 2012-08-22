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
using System.Data;
using ComLib;



namespace ComLib.Data
{
    /// <summary>
    /// Abstract class for mapping a DataTable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RowMapperTableBased<T> : RowMapperBaseWithCallBacks<DataTable, T, int>, IRowMapper<DataTable, T>
    {
        /// <summary>
        /// Maps all the rows using DataTable.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public IList<T> MapRows(DataTable table)
        {
            IList<T> records = new List<T>();
            for (int ndx = 0; ndx < table.Rows.Count; ndx++ )
            {
                T record = MapRow(table, ndx);
                records.Add(record);
            }
            if (IsCallbackEnabledForAfterRowsMapped)
                OnAfterRowsMapped(records);

            return records;
        }
    }



    /// <summary>
    /// Abstract class for mapping a row from a DataReader.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RowMapperReaderBased<T> : RowMapperBaseWithCallBacks<IDataReader, T, int>, IRowMapper<IDataReader, T>
    {
        /// <summary>
        /// Map all the rows to IList of objects T using DataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public IList<T> MapRows(IDataReader reader)
        {
            IList<T> records = new List<T>();
            int ndx = 0;
            while(reader.Read())
            {
                T record = MapRow(reader, ndx);
                records.Add(record);
                ndx++;
            }
            if (IsCallbackEnabledForAfterRowsMapped)
                OnAfterRowsMapped(records);

            return records;
        }
    }



    /// <summary>
    /// Context used when mapping a row.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TRowId"></typeparam>
    public class RowMappingContext<TSource, TResult, TRowId>
    {
        /// <summary>
        /// Just used for contextual information at the moment.
        /// </summary>
        public bool IsRowIdStringBased;


        /// <summary>
        /// The Datasource. e.g. Xmldocument, Inidocument, Csvdocument.
        /// </summary>
        public TSource Source;


        /// <summary>
        /// The Row id, either a sting, int or some other object.
        /// </summary>
        public TRowId RowId;


        /// <summary>
        /// Collect the errors during the mapping.
        /// </summary>
        public IValidationResults ValidationResults;
    }



    /// <summary>
    /// Abstract class for row mapping.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TRowId"></typeparam>
    public abstract class RowMapperBaseWithCallBacks<TSource, TResult, TRowId> : RowMapperBase<TSource, TResult, TRowId>
    {
        /// <summary>
        /// Whether or not the callback for after rows have been mapped is enabeld.
        /// </summary>
        public bool IsCallbackEnabledForAfterRowsMapped { get; set; }


        /// <summary>
        /// Callback after rows have been mapped to items.
        /// </summary>
        /// <param name="items"></param>
        public virtual void OnAfterRowsMapped(IList<TResult> items)
        {
        }
    }



    /// <summary>
    /// Abstract class for row mapping.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TRowId"></typeparam>
    public abstract class RowMapperBase<TSource, TResult, TRowId>
    {
        /// <summary>
        /// Map the row number.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="rowNumber"></param>
        /// <returns></returns>
        public abstract TResult MapRow(TSource source, TRowId rowNumber);
    }



    /// <summary>
    /// Row Mapper with contextual information.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TRowId"></typeparam>
    public abstract class RowMapperContextual<TSource, TResult, TRowId> : RowMapperBase<TSource, TResult, TRowId>
    {
        /// <summary>
        /// Map rows with more extensive error capturing.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public abstract BoolMessageItem<IList<TResult>> MapRows(RowMappingContext<TSource, TResult, TRowId> ctx);


        /// <summary>
        /// Map row with more extensive error capturing.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public abstract BoolMessageItem<TResult> MapRow(RowMappingContext<TSource, TResult, TRowId> ctx);
    }



    /// <summary>
    /// Helper class for row mapping.
    /// </summary>
    public class RowMapperHelper
    {
        /// <summary>
        /// Maps all the rows using DataTable.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="rowmapper"></param>
        /// <param name="onAfterRowsMapped"></param>
        /// <returns></returns>
        public static IList<T> MapRows<T>(string connection, string sql, Func<DataTable, int, T> rowmapper, Action<IList<T>> onAfterRowsMapped = null)
        {
            Data.Database db = new Data.Database(connection);
            DataTable table = db.ExecuteDataTableText(sql);
            if (table == null || table.Rows == null || table.Rows.Count == 0)
                return new List<T>();

            IList<T> records = new List<T>();
            for (int ndx = 0; ndx < table.Rows.Count; ndx++)
            {
                T record = rowmapper(table, ndx);
                records.Add(record);
            }
            if (onAfterRowsMapped != null)
                onAfterRowsMapped(records);

            return records;
        }


        /// <summary>
        /// Maps all the rows using DataTable.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="rowmapper"></param>
        /// <param name="onAfterRowsMapped"></param>
        /// <returns></returns>
        public static IList<T> MapRows<T>(DataTable table, Func<DataTable, int, T> rowmapper, Action<IList<T>> onAfterRowsMapped = null)
        {
            IList<T> records = new List<T>();
            for (int ndx = 0; ndx < table.Rows.Count; ndx++)
            {
                T record = rowmapper(table, ndx);
                records.Add(record);
            }
            if (onAfterRowsMapped != null)
                onAfterRowsMapped(records);

            return records;
        }


        /// <summary>
        /// Map all the rows to IList of objects T using DataReader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="rowmapper"></param>
        /// <param name="onAfterRowsMapped"></param>
        /// <returns></returns>
        public static IList<T> MapRows<T>(IDataReader reader, Func<IDataReader, int, T> rowmapper, Action<IList<T>> onAfterRowsMapped = null)
        {
            IList<T> records = new List<T>();
            int ndx = 0;
            while (reader.Read())
            {
                T record = rowmapper(reader, ndx);
                records.Add(record);
                ndx++;
            }
            if (onAfterRowsMapped != null)
                onAfterRowsMapped(records);

            return records;
        }


        /// <summary>
        /// Map all the rows to IList of objects T using DataReader
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="rowmapper"></param>
        /// <param name="onAfterRowsMapped"></param>
        /// <returns></returns>
        public static IList<T> MapRows<T>(string connection, string sql, Func<IDataReader, int, T> rowmapper, Action<IList<T>> onAfterRowsMapped = null)
        {
            Data.Database db = new Data.Database(connection);
            IList<T> records = new List<T>();
                
            db.ExecuteReaderText(sql, reader =>
            {
                int ndx = 0;
                while (reader.Read())
                {
                    T record = rowmapper(reader, ndx);
                    records.Add(record);
                    ndx++;
                }
                if (onAfterRowsMapped != null)
                    onAfterRowsMapped(records);

            }, null);
            
            return records;
        }
    }
}
