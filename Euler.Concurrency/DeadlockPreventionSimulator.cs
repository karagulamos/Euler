// Author: Alexander Karagulamos
// Date:
// Comment:

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Euler.Concurrency
{
    public class DeadlockPreventionSimulator
    {
        private readonly Task[] _concurrentTasks;

        public DeadlockPreventionSimulator(int numberOfTasks)
        {
            _concurrentTasks = new Task[numberOfTasks];
            var resourceMonitors = new object[numberOfTasks];

            for (var idx = 0; idx < numberOfTasks; ++idx)
            {
                var firstResourceId = idx;
                var secondResourceId = (firstResourceId + 1) % numberOfTasks;

                resourceMonitors[firstResourceId] = new object();

                // Uncomment the conditional block below to simulate a deadlock
                if ((firstResourceId & 1) == 0)
                {
                    _concurrentTasks[idx] = new Task(() =>
                        {
                            var task = new ConcurrentTask(secondResourceId, firstResourceId, resourceMonitors);
                            task.Run();
                        }
                    );

                    continue;
                }

                _concurrentTasks[idx] = new Task(() =>
                    {
                        var task = new ConcurrentTask(firstResourceId, secondResourceId, resourceMonitors);
                        task.Run();
                    }
                );
            }
        }

        public static DeadlockPreventionSimulator Get(int numberOfTasks = 5) => new DeadlockPreventionSimulator(numberOfTasks);

        public void Run()
        {
            foreach (var task in _concurrentTasks)
            {
                task.Start();
            }

            Task.WaitAll(_concurrentTasks);
        }

        private class ConcurrentTask
        {
            private readonly int _firstResourceId;
            private readonly int _secondResourceId;
            private readonly object[] _resourceMonitors;

            public ConcurrentTask(int firstResourceId, int secondResourceId, object[] resourceMonitors)
            {
                _firstResourceId = firstResourceId;
                _secondResourceId = secondResourceId;
                _resourceMonitors = resourceMonitors;
            }

            public void Run()
            {
                while (true)
                {
                    Console.WriteLine();

                    Message($"Preparing to access resources {_firstResourceId} and {_secondResourceId}.");

                    Console.WriteLine();

                    Message($"Acquiring resource {_firstResourceId}.");
                    lock (_resourceMonitors[_firstResourceId])
                    {
                        // In case you're simulating a deadlock (see DeadlockPreventionSimulator constructor), 
                        // I suggest you play around with different timeout values and observe the behaviour
                        Thread.Sleep(TimeSpan.FromSeconds(1)); 

                        Message($"Resource {_firstResourceId} successfully acquired.");

                        Console.WriteLine();

                        Message($"Acquiring resource {_secondResourceId}.");
                        lock (_resourceMonitors[_secondResourceId])
                        {
                            Message($"Resource {_secondResourceId} successfully acquired.");
                            Console.WriteLine();
                        }

                        Message($"Resources {_firstResourceId} and {_secondResourceId} successful acquired.");
                    }
                }
            }

            private void Message(string text)
            {
                Console.WriteLine($"Concurrent Task {_firstResourceId}: {text}.");
            }
        }
    }
}
