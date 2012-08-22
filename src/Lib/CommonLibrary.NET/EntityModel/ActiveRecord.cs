/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: ? 2009 Kishore Reddy
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
using System.Text;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

using ComLib;
using ComLib.Data;

namespace ComLib.Entities
{

    /// <summary>
    /// Provides Domain Object and Active record support.
    /// 1. Crud methods
    /// 2. Find methods
    /// 3. Domain Object methods - validation.
    /// </summary>
    /// <remarks>
    /// If .NET supported multiple inheritance, this class would extend from
    /// both DomainObject, and ActiveRecord, however the IActiveRecord interface
    /// has to be implemented in this class.
    /// 1. Possible alternatives are extension methods 
    /// 2. delegation.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class ActiveRecordBaseEntity<T> : Entity<T> where T : class, IEntity, new()
    {

        #region Static Active Record Initialization
        /// <summary>
        /// Initialize the behaviour of creating the service and repository.
        /// </summary>
        /// <param name="service">The entity service</param>
        public static void Init(IEntityService<T> service)
        {
            Init(service, false);
        }


        /// <summary>
        /// Singleton service and repository with optional flag to indicate 
        /// whether or not to configure the repository.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="configureRepository"></param>
        public static void Init(IEntityService<T> service, bool configureRepository)
        {
            var serviceCreator = new Func<IEntityService<T>>(() => service);
            IEntitySettings settings = service.Settings == null ? null : service.Settings; 
            Init(serviceCreator, settings, configureRepository);
        }


        /// <summary>
        /// Singleton repository with optional flag to indicate
        /// whether or not to configure the repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="configureRepository">if set to <c>true</c> [configure repository].</param>
        public static void Init(IRepository<T> repository, bool configureRepository)
        {
            var repoCreator = new Func<IRepository<T>>(() => repository);
            Init(repoCreator, configureRepository, string.Empty);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        public static void Init(Func<IEntityService<T>> serviceCreator, bool configureRepository)
        {
            Init(serviceCreator, null, null, new EntitySettings<T>(), new EntityResources(), configureRepository, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="settings"></param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        public static void Init(Func<IEntityService<T>> serviceCreator, IEntitySettings settings, bool configureRepository)
        {
            var settingsToUse = settings == null ? new EntitySettings<T>() : settings;
            Init(serviceCreator, null, null, settingsToUse, new EntityResources(), configureRepository, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        public static void Init(Func<IRepository<T>> repoCreator, bool configureRepository)
        {
            var serviceCreator = new Func<IEntityService<T>>(() => new EntityService<T>()); 
            Init(serviceCreator, repoCreator, null, new EntitySettings<T>(), new EntityResources(), configureRepository, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId">The connId to use when configuring the repository.</param>
        public static void Init(Func<IRepository<T>> repoCreator, bool configureRepository, string connId)
        {
            var serviceCreator = new Func<IEntityService<T>>(() => new EntityService<T>());
            Init(serviceCreator, repoCreator, null, new EntitySettings<T>(), new EntityResources(), configureRepository, connId);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="settings">The settings for the entity.</param>
        /// <param name="configureRepo">Whether or not to configure the reposiory.</param>
        public static void Init(Func<IRepository<T>> repoCreator, IEntitySettings settings, bool configureRepo)
        {
            var serviceCreator = new Func<IEntityService<T>>(() => new EntityService<T>());
            Init(serviceCreator, repoCreator, null, settings, new EntityResources(), configureRepo, null);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="validatorCreator">The validator creator</param>
        /// <param name="settings">The settings for the entity.</param>
        /// <param name="configureRepo">Whether or not to configure the reposiory.</param>
        /// <param name="connId"></param>
        public static void Init(Func<IRepository<T>> repoCreator, Func<IEntityValidator> validatorCreator, IEntitySettings settings, bool configureRepo, string connId)
        {
            var serviceCreator = new Func<IEntityService<T>>(() => new EntityService<T>());
            Init(serviceCreator, repoCreator, validatorCreator, settings, new EntityResources(), configureRepo, connId);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="validatorCreator"></param>
        /// <param name="settings"></param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId"></param>
        public static void Init(Func<IEntityService<T>> serviceCreator, Func<IRepository<T>> repoCreator, Func<IEntityValidator> validatorCreator, IEntitySettings settings, bool configureRepository, string connId)
        {
            Init(serviceCreator, repoCreator, validatorCreator, settings, new EntityResources(), configureRepository, connId);
        }


        /// <summary>
        /// Initialize the service, repository creators.
        /// </summary>
        /// <param name="serviceCreator">The service creator.</param>
        /// <param name="repoCreator">The repository creator.</param>
        /// <param name="validatorCreator"></param>
        /// <param name="settings"></param>
        /// <param name="resources"></param>
        /// <param name="configureRepository">Whether or not to configure the reposiory.</param>
        /// <param name="connId"></param>
        public static void Init(Func<IEntityService<T>> serviceCreator, Func<IRepository<T>> repoCreator, Func<IEntityValidator> validatorCreator,
                                IEntitySettings settings, IEntityResources resources, bool configureRepository, string connId)
        {
            EntityRegistration.Register<T>(serviceCreator, repoCreator, validatorCreator, settings, resources, configureRepository, connId );
            var ctx = EntityRegistration.GetRegistrationContext(typeof(T).FullName);
            MethodInfo callback = typeof(T).GetMethod("OnAfterInit");
            if(callback != null) callback.Invoke(null, null);

            // Setup flags for entity life-cycle callbacks.
            ctx.HasOnBeforeValidate = HasMethod("OnBeforeValidate");
            ctx.HasOnBeforeValidateCreate = HasMethod("OnBeforeValidateCreate");
            ctx.HasOnBeforeValidateUpdate = HasMethod("OnBeforeValidateUpdate");
            ctx.HasOnBeforeCreate = HasMethod("OnBeforeCreate");
            ctx.HasOnBeforeUpdate = HasMethod("OnBeforeUpdate");
            ctx.HasOnBeforeSave = HasMethod("OnBeforeSave");
            ctx.HasOnBeforeDelete = HasMethod("OnBeforeDelete"); 
            
            ctx.HasOnAfterValidate = HasMethod("OnAfterValidate");
            ctx.HasOnAfterValidateCreate = HasMethod("OnAfterValidateCreate");
            ctx.HasOnAfterValidateUpdate = HasMethod("OnAfterValidateUpdate");
            ctx.HasOnAfterCreate = HasMethod("OnAfterCreate");
            ctx.HasOnAfterUpdate = HasMethod("OnAfterUpdate");
            ctx.HasOnAfterSave = HasMethod("OnAfterSave");
            ctx.HasOnAfterDelete = HasMethod("OnAfterDelete");
        }

        private static bool HasMethod(string methodName)
        {
            MethodInfo m = typeof(T).GetMethod(methodName) ;
            bool hasMethod = m != null && m.DeclaringType != typeof(Entity);
            return hasMethod;
        }
        #endregion


        #region Static Active Record CRUD Methods
        /// <summary>
        /// Creates the entity
        /// </summary>
        public static void Create(T entity)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Create(entity);
        }


        /// <summary>
        /// Creates the entities.
        /// </summary>
        public static void Create(IList<T> entities)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Create(entities);
        }


        /// <summary>
        /// Creates the entities conditionally based on whether they exists in the datastore.
        /// Existance in the datastore is done by finding any entities w/ matching values for the 
        /// <paramref name="checkFields"/> supplied.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="checkFields"></param>
        public static void Create(IList<T> entities, params Expression<Func<T, object>>[] checkFields)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Create(entities, checkFields);
        }


        /// <summary>
        /// Updates the entity.
        /// </summary>
        public static void Update(T entity)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Update(entity);
        }


        /// <summary>
        /// Updates the entities.
        /// </summary>
        public static void Update(IList<T> entities)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Update(entities);
        }


        /// <summary>
        /// Saves the entity.
        /// </summary>
        public static void Save(T entity)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Save(entity);
        }


        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <returns></returns>
        public static void Delete(T entity)
        {
            DoEntityAction(entity, (context, service) => service.Delete(context));
        }


        /// <summary>
        /// Deletes all the entities from the system.
        /// </summary>
        /// <returns></returns>
        public static new void DeleteAll()
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.DeleteAll();
        }


        /// <summary>
        /// Delete the model associated with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Delete(int id)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Delete(id);
        }


        /// <summary>
        /// Retrieve the model associated with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Get(int id)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            T item = service.Get(id);
            return item;
        }


        /// <summary>
        /// Retrieve all instances of model.
        /// </summary>
        /// <returns></returns>
        public static new IList<T> GetAll()
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            IList<T> result = service.GetAll();
            return result;
        }


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public static PagedList<T> Get(int pageNumber, int pageSize)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            PagedList<T> result = service.Get(pageNumber, pageSize);
            return result;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <returns></returns>
        public static T First(string filter)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            T result = service.First(filter);
            return result;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public static PagedList<T> Find(string filter, int pageNumber, int pageSize)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            PagedList<T> result = service.Find(filter, pageNumber, pageSize);
            return result;
        }


        /// <summary>
        /// Get items by page using Criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public static PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            PagedList<T> result = service.Find(criteria, pageNumber, pageSize);
            return result;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter">e.g. "UserNameLowered = 'kishore'"</param>
        /// <returns></returns>
        public static IList<T> Find(string filter)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            IList<T> result = service.Find(filter);
            return result;
        }


        /// <summary>
        /// Get items by page using Criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IList<T> Find(IQuery criteria)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            IList<T> result = service.Find(criteria);
            return result;
        }


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public static PagedList<T> GetRecent(int pageNumber, int pageSize)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            PagedList<T> result = service.GetRecent(pageNumber, pageSize);
            return result;
        }


        /// <summary>
        /// Increments the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public static void Increment(Expression<Func<T, object>> member, int by, int id)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Increment(member, by, id);
        }


        /// <summary>
        /// Decrements the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public static void Decrement(Expression<Func<T, object>> member, int by, int id)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            service.Decrement(member, by, id);
        }


        /// <summary>
        /// The the total count of entities.
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            IRepository<T> repo = EntityRegistration.GetRepository<T>();
            int count = repo.Count();
            return count;
        }
        #endregion


        #region Static Public Methods
        /// <summary>
        /// Get the repository associated w/ this entity.
        /// </summary>
        /// <returns></returns>
        public static IRepository<T> Repository
        {
            get
            {
                return EntityRegistration.GetRepository<T>();
            }
        }
        #endregion


        #region Static Protected methods
        /// <summary>
        /// Performs the actual entity action specified by the delegate <paramref name="executor"/>
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executor"></param>
        /// <returns></returns>
        protected static void DoEntityAction(T entity, Action<IActionContext, IEntityService<T>> executor)
        {
            IActionContext ctx = EntityRegistration.GetContext<T>();
            ctx.CombineMessageErrors = true;
            ctx.Item = entity;
            IEntityService<T> service = EntityRegistration.GetService<T>();
            executor(ctx, service);
        }
        #endregion
    }
}
