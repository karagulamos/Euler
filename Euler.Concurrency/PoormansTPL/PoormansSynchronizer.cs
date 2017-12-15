using System;
using System.Threading;

namespace Euler.Concurrency.PoormansTPL
{
    internal class PoormansSynchronizer
    {
        private volatile bool _signaled;
        private readonly object _internalMonitor = new object();
        private static readonly Lazy<PoormansSynchronizer> LazyInstance = new Lazy<PoormansSynchronizer>(() => new PoormansSynchronizer(), true);

        private PoormansSynchronizer() { }

        public static PoormansSynchronizer Get()
        {
            return LazyInstance.Value;
        }

        public bool Wait()
        {
            bool status = false;

            lock (_internalMonitor)
            {
                while (!_signaled)
                    status = Monitor.Wait(_internalMonitor);
                _signaled = false;
            }

            return status;
        }

        public void Signal()
        {
            lock (_internalMonitor)
            {
                _signaled = true;
                Monitor.PulseAll(_internalMonitor);
            }
        }

        public void WaitAny()
        {
            lock (_internalMonitor)
            {
                while (!_signaled)
                    Monitor.Wait(_internalMonitor);
            }
        }
    }
}