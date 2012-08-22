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


namespace ComLib.CsvParse
{
    /// <summary>
    /// Settings for the csv.
    /// </summary>
    public class CsvConfig
    {
        /// <summary>
        /// Indicates if loading as readonly.
        /// </summary>
        public bool IsReadOnly;


        /// <summary>
        /// The character used to separate values in csv file.
        /// </summary>
        public char Separator = ',';
        
        
        /// <summary>
        /// Indicate if first line contains headers/columns.
        /// </summary>
        public bool ContainsHeaders = true;
    }
}
