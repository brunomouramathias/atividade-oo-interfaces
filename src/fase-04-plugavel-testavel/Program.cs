using System;
using Fase04PlugavelTestavel.Domain.Algorithms;
using Fase04PlugavelTestavel.Services;

namespace Fase04PlugavelTestavel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 4 - INTERFACE PLUGÁVEL E TESTÁVEL ===\n");

            // Cenário 1: Seleção manual do algoritmo
            Console.WriteLine("--- Cenário 1: Seleção Manual ---\n");
            Console.Write("Informe o modo (bubble, quick, insertion): ");
            string modo = Console.ReadLine() ?? "quick";
            
            int[] lista1 = { 42, 13, 7, 25, 18, 3 };
            Console.WriteLine($"Lista original: [{string.Join(", ", lista1)}]");
            
            var algoritmo1 = CatalogoAlgoritmos.Resolver(modo);
            var servico1 = new ServicoOrdenacao(algoritmo1);
            var resultado1 = servico1.ProcessarLista(lista1);
            
            Console.WriteLine($"Algoritmo: {algoritmo1.GetType().Name}");
            Console.WriteLine($"Lista ordenada: [{string.Join(", ", resultado1)}]\n");

            // Cenário 2: Seleção automática por tamanho
            Console.WriteLine("--- Cenário 2: Seleção Automática por Tamanho ---\n");
            
            int[] listaPequena = { 5, 2, 8, 1 };
            var algoPequeno = CatalogoAlgoritmos.ResolverPorTamanho(listaPequena.Length);
            var servicoPequeno = new ServicoOrdenacao(algoPequeno);
            Console.WriteLine($"Lista pequena ({listaPequena.Length} elementos): {algoPequeno.GetType().Name}");
            Console.WriteLine($"  Original: [{string.Join(", ", listaPequena)}]");
            Console.WriteLine($"  Ordenada: [{string.Join(", ", servicoPequeno.ProcessarLista(listaPequena))}]\n");
            
            int[] listaGrande = new int[60];
            var random = new Random(42);
            for (int i = 0; i < listaGrande.Length; i++)
                listaGrande[i] = random.Next(1, 100);
            
            var algoGrande = CatalogoAlgoritmos.ResolverPorTamanho(listaGrande.Length);
            var servicoGrande = new ServicoOrdenacao(algoGrande);
            var resultadoGrande = servicoGrande.ProcessarLista(listaGrande);
            Console.WriteLine($"Lista grande ({listaGrande.Length} elementos): {algoGrande.GetType().Name}");
            Console.WriteLine($"  Ordenada: [{resultadoGrande[0]}, {resultadoGrande[1]}, ..., {resultadoGrande[^2]}, {resultadoGrande[^1]}]\n");

            // Cenário 3: Demonstração de plugabilidade
            Console.WriteLine("--- Cenário 3: Plugabilidade ---\n");
            int[] listaTeste = { 9, 3, 7, 1, 5 };
            Console.WriteLine($"Lista: [{string.Join(", ", listaTeste)}]\n");
            
            var servicoBubble = new ServicoOrdenacao(new BubbleSortAlgorithm());
            var servicoInsertion = new ServicoOrdenacao(new InsertionSortAlgorithm());
            var servicoQuick = new ServicoOrdenacao(new QuickSortAlgorithm());
            
            Console.WriteLine($"Bubble Sort:    [{string.Join(", ", servicoBubble.ProcessarLista(listaTeste))}]");
            Console.WriteLine($"Insertion Sort: [{string.Join(", ", servicoInsertion.ProcessarLista(listaTeste))}]");
            Console.WriteLine($"Quick Sort:     [{string.Join(", ", servicoQuick.ProcessarLista(listaTeste))}]");
            
            Console.WriteLine("\n=== Para executar os testes, rode o projeto Fase04PlugavelTestavel.Tests ===");
        }
    }
}
