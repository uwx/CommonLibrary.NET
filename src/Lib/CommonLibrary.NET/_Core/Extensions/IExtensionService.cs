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
using ComLib.Collections;
using ComLib.Configuration;
using ComLib.Reflection;

namespace ComLib
{
    
    /// <summary>
    /// Interface to be implemented by an extension Service for dynamically loading types from an assembly(s).
    /// <typeparam name="TAttrib">Type of discovery attribute associated with this instance.</typeparam>
    /// <typeparam name="TExtension">Type of extension associated with this instance.</typeparam>
    /// </summary>
    public interface IExtensionService<TAttrib, TExtension> where TAttrib : ExtensionAttribute
    {
        /// <summary>
        /// Lookup all the availably dynamically loaded extensions
        /// </summary>
        IDictionary<string, ExtensionMetaData> Lookup { get; }


        /// <summary>
        /// Load all the information tasks available.
        /// </summary>
        /// <param name="assembliesDelimited">List of assemblies to look into.</param>
        void Load(string assembliesDelimited);

        /// <summary>
        /// Create the instance of the extension.
        /// </summary>
        /// <param name="name">Name of the extension.</param>
        /// <returns>Instance of the extension.</returns>
        TExtension Create(string name);


        /// <summary>
        /// Register the attribute.
        /// </summary>
        /// <param name="attrib">Attribute to register.</param>
        void Register(TAttrib attrib);


        /// <summary>
        /// Register the id.
        /// </summary>
        /// <param name="id">Extension id.</param>
        /// <param name="metadata">Extension metadata.</param>
        void Register(string id, ExtensionMetaData metadata);


        /// <summary>
        /// Register the attributes.
        /// </summary>
        /// <param name="attributes">List of attributes to register.</param>
        void Register(IList<TAttrib> attributes);


        /// <summary>
        /// Register the attributes.
        /// </summary>
        /// <param name="pairs">List of attributes to register.</param>
        void Register(IList<KeyValuePair<Type, TAttrib>> pairs);
    }
}
