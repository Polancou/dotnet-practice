using ECommerceAPI.Domain.Common;
using ECommerceAPI.Domain.ValueObjects;

namespace ECommerceAPI.Domain.Entities
{
    public class DigitalProduct : Product
    {
        public string DownloadLink { get; private set; }

        public DigitalProduct(string name, Money price, string downloadLink) 
            : base(name, price)
        {
            DownloadLink = downloadLink;
        }

        // Overriding the abstract method (Polymorphism)
        public override Money CalculateShippingCost()
        {
            // Digital products have no shipping cost
            return Money.Zero(Price.Currency);
        }
    }
}
