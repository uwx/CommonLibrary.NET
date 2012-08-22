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
    /// This class contains methods to help with environmental variables.
    /// </summary>
    public class EnvironmentVariables
    {
        /// <summary>
        /// Get environment variable from current process, user variable, machine variable.
        /// </summary>
        /// <param name="name">Environment variable name.</param>
        /// <returns>Environmental variable corresponding to the name.</returns>
        public static string GetAny(string name)
        {
            string env = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(env))
            {
                env = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
                if (string.IsNullOrEmpty(env))
                {
                    env = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
                }
            }
            return env;
        }
    }
}
