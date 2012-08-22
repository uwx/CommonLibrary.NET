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
using System.Collections;
using System.Collections.Generic;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Summary description for CountryDetails
    /// </summary>
    public class Country : LocationBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public Country()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="countryCode">The country code.</param>
        public Country(string name, string countryCode)
        {
            Name = name;
            CountryCode = countryCode;
            IsActive = true;
        }

        
        /// <summary>
        /// Same as abbreviation. This is more descriptive.
        /// </summary>
        public virtual string CountryCode
        {
            get { return Abbreviation; }
            set { Abbreviation = value; }
        }
    }
}