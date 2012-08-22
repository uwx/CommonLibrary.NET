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
    /// Specifies how to create the service for the entity.
    /// If set to IocWithNamingConvention names will have to be :
    /// 1. Service      "(EntityName)Service"
    /// 2. Repository   "(EntityName)Repository"
    /// 3. Validator    "(EntityName)Valdiator"
    /// 4. Controller   "(EntityName)Controller"
    /// 5. Settings     "(EntityName)Settings"
    /// </summary>
    public enum EntityCreationType { 
        /// <summary>
        /// Ioc with naming convention.
        /// </summary>
        IocWithNamingConvention, 
        
        
        /// <summary>
        /// Factory generation.
        /// </summary>
        Factory };



    /// <summary>
    /// This represents whether or not to create a Service/Repository/Validator
    /// and how to create it.
    /// </summary>
    public class EntityRegistrationContext
    {
        #region DataType
        /// <summary>
        /// The prefix used for naming all the various components in the DomainModel
        /// </summary>
        public string Name = "";


        /// <summary>
        /// Id of the connection string to use.
        /// </summary>
        public string ConnectionId;


        /// <summary>
        /// The type of the entity.
        /// Post or CommonLibrary.NamedQuery
        /// </summary>
        public Type EntityType;        


        /// <summary>
        /// Get the type of the context used for an entity service.
        /// e.g. type that extends IActionContext
        /// </summary>
        public Type ActionContextType;
        #endregion


        #region Singleton objects
        /// <summary>
        /// Singleton service.
        /// </summary>
        public object Service;


        /// <summary>
        /// Singleton repository
        /// </summary>
        public object Repository;


        /// <summary>
        /// Validator for the entity.
        /// </summary>
        public object Validator;


        /// <summary>
        /// The settings object for the type.
        /// </summary>
        public object Settings;


        /// <summary>
        /// Resource strings for the object.
        /// </summary>
        public object Resources;
        #endregion
        

        #region Factory Methods
        /// <summary>
        /// Reference to the factory-like method for creating the entityService.
        /// </summary>
        public Func<object> FactoryMethodForService = null;


        /// <summary>
        /// Factory for the repository.
        /// </summary>
        public Func<object> FactoryMethodForRepository = null;


        /// <summary>
        /// Validator creator.
        /// </summary>
        public Func<object> FactoryMethodForValidator = null;
        #endregion


        #region Flags for singleton
        /// <summary>
        /// Singleton service.
        /// </summary>
        public bool IsSingletonService;


        /// <summary>
        /// Singleton repository.
        /// </summary>
        public bool IsSingletonRepository;


        /// <summary>
        /// Singleton validator.
        /// </summary>
        public bool IsSingletonValidator;


        /// <summary>
        /// Does the repository need to be configured w/ the connection?
        /// </summary>
        public bool IsRepositoryConfigurationRequired;


        /// <summary>
        /// The approach used to create the components used in the ActiveRecord DomainModel.
        /// </summary>
        public EntityCreationType CreationMethod = EntityCreationType.Factory;
        #endregion


        #region Flags for life-cycle events
        /// <summary>
        /// True if before validate callback exists.
        /// </summary>
        public bool HasOnBeforeValidate;


        /// <summary>
        /// True if before validate create callback exists.
        /// </summary>
        public bool HasOnBeforeValidateCreate;


        /// <summary>
        /// True if before validate update callback exists.
        /// </summary>
        public bool HasOnBeforeValidateUpdate;


        /// <summary>
        /// True if before create callback exists.
        /// </summary>
        public bool HasOnBeforeCreate;


        /// <summary>
        /// True if before update callback exists.
        /// </summary>
        public bool HasOnBeforeUpdate;


        /// <summary>
        /// True if before save callback exists.
        /// </summary>
        public bool HasOnBeforeSave;


        /// <summary>
        /// True if before delete callback exists.
        /// </summary>
        public bool HasOnBeforeDelete;


        /// <summary>
        /// True if before initialize callback exists.
        /// </summary>
        public bool HasOnAfterInitialize;


        /// <summary>
        /// True if after validate callback exists.
        /// </summary>
        public bool HasOnAfterValidate;


        /// <summary>
        /// True if after validate create callback exists.
        /// </summary>
        public bool HasOnAfterValidateCreate;


        /// <summary>
        /// True if after validate update callback exists.
        /// </summary>
        public bool HasOnAfterValidateUpdate;


        /// <summary>
        /// True if after create callback exists.
        /// </summary>
        public bool HasOnAfterCreate;


        /// <summary>
        /// True if after update callback exists.
        /// </summary>
        public bool HasOnAfterUpdate;


        /// <summary>
        /// True if after save callback exists.
        /// </summary>
        public bool HasOnAfterSave;


        /// <summary>
        /// True if after delete callback exists.
        /// </summary>
        public bool HasOnAfterDelete;
        #endregion


        #region Flags for Error handling
        /// <summary>
        /// Gets or sets a value indicating whether this instance is exception thrown.
        /// </summary>
        /// <value><c>true</c> if this instance is exception thrown; otherwise, <c>false</c>.</value>
        public bool IsExceptionThrownOnCrudFailure { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is entity error collection populated.
        /// </summary>
        /// <value><c>true</c> if this instance is entity error collection populated; otherwise, <c>false</c>.</value>
        public bool IsEntityErrorCollectionPopulated { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is entity error collection cleared first.
        /// </summary>
        /// <value><c>true</c> if this instance is entity error collection cleared first; otherwise, <c>false</c>.</value>
        public bool IsEntityErrorCollectionClearedBeforePopulation { get; set; }
        #endregion


        /// <summary>
        /// Allow default construction.
        /// </summary>
        public EntityRegistrationContext() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRegistrationContext"/> class.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityType"></param>
        /// <param name="contextType"></param>
        public EntityRegistrationContext(string entityName, Type entityType, Type contextType)
        {
            Name = entityName;
            EntityType = entityType;
            ActionContextType = contextType;

            // TO_DO:Currently defaulting to IoC creation method.
            CreationMethod = EntityCreationType.IocWithNamingConvention;
        }
    }
}
