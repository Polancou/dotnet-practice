using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Infrastructure.Persistence;
using ECommerceAPI.Infrastructure.Repositories;
using ECommerceAPI.Domain.Common;

namespace ECommerceAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // Configure DbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Ensure memory cache is registered for the decorator
            services.AddMemoryCache();

            // Register standard repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Decorate generic repositories with cache
            services.Decorate(typeof(IRepository<>), typeof(CachedRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
