using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Moq;
using ECommerceAPI.Application.Commands;
using ECommerceAPI.Application.DTOs;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Tests.Application
{
    public class CreateOrderCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldCreateOrder_WhenProductsExist()
        {
            // Arrange
            var orderRepoMock = new Mock<IRepository<Order>>();
            var productRepoMock = new Mock<IRepository<Product>>();
            var uowMock = new Mock<IUnitOfWork>();

            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var command = new CreateOrderCommand(customerId, new List<OrderItemDto>
            {
                new OrderItemDto(productId, 2)
            });

            var handler = new CreateOrderCommandHandler(
                orderRepoMock.Object, 
                productRepoMock.Object, 
                uowMock.Object);

            var product = new PhysicalProduct("Phone", new Money(500m, "USD"), 0.5m, "10x5x1");
            // Reflection used to set the base class Id here just for testing
            var idProp = typeof(ECommerceAPI.Domain.Common.Entity).GetProperty("Id");
            idProp?.SetValue(product, productId, null);

            productRepoMock.Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var orderId = await handler.Handle(command, CancellationToken.None);

            // Assert
            orderId.Should().NotBeEmpty();
            orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
            uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var orderRepoMock = new Mock<IRepository<Order>>();
            var productRepoMock = new Mock<IRepository<Product>>();
            var uowMock = new Mock<IUnitOfWork>();

            var command = new CreateOrderCommand(Guid.NewGuid(), new List<OrderItemDto>
            {
                new OrderItemDto(Guid.NewGuid(), 1)
            });

            var handler = new CreateOrderCommandHandler(
                orderRepoMock.Object, 
                productRepoMock.Object, 
                uowMock.Object);

            productRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Product)null!);

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Product with ID * not found.");
                
            uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
