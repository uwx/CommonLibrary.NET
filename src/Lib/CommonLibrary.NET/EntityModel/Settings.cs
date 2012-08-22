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

using ComLib.Configuration;
using ComLib.IO;


namespace ComLib.Entities
{
    /// <summary>
    /// This settings class uses the configSourceDecorator to allow the config settings
    /// to be either file based or database based.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntitySettings<T> : ConfigSourceDynamic, IEntitySettings
    {
        /// <summary>
        /// Creates a new instance of this class with validation enabled.
        /// </summary>
        public EntitySettings()
        {
            EnableValidation = true;
        }


        /// <summary>
        /// Default construction.
        /// </summary>
        public void SetCoreFlags(bool enableAuthentication, bool enableValidation, string editRoles)
        {
            EnableAuthentication = enableAuthentication;
            EnableValidation = enableValidation;
            EditRoles = editRoles;
        }


        /// <summary>
        /// Whether authentication is required to edit the entity.
        /// </summary>
        public bool EnableAuthentication { get; set; }


        /// <summary>
        /// Whether or not to validate the entity.
        /// </summary>
        public bool EnableValidation { get; set; }


        /// <summary>
        /// Roles allowed to edit the entity.
        /// </summary>
        public string EditRoles { get; set; }
    }
}
