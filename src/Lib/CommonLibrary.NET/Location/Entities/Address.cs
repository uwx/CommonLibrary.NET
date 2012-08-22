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
    /// Address
    /// </summary>
    public class Address
    {
        private string _venue;
        private string _street;
        private string _city;
        private int _cityId;
        private string _state;
        private string _zip;
        private string _stateAbbr;
        private int _stateId;
        private int _countryId;
        private string _countryName;
        private static readonly Address _empty;
        private bool _isOnline;
        private bool _isEnabled;


        static Address()
        {
            Address empty = new Address();
            empty.Clear();
            _empty = empty;
        }


        /// <summary>
        /// Empty address.
        /// </summary>
        public static Address Empty
        {
            get { return _empty; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        public Address()
        {
            _isEnabled = true;
        }


        /// <summary>
        /// Initalize with data.
        /// </summary>
        /// <param name="venue">The venue.</param>
        /// <param name="street">The street.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="stateAbbr">The state abbr.</param>
        /// <param name="zip">The zip.</param>
        public Address(string venue, string street, string city, string state, string stateAbbr, string zip)
        {
            Set(venue, street, city, state, stateAbbr, zip, string.Empty);
        }


        /// <summary>
        /// Set the various fields of the address.
        /// </summary>
        /// <param name="venue">The venue.</param>
        /// <param name="street">The street.</param>
        /// <param name="city">The city.</param>
        /// <param name="state">The state.</param>
        /// <param name="stateAbbr">The state abbr.</param>
        /// <param name="zip">The zip.</param>
        /// <param name="country">The country.</param>
        public void Set(string venue, string street, string city, string state, string stateAbbr, string zip, string country)
        {
            _venue = venue;
            _street = street;
            _city = city;
            _stateAbbr = stateAbbr;
            _state = state;
            _zip = zip;
            _countryName = country;
            _isEnabled = true;
        }


        /// <summary>
        /// Gets or sets the city id.
        /// </summary>
        /// <value>The city id.</value>
        public virtual int CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }


        /// <summary>
        /// Gets or sets the state id.
        /// </summary>
        /// <value>The state id.</value>
        public virtual int StateId
        {
            get { return _stateId; }
            set { _stateId = value; }
        }


        /// <summary>
        /// Gets or sets the country id.
        /// </summary>
        /// <value>The country id.</value>
        public virtual int CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }


        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>The zip.</value>
        public virtual string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }


        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The city.</value>
        public virtual string City
        {
            get { return _city; }
            set { _city = value; }
        }


        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public virtual string State
        {
            get { return _state; }
            set { _state = value; }
        }


        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public virtual string Country
        {
            get { return _countryName; }
            set { _countryName = value; }
        }


        /// <summary>
        /// Gets or sets the state abbr.
        /// </summary>
        /// <value>The state abbr.</value>
        public virtual string StateAbbr
        {
            get { return _stateAbbr; }
            set { _stateAbbr = value; }
        }


        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public virtual string Street
        {
            get { return _street; }
            set { _street = value; }
        }


        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The street.</value>
        public virtual string Venue
        {
            get { return _venue; }
            set { _venue = value; }
        }


        /// <summary>
        /// Is Online.
        /// </summary>
        public virtual bool IsOnline
        {
            get { return _isOnline; }
            set { _isOnline = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }


        /// <summary>
        /// Whether or not there is any data for city, zip, state, country.
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                bool isEmpty = (string.IsNullOrEmpty(_countryName) && string.IsNullOrEmpty(_state)
                                && string.IsNullOrEmpty(_city) && string.IsNullOrEmpty(_zip));
                return isEmpty;
            }
        }


        /// <summary>
        /// Set the full address. (venue), (street), (city), (state), (zip), (country)
        /// </summary>
        public virtual string FullAddress
        {
            get
            {
                return ToOneLine();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                if (string.Compare(value, "online", true) == 0)
                {
                    IsOnline = true;
                    return;
                }

                string[] tokens = value.Split(',');
                if (tokens == null || tokens.Length == 0)
                    return;

                // Assume no venue, and country specified, default country to USA.
                if (tokens.Length == 4)
                    Set(string.Empty, tokens[0], tokens[1], tokens[2], string.Empty, tokens[3], "USA");
                if (tokens.Length == 5)                
                    Set(string.Empty, tokens[0], tokens[1], tokens[2], string.Empty, tokens[3], tokens[4]);
                else if( tokens.Length == 6)
                    Set(tokens[0], tokens[1], tokens[2], tokens[3], string.Empty, tokens[4], tokens[5]);
            }
        }


        /// <summary>
        /// Default to empty
        /// </summary>
        public void Clear()
        {
            _street = string.Empty;
            _city = string.Empty;
            _countryId = LocationConstants.CountryId_USA;
            _zip = string.Empty;
            _stateId = LocationConstants.StateId_NA_Online;
            _cityId = LocationConstants.CityId_NA;
            _state = string.Empty;
            _stateAbbr = string.Empty;
        }


        /// <summary>
        /// Single line format of address.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (_isOnline || !_isEnabled) return string.Empty;
            return _street + " " + _city + ", " + _stateAbbr + " " + _zip;
        }


        /// <summary>
        /// Converts the address to a single line address separating the street,city,state,zip,country by ",".
        /// </summary>
        /// <returns></returns>
        public string ToOneLine()
        {
            // Not applicable.
            if (!_isEnabled) return "N/A";
            if (_isOnline) return "Online";

            List<string> items = new List<string>();
            if (!string.IsNullOrEmpty(Street)) items.Add(Street);
            if (!string.IsNullOrEmpty(City)) items.Add(City);
            if (!string.IsNullOrEmpty(State)) items.Add(State);
            if (!string.IsNullOrEmpty(Zip)) items.Add(Zip);
            if (!string.IsNullOrEmpty(Country)) items.Add(Country);

            string line = items.Join(",");
            return line;
        }


        /// <summary>
        /// Converts the address to a single line address separating the street,city,state,zip,country by ",".
        /// </summary>
        /// <param name="displayLevel">The display level. "Venue", "Street", "City", "State", "Country"</param>
        /// <returns></returns>
        public string ToOneLine(string displayLevel)
        {
            if (string.IsNullOrEmpty(displayLevel)) return string.Empty;
            if (!_isEnabled) return "N/A";
            if (_isOnline) return "Online";

            displayLevel = displayLevel.ToLower();
            List<string> items = new List<string>();

            switch (displayLevel)
            {
                case "venue":
                    if (!string.IsNullOrEmpty(Venue)) items.Add(Venue);
                    if (!string.IsNullOrEmpty(Street)) items.Add(Street);
                    if (!string.IsNullOrEmpty(City)) items.Add(City);
                    if (!string.IsNullOrEmpty(State)) items.Add(State);
                    if (!string.IsNullOrEmpty(Zip)) items.Add(Zip);
                    if (!string.IsNullOrEmpty(Country)) items.Add(Country);
                    break;

                case "street":
                    if (!string.IsNullOrEmpty(Street)) items.Add(Street);
                    if (!string.IsNullOrEmpty(City)) items.Add(City);
                    if (!string.IsNullOrEmpty(State)) items.Add(State);
                    if (!string.IsNullOrEmpty(Zip)) items.Add(Zip);
                    if (!string.IsNullOrEmpty(Country)) items.Add(Country);
                    break;

                case "city":
                    if (!string.IsNullOrEmpty(City)) items.Add(City);
                    if (!string.IsNullOrEmpty(State)) items.Add(State);
                    if (!string.IsNullOrEmpty(Zip)) items.Add(Zip);
                    if (!string.IsNullOrEmpty(Country)) items.Add(Country);
                    break;

                case "state":
                    if (!string.IsNullOrEmpty(State)) items.Add(State);
                    if (!string.IsNullOrEmpty(Country)) items.Add(Country);
                    break;

                case "country":
                    if (!string.IsNullOrEmpty(Country)) items.Add(Country);
                    break;
            }
            string line = items.Join(", ");
            return line;
        }
    }
}
