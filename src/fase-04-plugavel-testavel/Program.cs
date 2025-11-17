using System;

namespace Fase04PlugavelTestavel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FASE 4 - INTERFACE PLUGAVEL E TESTAVEL ===\n");

            // Primeiro, executar testes automatizados
            TestesComDubles.ExecutarTodos();

            // Cenário 1: Composição com seleção manual
            Console.WriteLine("--- Cenario 1: Selecao Manual do Algoritmo ---");
            Console.Write("Informe o modo (bubble, quick, insertion): ");
            string modo = Console.ReadLine() ?? "quick";
            
            int[] lista1 = { 42, 13, 7, 25, 18, 3 };
            Console.WriteLine($"Lista original: [{string.Join(", ", lista1)}]");
            
            IAlgoritmoOrdenacao algoritmo1 = CatalogoAlgoritmos.Resolver(modo);
            ServicoOrdenacao servico1 = new ServicoOrdenacao(algoritmo1);
            int[] resultado1 = servico1.ProcessarLista(lista1);
            
            Console.WriteLine($"Lista ordenada ({modo}): [{string.Join(", ", resultado1)}]\n");

            // Cenário 2: Composição automática por tamanho
            Console.WriteLine("--- Cenario 2: Selecao Automatica por Tamanho ---");
            
            int[] listaPequena = { 5, 2, 8, 1 };
            Console.WriteLine($"Lista pequena: [{string.Join(", ", listaPequena)}]");
            IAlgoritmoOrdenacao algoPequeno = CatalogoAlgoritmos.ResolverPorTamanho(listaPequena.Length);
            ServicoOrdenacao servicoPequeno = new ServicoOrdenacao(algoPequeno);
            Console.WriteLine($"Ordenada: [{string.Join(", ", servicoPequeno.ProcessarLista(listaPequena))}]");
            
            int[] listaGrande = new int[60];
            Random random = new Random();
            for (int i = 0; i < listaGrande.Length; i++)
                listaGrande[i] = random.Next(1, 100);
            
            Console.WriteLine($"\nLista grande: [{listaGrande[0]}, {listaGrande[1]}, ... (60 elementos)]");
            IAlgoritmoOrdenacao algoGrande = CatalogoAlgoritmos.ResolverPorTamanho(listaGrande.Length);
            ServicoOrdenacao servicoGrande = new ServicoOrdenacao(algoGrande);
            int[] resultadoGrande = servicoGrande.ProcessarLista(listaGrande);
            Console.WriteLine($"Ordenada: [{resultadoGrande[0]}, {resultadoGrande[1]}, ... {resultadoGrande[58]}, {resultadoGrande[59]}]\n");

            // Cenário 3: Demonstração de plugabilidade
            Console.WriteLine("--- Cenario 3: Demonstracao de Plugabilidade ---");
            int[] listaTeste = { 9, 3, 7, 1, 5 };
            Console.WriteLine($"Lista: [{string.Join(", ", listaTeste)}]");
            
            Console.WriteLine("\nTestando com diferentes algoritmos:");
            
            ServicoOrdenacao servicoBubble = new ServicoOrdenacao(new BubbleSortAlgorithm());
            Console.WriteLine($"Bubble Sort: [{string.Join(", ", servicoBubble.ProcessarLista(listaTeste))}]");
            
            ServicoOrdenacao servicoInsertion = new ServicoOrdenacao(new InsertionSortAlgorithm());
            Console.WriteLine($"Insertion Sort: [{string.Join(", ", servicoInsertion.ProcessarLista(listaTeste))}]");
            
            ServicoOrdenacao servicoQuick = new ServicoOrdenacao(new QuickSortAlgorithm());
            Console.WriteLine($"Quick Sort: [{string.Join(", ", servicoQuick.ProcessarLista(listaTeste))}]");
        }
    }
}
