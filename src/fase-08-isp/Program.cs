using System;
using Fase08Isp.Domain.Entities;
using Fase08Isp.Domain.Repositories;
using Fase08Isp.Services;
using Fase08Isp.Tests;

namespace Fase08Isp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 8 - ISP (Interface Segregation Principle) ===\n");

            // Executar testes primeiro
            TestesIsp.ExecutarTodos();

            // Criar repositório
            var repository = new InMemoryBookRepository();

            // Cenário 1: Serviço completo (leitura + escrita)
            Console.WriteLine("--- Cenario 1: Servico Completo (IRepository) ---");
            var fullService = new BookService(repository);
            
            fullService.Register(new Book(1, "Código Limpo", "Robert C. Martin"));
            fullService.Register(new Book(2, "Domain-Driven Design", "Eric Evans"));
            fullService.Register(new Book(3, "Refactoring", "Martin Fowler"));
            
            Console.WriteLine("Livros cadastrados:");
            foreach (var book in fullService.ListAll())
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 2: Serviço somente leitura (IReadRepository)
            Console.WriteLine("--- Cenario 2: Servico Somente Leitura (IReadRepository) ---");
            var readOnlyService = new ReadOnlyBookService(repository);
            
            Console.WriteLine($"Total de livros: {readOnlyService.CountBooks()}");
            Console.WriteLine($"Livro ID 2 existe? {readOnlyService.BookExists(2)}");
            Console.WriteLine($"Livro ID 99 existe? {readOnlyService.BookExists(99)}");
            
            var book1 = readOnlyService.FindById(1);
            if (book1 != null)
            {
                Console.WriteLine($"Encontrado: {book1.Title}");
            }
            Console.WriteLine();

            // Cenário 3: Benefícios do ISP
            Console.WriteLine("--- Cenario 3: Beneficios do ISP ---");
            Console.WriteLine("✓ ReadOnlyService não tem acesso a métodos de escrita");
            Console.WriteLine("✓ Compilador garante que não pode modificar dados");
            Console.WriteLine("✓ Interfaces menores e mais coesas");
            Console.WriteLine("✓ Clientes dependem apenas do que precisam");
            Console.WriteLine("✓ Facilita testes (pode mockar apenas IReadRepository)");
            Console.WriteLine();

            // Cenário 4: Estrutura de interfaces
            Console.WriteLine("--- Cenario 4: Estrutura de Interfaces ---");
            Console.WriteLine("IReadRepository<T, TId>");
            Console.WriteLine("  ├── GetById(TId id)");
            Console.WriteLine("  └── ListAll()");
            Console.WriteLine();
            Console.WriteLine("IWriteRepository<T, TId>");
            Console.WriteLine("  ├── Add(T entity)");
            Console.WriteLine("  ├── Update(T entity)");
            Console.WriteLine("  └── Remove(TId id)");
            Console.WriteLine();
            Console.WriteLine("IRepository<T, TId> : IReadRepository, IWriteRepository");
            Console.WriteLine("  └── (herda todos os métodos)");
            Console.WriteLine();

            // Cenário 5: Demonstração de segregação
            Console.WriteLine("--- Cenario 5: Demonstracao de Segregacao ---");
            
            // Atualização através do serviço completo
            fullService.UpdateBook(new Book(1, "Código Limpo (2ª Edição)", "Robert C. Martin"));
            Console.WriteLine("Atualizado via BookService (IRepository)");
            
            // Consulta através do serviço de leitura
            var updated = readOnlyService.FindById(1);
            if (updated != null)
            {
                Console.WriteLine($"Consultado via ReadOnlyService: {updated.Title}");
            }
            
            Console.WriteLine("\n✓ Mesmo repositório, interfaces diferentes!");
            Console.WriteLine("✓ ISP permite uso flexível conforme necessidade do cliente");
        }
    }
}
