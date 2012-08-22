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

using ComLib.Entities;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Location service to parse locatiion data.
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Cities repository
        /// </summary>
        IRepository<City> Cities { get; set; }


        /// <summary>
        /// States repository.
        /// </summary>
        IRepository<State> States { get; set; }


        /// <summary>
        /// Countries repository.
        /// </summary>
        IRepository<Country> Countries { get; set; }


        /// <summary>
        /// Get the cities lookup
        /// </summary>
        CityLookUp CitiesLookup { get; }
        
        
        /// <summary>
        /// Get the city list.
        /// </summary>
        IList<City> CitiesList { get; }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        CountryLookUp CountriesLookup { get; }


        /// <summary>
        /// Get the country lookup.
        /// </summary>
        IList<Country> CountriesList { get; }


        /// <summary>
        /// Get the city lookup.
        /// </summary>
        StateLookUp StatesLookup { get; }


        /// <summary>
        /// Get the states list.
        /// </summary>
        IList<State> StatesList { get; }


        /// <summary>
        /// Get the location settings.
        /// </summary>
        LocationSettings Settings { get; }


        /// <summary>
        /// Parses string for either the zip or city/state.
        /// e.g.
        ///     City:  "Bronx", "Stamford"
        ///     State: "NY", "NJ", "California"
        ///     CityState: "Queens,New York"
        ///     Country: "USA"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        LocationLookUpResult Parse(string text);


        /// <summary>
        /// Create a city in the underlying datastore using the fields supplied
        /// </summary>
        /// <param name="name">Name of the city</param>
        /// <param name="state">Name of the state the city is in</param>
        /// <param name="country">Name of the country the city is in</param>
        /// <returns></returns>
        BoolMessageItem<City> CreateCity(string name, string state, string country);


        /// <summary>
        /// Create a country with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        BoolMessageItem<Country> CreateCountry(string name);


        /// <summary>
        /// Create the country using the name, alias.
        /// </summary>
        /// <param name="name">Country name</param>
        /// <param name="alias">Alias for the country</param>
        /// <param name="isAlias">Whether or not creating an alias.</param>
        /// <returns></returns>
        BoolMessageItem<Country> CreateCountry(string name, string alias, bool isAlias);


        /// <summary>
        /// Create the state.
        /// </summary>
        /// <param name="name">Name of the state.</param>
        /// <param name="abbreviation">Abbreviation used for the state.</param>
        /// <param name="country">Name of the country this state belongs to.</param>
        /// <returns></returns>
        BoolMessageItem<State> CreateState(string name, string abbreviation, string country);


        /// <summary>
        /// Get the country with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        BoolMessageItem<Country> Country(string name);


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <returns></returns>
        BoolMessageItem<IList<State>> StatesFor(string countryname);


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        BoolMessageItem<IList<City>> CitiesFor(string countryname, string states);
    }
}
