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


namespace ComLib.Entities
{

    /// <summary>
    /// Base class for entities than can be persisted.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class EntityPersistant<TId> : IEntityPersistant<TId>
    {
        #region IEntityPersistant<TId> Members

        /// <summary>
        /// Get the id of a persistant entity.
        /// </summary>
        /// <value></value>
        public virtual TId Id { get; set; }


        /// <summary>
        /// Determines whether this instance is persistant.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is persistant; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsPersistant()
        {
            return (Id != null && !Id.Equals(default(TId)));
        }

        #endregion
    }
}
