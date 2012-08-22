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
using System.Linq;
using System.Text;


namespace ComLib
{
    /// <summary>
    /// Settings for Lex parser.
    /// </summary>
    public class LexSettings
    {
        /// <summary>
        /// Tokens used to enclose other tokens.
        /// </summary>
        public char[] QuotesChars = new char[] { '"', '\'' };


        /// <summary>
        /// Escape char
        /// </summary>
        public char EscapeChar = '\\';


        /// <summary>
        /// Chars used as white space.
        /// </summary>
        public char[] WhiteSpaceChars = new char[] { ' ', '\t' };


        /// <summary>
        /// New line tokens.
        /// </summary>
        public string[] EolChars = new string[] { "\n", "\r\n" };
    }
}
