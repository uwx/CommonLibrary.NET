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
using ComLib.Data;


namespace ComLib.Models
{
    /// <summary>
    /// This class provides settings for the code generation model.
    /// </summary>
    public class ModelBuilderSettings
    {
        /// <summary>
        /// Database connectio used to create the tables associated with a model.
        /// </summary>
        public ConnectionInfo Connection { get; set; }


        /// <summary>
        /// Assembly name.
        /// </summary>
        public string AssemblyName { get; set; }


        /// <summary>
        /// Location of the generated code.
        /// </summary>
        public string ModelCodeLocation { get; set; }


        /// <summary>
        /// Location of the templates for code generation
        /// </summary>
        public string ModelCodeLocationTemplate { get; set; }


        /// <summary>
        /// Location where the sql schema files are created.
        /// </summary>
        public string ModelInstallLocation { get; set; }


        /// <summary>
        /// Location where the sql schema files are created.
        /// </summary>
        public string ModelCodeLocationUI { get; set; }


        /// <summary>
        /// Location where the UI templates are located.
        /// </summary>
        public string ModelCodeLocationUITemplate { get; set; }


        /// <summary>
        /// Location where orm mapping file should be created.
        /// </summary>
        public string ModelOrmLocation { get; set; }


        /// <summary>
        /// Location of the stored procedure templates.
        /// </summary>
        public string ModelDbStoredProcTemplates { get; set; }


        /// <summary>
        /// Location where orm mapping file should be created.
        /// </summary>
        public DbCreateType DbAction_Create { get; set; }
    }
}
