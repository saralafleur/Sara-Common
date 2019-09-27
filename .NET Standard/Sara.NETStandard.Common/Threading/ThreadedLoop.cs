using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sara.NETStandard.Common.Debug;

namespace Sara.NETStandard.Common.Threading
{
    public abstract class ThreadedLoop<T>
    {
        public string ThreadInfo =>
            $@"
Active Workers: {ActiveWorkers}
Average Run(ms): {_run.AverageDurationMs}
Runs per second: {RunsPerSecond}
Est Total Time(m): {EstTotalTime}
Waiting on Queue: {WaitingOnQueue}
Average Queue wait(ms): {_queue.AverageDurationMs}{AdditionalThreadInfo}";

        public string AdditionalThreadInfo;
        /// <summary>
        /// Number of worker threads that will be created
        /// </summary>
        public int WorkerLimit = 5;
        public int ActiveWorkers;
        public int WaitingOnQueue;
        public double RunsPerSecond
        {
            get
            {
                var totalSeconds = (DateTime.Now - _start).TotalSeconds;
                return (Current / totalSeconds);
            }
        }
        public string EstTotalTime => ((_total / RunsPerSecond) / 60).ToString();
        private Trace _run;
        private Trace _queue;
        private DateTime _start;
        private DateTime? _stop;
        /// <summary>
        /// Total Duration of a Run in milliseconds
        /// </summary>
        public double RunDurationMs => (_start - _stop)?.TotalMilliseconds ?? (_start - DateTime.Now).TotalMilliseconds;

        /// <summary>
        /// Interval used for Progress Updates
        /// </summary>
        public int UpdateInterval = 100;
        /// <summary>
        /// Total Duration in Milliseconds of Threads waiting for next Task.
        /// </summary>
        public long QueueWaitMs => Interlocked.Read(ref _queueWaitMs);

        private long _queueWaitMs;

        public Queue<T> Queue;
        private readonly object _queueLock = new object();
        private int _total;
        public int Current;
        public volatile bool Cancelled = false;
        public string Name { get; set; }
        public void Run()
        {
            _start = DateTime.Now;
            _stop = null;
            if (Queue.Count == 0)
            {
                return;
            }

            _total = Queue.Count();
            Current = 0;

            var workers = new List<Thread>();

            for (var i = 0; i < WorkerLimit; i++)
            {
                var worker = new Thread(ProcessQueue) { Name = Name };
                worker.Start();
                workers.Add(worker);
            }

            while (true & !Cancelled)
            {
                Thread.Sleep(UpdateInterval);
                lock (_queueLock)
                {
                    if (Queue.Count == 0) break;
                }
            }

            for (var i = 0; i < WorkerLimit; i++)
            {
                workers[i].Join();
            }
            _stop = DateTime.Now;
        }

        private void ProcessQueue()
        {
            while (true & !Cancelled)
            {
                T item;
                var _startQueueWait = DateTime.Now;

                Interlocked.Increment(ref WaitingOnQueue);
                lock (_queueLock)
                {
                    if (Queue.Count == 0)
                    {
                        // queue is empty
                        return;
                    }
                    item = Queue.Dequeue();
                }
                Interlocked.Decrement(ref WaitingOnQueue);
                Interlocked.Increment(ref Current);

                _queue.Add(_startQueueWait);


                Interlocked.Increment(ref ActiveWorkers);
                ProgressUpdate(Current, _total);
                var startDuration = DateTime.Now;

                var _startRun = DateTime.Now;

                RunItem(item);

                _run.Add(_startRun);

                Interlocked.Decrement(ref ActiveWorkers);
            }
        }

        protected abstract void RunItem(T t);

        protected abstract void ProgressUpdate(int current, int total);

        public ThreadedLoop()
        {
            _run = new Trace();
            _queue = new Trace();
        }
    }
}
