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

namespace ComLib.Types
{
    /// <summary>
    /// This class contains type parser methods
    /// that also provide default values.
    /// </summary>
    public class TypeParsers
    {
        /// <summary>
        /// Parse the string as an int.
        /// </summary>
        /// <param name="val">String representation of integer.</param>
        /// <param name="defaultVal">Default value if string is null or empty.</param>
        /// <returns>Parsed integer.</returns>
        public static int ParseInt(string val, int defaultVal)
        {
            if (string.IsNullOrEmpty(val)) return defaultVal;

            int convertedVal = 0;

            if (!int.TryParse(val, out convertedVal))
                convertedVal = defaultVal;

            return convertedVal;
        }
    }
}
