using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fase09DublesAvancados.Domain.Interfaces
{
    // Interface com métodos assíncronos
    public interface IAsyncRepository<T, TId>
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(TId id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(TId id);
    }
}

