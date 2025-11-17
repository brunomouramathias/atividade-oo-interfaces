using System.Collections.Generic;

namespace Fase05RepositoryInMemory
{
    // Contrato genérico para acesso a dados
    public interface IRepository<T, TId>
    {
        T Add(T entity);
        T? GetById(TId id);
        IReadOnlyList<T> ListAll();
        bool Update(T entity);
        bool Remove(TId id);
    }
}

