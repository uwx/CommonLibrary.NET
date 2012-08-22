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
using System.Reflection;
using System.Collections.Specialized;
using System.Collections;


namespace ComLib.Data
{
    /// <summary>
    /// Database / table related utility functions.
    /// </summary>
    public class DataUtils
    {
        /// <summary>
        /// Encode the text for single quotes.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encode(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text.Replace("'", "''");
        }


        /// <summary>
        /// Builds a column name excluding potentially unsafe characters.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string BuildSafeColumn(string column)
        {
            if (string.IsNullOrEmpty(column))
                return column;

            string finalname = "";
            for (int ndx = 0; ndx < column.Length; ndx++)
                if (Char.IsLetterOrDigit(column[ndx]))
                    finalname += column[ndx];

            return finalname;
        }


        /// <summary>
        /// Converts the list of objects to a data table using the types public / get properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static DataTable ConvertListToDataTable<T>(IList<T> objects)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.GetProperty);
            DataTable table = new DataTable();

            // Define all the columns.
            foreach (PropertyInfo property in props)
            {
                DataColumn column = new DataColumn();
                column.ColumnName = property.Name;
                column.DataType = typeof(string);
                column.DefaultValue = string.Empty;
                table.Columns.Add(column);
            }

            // Now create each record.
            foreach (object obj in objects)
            {
                DataRow row = table.NewRow();

                foreach (PropertyInfo property in props)
                {
                    string val = string.Empty;
                    object propVal = property.GetValue(obj, null);
                    row[property.Name] = Convert.ToString(propVal);
                }
                table.Rows.Add(row);
            }
            return table;
        }


        /// <summary>
        /// Generates a data table with property names
        /// as columns and property values as row data.
        /// </summary>
        /// <param name="objects">List of objects to extract row data from.</param>
        /// <param name="properties">List of properties to create columns from.</param>
        /// <returns>Data table with property columns and property value rows.</returns>
        public static DataTable ConvertPropertyCollectionToDataTable(IList objects, IList<PropertyInfo> properties)
        {
            DataTable table = new DataTable();

            // Define all the columns.
            foreach (PropertyInfo property in properties)
            {
                DataColumn column = new DataColumn();
                column.ColumnName = property.Name;
                column.DataType = typeof(string);
                column.DefaultValue = string.Empty;
                table.Columns.Add(column);
            }

            // Now create each record.
            foreach (object obj in objects)
            {
                DataRow row = table.NewRow();

                foreach (PropertyInfo property in properties)
                {
                    string val = string.Empty;
                    object propVal = property.GetValue(obj, null);
                    row[property.Name] = Convert.ToString(propVal);
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
