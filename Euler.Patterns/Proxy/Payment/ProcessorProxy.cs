// Author: Alexander Karagulamos
// Date:
// Comment:

using Euler.Patterns.Proxy.Payment.Entities;
using Euler.Patterns.Proxy.Payment.Enums;
using Euler.Patterns.Proxy.Payment.Processors;

namespace Euler.Patterns.Proxy.Payment
{
    public class ProcessorProxy : IProcessor
    {
        public decimal GetBalance(Transaction transaction)
        {
            var processor = GetProcessor(transaction.Processor);
            return processor.GetBalance(transaction);
        }

        public void Withdraw(Transaction transaction)
        {
            var processor = GetProcessor(transaction.Processor);

            var balance = processor.GetBalance(transaction);

            if (balance < transaction.Amount)
            {
                transaction.StatusMessage = "Insufficient balance.";
                transaction.Status = TransactionStatus.Failed;;
                return;
            }

            processor.Withdraw(transaction);
        }

        public void Deposit(Transaction transaction)
        {
            var processor = GetProcessor(transaction.Processor);
            processor.Deposit(transaction);
        }

        public static IProcessor Create() => new ProcessorProxy();

        private static IProcessor GetProcessor(Processor processor)
        {
            switch (processor)
            {
                case Processor.AmericanExpress:
                    return new AmericanExpress();
                case Processor.MasterCard:
                    return new MasterCard();
                case Processor.Visa:
                    return new Visa();
                default:
                    return new InvalidProcessor();
            }
        }
    }
}
