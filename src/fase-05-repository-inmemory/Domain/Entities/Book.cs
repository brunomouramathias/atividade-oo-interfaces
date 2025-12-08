namespace Fase05RepositoryInMemory.Domain.Entities
{
    /// <summary>
    /// Entidade de domínio: Livro.
    /// Record imutável para garantir integridade dos dados.
    /// </summary>
    public sealed record Book(int Id, string Title, string Author);
}
