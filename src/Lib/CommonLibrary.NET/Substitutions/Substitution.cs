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
    /// Substitution.
    /// </summary>
    public class Substitution
    {
        /// <summary>
        /// empty / null object.
        /// </summary>
        public static readonly Substitution Empty = new Substitution(string.Empty, string.Empty, false, new string[0]{});


        /// <summary>
        /// Group name.
        /// </summary>
        public string Groupname;


        /// <summary>
        /// Func name to call.
        /// </summary>
        public string FuncName;


        /// <summary>
        /// Arguments passed to func.
        /// </summary>
        public string[] Args;


        /// <summary>
        /// Is valid.
        /// </summary>
        public bool IsValid;


        /// <summary>
        /// Default.
        /// </summary>
        public Substitution() { }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="group">Group of substitution.</param>
        /// <param name="func">Substitution function.</param>
        /// <param name="isValid">Valid flag.</param>
        /// <param name="args">Substitution arguments.</param>
        public Substitution(string group, string func, bool isValid, string[] args)
        {
            Groupname = group;
            FuncName = func;
            Args = args;
            IsValid = isValid;
        }
    }
}
