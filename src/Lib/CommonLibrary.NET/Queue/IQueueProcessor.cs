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
using System.Text;
using System.Threading;


namespace ComLib.Queue
{
    /// <summary>
    /// Queue processing interface.
    /// </summary>
    public interface IQueueProcessor
    {
        /// <summary>
        /// Gets the number of items in the queue.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        bool IsBusy { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is idle.
        /// </summary>
        /// <value><c>true</c> if this instance is idle; otherwise, <c>false</c>.</value>
        bool IsIdle { get; }


        /// <summary>
        /// Gets a value indicating whether this instance is stopped.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is stopped; otherwise, <c>false</c>.
        /// </value>
        bool IsStopped { get; }


        /// <summary>
        /// Processes this instance.
        /// </summary>
        void Process();


        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        QueueProcessState State { get; }


        /// <summary>
        /// Gets information about the current state.
        /// </summary>
        /// <returns></returns>
        QueueStatus GetStatus();
    }



    /// <summary>
    /// Queue processing interface w/ specific type.
    /// </summary>
    /// <typeparam name="T">Type of items to store in queue.</typeparam>
    public interface IQueueProcessor<T> : IQueueProcessor
    {

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Enqueue(T item);


        /// <summary>
        /// Dequeues a single item from the queue.
        /// </summary>
        /// <returns></returns>
        T Dequeue();


        /// <summary>
        /// Dequeues the specified number to dequeue.
        /// </summary>
        /// <param name="numberToDequeue">The number to dequeue.</param>
        /// <returns>List of dequeued items.</returns>
        IList<T> Dequeue(int numberToDequeue);


        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        object SyncRoot { get; }
    }

}
