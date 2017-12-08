// Author: Alexander Karagulamos
// Date:
// Comment:

using Euler.Concurrency.PoormansTPL;
using System;
using System.Threading;

namespace Euler.Concurrency.DeadlockPrevention
{
    public class DeadlockPreventionSimulator
    {
        private readonly PoormansTask[] _concurrentActors;

        public DeadlockPreventionSimulator(int numberOfTasks)
        {
            _concurrentActors = new PoormansTask[numberOfTasks];
            var resourceMonitors = new object[numberOfTasks];

            for (var idx = 0; idx < numberOfTasks; ++idx)
            {
                var firstResourceId = idx;
                var secondResourceId = (idx + 1) % numberOfTasks;

                resourceMonitors[firstResourceId] = new object();

                // Uncomment the conditional block below to simulate a deadlock
                if ((firstResourceId & 1) == 0)
                {
                    _concurrentActors[idx] = new PoormansTask(() =>
                       {
                           var actor = new Actor(secondResourceId, firstResourceId, resourceMonitors);
                           actor.Execute();
                       }
                    );

                    continue;
                }

                _concurrentActors[idx] = new PoormansTask (() =>
                    {
                        var actor = new Actor(firstResourceId, secondResourceId, resourceMonitors);
                        actor.Execute();
                    }
                );
            }
        }

        public static DeadlockPreventionSimulator Get(int numberOfTasks = 5) => new DeadlockPreventionSimulator(numberOfTasks);

        public void Run()
        {
            foreach (var task in _concurrentActors)
            {
                task.Start();
            }

            PoormansTask.WaitAll(_concurrentActors);
        }

        private class Actor
        {
            private readonly int _firstResourceId;
            private readonly int _secondResourceId;
            private readonly object[] _resourceMonitors;

            public Actor(int firstResourceId, int secondResourceId, object[] resourceMonitors)
            {
                _firstResourceId = firstResourceId;
                _secondResourceId = secondResourceId;
                _resourceMonitors = resourceMonitors;
            }

            public void Execute()
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

                        Message($"Resources {_firstResourceId} and {_secondResourceId} successfully acquired.");
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
