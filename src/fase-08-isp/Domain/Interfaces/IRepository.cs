namespace Fase08Isp.Domain.Interfaces
{
    // Interface completa: herda de ambas para compatibilidade
    public interface IRepository<T, TId> : IReadRepository<T, TId>, IWriteRepository<T, TId>
    {
        // Não precisa adicionar métodos, herda de ambas
    }
}

