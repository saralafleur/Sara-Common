using System;
using Sara.NETStandard.Common.Extension;

namespace Sara.NETStandard.Common.DateTimeNS
{
    public static class StopWatchOutput
    {
        public static Action<string> Log { get; set; }
    }
    public class Stopwatch
    {
        private const int DEFAULT_LIMIT = 50;
        public DateTime StartTime { get; set; }
        private DateTime StopTime { get; set; }
        private string Message { get; set; }
        private bool Enabled { get; set; }
        public Stopwatch(string message)
            : this(message, true)
        {
        }
        public Stopwatch(string message, bool enabled)
        {
            StartTime = DateTime.Now;
            Message = message;
            Enabled = enabled;
            Duration = null;
        }
        public void Stop()
        {
            Stop(DEFAULT_LIMIT);
        }
        public void Stop(string additionalMessage)
        {
            Stop(additionalMessage, DEFAULT_LIMIT);
        }
        public void ChangeMessage(string message)
        {
            Message = message;
        }
        public int Stop(int limitMs, bool showWarning = false)
        {
            return Stop(string.Empty, limitMs);
        }
        public int Stop(string additionalMessage, int limitMs)
        {
            try
            {
                if (!Enabled)
                    return 0;
                StopTime = DateTime.Now;
                Duration = StartTime.Difference(StopTime);
                // Note: Stopwatch will only output a message if the duration passes the limit!
                if (limitMs != 0 && Duration.Value.TotalMilliseconds < limitMs) return 0;

                additionalMessage = additionalMessage == string.Empty ? "" : $" : {additionalMessage}";

                var m = $"Stopwatch: {Duration.Value.ToReadableString(30)} - {Message}{additionalMessage}";

                StopWatchOutput.Log?.Invoke(m);

                return Duration.Value.Seconds;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public TimeSpan? Duration { get; set; }
    }
}