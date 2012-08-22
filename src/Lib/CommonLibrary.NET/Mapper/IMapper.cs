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
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Reflection;

using ComLib.IO;
using ComLib.Extensions;
using ComLib.Reflection;
using ComLib.CsvParse;


namespace ComLib.MapperSupport
{
    /// <summary>
    /// Mapper for sourcing data from Ini files.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMapper<T>
    {
        /// <summary>
        /// Get the supported formate of the data source.
        /// </summary>
        string SupportedFormat { get; }


        /// <summary>
        /// Map objects used internal state.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        IList<T> Map(IErrors errors);


        /// <summary>
        /// Map objects from the source and convert to list of type T. Collect errors into the IErrors.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        IList<T> Map(object source, IErrors errors);  


        /// <summary>
        /// Map objects from the source and convert to list of type T. Collect errors into the IErrors.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        IList<T> MapFromFile(string filepath, IErrors errors);


        /// <summary>
        /// Map objects from the source and convert to list of type T. Collect errors into the IErrors.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        IList<T> MapFromText(string content, IErrors errors);         
    }
}
