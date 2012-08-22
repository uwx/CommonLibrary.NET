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
    /// Class to represent the currently selected environment.
    /// This is just a provider pattern.
    /// </summary>
    public class Env
    {
        private static IEnv _provider;


        /// <summary>
        /// Event handler for an environment change.
        /// </summary>
        public static event EventHandler OnChange;


        static Env()
        {
            if (_provider == null)
                Envs.Set("dev");
        }


        /// <summary>
        /// Initialize with current environment.
        /// </summary>
        /// <param name="environment"></param>
        public static void Init(IEnv environment)
        {
            _provider = environment;
            _provider.OnEnvironmentChange += new EventHandler(Current_OnEnvironmentChange);
        }
        

        #region Public Properties
        /// <summary>
        /// Name of current envionment selected during initialization. e.g. "london.prod" or "prod | uat | qa | dev".
        /// </summary>
        public static string Name
        {
            get { return _provider.Selected.Name; }
        }


        /// <summary>
        /// The environment type (prod, qa, etc ) of current selected environment
        /// </summary>
        public static EnvType EnvType 
        { 
            get { return _provider.EnvType; } 
        }


        /// <summary>
        /// Inheritance path of the currently selected environment.
        /// e.g. prod could inherit from qa->dev.
        /// </summary>
        public static string Inherits
        {
            get { return _provider.Inherits; }
        }


        /// <summary>
        /// Inheritance list of environments.
        /// Prod->Qa->Dev
        /// </summary>
        public static List<EnvItem> Inheritance
        {
            get { return _provider.Inheritance; }
        }


        /// <summary>
        /// The current environment.
        /// </summary>
        public static EnvItem Selected
        {
            get { return _provider.Selected; }
        }


        /// <summary>
        /// Get the current reference path. Which could be the 
        /// paths to the config files.
        /// </summary>
        public static string RefPath
        {
            get { return _provider.RefPath; }
        }


        /// <summary>
        /// Provides list of names( "prod,uat,qa,dev") of available environments than can be selected by user.
        /// </summary>
        public static List<string> Available
        {
            get { return _provider.Available; }
        }


        /// <summary>
        /// Is current env type production.
        /// </summary>
        public static bool IsProd { get { return _provider.IsProd; } }


        /// <summary>
        /// Is current env type Qa
        /// </summary>
        public static bool IsQa { get { return _provider.IsQa; } }


        /// <summary>
        /// Is current env type development.
        /// </summary>
        public static bool IsDev { get { return _provider.IsDev; } }


        /// <summary>
        /// Is current env type uat.
        /// </summary>
        public static bool IsUat { get { return _provider.IsUat; } }


        /// <summary>
        /// Get the env entry associated with the name.
        /// </summary>
        /// <param name="envName"></param>
        /// <returns></returns>
        public static EnvItem Get(string envName)
        {
            return _provider.Get(envName);
        }


        /// <summary>
        /// Get the number of available envs.
        /// </summary>
        public static int Count { get { return _provider.Count; } }
        #endregion


        /// <summary>
        /// Change the environment set environment name e.g. ("ny.prod,uat,qa,dev").
        /// </summary>
        /// <param name="environmentName"></param>
        public static void Change(string environmentName)
        {
            _provider.Change(environmentName);
        }


        /// <summary>
        /// Notifiy environment changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Current_OnEnvironmentChange(object sender, EventArgs e)
        {
            if (OnChange != null)
                OnChange(null, e);
        }

    }

}
