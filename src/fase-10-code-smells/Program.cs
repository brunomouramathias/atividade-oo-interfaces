using System;
using Fase10CodeSmells.Domain.Entities;
using Fase10CodeSmells.Domain.Repositories;

namespace Fase10CodeSmells
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 10 - CODE SMELLS E REFATORAÇÕES ===\n");
            Console.WriteLine("Demonstração de código antes e depois de refatorações\n");
            Console.WriteLine("Ver CODE-SMELLS.md para exemplos completos\n");
            
            var repo = new ProductRepository();
            repo.Add(new Product(1, "Produto A", 100m, 50));
            repo.Add(new Product(2, "Produto B", 200m, 30));
            
            Console.WriteLine("Produtos cadastrados:");
            foreach (var product in repo.ListAll())
            {
                Console.WriteLine($"  #{product.Id} - {product.Name} - R${product.Price}");
            }
        }
    }
}
