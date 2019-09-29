using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sara.Common.Threading
{
    /// <summary>
    /// This Queue will only process the current item.
    /// While processing the current item, if a backlog of items build,
    /// the Queue will ignore the backlog and only process the most recent item.
    /// Backlog Items are discarded.
    /// </summary>
    public class IgnoreBacklogQueue<T>
    {
        #region Properties
        private Thread _consumerThread;
        private readonly AutoResetEvent _newItemEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _shutdownEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _shutdownEvent2 = new AutoResetEvent(false);
        private readonly AutoResetEvent _shutdownAckEvent = new AutoResetEvent(false);
        private readonly AutoResetEvent _itemCompleteEvent = new AutoResetEvent(false);
        private readonly object _syncObject = new object();
        private bool IsClosing;
        public event Action<T> ProcessItemEvent;
        private List<T> _Items { get; set; }
        #endregion

        #region Setup
        public IgnoreBacklogQueue(string queueName = "Thread Queue Only One")
        {
            _Items = new List<T>();

            _consumerThread = new Thread(ConsumeItem)
            {
                Name = queueName,
                IsBackground = false
            };
            _consumerThread.Start();

        }
        ~IgnoreBacklogQueue()
        {
            Exit();
        }
        #endregion

        public void Exit()
        {
            IsClosing = true;

            _shutdownEvent.Set();
            _shutdownEvent2.Set();
        }

        private void ConsumeItem()
        {
            var waitHandles = new WaitHandle[]
            {
                _newItemEvent,
                _shutdownEvent
            };
            var itemWaitHandles = new WaitHandle[]
            {
                _itemCompleteEvent,
                _shutdownEvent2
            };

            var done = false;
            while (done == false && !IsClosing)
            {
                // block and wait for the items gets to the queue
                switch (WaitHandle.WaitAny(waitHandles))
                {
                    case 0: // New item
                        {
                            var _newList = new List<T>();
                            T _current;
                            lock (_syncObject)
                            {
                                _current = _Items.Last();
                                if (_current == null)
                                    throw new Exception("There must always be at least 1 item in the Queue");
                                if (_Items.Count > 1)
                                    _newList.Add(_Items[_Items.Count - 1]);
                                _Items = _newList;
                            }

                            ProcessItemEvent?.Invoke(_current);
                            WaitHandle.WaitAny(itemWaitHandles);
                        }
                        break;
                    case 1: // Told to exit
                        done = true;
                        break;
                }
            }
            _shutdownAckEvent.Set();
        }
        public void ItemComplete()
        {
            _itemCompleteEvent.Set();
        }
        public void Add(T item)
        {
            lock (_syncObject)
                _Items.Add(item);

            _newItemEvent.Set();
        }
    }
}
