using System;
using Fase03Interfaces.Domain.Algorithms;
using Fase03Interfaces.Services;

namespace Fase03Interfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 3 - INTERFACES ===\n");

            var catalogo = new CatalogoAlgoritmos();

            // Demonstração 1: Seleção automática por tamanho
            Console.WriteLine("--- Seleção Automática por Tamanho ---\n");

            int[] listaPequena = { 5, 2, 8, 1, 9 };
            DemonstrarOrdenacao("Lista pequena (5 elementos)", listaPequena, catalogo.SelecionarPorTamanho(5));

            int[] listaMedia = { 42, 13, 7, 25, 18, 3, 55, 28, 11, 36, 22, 8, 45, 19, 33 };
            DemonstrarOrdenacao("Lista média (15 elementos)", listaMedia, catalogo.SelecionarPorTamanho(15));

            int[] listaGrande = GerarListaAleatoria(60);
            DemonstrarOrdenacao("Lista grande (60 elementos)", listaGrande, catalogo.SelecionarPorTamanho(60));

            // Demonstração 2: Diferentes algoritmos na mesma lista
            Console.WriteLine("\n--- Comparação de Algoritmos ---\n");

            int[] listaTeste = { 64, 34, 25, 12, 22, 11, 90 };
            Console.WriteLine($"Lista original: [{string.Join(", ", listaTeste)}]\n");

            var servicoBubble = new ServicoOrdenacao(new BubbleSortAlgorithm());
            var servicoQuick = new ServicoOrdenacao(new QuickSortAlgorithm());
            var servicoInsertion = new ServicoOrdenacao(new InsertionSortAlgorithm());

            Console.WriteLine($"Bubble Sort:    [{string.Join(", ", servicoBubble.OrdenarLista(listaTeste))}]");
            Console.WriteLine($"Quick Sort:     [{string.Join(", ", servicoQuick.OrdenarLista(listaTeste))}]");
            Console.WriteLine($"Insertion Sort: [{string.Join(", ", servicoInsertion.OrdenarLista(listaTeste))}]");

            Console.WriteLine("\n=== DEMONSTRAÇÃO CONCLUÍDA ===");
        }

        static void DemonstrarOrdenacao(string descricao, int[] lista, Domain.Interfaces.IAlgoritmoOrdenacao algoritmo)
        {
            var servico = new ServicoOrdenacao(algoritmo);
            var ordenada = servico.OrdenarLista(lista);

            Console.WriteLine($"{descricao}");
            Console.WriteLine($"  Algoritmo: {algoritmo.GetType().Name}");
            
            if (lista.Length <= 10)
            {
                Console.WriteLine($"  Original:  [{string.Join(", ", lista)}]");
                Console.WriteLine($"  Ordenada:  [{string.Join(", ", ordenada)}]");
            }
            else
            {
                Console.WriteLine($"  Primeiros 5: [{lista[0]}, {lista[1]}, {lista[2]}, {lista[3]}, {lista[4]}, ...]");
                Console.WriteLine($"  Ordenada:    [{ordenada[0]}, {ordenada[1]}, {ordenada[2]}, ..., {ordenada[^2]}, {ordenada[^1]}]");
            }
            Console.WriteLine();
        }

        static int[] GerarListaAleatoria(int tamanho)
        {
            var random = new Random(42); // Seed fixa para reprodutibilidade
            var lista = new int[tamanho];
            for (int i = 0; i < tamanho; i++)
                lista[i] = random.Next(1, 100);
            return lista;
        }
    }
}
