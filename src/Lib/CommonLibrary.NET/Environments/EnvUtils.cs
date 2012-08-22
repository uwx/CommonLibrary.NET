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
    /// Utility class for loading inheritance based environments.
    /// </summary>
    public class EnvUtils
    {
        /// <summary>
        /// Get environment type from name.
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static EnvType GetEnvType(string env)
        {
            env = env.ToLower().Trim();
            // Determine prod/qa etc.
            if (env == "prod") return EnvType.Prod;
            if (env == "uat") return EnvType.Uat;
            if (env == "qa") return  EnvType.Qa;
            if (env == "dev") return EnvType.Dev;
            return EnvType.Unknown;
        }


        /// <summary>
        /// Parse the selected environments and config paths.
        /// </summary>
        /// <param name="envs">
        /// 1. "prod,qa,dev". If the names are the same as the types.
        /// 2. "prod1:prod,qa1:qa,mydev:dev" If the names are different that the env type names.
        /// </param>
        /// <returns>List of environment items.</returns>
        public static List<EnvItem> ParseEnvsToItems(string envs)
        {
            string[] envNames = envs.Contains(",") ? envs.Split(',') : new string[] { envs };
            var envItems = new List<EnvItem>();
            foreach (string env in envNames)
            {
                // Check for ":" which indicates name/type pair.
                string envName = env;
                string envType = env;
                if (env.Contains(":"))
                {
                    envName = env.Substring(0, env.IndexOf(":"));
                    envType = env.Substring(env.IndexOf(":") + 1);		
                }
                EnvItem item = new EnvItem();
                item.Name = envName;
                item.EnvType = EnvUtils.GetEnvType(envType);
                item.IsSelectable = true;
                item.Inherits = string.Empty;
                envItems.Add(item);
            }
            return envItems;
        }


        /// <summary>
        /// Parse the selected environments and config paths.
        /// </summary>
        /// <param name="envNames">"prod,qa,dev"</param>
        /// <returns></returns>
        public static List<string> ParseEnvsToNames(string envNames)
        {
            string[] envs = envNames.Contains(",") ? envNames.Split(',') : new string[] { envNames };
            return envs.ToList<string>();
        }


        /// <summary>
        /// Get list of names of environments that can be selected.
        /// </summary>
        /// <param name="envItems"></param>
        /// <returns></returns>
        public static List<string> GetSelectableEnvironments(List<EnvItem> envItems)
        {
            List<string> envNames = (from env in envItems where env.IsSelectable = true select env.Name).ToList<string>();
            return envNames;
        }


        /// <summary>
        /// Get list of inherited environments.
        /// </summary>
        /// <param name="selectedEnv">"prod,qa,dev"</param>
        /// <param name="firstEnv"></param>
        /// <returns></returns>
        public static List<EnvItem> Parse(string selectedEnv, out string firstEnv)
        {
            // Validate.
            if(string.IsNullOrEmpty(selectedEnv)) throw new ArgumentException("Must provide the selected environment.");
            List<string> envsSelected = ParseEnvsToNames(selectedEnv);

            // Create the environment.
            firstEnv = envsSelected[0];
            EnvItem env = new EnvItem(firstEnv, false, EnvUtils.GetEnvType(firstEnv), string.Empty);
            List<EnvItem> envs = new List<EnvItem>() { env };

            // More than 1 supplied. This represents an inheritance path.
            if (envsSelected.Count > 1)
            {
                for (int ndx = 1; ndx < envsSelected.Count; ndx++)
                {
                    string envName = envsSelected[ndx];
                    var newEnv = new EnvItem(envName, false, EnvUtils.GetEnvType(envName), "");
                    envs.Add(newEnv);

                    // Append the inheritance path to the first one.
                    envs[0].Inherits += (string.IsNullOrEmpty(env.Inherits)) ? envName : "," + envName;
                }
            }
            return envs;
        }


        /// <summary>
        /// Traverses the nodes inheritance path to build a single flat delimeted line of 
        /// inheritance paths.
        /// e.g. returns "Prod,Uat,Qa,Dev".
        /// </summary>
        /// <returns></returns>
        public static string ConvertNestedToFlatInheritance(EnvItem env, IDictionary<string, EnvItem> envItems)
        {
            // Return name of environment provided if it doesn't have 
            // any inheritance chain.
            if (string.IsNullOrEmpty(env.Inherits))
                return env.Name;

            // Single parent.
            if (env.Inherits.IndexOf(",") < 0)
            {
                // Get the parent.
                EnvItem parent = envItems[env.Inherits.Trim().ToLower()];
                return env.Name + "," + ConvertNestedToFlatInheritance(parent, envItems);
            }
            
            // Multiple parents.
            string[] parents = env.Inherits.Split(',');
            string path = env.Name;
            foreach (string parent in parents)
            {
                EnvItem parentEnv = envItems[env.Inherits.Trim().ToLower()];
                path += "," + ConvertNestedToFlatInheritance(parentEnv, envItems);
            }
            return path;
        }


        /// <summary>
        /// Loads an inheritance chain delimited by ,(comma)
        /// </summary>
        /// <param name="coreEnv"></param>
        /// <param name="envItems"></param>
        /// <returns></returns>
        public static List<EnvItem> LoadInheritance(EnvItem coreEnv, IDictionary<string, EnvItem> envItems)
        {
            // No inheritance chain.
            if (string.IsNullOrEmpty(coreEnv.Inherits)) return new List<EnvItem>() { coreEnv };

            string inheritancePath = coreEnv.Inherits;
            StringBuilder buffer = new StringBuilder();

            // Get flat list "prod,uat,qa,dev" from traversing the refrenced nodes.
            if (coreEnv.InheritsDeeply)
                inheritancePath = ConvertNestedToFlatInheritance(coreEnv, envItems);

            string[] parents = inheritancePath.Split(',');
            List<EnvItem> inheritanceList = new List<EnvItem>();

            // Build inheritance path where the first one is the core, following the
            // older parents.
            foreach (string parent in parents)
            {
                if (envItems.ContainsKey(parent))
                {
                    EnvItem parentEnv = envItems[parent];
                    inheritanceList.Add(parentEnv);
                }
            }
            return inheritanceList;
        }

        
        /// <summary>
        /// Build a delimited inheritance path of environments names using each
        /// of the envitems supplied.
        /// e.g. prod,qa,dev.
        /// if "," is the delimeter and "prod", "qa" are the environment names.
        /// </summary>
        /// <param name="inheritedChainedEnvs"></param>
        /// <param name="delimeter"></param>
        /// <param name="propGetter"></param>
        /// <returns></returns>
        public static string CollectEnvironmentProps(List<EnvItem> inheritedChainedEnvs, string delimeter, Func<EnvItem, string> propGetter )
        {
            // Check for null/empty.
            if (inheritedChainedEnvs == null || inheritedChainedEnvs.Count == 0)
                return string.Empty;

            // Only 1. no inheritance.
            if (inheritedChainedEnvs.Count == 1)
                return propGetter(inheritedChainedEnvs[0]);

            string chain = propGetter(inheritedChainedEnvs[0]);

            for (int ndx = 1; ndx < inheritedChainedEnvs.Count; ndx++)
                chain += delimeter + propGetter(inheritedChainedEnvs[ndx]);

            return chain;
        }
    }
}
