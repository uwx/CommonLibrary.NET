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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.Security.Authentication;

using ComLib.Entities;



namespace ComLib.StatusUpdater
{
    /// <summary>
    /// Service class for StatusUpdate.
    /// </summary>
    public partial class StatusUpdateService : EntityService<StatusUpdate>
    {
        /// <summary>
        /// default construction
        /// </summary>
        public StatusUpdateService()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="repository">Repository with updates.</param>
        /// <param name="validator">Validator.</param>
        /// <param name="settings">Status update settings.</param>
        public StatusUpdateService(IRepository<StatusUpdate> repository, StatusUpdateValidator validator,
                StatusUpdateSettings settings ) : base(repository, validator, settings)
        {
        }
    }
}
