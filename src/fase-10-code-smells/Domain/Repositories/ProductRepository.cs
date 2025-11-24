using System.Collections.Generic;
using System.Linq;
using Fase10CodeSmells.Domain.Entities;
using Fase10CodeSmells.Domain.Interfaces;

namespace Fase10CodeSmells.Domain.Repositories
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new List<Product>();

        public Product Add(Product product)
        {
            _products.Add(product);
            return product;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IReadOnlyList<Product> ListAll()
        {
            return _products;
        }
    }
}

