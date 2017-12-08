using System;
using System.Runtime.CompilerServices;

namespace Euler.Concurrency.PoormansTPL
{
    internal class PoormansAwaiter : INotifyCompletion
    {
        protected readonly PoormansTask Task;

        public PoormansAwaiter(PoormansTask task)
        {
            Task = task;
        }

        public bool IsCompleted => Task.HasCompleted();

        public void OnCompleted(Action continuation)
        {
            continuation();
        }

        public void GetResult()
        {
            Task.Wait();
        }
    }

    internal class PoormansAwaiter<TResult> : PoormansAwaiter
    {
        public PoormansAwaiter(PoormansTask poormansTask) : base(poormansTask)
        {
        }

        public new TResult GetResult()
        {
            return ((PoormansTask<TResult>)Task).Result;
        }
    }
}
