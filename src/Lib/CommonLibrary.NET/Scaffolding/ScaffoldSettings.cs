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

namespace ComLib.Scaffolding
{
    public class ScaffoldSettings
    {
        /// <summary>
        /// TO_DO: This feature is not yet implemented.
        /// Flag indicating whether or not to display all the properties of 
        /// an entity in each entity "record" in the UI.
        /// </summary>
        public bool DisplayAllEntityProperies = true;


        /// <summary>
        /// TO_DO: This feature is not yet implemented.
        /// Flag indicating whether or not to generate "delete, edit" links next 
        /// to each entity "record" in the UI.
        /// </summary>
        public bool EnableEditDeleteLinksInRow = false;
    }
}
