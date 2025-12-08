using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Repositories;
using Fase09DublesAvancados.Services;

namespace Fase09DublesAvancados
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== FASE 9 - DUBLÊS AVANÇADOS E TESTES ASSÍNCRONOS ===\n");

            // Cenário 1: Tipos de Dublês
            Console.WriteLine("--- Tipos de Dublês ---");
            Console.WriteLine("DUMMY - Objeto que não faz nada (preenche parâmetros)");
            Console.WriteLine("STUB  - Retorna valores fixos pré-configurados");
            Console.WriteLine("SPY   - Registra as chamadas feitas");
            Console.WriteLine("MOCK  - Verifica se expectativas foram atendidas");
            Console.WriteLine("FAKE  - Implementação funcional simplificada");
            Console.WriteLine();

            // Cenário 2: Repositório Assíncrono Real
            Console.WriteLine("--- Repositório Assíncrono ---");
            var repository = new AsyncBookRepository(ioDelayMs: 50);
            var service = new AsyncBookService(repository);
            
            await service.RegisterAsync(new Book(1, "Design Patterns", "GoF"));
            await service.RegisterAsync(new Book(2, "Clean Code", "Robert C. Martin"));
            await service.RegisterAsync(new Book(3, "Refactoring", "Martin Fowler"));
            
            var books = await service.ListAllAsync();
            Console.WriteLine($"Livros cadastrados: {books.Count}");
            foreach (var book in books)
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 3: Operações Paralelas
            Console.WriteLine("--- Operações Paralelas (Task.WhenAll) ---");
            
            var startParallel = DateTime.Now;
            
            var task1 = service.FindByIdAsync(1);
            var task2 = service.FindByIdAsync(2);
            var task3 = service.FindByIdAsync(3);
            
            await Task.WhenAll(task1, task2, task3);
            
            var parallelTime = (DateTime.Now - startParallel).TotalMilliseconds;
            
            Console.WriteLine($"3 consultas em paralelo: {parallelTime:F0}ms");
            Console.WriteLine("(Sequencial seria ~150ms)");
            Console.WriteLine();

            // Cenário 4: Atualização e Remoção
            Console.WriteLine("--- Atualização e Remoção ---");
            
            var updated = await service.UpdateBookAsync(new Book(1, "Design Patterns (2ª Ed)", "GoF"));
            Console.WriteLine($"Livro #1 atualizado: {updated}");
            
            var removed = await service.RemoveBookAsync(3);
            Console.WriteLine($"Livro #3 removido: {removed}");
            
            var count = await service.CountBooksAsync();
            Console.WriteLine($"Total após operações: {count}");
            Console.WriteLine();

            // Cenário 5: Comparação de Dublês
            Console.WriteLine("--- Comparação de Dublês (para testes) ---");
            Console.WriteLine("┌──────────┬──────────────────┬────────────────────┐");
            Console.WriteLine("│ Dublê    │ Quando Usar      │ Característica     │");
            Console.WriteLine("├──────────┼──────────────────┼────────────────────┤");
            Console.WriteLine("│ Dummy    │ Não é chamado    │ Lança exceção      │");
            Console.WriteLine("│ Stub     │ Dados fixos      │ Respostas simples  │");
            Console.WriteLine("│ Spy      │ Verificar uso    │ Registra chamadas  │");
            Console.WriteLine("│ Mock     │ Expectativas     │ Verifica contratos │");
            Console.WriteLine("│ Fake     │ Funcionalidade   │ Implementação real │");
            Console.WriteLine("└──────────┴──────────────────┴────────────────────┘");
            
            Console.WriteLine("\n=== Para ver os testes com dublês, rode o projeto de testes ===");
        }
    }
}
