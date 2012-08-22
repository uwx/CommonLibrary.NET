using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ComLib
{
    /// <summary>
    /// This class provides methods that can be used
    /// to check for and begin an upgrade process.
    /// </summary>
    public class Upgrade
    {
        private Version _currentVersion;
        private bool _catchErrors;
        private IErrors _errors = new Errors();


        /// <summary>
        /// Initialize w/ the current version.
        /// </summary>
        /// <param name="currentVersion">Current version.</param>
        /// <param name="catchErrors">Whether or not to catch errors and store them into the error collection.</param>
        public Upgrade(string currentVersion, bool catchErrors)
        {
            if (string.IsNullOrEmpty(currentVersion))
                throw new ArgumentNullException("currentVersion");

            _catchErrors = catchErrors;
            _currentVersion = ToVersion(currentVersion);
            CurrentVersion = currentVersion;
        }


        /// <summary>
        /// The current version.
        /// </summary>
        public string CurrentVersion { get; set; }


        /// <summary>
        /// Excutes the upgrade callback if conditions match.
        /// </summary>
        /// <param name="comparison">Comparison operation ("=", "!=", etc).</param>
        /// <param name="version">New version string.</param>
        /// <param name="callback">Callback to execute.</param>
        public void ExecuteIf(string comparison, string version, Action<object> callback)
        {
            ExecuteIf(comparison, version, null, null, null, callback);
        }


        /// <summary>
        /// Excutes the upgrade callback if conditions match.
        /// </summary>
        /// <param name="comparison">Comparison operation ("=", "!=", etc).</param>
        /// <param name="name">A name for the upgrade.</param>
        /// <param name="version">New version string.</param>
        /// <param name="callback">Callback to execute.</param>
        public void ExecuteIf(string comparison, string version, string name, Action<object> callback)
        {
            ExecuteIf(comparison, version, name, null, null, callback);
        }

        
        /// <summary>
        /// Excutes the upgrade callback if conditions match.
        /// </summary>
        /// <param name="comparison">Comparison operation ("=", "!=", etc).</param>
        /// <param name="name">A name for the upgrade description.</param>
        /// <param name="description">A description for the upgrade.</param>
        /// <param name="args">Args to pass to callback.</param>
        /// <param name="version">New version.</param>
        /// <param name="callback">Callback to execute.</param>
        public void ExecuteIf(string comparison, string version, string name, string description, object args, Action<object> callback)
        {
            Guard.IsNotNull(comparison, "Comparison must be supplied");
            Guard.IsNotNull(version, "Version to compare must be supplied.");
            Guard.IsNotNull(callback, "Callback method must be supplied");

            Version versionToCheck = ToVersion(version);
            bool execute = false;
            if (comparison == "=" && _currentVersion == versionToCheck
                || comparison == "<" && _currentVersion < versionToCheck
                || comparison == "<=" && _currentVersion <= versionToCheck
                || comparison == ">" && _currentVersion > versionToCheck
                || comparison == ">=" && _currentVersion >= versionToCheck
                || comparison == "!=" && _currentVersion != versionToCheck)
                execute = true;

            if (!execute) return;

            if (_catchErrors)
            {
                try
                {
                    callback(args);
                }
                catch (Exception ex)
                {
                    _errors.Add(string.Format("Error ugrading {0}:{1} : {2} ", name, description, ex.Message));
                }
            }
            else
                callback(args);
        }


        /// <summary>
        /// Converts a string version to a Version object.
        /// </summary>
        /// <param name="versionText">Version text.</param>
        /// <returns>Instance of equivalent Version object.</returns>
        public static Version ToVersion(string versionText)
        {
            string[] strParts = versionText.Split('.');
            List<int> parts = new List<int>();
            foreach (var part in strParts) parts.Add(Convert.ToInt32(part));
            Version version = null;
            if (parts.Count == 1)
                version = new Version(parts[0], 0);
            else if (parts.Count == 2)
                version = new Version(parts[0], parts[1]);
            else if (parts.Count == 3)
                version = new Version(parts[0], parts[1], parts[2], 0);
            else if (parts.Count == 4)
                version = new Version(parts[0], parts[1], parts[2], parts[3]);

            return version;
        }
    }
}
