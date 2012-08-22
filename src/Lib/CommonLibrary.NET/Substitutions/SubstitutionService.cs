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
    //public delegate T Func<T>();
    //public delegate TResult Func<T, TResult>(T input);


    /// <summary>
    /// Class to replace symbol names with actual values.
    /// Such as ${today}
    /// </summary>
    public class SubstitutionService : ISubstitutionService
    {
        internal Dictionary<string, IDictionary<string, Func<string, string>>> _groups;


        /// <summary>
        /// Initialize.
        /// </summary>
        public SubstitutionService()
        {
            _groups = new Dictionary<string, IDictionary<string, Func<string, string>>>();
            Init();
        }
         

        /// <summary>
        /// Performs substitutions on all the string items in the list supplied.
        /// converts: List[0] = ${today} = 03/28/2009.
        /// </summary>
        /// <param name="names">List of strings for substitution.</param>
        public void Substitute(List<string> names)
        {
            for (int ndx = 0; ndx < names.Count; ndx++)
            {
                string name = names[ndx];
                string val = this[name];
                names[ndx] = val;
            }
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
        /// <param name="funcCall">Function call.</param>
        /// <returns>Interpreted value.</returns>
        public string this[string funcCall]
        {
            get
            {
                Substitution sub = SubstitutionUtils.Parse(funcCall, this);
                if (!sub.IsValid) return funcCall;

                return SubstitutionUtils.Eval(sub, this);
            }
        }


        /// <summary>
        /// Registers custom substitutions for the respective group.
        /// </summary>
        /// <param name="group">Substitution group.</param>
        /// <param name="interpretedVals">Custom substitutions.</param>
        public void Register(string group, IDictionary<string, Func<string, string>> interpretedVals)
        {
            _groups[group] = interpretedVals;
        }


        /// <summary>
        /// Registers custom substitution.
        /// </summary>
        /// <param name="group">Substitution group.</param>
        /// <param name="replacement">String to substitute.</param>
        /// <param name="interpretor">Function that calculates substitution value.</param>
        public void Register(string group, string replacement, Func<string, string> interpretor)
        {
            _groups[group][replacement] = interpretor;
        }


        /// <summary>
        /// Sets up the default substitution name/values.
        /// </summary>
        private void Init()
        {
            _groups = new Dictionary<string, IDictionary<string, Func<string, string>>>();

            // Default functions.
            _groups[""] = new Dictionary<string, Func<string, string>>();
            _groups[""]["today"]     = (s) => { return DateTime.Today.ToShortDateString(); };
            _groups[""]["yesterday"] = (s) => { return DateTime.Today.AddDays(-1).ToShortDateString(); };
            _groups[""]["tommorrow"] = (s) => { return DateTime.Today.AddDays(1).ToShortDateString(); };
            _groups[""]["t"]         = (s) => { return DateTime.Today.ToShortDateString(); };
            _groups[""]["t-1"]       = (s) => { return DateTime.Today.AddDays(-1).ToShortDateString(); };
            _groups[""]["t+1"]       = (s) => { return DateTime.Today.AddDays(1).ToShortDateString(); };
            _groups[""]["today+1"] = (s) => { return DateTime.Today.AddDays(1).ToShortDateString(); };
            _groups[""]["today-1"] = (s) => { return DateTime.Today.AddDays(-1).ToShortDateString(); };
            _groups[""]["username"] = (s) => { return Environment.UserName; };
            

            // Environment variable functions
            _groups["env"] = new Dictionary<string, Func<string, string>>();
            _groups["env"]["var"] = delegate(string name) { return Environment.GetEnvironmentVariable(name); };

            // Environment variable functions
            _groups["enc"] = new Dictionary<string, Func<string, string>>();
            _groups["enc"]["decode"] = delegate(string name) { return name; };
        }
    }
}
