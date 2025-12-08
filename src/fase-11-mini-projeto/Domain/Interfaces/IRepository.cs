using System.Collections.Generic;

namespace Fase11MiniProjeto.Domain.Interfaces
{
    /// <summary>
    /// Interface segregada: apenas operações de leitura.
    /// Aplica o princípio ISP (Interface Segregation Principle).
    /// </summary>
    public interface IReadRepository<T, TId>
    {
        T? GetById(TId id);
        IReadOnlyList<T> ListAll();
    }

    /// <summary>
    /// Interface segregada: apenas operações de escrita.
    /// </summary>
    public interface IWriteRepository<T, TId>
    {
        T Add(T entity);
        bool Update(T entity);
        bool Remove(TId id);
    }

    /// <summary>
    /// Interface completa: herda leitura e escrita.
    /// </summary>
    public interface IRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
    {
    }
}
