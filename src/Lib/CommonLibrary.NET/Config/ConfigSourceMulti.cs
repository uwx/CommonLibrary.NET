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
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;



namespace ComLib.Configuration
{
    /// <summary>
    /// Config source multi - list of multiple config sources.
    /// Need to hit make this thread safe.
    /// </summary>
    public class ConfigSourceMulti : ConfigSection, IConfigSource
    {
        private IList<IConfigSource> _configSources;
        private string _sourcePath;
        private string _fullName;


        /// <summary>
        /// Config source list.
        /// </summary>
        /// <param name="sources"></param>
        public ConfigSourceMulti(IList<IConfigSource> sources)
        {
            _configSources = sources;
            
            // Iterate through each config and get the name/paths.
            _configSources.ForEach( configSource => 
            {
                configSource.OnConfigSourceChanged += new EventHandler(configSource_OnConfigSourceChanged);
                _fullName += configSource.Name + ",";
                _sourcePath += configSource.SourcePath + ",";
            });
            _sourcePath = _sourcePath.Replace("/", "\\");
            Merge();
        }


        /// <summary>
        /// Event handler for on config source changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void configSource_OnConfigSourceChanged(object sender, EventArgs e)
        {
            if (OnConfigSourceChanged != null)
                OnConfigSourceChanged(sender, e);
        }


        #region IConfigSource Members
        /// <summary>
        /// Notifies subscribers when any configsource was changed.
        /// </summary>
        public event EventHandler OnConfigSourceChanged;


        /// <summary>
        /// Initialization after construction.
        /// </summary>
        public void Init()
        {
        }


        /// <summary>
        /// Load settings.
        /// </summary>
        public void Load()
        {
            _configSources.ForEach(configSource => configSource.Load());
        }


        /// <summary>
        /// Save the sources.
        /// </summary>
        public void Save()
        {
            _configSources.ForEach(configSource => configSource.Save());
        }


        /// <summary>
        /// Get the source paths.
        /// e.g. c:\app\prod.config,c:\app\dev.config
        /// </summary>
        public string SourcePath
        {
            get { return _sourcePath; }
        }
        #endregion


        /// <summary>
        /// Merge all the config sources.
        /// </summary>
        protected virtual void Merge()
        {
            // The config sources are from highest inhertance to lowest.
            // e.g. prod,qa,dev.
            // prod inherits qa, inherited by dev.
            // This merge has to be done in reverse order.
            for(int ndx = _configSources.Count - 1; ndx >= 0; ndx--)
            {
                var configSource = _configSources[ndx];
                Merge(configSource, this);
            }
        }


        /// <summary>
        /// Merge with config source specified.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        protected virtual void Merge(IConfigSection source, IConfigSection dest)
        {
            // Get all the sections.
            foreach (DictionaryEntry entry in source)
            {
                // Create new config section.
                if (entry.Value is IConfigSection)
                {
                    IConfigSection newDest = null;
                    if (dest.Contains(entry.Key))
                    {
                        newDest = dest.GetSection(entry.Key.ToString());
                    }
                    else
                    {
                        newDest = new ConfigSection(entry.Key.ToString());
                        dest.Add(newDest.Name, newDest);
                    }
                    Merge(entry.Value as IConfigSection, newDest);
                }
                else // Just overwrite the keys.
                {
                    dest[entry.Key] = entry.Value;
                }
            }
        }
    }
}
