using System;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Application.Strategies
{
    // Concrete strategy 1: No discount
    public sealed class NoDiscountStrategy : IDiscountStrategy
    {
        public Money CalculateDiscount(Order order)
        {
            return Money.Zero();
        }
    }

    // Concrete strategy 2: Fixed amount discount
    public sealed class FixedAmountDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _discountAmount;

        public FixedAmountDiscountStrategy(decimal discountAmount)
        {
            _discountAmount = discountAmount;
        }

        public Money CalculateDiscount(Order order)
        {
            var total = order.CalculateTotal();
            if (total.Amount < _discountAmount)
            {
                return Money.Zero(total.Currency);
            }

            return new Money(_discountAmount, total.Currency);
        }
    }

    // Concrete strategy 3: Percentage discount
    public sealed class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _percentage; // e.g. 0.10 for 10%

        public PercentageDiscountStrategy(decimal percentage)
        {
            if (percentage < 0 || percentage > 1)
            {
                throw new ArgumentException("Percentage must be between 0 and 1");
            }

            _percentage = percentage;
        }

        public Money CalculateDiscount(Order order)
        {
            var total = order.CalculateTotal();
            var discountAmount = total.Amount * _percentage;

            return new Money(discountAmount, total.Currency);
        }
    }
}
