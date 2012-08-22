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
    /// Parser for location data.
    /// </summary>
    public static class LocationParser
    {
        /// <summary>
        /// Parses the city
        /// </summary>
        /// <param name="cityLookUp">The city look up component</param>
        /// <param name="stateLookUp">The state look up component</param>
        /// <param name="countryLookUp">The country look up component</param>        
        /// <param name="cityname">The city name.</param>
        /// <returns></returns>
        public static LocationLookUpResult ParseCity(CityLookUp cityLookUp, StateLookUp stateLookUp, CountryLookUp countryLookUp, string cityname)
        {
            City city = cityLookUp[cityname];
            LocationLookUpResult result = null;

            // Empty city ?
            if (city == null)
                return new LocationLookUpResult(LocationLookUpType.City, false, "Unknown city supplied") { City = cityname };
            

            // Set the city, state, country information.
            result = ParseStateById(LocationLookUpType.City, true, string.Empty, stateLookUp, countryLookUp, city.StateId, city.CountryId);
            result.City = city.Name;
            result.CityId = city.Id;
            return result;
        }


        /// <summary>
        /// Parses the state.
        /// </summary>
        /// <param name="stateLookUp">The state look up component</param>
        /// <param name="countryLookUp">The country look up component</param>        
        /// <param name="stateFullNameOrAbbr"></param>
        /// <returns></returns>
        public static LocationLookUpResult ParseState(StateLookUp stateLookUp, CountryLookUp countryLookUp, string stateFullNameOrAbbr)
        {
            State state = stateLookUp[stateFullNameOrAbbr];
            LocationLookUpResult result = null;

            // Empty state return state.
            if (state == null)            
                return new LocationLookUpResult(LocationLookUpType.State, false, "Unknown state supplied") { State = stateFullNameOrAbbr };            

            // Build up the location data.
            result = ParseStateById(LocationLookUpType.State, true, string.Empty, stateLookUp, countryLookUp, state.Id, state.CountryId);
            return result;
        }


        /// <summary>
        /// Parse the country.
        /// </summary>
        /// <param name="countryLookUp">The country lookup component</param>
        /// <param name="countryText">The text representing a country</param>
        /// <returns></returns>
        public static LocationLookUpResult ParseCountry(CountryLookUp countryLookUp, string countryText)
        {
            Country country = countryLookUp[countryText];
            LocationLookUpResult result = null;

            if (country == null)
                return new LocationLookUpResult(LocationLookUpType.Country, false, "Unknown country supplied.") { Country = countryText };            

            // Set the city, state, country information.
            result = new LocationLookUpResult(LocationLookUpType.Country, true, string.Empty);
            result.CountryId = country.Id;
            result.Country = country.Name;
            return result;
        }        


        /// <summary>
        /// Parse 'city','state' | 'country'.
        /// e.g. Georgetown, Texas or GeorgeTown, Guyana
        /// </summary>
        /// <remarks>The area after the comma can be either the state or country.
        /// We store a list of valid states/regions, and countries</remarks>
        /// <param name="cityLookUp">The city look up component</param>
        /// <param name="stateLookUp">The state look up component</param>
        /// <param name="countryLookUp">The country look up component</param>
        /// <param name="smallLarge"></param>
        /// <returns></returns>
        public static LocationLookUpResult ParseCityWithStateOrCountry(CityLookUp cityLookUp, StateLookUp stateLookUp, CountryLookUp countryLookUp, string smallLarge)
        {
            // Validate.
            if (string.IsNullOrEmpty(smallLarge)) { return LocationLookUpResult.Empty; }

            string[] tokens = smallLarge.Split(',');

            // Validate. Only 2 tokens. city, state or city, country.
            if ( tokens.Length != 2)
            {
                return new LocationLookUpResult(LocationLookUpType.CityState, false, "Invalid city,state/country.");
            }

            // Get each token ( before comma, and after )
            string smallArea = tokens[0];
            string largeArea = tokens[1];
            smallLarge = smallLarge.Trim();
            smallArea = smallArea.Trim();
            largeArea = largeArea.Trim();

            // Small area has to be city.
            // Large area can be either state, country.
            City city = cityLookUp[smallArea];
            bool knownCity = city != null;
            State state = null;
            Country country = null;

            // Valid city.
            if ( knownCity )
            {
                // Check the user supplied state or country.
                // Return result if the city matches with the state or country.
                LocationLookUpResult result = new LocationLookUpResult(LocationLookUpType.City, false, string.Empty);
                if ( IsCityMatchedWithStateCountry(largeArea, city, stateLookUp, countryLookUp, ref result) )
                {
                    return result;
                }
            }

            // Check state.
            state = stateLookUp[largeArea];
            
            // Unknown city entered, but valid State.
            if ( state != null )
            {
                country = countryLookUp[state.CountryId];
                LocationLookUpResult result = Build(LocationLookUpType.CityState, true, string.Empty, country, state, null);
                result.City = smallArea;
                return result;
            }

            // Check country.
            country = countryLookUp[largeArea];

            // Unknown city entered, but Valid country
            if ( country != null )
            {
                LocationLookUpResult result = Build(LocationLookUpType.CityCountry, true, string.Empty, country, null, null);
                result.City = smallArea;
                return result;
            }
            
            return new LocationLookUpResult(LocationLookUpType.CityState, false, "Unknown city/state or city/country : " 
                + smallArea + ", " + largeArea);          
        }        


        #region Private methods
        /// <summary>
        /// Given a user entered text contain a city and (state or country) combination,
        /// this method checks that the state or country entered by the user matches 
        /// the state or country of the city that was matched.
        /// </summary>
        /// <param name="stateOrCountryTrimmed">The state or country entered by the user.</param>
        /// <param name="city">The city that was found in our system, entered by the user.</param>
        /// <param name="stateLookUp">The state lookup component.</param>
        /// <param name="countryLookUp">The country lookup component.</param>
        /// <param name="lookUpResult"></param>
        /// <returns></returns>
        private static bool IsCityMatchedWithStateCountry(string stateOrCountryTrimmed,
            City city, StateLookUp stateLookUp, CountryLookUp countryLookUp, ref LocationLookUpResult lookUpResult)
        {
            // Require that the city is not null.
            if (city == null) { return false; }

            State state = stateLookUp[stateOrCountryTrimmed];
            Country country = countryLookUp[stateOrCountryTrimmed];

            // Matched state of city and state input.
            // This now turns into a simple city based search.
            if ( state != null && city.StateId == state.Id)
            {
                country = countryLookUp[state.CountryId];
                Build(lookUpResult, LocationLookUpType.City, true, string.Empty, country, state, city);
                return true;
            }

            // Matched country of city and country input
            // This now turns into a simple city based search.
            if ( country != null && city.CountryId == country.Id )
            {
                state = stateLookUp[city.StateId];
                Build(lookUpResult, LocationLookUpType.City, true, string.Empty, country, state, city);
                return true;
            }

            return false;            
        }


        /// <summary>
        /// Builds up the LocationLookupResult based on the location components passed in.
        /// City, state, country can be null.
        /// </summary>
        /// <param name="lookupType">The location lookup type</param>
        /// <param name="isValid">Whether or not the location was valid.</param>
        /// <param name="message">Error message if not valid.</param>
        /// <param name="country">Optional - The country to be used for setting the id and name of the country.</param>
        /// <param name="state">Optional - The state to be used for setting the id and name of the state.</param>
        /// <param name="city">Optional - The city to be used for setting the id and name of the city</param>
        /// <returns></returns>
        private static LocationLookUpResult Build(LocationLookUpType lookupType, bool isValid, string message, Country country, State state, City city)
        {
            LocationLookUpResult result = new LocationLookUpResult(lookupType, isValid, message);
            Build(country, state, city, result);
            return result;
        }


        private static void Build(LocationLookUpResult result, LocationLookUpType lookupType, bool isValid, string message, Country country, State state, City city)
        {
            result.Init(lookupType, isValid, message);
            Build(country, state, city, result);
        }


        private static void Build(Country country, State state, City city, LocationLookUpResult result)
        {
            if (city != null)
            {
                result.City = city.Name;
                result.CityId = city.Id;
            }
            if (state != null)
            {
                result.State = state.Name;
                result.StateId = state.Id;
                result.StateAbbr = state.Abbreviation;
            }
            if (country != null)
            {
                result.CountryId = country.Id;
                result.Country = country.Name;
            }
        }


        private static LocationLookUpResult ParseStateById(LocationLookUpType lookupType, bool isValid, string message,
            StateLookUp stateLookUp, CountryLookUp countryLookUp, int stateId, int countryId)
        {
            State state = stateLookUp[stateId];

            Country country = null;
            if (state != null)
            {
                country = countryLookUp[state.CountryId];
            }
            else
            {
                country = countryLookUp[countryId];
            }

            return Build(lookupType, isValid, message, country, state, null);
        }
        #endregion
    }   
}
