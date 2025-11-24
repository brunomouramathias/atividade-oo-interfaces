using System.Collections.Generic;

namespace Fase08Isp.Domain.Interfaces
{
    // Interface segregada: apenas operações de leitura
    public interface IReadRepository<T, TId>
    {
        T? GetById(TId id);
        IReadOnlyList<T> ListAll();
    }
}

