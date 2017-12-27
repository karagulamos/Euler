using Euler.Algorithms.Permutations;
using Euler.Concurrency.DeadlockPrevention;
using Euler.Concurrency.PoormansTPL;
using Euler.DataStructures.BinaryTree;
using Euler.DataStructures.Heap;
using Euler.Patterns.Bridge.Drivers;
using Euler.Patterns.Proxy.Payment;
using Euler.Patterns.Proxy.Payment.Entities;
using Euler.Patterns.Proxy.Payment.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Euler
{
    public partial class Test
    {
        public static void Main()
        {
            //******** Data Structures & Algorithms ********//

            //RunPermutationExample();

            //RunHeapSortExample();

            RunGoConcurrentTreeWalkExercise();

            //******** Concurreny ********//

            // RunDeadlockPreventionSimulationExample();

            //******** Design Patterns ********//

            // RunConnectionDriverBridgeExample();

            // RunPaymentProxyExample();

            Console.ReadLine();
        }

        private static void RunGoConcurrentTreeWalkExercise()
        {
            // https://tour.golang.org/concurrency/8

            var tree1 = new Tree<int>();
            var tree2 = new Tree<int>();

            for (var i = 1; i <= 10; i++)
            {
                tree1.Add(i);
                tree2.Add(i << 1);
            }
            
            Console.WriteLine($"IsSame: {IsSame(tree1, tree1)}");
            Console.WriteLine($"IsSame: {IsSame(tree1, tree2)}");

            Console.WriteLine();
        }

        private static bool IsSame(Tree<int> tree1, Tree<int> tree2)
        {
            if(tree1.Count != tree2.Count)
                throw new InvalidOperationException("To avoid a deadlock, both trees have to be equal.");

            // I've set the Bounded Capacities to 1 to ensure that 
            // we only produce and consume both requests at a time.

            var channel1 = new BlockingCollection<int>(1);
            PoormansTask.Run(() => tree1.Walk(value => channel1.Add(value)));

            var channel2 = new BlockingCollection<int>(1);
            PoormansTask.Run(() => tree2.Walk(value => channel2.Add(value)));

            for (var i = 0; i < tree1.Count; i++)
            {
                if (channel1.Take() != channel2.Take())
                    return false;
            }

            return true;
        }

        private static void RunPermutationExample()
        {
            IPermuter permuter = PermuterFactory.Get(PermutationType.BreadthFirstPrefixSearch);
            permuter.Execute("12345");

            Console.WriteLine();
        }

        private static void RunHeapSortExample()
        {
            Heap<int> heap = new Heap<int>(new Heap<int>.MinHeap());

            heap.Push(new List<int> { 1, 5, 1, 80, 45, 1, 1, 2, 3, 4, 5, 90, 4, 2, 54, 13 });

            Console.WriteLine("Heap");
            Console.WriteLine("====");
            Console.WriteLine(heap);

            Console.WriteLine();

            Console.WriteLine("Sorted");
            Console.WriteLine("======");

            while (heap.Size > 0)
            {
                Console.Write($"{heap.Peek()} ");
                heap.Pop();
            }

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("After Sorting");
            Console.WriteLine("=============");
            Console.WriteLine(heap.Size == 0 ? "Empty" : heap.ToString());
        }

        private static void RunDeadlockPreventionSimulationExample()
        {
            var deadlockPreventionSimulation = DeadlockPreventionSimulator.Get();
            deadlockPreventionSimulation.Run();

            Console.WriteLine();
        }

        private static void RunConnectionDriverBridgeExample()
        {
            // The driver manager provides the bridge between our connection abstraction
            // and driver implementations. The "Use" interface allows us to dynamically 
            // vary our connection drivers without changing our connection abstraction -- 
            // not even via inheritance ;)

            IDriverManager driverManager = DriverManager.GetInstance();

            driverManager.Use(new SqlServerConnectionDriver());

            using (IConnection connection = driverManager.GetConnection("tcp://127.0.0.1"))
            {
                connection.ExecuteQuery("select * from table");
                Console.WriteLine(connection.Result);
            }

            Console.WriteLine();

            driverManager.Use(new OracleConnectionDriver());

            using (IConnection connection = driverManager.GetConnection("tcp://192.168.1.8"))
            {
                connection.ExecuteQuery("select * from table");
                Console.WriteLine(connection.Result);
            }

            Console.WriteLine();
        }

        private static void RunPaymentProxyExample()
        {
            // This code is for demonstration purposes only and not production worthy,
            // as it does some things that could have been handled separately by other 
            // services. For example, several validations could have been done before 
            // this phase in order to reduce latency, increase throughput and promote 
            // correctness. Also, we aren't taking advantage of .NET's TPL APIs.

            IProcessor processorProxy = ProcessorProxy.Get();

            IEnumerable<Transaction> transactions = LoadPendingTransactionsFromDb();

            foreach (var transaction in transactions)
            {
                processorProxy.Withdraw(transaction);

                Console.WriteLine(
                    transaction.Status == TransactionStatus.Successful
                    ? "Payment successful"
                    : $"Payment failed. Reason {transaction.StatusMessage}"
                );

                // update transaction and inform notification service

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static IEnumerable<Transaction> LoadPendingTransactionsFromDb()
        {
            return new[]
            {
                new Transaction { Processor = Processor.MasterCard, Amount = 1000},
                new Transaction { Processor = Processor.Visa, Amount = 1000},
                new Transaction { Processor = Processor.AmericanExpress, Amount = 1000},
                new Transaction () // Invalid Processor: this could have been validated by a separate service
            };
        }
    }
}
