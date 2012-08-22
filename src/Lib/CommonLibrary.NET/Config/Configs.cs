using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

using ComLib;
using ComLib.Reflection;
using ComLib.Data;
using ComLib.IO;


namespace ComLib.Configuration
{
    /// <summary>
    /// This class provides methods that can load
    /// a configuration and return a config source.
    /// </summary>
    public class Configs
    {
        private static ConnectionInfo _connection;


        /// <summary>
        /// Initialize db connection for LoadFromDb methods.
        /// </summary>
        /// <param name="connection"></param>
        public static void Init(ConnectionInfo connection)
        {
            _connection = connection;
        }


        /// <summary>
        /// Load config from single file or multiple files.
        /// </summary>
        /// <param name="configs">"prod.config" or multiple paths delimited by command.
        /// e.g. "prod.config, qa.config, dev.config"</param>
        /// <returns></returns>
        public static IConfigSource LoadFiles(string configs)
        {
            return LoadFiles(string.Empty, configs);
        }


        /// <summary>
        /// Load config from single file or multiple files.
        /// </summary>
        /// <param name="configDirectory"></param>
        /// <param name="configs">"prod.config" or multiple paths delimited by command.
        /// e.g. "prod.config, qa.config, dev.config"</param>
        /// <returns></returns>
        public static IConfigSource LoadFiles(string configDirectory, string configs)
        {
            bool hasConfigDir = !string.IsNullOrEmpty(configDirectory);

            // CASE 1 : File(s) do not exist.
            if (string.IsNullOrEmpty(configs)) return new IniDocument();
            
            // CASE 2 : single environment, represented with single configuration file.
            // e.g. "prod", "prod.config".
            if (!configs.Contains(","))
            {
                // Check file.
                if (hasConfigDir) configs = configDirectory + @"\" + configs;
                if (!File.Exists(configs)) return new IniDocument();

                return new IniDocument(configs, configs, true, true);
            }
            // CASE 3 : single environment, represented with multiple configuration file.
            // e.g. "prod", "prod.config, qa.config, dev.config".
            string[] configPaths = configs.Split(',');
            var configSources = new List<IConfigSource>();
            
            configPaths.ForEach(configPath =>
            {
                if (hasConfigDir) configPath = configDirectory + @"\" + configPath;
                if (!File.Exists(configPath)) throw new FileNotFoundException("Config file : " + configPath + " not found.");
                FileInfo configFile = new FileInfo(configPath);

                configSources.Add(new IniDocument(configFile.Name, configPath, true, true));
            });

            IConfigSource inheritedConfig = new ConfigSourceMulti(configSources);
            return inheritedConfig;
        }


        /// <summary>
        /// Load from the database
        /// </summary>
        /// <param name="configNames">Comma delimited names of the configs to load
        /// from the database.</param>
        /// <returns></returns>
        public static IConfigSource LoadDb(string configNames)
        {
            return null;
        }


        /// <summary>
        /// Load config settings into a configSource from an object using
        /// it's public properties.
        /// </summary>
        /// <param name="configObj"></param>
        /// <returns></returns>
        public static IConfigSource LoadObject(object configObj)
        {
            IniDocument doc = new IniDocument();
            PropertyInfo[] props = configObj.GetType().GetProperties(BindingFlags.Public);
            props.ForEach(prop => doc["global", prop.Name] = ReflectionUtils.GetPropertyValueSafely(configObj, prop));
            return doc;
        }


        /// <summary>
        /// Load from string.
        /// </summary>
        /// <param name="configText"></param>
        /// <returns></returns>
        public static IConfigSource LoadString(string configText)
        {
            IConfigSource config = new IniDocument(configText, false);
            return config;
        }
    }
}
