using System.Collections.Generic;

namespace Fase05RepositoryInMemory.Domain.Interfaces
{
    /// <summary>
    /// Contrato genérico para acesso a dados (padrão Repository).
    /// Define operações CRUD básicas independentes da implementação.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade.</typeparam>
    /// <typeparam name="TId">Tipo do identificador.</typeparam>
    public interface IRepository<T, TId>
    {
        /// <summary>
        /// Adiciona uma entidade ao repositório.
        /// </summary>
        T Add(T entity);

        /// <summary>
        /// Obtém uma entidade pelo identificador.
        /// </summary>
        T? GetById(TId id);

        /// <summary>
        /// Lista todas as entidades do repositório.
        /// </summary>
        IReadOnlyList<T> ListAll();

        /// <summary>
        /// Atualiza uma entidade existente.
        /// </summary>
        /// <returns>True se a entidade foi encontrada e atualizada.</returns>
        bool Update(T entity);

        /// <summary>
        /// Remove uma entidade pelo identificador.
        /// </summary>
        /// <returns>True se a entidade foi encontrada e removida.</returns>
        bool Remove(TId id);
    }
}
