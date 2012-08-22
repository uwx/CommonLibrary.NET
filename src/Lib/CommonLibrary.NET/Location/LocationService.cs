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
using System.Collections;
using System.Collections.ObjectModel;
using System.Resources;

using ComLib;
using ComLib.Caching;
using ComLib.Logging;
using ComLib.Locale;
using ComLib.Entities;


namespace ComLib.LocationSupport
{
    
    /// <summary>
    /// LocationService parses text representing a location which can be various formats
    /// such as city, city/state, city/country, state, country.
    /// </summary>    
    public class LocationService : ILocationService
    {
        private LocationSettings _settings;


        /// <summary>
        /// Constuctor that also takes in the short name dao.
        /// </summary>
        /// <param name="countryRepoGetter"></param>
        /// <param name="stateRepoGetter"></param>
        /// <param name="cityRepoGetter"></param>
        public LocationService(Func<IRepository<Country>> countryRepoGetter, Func<IRepository<State>> stateRepoGetter, Func<IRepository<City>> cityRepoGetter)
        {
            Init(countryRepoGetter(), stateRepoGetter(), cityRepoGetter());
        }


        /// <summary>
        /// Constuctor that also takes in the short name dao.
        /// </summary>
        /// <param name="countryRepo"></param>
        /// <param name="stateRepo"></param>
        /// <param name="cityRepo"></param>
        public LocationService(IRepository<Country> countryRepo, IRepository<State> stateRepo, IRepository<City> cityRepo)
        {
            Init(countryRepo, stateRepo, cityRepo);
        }


        #region Repositories and Settings
        /// <summary>
        /// Repository for the cities.
        /// </summary>
        public IRepository<City> Cities { get; set; }


        /// <summary>
        /// Repository for the states.
        /// </summary>
        public IRepository<State> States { get; set; }


        /// <summary>
        /// Repository for the countries
        /// </summary>
        public IRepository<Country> Countries { get; set; }


        /// <summary>
        /// Settings for this Location service
        /// </summary>
        public LocationSettings Settings { get; set; }
        #endregion
        

        #region Lists & Lookups
        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public CityLookUp CitiesLookup
        {
            get { return GetCachedLookup<CityLookUp>("cities_lookup", _settings.CacheCityLookup, _settings.CacheTimeForCitiesInSeconds, () => new CityLookUp(Cities.GetAll()), (lookup) => lookup == null || lookup.Lookup1.Count == 0); }
        }


        /// <summary>
        /// Get the city list.
        /// </summary>
        public IList<City> CitiesList
        {
            get { return GetCached<City>("cities_list", _settings.CacheCityList, _settings.CacheTimeForCitiesInSeconds, () => Cities.GetAll()); }
        }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public CountryLookUp CountriesLookup
        {
            get { return GetCachedLookup<CountryLookUp>("countries_lookup", _settings.CacheCountryLookup, _settings.CacheTimeForCountriesInSeconds, () => new CountryLookUp(Countries.GetAll()), (lookup) => lookup == null || lookup.Lookup1.Count == 0 ); }
        }


        /// <summary>
        /// Get the country lookup
        /// </summary>        
        public IList<Country> CountriesList
        {
            get { return GetCached<Country>("countries_list", _settings.CacheCountryList, _settings.CacheTimeForCountriesInSeconds, () => Countries.GetAll());  }
        }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public StateLookUp StatesLookup
        {
            get { return GetCachedLookup<StateLookUp>("states_lookup", _settings.CacheStateLookup, _settings.CacheTimeForStatesInSeconds, () => new StateLookUp(States.GetAll()), (lookup) => lookup == null || lookup.Lookup1.Count == 0); }
        }


        /// <summary>
        /// Get the states list.
        /// </summary>
        public IList<State> StatesList
        {
            get { return GetCached<State>("states_list", _settings.CacheStateList, _settings.CacheTimeForStatesInSeconds, () => States.GetAll()); }
        }
        #endregion  


        #region ILocationService Members
        /// <summary>
        /// Does a high-level check of the format supplied and determines what type
        /// of location input was supplied.
        /// 
        /// Formats are:
        /// 1. city                         - "Bronx"
        /// 2. city,state                   - "Bronx , New York"
        /// 3. city,state( abbreviation )   - "Bronx , NY"
        /// 4. city,country                 - "HomeTown , USA"
        /// 5. state                        - "New Jersey"
        /// 6. state abbreviation           - "NJ"
        /// 7. country                      - "Italy"
        /// the actuall parsing 
        /// </summary>
        /// <param name="location">Text representing the location.</param>
        /// <returns></returns>
        public LocationLookUpResult Parse(string location)
        {
            // Validate.
            if (IsEmptyLocation(location))
            {
                return new LocationLookUpResult(LocationLookUpType.None, false, "Location was not supplied.");
            }

            // Trim any spaces.
            location = location.Trim();
            return InternalParse(location);
        }


        /// <summary>
        /// Create a city in the underlying datastore using the fields supplied
        /// </summary>
        /// <param name="name">Name of the city</param>
        /// <param name="stateName">Name of the state the city is in</param>
        /// <param name="countryName">Name of the country the city is in</param>
        /// <returns></returns>
        public BoolMessageItem<City> CreateCity(string name, string stateName, string countryName)
        {
            City city = new City(name, name, 0, 0) { StateName = stateName, CountryName = countryName };            
            LocationHelper.ApplyCountryState(city);
            Cities.Create(city);
            return new BoolMessageItem<City>(city, city.Id > 0, "");
        }


        /// <summary>
        /// Create all the countries.
        /// </summary>
        /// <param name="countries"></param>
        public void CreateCountries(IList<Country> countries)
        {
            foreach (var country in countries)
            {
                var result = CreateCountry(country);
                if (!result.Success)
                    Logger.Error("Unable to create country : " + result.Message);
            }
        }


        /// <summary>
        /// Create a country with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BoolMessageItem<Country> CreateCountry(string name)
        {
            return CreateCountry(name, name, false);
        }


        /// <summary>
        /// Create the country using the name, alias.
        /// </summary>
        /// <param name="name">Country name</param>
        /// <param name="alias">Alias for the country</param>
        /// <param name="isAlias">Whether or not creating an alias.</param>
        /// <returns></returns>
        public BoolMessageItem<Country> CreateCountry(string name, string alias, bool isAlias)
        {
            var country = new Country(name, name) { IsAlias = isAlias };
            if (isAlias)
            {
                country.Name = alias;
                country.AliasRefName = name;
            }
            return CreateCountry(country);
        }


        /// <summary>
        /// Create the country using the name, alias.
        /// </summary>
        /// <param name="country">Country to create</param>
        /// <returns></returns>
        public BoolMessageItem<Country> CreateCountry(Country country)
        {
            // Duplicate ??
            string name = country.IsAlias ? country.AliasRefName : country.Name;
            
            Country countrySearched = CountriesLookup[name];
            if (!country.IsAlias && countrySearched != null) return new BoolMessageItem<Country>(null, false, "Country with name : " + name + " already exists");
            if (country.IsAlias && countrySearched == null) return new BoolMessageItem<Country>(null, false, "Unknown country with name : " + name);

            // Create
            if (country.IsAlias)
            {
                country.AliasRefId = countrySearched.RealId;
            }

            Countries.Create(country);
            return new BoolMessageItem<Country>(country, country.Id > 0, "");
        }


        /// <summary>
        /// Create the state.
        /// </summary>
        /// <param name="state">The state to create</param>
        /// <returns></returns>
        public BoolMessageItem<State> CreateState(State state)
        {
            LocationHelper.ApplyCountry(state);
            States.Create(state);
            return new BoolMessageItem<State>(state, state.Id > 0, "");
        }


        /// <summary>
        /// Create the state.
        /// </summary>
        /// <param name="name">Name of the state.</param>
        /// <param name="abbreviation">Abbreviation used for the state.</param>
        /// <param name="countryName">Name of the country this state belongs to.</param>
        /// <returns></returns>
        public BoolMessageItem<State> CreateState(string name, string abbreviation, string countryName)
        {
            State state = new State(name, 0, countryName, abbreviation);
            return CreateState(state);
        }


        /// <summary>
        /// Get the country with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BoolMessageItem<Country> Country(string name)
        {
            // Check for null.
            if (string.IsNullOrEmpty(name))
                return new BoolMessageItem<Country>(null, false, "Country not supplied.");

            var countryLookup = Cacher.Get<LookupMulti<Country>>("countries_lookup", 300, () => Location.Countries.ToLookUpMulti<string>("Name"));

            // Valid country.
            Country country = countryLookup.ContainsKey(name) ? countryLookup[name] : null;
            if (country == null)
                return new BoolMessageItem<Country>(null, false, "Unknown country supplied.");

            return new BoolMessageItem<Country>(country, true, string.Empty);
        }


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <returns></returns>
        public BoolMessageItem<IList<State>> StatesFor(string countryname)
        {
            BoolMessageItem<Country> countryResult = Country(countryname);
            if (!countryResult.Success)
                return new BoolMessageItem<IList<State>>(null, false, countryResult.Message);

            var country = countryResult.Item;
            var states = Cacher.Get<IList<State>>("states_list_for_" + country.RealId, 300, () => Location.States.GetAll().Where(s => s.CountryId == country.RealId && !s.IsAlias && s.IsActive).ToList());
            bool success = states != null && states.Count > 0;
            string message = success ? string.Empty : "States not available for country : " + countryname;
            return new BoolMessageItem<IList<State>>(states, success, message);
        }


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <param name="statename"></param>
        /// <returns></returns>
        public BoolMessageItem<IList<City>> CitiesFor(string countryname, string statename)
        {
            BoolMessageItem<Country> countryResult = Country(countryname);
            if (!countryResult.Success)
                return new BoolMessageItem<IList<City>>(null, false, countryResult.Message);

            var country = countryResult.Item;
            var states = Cacher.Get<StateLookUp>("states_lookup_for_" + country.RealId, 300, () => new StateLookUp(Location.States.GetAll().Where(s => s.CountryId == country.RealId && !s.IsAlias && s.IsActive).ToList()));
            State state = states[statename];
            if (state == null)
                return new BoolMessageItem<IList<City>>(null, false, "Unknown state supplied.");

            var cities = Cacher.Get<IList<City>>(string.Format("cities_for_{0}_{1}", country.RealId, state.RealId), 300, () => Location.Cities.GetAll().Where(c => c.CountryId == country.RealId && c.StateId == state.RealId && !c.IsAlias && c.IsActive).ToList());
            bool success = cities != null && cities.Count > 0;
            string message = success ? string.Empty : "Cities not available for country : " + country.Name + ", state : " + state.Name;
            return new BoolMessageItem<IList<City>>(cities, success, message);
        }
        #endregion


        #region Private Methods
        private IList<T> GetCached<T>(string key, bool enableCache, int cacheTime, Func<IList<T>> fetcher)
        {
            var latestitems = Cacher.Get<IList<T>>(key, enableCache, cacheTime, false, () => fetcher());

            if (latestitems == null || latestitems.Count == 0)
            {
                latestitems = fetcher();
                if (enableCache && latestitems != null && latestitems.Count > 0)
                {
                    Cacher.Insert(key, latestitems, cacheTime, false);
                }
            }
            return latestitems;
        }


        private T GetCachedLookup<T>(string key, bool enableCache, int cacheTime, Func<T> fetcher, Func<T, bool> check)
        {
            var latestitems = Cacher.Get<T>(key, enableCache, cacheTime, false, () => fetcher());

            if (check(latestitems))
            {
                latestitems = fetcher();
                if (enableCache)
                {
                    Cacher.Insert(key, latestitems, cacheTime, false);
                }
            }
            return latestitems;
        }
        
    
        /// <summary>
        /// Does a high-level check of the format supplied and determines what type
        /// of location input was supplied.
        /// 
        /// Formats are:
        /// 1. city                         - "Bronx"
        /// 2. city,state                   - "Bronx , New York"
        /// 3. city,state( abbreviation )   - "Bronx , NY"
        /// 4. city,country                 - "HomeTown , USA"
        /// 5. state                        - "New Jersey"
        /// 6. state abbreviation           - "NJ"
        /// 7. country                      - "Italy"
        /// the actuall parsing 
        /// </summary>
        /// <param name="locationData">The location to parse. Can be any combination of
        /// inputs, check the summary above.</param>
        /// <returns></returns>
        private LocationLookUpResult InternalParse(string locationData)
        {
            CityLookUp cityLookUp = new CityLookUp(Cities.GetAll());
            StateLookUp stateLookUp = new StateLookUp(States.GetAll());
            CountryLookUp countryLookUp = new CountryLookUp(Countries.GetAll());

            try
            {
                bool isValidUSZipCode = IsUnitedStatesZipCode(locationData);
                bool containsComma = isValidUSZipCode ? false : locationData.Contains(",");

                // United states Zip code format
                if ( isValidUSZipCode )
                {
                    return ParseUnitedStatesZipCode(locationData);
                }

                // City, ( State or Country )
                // Comma indicates search by city, <state> or <country>
                if ( containsComma )
                {
                    return LocationParser.ParseCityWithStateOrCountry(cityLookUp, stateLookUp, countryLookUp, locationData);
                }
                
                // Check for city, state, or country. 
                // Start with narrowest search.
                // Check city.
                LocationLookUpResult result = LocationParser.ParseCity(cityLookUp, stateLookUp, countryLookUp, locationData);
                if ( result != LocationLookUpResult.Empty && result.IsValid )
                {
                    return result;
                }

                // Check State - 2nd most restrictive search.
                result = LocationParser.ParseState(stateLookUp, countryLookUp, locationData);
                if (result != LocationLookUpResult.Empty && result.IsValid)
                {
                    return result;                        
                }

                // Check country - 3rd and final search criteria                 
                result = LocationParser.ParseCountry(countryLookUp, locationData);
                if (result != LocationLookUpResult.Empty && result.IsValid)
                {
                    return result;                        
                }                                
            }
            catch (Exception ex)
            {
                // Some error during parsing.
                // Log the sucker into our system.
                Logger.Error("Error verifying search location", ex);
            }

            return new LocationLookUpResult(LocationLookUpType.None, false, "Unable to determine location");
        }


        /// <summary>
        /// Checks whether the location text supplied is empty.
        /// This trims any white space from both sides of the text before checking
        /// for empty string.
        /// </summary>
        /// <param name="locationData"></param>
        /// <returns></returns>
        private bool IsEmptyLocation(string locationData)
        {
            // Validate.
            if (string.IsNullOrEmpty(locationData))
            {
                return true;
            }
            locationData = locationData.Trim();
            if (locationData == string.Empty)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Determines if the location text supplied is a U.S. zipcode.
        /// This is true if text is 5 characters that are all numbers.
        /// </summary>
        /// <param name="locationText">e.g. "10465"</param>
        /// <returns></returns>
        private bool IsUnitedStatesZipCode(string locationText)
        {
            if (locationText.Length == LocationConstants.ZipCodeLength)
            {
                int zipcodeParsed = 0;
                if (int.TryParse(locationText, out zipcodeParsed))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Parse the zipcode representing a United States Zipcode.
        /// This must be a 5 digit zipcode.
        /// e.g. "10465"
        /// </summary>
        /// <param name="zip"></param>
        /// <returns></returns>
        private LocationLookUpResult ParseUnitedStatesZipCode(string zip)
        {
            LocationLookUpResult result;
            if (zip.Length == 5)
            {
                for(int ndx = 0; ndx < zip.Length; ndx++)
                {
                    if (!Char.IsDigit(zip, ndx))
                    {
                        result = new LocationLookUpResult(LocationLookUpType.Zip, false, "U.S. Zip code format is not valid.");
                        result.Zip = zip;
                        result.CountryId = LocationConstants.CountryId_USA;
                        return result;
                    }
                }
            }

            result = new LocationLookUpResult(LocationLookUpType.Zip, true, string.Empty);
            result.Zip = zip;
            result.CountryId = LocationConstants.CountryId_USA;
            return result;
        }


        private void Init(IRepository<Country> countryDao, IRepository<State> stateDao, IRepository<City> cityDao)
        {
            Cities = cityDao;
            States = stateDao;
            Countries = countryDao;
            _settings = new LocationSettings();
        }
        #endregion
    }
}