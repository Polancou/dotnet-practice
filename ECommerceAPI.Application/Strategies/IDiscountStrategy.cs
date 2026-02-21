using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Application.Strategies
{
    // Strategy pattern interface
    public interface IDiscountStrategy
    {
        Money CalculateDiscount(Order order);
    }
}
