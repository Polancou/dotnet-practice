using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Domain.Entities
{
    public abstract class Product : Entity
    {
        public string Name { get; private set; }
        public Money Price { get; private set; }

        protected Product(string name, Money price)
        {
            Name = name;
            Price = price;
        }

        // Abstract method to be implemented by derived classes (Polymorphism)
        public abstract Money CalculateShippingCost();
    }
}
