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
    /// Controlls the processing of the notification tasks.
    /// </summary>
    public class QueueProcessor<T> : IQueueProcessor<T>
    {
        private Queue<T> _queue;
        private object _synObject = new object();
        private QueueProcessState _state;
        private int _numberToDequeuAtOnce = 5;
        private Action<IList<T>> _handler;
        private DateTime _lastProcessDate = DateTime.MinValue;
        private TimeSpan _elapsedTimeSinceLastProcessDate = TimeSpan.MinValue;
        private int _numberOfTimesProcessed = 0;
        private int _totalProcessed;


        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProcessor&lt;T&gt;"/> class.
        /// </summary>
        public QueueProcessor() : this(5, null)
        {            
        }


        /// <summary>
        /// Intialize w/ a specific handler.
        /// </summary>
        /// <param name="handler">Action to be called on item dequeue.</param>
        public QueueProcessor(Action<IList<T>> handler) : this(5, handler)
        {
        }


        /// <summary>
        /// Intialize w/ a specific handler.
        /// </summary>
        /// <param name="numberOfItemsToProcessPerDequeue">Number of items to process per dequeue.</param>
        /// <param name="handler">Action to be called on item dequeue.</param>
        public QueueProcessor(int numberOfItemsToProcessPerDequeue, Action<IList<T>> handler)
        {
            _queue = new Queue<T>();
            _state = QueueProcessState.Idle;
            _handler = handler;
            _numberToDequeuAtOnce = numberOfItemsToProcessPerDequeue;
        }


        #region Public Properties and Methods
        /// <summary>
        /// Gets the sync root.
        /// </summary>
        /// <value>The sync root.</value>
        public object SyncRoot
        {
            get { return _synObject; }
        }


        /// <summary>
        /// Gets or sets the number to process per dequeue.
        /// </summary>
        /// <value>The number to process per dequeue.</value>
        public int NumberToProcessPerDequeue
        {
            get { return _numberToDequeuAtOnce; }
            set { _numberToDequeuAtOnce = value; }
        }


        /// <summary>
        /// Add a message to the queue.
        /// </summary>
        /// <param name="item">Item to enqueue.</param>
        public void Enqueue(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
            }            
        }


        /// <summary>
        /// Dequeues the specified number to dequeue.
        /// </summary>
        /// <returns>Dequeued item.</returns>
        public T Dequeue()
        {
            IList<T> items = Dequeue(1);
            if (items == null || items.Count == 0)
                return default(T);

            return items[0];
        }


        /// <summary>
        /// Dequeues the specified number to dequeue.
        /// </summary>
        /// <param name="numberToDequeue">The number to dequeue.</param>
        /// <returns>List of dequeued items.</returns>
        public IList<T> Dequeue(int numberToDequeue)
        {
            IList<T> items = null;
            lock (_synObject)
            {
                items = DequeueInternal(numberToDequeue);
            }

            return items;
        }


        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                lock (_synObject)
                {
                    return _queue.Count;
                }
            }
        }


        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <value>The state.</value>
        public QueueProcessState State
        {
            get 
            { 
                lock (_synObject) { return _state; } 
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is idle.
        /// </summary>
        /// <value><c>true</c> if this instance is idle; otherwise, <c>false</c>.</value>
        public bool IsIdle
        {
            get
            {
                lock (_synObject)
                {
                    return _state == QueueProcessState.Idle;
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is stopped.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is stopped; otherwise, <c>false</c>.
        /// </value>
        public bool IsStopped
        {
            get
            {
                lock (_synObject)
                {
                    return _state == QueueProcessState.Stopped;
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
        public bool IsBusy
        {
            get
            {
                lock (_synObject)
                {
                    return _state == QueueProcessState.Busy;
                }
            }
        }


        /// <summary>
        /// Processes this instance.
        /// </summary>
        public virtual void Process()
        {
            IList<T> itemsToProcess = null;

            // If currently busy, don't do anything.
            if (IsBusy) { return; }

            lock (SyncRoot)
            {
                if (Count == 0)
                {
                    return;
                }

                itemsToProcess = DequeueInternal(NumberToProcessPerDequeue);

                //Check whether or not there is anything to process.
                if (itemsToProcess == null) { return; }

                UpdateState(QueueProcessState.Busy, false);
            }

            UpdateMetaInfo(itemsToProcess.Count);
            Process(itemsToProcess);            

            // Update to idle.
            UpdateState(QueueProcessState.Idle, true);
        }

        
        /// <summary>
        /// Get the state of the queue.
        /// </summary>
        /// <returns>Status of queue.</returns>
        public virtual QueueStatus GetStatus()
        {
            QueueStatus status = null;
            lock (SyncRoot)
            {
                status = new QueueStatus(_state, _queue.Count, _lastProcessDate, _numberOfTimesProcessed, _elapsedTimeSinceLastProcessDate, _totalProcessed, NumberToProcessPerDequeue);
            }
            return status;
        }
        #endregion


        /// <summary>
        /// Processes the specified items to process.
        /// </summary>
        /// <param name="itemsToProcess">The items to process.</param>
        protected virtual void Process(IList<T> itemsToProcess)
        {
            if (_handler != null)
                _handler(itemsToProcess);
        }


        /// <summary>
        /// Dequeues the internal.
        /// </summary>
        /// <param name="numberToDequeue">The number to dequeue.</param>
        /// <returns>List of dequeued items.</returns>
        protected IList<T> DequeueInternal(int numberToDequeue)
        {
            if (_queue.Count == 0)
                return null;

            IList<T> itemsToDeque = new List<T>();

            if (numberToDequeue > _queue.Count)
            {
                numberToDequeue = _queue.Count;
            }

            for (int count = 1; count <= numberToDequeue; count++)
            {
                itemsToDeque.Add(_queue.Dequeue());
            }

            return itemsToDeque;
        }


        /// <summary>
        /// Updates the state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <param name="performLock">if set to <c>true</c> [perform lock].</param>
        protected void UpdateState(QueueProcessState newState, bool performLock)
        {
            if (performLock)
            {
                lock (_synObject)
                {
                    _state = newState;
                }
            }
            else
            {
                _state = newState;
            }
        }


        private void UpdateMetaInfo(int numberBeingProcessed)
        {
            DateTime now = DateTime.Now;
            if (_lastProcessDate != DateTime.MinValue)
            {
                _elapsedTimeSinceLastProcessDate = now.TimeOfDay - _lastProcessDate.TimeOfDay;
            }
            _lastProcessDate = now;
            Interlocked.Increment(ref _numberOfTimesProcessed);
            _totalProcessed += numberBeingProcessed;
        }
    }
}
