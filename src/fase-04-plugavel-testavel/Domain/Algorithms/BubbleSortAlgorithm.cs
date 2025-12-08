using Fase04PlugavelTestavel.Domain.Interfaces;

namespace Fase04PlugavelTestavel.Domain.Algorithms
{
    /// <summary>
    /// Implementação do algoritmo Bubble Sort.
    /// Complexidade: O(n²) - Recomendado para listas pequenas.
    /// </summary>
    public sealed class BubbleSortAlgorithm : IAlgoritmoOrdenacao
    {
        public int[] Ordenar(int[] lista)
        {
            if (lista == null || lista.Length == 0)
                return lista ?? System.Array.Empty<int>();

            int[] copia = (int[])lista.Clone();
            int n = copia.Length;
            
            for (int i = 0; i < n - 1; i++)
            {
                bool trocou = false;
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (copia[j] > copia[j + 1])
                    {
                        (copia[j], copia[j + 1]) = (copia[j + 1], copia[j]);
                        trocou = true;
                    }
                }
                if (!trocou) break;
            }
            
            return copia;
        }
    }
}
