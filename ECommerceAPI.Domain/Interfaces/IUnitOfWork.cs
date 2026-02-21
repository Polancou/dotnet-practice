using System.Threading;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
