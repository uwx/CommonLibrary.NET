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
using ComLib.Entities;

namespace ComLib.Entities.Management
{
    /// <summary>
    /// Contains publicly accessible method names.
    /// </summary>
    public class EntityServiceSettings
    {
        /// <summary>
        /// Method for Creating entity.
        /// </summary>
        public string CreateMethod = "Create";


        /// <summary>
        /// Method for updating domain entity.
        /// </summary>
        public string UpdateMethod = "Update";


        /// <summary>
        /// Method for deleting domain entity.
        /// </summary>
        public string DeleteMethod = "Delete";


        /// <summary>
        /// Save method.
        /// </summary>
        public string SaveMethod = "Save";


        /// <summary>
        /// Method for retrieving domain entity.
        /// </summary>
        public string RetrieveMethod = "Get";


        /// <summary>
        /// Method for retrieving all domain entities.
        /// </summary>
        public string RetrieveAllMethod = "GetAllItems";
    }
}
