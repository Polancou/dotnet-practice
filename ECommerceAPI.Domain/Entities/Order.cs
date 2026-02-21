using System;
using System.Collections.Generic;
using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.Enums;
using ECommerceAPI.Domain.Events;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Domain.Entities
{
    // Define the delegate for the event
    public delegate void OrderCompletedHandler(object sender, OrderEventArgs e);

    public class Order : Entity
    {
        public Guid CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        
        // Encapsulation: The collection is protected from external manipulation
        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        // The Event (using the delegate)
        public event OrderCompletedHandler? OnOrderCompleted;

        public Order(Guid customerId)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            CreatedAt = DateTime.UtcNow;
        }

        // Behavior/State mutation via encapsulated methods
        public void AddLineItem(Product product, int quantity)
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot add items to an order that is not pending.");

            var item = new OrderItem(product, quantity, product.Price);
            _items.Add(item);
        }

        public Money CalculateTotal()
        {
            decimal totalAmount = 0m;
            string currency = "USD"; // Defaulting for simplicity

            foreach (var item in _items)
            {
                totalAmount += item.CalculateTotal().Amount;
                currency = item.CalculateTotal().Currency;
            }

            return new Money(totalAmount, currency);
        }

        // Using 'ref' keyword to demonstrate modifying a value type by reference
        public void ApplyTax(ref decimal currentTotal, decimal taxRate)
        {
            currentTotal += currentTotal * taxRate;
        }

        public void CompleteOrder()
        {
            if (Status == OrderStatus.Delivered)
                throw new InvalidOperationException("Order is already completed.");

            Status = OrderStatus.Delivered;

            // Trigger the event
            OnOrderCompleted?.Invoke(this, new OrderEventArgs(this));
        }
    }
}
