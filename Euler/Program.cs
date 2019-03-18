using Euler.Algorithms.Encoding;
using Euler.Algorithms.Permutations;
using Euler.Concurrency.DeadlockPrevention;
using Euler.Concurrency.PoormansTPL;
using Euler.DataStructures.BinaryTree;
using Euler.DataStructures.Heap;
using Euler.DataStructures.Stack.RPNCalculator;
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

            RunRpnCalculatorExample();

            // RunGoRotEncoderExercise();

            // RunGoConcurrentTreeWalkExercise();

            //******** Concurreny ********//

            //RunDeadlockPreventionSimulationExample();

            //******** Design Patterns ********//

            // RunConnectionDriverBridgeExample();

            // RunPaymentProxyExample();

            Console.ReadLine();
        }

        private static void RunRpnCalculatorExample()
        {
            Console.WriteLine("Enter Infix Expression: ");

            var infix = Calculator.FormatExpression(Console.ReadLine());

            Console.WriteLine("Infix: " + infix);

            var converter = new InfixToPostfixConverter();
            var postfix = converter.Convert(infix);

            Console.WriteLine("Postfix: " + Calculator.ConvertToString(postfix));

            var calculator = Calculator.Create(CalculatorType.Postfix);
            var result = calculator.Evaluate(postfix);

            Console.WriteLine("Result: " + result);
        }

        private static void RunGoRotEncoderExercise()
        {
            // https://tour.golang.org/methods/23

            var encoder = RotEnconder.Create(13);
            Console.WriteLine(encoder.Transform("Lbh penpxrq gur pbqr!"));

            Console.WriteLine();
        }

        private static void RunGoConcurrentTreeWalkExercise()
        {
            // https://tour.golang.org/concurrency/8

            var tree1 = Tree<int>.Create();
            var tree2 = Tree<int>.Create();

            for (var i = 1; i <= 1000; i++)
            {
                tree1.Add(i);
                tree2.Add(i << 1);
            }

            Console.WriteLine($"IsSame: {IsSame(tree1, tree1)}");
            Console.WriteLine($"IsSame: {IsSame(tree1, tree2)}");

            var tree3 = Tree<int>.Create(tree2);

            Console.WriteLine($"IsSame: {IsSame(tree2, tree3)}");

            Console.WriteLine();
        }

        private static bool IsSame(Tree<int> tree1, Tree<int> tree2)
        {
            if (tree1.Count != tree2.Count)
                throw new ArgumentException("Both trees have to be equal. Otherwise, this code will deadlock.");

            var channel1 = new BlockingCollection<int>(1);
            PoormansTask.Run(() => tree1.Walk(item => channel1.Add(item)));

            var channel2 = new BlockingCollection<int>(1);
            PoormansTask.Run(() => tree2.Walk(item => channel2.Add(item)));

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

            using (IConnection connection = driverManager.CreateConnection("tcp://127.0.0.1"))
            {
                connection.ExecuteQuery("select * from table");
                Console.WriteLine(connection.Result);
            }

            Console.WriteLine();

            driverManager.Use(new OracleConnectionDriver());

            using (IConnection connection = driverManager.CreateConnection("tcp://192.168.1.8"))
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

            IProcessor processorProxy = ProcessorProxy.Create();

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
