using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Sara.NETStandard.Common.Threading
{
    /// <summary>
    /// Wraps a Background Thread into a simple call that will wait until complete before continues.
    /// </summary>
    public class SimpleBackgroundThreadWait
    {
        public void DoWork(Action work, Action doEvents)
        {
            var doneEvent = new AutoResetEvent(false);
            var bw = new BackgroundWorker();

            bw.DoWork += (sender, e) =>
            {
                try
                {
                    work();
                }
                finally
                {
                    doneEvent.Set();
                }
            };

            bw.RunWorkerAsync();

            do
            {
                doEvents();
            }
            while (!doneEvent.WaitOne(1000));

        }
    }
}
