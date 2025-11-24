using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fase09DublesAvancados.Domain.Entities;
using Fase09DublesAvancados.Services;
using Fase09DublesAvancados.Tests.Dubles;

namespace Fase09DublesAvancados.Tests
{
    // Testes assíncronos demonstrando diferentes tipos de dublês
    public static class TestesAssincronos
    {
        public static async Task ExecutarTodosAsync()
        {
            Console.WriteLine("=== TESTES ASSÍNCRONOS COM DUBLÊS AVANÇADOS ===\n");

            await Teste1_Stub_RetornaValoresFixosAsync();
            await Teste2_Spy_RegistraChamadasAsync();
            await Teste3_Mock_VerificaExpectativasAsync();
            await Teste4_Fake_SimulaComportamentoRealAsync();
            await Teste5_Fake_ComLatenciaAsync();
            await Teste6_OperacoesParalelas_FuncionamAsync();
            await Teste7_Task_WhenAll_ExecutaEmParaleloAsync();
            await Teste8_Spy_VerificaOrdemDeChamadasAsync();

            Console.WriteLine("\n=== TODOS OS TESTES ASSÍNCRONOS PASSARAM! ===\n");
        }

        private static async Task Teste1_Stub_RetornaValoresFixosAsync()
        {
            Console.WriteLine("Teste 1: Stub_RetornaValoresFixos");
            
            var fixedData = new List<Book>
            {
                new Book(1, "Livro A", "Autor A"),
                new Book(2, "Livro B", "Autor B")
            };
            
            var stub = new StubRepository(fixedData);
            var service = new AsyncBookService(stub);
            
            var books = await service.ListAllAsync();
            Verificar(books.Count == 2, "Stub deveria retornar 2 livros");
            
            var book1 = await service.FindByIdAsync(1);
            Verificar(book1 != null && book1.Title == "Livro A", "Stub deveria retornar Livro A");
            
            Console.WriteLine("  ✓ Passou - Stub retorna valores pré-configurados\n");
        }

        private static async Task Teste2_Spy_RegistraChamadasAsync()
        {
            Console.WriteLine("Teste 2: Spy_RegistraChamadas");
            
            var spy = new SpyRepository();
            var service = new AsyncBookService(spy);
            
            await service.RegisterAsync(new Book(1, "Livro A", "Autor"));
            await service.RegisterAsync(new Book(2, "Livro B", "Autor"));
            await service.FindByIdAsync(1);
            await service.FindByIdAsync(99);
            await service.ListAllAsync();
            
            // Verifica registros do spy
            Verificar(spy.AddAsyncCallCount == 2, "AddAsync deveria ter sido chamado 2 vezes");
            Verificar(spy.GetByIdAsyncCallCount == 2, "GetByIdAsync deveria ter sido chamado 2 vezes");
            Verificar(spy.ListAllAsyncCallCount == 1, "ListAllAsync deveria ter sido chamado 1 vez");
            Verificar(spy.AddedBooks.Count == 2, "Spy deveria registrar 2 livros adicionados");
            Verificar(spy.QueriedIds.Count == 2, "Spy deveria registrar 2 IDs consultados");
            Verificar(spy.QueriedIds[0] == 1, "Primeiro ID consultado deveria ser 1");
            Verificar(spy.QueriedIds[1] == 99, "Segundo ID consultado deveria ser 99");
            
            Console.WriteLine("  ✓ Passou - Spy registra todas as chamadas corretamente\n");
        }

        private static async Task Teste3_Mock_VerificaExpectativasAsync()
        {
            Console.WriteLine("Teste 3: Mock_VerificaExpectativas");
            
            var mock = new MockRepository();
            var service = new AsyncBookService(mock);
            
            var book = new Book(1, "Livro A", "Autor");
            mock.ExpectAddAsync(book, times: 1);
            
            await service.RegisterAsync(book);
            
            // Mock verifica se expectativas foram atendidas
            mock.Verify();
            
            Console.WriteLine("  ✓ Passou - Mock verifica expectativas corretamente\n");
        }

        private static async Task Teste4_Fake_SimulaComportamentoRealAsync()
        {
            Console.WriteLine("Teste 4: Fake_SimulaComportamentoReal");
            
            var fake = new FakeRepository();
            var service = new AsyncBookService(fake);
            
            // Fake funciona como repositório real
            await service.RegisterAsync(new Book(1, "Livro A", "Autor"));
            await service.RegisterAsync(new Book(2, "Livro B", "Autor"));
            
            var all = await service.ListAllAsync();
            Verificar(all.Count == 2, "Fake deveria ter 2 livros");
            
            var updated = await service.UpdateBookAsync(new Book(1, "Livro A (Atualizado)", "Autor"));
            Verificar(updated, "Update deveria retornar true");
            
            var book1 = await service.FindByIdAsync(1);
            Verificar(book1 != null && book1.Title == "Livro A (Atualizado)", "Título deveria estar atualizado");
            
            var removed = await service.RemoveBookAsync(2);
            Verificar(removed, "Remove deveria retornar true");
            
            var count = await service.CountBooksAsync();
            Verificar(count == 1, "Deveria ter 1 livro após remoção");
            
            Console.WriteLine("  ✓ Passou - Fake simula comportamento real perfeitamente\n");
        }

        private static async Task Teste5_Fake_ComLatenciaAsync()
        {
            Console.WriteLine("Teste 5: Fake_ComLatencia");
            
            var fake = new FakeRepository(delayMs: 10); // Simula latência de 10ms
            var service = new AsyncBookService(fake);
            
            var startTime = DateTime.Now;
            await service.RegisterAsync(new Book(1, "Livro", "Autor"));
            var endTime = DateTime.Now;
            
            var elapsed = (endTime - startTime).TotalMilliseconds;
            Verificar(elapsed >= 10, $"Operação deveria levar pelo menos 10ms (levou {elapsed}ms)");
            
            Console.WriteLine($"  ✓ Passou - Fake simula latência (levou {elapsed:F0}ms)\n");
        }

        private static async Task Teste6_OperacoesParalelas_FuncionamAsync()
        {
            Console.WriteLine("Teste 6: OperacoesParalelas_Funcionam");
            
            var fake = new FakeRepository(delayMs: 20);
            var service = new AsyncBookService(fake);
            
            // Adiciona 3 livros
            await service.RegisterAsync(new Book(1, "Livro 1", "Autor"));
            await service.RegisterAsync(new Book(2, "Livro 2", "Autor"));
            await service.RegisterAsync(new Book(3, "Livro 3", "Autor"));
            
            var startTime = DateTime.Now;
            
            // Executa 3 consultas em paralelo
            var task1 = service.FindByIdAsync(1);
            var task2 = service.FindByIdAsync(2);
            var task3 = service.FindByIdAsync(3);
            
            await Task.WhenAll(task1, task2, task3);
            
            var endTime = DateTime.Now;
            var elapsed = (endTime - startTime).TotalMilliseconds;
            
            Verificar(task1.Result != null, "Livro 1 deveria ser encontrado");
            Verificar(task2.Result != null, "Livro 2 deveria ser encontrado");
            Verificar(task3.Result != null, "Livro 3 deveria ser encontrado");
            
            // Paralelo deveria ser mais rápido que sequencial (< 60ms vs ~60ms)
            Verificar(elapsed < 50, $"Operações paralelas deveriam ser mais rápidas (levou {elapsed:F0}ms)");
            
            Console.WriteLine($"  ✓ Passou - Operações paralelas funcionam (levou {elapsed:F0}ms)\n");
        }

        private static async Task Teste7_Task_WhenAll_ExecutaEmParaleloAsync()
        {
            Console.WriteLine("Teste 7: Task_WhenAll_ExecutaEmParalelo");
            
            var fake = new FakeRepository(delayMs: 30);
            var service = new AsyncBookService(fake);
            
            var startTime = DateTime.Now;
            
            // Executa 5 adições em paralelo
            var tasks = new[]
            {
                service.RegisterAsync(new Book(1, "Livro 1", "Autor")),
                service.RegisterAsync(new Book(2, "Livro 2", "Autor")),
                service.RegisterAsync(new Book(3, "Livro 3", "Autor")),
                service.RegisterAsync(new Book(4, "Livro 4", "Autor")),
                service.RegisterAsync(new Book(5, "Livro 5", "Autor"))
            };
            
            await Task.WhenAll(tasks);
            
            var endTime = DateTime.Now;
            var elapsed = (endTime - startTime).TotalMilliseconds;
            
            var count = await service.CountBooksAsync();
            Verificar(count == 5, "Deveriam ter 5 livros");
            
            // Se fosse sequencial: 5 * 30ms = 150ms
            // Em paralelo: ~30ms
            Verificar(elapsed < 100, $"WhenAll deveria executar em paralelo (levou {elapsed:F0}ms)");
            
            Console.WriteLine($"  ✓ Passou - Task.WhenAll executa em paralelo (levou {elapsed:F0}ms)\n");
        }

        private static async Task Teste8_Spy_VerificaOrdemDeChamadasAsync()
        {
            Console.WriteLine("Teste 8: Spy_VerificaOrdemDeChamadas");
            
            var spy = new SpyRepository();
            var service = new AsyncBookService(spy);
            
            await service.RegisterAsync(new Book(1, "Livro 1", "Autor"));
            await service.RegisterAsync(new Book(2, "Livro 2", "Autor"));
            await service.RegisterAsync(new Book(3, "Livro 3", "Autor"));
            
            // Spy registra ordem dos livros adicionados
            Verificar(spy.AddedBooks.Count == 3, "Spy deveria registrar 3 livros");
            Verificar(spy.AddedBooks[0].Id == 1, "Primeiro livro deveria ter ID 1");
            Verificar(spy.AddedBooks[1].Id == 2, "Segundo livro deveria ter ID 2");
            Verificar(spy.AddedBooks[2].Id == 3, "Terceiro livro deveria ter ID 3");
            
            Console.WriteLine("  ✓ Passou - Spy registra ordem correta das chamadas\n");
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

