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

namespace ComLib
{
    /// <summary>
    /// Interface that must be implemented by an attribute class
    /// that will be used to designate an dynamically loadable extension.
    /// </summary>
    public interface IExtensionAttribute
    {
        /// <summary>
        /// Name of the widget.
        /// </summary>
        string Name { get; set; }


        /// <summary>
        /// Group
        /// </summary>
        string Group { get; set; }


        /// <summary>
        /// Name to display ( friendly name )
        /// </summary>
        string DisplayName { get; set; }


        /// <summary>
        /// Description for the widget
        /// </summary>
        string Description { get; set; }


        /// <summary>
        /// Author of the widget.
        /// </summary>
        string Author { get; set; }


        /// <summary>
        /// Email of the author.
        /// </summary>
        string Email { get; set; }


        /// <summary>
        /// Url for reference / documentation.
        /// </summary>
        string Url { get; set; }


        /// <summary>
        /// Version of the widget.
        /// </summary>
        string Version { get; set; }


        /// <summary>
        /// Used to sort model in list of models.
        /// </summary>
        int SortIndex { get; set; }


        /// <summary>
        /// The name of the class representing the widget.
        /// </summary>
        string DeclaringType { get; set; }


        /// <summary>
        /// The name of the class representing the widget.
        /// </summary>
        Type DeclaringDataType { get; set; }


        /// <summary>
        /// The name of the assembly containing the widget.
        /// </summary>
        string DeclaringAssembly { get; set; }
    }


    /// <summary>
    /// This is a default attribute class, used to decorate implementation
    /// of dynamically loadable extension classes.
    /// </summary>
    public class ExtensionAttribute : Attribute, IExtensionAttribute
    {
        /// <summary>
        /// Name of the widget.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Group
        /// </summary>
        public string Group { get; set; }


        /// <summary>
        /// Name to display ( friendly name )
        /// </summary>
        public string DisplayName { get; set; }


        /// <summary>
        /// Description for the widget
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Author of the widget.
        /// </summary>
        public string Author { get; set; }


        /// <summary>
        /// Email of the author.
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Url for reference / documentation.
        /// </summary>
        public string Url { get; set; }
        
        
        /// <summary>
        /// Version of the widget.
        /// </summary>
        public string Version { get; set; }


        /// <summary>
        /// Used to sort model in list of models.
        /// </summary>
        public int SortIndex { get; set; }


        /// <summary>
        /// The name of the class representing the widget.
        /// </summary>
        public string DeclaringType { get; set; }


        /// <summary>
        /// The name of the class representing the widget.
        /// </summary>
        public Type DeclaringDataType { get; set; }


        /// <summary>
        /// The name of the assembly containing the widget.
        /// </summary>
        public string DeclaringAssembly { get; set; }

        
        /// <summary>
        /// Roles that can access this information.
        /// </summary>
        public string Roles { get; set; }


        /// <summary>
        /// Whether or not this extension is reusable.
        /// </summary>
        public bool IsReusable { get; set; }
    }
}
