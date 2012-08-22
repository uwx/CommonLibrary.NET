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
using System.IO;
using System.Collections;
using System.Reflection;
using System.ServiceProcess;
using System.Web;
using System.Diagnostics;


namespace ComLib.Diagnostics
{

    /// <summary>
    /// Obtains diagnostic information related to the Machine, Currently executing 
    /// process, among other areas.
    /// </summary>
    public class Diagnostics
    {
        private static Func<IDiagnosticsService> _serviceCreator;

        
        /// <summary>
        /// Default initialization.
        /// </summary>
        static Diagnostics()
        {
            _serviceCreator = () => new DiagnosticsService();
        }


        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="serviceCreator">Function to execute.</param>
        public static void Init(Func<IDiagnosticsService> serviceCreator)
        {
            _serviceCreator = serviceCreator;
        }


        /// <summary>
        /// Get all the diagnostic information.
        /// </summary>
        /// <returns>Diagnostic information.</returns>
        public static string GetAllInfo()
        {
            return GetInfo(string.Empty);
        }


        /// <summary>
        /// Get all the diagnostic information.
        /// <param name="groups">MachineInfo,AppDomain,Env_System,Env_User</param>
        /// </summary>
        /// <returns>Dictionary with diagnostic information.</returns>
        public static IDictionary GetDataAsDictionary(params DiagnosticGroup[] groups)
        {
            IDiagnosticsService svc = _serviceCreator();

            // Env_System,Env_User,Drives
            if (groups == null || groups.Length == 0)
            {
                var filter = "MachineInfo,AppDomain,Env_System,Env_User";
                svc.FilterOn(true, filter);
            }
            else            
                svc.FilterOn(true, groups);

            IDictionary data = svc.GetData();
            return data;
        }


        /// <summary>
        /// Get all the information associated with the specified groups.
        /// </summary>
        /// <param name="commaDelimitedGroups">String with comma-delimited groups.</param>
        /// <returns>Information for specified groups.</returns>
        public static string GetInfo(string commaDelimitedGroups)
        {
            IDiagnosticsService svc = _serviceCreator();
            svc.FilterOn(true, commaDelimitedGroups);
            string data = svc.GetDataTextual();
            return data;
        }


        /// <summary>
        /// Get all the information associated with the specified groups.
        /// </summary>
        /// <param name="groups">Array with groups.</param>
        /// <returns>Information for specified groups.</returns>
        public static string GetInfo(params DiagnosticGroup[] groups)
        {
            IDiagnosticsService svc = _serviceCreator();
            svc.FilterOn(true, groups);
            string data = svc.GetDataTextual();
            return data;
        }


        /// <summary>
        /// Write all diagnostic information to the file specified.
        /// </summary>
        /// <param name="path"></param>
        public static void WriteAllInfo(string path)
        {
            WriteInfo(string.Empty, path, string.Empty);
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="commaDelimitedGroups">"Machine,AppDomain"</param>
        /// <param name="path">Path of file to write information to.</param>
        public static void WriteInfo(string commaDelimitedGroups, string path)
        {
            WriteInfo(commaDelimitedGroups, path, string.Empty);
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="commaDelimitedGroups">"Machine,AppDomain"</param>
        /// <param name="path">Path of file to write information to.</param>
        /// <param name="referenceMessage">Reference message.</param>
        public static void WriteInfo(string commaDelimitedGroups, string path, string referenceMessage)
        {
            IDiagnosticsService svc = _serviceCreator();
            svc.WriteInfo(path, referenceMessage, commaDelimitedGroups);
        }


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="path">Path of file to write information to.</param>
        /// <param name="referenceMessage">Reference message.</param>
        /// <param name="groups">The groups to write out</param>
        public static void WriteInfo(string path, string referenceMessage, params DiagnosticGroup[] groups)
        {
            IDiagnosticsService svc = _serviceCreator();
            svc.WriteInfo(path, referenceMessage, groups);
        }
    }
}
