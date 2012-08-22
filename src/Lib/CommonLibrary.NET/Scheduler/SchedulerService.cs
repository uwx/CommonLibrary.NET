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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Timers;


namespace ComLib.Scheduling
{
    /// <summary>
    /// Light-weight scheduler.
    /// </summary>
    public class SchedulerService : IScheduler
    {
        private object _syncObject = new object();
        private OrderedDictionary _tasks;
        private SchedulerStatus _status = SchedulerStatus.NotStarted;


        /// <summary>
        /// Default initialization.
        /// </summary>
        public SchedulerService()
        {            
            _tasks = new OrderedDictionary();
            _status = SchedulerStatus.Started;
        }


        #region IScheduler Members
        /// <summary>
        /// Schedules the specified task via a delegate.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="trigger">Task trigger.</param>
        /// <param name="start">True to start task.</param>
        /// <param name="executor">Action associated with task</param>
        /// <param name="onCompletedAction">Action to execute when task is completed.</param>
        public void Schedule(string name, Trigger trigger, bool start, Action executor, Action<Task> onCompletedAction)
        {
            Task task = new Task(name, executor, trigger);            
            task.OnTaskCompleted = onCompletedAction;

            lock (_syncObject)
            {
                InternalSchedule(task);
            }
        }


        /// <summary>
        /// Is started.
        /// </summary>
        public bool IsStarted
        {
            get 
            {
                lock (_syncObject)
                {
                    return _status == SchedulerStatus.Started;
                }
            }
        }


        /// <summary>
        /// Is shut down.
        /// </summary>
        public bool IsShutDown
        {
            get 
            {
                lock (_syncObject)
                {
                    return _status == SchedulerStatus.Shutdown;
                }
            }
        }


        /// <summary>
        /// Pauses the task.
        /// </summary>
        /// <param name="name">Name of task to pause.</param>
        public void Pause(string name)
        {
            lock (_syncObject)
            {
                InternalPause(name);
            }
        }


        /// <summary>
        /// Run the task.
        /// </summary>
        /// <param name="name">Name of task to run.</param>
        public void Run(string name)
        {
            lock (_syncObject)
            {
                InternalRun(name);
            }
        }


        /// <summary>
        /// Resumes the task.
        /// </summary>
        /// <param name="name">Name of task to resume.</param>
        public void Resume(string name)
        {
            lock (_syncObject)
            {
                InternalResume(name);
            }
        }


        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="name">Name of task to delete.</param>
        public void Delete(string name)
        {
            lock (_syncObject)
            {
                InternalDelete(name);
            }
        }


        /// <summary>
        /// Gets all the active tasks in the schedule.
        /// BUG: Currently does not return the task name that are associated 
        /// with the group name.
        /// </summary>
        /// <returns>Array with names of active tasks.</returns>
        public string[] GetNames()
        {
            lock (_syncObject)
            {
                ICollection keyCollection = _tasks.Keys;
                string[] keys = new String[_tasks.Count];
                keyCollection.CopyTo(keys, 0);
                return keys;
            }
        }


        /// <summary>
        /// Pause all tasks.
        /// </summary>
        public void PauseAll()
        {
            lock (_syncObject)
            {
                InternalPauseAll();
            }
        }


        /// <summary>
        /// Resume all tasks.
        /// </summary>
        public void ResumeAll()
        {
            lock (_syncObject)
            {
                InternalResumeAll();
            }
        }


        /// <summary>
        /// Shuts down the scheduler.
        /// </summary>
        public void ShutDown()
        {
            lock (_syncObject)
            {
                InternalPauseAll();
                _status = SchedulerStatus.Shutdown;
            }
        }


        /// <summary>
        /// Get the statuses of all tasks.
        /// </summary>
        /// <returns>List with statuses of all tasks.</returns>
        public IList<TaskSummary> GetStatus()
        {
            IList<TaskSummary> summaries = new List<TaskSummary>();

            lock (_syncObject)
            {
                foreach (DictionaryEntry entry in _tasks)
                {
                    var summary = ((Task)entry.Value).GetStatus();
                    summaries.Add(summary);
                }
            }
            return summaries;
        }


        /// <summary>
        /// Returns summary info for a task.
        /// </summary>
        /// <param name="name">Name of task.</param>
        /// <returns>Information of task.</returns>
        public TaskSummary GetStatus(string name)
        {
            TaskSummary summary = null;
            lock (_syncObject)
            {
                var task = _tasks[name] as Task;
                summary = task.GetStatus();
            }
            return summary;
        }
        #endregion


        private void InternalSchedule(Task task)
        {
            _tasks.Add(task.Name, task);
            if (!task.IsOnDemand)
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Interval = task.Trigger.Interval;
                timer.Elapsed += task.OnTimerElapsed;
                task.TimerInfo = timer;
                task.TimerInfo.Start();
            }
        }


        private void InternalPause(string name)
        {
            var task = _tasks[name] as Task;
            if (CanPauseOrResume(task))
            {
                task.TimerInfo.Stop();
            }
        }


        private void InternalResume(string name)
        {
            var task = _tasks[name] as Task;
            if (CanPauseOrResume(task))
            {
                task.TimerInfo.Start();
            }
        }


        private void InternalRun(string name)
        {
            var task = _tasks[name] as Task;
            if (task != null)
            {
                task.Run();
            }
        }


        private void InternalDelete(string name)
        {
            var task = _tasks[name] as Task;
            if (task != null)
            {
                if (!task.IsOnDemand)
                {
                    task.TimerInfo.Stop();
                }
                _tasks.Remove(name);
            }
        }


        private void InternalResumeAll()
        {
            for (int ndx = 0; ndx < _tasks.Count; ndx++)
            {
                Task task = _tasks[ndx] as Task;
                InternalResume(task.Name);
            }
        }


        private void InternalPauseAll()
        {
            for (int ndx = 0; ndx < _tasks.Count; ndx++)
            {
                Task task = _tasks[ndx] as Task;
                InternalPause(task.Name);
            }
        }


        private bool CanPauseOrResume(Task task)
        {
            return task != null && !task.IsOnDemand;
        }
    }
}