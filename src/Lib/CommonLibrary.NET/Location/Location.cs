using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Static class to provide access to parsing, searching/lookup functionality on cities, states and countries.
    /// </summary>
    public class Location
    {
        private static ILocationService _provider;


        /// <summary>
        /// Initialize with 
        /// </summary>
        /// <param name="service"></param>
        public static void Init(ILocationService service)
        {
            _provider = service;
        }


        /// <summary>
        /// Instance of the location service
        /// </summary>
        public static ILocationService Service
        {
            get { return _provider; }
        }


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
        /// <param name="text">Text representing the location.</param>
        /// <returns></returns>
        public static LocationLookUpResult Parse(string text)
        {
            return _provider.Parse(text);
        }


        /// <summary>
        /// Create a city in the underlying datastore using the fields supplied
        /// </summary>
        /// <param name="name">Name of the city</param>
        /// <param name="state">Name of the state the city is in</param>
        /// <param name="country">Name of the country the city is in</param>
        /// <returns></returns>
        public static BoolMessageItem<City> CreateCity(string name, string state, string country)
        {
            return _provider.CreateCity(name, state, country);
        }
        

        /// <summary>
        /// Create a country with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BoolMessageItem<Country> CreateCountry(string name)
        {
            return _provider.CreateCountry(name);
        }


        /// <summary>
        /// Create the country using the name, alias.
        /// </summary>
        /// <param name="name">Country name</param>
        /// <param name="alias">Alias for the country</param>
        /// <param name="isAlias">Whether or not creating an alias.</param>
        /// <returns></returns>
        public static BoolMessageItem<Country> CreateCountry(string name, string alias, bool isAlias)
        {
            return _provider.CreateCountry(name, alias, isAlias);
        }


        /// <summary>
        /// Create the state.
        /// </summary>
        /// <param name="name">Name of the state.</param>
        /// <param name="abbreviation">Abbreviation used for the state.</param>
        /// <param name="country">Name of the country this state belongs to.</param>
        /// <returns></returns>
        public static BoolMessageItem<State> CreateState(string name, string abbreviation, string country)
        {
            return _provider.CreateState(name, abbreviation, country);
        }


        /// <summary>
        /// Repository for the cities
        /// </summary>
        public static IRepository<City> Cities { get { return _provider.Cities; } }


        /// <summary>
        /// Repository for the States
        /// </summary>
        public static IRepository<State> States { get { return _provider.States; } }


        /// <summary>
        /// Repository for the countries.
        /// </summary>
        public static IRepository<Country> Countries { get { return _provider.Countries; } }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public static CityLookUp CitiesLookup { get { return _provider.CitiesLookup; } }


        /// <summary>
        /// Get the city list.
        /// </summary>
        public static IList<City> CitiesList { get { return _provider.CitiesList; } }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public static CountryLookUp CountriesLookup { get { return _provider.CountriesLookup; } }


        /// <summary>
        /// Get the country lookup
        /// </summary>        
        public static IList<Country> CountriesList { get { return _provider.CountriesList; } }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        public static StateLookUp StatesLookup { get { return _provider.StatesLookup; } }


        /// <summary>
        /// Get the states list.
        /// </summary>
        public static IList<State> StatesList { get { return _provider.StatesList; } }


        #endregion


        /// <summary>
        /// Get country with the specified name.
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static BoolMessageItem<Country> GetCountry(string countryName)
        {
            return _provider.Country(countryName);
        }   


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <returns></returns>
        public static BoolMessageItem<IList<State>> StatesFor(string countryname)
        {
            return _provider.StatesFor(countryname);
        }


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <param name="statename"></param>
        /// <returns></returns>
        public static BoolMessageItem<IList<City>> CitiesFor(string countryname, string statename)
        {
            return _provider.CitiesFor(countryname, statename);
        }
    }
}
