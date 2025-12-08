using System.Collections.Generic;
using Fase10CodeSmells.Domain.Entities;

namespace Fase10CodeSmells.Domain.Interfaces
{
    public interface IProductRepository
    {
        Product? GetById(int id);
        IReadOnlyList<Product> ListAll();
        Product Add(Product product);
    }
}


