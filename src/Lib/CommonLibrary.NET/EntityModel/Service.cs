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
using System.Linq;
using System.Linq.Expressions;

using ComLib;
using ComLib.Data;
using ComLib.Logging;
using ComLib.Exceptions;
using ComLib.Authentication;
using ComLib.ValidationSupport;
using ComLib.Entities.Extensions;

namespace ComLib.Entities
{
    
    /// <summary>
    /// Service class used for handling entity actions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityService<T> : IEntityService<T> where T : IEntity
    {
        /// <summary>
        /// The underlying repository for the entities.
        /// </summary>
        protected IRepository<T> _repository;


        /// <summary>
        /// Settings class for the entity
        /// </summary>
        protected IEntitySettings _settings;


        /// <summary>
        /// The validator for the entity.
        /// </summary>
        protected IEntityValidator _validator;


        /// <summary>
        /// Localization strings for the entity.
        /// </summary>
        protected IEntityResources _resources;


        /// <summary>
        /// Initializes a new instance using defaults
        /// </summary>
        public EntityService() 
        {
            Init();
        }


        /// <summary>
        /// Initialize with Repository and validator.
        /// </summary>
        /// <param name="repository">PesistantStorage for the entity.</param>
        /// <param name="settings">Settings for the entity.</param>
        /// <param name="validator">Validator for the entity</param>
        public EntityService(IRepository<T> repository, IEntityValidator validator, IEntitySettings settings)
        {
            _repository = repository;
            if (validator != null) 
                _validator = validator;

            if (settings != null)  
                _settings  = settings;

            Init();
        }

        
        #region IEntityService<T> Members
        /// <summary>
        /// This method is useful for derived classes to implement 
        /// their own initialization behaviour.
        /// </summary>
        public virtual void Init() { }


        /// <summary>
        /// The resposity for the model.
        /// </summary>
        /// <value></value>
        public virtual IRepository<T> Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }


        /// <summary>
        /// Resources used for the service.
        /// </summary>
        public virtual IEntityResources Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }


        /// <summary>
        /// The validator for the model.
        /// </summary>
        /// <value></value>
        public virtual IEntityValidator Validator
        {
            get { return GetValidator(); }
            set { _validator = value; }
        }


        /// <summary>
        /// Configuration settings for the service.
        /// </summary>
        /// <value></value>
        public virtual IEntitySettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        /// <summary>
        /// Get the settings as a typed object.
        /// </summary>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        public virtual TSettings GetSettings<TSettings>() where TSettings : class
        {
            return _settings as TSettings;
        }

        
        /// <summary>
        /// Get the validator.
        /// </summary>
        /// <returns></returns>
        public virtual IEntityValidator GetValidator()
        {
            return _validator;
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Create(T entity)
        {
            IActionContext ctx = new ActionContext(entity, entity.Errors);
            InternalCreate(ctx, null);
        }



        /// <summary>
        /// Create multiple entities.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Create(IList<T> entities)
        {
            if (entities == null || entities.Count == 0)
                return;

            IActionContext ctx = new ActionContext();
            foreach (var entity in entities)
            {
                ctx.Item = entity;
                ctx.Errors = entity.Errors;
                InternalCreate(ctx, null);
            }
        }


        /// <summary>
        /// Creates the entities conditionally based on whether they exists in the datastore.
        /// Existance in the datastore is done by finding any entities w/ matching values for the 
        /// <paramref name="checkFields"/> supplied.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="checkFields"></param>
        public void Create(IList<T> entities, params Expression<Func<T, object>>[] checkFields)
        {
            if (entities == null || entities.Count == 0)
                return;

            IActionContext ctx = new ActionContext();
            foreach (var entity in entities)
            {
                ctx.Item = entity;
                InternalCreate(ctx, context => _repository.Create(entity, checkFields));
            }
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual void Create(IActionContext ctx)
        {
            if(ctx.Item != null)
                InternalCreate(ctx, null);
            else if (ctx.Items != null && ctx.Items is IList<T>)
            {
                IList<T> items = ctx.Items as IList<T>;
                foreach (T item in items)
                {
                    ctx.Item = item;
                    InternalCreate(ctx, null);
                }
            }
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="executor">The executor to call to create the entity. 
        /// If empty, _repository.Create(entity) is automatically called.</param>
        /// <returns></returns>
        protected virtual void InternalCreate(IActionContext ctx, Action<IActionContext> executor)
        {
            // If the ctx errors are null, then use the errors from the entity.errors collection.
            // This is used for the following use cases:
            // 1. Error collecting into separate unified error collection.
            // 2. Import / Export
            // 3. Batch processing
            var entityCtx = EntityRegistration.GetRegistrationContext(typeof(T).FullName);
            IEntity entity = ctx.Item as IEntity;

            // Do the action(create)
            var result = PerformAction(ctx, context => 
            { 
                // 1. Clear errors first?
                if (entityCtx.IsEntityErrorCollectionClearedBeforePopulation)
                    entity.Errors.Clear();

                // 2. Life-cycle callbacks before validation.
                if (entityCtx.HasOnBeforeValidate) entity.OnBeforeValidate(ctx);
                if (entityCtx.HasOnBeforeValidateCreate) entity.OnBeforeValidateCreate(ctx);

                // 3: Validate the entity.
                var validResult = PerformValidation(ctx, EntityAction.Create);
                if (!validResult.Success)
                    return;

                // 4. Life-cycle callbacks after validation.
                if (entityCtx.HasOnAfterValidateCreate) entity.OnAfterValidateCreate();
                if (entityCtx.HasOnAfterValidate) entity.OnAfterValidate();
                if (entityCtx.HasOnBeforeSave) entity.OnBeforeSave(ctx);
                if (entityCtx.HasOnBeforeCreate) entity.OnBeforeCreate(ctx);            

                // 5. Persist the entity.           
                if (executor == null)
                    _repository.Create((T)context.Item);
                else
                    executor(context);

                // 6. Life-cycle callbacks after persistance.
                if (entityCtx.HasOnAfterCreate) entity.OnAfterCreate();
                if (entityCtx.HasOnAfterSave) entity.OnAfterSave();
            
            }, EntityAction.Create);
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Update(T entity)
        {
            IActionContext ctx = new ActionContext(entity, entity.Errors);
            InternalUpdate(ctx, null);
        }


        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(IList<T> entities)
        {
            if (entities == null || entities.Count == 0)
                return;

            IActionContext ctx = new ActionContext();
            foreach (var entity in entities)
            {
                ctx.Item = entity;
                InternalUpdate(ctx, null);
            }
        }


        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual void Update(IActionContext ctx)
        {
            InternalUpdate(ctx, null);
        }


        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="executor">The action to execute</param>
        /// <returns></returns>
        protected virtual void InternalUpdate(IActionContext ctx, Action<IActionContext> executor)
        {
            var entityCtx = EntityRegistration.GetRegistrationContext(typeof(T).FullName);
            IEntity entity = ctx.Item as IEntity;

            // Do the action(create)
            var result = PerformAction(ctx, context =>
            {
                // 1. Clear errors first?
                if (entityCtx.IsEntityErrorCollectionClearedBeforePopulation)
                    entity.Errors.Clear();

                // 2. Life-cycle callbacks before validation.
                if (entityCtx.HasOnBeforeValidate) entity.OnBeforeValidate(ctx);
                if (entityCtx.HasOnBeforeValidateUpdate) entity.OnBeforeValidateUpdate(ctx);

                // 3: Validate the entity.
                var validResult = PerformValidation(ctx, EntityAction.Update);
                if (!validResult.Success)
                    return;

                // 4. Life-cycle callbacks after validation.
                if (entityCtx.HasOnAfterValidateUpdate) entity.OnAfterValidateUpdate();
                if (entityCtx.HasOnAfterValidate) entity.OnAfterValidate();
                if (entityCtx.HasOnBeforeSave) entity.OnBeforeSave(ctx);
                if (entityCtx.HasOnBeforeUpdate) entity.OnBeforeUpdate(ctx);

                // 5. Persist the entity.    
                if (executor == null)
                    _repository.Update((T)context.Item);
                else
                    executor(context);

                // 6. Life-cycle callbacks after persistance.
                if (entityCtx.HasOnAfterUpdate) entity.OnAfterUpdate();
                if (entityCtx.HasOnAfterSave) entity.OnAfterSave();

            }, EntityAction.Update);
        }


        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Save(T entity)
        {
            IActionContext ctx = new ActionContext(entity);
            Save(ctx);
        }


        /// <summary>
        /// Updates the model in the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual void Save(IActionContext ctx)
        {
            T item = (T)ctx.Item;
            if (item.IsPersistant())
                Update(ctx);
            else
                Create(ctx);
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Delete(T entity)
        {
            IActionContext ctx = new ActionContext(entity);
            Delete(ctx);
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual void Delete(int id)
        {
            IActionContext ctx = new ActionContext();
            ctx.Id = id;
            Delete(ctx);
        }


        /// <summary>
        /// Delete all entities from the system.
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteAll()
        {
            try
            {
                _repository.DeleteAll();
            }
            catch (Exception ex)
            {
                string errMsg = "Unable to delete all entities from : " + typeof(T).Name;
                Logger.Error(errMsg, ex);
            }
        }


        /// <summary>
        /// Deletes the model from the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual void Delete(IActionContext ctx)
        {
            var entityCtx = EntityRegistration.GetRegistrationContext(typeof(T).FullName);
            T entity = default(T);

            if (ctx.Item != null )
                entity = (T)ctx.Item;
            else 
            {
                entity = Get(ctx.Id);
                ctx.Item = entity;
            }

            // already deleted
            if (entity == null) return;

            var result = PerformAction(ctx, delegate(IActionContext context)
            {
                if (entityCtx.HasOnBeforeDelete)
                    entity.OnBeforeDelete(ctx);

                if (context.Item == null)
                    _repository.Delete(context.Id);
                else
                    _repository.Delete((T)context.Item);

                if (entityCtx.HasOnAfterDelete)
                    entity.OnAfterDelete();
            }, 
            EntityAction.Delete);
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(int id)
        {
            IActionContext ctx = new ActionContext();
            ctx.Id = id;
            return Get(ctx);
        }


        /// <summary>
        /// Retrieves the model from the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual T Get(IActionContext ctx)
        {
            T item = default(T);
            try
            {
                item = _repository.Get(ctx.Id);                
            }
            catch (Exception ex)
            {
                string error = "Unable to retrieve " + typeof(T).Name + " with : " + ctx.Id;
                ErrorManager.Handle(error, ex, ctx.Errors);
            }
            return item;
        }


        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll()
        {
            IActionContext ctx = new ActionContext();
            return GetAll(ctx);
        }


        /// <summary>
        /// Retrieves all the instances of the model from the repository
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll(IActionContext ctx)
        {
            IList<T> items = null;
            try
            {
                items = _repository.GetAll();
            }
            catch (Exception ex)
            {
                string error = "Unable to retrieve all items of type " + typeof(T).Name;
                ErrorManager.Handle(error, ex, ctx.Errors);
            }
            return items;
        }


        /// <summary>
        /// Retrieves all the instances of the model from the repository
        /// </summary>
        /// <returns></returns>
        public virtual IList GetAllItems(IActionContext ctx)
        {
            IList items = null;
            try
            {
                items = _repository.GetAllItems();
            }
            catch (Exception ex)
            {
                string error = "Unable to retrieve all items of type " + typeof(T).Name;
                ErrorManager.Handle(error, ex, ctx.Errors);
            }
            return items;
        }


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public PagedList<T> Get(int pageNumber, int pageSize)
        {
            PagedList<T> pagedList = _repository.Find(string.Empty, pageNumber, pageSize);
            return pagedList;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter">The sql filter</param>
        /// <param name="pageNumber">Page number to get. e.g. 1</param>
        /// <param name="pageSize">Number of records per page. e.g. 15</param>
        /// <returns></returns>
        public PagedList<T> Find(string filter, int pageNumber, int pageSize)
        {
            PagedList<T> pagedList = _repository.Find(filter, pageNumber, pageSize);
            return pagedList;
        }


        /// <summary>
        /// Get items by page using the specified criteria.
        /// </summary>
        /// <param name="query">The query</param>
        /// <param name="pageNumber">Page number to get. e.g. 1</param>
        /// <param name="pageSize">Number of records per page. e.g. 15</param>
        /// <returns></returns>
        public PagedList<T> Find(IQuery query, int pageNumber, int pageSize)
        {
            PagedList<T> pagedList = _repository.Find(query, pageNumber, pageSize);
            return pagedList;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <returns></returns>
        public T First(string filter)
        {
            T result = _repository.First(filter);
            return result;
        }


        /// <summary>
        /// Get items by page and by the user who created them.
        /// </summary>
        /// <param name="userName">Name of user who created the entities.</param>
        /// <param name="pageNumber">Page number to get. e.g. 1</param>
        /// <param name="pageSize">Number of records per page. e.g. 15</param>
        /// <returns></returns>
        public PagedList<T> FindByUser(string userName, int pageNumber, int pageSize)
        {
            string filter = "CreateUser = '" + userName + "'";
            return Find(filter, pageNumber, pageSize);
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter">The sql filter string to use.</param>
        /// <returns></returns>
        public IList<T> Find(string filter)
        {
            IList<T> items = _repository.Find(filter, false);
            return items;
        }


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <returns></returns>
        public IList<T> Find(IQuery criteria)
        {
            IList<T> items = _repository.Find(criteria);
            return items;
        }


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public PagedList<T> GetRecent(int pageNumber, int pageSize)
        {
            PagedList<T> pagedList = _repository.FindRecent(pageNumber, pageSize);
            return pagedList;
        }


        /// <summary>
        /// Get recent items as a different type.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        public IList<T2> GetRecentAs<T2>(int pageNumber, int pageSize)
        {
            IList<T2> items = _repository.FindRecentAs<T2>(pageNumber, pageSize);
            return items;
        }


        /// <summary>
        /// Increments the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public void Increment(Expression<Func<T, object>> member, int by, int id)
        {
            _repository.Increment(member, by, id);
        }


        /// <summary>
        /// Decrements the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        public void Decrement(Expression<Func<T, object>> member, int by, int id)
        {
            _repository.Decrement(member, by, id);
        }
        #endregion


        /// <summary>
        /// Performs the action.
        /// </summary>
        /// <param name="ctx">The actioncontext.</param>
        /// <param name="action">Delegate to call to perform the action on the model.</param>
        /// <param name="entityAction">Name of the action.</param>
        protected virtual BoolResult<T> PerformAction(IActionContext ctx, Action<IActionContext> action, EntityAction entityAction)
        {
            string entityName = ctx.Item == null ? " with id : " + ctx.Id : ctx.Item.GetType().FullName;                
            try
            {
                // Step 1: Authenticate and check security.                
                BoolResult<T> result = PerformAuthentication(ctx, entityAction);
                if (!result.Success) return new BoolResult<T>(default(T), false, result.Message, ctx.Errors);
                
                // Step 2: Massage Data before validation.
                if (ctx.Item != null)
                {
                    IEntity entity = ctx.Item as IEntity;
                    Entities.Extensions.EntityExtensions.AuditAll(entity);
                }

                // Step 3: Now persist the entity.
                action(ctx);

                // Build the message.
                string message = "Successfully " + entityAction.ToString() + entityName;
                if (!ctx.Errors.IsValid)
                    message = "Unable to " + entityAction.ToString() + typeof(T).Name;

                return new BoolResult<T>((T)ctx.Item, ctx.Errors.IsValid, message, ctx.Errors);
            }
            catch (Exception ex)
            {
                string error = "Unable to : " + entityAction.ToString() + " entity : " + entityName;
                ErrorManager.Handle(error, ex, ctx.Errors);
                return new BoolResult<T>(default(T), false, error, ctx.Errors);
            }
        }


        /// <summary>
        /// Performs the validation on the model.
        /// </summary>
        /// <param name="ctx">The action context.</param>
        /// <param name="entityAction">The type of entity action</param>
        /// <returns></returns>
        protected virtual BoolMessage PerformValidation(IActionContext ctx, EntityAction entityAction)
        {
            if (_settings == null)
                return BoolMessage.True;

            if (!_settings.EnableValidation)
                return BoolMessage.True;

            IEntity entity = ctx.Item as IEntity;            
            bool isValid = entity.Validate(ctx.Errors);
            if (!isValid)
            {
                if (ctx.CombineMessageErrors)
                {
                    string errorMessage = ctx.Errors.Message();
                    return new BoolMessage(false, errorMessage);
                }
                return BoolMessage.False;
            }
            return BoolMessage.True;
        }


        /// <summary>
        /// Performs authentication to determine if the current use is allowed to edit
        /// this entity.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual BoolResult<T> PerformAuthentication(IActionContext ctx, EntityAction action)
        {
            if (_settings == null)
                return BoolResult<T>.True;

            if (!_settings.EnableAuthentication)
                return BoolResult<T>.True;

            if (ctx.Item != null && ctx.Item is IEntity && action == EntityAction.Update || action == EntityAction.Delete)
            {
                // Validate.
                IEntity entity = ctx.Item as IEntity;
                if (!entity.IsOwnerOrAdmin())
                {
                    ctx.Errors.Add("Not authorized to delete.");
                    entity.Errors.Add("Not authorized to delete.");
                    return new BoolResult<T>(default(T), false, "User : " + Auth.UserName + " is not authorized to edit / delete this item.", ctx.Errors);
                }
            }

            if (string.IsNullOrEmpty(_settings.EditRoles))
                return BoolResult<T>.True;
            
            bool isAuthorized = Auth.IsUserInRoles(_settings.EditRoles);
            string message = isAuthorized ? string.Empty : "User : " + Auth.UserName 
                + " is not authorized to edit/delete this entity";

            BoolResult<T> result = new BoolResult<T>(default(T), isAuthorized, message, ctx.Errors);
            if (!result.Success)
                ctx.Errors.Add(result.Message);

            return result;
        }
    }
}
