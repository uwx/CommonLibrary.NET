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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;


namespace CommonLibrary
{
    /// <summary>
    /// Csv document.
    /// </summary>
    public class CsvDocument
    {
        #region Private members
        private static CsvSettings _defaultSettings = new CsvSettings();
        private string _content = "";
        private string _filePath = "";
        private bool _isFileBased = false;
        private LexList _parser;
        private ReadOnlyCollection<string> _columnNames;
        private List<List<string>> _recordsAsListData;
        private List<IDictionary<string, string>> _recordsAsMap = null;
        private CsvSettings _settings = null;
        #endregion


        #region Constructors
        /// <summary>
        /// Loads a new csv document.
        /// </summary>
        /// <param name="contentOrFilePath"></param>
        /// <param name="isFilePath"></param>
        /// <param name="isCaseSensitive"></param>
        public CsvDocument(string contentOrFilePath, bool isFilePath) 
            :this(contentOrFilePath, isFilePath, _defaultSettings)
        {
        }


        /// <summary>
        /// Create using supplied settings.
        /// </summary>
        /// <param name="contentOfFilePath"></param>
        /// <param name="isFilePath"></param>
        /// <param name="settings"></param>
        public CsvDocument(string contentOrFilePath, bool isFilePath, CsvSettings settings)
        {
            _content = contentOrFilePath;

            if (isFilePath)
            {
                _filePath = contentOrFilePath;
                _content = File.ReadAllText(contentOrFilePath);
                _isFileBased = true;
            }
            LexListSettings lexListSettings = new LexListSettings();
            lexListSettings.MultipleRecordsUsingNewLine = true;
            _parser = new LexList(lexListSettings);
        }
        #endregion


        #region Public Properties
        /// <summary>
        /// Column names.
        /// </summary>
        public ReadOnlyCollection<string> Columns
        {
            get { return _columnNames; }
        }


        /// <summary>
        /// Records as a list of dictionaries.
        /// </summary>
        public List<IDictionary<string, string>> RecordsMap
        {
            get { return _recordsAsMap; }
        }


        /// <summary>
        /// Records as list.
        /// </summary>
        public List<List<string>> RecordsList
        {
            get { return _recordsAsListData; }
        }
        #endregion


        /// <summary>
        /// Parse lists.
        /// </summary>
        /// <returns></returns>
        public void ParseLists()
        {
            List<List<string>> records = Parse(_content);
            if (_settings.FirstLineHasHeaders)
            {
                _columnNames = new ReadOnlyCollection<string>(records[0]);
                records.RemoveAt(0);
                _recordsAsListData = records;
            }
            else
            {
                _recordsAsListData = records;
            }
        }
        

        /// <summary>
        /// Parse as list of records where each record is a dictionary.
        /// This should be used if the first column has headers.
        /// </summary>
        /// <returns></returns>
        public void ParseDict()
        {
            List<List<string>> records = Parse(_content);

            // Columns
            List<string> columnNames = records[0];
            List<IDictionary<string, string>> tableData = new List<IDictionary<string,string>>();

            for(int ndx = 1; ndx < records.Count; ndx++)
            {
                List<string> record = records[ndx];
                IDictionary<string, string> recordMap = new Dictionary<string, string>();

                // Assert that each record has same number of columns
                // TO_DO: Need better way to handle this ??
                if (record.Count != columnNames.Count)
                    throw new ArgumentException("Record at line : " + (ndx + 1) + " does not have same number of columns as in header.");

                // Map the column names to record values.
                for(int colIndex = 0; colIndex < columnNames.Count; colIndex++)
                {
                    string colName = columnNames[colIndex];
                    recordMap[colName] = record[colIndex];
                }

                // Now store record.
                tableData.Add(recordMap);
            }
            _recordsAsMap = tableData;
            _columnNames = new ReadOnlyCollection<string>(columnNames);
        }


        /// <summary>
        /// Returns all the records in the csv content.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private List<List<string>> Parse(string content)
        {
            return _parser.ParseLines(content);
        }
    }



    /// <summary>
    /// Settings for the csv.
    /// </summary>
    public class CsvSettings
    {
        public bool IsReadOnly;
        public string Separator = ",";
        public bool FirstLineHasHeaders = true;
    }
}
