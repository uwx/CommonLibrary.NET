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
    /// Lexical parser used to parsing text.
    /// e.g. Such as parsing arguments to a program.
    /// </summary>
    /// <example>
    ///     IDictionary&lt;string, string&gt; args1 = LexArgs.Parse("-trace=4 -config=PROD -appid=Knowledgedrink");
    ///     IDictionary&lt;string, string&gt; args2 = LexArgs.Parse(" backup \"comment's\" 'blogs'");
    ///     IDictionary&lt;string, string&gt; args2 = LexArgs.Parse("appid=KnowledgeDrink --trace=4 --config=\"prod.config\"   BLOGS 'c: d: e:'");
    /// </example>
    public class LexArgs : LexBase
    {
        /// <summary>
        /// Default settings
        /// </summary>
        private static LexSettings _defaultSettings = new LexSettings();        


        /// <summary>
        /// Parse supplied text using default settings.
        /// </summary>
        /// <param name="line">Line to parse.</param>
        /// <returns>Dictionary with parsed results.</returns>
        public static IDictionary<string, string> Parse(string line)
        {
            return ToDictionary<string>(Parse(line, _defaultSettings));
        }


        /// <summary>
        /// Parse supplied text using default settings.
        /// </summary>
        /// <param name="line">Line to parse.</param>
        /// <returns>List with parsed results.</returns>
        public static List<string> ParseList(string line)
        {
            return Parse(line, _defaultSettings);
        }


        /// <summary>
        /// Parse supplied text using supplied settings.
        /// </summary>
        /// <param name="line">List to parse.</param>
        /// <param name="settings">Parsing settings to use.</param>
        /// <returns>List with parse results.</returns>
        public static List<string> Parse(string line, LexSettings settings)
        {
            LexArgs lex = new LexArgs(settings);
            return lex.ParseText(line);
        }


        /// <summary>
        /// Create using default settings.
        /// </summary>
        public LexArgs()
        {
            Init(_defaultSettings);
        }


        /// <summary>
        /// Create with supplied settings.
        /// </summary>
        /// <param name="settings">Parsing settings to use.</param>
        public LexArgs(LexSettings settings)
        {
            Init(settings);
        }
    }
}
