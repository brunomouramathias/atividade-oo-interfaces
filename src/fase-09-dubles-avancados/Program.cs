using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Domain.Repositories;
using Fase09DublesAvancados.Services;
using Fase09DublesAvancados.Tests;
using Fase09DublesAvancados.Tests.Dubles;

namespace Fase09DublesAvancados
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== FASE 9 - DUBLÊS AVANÇADOS E TESTES ASSÍNCRONOS ===\n");

            // Executar testes assíncronos primeiro
            await TestesAssincronos.ExecutarTodosAsync();

            // Cenário 1: Tipos de Dublês
            Console.WriteLine("--- Cenario 1: Tipos de Dubles ---");
            Console.WriteLine("✓ DUMMY - Objeto que não faz nada (preenche parâmetros)");
            Console.WriteLine("✓ STUB - Retorna valores fixos pré-configurados");
            Console.WriteLine("✓ SPY - Registra as chamadas feitas");
            Console.WriteLine("✓ MOCK - Verifica se expectativas foram atendidas");
            Console.WriteLine("✓ FAKE - Implementação funcional simplificada");
            Console.WriteLine();

            // Cenário 2: Stub em ação
            Console.WriteLine("--- Cenario 2: Stub em Acao ---");
            var stubData = new List<Book>
            {
                new Book(1, "Design Patterns", "GoF"),
                new Book(2, "Clean Code", "Robert C. Martin")
            };
            var stub = new StubRepository(stubData);
            var stubService = new AsyncBookService(stub);
            
            var stubBooks = await stubService.ListAllAsync();
            Console.WriteLine($"Stub retornou {stubBooks.Count} livros pré-configurados:");
            foreach (var book in stubBooks)
            {
                Console.WriteLine($"  #{book.Id} - {book.Title} ({book.Author})");
            }
            Console.WriteLine();

            // Cenário 3: Spy em ação
            Console.WriteLine("--- Cenario 3: Spy em Acao ---");
            var spy = new SpyRepository();
            var spyService = new AsyncBookService(spy);
            
            await spyService.RegisterAsync(new Book(1, "Livro A", "Autor A"));
            await spyService.RegisterAsync(new Book(2, "Livro B", "Autor B"));
            await spyService.FindByIdAsync(1);
            await spyService.ListAllAsync();
            await spyService.RemoveBookAsync(2);
            
            Console.WriteLine("Spy registrou:");
            Console.WriteLine($"  AddAsync chamado: {spy.AddAsyncCallCount} vezes");
            Console.WriteLine($"  GetByIdAsync chamado: {spy.GetByIdAsyncCallCount} vezes");
            Console.WriteLine($"  ListAllAsync chamado: {spy.ListAllAsyncCallCount} vezes");
            Console.WriteLine($"  RemoveAsync chamado: {spy.RemoveAsyncCallCount} vezes");
            Console.WriteLine($"  Livros adicionados: {spy.AddedBooks.Count}");
            Console.WriteLine($"  IDs consultados: {string.Join(", ", spy.QueriedIds)}");
            Console.WriteLine($"  IDs removidos: {string.Join(", ", spy.RemovedIds)}");
            Console.WriteLine();

            // Cenário 4: Mock em ação
            Console.WriteLine("--- Cenario 4: Mock em Acao ---");
            var mock = new MockRepository();
            var mockService = new AsyncBookService(mock);
            
            var expectedBook = new Book(10, "Expected Book", "Author");
            mock.ExpectAddAsync(expectedBook, times: 1);
            
            await mockService.RegisterAsync(expectedBook);
            
            try
            {
                mock.Verify();
                Console.WriteLine("✓ Mock: Expectativas atendidas!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Mock: {ex.Message}");
            }
            Console.WriteLine();

            // Cenário 5: Fake vs Repositório Real
            Console.WriteLine("--- Cenario 5: Fake vs Repositorio Real ---");
            
            var fake = new FakeRepository(delayMs: 10);
            var real = new AsyncBookRepository(ioDelayMs: 50);
            
            var fakeService = new AsyncBookService(fake);
            var realService = new AsyncBookService(real);
            
            var startFake = DateTime.Now;
            await fakeService.RegisterAsync(new Book(1, "Livro", "Autor"));
            var fakeTime = (DateTime.Now - startFake).TotalMilliseconds;
            
            var startReal = DateTime.Now;
            await realService.RegisterAsync(new Book(1, "Livro", "Autor"));
            var realTime = (DateTime.Now - startReal).TotalMilliseconds;
            
            Console.WriteLine($"Fake com 10ms de latência: {fakeTime:F0}ms");
            Console.WriteLine($"Real com 50ms de latência: {realTime:F0}ms");
            Console.WriteLine("✓ Fake é mais rápido em testes!");
            Console.WriteLine();

            // Cenário 6: Operações Assíncronas em Paralelo
            Console.WriteLine("--- Cenario 6: Operacoes Assincronas em Paralelo ---");
            
            var parallelRepo = new FakeRepository(delayMs: 30);
            var parallelService = new AsyncBookService(parallelRepo);
            
            await parallelService.RegisterAsync(new Book(1, "Livro 1", "Autor"));
            await parallelService.RegisterAsync(new Book(2, "Livro 2", "Autor"));
            await parallelService.RegisterAsync(new Book(3, "Livro 3", "Autor"));
            
            var startParallel = DateTime.Now;
            
            // Executa 3 consultas em paralelo
            var task1 = parallelService.FindByIdAsync(1);
            var task2 = parallelService.FindByIdAsync(2);
            var task3 = parallelService.FindByIdAsync(3);
            
            await Task.WhenAll(task1, task2, task3);
            
            var parallelTime = (DateTime.Now - startParallel).TotalMilliseconds;
            
            Console.WriteLine($"3 consultas em paralelo: {parallelTime:F0}ms");
            Console.WriteLine("(Se fosse sequencial: ~90ms)");
            Console.WriteLine("✓ Task.WhenAll permite execução paralela!");
            Console.WriteLine();

            // Cenário 7: Benefícios dos Testes Assíncronos
            Console.WriteLine("--- Cenario 7: Beneficios dos Testes Assincronos ---");
            Console.WriteLine("✓ Simula operações I/O (banco de dados, APIs, arquivos)");
            Console.WriteLine("✓ Testa código assíncrono real (async/await)");
            Console.WriteLine("✓ Permite testes paralelos (mais rápidos)");
            Console.WriteLine("✓ Dublês com latência simulam cenários reais");
            Console.WriteLine("✓ Spy registra ordem e quantidade de chamadas");
            Console.WriteLine();

            // Cenário 8: Comparação de Dublês
            Console.WriteLine("--- Cenario 8: Comparacao de Dubles ---");
            Console.WriteLine("┌──────────┬──────────────────┬────────────────────┐");
            Console.WriteLine("│ Dublê    │ Quando Usar      │ Característica     │");
            Console.WriteLine("├──────────┼──────────────────┼────────────────────┤");
            Console.WriteLine("│ Dummy    │ Não é chamado    │ Lança exceção      │");
            Console.WriteLine("│ Stub     │ Dados fixos      │ Respostas simples  │");
            Console.WriteLine("│ Spy      │ Verificar uso    │ Registra chamadas  │");
            Console.WriteLine("│ Mock     │ Expectativas     │ Verifica contratos │");
            Console.WriteLine("│ Fake     │ Funcionalidade   │ Implementação real │");
            Console.WriteLine("└──────────┴──────────────────┴────────────────────┘");
        }
    }
}
