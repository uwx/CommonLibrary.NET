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

namespace ComLib.BootStrapSupport
{
    /// <summary>
    /// Simple interface to facilitate storage of data
    /// to be passed between tasks.
    /// </summary>
    public interface IAppContext
    {
        /// <summary>
        /// Get/set the data bag stored in this instance.
        /// </summary>
        IDictionary<string, object> Bag { get; set; }
    }

    
    /// <summary>
    /// Simple wrapper for passing context data between tasks.
    /// </summary>
    public class AppContext : IAppContext
    {        
        /// <summary>
        /// Default class constructor.
        /// </summary>
        public AppContext()
        {
            Bag = new Dictionary<string, object>();
        }


        /// <summary>
        /// Get/set the data bag stored in this instance.
        /// </summary>
        public IDictionary<string, object> Bag { get; set; }
    }
}
