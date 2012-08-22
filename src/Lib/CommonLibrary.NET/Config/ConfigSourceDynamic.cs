using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Collections;
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace ComLib.Configuration
{
    /// <summary>
    /// Config source dynamicall saves all properties that are in this or sub-classed type.
    /// </summary>
    public class ConfigSourceDynamic : IConfigSourceDynamic
    {
        private string _sourcePath;
        private bool _isSourcePathFileBased = false;
        private Func<IConfigSource> _configStoreFactory;
        private IRepository<ConfigItem> _repo;
        private IConfigSource _configStore;


        /// <summary>
        /// Default construction.
        /// </summary>
        public ConfigSourceDynamic()
        {
            string name = this.GetType().Name.Replace("Settings", "");
            SetNames(string.Empty, name);
            Init();
        }


        /// <summary>
        /// Initialize with the reference to source path.
        /// </summary>
        /// <param name="appName">E.g. "MyWebApp"</param>
        /// <param name="configName">E.g. "Dev.config"</param>
        /// <param name="sourcePath">File path or connectionstring</param>
        /// <param name="isSourcePathFileBased">Whether or not the source path is a file path or database connection string.</param>
        public ConfigSourceDynamic( string appName, string configName, string sourcePath, bool isSourcePathFileBased)
        {
            _sourcePath = sourcePath;
            SetNames(appName, configName);
            SetRepository(null, null);
            Init();
        }


        /// <summary>
        /// Initialize w/ lamda factory method for creating the configSourcePersistant
        /// </summary>
        /// <param name="appName">E.g. "MyWebApp"</param>
        /// <param name="configName">E.g. "Dev.config"</param>
        /// <param name="configRepo"></param>
        public ConfigSourceDynamic( string appName, string configName, IRepository<ConfigItem> configRepo)
        {
            SetNames(appName, configName);
            SetRepository(null, configRepo);
            Init();
        }


        /// <summary>
        /// Initialize w/ lamda factory method for creating the configSourcePersistant
        /// </summary>
        /// <param name="appName">E.g. "MyWebApp"</param>
        /// <param name="configName">E.g. "Dev.config"</param>
        /// <param name="configStoreFactoryMethod"></param>
        public ConfigSourceDynamic(string appName, string configName, Func<IConfigSource> configStoreFactoryMethod)
        {
            SetNames(appName, configName);
            SetRepository(configStoreFactoryMethod, null);
            Init();
        }


        /// <summary>
        /// Called after construction.
        /// </summary>
        public virtual void Init()
        {
        }


        /// <summary>
        /// Event handler for when the underlying config source changed.
        /// </summary>
        public event EventHandler OnConfigSourceChanged;


        /// <summary>
        /// Get the underlying storage.
        /// </summary>
        public IConfigSource Storage
        {
            get { return _configStore; }
        }


        /// <summary>
        /// The source file path.
        /// </summary>
        public string SourcePath
        {
            get { return _sourcePath; }
        }


        /// <summary>
        /// Application name.
        /// </summary>
        public string AppName { get; set; }


        /// <summary>
        /// Config name
        /// </summary>
        public string ConfigName { get; set; }


        /// <summary>
        /// List of all the props that should be excluded during load/save.
        /// </summary>
        public IDictionary<string, string> ExcludedProps { get; set; }


        /// <summary>
        /// Dictionary of all the props that should be included during load/save.
        /// </summary>
        public IDictionary<string, string> IncludedProps { get; set; }

               
        /// <summary>
        /// On initialize called after construction.
        /// </summary>
        public virtual void SetRepository(Func<IConfigSource> configStoreFactoryMethod, IRepository<ConfigItem> repo)
        {
            IncludedProps = new Dictionary<string, string>();
            ExcludedProps = new Dictionary<string, string>();
            _configStoreFactory = configStoreFactoryMethod;
            _repo = repo;
            _configStore = GetStorage();

            ExcludedProps["AppName"] = "AppName";
            ExcludedProps["ConfigName"] = "ConfigName";
            ExcludedProps["IncludedProps"] = "IncludedProps";
            ExcludedProps["ExcludedProps"] = "ExcludedProps";
            ExcludedProps["SourcePath"] = "SourcePath";
            ExcludedProps["Storage"] = "Storage";
        }


        /// <summary>
        /// On initialize called after construction.
        /// </summary>
        public virtual void Init(string appName, string configName, Func<IConfigSource> configStoreFactoryMethod, IRepository<ConfigItem> repo)
        {
            SetNames(appName, configName);
            SetRepository(configStoreFactoryMethod, repo);
        }


        /// <summary>
        /// Load the settings from the datastore.
        /// </summary>
        public void Load()
        {
            Load(this);
        }


        /// <summary>
        /// Load the settings from the datastore.
        /// </summary>
        public void Load(object config)
        {
            OnBeforeLoad();
            OnLoad(config);
            OnAfterLoad();
        }


        /// <summary>
        /// Save the settings to the datastore.
        /// </summary>
        public void Save()
        {
            Save(this);
        }


        /// <summary>
        /// Save the config settings object provided to the datasource.
        /// </summary>
        /// <param name="config"></param>
        public void Save(object config)
        {
            OnBeforeSave();
            OnSave(config);
            OnAfterSave();
        }


        #region Life-Cycle events
        /// <summary>
        /// Event fired before load.
        /// </summary>
        public virtual void OnBeforeLoad()
        {
        }

        
        /// <summary>
        /// Event fired on load.
        /// </summary>
        /// <param name="config">Config to load.</param>
        public virtual void OnLoad(object config)
        {
            Execute(() =>
            {
                Type settings = config.GetType();
                _configStore.Load();

                foreach (var prop in settings.GetProperties())
                {
                    if (!ExcludedProps.ContainsKey(prop.Name))
                    {
                        // 1. Get value.
                        object val = _configStore["", prop.Name];

                        // 2. Set the value.
                        Reflection.ReflectionUtils.SetProperty(config, prop.Name, val);
                    }
                }
            });
        }


        /// <summary>
        /// Event fired after load.
        /// </summary>
        public virtual void OnAfterLoad()
        {
        }


        /// <summary>
        /// Event fired before save.
        /// </summary>
        public virtual void OnBeforeSave()
        {
        }


        /// <summary>
        /// Event fired on save.
        /// </summary>
        /// <param name="config">Config to use.</param>
        public virtual void OnSave(object config)
        {
            Execute(() =>
            {
                Type settings = config.GetType();
                foreach (var prop in settings.GetProperties())
                {
                    if (!ExcludedProps.ContainsKey(prop.Name))
                    {
                        // 1. Get value.
                        object val = prop.GetValue(config, null);

                        // 2. Convert to string checking for min values. e.g. DateTime.Min,
                        // string valStr = Types.TypeParsers.ConvertTo<string>(val);

                        _configStore["", prop.Name] = val;
                    }
                }
                _configStore.Save();
            });
        }


        /// <summary>
        /// Event fired on save.
        /// </summary>
        public virtual void OnAfterSave()
        {
        }
        #endregion


        /// <summary>
        /// Execute the action in try catch.
        /// </summary>
        /// <param name="action"></param>
        private void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Try.Catch(() => Logging.Logger.Error("Unable to load/save settings", ex));
            }
        }


        private void SetNames(string appName, string configName)
        {
            AppName = appName;
            ConfigName = configName;
        }


        private IConfigSource GetStorage()
        {
            IConfigSource config = null;

            if (_configStoreFactory != null) config = _configStoreFactory();
            else if (_repo != null) config = new ConfigSourceDb(this.AppName, this.ConfigName, _repo, false);
            else if (_isSourcePathFileBased) config = new ComLib.IO.IniDocument(_sourcePath, true);
            else config = new ConfigSourceDb(this.AppName, this.ConfigName, this.SourcePath);

            return config;
        }
    }
}
