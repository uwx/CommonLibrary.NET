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
    /// Result when trying to search by a location.
    /// This is actually the result provided back from location service
    /// when parsing some location.
    /// 
    /// e.g.
    /// 
    /// zipcode    : "10465"
    /// state      : "ny"
    /// city/state : "bronx, ny"
    /// </summary>
    public class LocationLookUpResult 
    {
        private static LocationLookUpResult _empty;
        
        private LocationLookUpType _lookupType;

        /// <summary>
        /// True if the location is valid/
        /// </summary>
        protected bool _isValid;


        /// <summary>
        /// Validation error.
        /// </summary>
        protected string _error;
        
        
        /// <summary>
        /// Static constructor to build the null object "Empty"
        /// </summary>
        static LocationLookUpResult()
        {
            _empty = new LocationLookUpResult(LocationLookUpType.All, true, string.Empty);
        }


        /// <summary>
        /// Null object. 
        /// </summary>
        public static LocationLookUpResult Empty
        {
            get { return _empty; }
            set { _empty = value; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lookupType"></param>
        /// <param name="isValid"></param>
        /// <param name="error"></param>
        public LocationLookUpResult(LocationLookUpType lookupType, bool isValid, string error)            
        {
            _lookupType = lookupType;
            _isValid = isValid;
            _error = error;
        }


        internal void Init(LocationLookUpType lookupType, bool isValid, string error)
        {
            _lookupType = lookupType;
            _isValid = isValid;
            _error = error;
        }

        
        /// <summary>
        /// Indicates if looking up by zip
        /// </summary>
        public bool IsLookUpByZip
        {
            get { return _lookupType == LocationLookUpType.Zip; }
        }


        /// <summary>
        /// Indicates if looking up by state name
        /// </summary>
        public bool IsLookUpByState
        {
            get { return _lookupType == LocationLookUpType.State; }
        }


        /// <summary>
        /// Searching by country
        /// </summary>
        public bool IsLookUpByCountry
        {
            get { return _lookupType == LocationLookUpType.Country; }
        }


        /// <summary>
        /// Looking up by locatio not applicable.
        /// </summary>
        public bool IsLookUpByNotApplicable
        {
            get { return _lookupType == LocationLookUpType.None; }
        }


        /// <summary>
        /// Indicates if looking up by city.
        /// </summary>
        public bool IsLookUpByCity
        {
            get { return _lookupType == LocationLookUpType.City; }
        }

        /// <summary>
        /// Indicates if looking up by city/state combination.
        /// </summary>
        public bool IsLookUpByCityState
        {
            get { return _lookupType == LocationLookUpType.CityState; }
        }


        /// <summary>
        /// Indicates if looking up by City country combination.
        /// </summary>
        public bool IsLookUpByCityCountry
        {
            get { return _lookupType == LocationLookUpType.CityCountry; }
        }


        /// <summary>
        /// Get the lookup type.
        /// </summary>
        public LocationLookUpType LookUpType
        {
            get { return _lookupType; }
        }


        /// <summary>
        /// Valid zip code?
        /// </summary>
        public bool IsValid
        {
            get { return _isValid; }
        }


        /// <summary>
        /// Error if invalid.
        /// </summary>
        public string Error
        {
            get { return _error; }
        }


        /// <summary>
        /// City name
        /// </summary>
        public string City;

        /// <summary>
        /// City Id.
        /// </summary>
        public int CityId;

        /// <summary>
        /// State name
        /// </summary>
        public string State;
        
        /// <summary>
        /// State abbreviation
        /// </summary>
        public string StateAbbr;

        /// <summary>
        /// State id
        /// </summary>
        public int StateId;

        /// <summary>
        /// Country name
        /// </summary>
        public string Country;

        /// <summary>
        /// Country id
        /// </summary>
        public int CountryId;

        /// <summary>
        /// Zip code
        /// </summary>
        public string Zip;
    }    
}
