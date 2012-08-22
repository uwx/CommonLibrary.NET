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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Data;

using ComLib;
using ComLib.Data;


namespace ComLib.Entities
{    
    /// <summary>
    /// Wrapper around Repository to version the entities.
    /// </summary>
    /// <typeparam name="T">Type of repository items.</typeparam>
    public interface IRepositoryVersionable<T> : IRepository<T>
    {
        /// <summary>
        /// To enable / disable versioning.
        /// </summary>
        bool IsVersioned { get; set; }


        /// <summary>
        /// Get whether strict versioning is enabled.
        /// </summary>
        bool IsVersioningStrict { get; }


        /// <summary>
        /// Roll back the version of the entity with the specified id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        void RollBack(int id);


        /// <summary>
        /// Rollback the specified entity.
        /// </summary>
        /// <param name="entity">Entity to rollback.</param>
        void RollBack(T entity);


        /// <summary>
        /// Get all the versions ( latest / historic ) for the specified entity with id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <returns>List with all entity versions.</returns>
        IList<T> GetAllVersions(int id);


        /// <summary>
        /// Find entities using the criteria with option to get only the latest versions and
        /// not all ( latest/historic ).
        /// </summary>
        /// <param name="criteria">Criteria to match.</param>
        /// <param name="onlyLatestVersions">True to get only the latest versions.</param>
        /// <returns>List of matched items.</returns>
        IList<T> Find(IQuery criteria, bool onlyLatestVersions);
    }    
}