using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;

namespace ECommerceAPI.Application.Commands
{
    // Sealed class because we don't expect/want anyone to inherit from this specific handler
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IRepository<Order> orderRepository, 
            IRepository<Product> productRepository, 
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order(request.CustomerId);

            foreach (var itemDto in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {itemDto.ProductId} not found.");
                }

                // Encapsulation: we add it to the order rather than setting properties
                order.AddLineItem(product, itemDto.Quantity);
            }

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}
