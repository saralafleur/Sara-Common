using System;
using System.Collections.Generic;
using System.Threading;

namespace Sara.Common.Threading
{
    /// <summary>
    /// ThreadedWorkerQueue manages a queue that is processed by a worker thread.
    /// </summary>
    public abstract class ThreadedWorkerQueue<T> : IDisposable
    {
        readonly Queue<Action> _queue = new Queue<Action>();
        readonly ManualResetEvent _hasNewItems = new ManualResetEvent(false);
        readonly ManualResetEvent _terminate = new ManualResetEvent(false);
        readonly ManualResetEvent _waiting = new ManualResetEvent(false);

        readonly Thread _loggingThread;

        public string Name
        {
            get { return _loggingThread.Name; }
            set { _loggingThread.Name = value; }
        }

        protected ThreadedWorkerQueue()
        {
            _loggingThread = new Thread(ProcessQueue) { IsBackground = true };
            // this is performed from a bg thread, to ensure the queue is serviced from a single thread
            _loggingThread.Start();
        }

        void ProcessQueue()
        {
            while (true)
            {
                _waiting.Set();
                var i = WaitHandle.WaitAny(new WaitHandle[] { _hasNewItems, _terminate });
                // terminate was signaled 
                if (i == 1) return;
                _hasNewItems.Reset();
                _waiting.Reset();

                Queue<Action> queueCopy;
                lock (_queue)
                {
                    queueCopy = new Queue<Action>(_queue);
                    _queue.Clear();
                }

                foreach (var log in queueCopy)
                {
                    log();
                }
            }
        }

        public void Enqueue(T t)
        {
            lock (_queue)
            {
                _queue.Enqueue(() => ProcessItem(t));
            }
            _hasNewItems.Set();
        }

        protected abstract void ProcessItem(T t);

        public void Flush()
        {
            _waiting.WaitOne();
        }

        public void Dispose()
        {
            _terminate.Set();
            _loggingThread.Join();
        }
    }
}
