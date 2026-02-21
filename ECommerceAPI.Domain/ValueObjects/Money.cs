using System;

namespace ECommerceAPI.Domain.ValueObjects
{
    public readonly struct Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency must be specified.", nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot add differnt currencies");

            return new Money(a.Amount + b.Amount, a.Currency);
        }

        public static Money Zero(string currency = "USD") => new Money(0, currency);

        public override string ToString() => $"{Amount:0.00} {Currency}";
    }
}
