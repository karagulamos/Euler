// Author: Alexander Karagulamos
// Date:
// Comment:

using System;
using System.Collections.Generic;
using System.Threading;

namespace Euler.Concurrency.PoormansTPL
{
    internal class PoormansTask
    {
        private Thread _worker;
        private readonly List<Exception> _exceptions = new List<Exception>();
        private readonly object _exceptionMonitor = new object();

        protected PoormansTask() { }

        public PoormansTask(Action action)
        {
            this.CreateNewThreadObject(action);
        }

        public PoormansAwaiter GetAwaiter()
        {
            return new PoormansAwaiter(this);
        }

        public IReadOnlyList<Exception> Exceptions
        {
            get
            {
                lock (_exceptionMonitor)
                    return _exceptions;
            }
        }

        public void Start()
        {
            _worker.Start();
        }

        public static PoormansTask Run(Action action)
        {
            var poorTask = new PoormansTask(action);
            poorTask.Start();
            return poorTask;
        }

        public static PoormansTask<TResult> Run<TResult>(Func<TResult> action)
        {
            var poorTask = new PoormansTask<TResult>(action);
            poorTask.Start();
            return poorTask;
        }

        public static PoormansTask<TResult> Run<TResult>(Func<PoormansTask<TResult>> action)
        {
            var poorTask = new PoormansTask<PoormansTask<TResult>>(action);
            poorTask.Start();
            return poorTask.Result;
        }

        public static void WaitAll(params PoormansTask[] tasks)
        {
            foreach (var task in tasks)
            {
                task.Wait();
            }
        }

        public void Wait()
        {
            _worker.Join();
            this.ThrowAggregateExceptionIfFaulted();
        }
        public static int WaitAny(params PoormansTask[] tasks)
        {
            return WaitAny(tasks, false);
        }

        public static int WaitAny(PoormansTask[] tasks, bool cancelUnfinishedTasks)
        {
            var sychronizer = PoormansSynchronizer.Get();

            int completedTaskIndex = -1;

            while (completedTaskIndex < 0)
            {
                sychronizer.WaitAny();
                completedTaskIndex = Array.FindIndex(tasks, t => t.HasCompleted());
            }

            if (cancelUnfinishedTasks)
            {
                foreach (var task in tasks)
                {
                    task.Cancel();
                }
            }

            return completedTaskIndex;
        }

        protected void Cancel()
        {
            _worker.Abort();
        }

        public bool HasCompleted()
        {
            return Thread.CurrentThread != _worker && _worker.Join(TimeSpan.Zero);
        }

        protected void ThrowAggregateExceptionIfFaulted()
        {
            lock (_exceptionMonitor)
            {
                if (_exceptions.Count > 0)
                    throw new AggregateException(_exceptions);
            }
        }

        protected void CreateNewThreadObject(Action action)
        {
            var sychronizer = PoormansSynchronizer.Get();

            _worker = new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    lock (_exceptionMonitor)
                        _exceptions.Add(exception);
                }
                finally
                {
                    sychronizer.Signal();
                }
            })
            { IsBackground = true };

        }
    }

    internal sealed class PoormansTask<TResult> : PoormansTask
    {
        private TResult _result;

        public PoormansTask(Func<TResult> action)
        {
            base.CreateNewThreadObject(() => _result = action());
        }

        public TResult Result
        {
            get
            {
                base.Wait();
                return _result;
            }
        }

        public new PoormansAwaiter<TResult> GetAwaiter()
        {
            return new PoormansAwaiter<TResult>(this);
        }
    }
}
