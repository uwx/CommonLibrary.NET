using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ComLib.Models;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// Class to encapsulate a code file.
    /// </summary>
    public class CodeFile
    {
        /// <summary>
        /// Name of file.
        /// </summary>
        public string Name;


        /// <summary>
        /// File information.
        /// </summary>
        public FileInfo File;


        /// <summary>
        /// Qualified (alternate) name for file.
        /// </summary>
        public string QualifiedName;


        /// <summary>
        /// Folder where file resides.
        /// </summary>
        public string Folder;


        /// <summary>
        /// Output folder.
        /// </summary>
        public string OutputFolder;


        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="name">Name of file.</param>
        /// <param name="file">File information.</param>
        /// <param name="folder">Name of folder.</param>
        /// <param name="newName">Qualified name.</param>
        public CodeFile(string name, FileInfo file, string folder, string newName)
        {
            Name = name;
            File = file;
            QualifiedName = string.IsNullOrEmpty(newName) ? name : newName;
            Folder = folder;
        }
    }


    /// <summary>
    /// This class contains utility methods for code generation.
    /// </summary>
    public class CodeBuilderUtils
    {
        /// <summary>
        /// Get the list of files to process.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="inputPattern"></param>
        /// <param name="codeTemplateFolder"></param>
        /// <returns></returns>
        public static Dictionary<string, CodeFile> GetFiles(ModelContext ctx, string inputPattern, string codeTemplateFolder)
        {
            inputPattern = string.IsNullOrEmpty(inputPattern) ? "*.cs" : inputPattern;
            // Get all files in the domain model.           
            string[] patterns = inputPattern.Split(',');
            List<string> allfiles = new List<string>();
            foreach (string pattern in patterns)
            {
                string[] files = Directory.GetFiles(codeTemplateFolder, pattern, SearchOption.AllDirectories);
                if (files != null || files.Length >= 0)
                    allfiles.AddRange(files);
            }
            if (allfiles.Count == 0)
                return null;

            List<FileInfo> fileInfos = new List<FileInfo>();
            Dictionary<string, CodeFile> fileMap = new Dictionary<string, CodeFile>();

            allfiles.ForEach(f => fileInfos.Add(new FileInfo(f)));
            fileInfos.ForEach(f => fileMap[f.Name] = new CodeFile(f.Name, f, GetFolder(f.DirectoryName), string.Empty));
            return fileMap;
        }

        private static string GetFolder(string fullPath)
        {
            int ndxLastDash = fullPath.LastIndexOf(@"\");
            string folder = fullPath.Substring(ndxLastDash + 1);
            return folder;
        }


        /// <summary>
        /// Writes out generated files.
        /// </summary>
        /// <param name="templateFiles">Dictionary of files to generate.</param>
        /// <param name="subs">List of key/value pairs of substitutions.</param>
        /// <param name="codePath"></param>
        public static void WriteFiles(Dictionary<string, CodeFile> templateFiles, List<KeyValuePair<string, string>> subs, string codePath)
        {
            // Make the substitutions in each of the template files.
            foreach (string fileName in templateFiles.Keys)
            {
                CodeFile file = templateFiles[fileName];
                string templateFile = file.File.FullName;
                
                // Contents of template file.
                string fileContent = File.ReadAllText(templateFile);

                // Substitutions.
                subs.ForEach(sub => fileContent = fileContent.Replace(sub.Key, sub.Value));

                // Create generated code directory.
                if (!Directory.Exists(file.OutputFolder)) Directory.CreateDirectory(file.OutputFolder);

                string generatedFile = file.OutputFolder + "\\" + file.QualifiedName;
                
                // Write out the file.
                using(StreamWriter writer = File.CreateText(generatedFile))
                {
                    writer.Write(fileContent);
                    writer.Close();
                }
            }
        }
    }


    /// <summary>
    /// Class to expose mapping between types of different data sources.
    /// </summary>
    public class TypeMap
    {
        /// <summary>
        /// Dictionary with static type mappings.
        /// </summary>
        protected static IDictionary<string, IDictionary<string, object>> _typeMaps = new Dictionary<string, IDictionary<string, object>>();


        /// <summary>
        /// Key to use for getting sqlserver type map for .net types.
        /// </summary>
        public const string SqlServer = "sqlserver";


        /// <summary>
        /// Key to use for getting sqlclient type map.
        /// </summary>
        public const string SqlClient = "sqlclient";


        /// <summary>
        /// .NET format types to csharp short names.
        /// e.g. Int32 to int.
        /// </summary>
        public const string NetFormatToCSharp = "nettocsharp";


        /// <summary>
        /// .NET format types to csharp short names.
        /// e.g. System.Int32 to Type.
        /// </summary>
        public const string NetFormatToCSharpType = "nettocsharpType";
        

        /// <summary>
        /// Initialize basic datatypes.
        /// </summary>
        static TypeMap()
        {
            RegisterDefaultTypeMaps();
        }


        /// <summary>
        /// Register a specific type map.
        /// </summary>
        /// <param name="mapName"></param>
        /// <param name="typeMap"></param>
        public static void RegisterTypeMap(string mapName, IDictionary<string, object> typeMap)
        {            
            _typeMaps[mapName] = typeMap;
        }


        /// <summary>
        /// Registers the default type mappings for :
        /// 1. .NET Types to Sql Server types.
        /// 2. .NET typeof(Type).FullName to short names.
        /// </summary>
        public static void RegisterDefaultTypeMaps()
        {
            // .NET formal names to sql-server types.
            Dictionary<string, object> sqlServerTypes = new Dictionary<string, object>();
            sqlServerTypes[typeof(string).Name] = "nvarchar";
            sqlServerTypes[typeof(ComLib.Models.StringClob).Name] = "ntext";
            sqlServerTypes[typeof(ComLib.Models.Image).Name] = "image";
            sqlServerTypes[typeof(DateTime).Name] = "datetime";
            sqlServerTypes[typeof(double).Name] = "decimal";
            sqlServerTypes[typeof(int).Name] = "int";
            sqlServerTypes[typeof(sbyte).Name] = "smallint";
            sqlServerTypes[typeof(long).Name] = "bigint";
            sqlServerTypes[typeof(float).Name] = "float";
            sqlServerTypes[typeof(bool).Name] = "bit";
            
            _typeMaps[SqlServer] = sqlServerTypes;

            Dictionary<string, object> sqlClientTypes = new Dictionary<string, object>();
            sqlClientTypes[typeof(string).Name] = System.Data.SqlDbType.NVarChar;
            sqlClientTypes[typeof(ComLib.Models.StringClob).Name] = System.Data.SqlDbType.NText;
            sqlClientTypes[typeof(ComLib.Models.Image).Name] = System.Data.SqlDbType.Image;
            sqlClientTypes[typeof(DateTime).Name] = System.Data.SqlDbType.DateTime; 
            sqlClientTypes[typeof(double).Name] = System.Data.SqlDbType.Decimal;
            sqlClientTypes[typeof(int).Name] = System.Data.SqlDbType.Int;
            sqlClientTypes[typeof(sbyte).Name] = System.Data.SqlDbType.SmallInt;
            sqlClientTypes[typeof(long).Name] = System.Data.SqlDbType.BigInt;
            sqlClientTypes[typeof(float).Name] = System.Data.SqlDbType.Float;
            sqlClientTypes[typeof(bool).Name] = System.Data.SqlDbType.Bit;
            _typeMaps[SqlClient] = sqlClientTypes;

            // .NET formal names to c# short names.
            Dictionary<string, object> netTypes = new Dictionary<string, object>();
            netTypes.Add(typeof(string).Name, "string");
            netTypes.Add(typeof(sbyte).Name, "sbyte");
            netTypes.Add(typeof(int).Name, "int");
            netTypes.Add(typeof(long).Name, "long");
            netTypes.Add(typeof(bool).Name, "bool");
            netTypes.Add(typeof(float).Name, "float");
            netTypes.Add(typeof(double).Name, "double");
            netTypes.Add(typeof(DateTime).Name, "DateTime");
            netTypes.Add(typeof(ComLib.Models.StringClob).Name, "string");
            netTypes.Add(typeof(ComLib.Models.Image).Name, "byte[]");
            _typeMaps[NetFormatToCSharp] = netTypes;

            // .NET formal full names to Types.
            Dictionary<string, object> netFullNamesToTypes = new Dictionary<string, object>();
            netFullNamesToTypes.Add(typeof(string).FullName, typeof(string));
            netFullNamesToTypes.Add(typeof(int).FullName, typeof(int));
            netFullNamesToTypes.Add(typeof(sbyte).FullName, typeof(sbyte));
            netFullNamesToTypes.Add(typeof(long).FullName, typeof(long));
            netFullNamesToTypes.Add(typeof(bool).FullName, typeof(bool));
            netFullNamesToTypes.Add(typeof(double).FullName, typeof(double));
            netFullNamesToTypes.Add(typeof(DateTime).FullName, typeof(DateTime));
            netFullNamesToTypes.Add(typeof(ComLib.Models.StringClob).FullName, typeof(ComLib.Models.StringClob));
            netFullNamesToTypes.Add(typeof(ComLib.Models.Image).FullName, typeof(ComLib.Models.Image));
            _typeMaps[NetFormatToCSharpType] = netFullNamesToTypes;
        }


        /// <summary>
        /// Get a mapping and cast to specific type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string source, string key)
        {
            object obj = _typeMaps[source][key];
            return (T)obj;
        }


        /// <summary>
        /// Return true of false if the specified key is the typemap associated with source.
        /// </summary>
        /// <param name="source">"sqlserver"</param>
        /// <param name="key">"Int32"</param>
        /// <returns></returns>
        public static bool ContainsKey(string source, string key)
        {
            return _typeMaps[source].ContainsKey(key);
        }


        /// <summary>
        /// Is the type a basic datatype( int, bool, long, double, datetime, string )?
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static bool IsBasicNetType(Type dataType)
        {
            return TypeMap.ContainsKey(TypeMap.NetFormatToCSharp, dataType.Name);
        }

    }
}
