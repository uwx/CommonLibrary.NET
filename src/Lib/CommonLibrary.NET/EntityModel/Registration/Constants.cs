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


namespace ComLib.Entities
{
    /// <summary>
    /// This class contains entity registration constants.
    /// </summary>
    public class EntityRegistrationConstants
    {        
        /// <summary>
        /// suffix used for the naming the DomainModel service object.
        /// e.g. "reportservice"
        /// </summary>
        public const string SuffixService = "Service";


        /// <summary>
        /// suffix used for the naming the DomainModel service object.
        /// e.g. "reportvalidator"
        /// </summary>
        public const string SuffixValidator = "Validator";


        /// <summary>
        /// suffix used for the naming the DomainModel service object.
        /// e.g. "reportrepository"
        /// </summary>
        public const string SuffixRepository = "Repository";


        /// <summary>
        /// suffix used for the naming the DomainModel service object.
        /// e.g. "reportcontroller"
        /// </summary>
        public const string SuffixController = "Controller";


        /// <summary>
        /// suffix used for the naming the DomainModel service object.
        /// e.g. "reportsettings"
        /// </summary>
        public const string SuffixSettings = "Settings";
    }
}
