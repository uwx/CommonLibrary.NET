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
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;

using ComLib.LocationSupport;


namespace ComLib.Maps
{

    /// <summary>
    /// This class is used to create links for Google maps.
    /// <a href="http://maps.google.com/maps?f=q&amp;hl=en&amp;q=10-11+12th+avenue,+queens+ny+12345"></a>
    /// <a href="http://maps.google.com/maps?f=q&amp;hl=en&amp;q=44+Levitt+avenue,+bronx+ny+12345"></a>
    /// </summary>
    public class GoogleMapUrlBuilder : IMapUrlBuilder
    {
        private string _urlPrefix;


        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapUrlBuilder"/> class.
        /// </summary>
        public GoogleMapUrlBuilder()
        {
            _urlPrefix = "http://maps.google.com/maps?f=q&amp;hl=en&amp;q=";
        }


        #region IMapUrlBuilder Members        
        /// <summary>
        /// Set the url prefix:
        /// <a href="http://maps.google.com/maps?f=q&amp;hl=en&amp;q="></a>
        /// </summary>
        public string UrlPrefix
        {
            get { return _urlPrefix; }
            set { _urlPrefix = value; }
        }


        /// <summary>
        /// builds the url.
        /// e.g. 
        /// Address : 439 calhoun ave. bronx, ny 10465
        /// ="439+calhoun+ave.+bronx,+ny+10465"
        /// </summary>
        /// <param name="address">Address to location.</param>
        /// <returns>Url with mapped address.</returns>
        public string Build(Address address)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(_urlPrefix);
            bool containsAddress = false;
            bool containsCity = false;
            bool containsState = false;

            if (!string.IsNullOrEmpty(address.Street))
            {
                buffer.Append(address.Street.Replace(' ', '+'));
                containsAddress = true;
            }
            if (!string.IsNullOrEmpty(address.City))
            {
                if (containsAddress)
                {
                    buffer.Append("+");
                }
                buffer.Append(address.City.Replace(' ', '+'));
                containsCity = true;
            }
            if (!string.IsNullOrEmpty(address.StateAbbr))
            {
                if (containsCity)
                {
                    buffer.Append("+");
                }
                buffer.Append(address.StateAbbr.Trim());
                containsState = true;
            }
            if (!containsState && !string.IsNullOrEmpty(address.State))
            {
                if (containsCity)
                {
                    buffer.Append("+");
                }
                buffer.Append(address.State.Trim());
                containsState = true;
            }
            if (!string.IsNullOrEmpty(address.Zip))
            {
                if (containsState)
                {
                    buffer.Append("+");
                }
                buffer.Append(address.Zip);
            }
            if (!string.IsNullOrEmpty(address.Country))
            {
                if (containsState)
                {
                    buffer.Append("+");
                }
                buffer.Append(address.Country);
            }
            string url = buffer.ToString();
            return url;
        }


        #endregion
    }
}
