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
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;


namespace ComLib.Configuration
{
    /// <summary> 
    /// Simple class to lookup stored configuration settings by key. 
    /// Also provides type conversion methods. 
    /// GetInt("PageSize"); 
    /// GetBool("IsEnabled"); 
    /// </summary> 
    public class ConfigSourceDecorator : IConfigSource
    {
        private IConfigSource _provider;

        /// <summary>
        /// Initialize w/ the actual provider.
        /// </summary>
        /// <param name="provider"></param>
        public ConfigSourceDecorator(IConfigSource provider)
        {
            _provider = provider;
        }


        #region IConfigSource Members

        /// <summary>
        /// Event handler for when the underlying config source changed.
        /// </summary>
        public event EventHandler OnConfigSourceChanged;


        /// <summary>
        /// The configuration source path.
        /// </summary>
        public string SourcePath
        {
            get { return _provider.SourcePath; }
        }


        /// <summary>
        /// Initialization after construction.
        /// </summary>
        public void Init()
        {
        }


        /// <summary>
        /// Load the configuration.
        /// </summary>
        public void Load()
        {
            _provider.Load();
        }


        /// <summary>
        /// Save the configuration
        /// </summary>
        public void Save()
        {
            _provider.Save();
        }

        #endregion


        #region IConfigSection Members
        /// <summary>
        /// Name of the configuration source.
        /// </summary>
        public string Name
        {
            get 
            {
                return _provider.Name;
            }
            set
            {
                _provider.Name = value;
            }
        }


        /// <summary>
        /// Get value of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return _provider.Get<T>(key);
        }


        /// <summary>
        /// Get value or default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetDefault<T>(string key, T defaultValue)
        {
            return _provider.GetDefault<T>(key, defaultValue);
        }


        /// <summary>
        /// Get the section/key value.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string section, string key)
        {
            return _provider.Get(section, key);
        }


        /// <summary>
        /// Get typed section/key value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string section, string key)
        {
            return _provider.Get<T>(section, key);
        }


        /// <summary>
        /// Get typed section/key value or default  value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetDefault<T>(string section, string key, T defaultValue)
        {
            return _provider.GetDefault<T>(section, key, defaultValue);
        }


        /// <summary>
        /// Get section with name.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public IConfigSection GetSection(string sectionName)
        {
            return _provider.GetSection(sectionName);
        }


        /// <summary>
        /// Get section with specified name at index.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="ndx"></param>
        /// <returns></returns>
        public IConfigSection GetSection(string sectionName, int ndx)
        {
            return _provider.GetSection(sectionName, ndx);
        }


        /// <summary>
        /// Get / set section/key value.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string sectionName, string key]
        {
            get
            {
                return _provider[sectionName, key];
            }
            set
            {
                _provider[sectionName, key] = value;
            }
        }


        /// <summary>
        /// Check if the section/key exists.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string section, string key)
        {
            return _provider.Contains(section, key);
        }


        /// <summary>
        /// Get the list of sections.
        /// </summary>
        public List<string> Sections
        {
            get { return _provider.Sections; }
        }


        /// <summary>
        /// Add the sectionname, key, value.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="overWrite"></param>
        public void Add(string sectionName, string key, object val, bool overWrite)
        {
            _provider.Add(sectionName, key, val, overWrite);
        }


        /// <summary>
        /// Add the sectionname, key/value.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void Add(string sectionName, string key, object val)
        {
            _provider.Add(sectionName, key, val);
        }


        /// <summary>
        /// Add multiple value to the same section/key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="overWrite"></param>
        public void AddMulti(string key, object val, bool overWrite)
        {
            _provider.AddMulti(key, val, overWrite);
        }

        #endregion


        #region IDictionary Members
        /// <summary>
        /// Add the key/value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(object key, object value)
        {
            _provider.Add(key, value);
        }


        /// <summary>
        /// Clearn all the entries.
        /// </summary>
        public void Clear()
        {
            _provider.Clear();
        }


        /// <summary>
        /// Indicates whether the key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(object key)
        {
            return _provider.Contains(key);
        }


        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator()
        {
            return _provider.GetEnumerator();
        }


        /// <summary>
        /// Indicate if fixed size.
        /// </summary>
        public bool IsFixedSize
        {
            get { return _provider.IsFixedSize; }
        }


        /// <summary>
        /// Is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get { return _provider.IsReadOnly; }
        }


        /// <summary>
        /// Get the keys.
        /// </summary>
        public ICollection Keys
        {
            get { return _provider.Keys; }
        }


        /// <summary>
        /// Remove the key.
        /// </summary>
        /// <param name="key"></param>
        public void Remove(object key)
        {
            _provider.Remove(key);
        }


        /// <summary>
        /// Get the values.
        /// </summary>
        public ICollection Values
        {
            get { return _provider.Values; }
        }


        /// <summary>
        /// Get / set the value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[object key]
        {
            get
            {
                return _provider[key];
            }
            set
            {
                _provider[key] = value;
            }
        }

        #endregion


        #region ICollection Members
        /// <summary>
        /// Copies the array starting at the specified index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            _provider.CopyTo(array, index);
        }


        /// <summary>
        /// Count
        /// </summary>
        public int Count
        {
            get { return _provider.Count; }
        }


        /// <summary>
        /// Whether or not this is synchronized.
        /// </summary>
        public bool IsSynchronized
        {
            get { return _provider.IsSynchronized; }
        }


        /// <summary>
        /// Get the synroot
        /// </summary>
        public object SyncRoot
        {
            get { return _provider.SyncRoot; }
        }
        #endregion


        #region IEnumerable Members
        /// <summary>
        /// GetEnumerator.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    } 
}
