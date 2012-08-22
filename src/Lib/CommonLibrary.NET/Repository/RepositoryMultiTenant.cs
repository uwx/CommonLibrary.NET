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
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Specialized;

using ComLib;
using ComLib.Reflection;
using ComLib.Entities;
using ComLib.Data;


namespace ComLib.Entities
{
    /// <summary>
    /// Multi-tenant repository.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class RepositoryMultiTenant<T> : RepositoryInMemory<T> where T : class, IEntity, IMultiTenant
    {
        private IRepository<T> _repo;
        private static Func<int> _tenantFetcher = () => GetTenantId();
        

        /// <summary>
        /// Multi-tenant repository.
        /// </summary>
        /// <param name="repo">Repository to initialize from.</param>
        public RepositoryMultiTenant(IRepository<T> repo)
        {
            _repo = repo;
        }


        /// <summary>
        /// Create the entity.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <returns>Created entity.</returns>
        public override T Create(T entity)
        {
            entity.TenantId = _tenantFetcher();
            return base.Create(entity);
        }

        
        /// <summary>
        /// Update the entity.
        /// </summary>
        /// <param name="entity">Entity to create/update.</param>
        /// <returns>Created/updated entity.</returns>
        public override T Update(T entity)
        {
            entity.TenantId = _tenantFetcher();
            return base.Update(entity);
        }


        /// <summary>
        /// Delete all the items for the specified tenant.
        /// </summary>
        public override void DeleteAll()
        {
            int tenantId = _tenantFetcher();
            var query = Query<T>.New().Where(t => t.TenantId).Is(tenantId);
            Delete(query);
        }


        /// <summary>
        /// Retrieves all entities stored by this instance.
        /// </summary>
        /// <returns>List of all entities.</returns>
        public override IList<T> GetAll()
        {
            int tenantId = _tenantFetcher();
            var query = Query<T>.New().Where(t => t.TenantId).Is(tenantId);
            return base.Find(query);
        }


        /// <summary>
        /// Returns the tenant id.
        /// </summary>
        /// <returns></returns>
        public static int GetTenantId()
        {
            return 0;
        }
    }
}
