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
using System.Reflection;

namespace ComLib
{
    
    /// <summary>
    /// Extension Service for dynamically loading types from an assembly(s).
    /// </summary>
    /// <typeparam name="TAttrib">Type of discovery attribute associated with this instance.</typeparam>
    /// <typeparam name="TExtension">Type of extension associated with this instance.</typeparam>
    public class ExtensionService<TAttrib, TExtension> where TAttrib : ExtensionAttribute
    {
        /// <summary>
        /// Assembly that should be searched for extensions.
        /// </summary>
        protected string _defaultAssemblyForExtensions = "CommonLibrary.Web.Modules";
        
        
        /// <summary>
        /// Lookup by the name to the extensions.
        /// </summary>
        protected IDictionary<string, ExtensionMetaData> _lookup = new DictionaryOrdered<string, ExtensionMetaData>();
        
        
        /// <summary>
        /// Callback method for when a single extension has been found in the dll and before it gets registered
        /// in the service.
        /// </summary>
        protected Action<KeyValuePair<Type, TAttrib>> _onLoadCallback;


        /// <summary>
        /// Callback method for when all the extensions have been loaded into the service.
        /// </summary>
        protected Action _onLoadCompleteCallback;


        /// <summary>
        /// Lookup all the availably dynamically loaded extensions
        /// </summary>
        public IDictionary<string, ExtensionMetaData> Lookup { get { return _lookup; } }


        /// <summary>
        /// Load all the information tasks available.
        /// </summary>
        /// <param name="assembliesDelimited">List of assemblies to look into.</param>
        public virtual void Load(string assembliesDelimited)
        {
            if (string.IsNullOrEmpty(assembliesDelimited))
                assembliesDelimited = _defaultAssemblyForExtensions;
            var assemblies = assembliesDelimited.Split(',');
            foreach (var assemblyname in assemblies)
            {
                // This dynamically gets all the classes that have the InfoAttribute on them,
                // which means they return some information.
                var extensions = Attributes.GetClassAttributesFromAssembly<TAttrib>(assemblyname, (pair) =>
                {
                    pair.Value.DeclaringType = pair.Key.FullName;
                    pair.Value.DeclaringDataType = pair.Key;
                    pair.Value.DeclaringAssembly = assemblyname;
                    if (_onLoadCallback != null)
                    {
                        _onLoadCallback(pair);
                    }
                    Register(pair.Value);
                });
            }
            if (_onLoadCompleteCallback != null)
                _onLoadCompleteCallback();
        }


        /// <summary>
        /// Create the instance of the extension.
        /// </summary>
        /// <param name="name">Name of the extension.</param>
        /// <returns>Instance of the extension.</returns>
        public virtual TExtension Create(string name)
        {
            var metadata = Lookup[name];
            
            // Can not handle lamda's only extensions implemented as classes.
            if (metadata.Lamda != null)
                throw new InvalidOperationException("Can not create instance of extension since it's registered as a lamda");

            Assembly assembly = null;
            Type type = null;
            if (metadata.Attribute.IsReusable)
            {
                if (metadata.Instance == null)
                {
                    assembly = Assembly.Load(metadata.Attribute.DeclaringAssembly);
                    type = assembly.GetType(metadata.Attribute.DeclaringType);
                    metadata.Instance = Activator.CreateInstance(type);
                }
                return (TExtension)metadata.Instance;
            }

            assembly = Assembly.Load(metadata.Attribute.DeclaringAssembly);
            type = assembly.GetType(metadata.Attribute.DeclaringType);
            var instance = Activator.CreateInstance(type);
            return (TExtension)instance;
        }

        /// <summary>
        /// Register the attribute.
        /// </summary>
        /// <param name="type">The type representing the extension</param>
        public virtual void Register(Type type)
        {
            // Register system supplied commands.
            var attrib = type.GetCustomAttributes(typeof(TAttrib), false)[0] as TAttrib;
            attrib.DeclaringType = type.FullName;
            attrib.DeclaringDataType = type;
            attrib.DeclaringAssembly = type.Assembly.FullName;
            Register(attrib);
        }


        /// <summary>
        /// Register the attribute.
        /// </summary>
        /// <param name="attrib">Attribute to register.</param>
        public virtual void Register(TAttrib attrib)
        {
            var metadata = new ExtensionMetaData
            {
                Id = attrib.Name,
                Attribute = attrib,
                Instance = null,
                DataType = attrib.DeclaringDataType
            };
            Register(attrib.Name, metadata);
        }


        /// <summary>
        /// Register the id.
        /// </summary>
        /// <param name="id">Extension id.</param>
        /// <param name="metadata">Extension metadata.</param>
        public virtual void Register(string id, ExtensionMetaData metadata)
        {
            Lookup[id] = metadata;
        }


        /// <summary>
        /// Register the attributes.
        /// </summary>
        /// <param name="attributes">List of attributes to register.</param>
        public virtual void Register(IList<TAttrib> attributes)
        {
            foreach (var attrib in attributes)
                Register(attrib);
        }


        /// <summary>
        /// Register the attributes.
        /// </summary>
        /// <param name="pairs">List of attributes to register.</param>
        public virtual void Register(IList<KeyValuePair<Type, TAttrib>> pairs)
        {
            foreach (var pair in pairs)
                Register(pair.Value);
        }


        /// <summary>
        /// Gets a list of all the extensions and their descriptions.
        /// </summary>
        /// <returns></returns>
        public string GetList(string template = null, Func<ExtensionAttribute, bool> filter = null)
        {
            var buffer = new StringBuilder();
            if(string.IsNullOrEmpty(template))
                template = "{0} : {1}" + Environment.NewLine;

            foreach (var pair in _lookup)
            {
                if(filter == null || filter(pair.Value.Attribute))
                {                     
                    var result = string.Format(template, pair.Value.Attribute.Name, pair.Value.Attribute.Description);
                    buffer.Append(result);
                }
            }
            return buffer.ToString();
        }


        /// <summary>
        /// Loads additional attributes.
        /// </summary>
        /// <typeparam name="TAttribAdditional"></typeparam>
        protected void LoadAdditionalAttributes<TAttribAdditional>()
        {
            foreach (var metaInfo in this._lookup)
            {
                var atts = metaInfo.Value.DataType.GetCustomAttributes(typeof(TAttribAdditional), false);
                foreach (var att in atts)
                {
                    if (att is TAttribAdditional)
                    {
                        metaInfo.Value.AdditionalAttributes.Add(att);
                    }
                }
            }
        }
    }
}
