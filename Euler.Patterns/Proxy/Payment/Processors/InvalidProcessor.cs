using Euler.Patterns.Proxy.Payment.Entities;
using System;

namespace Euler.Patterns.Proxy.Payment.Processors
{
    public class InvalidProcessor : IProcessor
    {
        private const string ErrorMessage = "Unable to connect to the requested processor";
        public decimal GetBalance(Transaction transaction)
        {
            Console.WriteLine($"{ErrorMessage}: {transaction.Processor}");
            return 0.0m;
        }

        public void Withdraw(Transaction transaction)
        {
            Console.WriteLine($"{ErrorMessage}: {transaction.Processor}");
        }

        public void Deposit(Transaction transaction)
        {
            Console.WriteLine($"{ErrorMessage}: {transaction.Processor}");
        }
    }
}