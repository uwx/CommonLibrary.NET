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



namespace ComLib.Scheduling.Tasks
{  
    /// <summary>
    /// Attribute used to define a widget.
    /// </summary>
    public class TaskAttribute : ExtensionAttribute
    {
        /// <summary>
        /// The start time of the task
        /// </summary>
        public DateTime StartTime { get; set; }


        /// <summary>
        /// End time of the task
        /// </summary>
        public DateTime EndTime { get; set; }


        /// <summary>
        /// Interval in seconds of the task
        /// </summary>
        public int Interval { get; set; }


        /// <summary>
        /// Whether or not this is on-demand.
        /// </summary>
        public bool IsOnDemand { get; set; }


        /// <summary>
        /// Maximum number of times this can run.
        /// </summary>
        public int MaxIterations { get; set; }


        /// <summary>
        /// Whether or not this can repeat.
        /// </summary>
        public bool Repeat { get; set; }
    }
}
