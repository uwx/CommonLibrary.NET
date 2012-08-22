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
using ComLib;


namespace ComLib.StatusUpdater
{
    /// <summary>
    /// Constants representing the various status.
    /// </summary>
    public partial class StatusUpdateConstants
    {
        /// <summary>
        /// Status update started.
        /// </summary>
        public const string Started = "started";


        /// <summary>
        /// Status update completed.
        /// </summary>
        public const string Completed = "completed";


        /// <summary>
        /// Status update running.
        /// </summary>
        public const string Running = "running";


        /// <summary>
        /// Status update failed.
        /// </summary>
        public const string Failed = "failed";
    }



    /// <summary>
    /// StatusUpdate extensions with helper methods
    /// </summary>
    public partial class StatusUpdates 
    {
        /// <summary>
        /// Update status for the specified runId, taskname combination.
        /// </summary>
        /// <remarks>Overloaded convenience method.</remarks>
        /// <param name="taskName">Name of task.</param>
        /// <param name="status">Status of task.</param>
        /// <param name="comment">Task comment.</param>
        /// <param name="started">Task start time.</param>
        /// <param name="ended">Task end time.</param>
        public static void Update(string taskName, string status, string comment, DateTime started, DateTime ended)
        {
            Service.Update(taskName, status, comment, started, ended);
        }


        /// <summary>
        /// Get the singleton service.
        /// </summary>
        public static StatusUpdateService Service
        {
            get { return EntityRegistration.GetService<StatusUpdate>() as StatusUpdateService; }
        }
    }



    /// <summary>
    /// Assign global values to be used for a specific batch run.
    /// </summary>
    public partial class StatusUpdateService : EntityService<StatusUpdate>
    {
        /// <summary>
        /// Name of the batch. e.g. "EndOfMonth"
        /// </summary>
        public string BatchName { get; set; }

        
        /// <summary>
        /// Batch time - Start time of the batch.
        /// </summary>
        public DateTime BatchTime { get; set; }


        /// <summary>
        /// Batch id identifies a single batch between multiple batch names,
        /// business dates.
        /// </summary>
        public int BatchId { get; set; }


        /// <summary>
        /// Business date of the batch / tasks run.
        /// </summary>
        public DateTime BusinessDate { get; set; }


        /// <summary>
        /// Get list of data massagers for the entity.
        /// </summary>
        /// <returns>Status with error message.</returns>
        protected override BoolMessage  PerformValidation(IActionContext ctx, EntityAction entityAction)
        {            
            var massager = new StatusUpdateMassager();
            massager.Massage(ctx.Item, entityAction);
 	        return base.PerformValidation(ctx, entityAction);
        }


        /// <summary>
        /// Update status for the specified runId, taskname combination.
        /// </summary>
        /// <remarks>Overloaded convenience method.</remarks>
        /// <param name="taskName">Name of task.</param>
        /// <param name="status">Status of task.</param>
        /// <param name="comment">Task comment.</param>
        /// <param name="started">Task start time.</param>
        /// <param name="ended">Task end time.</param>
        public void Update(string taskName, string status, string comment, DateTime started, DateTime ended)
        {
            // To uniquely identify a status.
            string filter = string.Format(" BatchId = {0} and BatchName = '{1}' and Task = '{2}' ", this.BatchId, this.BatchName, taskName);
            IList<StatusUpdate> items = Find(filter);
            bool isCreating = false;

            if (items == null || items.Count == 0)
                isCreating = true;

            StatusUpdate entry = isCreating ? new StatusUpdate() : items[0];
            entry.Task = taskName;
            entry.Status = status.ToLower().Trim();
            entry.Comment = comment;
            if (isCreating)
            {
                entry.StartTime = started;
                entry.EndTime = ended;
                Create(entry);
            }
            else
            {
                entry.EndTime = ended;
                Update(entry);
            }
        }
    }


    
    /// <summary>
    /// Data massager for StatusUpdates.
    /// </summary>
    public partial class StatusUpdateMassager : EntityMassager
    {
        /// <summary>
        /// Populate the username, computer and comment.
        /// </summary>
        /// <param name="entity">Entity to populate.</param>
        /// <param name="action">Action.</param>
        public override void Massage(object entity, EntityAction action)
        {
            StatusUpdate update = entity as StatusUpdate;
            update.Computer = Environment.MachineName;
            update.ExecutionUser = Environment.UserName;

            // Set times.
            if (update.StartTime == DateTime.MinValue) update.StartTime = DateTime.Now;
            if (update.EndTime == DateTime.MinValue) update.EndTime = DateTime.Now;
            if (update.BatchTime == DateTime.MinValue) update.BatchTime = StatusUpdates.Service.BatchTime;
            if (update.BusinessDate == DateTime.MinValue) update.BusinessDate = StatusUpdates.Service.BusinessDate;
            if (update.BatchId <= 0) update.BatchId = StatusUpdates.Service.BatchId;
            if (string.IsNullOrEmpty(update.BatchName)) update.BatchName = StatusUpdates.Service.BatchName;
            
            // Add comment.
            if (string.IsNullOrEmpty(update.Comment))
            {
                // Batch , Task, Status at Time.
                string comment = string.Format("[{0}] : [{1}] : [{2}]",
                    update.BatchName, update.Task, update.Status);
                update.Comment = comment;
            }
        }
    }
}
