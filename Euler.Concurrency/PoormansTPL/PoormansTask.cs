// Author: Alexander Karagulamos
// Date:
// Comment:

using System;
using System.Collections.Generic;

namespace Euler.Concurrency.PoormansTPL
{
    public class PoormansTask
    {
        private Action _action;
        private readonly List<Exception> _exceptions = new List<Exception>();
        private readonly object _exceptionMonitor = new object();
        private readonly PoormansSynchronizer _synchronizer = PoormansSynchronizer.Get();

        protected PoormansTask() { }

        public PoormansTask(Action action)
        {
            _action = action;
        }

        protected void SetTask(Action action)
        {
            _action = action;
        }

        internal PoormansAwaiter GetAwaiter()
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
            CreateTask(_action);
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
            _synchronizer.Wait();
            ThrowAggregateExceptionIfFaulted();
        }

        public static int WaitAny(params PoormansTask[] tasks)
        {
            return WaitAny(tasks, false);
        }

        public static int WaitAny(PoormansTask[] tasks, bool cancelUnfinishedTasks)
        {
            var synchronizer = PoormansSynchronizer.Get();

            int completedTaskIndex = -1;

            while (completedTaskIndex < 0)
            {
                synchronizer.WaitAny();
                completedTaskIndex = Array.FindIndex(tasks, t => t.HasCompleted);
            }

            if (cancelUnfinishedTasks)
            {
                PoormansThreadPool.CancelTasksInternal();
            }

            return completedTaskIndex;
        }

        public bool HasCompleted { get; private set; }

        protected void ThrowAggregateExceptionIfFaulted()
        {
            lock (_exceptionMonitor)
            {
                if (_exceptions.Count > 0)
                    throw new AggregateException(_exceptions);
            }
        }

        private void CreateTask(Action action)
        {
            PoormansThreadPool.EnqueueTaskInternal(() =>
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
                    HasCompleted = true;
                    _synchronizer.Signal();
                }
            });
        }
    }

    public sealed class PoormansTask<TResult> : PoormansTask
    {
        private TResult _result;

        public PoormansTask(Func<TResult> action)
        {
            base.SetTask(() => _result = action());
        }

        public TResult Result
        {
            get
            {
                base.Wait();
                return _result;
            }
        }

        internal new PoormansAwaiter<TResult> GetAwaiter()
        {
            return new PoormansAwaiter<TResult>(this);
        }
    }
}
