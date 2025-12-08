using System;
using Fase05RepositoryInMemory.Domain.Entities;
using Fase05RepositoryInMemory.Domain.Repositories;
using Fase05RepositoryInMemory.Services;

namespace Fase05RepositoryInMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 5 - REPOSITORY INMEMORY ===\n");

            // Cria repositório InMemory
            var repository = new InMemoryRepository<Book, int>(book => book.Id);
            var service = new BookService(repository);

            // Demonstração de operações CRUD
            Console.WriteLine("--- Cadastro de Livros ---\n");

            service.Register(new Book(1, "Código Limpo", "Robert C. Martin"));
            service.Register(new Book(2, "O Programador Pragmático", "David Thomas"));
            service.Register(new Book(3, "Design Patterns", "Gang of Four"));

            Console.WriteLine("Livros cadastrados:");
            foreach (var book in service.ListAll())
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} por {book.Author}");
            }

            // Busca por ID
            Console.WriteLine("\n--- Busca por ID ---\n");
            var found = service.FindById(2);
            Console.WriteLine($"Livro #2: {found?.Title ?? "Não encontrado"}");

            var notFound = service.FindById(99);
            Console.WriteLine($"Livro #99: {notFound?.Title ?? "Não encontrado"}");

            // Atualização
            Console.WriteLine("\n--- Atualização ---\n");
            var updated = service.UpdateBook(new Book(1, "Código Limpo (2ª Edição)", "Robert C. Martin"));
            Console.WriteLine($"Livro #1 atualizado: {updated}");

            var bookUpdated = service.FindById(1);
            Console.WriteLine($"Novo título: {bookUpdated?.Title}");

            // Remoção
            Console.WriteLine("\n--- Remoção ---\n");
            var removed = service.RemoveBook(3);
            Console.WriteLine($"Livro #3 removido: {removed}");
            Console.WriteLine($"Total de livros: {service.ListAll().Count}");

            Console.WriteLine("\n=== DEMONSTRAÇÃO CONCLUÍDA ===");
        }
    }
}
