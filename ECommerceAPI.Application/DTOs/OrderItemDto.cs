using System;

namespace ECommerceAPI.Application.DTOs
{
    // Using record for immutable DTOs
    public record OrderItemDto(Guid ProductId, int Quantity);
}
