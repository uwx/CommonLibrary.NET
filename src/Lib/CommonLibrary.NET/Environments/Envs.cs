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
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;


namespace ComLib.Environments
{
    /// <summary>
    /// Used to stored the environment for static access.
    /// </summary>
    public class Envs
    {
        private const string Default = "default";       
        private static IDictionary<string, IEnv> _environments = new Dictionary<string, IEnv>();


        /// <summary>
        /// Initialize w/ the selected environment and config paths.
        /// </summary>
        /// <param name="selected">Set either "prod | uat | qa | dev"</param>
        public static void Set(string selected)
        {
            Set(selected, "prod,uat,qa,dev", string.Empty, false, false);
        }


        /// <summary>
        /// Set the current environment and also supply available environment names/types.
        /// </summary>
        /// <param name="selected">"prod" or "prod1". This should match the name
        /// from <paramref name="availableEnvsCommaDelimited"/></param>
        /// <param name="availableEnvsCommaDelimited">
        /// 1. "prod,qa,dev". If the names are the same as the types.
        /// 2. "prod1:prod,qa1:qa,mydev:dev" If the names are different that the env type names.
        /// </param>
        public static void Set(string selected, string availableEnvsCommaDelimited)
        {
            Set(selected, availableEnvsCommaDelimited, string.Empty, false, false);
        }


        /// <summary>
        /// Set the current environment and also supply available environment names/types.
        /// </summary>
        /// <param name="selected">"prod" or "prod1". This should match the name
        /// from <paramref name="availableEnvsCommaDelimited"/></param>
        /// <param name="availableEnvsCommaDelimited">
        /// 1. "prod,qa,dev". If the names are the same as the types.
        /// 2. "prod1:prod,qa1:qa,mydev:dev" If the names are different that the env type names.
        /// </param>
        /// <param name="refPaths">The config reference paths. e.g. "prod.config,qa.config".</param>
        public static void Set(string selected, string availableEnvsCommaDelimited, string refPaths)
        {
            Set(selected, availableEnvsCommaDelimited, refPaths, false, false);
        }


        /// <summary>
        /// Set the current environment and also supply available environment names/types.
        /// </summary>
        /// <param name="selected">"prod" or "prod1". This should match the name
        /// from <paramref name="availableEnvsCommaDelimited"/></param>
        /// <param name="availableEnvsCommaDelimited">
        /// 1. "prod,qa,dev". If the names are the same as the types.
        /// 2. "prod1:prod,qa1:qa,mydev:dev" If the names are different that the env type names.
        /// </param>
        /// <param name="refPaths">The config reference paths. e.g. "prod.config,qa.config".</param>
        /// <param name="distributeRefPaths"></param>
        /// <param name="enableInheritance">Whether or not environment inheritance is enabled.</param>
        public static void Set(string selected, string availableEnvsCommaDelimited, string refPaths, bool distributeRefPaths, bool enableInheritance)
        {
            // Check available envs were supplied.
            if (string.IsNullOrEmpty(availableEnvsCommaDelimited))
                availableEnvsCommaDelimited = "prod,uat,qa,dev";

            // Parse "prod,uat,qa,dev" to List of EnvItems.
            var availableEnvs = EnvUtils.ParseEnvsToItems(availableEnvsCommaDelimited);

            // Environment Inheritance enabled. Default setup is :
            // e.g. Available Environments are "prod,uat,qa,dev".
            // Default inheritance becomes :
            // prod inherits uat
            // uat  inherits qa
            // qa   inherits dev
            if (enableInheritance)
            {                
                for (int ndx = 0; ndx <= availableEnvs.Count - 2; ndx++)
                    availableEnvs[ndx].Inherits = availableEnvs[ndx+1].Name;

                foreach (EnvItem env in availableEnvs) env.InheritsDeeply = true;
            }
            
            // Check if refpaths(configs) were supplied.
            bool hasConfigs = !string.IsNullOrEmpty(refPaths);
            string[] configs = hasConfigs ? refPaths.Split(',') : new string[] { };
            EnvItem selectedEnv = (from env in availableEnvs where string.Compare(env.Name, selected, true) == 0 select env).First<EnvItem>();

            if (hasConfigs)
            {
                // Case 1: Single config file = "prod.config"
                if (configs.Length == 1)
                    selectedEnv.RefPath = refPaths;

                // Case 2: Multiple config files "prod.config,qa.config" and not distributing across the env's.
                //         This means that ref paths "prod.config,qa.config" will be set on "selected" environment.
                else if (configs.Length > 1 && !distributeRefPaths)
                    selectedEnv.RefPath = refPaths;

                // Case 3: Multiple config files should be distributed to each environment.
                //         e.g. "prod,qa,dev", "prod.config,qa.config,dev.config".
                //         Apply prod.config to "prod" refpath, qa.config to "qa" refPath, dev.config to "dev" refpath.
                else if (configs.Length > 1 && distributeRefPaths)
                {
                    string error = string.Format("The number of reference paths({0}) must be less than or equal to the number of environments({1}).", configs.Length, availableEnvs.Count);
                    if(availableEnvs.Count < configs.Length) throw new ArgumentException(error);

                    for (int ndx = 0; ndx < availableEnvs.Count; ndx++)
                        availableEnvs[ndx].RefPath = configs[ndx];
                }
            }
            Set(Default, selected, availableEnvs);
        }


        /// <summary>
        /// Set the current environment.
        /// </summary>
        /// <param name="selected">Name of the selected environment</param>
        /// <param name="availableEnvs"></param>
        public static void Set(string selected, List<EnvItem> availableEnvs)
        {
            Set(Default, selected, availableEnvs); 
        }


        /// <summary>
        /// Set the current environment.
        /// </summary>
        /// <param name="envGroupName">Can have multiple environment definitions.
        /// If you just want to use the default one, supply emtpy string.</param>
        /// <param name="selected">"prod"</param>
        /// <param name="availableEnvs">All the available environments.</param>
        public static void Set(string envGroupName, string selected, List<EnvItem> availableEnvs)
        {
            IEnv envDef = new EnvDef(selected, availableEnvs);
            Set(envGroupName, envDef);                 
        }


        /// <summary>
        /// Register an environment for a specific group and also set the selected environment
        /// within the environment context for that group.
        /// </summary>
        /// <param name="env"></param>
        public static void Set(IEnv env)
        {
            Set(Default, env);
        }


        /// <summary>
        /// Register an environment for a specific group and also set the selected environment
        /// within the environment context for that group.
        /// </summary>
        /// <param name="envGroup">e.g. "database"</param>
        /// <param name="env"></param>
        public static void Set(string envGroup, IEnv env)
        {
            if (string.IsNullOrEmpty(envGroup)) envGroup = Default;

            _environments[envGroup] = env;
            if (envGroup == Default)
            {
                Env.Init(env);
            }
        }


        /// <summary>
        /// Get the current environment.
        /// </summary>
        public static IEnv Current { get { return _environments[Default]; } }


        /// <summary>
        /// Return the environment service the respective environment group.
        /// </summary>
        /// <param name="group">"database"</param>
        /// <returns></returns>
        public static IEnv Get(string group)
        {            
            if (!_environments.ContainsKey(group)) return null;

            return _environments[group];            
        }
    }
}
