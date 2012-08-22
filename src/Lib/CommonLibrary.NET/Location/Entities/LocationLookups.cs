using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Class to provide fast lookup for cities.
    /// The base class provides lookup by 
    /// 1. City id.
    /// 2. City name.
    /// 
    /// This class extends the lookup by also being able to lookup
    /// a city by country id.
    /// </summary>
    /// <remarks>
    /// Instead of storing another set of indexes for cityname, countryId
    /// This only stores the cityname, countryId
    /// for duplicate city names.
    /// </remarks>
    public class CityLookUp : LocationLookUpWithCountry<City>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cities"></param>
        public CityLookUp(IList<City> cities) : base(cities)
        {
        }
    }



    /// <summary>
    /// Class to lookup the states
    /// </summary>
    public class StateLookUp : LocationLookUpWithCountry<State>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allStates"></param>
        public StateLookUp(IList<State> allStates) : base(allStates)
        {
        }
    }



    /// <summary>
    /// Lookup countries by name or id.
    /// </summary>
    public class CountryLookUp : LookupMulti<Country>
    {
        /// <summary>
        /// Initialize the lookup
        /// </summary>
        /// <param name="countries"></param>
        public CountryLookUp(IList<Country> countries)            
        {
            Init(countries, null, item => item.Id, item2 => item2.Name.ToLower(), null);
        }
    }



    /// <summary>
    /// Class to provide fast lookup for location components (cities and states)
    /// that have a country id.
    /// The base class provides lookup by 
    /// 1. id.
    /// 2. name.
    /// 3. name and countryid.
    /// 
    /// This class extends the lookup by also being able to lookup
    /// a city by country id.
    /// </summary>
    /// <remarks>
    /// Instead of storing another set of indexes for cityname, countryId
    /// This only stores the cityname, countryId
    /// for duplicate city names.
    /// </remarks>
    public class LocationLookUpWithCountry<T> : LookupMulti<T> where T : LocationCountryBase
    {
        private IDictionary<string, T> _itemsByAbbreviation;
        private IDictionary<string, T> _itemsByCountryId;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="items"></param>
        public LocationLookUpWithCountry(IList<T> items)
        {
            Init(items, (item) => !item.IsAlias, item => item.Id, item2 => item2.Name.ToLower(), null);
        }



        /// <summary>
        /// Allow lookup by both the full state name and it's abbreviation.
        /// eg. New York or "NY"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T this[string id]
        {
            get
            {
                T result = base[id];
                if (result != default(T))
                    return result;

                if( !_itemsByAbbreviation.ContainsKey(id))
                    return default(T);

                result = _itemsByAbbreviation[id];
                return result;
            }
        }

        /// <summary>
        /// Initialize the lookup by :
        /// 1. searching by id
        /// 2. searching by name
        /// 3. searching by name and countryid.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="predicate"></param>
        /// <param name="id1Getter"></param>
        /// <param name="strId2Getter"></param>
        /// <param name="callback"></param>
        public override void Init(IList<T> items, Func<T, bool> predicate, Func<T, int> id1Getter, Func<T, string> strId2Getter, Action<T, int, string> callback)
        {
            _itemsByCountryId = new Dictionary<string, T>();
            _itemsByAbbreviation = new Dictionary<string, T>();

            base.Init(items, predicate, id1Getter, strId2Getter, (item, id, strId) =>
            {
                // Store by city/country.
                string nameWithCountry = BuildKey(item.Name, item.CountryId);
                _itemsByCountryId[nameWithCountry] = item;

                // Store abbreviations.
                if( !string.IsNullOrEmpty(item.Abbreviation))
                    _itemsByAbbreviation[item.Abbreviation.ToLower()] = item;
            });
        }


        /// <summary>
        /// Finds the city based on the country id.
        /// This is because there can be two countries with the same city name.
        /// e.g. City, Country
        ///      1. GeorgeTown, USA
        ///      2. GeorgeTown, Guyana
        /// </summary>
        /// <param name="name"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        public T FindByCountry(string name, int countryId)
        {
            T item = this[name];

            // Check if the city stored just by the name
            // is the same one being searched.
            if (item != null && item.CountryId == countryId)
            {
                return item;
            }

            // Now check the cityname_countryId indexes stored.
            string key = BuildKey(name, countryId);
            if (!_itemsByCountryId.ContainsKey(key)) return null;

            return _itemsByCountryId[key];
        }


        /// <summary>
        /// Creates a key string for a name and an id.
        /// </summary>
        /// <param name="name">Name-part of key.</param>
        /// <param name="id">Id-part of key.</param>
        /// <returns>String representation of key for supplied information.</returns>
        protected string BuildKey(string name, int id)
        {
            return name.Trim().ToLower() + "_" + id;
        }
    }
}
