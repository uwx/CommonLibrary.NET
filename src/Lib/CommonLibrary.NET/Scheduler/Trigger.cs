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
    /// Trigger for schedule entry. Represents when / how often to run a task.
    /// </summary>
    public class Trigger : ICloneable
    {
        /// <summary>
        /// Whether or not to repeat
        /// </summary>
        public bool Repeat;


        /// <summary>
        /// The starting time of the trigger.
        /// </summary>
        public DateTime StartTime;


        /// <summary>
        /// The ending time.
        /// </summary>
        public DateTime EndTime;


        /// <summary>
        /// Time duration between triggers.
        /// </summary>
        public int Interval;


        /// <summary>
        /// Whether or not there is a limit to the number of times this should run.
        /// </summary>
        public bool HasMaxIterations;


        /// <summary>
        /// Total number of times this should run.
        /// </summary>
        public int MaxIterations;

        
        /// <summary>
        /// Whether or not this is an ondemand trigger.
        /// </summary>
        public bool IsOnDemand;



        #region Fluent API
        /// <summary>
        /// Indicate that the trigger is ondemand.
        /// </summary>
        /// <returns>This instance.</returns>
        public Trigger OnDemand()
        {
            IsOnDemand = true;
            Repeat = false;
            MaxIterations = 0;
            HasMaxIterations = true;
            return this;
        }


        /// <summary>
        /// Set the max runs
        /// </summary>
        /// <param name="maxRuns">The max runs.</param>
        /// <returns>This instance.</returns>
        public Trigger MaxRuns(int maxRuns)
        {
            Repeat = true;
            MaxIterations = maxRuns;
            HasMaxIterations = true;
            return this;
        }


        /// <summary>
        /// Set the stop time.
        /// </summary>
        /// <param name="timeToStop">The time to stop.</param>
        /// <returns>This instance.</returns>
        public Trigger StopAt(DateTime timeToStop)
        {
            EndTime = timeToStop;
            return this;
        }


        /// <summary>
        /// Set up a repeating time.
        /// </summary>
        /// <param name="timespan">The timespan.</param>
        /// <returns>This instance.</returns>
        public Trigger Every(TimeSpan timespan)
        {
            Interval = (int)timespan.TotalMilliseconds;
            return this;
        }
        #endregion


        #region ICloneable Members
        /// <summary>
        /// Clone this object.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
