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

            // Demonstração do repositório refatorado
            var repo = new ProductRepository();
            repo.Add(new Product(1, "Produto A", 100m, 50));
            repo.Add(new Product(2, "Produto B", 200m, 30));
            repo.Add(new Product(3, "Produto C", 150m, 0));
            
            Console.WriteLine("--- Produtos Cadastrados ---");
            foreach (var product in repo.ListAll())
            {
                Console.WriteLine($"  #{product.Id} - {product.Name} - R${product.Price} (Estoque: {product.Stock})");
            }
            Console.WriteLine();

            // Exemplos de Code Smells e Refatorações aplicadas
            Console.WriteLine("--- Code Smells Identificados e Corrigidos ---\n");

            Console.WriteLine("1. MAGIC NUMBERS → CONSTANTES NOMEADAS");
            Console.WriteLine("   Antes:  if (stock < 10) ...");
            Console.WriteLine("   Depois: if (stock < MIN_STOCK_THRESHOLD) ...\n");

            Console.WriteLine("2. LONG METHOD → EXTRACT METHOD");
            Console.WriteLine("   Antes:  Um método com 50+ linhas");
            Console.WriteLine("   Depois: Vários métodos pequenos e focados\n");

            Console.WriteLine("3. DUPLICATE CODE → GENERICS/ABSTRAÇÃO");
            Console.WriteLine("   Antes:  BookRepository, ProductRepository separados");
            Console.WriteLine("   Depois: GenericRepository<T, TId>\n");

            Console.WriteLine("4. PRIMITIVE OBSESSION → VALUE OBJECTS");
            Console.WriteLine("   Antes:  decimal price; int quantity;");
            Console.WriteLine("   Depois: Money price; Quantity quantity;\n");

            Console.WriteLine("5. FEATURE ENVY → MOVE METHOD");
            Console.WriteLine("   Antes:  Service manipulando dados internos da Entity");
            Console.WriteLine("   Depois: Lógica movida para a própria Entity\n");

            // Demonstração de produto com estoque baixo
            Console.WriteLine("--- Verificação de Estoque ---");
            foreach (var product in repo.ListAll())
            {
                if (product.Stock == 0)
                    Console.WriteLine($"  ⚠ {product.Name}: SEM ESTOQUE");
                else if (product.Stock < 35)
                    Console.WriteLine($"  ⚠ {product.Name}: Estoque baixo ({product.Stock} unidades)");
                else
                    Console.WriteLine($"  ✓ {product.Name}: OK ({product.Stock} unidades)");
            }

            Console.WriteLine("\n=== REFATORAÇÕES APLICADAS COM SUCESSO ===");
        }
    }
}
