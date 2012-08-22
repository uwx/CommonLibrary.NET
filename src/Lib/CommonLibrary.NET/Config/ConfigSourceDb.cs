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
using System.Data;

using ComLib.Entities;
using ComLib.IO;
using ComLib.Data;
using ComLib.ValidationSupport;


namespace ComLib.Configuration
{
    /// <summary> 
    /// Simple class to lookup stored configuration settings by key. 
    /// Also provides type conversion methods. 
    /// GetInt("PageSize"); 
    /// GetBool("IsEnabled"); 
    /// </summary> 
    public class ConfigSourceDb : ConfigSection, IConfigSource
    {
        private string _appName;
        private string _configName;
        private IRepository<ConfigItem> _repo;

        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="configName"></param>
        /// <param name="connectionString">Connection to a database that contains the log events table</param>
        public ConfigSourceDb(string appName, string configName, string connectionString)
            : this(appName, configName, connectionString, false)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="configName"></param>
        /// <param name="connectionString">Connection to a database that contains the log events table</param>
        /// <param name="doLoad"></param>
        public ConfigSourceDb(string appName, string configName, string connectionString, bool doLoad)
        {
            // Initialize with real Database using the RepositoryLinq2Sql.
            var conInfo = new ConnectionInfo(connectionString);
            var repo = new ConfigItemRepository(conInfo, new Database());
            Init(appName, configName, repo, doLoad);
        }


        /// <summary>
        /// Constructor taking the IRepository that will handle CRUD operations
        /// of log messages to the Database.
        /// Also, the repo provided can be a FAKE( In-Memory ) implementation which
        /// is useful in testing.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="configName"></param>
        /// <param name="repo"></param>
        /// <param name="doLoad"></param>
        public ConfigSourceDb(string appName, string configName, IRepository<ConfigItem> repo, bool doLoad)
        {
            Init(appName, configName, repo, doLoad);
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="appName">"MyApplication"</param>
        /// <param name="configName">"dev.config,qa.config"</param>
        /// <param name="repo"></param>
        /// <param name="doLoad"></param>
        public void Init(string appName, string configName, IRepository<ConfigItem> repo, bool doLoad)
        {
            _appName = appName;
            _configName = configName;
            Name = _configName;
            _repo = repo;
            if(doLoad) Load();
        }


        #region IConfigSource Members
        /// <summary>
        /// Event handler for when the underlying config source changed.
        /// </summary>
        public event EventHandler OnConfigSourceChanged;


        /// <summary>
        /// The source path. "dev.config".
        /// </summary>
        public virtual string SourcePath
        {
            get { return _configName; }
        }


        /// <summary>
        /// Initialization after construction.
        /// </summary>
        public virtual void Init()
        {
        }


        /// <summary>
        /// Load from database.
        /// </summary>
        public virtual void Load()
        {
            var criteria = Query<ConfigItem>.New().Where(c => c.App).Is(_appName)
                                                     .And(c => c.Name).Is(_configName);
            var items = _repo.Find(criteria);

            if (items.Count == 0)
                return;

            // Load all the items.
            foreach (var item in items)
            {
                // Convert to the appropriate type.
                Type type = Type.GetType(item.ValType, true, true);
                object result = Converter.ConvertTo(type, item.Val);
                this[item.Section, item.Key] = result;
            }

            // Notifiy
            if (OnConfigSourceChanged != null)
                OnConfigSourceChanged(this, null);
        }


        /// <summary>
        /// Save to the database.
        /// </summary>
        public virtual void Save()
        {
            var itemsFromThis = new List<ConfigItem>();
            PopulateItems(itemsFromThis, this);
            
            var itemsToDelete = new List<ConfigItem>();
            var itemsToCreate = new List<ConfigItem>();
            var itemsToUpdate = new List<ConfigItem>();
            IConfigSource iniDoc = new IniDocument();
            var criteria = Query<object>.New().Where("App").Is(_appName).And("Name").Is(_configName);
            var fromDb = _repo.Find(criteria);

            foreach (var dbItem in fromDb)
            {
                // Does not exist.
                if (!this.Contains(dbItem.Section, dbItem.Key))                    
                {
                    itemsToDelete.Add(dbItem);
                }
                else
                {
                    itemsToUpdate.Add(dbItem);
                    dbItem.Val = this[dbItem.Section, dbItem.Key].ToString();
                }
                iniDoc[dbItem.Section, dbItem.Key] = dbItem.Val;
            }
            foreach(var itemToCreate in itemsFromThis)
            {
                if (!iniDoc.Contains(itemToCreate.Section, itemToCreate.Key))
                {
                    itemToCreate.CreateDate = DateTime.Now;
                    itemToCreate.CreateUser = "";
                    itemToCreate.UpdateDate = DateTime.Now;
                    itemToCreate.UpdateUser = "";
                    itemsToCreate.Add(itemToCreate);
                }

            }
            
            // Now persist to db.
            itemsToDelete.ForEach(c => _repo.Delete(c.Id));
            itemsToUpdate.ForEach(c => _repo.Update(c));
            itemsToCreate.ForEach(c => _repo.Create(c));
        }
        #endregion


        private void PopulateItems(List<ConfigItem> items, IConfigSection section)
        {
            foreach (DictionaryEntry entry in section)
            {
                if (entry.Value is IConfigSection)
                    PopulateItems(items, entry.Value as IConfigSection);

                else
                {
                    var item = new ConfigItem();
                    item.Name = this.Name;
                    item.App = this._appName;
                    item.Section = section.Name;
                    item.Key = entry.Key.ToString();                    
                    item.Val = entry.Value == null ? string.Empty : entry.Value.ToString();
                    item.ValType = entry.Value == null ? string.Empty : entry.Value.GetType().FullName;
                    items.Add(item);
                }
            }
        }
    }
}
