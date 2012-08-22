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
using System.Text;

namespace CommonLibrary
{
    public class MapperContext
    {        
        /// <summary>
        /// The object to map.
        /// </summary>
        public object MappedObject;


        /// <summary>
        /// Mappings to include ( properties of the mapping object) to include.
        /// </summary>
        public IDictionary<string, string> MappingsToInclude;


        /// <summary>
        /// Initializes a new instance of the <see cref="MapperContext"/> class.
        /// </summary>
        public MapperContext() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="MapperContext"/> class.
        /// </summary>
        /// <param name="mappedObject">The mapped object.</param>
        /// <param name="namingConvention">The naming convention.</param>
        /// <param name="mappingsToInclude">The mappings to include.</param>
        public MapperContext(object mappedObject, IDictionary<string, string> mappingsToInclude)
        {
            MappedObject = mappedObject;
            MappingsToInclude = mappingsToInclude;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MapperContext"/> class.
        /// </summary>
        /// <param name="mappedObject">The mapped object.</param>
        /// <param name="namingConvention">The naming convention.</param>
        /// <param name="mappingsToInclude">The mappings to include.</param>
        public MapperContext(object mappedObject, string mappingsToInclude)
        {
            MappedObject = mappedObject;
            //MappingsToInclude = CollectionUtils.ConvertDelimitedTextToDictionary(mappingsToInclude, ',');
        }
    }
}
