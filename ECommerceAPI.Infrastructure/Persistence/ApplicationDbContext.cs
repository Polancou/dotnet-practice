using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<PhysicalProduct> PhysicalProducts { get; set; } = null!;
        public DbSet<DigitalProduct> DigitalProducts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH (Table Per Hierarchy) mapping for products
            modelBuilder.Entity<Product>()
                .HasDiscriminator<string>("ProductType")
                .HasValue<PhysicalProduct>("Physical")
                .HasValue<DigitalProduct>("Digital");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Configure Encapsulated Collection
                entity.Metadata.FindNavigation(nameof(Order.Items))?
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            // Complex type / Value Objects in EF Core
            modelBuilder.Entity<Order>().OwnsMany(o => o.Items, item =>
            {
                item.WithOwner().HasForeignKey("OrderId");
                item.HasKey(i => i.Id);
                
                item.Property(i => i.PriceAtPurchase)
                    .HasConversion(
                        money => $"{money.Amount}|{money.Currency}",
                        value => new ECommerceAPI.Domain.ValueObjects.Money(
                            decimal.Parse(value.Split('|', StringSplitOptions.None)[0]), 
                            value.Split('|', StringSplitOptions.None)[1])
                    )
                    .HasColumnName("PriceAtPurchase");
            });

            modelBuilder.Entity<PhysicalProduct>().ComplexProperty(p => p.Price, p =>
            {
                p.Property(money => money.Amount).HasColumnType("decimal(18,2)");
                p.Property(money => money.Currency).HasMaxLength(3);
            });

            modelBuilder.Entity<DigitalProduct>().ComplexProperty(p => p.Price, p =>
            {
                p.Property(money => money.Amount).HasColumnType("decimal(18,2)");
                p.Property(money => money.Currency).HasMaxLength(3);
            });
        }
    }
}
