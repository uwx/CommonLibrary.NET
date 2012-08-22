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
using System.Reflection;

using ComLib;
using ComLib.Extensions;
using ComLib.Reflection;
using ComLib.Scheduling;


namespace ComLib.Scheduling.Tasks
{
    
    /// <summary>
    /// Tasks Service
    /// </summary>
    public class TaskService : ExtensionService<TaskAttribute, ITask>, ITaskService
    {
        /// <summary>
        /// Gets the statuses of all the tasks in the system.
        /// </summary>
        /// <returns>List with statuses for all tasks.</returns>
        public IList<TaskSummary> GetStatuses()
        {
            var summaries = Scheduler.GetStatuses();
            foreach (var summary in summaries)
            {
                if (_lookup.ContainsKey(summary.Name))
                {
                    var attrib = _lookup[summary.Name].Attribute as TaskAttribute;                
                    summary.Group = attrib.Group;
                    summary.Description = attrib.Description;
                }
            }
            return summaries;
        }


        /// <summary>
        /// Runs all the extension tasks.
        /// </summary>
        public void RunAll()
        {
            foreach (var entry in this._lookup)
            {
                var metadata = entry.Value.Attribute as TaskAttribute;
                var instance = Create(metadata.Name);
                var trigger = new Trigger()
                {
                    StartTime = metadata.StartTime, 
                    MaxIterations = metadata.MaxIterations,
                    Interval = (int)metadata.Interval.Seconds().TotalMilliseconds,
                    Repeat = metadata.Repeat,
                    EndTime = metadata.EndTime,
                    IsOnDemand = metadata.IsOnDemand
                };
                if (metadata.IsOnDemand)
                    trigger.OnDemand();
                else if (metadata.MaxIterations > 0)
                    trigger.MaxRuns(metadata.MaxIterations);
                
                Scheduler.Schedule(metadata.Name, trigger, true, () => instance.Process());
            }
        }
    }
}
