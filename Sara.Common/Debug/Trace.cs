using System;
using System.Threading;

namespace Sara.Common.Debug
{
    /// <summary>
    /// Simple class used to trace the duration of an activity in Milliseconds
    /// Returns the Total and AverageDuration of an activity
    /// </summary>
    public class Trace
    {
        public int TotalDurationMs;
        public int AverageDurationMs;
        private int _current;
        public void Add(System.DateTime start)
        {
            Interlocked.Increment(ref _current);
            var durationMs = (int)Math.Round((System.DateTime.Now - start).TotalMilliseconds);
            Interlocked.Add(ref TotalDurationMs, durationMs);
            Interlocked.Exchange(ref AverageDurationMs, TotalDurationMs / _current);
        }
    }
}
