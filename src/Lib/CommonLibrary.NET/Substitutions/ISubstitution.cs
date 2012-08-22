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


namespace ComLib.Subs
{
    /// <summary>
    /// This interface must be implemented by classes
    /// that want to provide a substitution service.
    /// </summary>
    public interface ISubstitutionService
    {
        /// <summary>
        /// Register a custom substitution for a group.
        /// </summary>
        /// <param name="group">Substitution group.</param>
        /// <param name="interpretedVals">Substitution to register.</param>
        void Register(string group, IDictionary<string, Func<string, string>> interpretedVals);


        /// <summary>
        /// Performs substitutions on all strings contained in the list.
        /// </summary>
        /// <param name="names">List with strings for substitution.</param>
        void Substitute(List<string> names);


        /// <summary>
        /// Get the interpreted value of the function call.
        /// </summary>
        /// <param name="funcCall">Function call to use.</param>
        /// <returns>Interpreted value.</returns>
        string this[string funcCall] { get; }
    }
}
