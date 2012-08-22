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
    /// This enumeration matches up with the enumerations used in the stored procedure
    /// for searching up by location.
    /// </summary>
    public enum LocationLookUpType { 
        /// <summary>
        /// Locate all.
        /// </summary>
        All = 0, 


        /// <summary>
        /// City lookup.
        /// </summary>
        City = 1, 


        /// <summary>
        /// City/state lookup.
        /// </summary>
        CityState = 2, 


        /// <summary>
        /// City/country lookup.
        /// </summary>
        CityCountry = 3, 


        /// <summary>
        /// City/state/country lookup.
        /// </summary>
        CityStateCountry = 4, 


        /// <summary>
        /// State lookup.
        /// </summary>
        State = 5, 


        /// <summary>
        /// Country lookup.
        /// </summary>
        Country = 6, 


        /// <summary>
        /// Zip lookup.
        /// </summary>
        Zip = 7, 


        /// <summary>
        /// No lookup.
        /// </summary>
        None = 8 };


    /// <summary>
    /// Location constants.
    /// </summary>
    public class LocationConstants
    {
        /// <summary>
        /// Country string for United States.
        /// </summary>
        public const string CountryUsa = "USA";


        /// <summary>
        /// Country id for United States.
        /// </summary>
        public const int CountryId_USA = 230;


        /// <summary>
        /// Length of zip code.
        /// </summary>
        public const int ZipCodeLength = 5;

        //Constants for Country, State, Zip if class is online.

        /// <summary>
        /// Country id iz online.
        /// </summary>
        public const int CountryId_NA_Online = -1;


        /// <summary>
        /// Zip code is online.
        /// </summary>
        public const string ZipCode_NA_Online = "00000";


        /// <summary>
        /// State id is online.
        /// </summary>
        public const int StateId_NA_Online = -1;


        /// <summary>
        /// City id is online.
        /// </summary>
        public const int CityId_NA = -1;
    }

}
