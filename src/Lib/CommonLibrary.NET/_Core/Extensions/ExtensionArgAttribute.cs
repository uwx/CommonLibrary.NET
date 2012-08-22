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
    /// This is a default attribute class, used to decorate implementation
    /// of dynamically loadable extension classes.
    /// </summary>
    public class ExtensionArgAttribute : Attribute
    {
        /// <summary>
        /// Name of the arg
        /// </summary>
        public string Name;


        /// <summary>
        /// Describes the argument name.
        /// </summary>
        public string Description = string.Empty;


        /// <summary>
        /// Default value.
        /// </summary>
        public object DefaultValue = null;


        /// <summary>
        /// Date type of the argument.
        /// </summary>
        public Type DataType;


        /// <summary>
        /// Indicates if is required.
        /// </summary>
        public bool IsRequired;


        /// <summary>
        /// Order number for the argument.
        /// </summary>
        public int OrderNum = 0;


        /// <summary>
        /// Example value.
        /// </summary>
        public string Example = string.Empty;


        /// <summary>
        /// Example of mutliple various values.
        /// </summary>
        public string ExampleMultiple = string.Empty;


        /// <summary>
        /// Used to group various arguments.
        /// e.g. The tag can be used to separate base/derived argument defintions.
        /// </summary>
        public string Tag = string.Empty;
    }
}
