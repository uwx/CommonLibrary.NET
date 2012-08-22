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

using ComLib.Entities;


namespace ComLib.LocationSupport
{
       
    /// <summary>
    /// Location data base class for state, country, location short name.
    /// </summary>
    public class LocationBase : Entity
    {       

        /// <summary>
        /// Full / Formal name.
        /// </summary>
        public virtual string Name { get; set; }


        /// <summary>
        /// Short name or abbreviation
        /// </summary>
        public virtual string Abbreviation { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is alias.
        /// </summary>
        /// <value><c>true</c> if this instance is alias; otherwise, <c>false</c>.</value>
        public virtual bool IsAlias { get; set; }


        /// <summary>
        /// Gets or sets the alias ref id.
        /// </summary>
        /// <value>The alias ref id.</value>
        public virtual int AliasRefId { get; set; }


        /// <summary>
        /// Gets or sets the alias ref name.
        /// </summary>
        /// <remarks>This is used more when importing location data from files.</remarks>
        /// <value>The alias ref id.</value>
        public virtual string AliasRefName { get; set; }


        /// <summary>
        /// Get the real id, if this is a country.
        /// </summary>
        public virtual int RealId
        {
            get { return IsAlias ? AliasRefId : Id; }
        }


        /// <summary>
        /// Indicates whether or not this entity is active
        /// </summary>
        public virtual bool IsActive { get; set; }
    }



    /// <summary>
    /// Location data with the country id.
    /// </summary>
    public class LocationCountryBase : LocationBase
    {
        /// <summary>
        /// Country Id
        /// </summary>
        public int CountryId { get; set; }


        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>The name of the country.</value>
        public virtual string CountryName { get; set; }      
    }

}
