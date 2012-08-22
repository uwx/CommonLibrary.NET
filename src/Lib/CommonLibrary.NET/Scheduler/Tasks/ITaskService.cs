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
using ComLib.Scheduling;

namespace ComLib.Scheduling.Tasks
{
    /// <summary>
    /// Interface to be implemented by a task control service.
    /// </summary>
    public interface ITaskService : IExtensionService<TaskAttribute, ITask>
    {
        /// <summary>
        /// Returns status information for all tasks.
        /// </summary>
        /// <returns>List with status information for all tasks.</returns>
        IList<TaskSummary> GetStatuses();


        /// <summary>
        /// Executes all tasks.
        /// </summary>
        void RunAll();
    }
}
