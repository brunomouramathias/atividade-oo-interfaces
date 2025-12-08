using System;
using System.IO;
using Fase06RepositoryCsv.Domain;
using Fase06RepositoryCsv.Domain.Interfaces;
using Fase06RepositoryCsv.Repository;
using Fase06RepositoryCsv.Services;

namespace Fase06RepositoryCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 6 - REPOSITORY CSV ===\n");

            // Caminho do arquivo CSV
            var path = Path.Combine(AppContext.BaseDirectory, "books.csv");
            Console.WriteLine($"Arquivo CSV: {path}\n");

            // Composição: Criação do Repository para Book (CSV)
            IRepository<Book, int> repo = new CsvBookRepository(path);

            // Cenário 1: Cadastro de livros
            Console.WriteLine("--- Cadastro de Livros ---");
            BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
            BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
            BookService.Register(repo, new Book(3, "Refactoring", "Martin Fowler"));
            Console.WriteLine("3 livros cadastrados com sucesso!\n");

            // Cenário 2: Listagem de todos os livros
            Console.WriteLine("--- Listagem de Livros (CSV) ---");
            var all = BookService.ListAll(repo);
            Console.WriteLine($"Total de livros: {all.Count}");
            foreach (var book in all)
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 3: Teste com vírgulas e aspas
            Console.WriteLine("--- Campos com Vírgulas e Aspas ---");
            BookService.Register(repo, new Book(10, "Livro, com vírgula", "Autor \"com aspas\""));
            var special = BookService.FindById(repo, 10);
            if (special != null)
            {
                Console.WriteLine($"Livro especial cadastrado:");
                Console.WriteLine($"  Título: {special.Title}");
                Console.WriteLine($"  Autor: {special.Author}");
            }
            Console.WriteLine();

            // Cenário 4: Atualização
            Console.WriteLine("--- Atualização ---");
            bool updated = BookService.UpdateBook(repo, new Book(1, "Código Limpo (2ª Edição)", "Robert C. Martin"));
            Console.WriteLine($"Atualização do livro ID 1: {(updated ? "Sucesso" : "Falhou")}");
            
            var updatedBook = BookService.FindById(repo, 1);
            if (updatedBook != null)
                Console.WriteLine($"Livro atualizado: {updatedBook.Title}");
            Console.WriteLine();

            // Cenário 5: Remoção
            Console.WriteLine("--- Remoção ---");
            bool removed = BookService.RemoveBook(repo, 3);
            Console.WriteLine($"Remoção do livro ID 3: {(removed ? "Sucesso" : "Falhou")}");
            Console.WriteLine($"Total de livros após remoção: {BookService.ListAll(repo).Count}\n");

            // Cenário 6: Mostrar conteúdo do arquivo CSV
            Console.WriteLine("--- Conteúdo do Arquivo CSV ---");
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                    Console.WriteLine($"  {line}");
            }

            Console.WriteLine("\n=== Para executar os testes, rode o projeto Fase06RepositoryCsv.Tests ===");
        }
    }
}
