using System;

namespace Fase03Interfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 3 - INTERFACES ===\n");

            // Catálogo de algoritmos
            CatalogoAlgoritmos catalogo = new CatalogoAlgoritmos();

            // Cenário 1: Lista pequena (usa Bubble Sort automaticamente)
            Console.WriteLine("--- Cenario 1: Lista Pequena ---");
            int[] listaPequena = { 5, 2, 8, 1, 9 };
            Console.WriteLine($"Original: [{string.Join(", ", listaPequena)}]");
            
            IAlgoritmoOrdenacao algoPequeno = catalogo.SelecionarPorTamanho(listaPequena.Length);
            ServicoOrdenacao servicoPequeno = new ServicoOrdenacao(algoPequeno);
            int[] resultadoPequeno = servicoPequeno.OrdenarLista(listaPequena);
            Console.WriteLine($"Ordenada (auto): [{string.Join(", ", resultadoPequeno)}]\n");

            // Cenário 2: Lista média (usa Insertion Sort automaticamente)
            Console.WriteLine("--- Cenario 2: Lista Media ---");
            int[] listaMedia = { 15, 8, 23, 4, 16, 42, 12, 7, 31, 19, 
                                 25, 3, 18, 11, 29, 6, 22, 14, 27, 9 };
            Console.WriteLine($"Original: [{string.Join(", ", listaMedia)}] (tamanho: {listaMedia.Length})");
            
            IAlgoritmoOrdenacao algoMedio = catalogo.SelecionarPorTamanho(listaMedia.Length);
            ServicoOrdenacao servicoMedio = new ServicoOrdenacao(algoMedio);
            int[] resultadoMedio = servicoMedio.OrdenarLista(listaMedia);
            Console.WriteLine($"Ordenada (auto): [{string.Join(", ", resultadoMedio)}]\n");

            // Cenário 3: Lista grande (usa Quick Sort automaticamente)
            Console.WriteLine("--- Cenario 3: Lista Grande ---");
            Random random = new Random();
            int[] listaGrande = new int[100];
            for (int i = 0; i < 100; i++)
                listaGrande[i] = random.Next(1, 500);
            
            Console.WriteLine($"Original: [{listaGrande[0]}, {listaGrande[1]}, {listaGrande[2]}, ... (100 elementos)]");
            
            IAlgoritmoOrdenacao algoGrande = catalogo.SelecionarPorTamanho(listaGrande.Length);
            ServicoOrdenacao servicoGrande = new ServicoOrdenacao(algoGrande);
            int[] resultadoGrande = servicoGrande.OrdenarLista(listaGrande);
            Console.WriteLine($"Ordenada (auto): [{resultadoGrande[0]}, {resultadoGrande[1]}, {resultadoGrande[2]}, ... {resultadoGrande[97]}, {resultadoGrande[98]}, {resultadoGrande[99]}]\n");

            // Cenário 4: Seleção manual de algoritmo
            Console.WriteLine("--- Cenario 4: Selecao Manual ---");
            int[] listaManual = { 42, 13, 7, 25, 18 };
            Console.WriteLine($"Original: [{string.Join(", ", listaManual)}]");
            
            IAlgoritmoOrdenacao algoManual = catalogo.ObterAlgoritmo("grande");
            ServicoOrdenacao servicoManual = new ServicoOrdenacao(algoManual);
            int[] resultadoManual = servicoManual.OrdenarLista(listaManual);
            Console.WriteLine($"Ordenada (Quick Sort manual): [{string.Join(", ", resultadoManual)}]\n");

            // Cenário 5: Comparação direta dos algoritmos
            Console.WriteLine("--- Cenario 5: Comparacao dos Algoritmos ---");
            int[] listaComparacao = { 12, 3, 9, 1, 15, 6 };
            Console.WriteLine($"Original: [{string.Join(", ", listaComparacao)}]");
            
            ServicoOrdenacao servicoBubble = new ServicoOrdenacao(new BubbleSortAlgorithm());
            Console.WriteLine($"Bubble Sort: [{string.Join(", ", servicoBubble.OrdenarLista(listaComparacao))}]");
            
            ServicoOrdenacao servicoInsertion = new ServicoOrdenacao(new InsertionSortAlgorithm());
            Console.WriteLine($"Insertion Sort: [{string.Join(", ", servicoInsertion.OrdenarLista(listaComparacao))}]");
            
            ServicoOrdenacao servicoQuick = new ServicoOrdenacao(new QuickSortAlgorithm());
            Console.WriteLine($"Quick Sort: [{string.Join(", ", servicoQuick.OrdenarLista(listaComparacao))}]");
        }
    }
}
