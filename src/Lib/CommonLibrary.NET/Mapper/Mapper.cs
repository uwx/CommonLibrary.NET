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
using System.Collections;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;

using ComLib.IO;
using ComLib.Extensions;
using ComLib.Reflection;
using ComLib.CsvParse;


namespace ComLib.MapperSupport
{
    /// <summary>
    /// Maps objects from one datasource to the object(s) represented by generic Type. 
    /// Used primarily for :
    /// 1. Converting files, in formats such as xml, csv, ini, containing serialized data representing object to the actual objects.
    /// 2. Converting a dictionary of key/value pairs to objects with the key/values in the dictionary mapping to property/values in the object.
    /// </summary>
    public class Mapper
    {        
        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IList<T> Map<T>(IDictionary data) where T : class, new()
        {
            return Map<T>(data, null);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IList<T> Map<T>(IDictionary data, IErrors errors) where T : class, new()
        {
            var mapper = new MapperIni<T>();
            var results = mapper.Map(data, errors);
            return results;
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path to the file.</param>
        /// <returns></returns>
        public static IList<T> MapConfigFile<T>(string path) where T : class, new()
        {
            string extension = FileHelper.GetOriginalExtension(path, ".config");
            return MapFile<T>(path, extension);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path to the file.</param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static IList<T> MapFile<T>(string path, string extension = null) where T : class, new()
        {
            FileInfo file = new FileInfo(path);
            string fileExtension = string.IsNullOrEmpty(extension) ? file.Extension.ToLower() : extension.ToLower();
            IList<T> result = null;

            switch (fileExtension)
            {
                case ".csv":
                    result = MapCsvFile<T>(path);
                    break;
                case ".ini":
                    result = MapIniFile<T>(path);
                    break;
                case ".xml":
                    result = MapXmlFile<T>(path);
                    break;
                case ".jsn":
                    result = MapJsonFile<T>(path);
                    break;
                case ".yml":
                    result = MapYamlFile<T>(path);
                    break;
            }
            return result;
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static IList<T> MapIniFile<T>(string path) where T : class, new()
        {
            var doc = new IniDocument(path, true);
            return Map<T>(doc);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <param name="isPath">if set to <c>true</c> [is path].</param>
        /// <param name="dataTypeKeyName">Name of the data type key.</param>
        /// <param name="factoryMethod">The factory method.</param>
        /// <returns></returns>
        public static IList<T> MapIniAs<T>(string content, bool isPath, string dataTypeKeyName, Func<string, T> factoryMethod) where T : class, new()
        {
            var doc = new IniDocument(content, isPath);
            var errors = new Errors();
            var mapper = new MapperIni<T>();
            var results = mapper.Map(doc, errors, true, dataTypeKeyName, factoryMethod);
            return results;
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IList<T> MapCsv<T>(string data) where T : class, new()
        {
            CsvDoc doc = new CsvDoc(data, false);
            return MapCsv<T>(doc, null);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static IList<T> MapCsv<T>(CsvDoc  data) where T : class, new()
        {
            return MapCsv<T>(data, null);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IList<T> MapCsv<T>(CsvDoc data, IErrors errors) where T : class, new()
        {
            var mapper = new MapperCsv<T>();
            var results = mapper.Map(data, errors);
            return results;
        }        


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<T> MapCsvFile<T>(string path) where T : class, new()
        {
            var doc = new CsvDoc(path, true);
            return MapCsv<T>(doc);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<T> MapXmlFile<T>(string path) where T : class, new()
        {
            string contents = File.ReadAllText(path);
            return MapXml<T>(contents);
        }


        /// <summary>
        /// Maps the xml string to a list of items of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contents"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static IList<T> MapXml<T>(string contents, IErrors errors = null) where T : class, new()
        {
            var mapper = new MapperXml<T>();
            var doc = new XmlDocument();
            doc.LoadXml(contents);
            var results = mapper.Map(doc, errors);
            return results;
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<T> MapJsonFile<T>(string path) where T : class, new()
        {
            throw new NotSupportedException("Mapping of Json files to <T> is not currently supported.");
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IList<T> MapYamlFile<T>(string path) where T : class, new()
        {
            throw new NotSupportedException("Mapping of Yaml files to <T> is not currently supported.");
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        /// <param name="errors"></param>        
        /// <returns></returns>
        public static void MapToCsv<T>(IList<T> items, string path, IErrors errors) where T : class, new()
        {
            var mapper = new MapperCsv<T>();
            mapper.MapToFile(items, null, path, errors);
        }


        /// <summary>
        /// Map items from the dictionary data to items of Type T.
        /// Each key in the data corresponds to a nested IDictionary which contains key/value
        /// pairs representing the properties/values of the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        /// <param name="errors"></param>
        /// <param name="excludeProps"></param>
        /// <returns></returns>
        public static void MapToCsv<T>(IList<T> items, string path, IErrors errors, List<string> excludeProps) where T : class, new()
        {
            var mapper = new MapperCsv<T>();
            IDictionary<string, string> excludedPropsMap = excludeProps.ToDictionary();
            mapper.MapToFile(items, excludedPropsMap, path, errors);
        }
    }
}
