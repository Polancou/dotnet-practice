using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Domain.Entities
{
    public class PhysicalProduct : Product
    {
        public decimal WeightInKg { get; private set; }
        public string Dimensions { get; private set; }

        public PhysicalProduct(string name, Money price, decimal weightInKg, string dimensions) 
            : base(name, price)
        {
            WeightInKg = weightInKg;
            Dimensions = dimensions;
        }

        // Overriding the abstract method (Polymorphism)
        public override Money CalculateShippingCost()
        {
            // Simple logic: base cost of 5.00 plus 2.00 per Kg
            decimal shippingCostAmount = 5.00m + (WeightInKg * 2.00m);
            return new Money(shippingCostAmount, Price.Currency);
        }
    }
}
