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


namespace ComLib.CsvParse
{
    /// <summary>
    /// Csv static class for parsing csv files.
    /// </summary>
    public class Csv
    {
        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc Load(string text, bool hasHeaders)
        {
            return Load(text, hasHeaders, false, ',');
        }


        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <param name="isReadOnly">Whether or not to make the parsed doc readonly.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc Load(string text, bool hasHeaders, bool isReadOnly)
        {
            return Load(text, hasHeaders, isReadOnly, ',');
        }


        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <param name="isReadOnly">Whether or not to make the parsed doc readonly.</param>
        /// <param name="delimeter">Dellimeter to use for separate values.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc Load(string text, bool hasHeaders, bool isReadOnly, char delimeter)
        {
            var settings = new CsvConfig() { ContainsHeaders = hasHeaders, Separator = delimeter, IsReadOnly = isReadOnly };
            CsvDoc doc = new CsvDoc(text, true, settings, true);
            return doc;
        }


        /// <summary>
        /// Whether or not the csv file can be loaded. 
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <param name="isReadOnly">Whether or not to make the parsed doc readonly.</param>
        /// <param name="delimeter">The delimeter in the text</param>
        /// <returns>A CsvDoc</returns>
        public static BoolMessageItem<CsvDoc> CanLoad(string text, bool hasHeaders, bool isReadOnly, char delimeter)
        {
            CsvDoc doc = null;
            bool loaded = true;
            string message = string.Empty;
            Try.Catch(() => doc = Load(text, hasHeaders, isReadOnly, delimeter), (ex) =>
            {
                loaded = false;
                message = ex.Message;
            });
            return new BoolMessageItem<CsvDoc>(doc, loaded, message);
        }


        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc LoadText(string text, bool hasHeaders)
        {
            return LoadText(text, hasHeaders, false, ',');
        }


        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <param name="isReadOnly">Whether or not to make the parsed doc readonly.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc LoadText(string text, bool hasHeaders, bool isReadOnly)
        {
            return LoadText(text, hasHeaders, isReadOnly, ',');
        }


        /// <summary>
        /// Parse the csv text.
        /// </summary>
        /// <param name="text">The csv formatted text string.</param>
        /// <param name="hasHeaders">Whether or not the csv text has headers.</param>
        /// <param name="isReadOnly">Whether or not to make the parsed doc readonly.</param>
        /// <param name="delimeter">Dellimeter to use for separate values.</param>
        /// <returns>A CsvDoc</returns>
        public static CsvDoc LoadText(string text, bool hasHeaders, bool isReadOnly, char delimeter)
        {
            var settings = new CsvConfig() {  ContainsHeaders = hasHeaders, Separator = delimeter, IsReadOnly = isReadOnly };
            CsvDoc doc = new CsvDoc(text, false, settings, true);
            return doc;
        }


        /// <summary>
        /// Writes csv data to the file using the settings provided.
        /// </summary>
        /// <param name="fileName">The file name to write the csv data to.</param>
        /// <param name="data">The csv data.</param>
        /// <param name="firstRowInDataAreColumns"></param>
        public static void Write(string fileName, List<List<object>> data, bool firstRowInDataAreColumns)
        {
            Write(fileName, data, ",", null, firstRowInDataAreColumns, false, "\"", Environment.NewLine, false);
        }


        /// <summary>
        /// Writes csv data to the file using the settings provided.
        /// </summary>
        /// <param name="fileName">The file name to write the csv data to.</param>
        /// <param name="data">The csv data.</param>
        /// <param name="firstRowInDataAreColumns"></param>
        /// <param name="delimeter">The delimeter to use.</param>
        public static void Write(string fileName, List<List<object>> data, bool firstRowInDataAreColumns, string delimeter)
        {
            Write(fileName, data, delimeter, null, firstRowInDataAreColumns, false, "\"", Environment.NewLine, false);
        }


        /// <summary>
        /// Writes csv data to the file using the settings provided.
        /// </summary>
        /// <param name="fileName">The file name to write the csv data to.</param>
        /// <param name="data">The csv data.</param>
        /// <param name="delimeter">The delimeter to use.</param>
        /// <param name="columns">The header columns.</param>
        public static void Write(string fileName, List<List<object>> data, string delimeter, List<string> columns)
        {
            Write(fileName, data, ",", columns, false, false, "\"", Environment.NewLine, false);
        }


        /// <summary>
        /// Writes csv data to the file using the settings provided.
        /// </summary>
        /// <param name="fileName">The file name to write the csv data to.</param>
        /// <param name="data">The csv data.</param>
        /// <param name="delimeter">The delimeter to use.</param>
        /// <param name="columns">The header columns.</param>
        /// <param name="firstRowInDataAreColumns">First Row in data are columns.</param>
        /// <param name="quoteAll">Whether or not to quote all the values.</param>
        /// <param name="quoteChar">The quote char to use to enclose values.</param>
        /// <param name="newLine">New Line to use</param>
        /// <param name="append">Whether or not to append to file.</param>
        public static void Write(string fileName, List<List<object>> data, string delimeter, List<string> columns, bool firstRowInDataAreColumns, bool quoteAll, string quoteChar, string newLine, bool append)
        {
            using (var writer = new CsvWriter(fileName, data, delimeter, columns, firstRowInDataAreColumns, quoteAll, quoteChar, newLine, append))
            {
                writer.Write();
            }
        }        
    }
}
