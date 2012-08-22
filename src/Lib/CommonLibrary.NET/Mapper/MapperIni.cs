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

using ComLib.IO;
using ComLib.Extensions;
using ComLib.Reflection;

namespace ComLib.MapperSupport
{
    /// <summary>
    /// Mapper for sourcing data from Ini files.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapperIni<T> : IMapper<T> where T : class, new()
    {
        private IDictionary _data;
        private bool _dynamicTypesEnabled;
        private string _dynamicTypeKeyName;
        private Func<string, T> _dynamicTypeFactory;
        

        /// <summary>
        /// Get the supported format of the data source.
        /// </summary>
        public string SupportedFormat { get { return "ini"; } }

        
        /// <summary>
        /// Map the objects.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public IList<T> Map(object source, IErrors errors)
        {
            if (source == null || !(source is IDictionary))
                return new List<T>();

            _data = source as IDictionary;
            return Map(errors);
        }


        /// <summary>
        /// Map the objects.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errors"></param>
        /// <param name="enableDynamicTypes"></param>
        /// <param name="dynamicTypeSectionKeyName"></param>
        /// <param name="dynamicTypeFactory"></param>
        /// <returns></returns>
        public IList<T> Map(object source, IErrors errors, bool enableDynamicTypes, string dynamicTypeSectionKeyName, Func<string, T> dynamicTypeFactory)
        {
            if (source == null || !(source is IDictionary))
                return new List<T>();

            _data = source as IDictionary;
            _dynamicTypesEnabled = enableDynamicTypes;
            _dynamicTypeKeyName = dynamicTypeSectionKeyName;
            _dynamicTypeFactory = dynamicTypeFactory;
            return Map(errors);
        }


        /// <summary>
        /// Map the ini file
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public IList<T> Map(IErrors errors)
        {
            IList<T> items = new List<T>();

            // Check inputs.
            if (_data == null || _data.Count == 0) return items;

            int counter = 0;       
            var propMapDefault = ReflectionUtils.GetPropertiesAsMap<T>(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, false);
            foreach (DictionaryEntry entry in _data)
            {
                // Represents single object.                
                if (entry.Value is IDictionary)
                {
                    Tuple2<T, IDictionary<string, PropertyInfo>> tuple = GetNewObjectAndPropertyMapAsTuple((IDictionary)entry.Value, propMapDefault);
                    MapperHelper.MapTo<T>(tuple.First, counter, tuple.Second, entry.Value as IDictionary);
                    items.Add(tuple.First);
                    counter++;
                }
                // Multiple sections with the same name ( "post", "post" )
                else if (entry.Value is List<object>)
                {
                    List<object> sections = entry.Value as List<object>;
                    foreach (object section in sections)
                    {
                        if (section is IDictionary)
                        {
                            Tuple2<T, IDictionary<string, PropertyInfo>> tuple = GetNewObjectAndPropertyMapAsTuple((IDictionary)section, propMapDefault);
                            MapperHelper.MapTo<T>(tuple.First, counter, tuple.Second, section as IDictionary);
                            items.Add(tuple.First);
                            counter++;
                        }
                    }
                }
            }
            return items;
        }


        /// <summary>
        /// Map objects from the source and convert to list of type T. Collect errors into the IErrors.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public IList<T> MapFromFile(string filepath, IErrors errors)
        {
            IniDocument doc = new IniDocument(filepath, true);
            _data = doc;
            return Map(errors);
        }


        /// <summary>
        /// Map objects from the source and convert to list of type T. Collect errors into the IErrors.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public IList<T> MapFromText(string content, IErrors errors)
        {
            IniDocument doc = new IniDocument(content, false);
            _data = doc;
            return Map(errors);
        }


        /// <summary>
        /// Returns a tuple that contains 1: A new instance of the datatype that an individual section is mapped to,
        /// and 2: A Dictionary of key : PropertyInfo pair values of that datatype.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="propMapDefault"></param>
        /// <returns></returns>
        private Tuple2<T, IDictionary<string, PropertyInfo>> GetNewObjectAndPropertyMapAsTuple(IDictionary section, IDictionary<string, PropertyInfo> propMapDefault)
        {
            T item = null;
            IDictionary<string, PropertyInfo> propMap = null;

            if (_dynamicTypesEnabled)
            {
                var typeName = section.Get<string>(_dynamicTypeKeyName);
                item = _dynamicTypeFactory(typeName);               
                propMap = ReflectionUtils.GetPropertiesAsMap(item.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, false);                
            }
            else
            {
                item = new T();
                propMap = propMapDefault;
            }
            return new Tuple2<T, IDictionary<string, PropertyInfo>>(item, propMap);
        }
    }
}
