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
using System.Collections.ObjectModel;
using System.Text;
using System.Collections;

using ComLib;


namespace ComLib.Entities
{
    /// <summary>
    /// class used to register the creation of the components of 
    /// the domain model.
    /// </summary>
    /// <remarks>
    /// NOTE: Currently, the entity registration and creation works using
    /// an IocContainer.
    /// </remarks>
    public class EntityRegistration 
    {
        private static IDictionary<string, EntityRegistrationContext> _managableEntities;
        private static IList<string> _managableEntitiesList;


        /// <summary>
        /// Default provider to Ioc
        /// </summary>
        static EntityRegistration()
        {            
            _managableEntities = new Dictionary<string, EntityRegistrationContext>();
            _managableEntitiesList = new List<string>();
        }


        #region Registration members
        /// <summary>
        /// Registers the specified CTX for creating the components
        /// necessary for the DomainModel ActiveRecord.
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        public static void Register(EntityRegistrationContext ctx)
        {
            if (!_managableEntities.ContainsKey(ctx.EntityType.FullName))
            {
                _managableEntities.Add(ctx.EntityType.FullName, ctx);
                _managableEntitiesList.Add(ctx.EntityType.FullName);
            }
            // Overwrite
            else
            {
                _managableEntities[ctx.EntityType.FullName] = ctx;
            }
            // Register.
            if (ctx.IsRepositoryConfigurationRequired && ctx.IsSingletonRepository)
                RepositoryFactory.Configure(ctx.ConnectionId, (IRepositoryConfigurable)ctx.Repository);
        }


        /// <summary>
        /// Register a singleton service/repository for the entity specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="configureRepository"></param>
        public static void Register<T>(IEntityService<T> service, bool configureRepository)
        {
            EntityRegistrationContext ctx = new EntityRegistrationContext();
            ctx.EntityType = typeof(T);
            ctx.Name = typeof(T).FullName;
            ctx.IsSingletonService = true;
            ctx.IsSingletonRepository = true;
            ctx.Service = service;
            ctx.Settings = service.Settings;
            ctx.Repository = service.Repository;
            ctx.IsRepositoryConfigurationRequired = configureRepository;
            ctx.CreationMethod = EntityCreationType.Factory;
            ctx.ActionContextType = typeof(ActionContext<T>);
            ctx.ConnectionId = string.Empty;

            if (ctx.IsRepositoryConfigurationRequired)
                RepositoryFactory.Configure(ctx.ConnectionId, service.Repository);

            Register(ctx);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repo"></param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId"></param>
        public static void Register<T>(IRepository<T> repo, bool configureRepository, string connId) where T : IEntity
        {
            Register<T>(() => new EntityService<T>(), () => repo, () => new EntityValidator(), null, null, configureRepository, connId);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        public static void Register<T>(IRepository<T> repo, bool configureRepository) where T : IEntity
        {
            Register<T>(() => new EntityService<T>(), () => repo, () => new EntityValidator(), null, null, configureRepository, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        public static void Register<T>(Func<IRepository<T>> repoCreator, bool configureRepository) where T : IEntity
        {
            Register<T>(() => new EntityService<T>(), repoCreator, () => new EntityValidator(), null, null, configureRepository, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId"></param>
        public static void Register<T>(Func<IRepository<T>> repoCreator, bool configureRepository, string connId) where T : IEntity
        {
            Register<T>(() => new EntityService<T>(), repoCreator, () => new EntityValidator(), null, null, configureRepository, connId);
        }


        /// <summary>
        /// Initialize the service, repository for the model T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="validatorCreator">The validator creator.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId">The conn id.</param>
        public static void Register<T>(Func<IRepository<T>> repoCreator,
            Func<IEntityValidator> validatorCreator, IEntitySettings settings, IEntityResources resources, bool configureRepository, string connId) where T : IEntity
        {
            Register<T>(() => new EntityService<T>(), repoCreator, validatorCreator, settings, resources, configureRepository, connId);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="validatorCreator">The validator creator.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resources">The resources.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connectionId">The connection id.</param>
        public static void Register<T>(Func<IEntityService<T>> serviceCreator, Func<IRepository<T>> repoCreator, 
            Func<IEntityValidator> validatorCreator, IEntitySettings settings, IEntityResources resources, bool configureRepository, string connectionId)
        {
            EntityRegistrationContext ctx = new EntityRegistrationContext();
            ctx.EntityType = typeof(T);
            ctx.Name = typeof(T).FullName;
            ctx.IsSingletonService = false;
            ctx.IsSingletonRepository = false;
            ctx.IsRepositoryConfigurationRequired = configureRepository;
            ctx.CreationMethod = EntityCreationType.Factory;
            ctx.ActionContextType = typeof(ActionContext<T>);
            ctx.Settings = settings;
            ctx.Resources = resources;
            ctx.FactoryMethodForService = new Func<object>(() => serviceCreator());
            ctx.ConnectionId = connectionId;

            if (repoCreator != null) ctx.FactoryMethodForRepository = new Func<object>(() => repoCreator());
            else if(serviceCreator != null)
            {
                ctx.Repository = serviceCreator().Repository;
            }
            if(validatorCreator != null) ctx.FactoryMethodForValidator = new Func<object>(() => validatorCreator());

            EntityRegistration.Register(ctx);
        }


        /// <summary>
        /// Registers the specified entity type to wire up ActiveRecord functionality.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="repositoryCreator">The repository creator.</param>
        public static void Register(Type entityType, Func<object> serviceCreator, Func<object> repositoryCreator)
        {
            EntityRegistrationContext ctx = null;
            if (_managableEntities.ContainsKey(entityType.FullName))
            {
                ctx = _managableEntities[entityType.FullName];                
            }
            else
            {
                ctx = new EntityRegistrationContext(entityType.Name, entityType, typeof(ActionContext));
            }

            // Setup the properties.
            ctx.CreationMethod = EntityCreationType.Factory;
            ctx.FactoryMethodForService = serviceCreator;
            ctx.FactoryMethodForRepository = repositoryCreator;
            Register(ctx);
        }


        /// <summary>
        /// Registers the specified entity type to wire up ActiveRecord functionality.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="repositoryCreator"></param>
        public static void Register(Type entityType, Func<object> repositoryCreator)
        {
            Register(entityType, null, repositoryCreator);
        }
        #endregion


        #region Contains methods
        /// <summary>
        /// Determine if the entity specified by the type is registered
        /// for being managable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ContainsEntity(Type type)
        {
            return _managableEntities.ContainsKey(type.FullName);
        }


        /// <summary>
        /// Determine if the entity specified by the type is registered
        /// for being managable.
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static bool ContainsEntity(string typeFullName)
        {
            return _managableEntities.ContainsKey(typeFullName);
        }


        /// <summary>
        /// Returns a list of strings representing the names of the 
        /// managable entities.
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<string> GetManagableEntities()
        {
            return new ReadOnlyCollection<string>(_managableEntitiesList);
        }


        /// <summary>
        /// Get the registration context for the entity full name.
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static EntityRegistrationContext GetRegistrationContext(string typeFullName)
        {
            return _managableEntities[typeFullName];
        }
        #endregion


        #region Get methods using generics
        /// <summary>
        /// Get the Domain service associated with the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEntityService<T> GetService<T>()
        {
            object service = GetService(typeof(T).FullName);
            return (IEntityService<T>)service;
        }


        /// <summary>
        /// Get repository with the specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IRepository<T> GetRepository<T>()
        {
            object repository = GetRepository(typeof(T).FullName);
            return (IRepository<T>)repository;
        }


        /// <summary>
        /// Get repository with the specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEntityValidator GetValidator<T>()
        {
            object validator = GetValidator(typeof(T).FullName);
            return (IEntityValidator)validator;
        }


        /// <summary>
        /// Get repository with the specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEntitySettings GetSettings<T>()
        {
            object settings = GetSettings(typeof(T).FullName);
            return (IEntitySettings)settings;
        }


        /// <summary>
        /// Get context object associated with the service of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IActionContext GetContext<T>()
        {
            return new ActionContext<T>();
        }


        /// <summary>
        /// Get context with id set on the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IActionContext GetContext<T>(int id)
        {
            IActionContext ctx = new ActionContext<T>();
            ctx.Id = id;
            ctx.CombineMessageErrors = true;
            ctx.Id = id;
            return ctx;
        }


        /// <summary>
        /// Get context with the entity set on the context.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IActionContext GetContext<T>(IEntity entity)
        {
            IActionContext ctx = new ActionContext<T>();
            ctx.CombineMessageErrors = true;
            ctx.Item = entity;
            return ctx;
        }
        #endregion



        /// <summary>
        /// Get the entity Service(supporting CRUD operations).
        /// </summary>
        /// <param name="typeFullName"></param>
        /// <returns></returns>
        public static object GetService(string typeFullName)
        {
            if (!_managableEntities.ContainsKey(typeFullName))
                throw new ArgumentException("The type : " + typeFullName + " has not been registered in EntityRegistration");

            EntityRegistrationContext ctx = _managableEntities[typeFullName];

            // Singleton.
            if (ctx.IsSingletonService)
                return ctx.Service;

            object service = null;

            if (ctx.CreationMethod == EntityCreationType.Factory)
            {
                service = ctx.FactoryMethodForService();
                if(ctx.FactoryMethodForRepository != null)
                {
                    var repository = GetRepository(ctx);
                    service.GetType().GetProperty("Repository").SetValue(service, repository, null);
                }
                if (ctx.FactoryMethodForValidator != null)
                {
                    var validator = GetValidator(typeFullName);
                    service.GetType().GetProperty("Validator").SetValue(service, validator, null);         
                }                
                if(ctx.Settings != null)
                    service.GetType().GetProperty("Settings").SetValue(service, ctx.Settings, null); 

                return service;
            }
            return ctx.Service;
        }
        

        /// <summary>
        /// Get instance of entity repository.
        /// </summary>
        /// <param name="typeFullName">typeof(T).FullName</param>
        /// <returns></returns>
        public static object GetRepository(string typeFullName)
        {
            if (!_managableEntities.ContainsKey(typeFullName))
                throw new ArgumentException("The type : " + typeFullName + " has not been registered in EntityRegistration");

            EntityRegistrationContext ctx = _managableEntities[typeFullName];
            IRepositoryConfigurable repo = null;

            // If factory method was supplied.
            if (ctx.FactoryMethodForRepository != null)
            {
                repo = ctx.FactoryMethodForRepository() as IRepositoryConfigurable;
                if (repo != null && ctx.IsRepositoryConfigurationRequired)
                    RepositoryFactory.Configure(ctx.ConnectionId, repo);

                return repo;
            }

            return ctx.Repository;
        }


        /// <summary>
        /// Get instance of entity repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static object GetRepository(EntityRegistrationContext ctx)
        {
            IRepositoryConfigurable repo = null;

            // If factory method was supplied.
            if (ctx.FactoryMethodForRepository != null)
            {
                repo = ctx.FactoryMethodForRepository() as IRepositoryConfigurable;
                if (repo != null && ctx.IsRepositoryConfigurationRequired)
                    RepositoryFactory.Configure(ctx.ConnectionId, repo);

                return repo;
            }

            return ctx.Repository;
        }


        /// <summary>
        /// Get instance of entity validator.
        /// </summary>
        /// <param name="typeFullName">typeof(T).FullName</param>
        /// <returns></returns>
        public static object GetValidator(string typeFullName)
        {
            EntityRegistrationContext ctx = _managableEntities[typeFullName];
            if (ctx.FactoryMethodForValidator != null)
                return ctx.FactoryMethodForValidator();

            return ctx.Validator;
        }


        /// <summary>
        /// Determines whether the specified type has validator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type has validator; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidator(Type type)
        {
            EntityRegistrationContext ctx = null;
            if (!_managableEntities.ContainsKey(type.FullName))
            {
                if (_managableEntities.ContainsKey(type.BaseType.FullName))
                {
                    ctx = _managableEntities[type.BaseType.FullName];
                }
            }
            else
            {
                ctx = _managableEntities[type.FullName];
            }

            if (ctx == null) return false;
            if (ctx.FactoryMethodForValidator == null && 
                ctx.Validator == null ) return false;

            return true;
        }


        /// <summary>
        /// Get instance of entity validator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetValidator(Type type)
        {
            EntityRegistrationContext ctx = _managableEntities[type.FullName];
            if (ctx.FactoryMethodForValidator != null)
                return ctx.FactoryMethodForValidator();

            return ctx.Validator;
        }


        /// <summary>
        /// Get instance of entity settings.
        /// </summary>
        /// <param name="typeFullName">typeof(T).FullName</param>
        /// <returns></returns>
        public static object GetSettings(string typeFullName)
        {
            EntityRegistrationContext ctx = _managableEntities[typeFullName];
            return ctx.Settings;
        }


        /// <summary>
        /// Get instance of entity context.
        /// </summary>
        /// <param name="typeFullName">typeof(T).FullName</param>
        /// <returns></returns>
        public static IActionContext GetContext(string typeFullName)
        {
            EntityRegistrationContext ctx = _managableEntities[typeFullName];
            return new ActionContext();
        }


        /// <summary>
        /// Get instance of entity.
        /// </summary>
        /// <param name="typeFullName">typeof(T).FullName</param>
        /// <returns></returns>
        public static object GetEntity(string typeFullName)
        {
            //return _entityRegistrarIoc.GetEntity(typeFullName);
            return null;
        }


        
        #region Factory helpers
        private static T Create<T>(ref T objectRef, object syncRoot, Func<T> creator)
        {
            if (objectRef == null)
            {
                lock (syncRoot)
                {
                    if (objectRef == null)
                    {
                        objectRef = creator();
                    }
                }
            }
            return objectRef;
        }
        #endregion
    }
}
