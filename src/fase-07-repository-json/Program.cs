using System;
using System.IO;
using Fase07RepositoryJson.Domain;
using Fase07RepositoryJson.Domain.Interfaces;
using Fase07RepositoryJson.Repository;
using Fase07RepositoryJson.Services;

namespace Fase07RepositoryJson
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 7 - REPOSITORY JSON (System.Text.Json) ===\n");

            // Caminho do arquivo JSON
            var path = Path.Combine(AppContext.BaseDirectory, "books.json");
            Console.WriteLine($"Arquivo JSON: {path}\n");

            // Composição: Criação do Repository para Book (JSON)
            IRepository<Book, int> repo = new JsonBookRepository(path);

            // Cenário 1: Cadastro de livros
            Console.WriteLine("--- Cadastro de Livros ---");
            BookService.Register(repo, new Book(1, "Código Limpo", "Robert C. Martin"));
            BookService.Register(repo, new Book(2, "Domain-Driven Design", "Eric Evans"));
            BookService.Register(repo, new Book(3, "Refactoring", "Martin Fowler"));
            Console.WriteLine("3 livros cadastrados com sucesso!\n");

            // Cenário 2: Listagem de todos os livros
            Console.WriteLine("--- Listagem de Livros (JSON) ---");
            var all = BookService.ListAll(repo);
            Console.WriteLine($"Total de livros: {all.Count}");
            foreach (var book in all)
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 3: Teste com caracteres especiais
            Console.WriteLine("--- Caracteres Especiais em JSON ---");
            BookService.Register(repo, new Book(10, "Livro com \"aspas\" e /barras/", "Autor: João & Maria"));
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

            // Cenário 6: Mostrar conteúdo do arquivo JSON
            Console.WriteLine("--- Conteúdo do Arquivo JSON ---");
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path);
                foreach (var line in lines)
                    Console.WriteLine($"  {line}");
            }

            Console.WriteLine("\n=== Para executar os testes, rode o projeto de testes separado ===");
        }
    }
}
