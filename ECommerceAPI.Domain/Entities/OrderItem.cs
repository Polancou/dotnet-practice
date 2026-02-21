using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Domain.Entities
{
    public class OrderItem : Entity
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public Money PriceAtPurchase { get; private set; }

        public OrderItem(Product product, int quantity, Money priceAtPurchase)
        {
            Product = product;
            Quantity = quantity;
            PriceAtPurchase = priceAtPurchase;
        }

        public Money CalculateTotal()
        {
            return new Money(PriceAtPurchase.Amount * Quantity, PriceAtPurchase.Currency);
        }
    }
}
