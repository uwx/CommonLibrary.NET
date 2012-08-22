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

namespace ComLib.Scheduling
{
    /// <summary>
    /// Runs the task
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Gets whether the scheduler is started.
        /// </summary>
        bool IsStarted { get; }


        /// <summary>
        /// Gets whether the scheduler is stopped.
        /// </summary>
        bool IsShutDown { get; }


        /// <summary>
        /// Schedules a task.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="trigger">Task trigger.</param>
        /// <param name="start">True to start task.</param>
        /// <param name="executor">Task executor.</param>
        /// <param name="onCompletedAction">Action to call when task completes.</param>
        void Schedule(string name, Trigger trigger, bool start, Action executor, Action<Task> onCompletedAction);


        /// <summary>
        /// Returns all active tasks.
        /// </summary>
        /// <returns></returns>
        string[] GetNames();


        /// <summary>
        /// Pauses a specific task.
        /// </summary>
        /// <param name="name">Task name.</param>
        void Pause(string name);


        /// <summary>
        /// Runs a specific task.
        /// </summary>
        /// <param name="name">Task name.</param>
        void Run(string name);


        /// <summary>
        /// Resumes a specific task.
        /// </summary>
        /// <param name="name">Task name.</param>
        void Resume(string name);


        /// <summary>
        /// Deletes a specific task.
        /// </summary>
        /// <param name="name">Task name.</param>
        void Delete(string name);


        /// <summary>
        /// Pauses all tasks.
        /// </summary>
        void PauseAll();


        /// <summary>
        /// Resumes all tasks.
        /// </summary>
        void ResumeAll();


        /// <summary>
        /// return a list of all the statuses of all tasks.
        /// </summary>
        /// <returns>List with statuses of all tasks.</returns>
        IList<TaskSummary> GetStatus();


        /// <summary>
        /// Get the status of the task specified by name.
        /// </summary>
        /// <param name="name">Name of task.</param>
        /// <returns>Task status.</returns>
        TaskSummary GetStatus(string name);


        /// <summary>
        /// Shuts down the scheduler.
        /// </summary>
        void ShutDown();
    }


    /// <summary>
    /// Status of the task scheduler.
    /// </summary>
    public enum SchedulerStatus { 
        /// <summary>
        /// Scheduler is started.
        /// </summary>
        Started, 
        

        /// <summary>
        /// Scheduler stopped.
        /// </summary>
        Shutdown, 
        
        
        /// <summary>
        /// Scheduler not started.
        /// </summary>
        NotStarted };
}
