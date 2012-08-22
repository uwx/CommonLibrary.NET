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
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;


namespace ComLib.Environments
{    

    /// <summary>
    /// Different types of environment.
    /// </summary>
    public enum EnvType
    {
        /// <summary>
        /// Development.
        /// </summary>
        Dev,


        /// <summary>
        /// Quality assurance.
        /// </summary>
        Qa,


        /// <summary>
        /// User acceptance testing.
        /// </summary>
        Uat,


        /// <summary>
        /// Production.
        /// </summary>
        Prod,


        /// <summary>
        /// Mixed environments with no envrionment being production.
        /// </summary>
        MixedNonProd,


        /// <summary>
        /// Mixed environments with at least one
        /// environment being production.
        /// </summary>
        MixedProd,


        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown
    }
}
