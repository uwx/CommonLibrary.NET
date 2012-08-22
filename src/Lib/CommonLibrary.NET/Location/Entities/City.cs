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
using System.Text;



namespace ComLib.LocationSupport
{
    
    /// <summary>
    /// City
    /// </summary>
    public class City : LocationCountryBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public City()
        {
        }


        /// <summary>
        /// City constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abbr"></param>
        /// <param name="stateId"></param>
        /// <param name="countryId"></param>
        public City(string name, string abbr, int stateId, int countryId)
        {
            Name = name;
            Abbreviation = abbr;
            StateId = stateId;
            CountryId = countryId;
            ParentId = LocationConstants.CityId_NA;
            IsPopular = false;
        }


        /// <summary>
        /// State id
        /// </summary>
        public virtual int StateId { get; set; }


        /// <summary>
        /// Parent id can be used to associate an area with it's city/county.
        /// e.g. Bronx = city, parent id = NYC
        /// </summary>
        public virtual int ParentId { get; set; }


        /// <summary>
        /// Is major / popular city.
        /// </summary>
        public virtual bool IsPopular { get; set; }


        /// <summary>
        /// Gets or sets the name of the state.
        /// </summary>
        /// <value>The name of the country.</value>
        public virtual string StateName { get; set; }   
    }
}
