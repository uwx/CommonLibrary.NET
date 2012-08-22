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



namespace ComLib.Account
{
    /// <summary>
    /// Service class for User.
    /// </summary>
    public partial class UserService : EntityService<User>, IAccountService
    {
        /// <summary>
        /// default construction
        /// </summary>
        public UserService()
        {
        }


        /// <summary>
        /// Initialize model with only the repository.
        /// </summary>
        /// <param name="repository">Repository for entity.</param>
        public UserService(IRepository<User> repository) : base(repository, null, null)
        {
        }


        /// <summary>
        /// Initialize model with repository and settings.
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="settings">Settings</param>
        public UserService(IRepository<User> repository, IEntitySettings settings)
            : base(repository, null, settings)
        {
        }


        /// <summary>
        /// Initialize the model w/ repository, validator, and it's settings.
        /// </summary>
        /// <param name="repository">Repository for the model.</param>
        /// <param name="validator">Validator for model.</param>
        /// <param name="settings">Settings for the model.</param>
        public UserService(IRepository<User> repository, IEntityValidator validator,
                IEntitySettings settings ) : base(repository, validator, settings)
        {
        }
    }



    /// <summary>
    /// Settings class for User.
    /// </summary>
    public partial class UserSettings : EntitySettings<User>, IEntitySettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettings"/> class.
        /// </summary>
        public UserSettings()
        {
            Init();
        }


        /// <summary>
        /// Initalize settings.
        /// </summary>
        public override void Init()
        {
            EditRoles = "";
            EnableValidation = true;
        }
    }
}
