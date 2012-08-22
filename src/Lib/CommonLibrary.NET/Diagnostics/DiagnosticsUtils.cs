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
using System.IO;
using System.Collections;
using System.Reflection;
using System.ServiceProcess;
using System.Web;
using System.Diagnostics;


namespace ComLib.Diagnostics
{

    /// <summary>
    /// Utility class for diagnostics.
    /// </summary>
    public class DiagnosticsHelper
    {
        /// <summary>
        /// Returns a key value pair list of drives and available space.
        /// </summary>
        /// <returns>Dictionary with drive information.</returns>
        public static IDictionary GetDrivesInfo()
        {
            string[] drives = Environment.GetLogicalDrives();
            StringBuilder buffer = new StringBuilder();
            IDictionary allDriveInfo = new SortedDictionary<object, object>();

            foreach (string drive in drives)
            {
                System.IO.DriveInfo di = new System.IO.DriveInfo(drive);
                if (di.IsReady)
                {
                    SortedDictionary<string, string> driveInfo = new SortedDictionary<string, string>();
                    driveInfo["AvailableFreeSpace"] = (di.AvailableFreeSpace / 1000000).ToString() + " Megs";
                    driveInfo["DriveFormat"] = di.DriveFormat;
                    driveInfo["DriveType"] = di.DriveType.ToString();
                    driveInfo["Name"] = di.Name;
                    driveInfo["TotalFreeSpace"] = (di.TotalFreeSpace / 1000000).ToString() + " Megs";
                    driveInfo["TotalSize"] = (di.TotalSize / 1000000).ToString() + " Megs";
                    driveInfo["VolumeLabel"] = di.VolumeLabel;
                    driveInfo["RootDirectory"] = di.RootDirectory.FullName;
                    allDriveInfo[drive] = driveInfo;                    
                }
            }
            return allDriveInfo as IDictionary;
        }


        /// <summary>
        /// Get the machine level information.
        /// </summary>
        /// <returns>Dictionary with machine information.</returns>
        public static IDictionary GetMachineInfo()
        {
            // Get all the machine info.
            IDictionary machineInfo = new SortedDictionary<string, object>();
            machineInfo["Machine Name"] = Environment.MachineName;
            machineInfo["Domain"] = Environment.UserDomainName;
            machineInfo["User Name"] = Environment.UserName;
            machineInfo["CommandLine"] = Environment.CommandLine;
            machineInfo["ProcessorCount"] = Environment.ProcessorCount;
            machineInfo["OS Version Platform"] = Environment.OSVersion.Platform.ToString();
            machineInfo["OS Version ServicePack"] = Environment.OSVersion.ServicePack;
            machineInfo["OS Version Version"] = Environment.OSVersion.Version.ToString();
            machineInfo["OS Version VersionString"] = Environment.OSVersion.VersionString;
            machineInfo["System Directory"] = Environment.SystemDirectory;
            machineInfo["Memory"] = Environment.WorkingSet.ToString();
            machineInfo["Version"] = Environment.Version.ToString();
            machineInfo["Current Directory"] = Environment.CurrentDirectory;
            return machineInfo;
        }


        /// <summary>
        /// Get all the list of services.
        /// </summary>
        /// <returns>Dictionary with services information.</returns>
        public static IDictionary GetServices()
        {
            ServiceController[] services = ServiceController.GetServices();
            IDictionary serviceListing = new SortedDictionary<object, object>();

            // Get the list of services.
            for (int ndx = 0; ndx < services.Length; ndx++)
            {
                ServiceController service = services[ndx];

                // Only list running processes.
                if (service.Status == ServiceControllerStatus.Running)
                {
                    serviceListing[service.DisplayName] = service.ServiceName ;
                }
            }
            return serviceListing;
        }


        /// <summary>
        /// Get information about the currently executing process.
        /// </summary>
        /// <returns>Dictionary with app domain information.</returns>
        public static IDictionary GetAppDomainInfo()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            Assembly[] loadedAssemblies = domain.GetAssemblies();
            IDictionary assemblyInfo = new SortedDictionary<object, object>();

            foreach (Assembly assembly in loadedAssemblies)
            {
                LoadedModule mod = new LoadedModule();
                try
                {
                    mod.Name = assembly.CodeBase.Substring(assembly.CodeBase.LastIndexOf("/") + 1);
                    mod.FullPath = assembly.Location;
                    mod.Version = assembly.ImageRuntimeVersion;
                    FileInfo file = new FileInfo(assembly.Location);
                    mod.TimeStamp = file.LastWriteTime.ToString();
                    mod.Directory = file.Directory.FullName;
                }
                catch { }
                assemblyInfo[mod.Name + "_" + mod.Version] = mod;
            }
            return assemblyInfo;
        }


        /// <summary>
        /// Get all the loaded modules in the current process.
        /// </summary>
        /// <returns>Dictionary with modules information.</returns>
        public static IDictionary GetModules()
        {
            SortedDictionary<object, object> modules = new SortedDictionary<object, object>();
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                LoadedModule moduleInfo = new LoadedModule();
                try
                {
                    moduleInfo.Name = module.ModuleName;
                    moduleInfo.FullPath = module.FileName;
                    FileInfo file = new System.IO.FileInfo(module.FileName);
                    moduleInfo.Directory = file.DirectoryName;
                    moduleInfo.TimeStamp = file.LastWriteTime.ToString();
                    moduleInfo.Version = module.FileVersionInfo.FileMajorPart + "."
                            + module.FileVersionInfo.FileMinorPart + "."
                            + module.FileVersionInfo.FilePrivatePart + "."
                            + module.FileVersionInfo.FileBuildPart;
                }
                catch { }
                modules[moduleInfo.Name + "_" + moduleInfo.Version] = moduleInfo;
            }
            return modules;
        }


        /// <summary>
        /// Get information about the currently executing process.
        /// </summary>
        /// <returns>Dictionary with process information.</returns>
        public static IDictionary GetProcesses()
        {
            Process[] processlist = Process.GetProcesses();
            IDictionary processInfo = new SortedDictionary<object, object>();

            foreach (Process p in processlist)
            {
                processInfo[p.ProcessName] = "";
            }
            return processInfo;
        }


        /// <summary>
        /// System level environment levels.
        /// </summary>
        /// <returns>Dictionary with system level environmental variables.</returns>
        public static IDictionary GetSystemEnvVariables()
        {
            return GetEnvVariables(() => Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine));
        }


        /// <summary>
        /// User level environment variables.
        /// </summary>
        /// <returns>Dictionary with user level environmental variables.</returns>
        public static IDictionary GetUserEnvVariables()
        {
            return GetEnvVariables(() => Environment.GetEnvironmentVariables(EnvironmentVariableTarget.User));
        }


        /// <summary>
        /// Get the environment variables.
        /// </summary>
        /// <param name="envGetter">Function to get environment.</param>
        /// <returns>Dictionary with environmental variables.</returns>
        public static IDictionary GetEnvVariables(Func<IDictionary> envGetter)
        {
            SortedDictionary<object, object> sortedEnvs = new SortedDictionary<object, object>();
            IDictionary envs = envGetter();
            foreach (DictionaryEntry entry in envs)
            {
                sortedEnvs[entry.Key] = entry.Value;
            }
            return sortedEnvs;
        }
        

        /// <summary>
        /// Converts an array of diagnostic groups
        /// to a list of diagnostic groups.
        /// </summary>
        /// <param name="groups">Array with diagnostic groups.</param>
        /// <returns>List with diagnostic groups.</returns>
        public static List<string> ConvertEnumGroupsToStringList(DiagnosticGroup[] groups)
        {
            var groupList = new List<string>();
            foreach (var group in groups)
                groupList.Add(group.ToString());

            return groupList;
        }
    }



    /// <summary>
    /// Stores the information about a loaded assembly/module.
    /// </summary>
    public class LoadedModule
    {
        /// <summary>
        /// Get/set the module name.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Get/set the module path.
        /// </summary>
        public string FullPath { get; set; }


        /// <summary>
        /// Get/set the module directory.
        /// </summary>
        public string Directory { get; set; }


        /// <summary>
        /// Get/set the module version.
        /// </summary>
        public string Version { get; set; }


        /// <summary>
        /// Get/set the module time stamp.
        /// </summary>
        public string TimeStamp { get; set; }


        /// <summary>
        /// Get formatted text.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string text = string.Format("{0}, {1}, {2}, {3}, {4}", Name, FullPath, Directory, Version, TimeStamp);
            return text;
        }
    }
}
