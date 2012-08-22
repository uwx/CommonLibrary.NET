using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Entities;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// 
    /// </summary>
    public class LocationDataMassager
    {
        public static CityLookUp CitiesLookup { get; set; }


        public static StateLookUp StatesLookup { get; set; }


        public static CountryLookUp CountriesLookup { get; set; }


        public LocationDataMassager(CityLookUp cityLookup, StateLookUp stateLookup, CountryLookUp countryLookup)
        {
            CitiesLookup = cityLookup;
            StatesLookup = stateLookup;
            CountriesLookup = countryLookup;
        }


        /// <summary>
        /// Massage the address by setting it's cityid, stateid, countryid 
        /// from the city, state, country name.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="entityAction"></param>
        public static void Massage(Address address, EntityAction entityAction)
        {
            List<string> errors = new List<string>();
            LocationHelper.ApplyCountry(address, CountriesLookup, errors);
            LocationHelper.ApplyState(address, StatesLookup, errors);
            LocationHelper.ApplyCity(address, CitiesLookup, StatesLookup, CountriesLookup);
        }
    }
}
