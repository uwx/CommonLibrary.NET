using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.LocationSupport
{
    /// <summary>
    /// This class is used to hold configuration settings
    /// that define how location information will be handled.
    /// </summary>
    public class LocationSettings
    {
        /// <summary>
        /// Default class constructor.
        /// </summary>
        public LocationSettings()
        {
            CacheCityList = false;
            CacheCityLookup = false;
            CacheCountryList = false;
            CacheCountryLookup = false;
            CacheStateList = false;
            CacheStateLookup = false;
            CacheTimeForCitiesInSeconds = 300;
            CacheTimeForCountriesInSeconds = 300;
            CacheTimeForStatesInSeconds = 300;
        }


        /// <summary>
        /// Get/set whether country list is cached.
        /// </summary>
        public bool CacheCountryList { get; set; }


        /// <summary>
        /// Get/set whether state list is cached.
        /// </summary>
        public bool CacheStateList { get; set; }


        /// <summary>
        /// Get/set whether city list is cached.
        /// </summary>
        public bool CacheCityList { get; set; }


        /// <summary>
        /// Get/set whether country lookup is cached.
        /// </summary>
        public bool CacheCountryLookup { get; set; }


        /// <summary>
        /// Get/set whether the state lookup is cached.
        /// </summary>
        public bool CacheStateLookup { get; set; }


        /// <summary>
        /// Get/set whether the city lookup is cached.
        /// </summary>
        public bool CacheCityLookup { get; set; }


        /// <summary>
        /// Get/set the cache timeout, in seconds, for countries.
        /// </summary>
        public int CacheTimeForCountriesInSeconds { get; set; }


        /// <summary>
        /// Get/set the cache timeout, in seconds, for states.
        /// </summary>
        public int CacheTimeForStatesInSeconds { get; set; }


        /// <summary>
        /// Get/set the cache timeout, in seconds, for cities.
        /// </summary>
        public int CacheTimeForCitiesInSeconds { get; set; }
    }
}
