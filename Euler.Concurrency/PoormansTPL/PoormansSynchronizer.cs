using System;
using System.Threading;

namespace Euler.Concurrency.PoormansTPL
{
    internal class PoormansSynchronizer
    {
        private volatile bool _signaled;
        private readonly object _internalLocker = new object();

        private static readonly Lazy<PoormansSynchronizer> LazyInstance = new Lazy<PoormansSynchronizer>(() => new PoormansSynchronizer(), true);

        private PoormansSynchronizer() {}

        public static PoormansSynchronizer Get()
        {
            return LazyInstance.Value;
        }

        public bool Wait()
        {
            bool status = false;

            lock (_internalLocker)
            {
                while (!_signaled)
                    status = Monitor.Wait(_internalLocker);
                _signaled = false;
            }

            return status;
        }

        public void Signal()
        {
            lock (_internalLocker)
            {
                _signaled = true;
                Monitor.PulseAll(_internalLocker);
            }
        }

        public void WaitAny()
        {
            lock (_internalLocker)
            {
                while (!_signaled)
                    Monitor.Wait(_internalLocker);
            }
        }
    }
}