using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void AddLineItem_ShouldAddProductToOrder_WhenOrderIsPending()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var order = new Order(customerId);
            
            var product = new PhysicalProduct("Laptop", new Money(1000m, "USD"), 2.5m, "30x20x2");

            // Act
            order.AddLineItem(product, 1);

            // Assert
            order.Items.Should().HaveCount(1);
            order.Items.First().Product.Name.Should().Be("Laptop");
            order.CalculateTotal().Amount.Should().Be(1000m);
        }

        [Fact]
        public void CompleteOrder_ShouldTriggerEvent_AndChangeStatus()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            bool eventTriggered = false;
            
            order.OnOrderCompleted += (sender, args) => 
            {
                eventTriggered = true;
                args.Order.Id.Should().Be(order.Id);
            };

            // Act
            order.CompleteOrder();

            // Assert
            order.Status.Should().Be(ECommerceAPI.Domain.Enums.OrderStatus.Delivered);
            eventTriggered.Should().BeTrue();
        }

        [Fact]
        public void Polymorphism_DigitalProduct_ShouldHaveZeroShipping()
        {
            // Arrange
            Product product = new DigitalProduct("E-Book", new Money(15m, "USD"), "http://download.link");

            // Act
            var shipping = product.CalculateShippingCost();

            // Assert
            shipping.Amount.Should().Be(0m);
        }

        [Fact]
        public void Polymorphism_PhysicalProduct_ShouldCalculateShipping()
        {
            // Arrange
            // 5 base + (2.5 * 2) = 10
            Product product = new PhysicalProduct("Laptop", new Money(1000m, "USD"), 2.5m, "30x20x2");

            // Act
            var shipping = product.CalculateShippingCost();

            // Assert
            shipping.Amount.Should().Be(10m);
        }

        [Fact]
        public void ApplyTax_ShouldModifyTotalByReference()
        {
            // Arrange
            var order = new Order(Guid.NewGuid());
            decimal currentTotal = 100m;
            decimal taxRate = 0.10m;

            // Act - Passing by reference
            order.ApplyTax(ref currentTotal, taxRate);

            // Assert
            currentTotal.Should().Be(110m);
        }
    }
}
