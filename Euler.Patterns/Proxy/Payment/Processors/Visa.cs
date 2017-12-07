using Euler.Patterns.Proxy.Payment.Entities;
using Euler.Patterns.Proxy.Payment.Enums;
using System;

namespace Euler.Patterns.Proxy.Payment.Processors
{
    internal class Visa : IProcessor
    {
        public decimal GetBalance(Transaction transaction)
        {
            Console.WriteLine($"Getting balance via {transaction.Processor}");
            return 300m;
        }

        public void Withdraw(Transaction transaction)
        {
            Console.WriteLine($"Withdrawing via {transaction.Processor}");

            transaction.Status = TransactionStatus.Successful;
            transaction.StatusMessage = "Successful";
        }

        public void Deposit(Transaction transaction)
        {
            Console.WriteLine($"Depositing via {transaction.Processor}");
        }
    }
}