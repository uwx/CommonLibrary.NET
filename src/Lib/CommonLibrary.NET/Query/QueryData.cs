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

namespace ComLib.Data
{
    /// <summary>
    /// This class is used to store query data.
    /// </summary>
    public class QueryData
    {
        /// <summary>
        /// List of query conditions.
        /// </summary>
        public List<Condition> Conditions;


        /// <summary>
        /// List of query orderings.
        /// </summary>
        public List<OrderByClause> Orderings;


        /// <summary>
        /// Order-by clause.
        /// </summary>
        public OrderByClause LastOrderBy;


        /// <summary>
        /// List of fields to select.
        /// </summary>
        public List<SelectField> SelectFields;


        /// <summary>
        /// Source of information.
        /// </summary>
        public string From;


        /// <summary>
        /// Top number of records to retrieve.
        /// </summary>
        public int RecordLimit;


        /// <summary>
        /// True if record limit is enabled.
        /// </summary>
        public bool IsRecordLimitEnabled;


        /// <summary>
        /// Default class constructor.
        /// </summary>
        public QueryData()
        {
            Conditions = new List<Condition>();
            SelectFields = new List<SelectField>();
            Orderings = new List<OrderByClause>();
        }
    }
}
