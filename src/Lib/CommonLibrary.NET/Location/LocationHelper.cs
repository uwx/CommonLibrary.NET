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
using System.Linq;

using ComLib;
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// This class contains various helper methods
    /// for the LocationSupport namespace.
    /// </summary>
    public class LocationHelper
    {
        /// <summary>
        /// Checks that the state belongs to the country.
        /// </summary>
        /// <param name="stateLookUp"></param>
        /// <param name="countryLookUp"></param>
        /// <param name="stateId"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public static bool IsValidStateCountryRelation(StateLookUp stateLookUp, CountryLookUp countryLookUp, int stateId, int countryId)
        {
            // indicates online.
            bool isUSA = countryId == LocationConstants.CountryId_USA;

            // If country id is online, disregard check.
            if (countryId == LocationConstants.CountryId_NA_Online) return true;

            // Do not have to select a state for Non-USA countries.
            if (stateId == LocationConstants.StateId_NA_Online && !isUSA) return true;

            // Must select a state for USA.
            if (stateId == LocationConstants.StateId_NA_Online && isUSA) return false;

            State state = stateLookUp[stateId];
            Country country = countryLookUp[countryId];

            // Check combination
            if (state.CountryId == country.Id) { return true; }

            return false;
        }


        /// <summary>
        /// Applies the city id to the address if the city is listed in the system
        /// and has a matching state and country id compared to what was supplied.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cityLookup"></param>
        /// <param name="stateLookup"></param>
        /// <param name="countryLookup"></param>
        public static void ApplyCity(Address address, CityLookUp cityLookup, StateLookUp stateLookup, CountryLookUp countryLookup)
        {
            address.CityId = LocationConstants.CityId_NA;

            // online( country id = online ) or city is null.
            // So don't change anything.
            if (address.CountryId == LocationConstants.CountryId_NA_Online) { return; }

            // Get the city.
            City city = cityLookup.FindByCountry(address.City, address.CountryId);
                        
            // CASE 1: Unknown city.
            if (city == null) { return; }

            // CASE 2: Matching country and state id's with city.
            // Most like this is for U.S.A cities. Where user has to specify state id.            
            if (city.CountryId == address.CountryId && city.StateId == address.StateId)
            {
                address.CityId = city.Id;
                address.City = city.Name;
                return;
            }

            // CASE 3: NON-U.S. country.
            // State not specified but matched the country specified with the city.
            if (address.StateId == LocationConstants.StateId_NA_Online && city.CountryId == address.CountryId)
            {
                address.StateId = city.StateId;
                address.CityId = city.Id;
                address.City = city.Name;
            }
        }


        /// <summary>
        /// Applies the city id to the address if the city is listed in the system
        /// and has a matching state and country id compared to what was supplied.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="stateLookup"></param>
        /// <param name="errors">list of errors to populate if any validation fails.</param>
        public static bool ApplyState(Address address, StateLookUp stateLookup, IList<string> errors)
        {
            if (address.StateId == LocationConstants.StateId_NA_Online) return true;
            if (address.StateId > 0) return true;

            // Can't determine state by name if it's not supplied.
            if (string.IsNullOrEmpty(address.State)) return false;
            
            int initialErrorCount = errors.Count;

            // Check the state.
            bool isCountryIdEmpty = (address.CountryId <= 0);
            State state = isCountryIdEmpty ? stateLookup[address.State] : stateLookup.FindByCountry(address.State, address.CountryId);
            if( ValidationUtils.Validate(state == null, errors, "Invalid state supplied.") )
            {
                address.StateId = state.Id;
                address.StateAbbr = state.Abbreviation;
                address.State = state.Name;
            }
            
            return initialErrorCount == errors.Count;
        }


        /// <summary>
        /// Applies the city id to the address if the city is listed in the system
        /// and has a matching state and country id compared to what was supplied.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="countryLookup"></param>
        /// <param name="errors">list of errors to populate if any validation fails.</param>
        /// <param name="useAliasIfApplicable"></param>
        public static bool ApplyCountry(Address address, CountryLookUp countryLookup, IList<string> errors, bool useAliasIfApplicable = false)
        {
            bool isCountryIdEmpty = address.CountryId == LocationConstants.CountryId_NA_Online || address.CountryId == 0;

            if (!isCountryIdEmpty) return true;
            if (string.IsNullOrEmpty(address.Country)) return false;

            int initialErrorCount = errors.Count;
            
            // Check the country.
            Country country = countryLookup[address.Country];
            if (ValidationUtils.Validate(country == null, errors, "Invalid country specified : " + address.Country))
            {
                address.CountryId = country.RealId;
                if (country.IsAlias && !useAliasIfApplicable)
                {
                    country = countryLookup[country.RealId];
                    address.Country = country.Name;
                }
            }            

            return initialErrorCount == errors.Count;
        }


        /// <summary>
        /// Configures the city by applying the country and state id based on the country/state names.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static BoolMessage ApplyCountryState(City city)
        {
            // Check country.
            Country country = Location.Service.CountriesLookup[city.CountryName];
            if (country == null) return new BoolMessage(false, "Unknown country : " + city.CountryName);

            city.CountryId = country.RealId;
            
            // Check state.
            State state = Location.Service.StatesList.Where(s => s.CountryId == country.RealId && string.Compare(s.Name, city.StateName, true) == 0).FirstOrDefault();
            if (state == null) return new BoolMessage(false, "Unknown state : " + city.StateName);

            city.StateId = state.Id;

            return BoolMessage.True;
        }


        /// <summary>
        /// Configures the state by applying the country id based on the country name.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static BoolMessage ApplyCountry(State state)
        {
            // Check country
            Country country = Location.Service.CountriesLookup[state.CountryName];
            if (country == null) return new BoolMessage(false, "Unknown country with name : " + state.CountryName);

            // Create state.
            state.CountryId = country.RealId;

            return BoolMessage.True;
        }


        /// <summary>
        /// Configures the state by applying the country id based on the country name.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public static void ApplyCountry(Country country)
        {
            string nametocheck = string.IsNullOrEmpty(country.AliasRefName) ? country.Name : country.AliasRefName;

            // Check country
            Country countrySearched = Location.Service.CountriesLookup[nametocheck];
            if (countrySearched == null) return;

            // Create
            if (country.IsAlias)
            {
                country.AliasRefId = countrySearched.RealId;
            }
            else
                country.Id = countrySearched.Id;  
        }
    }
}
