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
using ComLib;

using ComLib.Entities;


namespace ComLib.NamedQueries
{
    /// <summary>
    /// NamedQuery entity.
    /// </summary>
    public partial class NamedQuery : ActiveRecordBaseEntity<NamedQuery>
    {
        /// <summary>
        /// Returns a new instance of this class.
        /// </summary>
        /// <returns>Instance of NamedQuery.</returns>
        public static NamedQuery New()
        {
            return new NamedQuery();
        }


		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set Sql
		/// </summary>
		public string Sql { get; set; }


		/// <summary>
		/// Get/Set Parameters
		/// </summary>
		public string Parameters { get; set; }


		/// <summary>
		/// Get/Set IsStoredProcedure
		/// </summary>
		public bool IsStoredProcedure { get; set; }


		/// <summary>
		/// Get/Set IsPagingSuppored
		/// </summary>
		public bool IsPagingSuppored { get; set; }


		/// <summary>
		/// Get/Set IsScalar
		/// </summary>
		public bool IsScalar { get; set; }


		/// <summary>
		/// Get/Set OrderId
		/// </summary>
		public int OrderId { get; set; }


		/// <summary>
		/// Get/Set ItemType
		/// </summary>
		public string ItemType { get; set; }


		/// <summary>
		/// Get/Set Roles
		/// </summary>
		public string Roles { get; set; }
    }
}
