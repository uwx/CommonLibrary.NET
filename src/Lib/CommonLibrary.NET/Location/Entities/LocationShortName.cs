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


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Class to store short name data.
    /// e.g.
    /// 
    /// Maps just a city to it's full-qualified city/state name.
    /// 
    /// "bronx" maps to "bronx,ny"
    /// "queens" maps to "queens,ny"
    /// "Boston" maps to "Boston,MA"
    /// </summary>
    public class LocationShortName : LocationDataBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationShortName"/> class.
        /// </summary>
        public LocationShortName() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="LocationShortName"/> class.
        /// </summary>
        /// <param name="shortName">The short name.</param>
        /// <param name="longName">The long name.</param>
        public LocationShortName(string shortName, string longName)
        {
            this.Abbreviation = shortName;
            this.Name = longName;
        }
    }    



    /// <summary>
    /// Wrapper class around list of shortnames.
    /// This serves as read-only way of getting shortname data.
    /// </summary>
    public class ShortNameLookUp : LocationLookUp<LocationShortName>
    {
        /// <summary>
        /// Constructor to build up the lookup of shortnames.
        /// </summary>
        /// <param name="shortNames"></param>
        public ShortNameLookUp(IList<LocationShortName> shortNames ) : base(shortNames)
        {
        }
    }   
}
