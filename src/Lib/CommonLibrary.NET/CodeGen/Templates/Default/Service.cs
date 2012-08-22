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



namespace <%= model.NameSpace %>
{
    /// <summary>
    /// Service class for <%= model.Name %>.
    /// </summary>
    public partial class <%= model.Name %>Service : EntityService<<%= model.Name %>>
    {
        /// <summary>
        /// default construction
        /// </summary>
        public <%= model.Name %>Service()
        {
        }


        public <%= model.Name %>Service(IEntityRepository<<%= model.Name %>> repository, <%= model.Name %>Validator validator,
                <%= model.Name %>Settings settings ) : base(repository, validator, settings)
        {
        }


        /// <summary>
        /// Initialize model with only the repository.
        /// </summary>
        /// <param name="repository">Repository for entity.</param>
        public <%= model.Name %>Service(IEntityRepository<<%= model.Name %>> repository) : base(repository, null, null)
        {
        }


        /// <summary>
        /// Initialize model with repository and settings.
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="settings">Settings</param>
        public <%= model.Name %>Service(IEntityRepository<<%= model.Name %>> repository, IEntitySettings<<%= model.Name %>> settings)
            : base(repository, null, settings)
        {
        }


        /// <summary>
        /// Initialize the model w/ repository, validator, and it's settings.
        /// </summary>
        /// <param name="repository">Repository for the model.</param>
        /// <param name="validator">Validator for model.</param>
        /// <param name="settings">Settings for the model.</param>
        public <%= model.Name %>Service(IEntityRepository<<%= model.Name %>> repository, IEntityValidator<<%= model.Name %>> validator,
                IEntitySettings<<%= model.Name %>> settings ) : base(repository, validator, settings)
        {
        }
    }



    /// <summary>
    /// Data massager for an entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class <%= model.Name %>Massager : EntityMassager
    {        
    }



    /// <summary>
    /// Settings class for <%= model.Name %>.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class <%= model.Name %>Settings : EntitySettings<<%= model.Name %>>, IEntitySettings<<%= model.Name %>>
    {
        public <%= model.Name %>Settings()
        {            
            Init();
        }


        /// <summary>
        /// Initalize settings.
        /// </summary>
        public override void Init()
        {
            EditRoles = "<%= model.EditRoles %>";
            EnableValidation = true;
        }
    }


    
    /// <summary>
    /// Settings class for <%= model.Name %>.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class <%= model.Name %>Resources : EntityResources
    {
        public <%= model.Name %>Resources()
        {
            _entityName = "<%= model.Name %>";
        }
    }
}
