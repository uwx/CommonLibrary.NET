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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace ComLib.Diagnostics
{
    /// <summary>
    /// Interface to be implemented by a diagnostics service.
    /// </summary>
    public interface IDiagnosticsService
    {
        /// <summary>
        /// Filter the diagnostics on the list of groups
        /// representing the areas that can be diagnosed.
        /// </summary>
        /// <param name="groupNames">"MachineInfo,AppDomain,Drives"</param>
        /// <param name="include">Whether or the the groups supplied should be
        /// included, false value representing exclusion.</param>
        void FilterOn(bool include, List<string> groupNames);


        /// <summary>
        /// Filter the diagnostics on the supplied comma delimited list of groups
        /// representing the areas that can be diagnosed.
        /// </summary>
        /// <param name="groupNamesDelimited">"MachineInfo,AppDomain,Drives"</param>
        /// <param name="include">Whether or the the groups supplied should be
        /// included, false value representing exclusion.</param>
        void FilterOn(bool include, string groupNamesDelimited);


        /// <summary>
        /// Filter the diagnostics on the supplied list of groups
        /// representing the areas that can be diagnosed.
        /// </summary>
        /// <param name="include">Whether or the the groups supplied should be
        /// <param name="groups">Array with groups.</param>
        /// included, false value representing exclusion.</param>
        void FilterOn(bool include, params DiagnosticGroup[] groups);


        /// <summary>
        /// Get all diagnostic information about currently running process
        /// and machine information.
        /// </summary>
        /// <returns>Diagnostic information.</returns>
        IDictionary GetData();


        /// <summary>
        /// Get all diagnostic information about currently running process
        /// and machine information.
        /// </summary>
        /// <returns>Diagnostic information.</returns>
        string GetDataTextual();


        /// <summary>
        /// The names of the groups representing what can be diagnosed.
        /// </summary>
        ReadOnlyCollection<string> GroupNames { get; }


        /// <summary>
        /// The names of the groups representing what can be diagnosed.
        /// </summary>
        ReadOnlyCollection<string> GroupNamesAll { get; }


        /// <summary>
        /// Write all the diagnostic info to file.
        /// </summary>
        /// <param name="filePath">Path to file.</param>
        void WriteInfo(string filePath);


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="path">Path of file to write information to.</param>
        /// <param name="referenceMessage">Reference message.</param>
        /// <param name="commaDelimitedGroups">String with comma-delimited list of groups.</param>
        void WriteInfo(string path, string referenceMessage, string commaDelimitedGroups);


        /// <summary>
        /// Write diagnostic information associated with the delimited list
        /// of groups specified.
        /// </summary>
        /// <param name="path">Path of file to write information to.</param>
        /// <param name="referenceMessage">Reference message.</param>
        /// <param name="groups">Array with groups.</param>
        void WriteInfo(string path, string referenceMessage, params DiagnosticGroup[] groups);
    }
}
