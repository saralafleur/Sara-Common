using System;
using System.Collections.Generic;
using System.Threading;

namespace Sara.Common.Threading
{
    /// <summary>
    /// Processes a Queue on it's own thread allowing other processes to continue
    /// </summary>
    public class ThreadQueue<T>
    {
        #region Properties
        private const int QUEUE_START_SIZE = 100;
        private Thread _consumerThread;
        private readonly AutoResetEvent _newItemEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _shutdownEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _shutdownAckEvent = new AutoResetEvent(false);
        private readonly object _queueSyncObject = new object();
        public event Action<T> ProcessItemEvent;
        private Queue<T> _queuedMessages { get; set; }
        #endregion

        #region Setup
        public ThreadQueue(string queueName = "Thread Queue")
        {
            _queuedMessages = new Queue<T>(QUEUE_START_SIZE);

            _consumerThread = new Thread(ConsumeItem)
            {
                Name = queueName,
                IsBackground = false
            };
            _consumerThread.Start();
        }
        ~ThreadQueue()
        {
            Exit();
        }
        #endregion

        public void Exit()
        {
            _shutdownEvent.Set();
        }

        private void ConsumeItem()
        {
            var waitHandles = new WaitHandle[]
                      {
                                          _newItemEvent,
                                          _shutdownEvent
                      };
            var done = false;
            while (done == false)
            {
                // block and wait for the items gets to the queue
                switch (WaitHandle.WaitAny(waitHandles))
                {
                    case 0: // New item
                        {
                            Queue<T> itemsToProcess;
                            var newQueue = new Queue<T>(QUEUE_START_SIZE);

                            lock (_queueSyncObject)
                            {
                                itemsToProcess = _queuedMessages;
                                _queuedMessages = newQueue;
                            }
                            foreach (var item in itemsToProcess)
                            {
                                ProcessItemEvent?.Invoke(item);
                            }
                        }
                        break;
                    case 1: // Told to exit
                        done = true;
                        break;
                }
            }
            _shutdownAckEvent.Set();
        }

        public void Enqueue(T item)
        {
            lock (_queueSyncObject)
            {
                _queuedMessages.Enqueue(item);
            }

            // Notify the Thread a new Item has been enqueue
            _newItemEvent.Set();
        }
    }
}
