using System.Threading;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Infrastructure.Persistence;

namespace ECommerceAPI.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // The unit of work commits all changes
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
