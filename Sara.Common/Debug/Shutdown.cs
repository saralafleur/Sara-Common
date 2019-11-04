using System.Threading;

namespace Sara.Common.Debug
{
    /// <summary>
    /// Global Thread safe class to signal a process to Shutdown
    /// </summary>
    public static class Shutdown
    {
        private static readonly ManualResetEvent _shutdown = new ManualResetEvent(false);
        public static bool Now => _shutdown.WaitOne(0);

        public static WaitHandle WaitHandle => _shutdown;

        public static void Set()
        {
            _shutdown.Set();
        }
    }
}
