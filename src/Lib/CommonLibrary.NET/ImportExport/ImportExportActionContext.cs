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
using ComLib;



namespace ComLib.ImportExport
{
    /// <summary>
    /// Contextual information for import export actions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportExportActionContext<T> : ActionContext
    {
        private IList<T> _items = null;


        /// <summary>
        /// Initialize the action context.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="errorList"></param>
        /// <param name="messages"></param>
        public ImportExportActionContext()
        {
        }


        /// <summary>
        /// Initialize the action context.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="errorList"></param>
        /// <param name="messages"></param>
        public ImportExportActionContext(T item, IList<T> items, IValidationResults errors)
            : base(item)
        {
            _items = items;
            Errors = errors;
        }


        /// <summary>
        /// Get the list of items to import or export.
        /// </summary>
        public IList<T> ItemList
        {
            get { return _items; }
            set { _items = value; }
        }


        /// <summary>
        /// Gets or sets the import text.
        /// </summary>
        /// <value>The import text.</value>
        public string ImportText { get; set; }


        /// <summary>
        /// Gets or sets the size of the export batch.
        /// </summary>
        /// <value>The size of the export batch.</value>
        public int ExportBatchSize { get; set; }


        /// <summary>
        /// Gets or sets the index of the export batch.
        /// </summary>
        /// <value>The index of the export batch.</value>
        public int ExportBatchIndex { get; set; }


        /// <summary>
        /// Gets or sets the export total count.
        /// </summary>
        /// <value>The export total count.</value>
        public int ExportTotalCount { get; set; }
        

        /// <summary>
        /// Gets or sets the export format.
        /// </summary>
        /// <value>The export format.</value>
        public string ExportFormat { get; set; }
    }
}
