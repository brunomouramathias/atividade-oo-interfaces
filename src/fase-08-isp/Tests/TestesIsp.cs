using System;
using Fase08Isp.Domain.Entities;
using Fase08Isp.Domain.Repositories;
using Fase08Isp.Services;

namespace Fase08Isp.Tests
{
    // Testes demonstrando o ISP em ação
    public static class TestesIsp
    {
        public static void ExecutarTodos()
        {
            Console.WriteLine("=== TESTES ISP (Interface Segregation Principle) ===\n");

            Teste1_ReadOnlyService_ShouldOnlyNeedReadRepository();
            Teste2_FullService_ShouldNeedFullRepository();
            Teste3_ReadOnlyService_CannotModifyData();
            Teste4_ISP_AllowsDifferentImplementations();
            Teste5_ReadOnlyService_CountsCorrectly();

            Console.WriteLine("\n=== TODOS OS TESTES PASSARAM! ===\n");
        }

        private static void Teste1_ReadOnlyService_ShouldOnlyNeedReadRepository()
        {
            Console.WriteLine("Teste 1: ReadOnlyService_ShouldOnlyNeedReadRepository");
            
            var repo = new InMemoryBookRepository();
            repo.Add(new Book(1, "Livro A", "Autor A"));
            repo.Add(new Book(2, "Livro B", "Autor B"));
            
            // ReadOnlyService recebe apenas IReadRepository
            var readService = new ReadOnlyBookService(repo);
            
            var all = readService.ListAll();
            Verificar(all.Count == 2, "Deveria ter 2 livros");
            
            var found = readService.FindById(1);
            Verificar(found != null, "Deveria encontrar livro");
            Verificar(found.Title == "Livro A", "Título deveria ser Livro A");
            
            Console.WriteLine("  ✓ Passou - ReadOnlyService não precisa de métodos de escrita\n");
        }

        private static void Teste2_FullService_ShouldNeedFullRepository()
        {
            Console.WriteLine("Teste 2: FullService_ShouldNeedFullRepository");
            
            var repo = new InMemoryBookRepository();
            var service = new BookService(repo);
            
            // BookService pode adicionar, atualizar e remover
            service.Register(new Book(1, "Livro A", "Autor A"));
            Verificar(service.ListAll().Count == 1, "Deveria ter 1 livro");
            
            service.UpdateBook(new Book(1, "Livro A (Atualizado)", "Autor A"));
            Verificar(service.FindById(1)?.Title == "Livro A (Atualizado)", "Título deveria estar atualizado");
            
            service.RemoveBook(1);
            Verificar(service.ListAll().Count == 0, "Deveria ter 0 livros");
            
            Console.WriteLine("  ✓ Passou - BookService tem acesso completo\n");
        }

        private static void Teste3_ReadOnlyService_CannotModifyData()
        {
            Console.WriteLine("Teste 3: ReadOnlyService_CannotModifyData");
            
            var repo = new InMemoryBookRepository();
            repo.Add(new Book(1, "Livro Original", "Autor"));
            
            var readService = new ReadOnlyBookService(repo);
            
            // ReadOnlyService NÃO tem métodos para modificar dados
            // Isso é garantido em tempo de compilação pelo ISP
            
            var book = readService.FindById(1);
            Verificar(book != null, "Livro deveria existir");
            Verificar(book.Title == "Livro Original", "Título deveria ser original");
            
            Console.WriteLine("  ✓ Passou - ReadOnlyService não pode modificar (garantido pelo ISP)\n");
        }

        private static void Teste4_ISP_AllowsDifferentImplementations()
        {
            Console.WriteLine("Teste 4: ISP_AllowsDifferentImplementations");
            
            var repo = new InMemoryBookRepository();
            
            // A mesma implementação pode ser usada como IReadRepository ou IRepository
            repo.Add(new Book(1, "Livro A", "Autor"));
            repo.Add(new Book(2, "Livro B", "Autor"));
            
            var readService = new ReadOnlyBookService(repo);
            var fullService = new BookService(repo);
            
            // Ambos acessam os mesmos dados
            Verificar(readService.ListAll().Count == 2, "ReadService deveria ver 2 livros");
            Verificar(fullService.ListAll().Count == 2, "FullService deveria ver 2 livros");
            
            // FullService adiciona mais um
            fullService.Register(new Book(3, "Livro C", "Autor"));
            
            // ReadService também vê o novo
            Verificar(readService.ListAll().Count == 3, "ReadService deveria ver 3 livros agora");
            
            Console.WriteLine("  ✓ Passou - ISP permite flexibilidade de uso\n");
        }

        private static void Teste5_ReadOnlyService_CountsCorrectly()
        {
            Console.WriteLine("Teste 5: ReadOnlyService_CountsCorrectly");
            
            var repo = new InMemoryBookRepository();
            repo.Add(new Book(1, "Livro 1", "Autor"));
            repo.Add(new Book(2, "Livro 2", "Autor"));
            repo.Add(new Book(3, "Livro 3", "Autor"));
            
            var readService = new ReadOnlyBookService(repo);
            
            var count = readService.CountBooks();
            Verificar(count == 3, "Deveria contar 3 livros");
            
            var exists = readService.BookExists(2);
            Verificar(exists, "Livro 2 deveria existir");
            
            var notExists = readService.BookExists(99);
            Verificar(!notExists, "Livro 99 não deveria existir");
            
            Console.WriteLine("  ✓ Passou - ReadOnlyService opera corretamente\n");
        }

        private static void Verificar(bool condicao, string mensagem)
        {
            if (!condicao)
            {
                throw new Exception($"Falha na verificação: {mensagem}");
            }
        }
    }
}

