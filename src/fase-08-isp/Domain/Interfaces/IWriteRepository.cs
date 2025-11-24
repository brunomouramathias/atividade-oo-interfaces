namespace Fase08Isp.Domain.Interfaces
{
    // Interface segregada: apenas operações de escrita
    public interface IWriteRepository<T, TId>
    {
        T Add(T entity);
        bool Update(T entity);
        bool Remove(TId id);
    }
}

