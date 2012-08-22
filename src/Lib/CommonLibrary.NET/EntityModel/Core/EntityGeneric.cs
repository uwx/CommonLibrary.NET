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
using System.Data;
using System.Collections.Generic;

using ComLib;
using ComLib.Entities.Management;


namespace ComLib.Entities
{

    /// <summary>
    /// Generic based Entity class with supporting parameterless CRUD methods. 
    /// Create,update,save,delete.
    /// </summary>
    /// <typeparam name="T">The type of the Entity.</typeparam>
    public class Entity<T> : Entity where T : class, IEntity
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public virtual void Create()
        {
            var service = EntityRegistration.GetService<T>();
            service.Create(this as T);
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll()
        {            
            var service = EntityRegistration.GetService<T>();
            return service.GetAll();
        }


        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <returns></returns>
        public virtual void Update()
        {
            var service = EntityRegistration.GetService<T>();
            service.Update(this as T);
        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public virtual void Save()
        {
            var service = EntityRegistration.GetService<T>();
            service.Save(this as T);
        }


        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns></returns>
        public virtual void Delete()
        {
            var service = EntityRegistration.GetService<T>();
            service.Delete(this as T);
        }


        /// <summary>
        /// Deletes all.
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteAll()
        {
            var service = EntityRegistration.GetService<T>();
            service.DeleteAll();
        }
    }
}