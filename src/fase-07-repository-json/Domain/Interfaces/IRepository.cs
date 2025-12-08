using System.Collections.Generic;

namespace Fase07RepositoryJson.Domain.Interfaces
{
    /// <summary>
    /// Contrato genérico para acesso a dados (padrão Repository).
    /// </summary>
    public interface IRepository<T, TId>
    {
        T Add(T entity);
        T? GetById(TId id);
        IReadOnlyList<T> ListAll();
        bool Update(T entity);
        bool Remove(TId id);
    }
}
