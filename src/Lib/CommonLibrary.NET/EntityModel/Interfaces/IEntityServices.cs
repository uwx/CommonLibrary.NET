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
using System.Data;
using System.Linq.Expressions;

using ComLib;
using ComLib.Data;
using ComLib.Locale;
using ComLib.Configuration;


namespace ComLib.Entities
{

    /// <summary>
    /// Reuse the existing interface for the IValidator, but modify it 
    /// slightly to incorporate the entityaction being performed.
    /// This allows for a singleton validator using the it's non-stateful
    /// method.
    /// </summary>
    public interface IEntityValidator : IValidator
    {
        /// <summary>
        /// Validates <paramref name="target"/> for the <paramref name="action"/> specified
        /// and adds <see cref="IValidationResults"/> entires representing
        /// failures to the supplied <paramref name="results"/>.
        /// </summary>
        /// <param name="target">The object to validate.</param>
        /// <param name="results"></param>
        /// <param name="action"></param>
        bool Validate(object target, IValidationResults results, EntityAction action);
    }


    /// <summary>
    /// Interface for an entity data massager.
    /// </summary>
    public interface IEntityMassager
    {
        /// <summary>
        /// Massage
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        void Massage(object entity, EntityAction action);
    }




    /// <summary>
    /// Non-Typed base interface for an EntityService
    /// </summary>
    public interface IEntityService
    {
        /// <summary> 
        /// The validator for the model. 
        /// </summary> 
        IEntityValidator Validator { get; set; }


        /// <summary>
        /// Configuration settings for the service.
        /// </summary>
        IEntitySettings Settings { get; set; }


        /// <summary>
        /// Localized resource manager.
        /// </summary>
        IEntityResources Resources { get; set; }


        /// <summary>
        /// Get the validator.
        /// </summary>
        /// <returns></returns>
        IEntityValidator GetValidator();


        /// <summary> 
        /// Creates the entity. 
        /// </summary> 
        /// <param name="ctx"></param> 
        /// <returns></returns> 
        void Create(IActionContext ctx);


        /// <summary>
        /// Updates the entity in the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        void Update(IActionContext ctx);


        /// <summary>
        /// Saves the entity in the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        void Save(IActionContext ctx);


        /// <summary>
        /// Deletes the entity in the repository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(int id);


        /// <summary>
        /// Delete all entities from the system.
        /// </summary>
        /// <returns></returns>
        void DeleteAll();


        /// <summary>
        /// Deletes the entity from the repository.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        void Delete(IActionContext ctx);


        /// <summary>
        /// Get recent items as a different type.
        /// </summary>
        /// <param name="pageNumber">1 The page number to get.</param>
        /// <param name="pageSize">15 Number of records per page.</param>
        /// <returns></returns>
        IList<T2> GetRecentAs<T2>(int pageNumber, int pageSize);
    }



    /// <summary>
    /// The model service handles all actions on the model.
    /// This includes CRUD operations which are delegated to the 
    /// ModelRespository after a ModelService performs any appropriate
    /// security checks among other things.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityService<T> : IEntityService
    {
        /// <summary> 
        /// The resposity for the model. 
        /// </summary> 
        IRepository<T> Repository { get; set; }


        /// <summary>
        /// Get the settings as a typed object.
        /// </summary>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        TSettings GetSettings<TSettings>() where TSettings : class;


        /// <summary>
        /// This method is useful for derived classes to implement 
        /// their own initialization behaviour.
        /// </summary>
        void Init();


        /// <summary> 
        /// Creates the entity. 
        /// </summary> 
        /// <param name="entity"></param>
        /// <returns></returns> 
        void Create(T entity);


        /// <summary>
        /// Create multiple
        /// </summary>
        /// <param name="entities"></param>
        void Create(IList<T> entities);


        /// <summary>
        /// Creates the entities conditionally based on whether they exists in the datastore.
        /// Existance in the datastore is done by finding any entities w/ matching values for the 
        /// <paramref name="checkFields"/> supplied.
        /// </summary>
        void Create(IList<T> entities, params Expression<Func<T, object>>[] checkFields);

        
        /// <summary>
        /// Updates the entity in the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(T entity);
        
        
        /// <summary>
        /// Updates multiple entities.
        /// </summary>
        /// <param name="entities"></param>
        void Update(IList<T> entities);
        
        
        /// <summary>
        /// Saves the entity in the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Save(T entity);
        

        /// <summary>
        /// Deletes the entity in the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Delete(T entity);


        /// <summary>
        /// Gets the entity in the repository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);
        
        
        /// <summary> 
        /// Retrieves the entity from the repository.
        /// You can specify either the id or object.
        /// </summary> 
        /// <param name="ctx"></param>
        /// <returns></returns>
        T Get(IActionContext ctx);


        /// <summary>
        /// Retrieves all the instances of the model from the repository
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();


        /// <summary>
        /// Retrieves all the instances of the model from the repository
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll(IActionContext ctx);


        /// <summary>
        /// Retrieve all items as a non-generic list.
        /// This is to allow retrieving all items across different types using reflection.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        IList GetAllItems(IActionContext ctx);


        /// <summary>
        /// Get items by page.
        /// </summary>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15</param>
        /// <returns></returns>
        PagedList<T> Get(int pageNumber, int pageSize);


        /// <summary>
        /// Get items by page using filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15</param>
        /// <returns></returns>
        PagedList<T> Find(string filter, int pageNumber, int pageSize);


        /// <summary>
        /// Get items by page using IQuery.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15</param>
        /// <returns></returns>
        PagedList<T> Find(IQuery criteria, int pageNumber, int pageSize);


        /// <summary>
        /// Get all items at once(without paging records) using filter.
        /// </summary>
        /// <returns></returns>
        IList<T> Find(string filter);


        /// <summary>
        /// Finds entities using the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        IList<T> Find(IQuery criteria);


        /// <summary>
        /// Get the first one that matches the filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T First(string filter);


        /// <summary>
        /// Get items by page and by the user who created them.
        /// </summary>
        /// <param name="userName">Name of user who created the entities.</param>
        /// <param name="pageNumber">Page number to get. e.g. 1</param>
        /// <param name="pageSize">Number of records per page. e.g. 15</param>
        /// <returns></returns>
        PagedList<T> FindByUser(string userName, int pageNumber, int pageSize);


        /// <summary>
        /// Get recent items by page.
        /// </summary>
        /// <param name="pageNumber">1</param>
        /// <param name="pageSize">15</param>
        /// <returns></returns>
        PagedList<T> GetRecent(int pageNumber, int pageSize);


        /// <summary>
        /// Increments the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        void Increment(Expression<Func<T, object>> member, int by, int id);


        /// <summary>
        /// Decrements the specified member.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="by">The by.</param>
        /// <param name="id">The id.</param>
        void Decrement(Expression<Func<T, object>> member, int by, int id);
    }



    /// <summary>
    /// Interface for the models service settings that ares used to 
    /// configuration settings for the model service.
    /// </summary>
    public interface IEntitySettings : IConfigSourceBase
    {
        /// <summary>
        /// Whether authentication is required to edit the entity.
        /// </summary>
        bool EnableAuthentication { get; set; }


        /// <summary>
        /// Whether or not to validate the entity.
        /// </summary>
        bool EnableValidation { get; set; }


        /// <summary>
        /// Roles allowed to edit the entity.
        /// </summary>
        string EditRoles { get; set; }
    }



    /// <summary>
    /// Localization resource provider for entity.
    /// </summary>
    public interface IEntityResources : ILocalizationResourceProvider
    {
        /// <summary>
        /// Get the name of the entity.
        /// </summary>
        string EntityName { get; }
    }



    /// <summary>
    /// Row mapper for a model to map database rows to model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityRowMapper<T> : IRowMapper<IDataReader, T>
    {
    }    
}
