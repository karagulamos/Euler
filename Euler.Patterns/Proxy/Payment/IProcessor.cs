using Euler.Patterns.Proxy.Payment.Entities;

namespace Euler.Patterns.Proxy.Payment
{
    public interface IProcessor
    {
        decimal GetBalance(Transaction transaction);
        void Withdraw(Transaction transaction);
        void Deposit(Transaction transaction);
    }
}