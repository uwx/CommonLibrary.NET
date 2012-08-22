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
using System.Timers;


namespace ComLib.Scheduling
{
    /// <summary>
    /// Class representing the task to execute, it can either
    /// be a class that derives from ITask or a delegate.
    /// </summary>
    public class Task
    {
        private int _callCount = 0;
        private DateTime _lastCallTime = DateTime.MinValue;
        private TimeSpan _lastCallElapsedTime = TimeSpan.MinValue;
        
        /// <summary>
        /// The name of the task.
        /// </summary>
        public string Name;


        /// <summary>
        /// The action to execute.
        /// </summary>
        public Action OnTaskExecute;

        // Should make this Asynchronous ?

        /// <summary>
        /// Action to be called when a task is completed.
        /// </summary>
        public Action<Task> OnTaskCompleted;


        /// <summary>
        /// Trigger for this task.
        /// </summary>
        public Trigger Trigger;


        /// <summary>
        /// Timer information for this task.
        /// </summary>
        public Timer TimerInfo;


        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="executor">The executor.</param>
        /// <param name="trigger">The trigger.</param>
        public Task(string name, Action executor, Trigger trigger)
        {
            Name = name;
            Trigger = trigger;
            OnTaskExecute = executor;
        }


        /// <summary>
        /// Callback from timer.
        /// </summary>
        /// <param name="sender">Object raising the event.</param>
        /// <param name="e">Event arguments.</param>
        public void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Disable based on call iterations            
            if (Trigger.HasMaxIterations && _callCount >= Trigger.MaxIterations)
            {
                Stop();
                return;
            }
            // Disable based on end time passed.
            else if( Trigger.EndTime != DateTime.MinValue && e.SignalTime.TimeOfDay >= Trigger.EndTime.TimeOfDay)
            {
                Stop();
                return;
            }
            Run();
        }


        /// <summary>
        /// Run the task.
        /// </summary>
        public void Run()
        {
            OnTaskExecute();
            UpdateSummary();
        }


        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <returns>Task status information.</returns>
        public TaskSummary GetStatus()
        {
            bool isEnabled = IsOnDemand ? true : TimerInfo.Enabled;
            return new TaskSummary(Name, _lastCallTime, _callCount, _lastCallElapsedTime, isEnabled);
        }


        /// <summary>
        /// Whether or not this is an on demand task.
        /// </summary>
        /// <returns>True if this is an on demand task.</returns>
        public bool IsOnDemand
        {
            get { return this.Trigger.IsOnDemand; }
        }


        /// <summary>
        /// Stop this timer and send notification that this task is complete.
        /// </summary>
        private void Stop()
        {
            Try.Catch(() => TimerInfo.Stop());
            if (OnTaskCompleted != null)
            {
                Task copy = Copy(this);
                OnTaskCompleted(copy);
            }
        }


        /// <summary>
        /// Updates the summary.
        /// </summary>
        private void UpdateSummary()
        {
            System.Threading.Interlocked.Increment(ref _callCount);
            DateTime now = DateTime.Now;
            if (_lastCallTime != DateTime.MinValue)
            {
                _lastCallElapsedTime = now.TimeOfDay - _lastCallTime.TimeOfDay;
            }
            _lastCallTime = now;

        }


        /// <summary>
        /// Copies the specified task.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns>A new copy of this instance.</returns>
        public static Task Copy(Task task)
        {
            Task copy = new Task(task.Name, task.OnTaskExecute, task.Trigger.Clone() as Trigger);
            return copy;
        }
    }



    /// <summary>
    /// Summary info for a task which indicates last run time etc.
    /// </summary>
    public class TaskSummary
    {
        /// <summary>
        /// The name of the queue.
        /// </summary>
        public string Name;


        /// <summary>
        /// Group
        /// </summary>
        public string Group;


        /// <summary>
        /// Description of task.
        /// </summary>
        public string Description;


        /// <summary>
        /// The last time the queue was processed.
        /// </summary>
        public readonly DateTime LastProcessDate;


        /// <summary>
        /// Number of times the queue has been processed.
        /// </summary>
        public readonly int NumberOfTimesProcessed;


        /// <summary>
        /// Amount of time since the last process date.
        /// </summary>
        public readonly TimeSpan ElapsedTimeSinceLastProcessDate;


        /// <summary>
        /// Whether or nor the task is still active to run.
        /// </summary>
        public readonly bool IsActive;


        /// <summary>
        /// INitialize new task summary.
        /// </summary>
        /// <param name="name">Name of task.</param>
        /// <param name="lastProcessDate">Last datetime it was processed.</param>
        /// <param name="numberOfTimesProcessed">Number of times it was processed.</param>
        /// <param name="elaspedTime">Time elapsed between tasks</param>
        /// <param name="isEnabled">Whether or not the task is enabled.</param>
        public TaskSummary(string name, DateTime lastProcessDate, int numberOfTimesProcessed, TimeSpan elaspedTime, bool isEnabled)
        {
            Name = name;
            NumberOfTimesProcessed = numberOfTimesProcessed;
            LastProcessDate = lastProcessDate;
            ElapsedTimeSinceLastProcessDate = elaspedTime;
            IsActive = isEnabled;
        }
    } 
}
