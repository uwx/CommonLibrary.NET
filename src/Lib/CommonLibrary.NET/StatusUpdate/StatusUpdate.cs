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

using ComLib.Entities;


namespace ComLib.StatusUpdater
{
    /// <summary>
    /// StatusUpdate entity.
    /// </summary>
    public partial class StatusUpdate : Entity
    {
		/// <summary>
		/// Get/Set Computer
		/// </summary>
		public string Computer { get; set; }


		/// <summary>
		/// Get/Set ExecutionUser
		/// </summary>
		public string ExecutionUser { get; set; }


		/// <summary>
		/// Get/Set BusinessDate
		/// </summary>
		public DateTime BusinessDate { get; set; }


		/// <summary>
		/// Get/Set BatchName
		/// </summary>
		public string BatchName { get; set; }


		/// <summary>
		/// Get/Set BatchId
		/// </summary>
		public int BatchId { get; set; }


		/// <summary>
		/// Get/Set BatchTime
		/// </summary>
		public DateTime BatchTime { get; set; }


		/// <summary>
		/// Get/Set Task
		/// </summary>
		public string Task { get; set; }


		/// <summary>
		/// Get/Set Status
		/// </summary>
		public string Status { get; set; }


		/// <summary>
		/// Get/Set StartTime
		/// </summary>
		public DateTime StartTime { get; set; }


		/// <summary>
		/// Get/Set EndTime
		/// </summary>
		public DateTime EndTime { get; set; }


		/// <summary>
		/// Get/Set Ref
		/// </summary>
		public string Ref { get; set; }


		/// <summary>
		/// Get/Set Comment
		/// </summary>
		public string Comment { get; set; }



    }
}
