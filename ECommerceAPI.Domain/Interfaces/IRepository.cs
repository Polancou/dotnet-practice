using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceAPI.Domain.Common;

namespace ECommerceAPI.Domain.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
