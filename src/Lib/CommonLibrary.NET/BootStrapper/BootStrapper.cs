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

namespace ComLib.BootStrapSupport
{
    /// <summary>
    /// BootStrapper for application startup.
    /// </summary>
    public class BootStrapper
    {
        private List<Task> _startupTasks = new List<Task>();
        private List<Task> _shutdownTasks = new List<Task>();
        private IErrors StartupErrors = new Errors();
        private IErrors ShutdownErrors = new Errors();


        /// <summary>
        /// Applies default values.
        /// </summary>
        public BootStrapper()
        {
            PropagateException = true;
        }


        /// <summary>
        /// Whether or not to log all errors to the default logger.
        /// </summary>
        public bool LogErrors { get; set; }


        /// <summary>
        /// A lamda to call on errors.
        /// </summary>
        public Action<Exception> Logger { get; set; }


        /// <summary>
        /// If an exception occurrs in one of the tasks and that should prevent continuing,
        /// this indicates whether or catch the exception or propagate it up.
        /// </summary>
        public bool PropagateException { get; set; }


        /// <summary>
        /// Useful for derived classes that extend bootstrapper.
        /// use this method for adding startup and shutdown tasks.
        /// </summary>
        public virtual void Configure()
        {
        }


        /// <summary>
        /// Add a new startup task.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <param name="continueOnFailure">Whether or not to proceed to next task on failure.</param>
        /// <param name="action">Action to execute</param>
        public virtual void OnStart(string name, bool continueOnFailure, Action<IAppContext> action)
        {
            OnStart(name, "core", Task.Importance.High, continueOnFailure, action);
        }


        /// <summary>
        /// Add a new startup task.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <param name="group">The group.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="continueOnFailure">Whether or not to proceed to next task on failure.</param>
        /// <param name="action">Action to execute</param>
        public virtual void OnStart(string name, string group, Task.Importance priority, bool continueOnFailure, Action<IAppContext> action)
        {
            _startupTasks.Add(new Task() { Name = name, Group = group, Priority = priority, Action = action, ContinueOnError = continueOnFailure });
        }


        /// <summary>
        /// Add a new startup task.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <param name="group">The group.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="isEnabled">Whether or not the task is enabled.</param>
        /// <param name="continueOnFailure">Whether or not to proceed to next task on failure.</param>
        /// <param name="action">Action to execute</param>
        public virtual void OnStart(string name, string group, Task.Importance priority, bool isEnabled, bool continueOnFailure, Action<IAppContext> action)
        {
            _startupTasks.Add(new Task() { Name = name, Group = group, Priority = priority, Action = action, IsEnabled = isEnabled, ContinueOnError = continueOnFailure });
        }


        /// <summary>
        /// Add a new startup task.
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void OnStart(Task task)
        {
            _startupTasks.Add(task);
        }


        /// <summary>
        /// Add a shutdown task.
        /// </summary>
        /// <param name="name">Name of the action.</param>
        /// <param name="group">The group.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="continueOnFailure">Whether or not to proceed next shutdown task on failure.</param>
        /// <param name="action">Action to execute on shutdown</param>
        public virtual void OnStop(string name, string group, Task.Importance priority, bool continueOnFailure, Action<IAppContext> action)
        {
            _shutdownTasks.Add(new Task() { Name = name, Group = group, Priority = priority, Action = action, ContinueOnError = continueOnFailure });
        }


        /// <summary>
        /// Add a shutdown task.
        /// </summary>
        /// <param name="task">The task.</param>
        public virtual void OnStop(Task task)
        {
            _shutdownTasks.Add(task);
        }


        /// <summary>
        /// Run startup.
        /// </summary>
        /// <param name="context"></param>
        public BoolMessage StartUp(IAppContext context)
        {
            return InternalRun(_startupTasks, context, true);
        }


        /// <summary>
        /// Run startup.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="taskNames">The task names to startup.</param>
        /// <returns></returns>
        public BoolMessage StartUp(IAppContext context, params string[] taskNames)
        {
            return InternalRun(_startupTasks, context, true, taskNames);
        }


        /// <summary>
        /// Run Shutdown.
        /// </summary>
        /// <param name="context"></param>
        public BoolMessage ShutDown(IAppContext context)
        {
            return InternalRun(_startupTasks, context, true);
        }


        /// <summary>
        /// Run Shutdown.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="taskNames">The task names.</param>
        /// <returns></returns>
        public BoolMessage ShutDown(IAppContext context, params string[] taskNames)
        {
            return InternalRun(_startupTasks, context, true, taskNames);
        }


        /// <summary>
        /// Gets the startup status.
        /// </summary>
        /// <returns></returns>
        public IList<Task> GetStartupStatus()
        {
            IList<Task> results = new List<Task>();
            foreach (var task in _startupTasks)
            {
                results.Add(new Task()
                {
                    Name = task.Name,
                    Success = task.Success,
                    ExecutedOn = task.ExecutedOn,
                    Message = task.Message,
                    Error = task.Error,
                    ContinueOnError = task.ContinueOnError,
                    Group = task.Group,
                    Priority = task.Priority,
                    IsEnabled = task.IsEnabled,
                });
            }
            return results;
        }


        /// <summary>
        /// Run the tasks supplied.
        /// </summary>
        /// <param name="tasks"></param>
        /// <param name="context"></param>
        /// <param name="isStartup"></param>
        /// <param name="taskNames"></param>
        protected virtual BoolMessage InternalRun(IList<Task> tasks, IAppContext context, bool isStartup, params string[] taskNames)
        {
            bool allSuccess = true;
            string errorMessage = string.Empty;
            IDictionary<string, string> enabledTaskNames = null;

            if (taskNames != null && taskNames.Length > 0)
                enabledTaskNames = taskNames.ToDictionary();

            foreach (var task in tasks)
            {
                try
                {
                    // everything is enabled.
                    if (enabledTaskNames == null || ( enabledTaskNames != null && enabledTaskNames.ContainsKey(task.Name)))
                    {
                        if (task.IsEnabled)
                        {
                            task.ExecutedOn = DateTime.Now;
                            task.Action(context);
                            task.Success = true;
                        }
                        else
                            Console.WriteLine("Task : " + task.Name + " is disabled.");
                    }
                }
                catch (Exception ex)
                {
                    task.Success = false;
                    task.Error = ex;
                    task.Message = ex.Message;

                    if (LogErrors)
                    {
                        if (Logger != null)
                            Logger(ex);
                        else
                        {
                            string error = isStartup ? "Startup task" : "Shutdown task";
                            Console.WriteLine("ERROR : " + task.Name + " failed. " + ex.Message);
                        }
                    }
                    if (!task.ContinueOnError)
                    {
                        if (PropagateException)
                            throw ex;
                        else
                        {
                            allSuccess = false;
                            errorMessage += ex.Message + Environment.NewLine;
                            break;
                        }
                    }
                    else
                    {   
                        errorMessage += ex.Message + Environment.NewLine;                        
                    }
                }
            }
            return new BoolMessage(allSuccess, errorMessage);
        }
    }
}
