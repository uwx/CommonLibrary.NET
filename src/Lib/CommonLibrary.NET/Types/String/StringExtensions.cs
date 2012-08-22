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

namespace CommonLibrary
{
    public static class StringExtensions
    {
        /// <summary>
        /// Parses a delimited list of items into a readonly dictionary.
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static DictionaryReadOnly<string, string> ToReadOnlyDictionary(this string delimitedText, char delimeter)
        {
            return new DictionaryReadOnly<string, string>(ToDictionary(delimitedText, delimeter));
        }


        /// <summary>
        /// Convert to delimited text.
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static IDictionary<string, string> ToDictionary(this string delimitedText, char delimeter)
        {
            IDictionary<string, string> items = new Dictionary<string, string>();
            string[] tokens = delimitedText.Split(delimeter);

            // Check
            if (tokens == null) return new Dictionary<string, string>(items);

            foreach (string token in tokens)
            {
                items[token] = token;
            }
            return new Dictionary<string, string>(items);
        }


        /// <summary>
        /// Parses a delimited list of items into a string[].
        /// </summary>
        /// <param name="delimitedText"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string[] ToStringList(this string delimitedText, char delimeter)
        {
            if (string.IsNullOrEmpty(delimitedText))
                return null;

            string[] tokens = delimitedText.Split(delimeter);
            return tokens;
        }
    }
}
