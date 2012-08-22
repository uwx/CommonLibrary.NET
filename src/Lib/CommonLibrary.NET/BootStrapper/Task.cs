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
    /// Task class.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Priority for ToDo items.
        /// </summary>
        public enum Importance { 
            /// <summary>
            /// Low priority.
            /// </summary>
            Low, 
            

            /// <summary>
            /// Normal priority.
            /// </summary>
            Normal, 
            
            
            /// <summary>
            /// High priority.
            /// </summary>
            High, 
            
            
            /// <summary>
            /// Critical priority.
            /// </summary>
            Critical };


        /// <summary>
        /// Name of the task.
        /// </summary>
        public string Name;


        /// <summary>
        /// Group the task belongs to.
        /// </summary>
        public string Group;


        /// <summary>
        /// Priority of the task.
        /// </summary>
        public Importance Priority;


        /// <summary>
        /// Action that the task runs.
        /// </summary>
        public Action<IAppContext> Action;


        /// <summary>
        /// Whether or not the task is enabled.
        /// </summary>
        public bool IsEnabled = true;


        /// <summary>
        /// Whether or not the task should continue if there is an error.
        /// </summary>
        public bool ContinueOnError;


        /// <summary>
        /// Time the task was run
        /// </summary>
        public DateTime ExecutedOn;

        /// <summary>
        /// Whether or not the task was successful.
        /// </summary>
        public bool Success;


        /// <summary>
        /// Error or message of the task.
        /// </summary>
        public string Message;


        /// <summary>
        /// Exception that occurred for the task.
        /// </summary>
        public Exception Error;


        /// <summary>
        /// Get the status of the task.
        /// </summary>
        /// <returns></returns>
        public string Status()
        {
            if (!IsEnabled) return "Not Enabled";
            if (ExecutedOn == DateTime.MinValue) return "Not Yet Run";
            return Success ? "Successful" : "Failed";
        }
    }



    /// <summary>
    /// BootTask with fluent API.
    /// </summary>
    public class BootTask : Task
    {
        /// <summary>
        /// Sets the name of the task.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BootTask Named(string name)
        {
            BootTask task = new BootTask();
            task.Name = name;
            return task;
        }

        
        /// <summary>
        /// Set the group of the task.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public BootTask InGroup(string group)
        {
            this.Group = group;
            return this;
        }


        /// <summary>
        /// Sets the priority high.
        /// </summary>
        /// <value>The priority high.</value>
        public BootTask PriorityHigh
        {
            get 
            {
                this.Priority = Importance.High; 
                return this; 
            }
        }


        /// <summary>
        /// Sets the priority low.
        /// </summary>
        /// <value>The priority low.</value>
        public BootTask PriorityLow
        {
            get 
            {
                this.Priority = Importance.Low; 
                return this; 
            }
        }


        /// <summary>
        /// Sets priority normal.
        /// </summary>
        /// <value>The priority normal.</value>
        public BootTask PriorityNormal
        {
            get 
            {
                this.Priority = Importance.Normal; 
                return this; 
            }
        }


        /// <summary>
        /// Sets flag indicating task Must succeed.
        /// </summary>
        /// <returns></returns>
        public BootTask MustSucceed()
        {
            this.ContinueOnError = false;
            return this;
        }


        /// <summary>
        /// Sets flag indicating task can fail.
        /// </summary>
        /// <returns></returns>
        public BootTask CanFail()
        {
            this.ContinueOnError = true;
            return this;
        }


        /// <summary>
        /// Sets the action of the task
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public BootTask ActionIs(Action<IAppContext> action)
        {
            this.Action = action;
            return this;
        }


        /// <summary>
        /// Enables this task if the flag is true.
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public BootTask If(bool isEnabled)
        {
            this.IsEnabled = isEnabled;
            return this;
        }
    }
}
