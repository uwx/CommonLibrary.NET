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


namespace ComLib.Environments
{    
    /// <summary>
    /// Represents a single environment.
    /// </summary>
    /// <remarks>
    /// networkloc: "z:/env"
    /// localloc:   "c:/env"
    /// conn_dev2: "server:localhost, user=kishore, pass=kishore"
    /// 
    /// env: name=Dev,   Type=Dev,  Inherits="",           DeepInherit=Yes  RefPath="${localloc}/dev.config",    
    /// env: name=Dev2,  Type=Dev,  Inherits="",           DeepInherit=Yes  RefPath="${localloc}/dev2.config",   
    /// env: name=Qa,    Type=Qa,   Inherits="Dev",        DeepInherit=Yes  RefPath="${localloc}/qa.config",     
    /// env: name=Uat,   Type=Uat,  Inherits="Qa",         DeepInherit=Yes  RefPath="${localloc}/uat.config",    
    /// env: name=Prod,  Type=Prod, Inherits="Uat",        DeepInherit=Yes  RefPath="${localloc}/prod.config",   
    /// env: name=Lon,   Type=Prod, Inherits="Prod",       DeepInherit=Yes  RefPath="${localloc}/london.config", 
    /// env: name=kish,  Type=Dev,  Inherits="Dev2,Prod",  DeepInherit=No,  RefPath="${conn_dev2}", 
    /// </remarks>
    public class EnvItem
    {
        /// <summary>
        /// Dev or Qa, Name that uniquely identifies the environment.
        /// </summary>
        public string Name;
        

        /// <summary>
        /// Type environment type for this environment.
        /// </summary>
        public EnvType EnvType;


        /// <summary>
        /// Environmental Inheritance path.
        /// e.g. Uat. 
        /// if this environment name is Prod.
        /// and this inherit path is "Uat;CommonDev;"
        /// then this will load Uat backed by CommonDev settings.
        /// </summary>
        public string Inherits;


        /// <summary>
        /// e.g. Related to InheritPath.
        /// If Inherits from Env "UAT", this setting of true
        /// will also load all the dependent inherited files of UAT.
        /// </summary>
        public bool InheritsDeeply;
        

        /// <summary>
        /// Whether or not this is a selectable "Concrete" environment that 
        /// a user can choose from. Similar to abstract/concrete classes.
        /// I.e. Prod_NY might be a selectable environment, but
        /// Prod_Shared might not be as it may be a common envrionment
        /// that concrete environments like "prod_ny", "prod_london" inherit from.
        /// </summary>
        public bool IsSelectable { get; set; }


        /// <summary>
        /// Tag associated with environment.
        /// This can be used to store a reference to config files.
        /// e.g. prod.config.
        /// </summary>
        public string RefPath { get; set; }
        

        /// <summary>
        /// Default construction.
        /// </summary>
        public EnvItem() { }


        /// <summary>
        /// Initialize with the supplied values.
        /// </summary>
        /// <param name="name">"prod"</param>
        /// <param name="deepInherit"></param>
        /// <param name="envType">"Prod | Qa | Uat | Dev"</param>
        /// <param name="inherits"></param>
        public EnvItem(string name, bool deepInherit, EnvType envType, string inherits)
        {
            Name = name;
            IsSelectable = true;
            InheritsDeeply = deepInherit;
            EnvType = envType;
            Inherits = inherits;
        }
    }
}
