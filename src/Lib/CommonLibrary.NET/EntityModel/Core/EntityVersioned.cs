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
    /// Persistant entity that can be versioned.
    /// NOTE: If you want to have a Entity that uses generics
    /// and therefore provides the Create(), Update(), Save(), Delete() methods
    /// on it's own then, it's better to implemented the IEntityVersioned interface in your
    /// class using partial classes. Also the IEntityVersioned only has 3 methods so it's 
    /// pretty lightweight.
    /// </summary>
    public abstract class EntityVersioned : Entity, IEntityVersioned
    {
        /// <summary>
        /// The current version of the entity.
        /// </summary>
        public int Version { get; set; }


        /// <summary>
        /// Reference to latest/active entity if this is an older version of the entity.
        /// </summary>
        public int VersionRefId { get; set; }


        /// <summary>
        /// Whether or not this is the latest version.
        /// </summary>
        /// <returns></returns>
        public bool IsLatestVersion()
        {
            return VersionRefId == -1;
        }
    }
}
