using System;

namespace Fase05RepositoryInMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 5 - REPOSITORY IN MEMORY ===\n");

            // Executar testes primeiro
            TestesRepositorio.ExecutarTodos();

            // Composição: Criação do Repository para Book
            IRepository<Book, int> repo = new InMemoryRepository<Book, int>(book => book.Id);

            // Cenário 1: Cadastro de livros
            Console.WriteLine("--- Cenario 1: Cadastro de Livros ---");
            BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
            BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
            BookService.Register(repo, new Book(3, "Refactoring", "Martin Fowler"));
            Console.WriteLine("3 livros cadastrados com sucesso!\n");

            // Cenário 2: Listagem de todos os livros
            Console.WriteLine("--- Cenario 2: Listagem de Livros ---");
            var all = BookService.ListAll(repo);
            Console.WriteLine($"Total de livros: {all.Count}");
            foreach (var book in all)
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 3: Busca por ID
            Console.WriteLine("--- Cenario 3: Busca por ID ---");
            var found = BookService.FindById(repo, 2);
            if (found != null)
            {
                Console.WriteLine($"Livro encontrado: #{found.Id} - {found.Title} ({found.Author})");
            }
            
            var notFound = BookService.FindById(repo, 99);
            Console.WriteLine($"Livro ID 99: {(notFound == null ? "Não encontrado" : "Encontrado")}\n");

            // Cenário 4: Atualização
            Console.WriteLine("--- Cenario 4: Atualizacao ---");
            bool updated = BookService.UpdateBook(repo, new Book(1, "Código Limpo (2ª Edição)", "Robert C. Martin"));
            Console.WriteLine($"Atualização do livro ID 1: {(updated ? "Sucesso" : "Falhou")}");
            
            var updatedBook = BookService.FindById(repo, 1);
            if (updatedBook != null)
            {
                Console.WriteLine($"Livro atualizado: {updatedBook.Title}");
            }
            Console.WriteLine();

            // Cenário 5: Remoção
            Console.WriteLine("--- Cenario 5: Remocao ---");
            bool removed = BookService.RemoveBook(repo, 3);
            Console.WriteLine($"Remoção do livro ID 3: {(removed ? "Sucesso" : "Falhou")}");
            Console.WriteLine($"Total de livros após remoção: {BookService.ListAll(repo).Count}\n");

            // Cenário 6: Operações em lote
            Console.WriteLine("--- Cenario 6: Operacoes em Lote ---");
            for (int i = 10; i <= 15; i++)
            {
                BookService.Register(repo, new Book(i, $"Livro {i}", $"Autor {i}"));
            }
            Console.WriteLine($"Cadastrados mais 6 livros. Total: {BookService.ListAll(repo).Count}");
            
            Console.WriteLine("\nListagem final:");
            foreach (var book in BookService.ListAll(repo))
            {
                Console.WriteLine($"  #{book.Id} - {book.Title}");
            }
        }
    }
}
