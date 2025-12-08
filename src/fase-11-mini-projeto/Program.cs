using System;
using Fase11MiniProjeto.Domain.Repositories;
using Fase11MiniProjeto.Services;

namespace Fase11MiniProjeto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 11 - MINI-PROJETO DE CONSOLIDAÇÃO ===\n");
            Console.WriteLine("Tema: Ordenação de Coleções com Bubble Sort e Quick Sort\n");

            // Composição: Criação do repositório e serviço
            var repository = new InMemoryOrdenacaoRepository();
            var service = new OrdenacaoService(repository);

            // Cenário 1: Ordenação com seleção manual
            Console.WriteLine("--- Ordenação com Bubble Sort (lista pequena) ---");
            int[] listaPequena = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine($"Original: [{string.Join(", ", listaPequena)}]");
            
            var resultado1 = service.Ordenar(listaPequena, "bubble");
            Console.WriteLine($"Ordenada: [{string.Join(", ", resultado1.ListaOrdenada)}]");
            Console.WriteLine($"Algoritmo: {resultado1.AlgoritmoUsado}");
            Console.WriteLine($"Tempo: {resultado1.TempoMs}ms\n");

            // Cenário 2: Ordenação automática
            Console.WriteLine("--- Ordenação Automática (seleção por tamanho) ---");
            int[] listaGrande = new int[50];
            var random = new Random(42);
            for (int i = 0; i < listaGrande.Length; i++)
                listaGrande[i] = random.Next(1, 100);

            Console.WriteLine($"Lista com {listaGrande.Length} elementos");
            Console.WriteLine($"Primeiros 5: [{listaGrande[0]}, {listaGrande[1]}, {listaGrande[2]}, {listaGrande[3]}, {listaGrande[4]}, ...]");
            
            var resultado2 = service.OrdenarAutomatico(listaGrande);
            Console.WriteLine($"Algoritmo selecionado: {resultado2.AlgoritmoUsado}");
            Console.WriteLine($"Ordenada: [{resultado2.ListaOrdenada[0]}, {resultado2.ListaOrdenada[1]}, ..., {resultado2.ListaOrdenada[^2]}, {resultado2.ListaOrdenada[^1]}]\n");

            // Cenário 3: Histórico de ordenações (Repository)
            Console.WriteLine("--- Histórico de Ordenações (Repository Pattern) ---");
            
            // Mais algumas ordenações
            service.Ordenar(new[] { 5, 3, 8, 1 }, "bubble");
            service.Ordenar(new[] { 99, 42, 17 }, "quick");
            
            var historico = service.ListarHistorico();
            Console.WriteLine($"Total de ordenações realizadas: {historico.Count}");
            foreach (var item in historico)
            {
                Console.WriteLine($"  #{item.Id} - {item.AlgoritmoUsado} - {item.ListaOriginal.Length} elementos");
            }
            Console.WriteLine();

            // Cenário 4: Conceitos consolidados
            Console.WriteLine("--- Conceitos Consolidados neste Mini-Projeto ---");
            Console.WriteLine("✓ Interface IAlgoritmoOrdenacao (contrato)");
            Console.WriteLine("✓ Implementações: BubbleSortAlgorithm, QuickSortAlgorithm");
            Console.WriteLine("✓ Política de seleção: Bubble para pequenas, Quick para grandes");
            Console.WriteLine("✓ Repository Pattern para persistir resultados");
            Console.WriteLine("✓ ISP: IReadRepository, IWriteRepository, IRepository");
            Console.WriteLine("✓ Injeção de dependência no serviço");
            Console.WriteLine("✓ Código organizado em Domain/Services");
            
            Console.WriteLine("\n=== MINI-PROJETO CONCLUÍDO ===");
        }
    }
}
