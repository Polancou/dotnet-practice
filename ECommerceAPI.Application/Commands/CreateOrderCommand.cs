using System;
using System.Collections.Generic;
using MediatR;
using ECommerceAPI.Application.DTOs;

namespace ECommerceAPI.Application.Commands
{
    // Using record for an immutable CQRS command
    public record CreateOrderCommand(Guid CustomerId, List<OrderItemDto> Items) : IRequest<Guid>;
}
