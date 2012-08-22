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
    /// This class is used to create links for Yahoo maps.
    /// <a href="http://maps.yahoo.com/#mvt=m&amp;q1=100%20prox%20ave.%20bronx%20ny%2011225"></a>
    /// <a href="http://maps.yahoo.com/#mvt=m&amp;q1=105-88%206th%20ave.%20Queens%20NY%2012375"></a>
    /// </summary>
    public class YahooMapUrlBuilder : IMapUrlBuilder
    {
        private string _urlPrefix;


        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleMapUrlBuilder"/> class.
        /// </summary>
        public YahooMapUrlBuilder()
        {
            _urlPrefix = "http://maps.yahoo.com/#mvt=m&amp;q1=";
        }


        #region IMapUrlBuilder Members

        /// <summary>
        /// Set the url prefix:
        /// <a href="http://maps.yahoo.com/#mvt=m&amp;q1="></a>
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
                buffer.Append(address.Street.Replace(" ", "%20"));
                containsAddress = true;
            }
            if (!string.IsNullOrEmpty(address.City))
            {
                if (containsAddress)
                {
                    buffer.Append("%20");
                }
                buffer.Append(address.City.Replace(" ", "%20"));
                containsCity = true;
            }
            if (!string.IsNullOrEmpty(address.StateAbbr))
            {
                if (containsCity)
                {
                    buffer.Append("%20");
                }
                buffer.Append(address.StateAbbr.Trim());
                containsState = true;
            }
            if (!containsState && !string.IsNullOrEmpty(address.State))
            {
                if (containsCity)
                {
                    buffer.Append("%20");
                }
                buffer.Append(address.State.Trim());
                containsState = true;
            }
            if (!string.IsNullOrEmpty(address.Zip))
            {
                if (containsState)
                {
                    buffer.Append("%20");
                }
                buffer.Append(address.Zip);
            }
            if (!string.IsNullOrEmpty(address.Country))
            {
                if (containsState)
                {
                    buffer.Append("%20");
                }
                buffer.Append(address.Country);
            }
            string url = buffer.ToString();
            return url;
        }


        #endregion
    }
}
