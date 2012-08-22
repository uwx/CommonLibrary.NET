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
using System.Text.RegularExpressions;


namespace ComLib.Subs
{
    
    /// <summary>
    /// Class to replace symbol names with actual values.
    /// Such as ${today}
    /// </summary>
    public class Substitutor
    {
        private static SubstitutionService _provider;


        /// <summary>
        /// Static initializer.
        /// </summary>
        static Substitutor()
        {
            _provider = new SubstitutionService();
        }


        /// <summary>
        /// Performs substitutions on all the string items in the list supplied.
        /// converts: List[0] = ${today} = 03/28/2009.
        /// </summary>
        /// <param name="names">List of strings for substitution.</param>
        public static void Substitute(List<string> names)
        {
            _provider.Substitute(names);
        }


        /// <summary>
        /// Get the interpreted value of the function call.
        /// e.g. 
        /// 1. "${today}" will return today's date in MM/dd/YYYY format.
        /// 2. "${T-1}"   will returns yesterdays date in MM/dd/YYYY format.
        /// 3. "${Env.Var('PYTHON_HOME')} will return the value of the environment variable "PYTHON_PATH"
        /// 4. "${Enc.Decode('28asd42=')} will decrypt the encrypted value supplied, 
        ///                               using the provider setup in the cryptography service.
        /// </summary>
        /// <param name="substitution">Substitution to use.</param>
        /// <returns>Interpreted value.</returns>
        public static string Substitute(string substitution)
        {
            return _provider[substitution];
        }


        /// <summary>
        /// Register custom substitutions.
        /// </summary>
        /// <param name="group">Group to use.</param>
        /// <param name="interpretedVals">Custom substitutions.</param>
        public static void Register(string group, IDictionary<string, Func<string, string>> interpretedVals)
        {
            _provider.Register(group, interpretedVals);
        }


        /// <summary>
        /// Add a single custom substitution interpretor for the respective group, key.
        /// </summary>
        /// <param name="group">Group to use.</param>
        /// <param name="replacement">String to substitute.</param>
        /// <param name="interpretor">Function to calculate substitution.</param>
        public static void Register(string group, string replacement, Func<string, string> interpretor)
        {
            _provider.Register(group, replacement, interpretor);
        }
    }
}
