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
using System.Linq;
using System.Text;

namespace ComLib.Entities
{
    /// <summary>
    /// This class provides helper methods for the Repository namespace.
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Gets the specified entity and loads the TRelation. Defaults the foreign key to typeof(TRelation).Name + Id.
        /// Defaults the Property name to typeof(TRelation).
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <typeparam name="TRelation">The type of the relation.</typeparam>
        /// <param name="repo">The repo.</param>
        /// <param name="id">The id.</param>
        /// <param name="foreignKey">The foreign key.</param>
        /// <returns>Specified entity.</returns>
        public static T Get<T, TRelation>(this IRepository<T> repo, int id, string foreignKey) where TRelation : class where T : class
        {
            T entity = repo.Get(id);

            if (entity == default(T)) 
                return entity;

            // e.g. "CategoryId = id");
            var relationRepo = EntityRegistration.GetRepository<TRelation>();
            TRelation relation = relationRepo.First("where " + foreignKey + " = " + id);

            if (relation == default(TRelation))
                return entity;

            // e.g. Category.
            string propName = typeof(TRelation).Name;
            typeof(T).GetProperty(propName).SetValue(entity, relation, null);
            return entity;
        }


        /// <summary>
        /// Gets the specified entity and loads the 1-to-Many TRelation. Defaults the foreign key to typeof(TRelation).Name + Id.
        /// Defaults the Property name to typeof(TRelation)"s".
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <typeparam name="TRelation">The type of the relation.</typeparam>
        /// <param name="repo">The repo.</param>
        /// <param name="id">The id.</param>
        /// <param name="foreignKey">The foreign key.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Specified entity.</returns>
        public static T Get<T, TRelation>(this IRepository<T> repo, int id, string foreignKey, int pageIndex, int pageSize) where T : class where TRelation : class
        {
            T entity = repo.Get(id);

            if (entity == default(T))
                return entity;

            // e.g. "CategoryId = id");
            var relationRepo = EntityRegistration.GetRepository<TRelation>();
            IList<TRelation> relations = relationRepo.Find("where " + foreignKey + " = " + id);

            if (relations == null)
                return entity;

            // e.g. Category.
            string propName = typeof(TRelation).Name + "s";
            typeof(T).GetProperty(propName).SetValue(entity, relations, null);
            return entity;
        }
    }
}
