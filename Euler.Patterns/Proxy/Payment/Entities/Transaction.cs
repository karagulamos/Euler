using Euler.Patterns.Proxy.Payment.Enums;
using System;

namespace Euler.Patterns.Proxy.Payment.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public TransactionChannel Channel { get; set; }
        public PaymentType PaymentType { get; set; }
        public Processor Processor { get; set; }
        public TransactionStatus Status { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public DateTime Created { get; set; }
        public DateTime Completed { get; set; }
    }
}