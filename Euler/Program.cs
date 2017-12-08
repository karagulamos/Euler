using Euler.Algorithms.Permutations;
using Euler.Concurrency.DeadlockPrevention;
using Euler.Patterns.Bridge.Drivers;
using Euler.Patterns.Proxy.Payment;
using Euler.Patterns.Proxy.Payment.Entities;
using Euler.Patterns.Proxy.Payment.Enums;
using System;
using System.Collections.Generic;

namespace Euler
{
    public partial class Test
    {
        public static void Main()
        {
            //// Algorithms ////

            //RunPermutationExample();

            ///// Design Patterns ////

            // RunConnectionDriverBridgeExample();

            //RunPaymentProxyExample();

            //// Concurreny ////

            RunDeadlockPreventionSimulationExample();

            Console.ReadLine();
        }

        private static void RunPermutationExample()
        {
            IPermuter permuter = PermuterFactory.Get(PermutationType.Lexicographical);
            permuter.Execute("2142");

            Console.WriteLine();
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
