using System;

namespace ECommerceAPI.Infrastructure.Configuration
{
    // Sealed class representing a Singleton Configuration provider
    public sealed class AppConfiguration
    {
        private static readonly Lazy<AppConfiguration> _instance = 
            new Lazy<AppConfiguration>(() => new AppConfiguration());

        // Private constructor prevents external instantiation
        private AppConfiguration()
        {
            // Initialize defaults
            DefaultCurrency = "USD";
        }

        public static AppConfiguration Instance => _instance.Value;

        public string DefaultCurrency { get; set; }
    }
}
